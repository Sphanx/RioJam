using UnityEngine;

public class TargetZone : MonoBehaviour
{
    public PoweBar powerbarScript;
    public GameObject successImage; // Başarı resmi
    public GameObject failImage;    // Başarısızlık resmi
    public float stationaryTime = 2f; // Hedefte kaç saniye durmalı
    private float timer = 0f;
    private bool isInTarget = false;
    private bool hasStopped = false; // Bardak durdu mu?
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        successImage.SetActive(false);
        failImage.SetActive(false);
    }

    void Update()
    {
        if (powerbarScript.hasGameStarted)
        {
            // Bardak çok yavaşsa (neredeyse duruyorsa)
            if (rb.linearVelocity.magnitude < 0.1f)
            {
                timer += Time.deltaTime;
                if (timer >= stationaryTime) // Belirtilen sürede hareketsizse
                {
                    hasStopped = true;
                }
            }
            else
            {
                timer = 0f; // Hareket ederse süreyi sıfırla
                hasStopped = false;
            }

            // Eğer bardak durduysa ve hedef alandaysa başarı
            if (hasStopped && isInTarget)
            {
                successImage.SetActive(true);
                failImage.SetActive(false);
            }
            // Eğer bardak durdu ama hedef alanda değilse başarısızlık
            else if (hasStopped && !isInTarget)
            {
                failImage.SetActive(true);
                successImage.SetActive(false);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TargetZone")) // Hedef alana girerse
        {
            isInTarget = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("TargetZone")) // Hedef alandan çıkarsa
        {
            isInTarget = false;
            successImage.SetActive(false);

            if (hasStopped) // Eğer bardak durmadan çıktıysa başarısızlık resmi aç
            {
                failImage.SetActive(true);
            }
        }
    }
}
