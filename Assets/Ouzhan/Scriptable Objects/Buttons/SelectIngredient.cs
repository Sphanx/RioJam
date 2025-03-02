using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SelectIngredient : MonoBehaviour
{
    [Header("Hata Mesajı Ayarları")]
    public GameObject dialogueBox;           // Hata mesajı için DialogueBox objesi
    public GameObject nextButton;
    public TMP_Text errorMessageText;        // Hata mesajı metni
    public float errorMessageDuration = 2f;  // Hata mesajının ekranda kalma süresi (saniye)
    
    private Coroutine errorMessageCoroutine; // Hata mesajı coroutine'i

    public void SelectBottle(Drinks_SO drink)
    {
        // Eğer GameManager'da bir kokteyl tanımlanmışsa
        if (GameManager.Instance.cocktail != null)
        {
            bool isIngredientNeeded = false;
            
            // Seçilen içeceğin kokteyl için gerekli olup olmadığını kontrol et
            foreach (CocktailIngredient ingredient in GameManager.Instance.cocktail.ingredients)
            {
                if (ingredient.drink.drinkName == drink.drinkName)
                {
                    isIngredientNeeded = true;
                    break;
                }
            }
            
            // Eğer içecek gerekli değilse, hata mesajı göster
            if (!isIngredientNeeded)
            {
                ShowErrorMessage(GameManager.Instance.cocktail.cocktailName, drink.drinkName);
                return; // İçeceği ekleme
            }
            // Doğru içecek seçildiğinde diyalog kutusunu etkinleştir
            errorMessageText.text = "this will do";
            dialogueBox.SetActive(true);
        }
        
        // İçeceği GameManager'a ekle
        GameManager.Instance.AddDrink(drink);
        
        // Eğer GameManager'da kokteyl için gerekli iki içecek eklenmişse
        if (GameManager.Instance.cocktail != null &&
            GameManager.Instance.selectedDrinks.Count >= 2 &&
            GameManager.Instance.selectedDrinks.Count >= GameManager.Instance.cocktail.ingredients.Count)
        {
            // UIManager'dan nextPanel metodunu çağır
            nextButton.SetActive(true);
        }
    }

    // Hata mesajını göster 
    private void ShowErrorMessage(string cocktailName, string drinkName)
    {
        // Eğer DialogueBox veya errorMessageText null ise, hata mesajı gösterme
        if (dialogueBox == null || errorMessageText == null)
        {
            Debug.LogWarning("DialogueBox veya errorMessageText atanmamış!");
            return;
        }
        
        // Eğer önceki bir hata mesajı coroutine'i varsa, durdur
        if (errorMessageCoroutine != null)
        {
            StopCoroutine(errorMessageCoroutine);
        }
        
        // Hata mesajını ayarla
        errorMessageText.text = $"-I don't need {drinkName} for {cocktailName}.";
        
        // DialogueBox'ı aktifleştir
        dialogueBox.SetActive(true);
        
        // Hata mesajı coroutine'ini başlat
        errorMessageCoroutine = StartCoroutine(HideErrorMessageAfterDelay());
    }
    
    // Belirli bir süre sonra hata mesajını gizle
    private IEnumerator HideErrorMessageAfterDelay()
    {
        yield return new WaitForSeconds(errorMessageDuration);
        
        // DialogueBox'ı deaktifleştir
        dialogueBox.SetActive(false);
        
        // Coroutine referansını temizle
        errorMessageCoroutine = null;
    }
}
