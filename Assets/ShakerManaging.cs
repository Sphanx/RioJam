using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShakerManaging : MonoBehaviour
{
    public ShakerController shakerController; // Shaker kontrolcüsü
    public Image[] shapes; // UI þekillerinin listesi
    public float disappearTime = 1f; // Þeklin kaç saniye ekranda kalacaðýný belirler
    private int currentShapeIndex = 0; // Þu an hangi þekil açýk?

    void Start()
    {
        // Tüm þekilleri kapat, sadece ilkini aç
        for (int i = 0; i < shapes.Length; i++)
        {
            shapes[i].gameObject.SetActive(i == 0);
        }

        // Ýlk þeklin zamanlayýcýsýný baþlat
        if (shapes.Length > 0)
        {
            StartCoroutine(DisappearAndShowNext());
        }
    }

    IEnumerator DisappearAndShowNext()
    {
        while (currentShapeIndex < shapes.Length)
        {
            yield return new WaitForSeconds(disappearTime); // Belirtilen süreyi bekle

            // Mevcut þekli kapat
            shapes[currentShapeIndex].gameObject.SetActive(false);
            currentShapeIndex++;

            // Sýradaki þekli aç (eðer varsa)
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


