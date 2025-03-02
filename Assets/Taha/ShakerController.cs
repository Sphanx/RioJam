using UnityEngine;

public class ShakerController : MonoBehaviour
{
    public float shakeAmount = 0.2f; // Sallanma mesafesi
    public float shakeSpeed = 10f;   // Sallanma hızı
    private bool isShaking = false;
    private Vector3 startPosition;
    public AudioSource shakeSes;

    void Start()
    {
        startPosition = transform.position; // Başlangıç pozisyonunu kaydet
    }

    void Update()
    {
        if (isShaking)
        {
            float shakeOffsetY = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
            transform.position = startPosition + new Vector3(0, shakeOffsetY, 0);
        }
    }

    public void StartShaking()
    {
        if (!isShaking) // Zaten çalışıyorsa tekrar başlatmaya gerek yok
        {
            isShaking = true;
            shakeSes.Stop(); // Sesin baştan başlaması için önce durdur
            shakeSes.Play(); // Sonra tekrar başlat
        }
    }

    public void StopShaking()
    {
        if (isShaking) // Eğer zaten durmuşsa tekrar durdurmaya gerek yok
        {
            isShaking = false;
            shakeSes.Stop(); // Sallanma durunca sesi de durdur
        }
    }
}
