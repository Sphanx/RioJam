using UnityEngine;

[CreateAssetMenu(fileName = "Drinks_SO", menuName = "Scriptable Objects/Drinks_SO")]
public class Drinks_SO : ScriptableObject
{
    public string drinkName;
    public Sprite drinkImage;
    public float alcoholPercentage;
}
