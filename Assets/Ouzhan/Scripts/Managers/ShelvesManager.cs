using System.Collections.Generic;
using UnityEngine;

public class ShelvesManager : MonoBehaviour
{
    public static ShelvesManager Instance { get; set; }
    public List<Drinks_SO> selectedDrinks = new List<Drinks_SO>(); 
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    public void AddDrink(Drinks_SO drink)
    {
        selectedDrinks.Add(drink);
        Debug.Log(drink.drinkName + " eklendi!");
    }
}
