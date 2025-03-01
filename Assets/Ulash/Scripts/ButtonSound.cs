using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverSound : MonoBehaviour, IPointerEnterHandler
{
    public AudioClip hoverSound; // Ses dosyası
    private AudioSource audioSource;

    void Start()
    {
        // AudioSource bileşenini ekleyelim
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = hoverSound;
    }

    // Fare butonun üzerine geldiğinde bu fonksiyon çağrılır
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play(); // Ses çal
        }
    }
}