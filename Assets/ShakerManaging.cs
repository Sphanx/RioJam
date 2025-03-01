using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShakerManaging : MonoBehaviour
{
    public ShakerController shakerController; // Shaker kontrolc�s�
    public Image[] shapes; // UI �ekillerinin listesi
    public float disappearTime = 1f; // �eklin ka� saniye ekranda kalaca��n� belirler
    private int currentShapeIndex = 0; // �u an hangi �ekil a��k?

    void Start()
    {
        // T�m �ekilleri kapat, sadece ilkini a�
        for (int i = 0; i < shapes.Length; i++)
        {
            shapes[i].gameObject.SetActive(i == 0);
        }

        // �lk �eklin zamanlay�c�s�n� ba�lat
        if (shapes.Length > 0)
        {
            StartCoroutine(DisappearAndShowNext());
        }
    }

    IEnumerator DisappearAndShowNext()
    {
        while (currentShapeIndex < shapes.Length)
        {
            yield return new WaitForSeconds(disappearTime); // Belirtilen s�reyi bekle

            // Mevcut �ekli kapat
            shapes[currentShapeIndex].gameObject.SetActive(false);
            currentShapeIndex++;

            // S�radaki �ekli a� (e�er varsa)
            if (currentShapeIndex < shapes.Length)
            {
                shapes[currentShapeIndex].gameObject.SetActive(true);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        shakerController.StartShaking();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        shakerController.StopShaking();
    }
}


