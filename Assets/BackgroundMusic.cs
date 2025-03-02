using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance; // Singleton mantýðý

    void Awake()
    {
        if (instance == null) // Eðer baþka yoksa bunu koru
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Sahne deðiþse bile müzik devam etsin
            GetComponent<AudioSource>().Play(); // Müzik baþlasýn
        }
        else
        {
            Destroy(gameObject); // Eðer zaten varsa, yeni oluþaný yok et
        }
    }
}
