using UnityEngine;

public class SelectIngredient : MonoBehaviour
{
    public void SelectBottle(Drinks_SO drink){
        GameManager.Instance.AddDrink(drink);
    }
}
