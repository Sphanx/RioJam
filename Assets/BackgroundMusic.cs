using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance; // Singleton mant���

    void Awake()
    {
        if (instance == null) // E�er ba�ka yoksa bunu koru
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Sahne de�i�se bile m�zik devam etsin
            GetComponent<AudioSource>().Play(); // M�zik ba�las�n
        }
        else
        {
            Destroy(gameObject); // E�er zaten varsa, yeni olu�an� yok et
        }
    }
}
