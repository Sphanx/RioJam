using System.Collections.Generic;
using UnityEngine;

public class GameManager: MonoBehaviour
{
    public static GameManager Instance;

    public Cocktail_SO cocktail;
    public List<Drinks_SO> selectedDrinks = new List<Drinks_SO>(); 
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Sahne geçişlerinde kaybolmaz
        }
        else
        {
            Destroy(gameObject);
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
