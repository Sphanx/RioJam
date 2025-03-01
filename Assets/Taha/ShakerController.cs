using UnityEngine;

public class ShakerController : MonoBehaviour
{
    public float shakeAmount = 0.2f; // Sallanma mesafesi
    public float shakeSpeed = 10f;   // Sallanma h�z�
    private bool isShaking = false;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position; // Ba�lang�� pozisyonunu kaydet
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
        isShaking = true;
    }

    public void StopShaking()
    {
        isShaking = false;
    }
}
