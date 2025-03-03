using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
public class ShakerManager : MonoBehaviour
{
    public static ShakerManager Instance;
    public GameObject liquidPrefab;
    public Transform spawnPosition;
    public SpriteRenderer bottleImg;
    public int liquidMultiplier = 10;
    private float currentFill = 0f;
    private GameObject miscItems;
    public GameObject dialogueBox;
    public bool isFillingDone = false;
    private int previousLiquidCount = 0;

    [Header("Liquid Add Settings")]
    public float liquidAddCooldown = 0.2f;
    private float lastLiquidAddTime = 0f;

    private int currentIngredientIndex = 0;
    private float currentIngredientTarget = 0f;
    public TMP_Text currentIngredientText;
    private List<CocktailIngredient> ingredients;
    private bool isDone = false;


    private List<GameObject> liquidsFill = new List<GameObject>();
    public int ingredientModifier = 10;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        miscItems = GameObject.Find("MiscItems");
        GetCurrentCocktail();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && !isDone)
        {
            AddLiquid();
        }
        if (currentIngredientIndex >= 0 && currentIngredientIndex < ingredients.Count)
        {
            bottleImg.sprite = ingredients[currentIngredientIndex].drink.drinkImage;
        }

        if(isDone){
            ClearLiquids();
            Debug.Log("3131113");
        }
    }

    public void GetCurrentCocktail()
    {
        previousLiquidCount = 0;

        Cocktail_SO currentCocktail = GameManager.Instance.cocktail;
        if (currentCocktail != null)
        {
            ingredients = new List<CocktailIngredient>(currentCocktail.ingredients);

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

    private void SetCurrentIngredient(int index)
    {
        if (index >= 0 && index < ingredients.Count)
        {
            CocktailIngredient ingredient = ingredients[index];
            currentIngredientTarget = ingredient.amountInCl * liquidMultiplier;
            Debug.Log("Şu anki malzeme: " + ingredient.drink.drinkName);
        }
    }

    public void NextIngredient()
    {
        previousLiquidCount = liquidsFill.Count;
        currentIngredientIndex++;

        if (currentIngredientIndex < ingredients.Count)
        {
            SetCurrentIngredient(currentIngredientIndex);
            Debug.Log("Sıradaki malzemeye geçildi: " + ingredients[currentIngredientIndex].drink.drinkName);
        }
        else
        {
            Debug.Log("Tüm malzemeler dolduruldu! Kokteyl tamamlandı.");
            dialogueBox.SetActive(true);
            isDone = true;
        }
    }
    public void JumpNextScene(){
        isDone = false;
        dialogueBox.SetActive(false);
        UIManager.Instance.NextPanel();
    }

    [Header("Liquid Spawn Settings")]
    public float spawnRadius = 0.5f;

    private void ClearLiquids()
    {
        foreach (var liquid in liquidsFill)
        {
            Destroy(liquid);
        }
        liquidsFill.Clear();
        currentFill = 0f;
    }

    public void AddLiquid()
    {
        if (Time.time - lastLiquidAddTime < liquidAddCooldown)
        {
            return;
        }


        lastLiquidAddTime = Time.time;

        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
        Vector3 randomPosition = spawnPosition.position + new Vector3(randomOffset.x, randomOffset.y, 0f);

        liquidsFill.Add(Instantiate(liquidPrefab, randomPosition, Quaternion.identity, miscItems.transform));
        currentFill++;

        if (currentFill >= currentIngredientTarget)
        {
            NextIngredient();
        }
    }
}