using UnityEngine;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;

[CreateAssetMenu(fileName = "Cocktail_SO", menuName = "Scriptable Objects/Cocktail_SO")]
public class Cocktail_SO : ScriptableObject
{
    public string cocktailName;
    public Sprite cocktailImage;
    public List<CocktailIngredient> ingredients;
    public float alcoholPercentage;
    public bool isAlcoholic;
    public Image cocktailPostit;
}

[System.Serializable]
public class CocktailIngredient
{
    public Drinks_SO drink; 
    public float amountInCl;
}
