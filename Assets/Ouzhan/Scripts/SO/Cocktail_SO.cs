using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Cocktail_SO", menuName = "Scriptable Objects/Cocktail_SO")]
public class Cocktail_SO : ScriptableObject
{
    public string cocktailName;
    public Sprite cocktailImage;
    public List<CocktailIngredient> ingredients;
    public float alcoholPercentage;
    public bool isAlcoholic;
}

[System.Serializable]
public class CocktailIngredient
{
    public Drinks_SO drink; 
    public float amountInCl;
}
