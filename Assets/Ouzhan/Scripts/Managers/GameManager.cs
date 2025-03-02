using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager: MonoBehaviour
{
    public static GameManager Instance;

    public Cocktail_SO cocktail;
    public List<Drinks_SO> selectedDrinks = new List<Drinks_SO>();

    public GameObject cocktailPostitObject;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Sahne geçişlerinde kaybolmaz

            // cocktailPostit'i bir GameObject'e atama
            if (cocktail != null && cocktail.cocktailPostit != null)
            {
                cocktailPostitObject.GetComponent<SpriteRenderer>().sprite = cocktail.cocktailImage;
                cocktailPostitObject.SetActive(false);
            }


        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
     NewPanel();   
    }
    private void NewPanel()
    {
        // İlk sahne hariç diğer sahnelerde cocktailPostitObject'i aktif hale getir
        if (UIManager.Instance.GetCurrentPanelIndex() == 0 ||UIManager.Instance.GetCurrentPanelIndex() == 1)
        {
            cocktailPostitObject.SetActive(false);
        }
        else{
            cocktailPostitObject.SetActive(true);
            cocktailPostitObject.GetComponent<SpriteRenderer>().sprite = cocktail.cocktailImage;
        }
    }

    public void AddDrink(Drinks_SO drink)
    {
        selectedDrinks.Add(drink);
        Debug.Log(drink.drinkName + " eklendi!");
    }
    public void AddCocktail(Cocktail_SO cocktail){
        this.cocktail = cocktail;
    }
}
