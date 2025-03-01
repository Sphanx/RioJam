using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerData", menuName = "Scriptable Objects/CustomerData")]
public class CustomerData : ScriptableObject
{
    [Header("Müşteri Takibi")]
    public int currentCustomerIndex = 0; // Aktif müşteri sırasını tutar
    
    [Header("Müşteri Tercihleri")]
    [Tooltip("Müşterilerin tercih ettiği içecekler")]
    public List<CustomerPreference> customerPreferences = new List<CustomerPreference>();
    
    [Header("Müşteri İstatistikleri")]
    public int totalCustomersServed = 0; // Toplam hizmet edilen müşteri sayısı
    public int satisfiedCustomers = 0; // Memnun edilen müşteri sayısı
    public int dissatisfiedCustomers = 0; // Memnun edilmeyen müşteri sayısı
    
    [Header("Oyun İlerlemesi")]
    public bool tutorialCompleted = false; // Öğretici tamamlandı mı?
    public int dayNumber = 1; // Oyun günü
    public float customerSatisfactionRating = 0f; // Müşteri memnuniyet puanı (0-100)
    
    // Müşteri tercihlerini kaydetmek için yardımcı metod
    public void AddCustomerPreference(string customerName, Cocktail_SO preferredCocktail, bool isRegular)
    {
        // Eğer müşteri zaten kayıtlıysa, tercihini güncelle
        for (int i = 0; i < customerPreferences.Count; i++)
        {
            if (customerPreferences[i].customerName == customerName)
            {
                customerPreferences[i].preferredCocktail = preferredCocktail;
                customerPreferences[i].visitCount++;
                customerPreferences[i].isRegular = isRegular;
                return;
            }
        }
        
        // Yeni müşteri tercihi ekle
        CustomerPreference newPreference = new CustomerPreference
        {
            customerName = customerName,
            preferredCocktail = preferredCocktail,
            visitCount = 1,
            isRegular = isRegular,
            lastVisitDay = dayNumber
        };
        
        customerPreferences.Add(newPreference);
    }
    
    // Müşteri memnuniyet puanını güncelle
    public void UpdateSatisfactionRating(bool satisfied)
    {
        totalCustomersServed++;
        
        if (satisfied)
        {
            satisfiedCustomers++;
        }
        else
        {
            dissatisfiedCustomers++;
        }
        
        // Memnuniyet puanını hesapla (0-100 arası)
        if (totalCustomersServed > 0)
        {
            customerSatisfactionRating = (float)satisfiedCustomers / totalCustomersServed * 100f;
        }
    }
    
    // Yeni gün başlat
    public void StartNewDay()
    {
        dayNumber++;
    }
    
    // Oyun verilerini sıfırla
    public void ResetData()
    {
        currentCustomerIndex = 0;
        totalCustomersServed = 0;
        satisfiedCustomers = 0;
        dissatisfiedCustomers = 0;
        dayNumber = 1;
        customerSatisfactionRating = 0f;
        customerPreferences.Clear();
    }
}

[System.Serializable]
public class CustomerPreference
{
    public string customerName; // Müşteri adı
    public Cocktail_SO preferredCocktail; // Tercih edilen kokteyl
    public int visitCount = 0; // Ziyaret sayısı
    public bool isRegular = false; // Düzenli müşteri mi?
    public int lastVisitDay = 0; // Son ziyaret günü
}
