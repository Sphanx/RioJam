using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
public class ShakerManager : MonoBehaviour
{
    public GameObject liquidPrefab; // Eklenecek sıvı prefabı
    public Transform spawnPosition; // Sıvının spawn edileceği nokta
    public int maxCapacity = 500; // Shaker'ın toplam kapasitesi (ml)
    private float currentFill = 0f; // Mevcut doluluk seviyesi
    private GameObject miscItems;
    public bool isFillingDone = false;
    private int previousLiquidCount = 0; // Önceki sıvı sayısı (referans değeri)
    
    [Header("Liquid Add Settings")]
    public float liquidAddCooldown = 0.2f; // Sıvı ekleme arasındaki bekleme süresi (saniye)
    private float lastLiquidAddTime = 0f; // Son sıvı ekleme zamanı

    // Kokteyl malzemeleri için değişkenler
    private int currentIngredientIndex = 0; // Şu anki malzeme indeksi
    private float currentIngredientTarget = 0f; // Şu anki malzemenin hedef miktarı
    private List<CocktailIngredient> ingredients; // Kokteyl malzemeleri listesi

    public Slider fillSlider; // UI'de sıvı seviyesini gösteren slider
    public TMP_Text currentIngredientText; // Şu anki malzemenin adını gösteren metin (opsiyonel)
    private List<GameObject> liquidsFill = new List<GameObject>(); // Sıvı objeleri listesi
    public int ingredientModifier = 10;


    void Start()
    {
        miscItems = GameObject.Find("MiscItems");
        // Başlangıçta kokteyl bilgilerini al
        GetCurrentCocktail();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            AddLiquid();
        }
        // Mevcut doluluk seviyesini hesapla (toplam sıvı sayısı - önceki sıvı sayısı)
        currentFill = liquidsFill.Count - previousLiquidCount;

        if (currentFill >= fillSlider.value)
        {
            Debug.Log("Yeterince malzeme dolduruldu.");

            // Bir sonraki malzemeye otomatik olarak geç
            NextIngredient();
        }
    }

    // GameManager'dan şu anki kokteyli al
    public void GetCurrentCocktail()
    {
        // Önceki sıvı sayısını sıfırla
        previousLiquidCount = 0;

        // GameManager'dan şu anki kokteyli al
        Cocktail_SO currentCocktail = GameManager.Instance.cocktail;

        if (currentCocktail != null)
        {
            // Kokteyl malzemelerini al
            ingredients = new List<CocktailIngredient>(currentCocktail.ingredients);

            // Tüm malzemelerin toplam miktarını hesapla
            float totalAmount = 0f;
            foreach (CocktailIngredient ingredient in ingredients)
            {
                totalAmount += ingredient.amountInCl;
            }
            
            // Slider'ın maxValue değerini tüm malzemelerin toplam miktarına eşitle
            fillSlider.maxValue = totalAmount * ingredientModifier;
            
            // Diğer kokteyl bilgilerini kullanmak için gerekirse burada işlem yapılabilir
            Debug.Log("Kokteyl alındı: " + currentCocktail.cocktailName + ", Toplam miktar: " + totalAmount + " cl");

            // İlk malzemeyi ayarla (eğer malzeme varsa)
            if (ingredients.Count > 0)
            {
                currentIngredientIndex = 0;
                SetCurrentIngredient(currentIngredientIndex);
            }
            else
            {
                Debug.LogWarning("Kokteylde malzeme bulunamadı!");
            }
        }
        else
        {
            Debug.LogError("GameManager'da kokteyl tanımlanmamış!");
        }
    }

    // Belirtilen indeksteki malzemeyi ayarla
    private void SetCurrentIngredient(int index)
    {
        if (index >= 0 && index < ingredients.Count)
        {
            CocktailIngredient ingredient = ingredients[index];
            currentIngredientTarget = ingredient.amountInCl;

            // UI güncelleme
            if (currentIngredientText != null)
            {
                currentIngredientText.text = ingredient.drink.drinkName + " (" + ingredient.amountInCl + " cl)";
            }

            // Sadece value değerini güncelle, maxValue değişmez
            fillSlider.value = ingredient.amountInCl * ingredientModifier;

            Debug.Log("Şu anki malzeme: " + ingredient.drink.drinkName);
        }
    }

    // Bir sonraki malzemeye geç
    public void NextIngredient()
    {
        // Mevcut sıvı sayısını referans olarak kaydet
        previousLiquidCount = liquidsFill.Count;

        // Şu anki malzeme indeksini artır
        currentIngredientIndex++;

        // Eğer daha fazla malzeme varsa
        if (currentIngredientIndex < ingredients.Count)
        {
            // Bir sonraki malzemeyi ayarla
            SetCurrentIngredient(currentIngredientIndex);
            Debug.Log("Sıradaki malzemeye geçildi: " + ingredients[currentIngredientIndex].drink.drinkName);
        }
        else
        {
            // Tüm malzemeler dolduruldu
            Debug.Log("Tüm malzemeler dolduruldu! Kokteyl tamamlandı.");

            // Burada kokteyl tamamlandığında yapılacak işlemleri ekleyebilirsiniz
            // Örneğin: Bir UI paneli gösterme, puan verme, vb.
        }
    }

    [Header("Liquid Spawn Settings")]
    public float spawnRadius = 0.5f; // Sıvının spawn edilebileceği maksimum yarıçap

    public void AddLiquid()
    {
        // Cooldown kontrolü yap
        if (Time.time - lastLiquidAddTime < liquidAddCooldown)
        {
            // Cooldown süresi dolmadıysa, sıvı ekleme
            return;
        }
        
        // Son sıvı ekleme zamanını güncelle
        lastLiquidAddTime = Time.time;
        
        // Spawn pozisyonu etrafında rastgele bir konum oluştur
        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
        Vector3 randomPosition = spawnPosition.position + new Vector3(randomOffset.x, randomOffset.y, 0f);

        // Rastgele konumda sıvı prefabını instantiate et
        liquidsFill.Add(Instantiate(liquidPrefab, randomPosition, Quaternion.identity, miscItems.transform));
    }
}