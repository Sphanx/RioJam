using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ShakerManaging : MonoBehaviour
{
    public ShakerController shakerController; // Shaker kontrolc�s�
    public Image[] shapes; // UI �ekillerinin listesi
    public Slider progressBar; // 5 saniye dolma �ubu�u
    public Canvas canvas;
    public float disappearTime = 1f; // �eklin ekranda kalma s�resi
    public float maxHoldTime = 5f; // Ka� saniye tutarsak yeni sahneye ge�elim?

    private float holdTimer = 0f; // Fareyi ka� saniye tuttuk?
    private int currentShapeIndex = 0; // �u an hangi �ekil a��k?

    void Start()
    {
        progressBar.value = 0;
        ShowNextShape();
    }

    void ShowNextShape()
    {
        // �nce t�m �ekilleri kapat
        foreach (var shape in shapes)
        {
            shape.gameObject.SetActive(false);
        }

        // Rastgele bir �ekli a� ve belirlenen aral�kta konumland�r
        if (shapes.Length > 0)
        {
            currentShapeIndex = Random.Range(0, shapes.Length);
            shapes[currentShapeIndex].gameObject.SetActive(true);

            // **�ekli rastgele noktaya yerle�tir**
            shapes[currentShapeIndex].rectTransform.anchoredPosition = GetRandomPosition();

            // 1 saniye sonra yeni bir �ekil g�ster
            Invoke("ShowNextShape", disappearTime);
        }
    }

    Vector2 GetRandomPosition()
    {
        float randomX = Random.Range(-800, 600);
        float randomY = Random.Range(-400, 400);
        return new Vector2(randomX, randomY);
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        shakerController.StartShaking();
        StopAllCoroutines();
        StartCoroutine(FillProgressBar());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        shakerController.StopShaking();
        StopAllCoroutines();
        holdTimer = 0;
    }

    IEnumerator FillProgressBar()
    {
        while (holdTimer < maxHoldTime)
        {
            holdTimer += Time.deltaTime;
            progressBar.value += 0.3f;

            if (progressBar.value == progressBar.maxValue)
            {
                progressBar.value = 0;
                UIManager.Instance.NextPanel();
            }

            yield return null;
        }
    }

}