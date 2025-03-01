using UnityEngine;

[CreateAssetMenu(fileName = "CustomerData", menuName = "Scriptable Objects/CustomerData")]
public class CustomerData : ScriptableObject
{
    public int currentCustomerIndex = 0; // Müşteri sırasını tutar
}
