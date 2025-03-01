using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ShakerManaging : MonoBehaviour
{
    public ShakerController shakerController; // Shaker kontrolcüsü
    public Image[] shapes; // UI þekillerinin listesi
    public Slider progressBar; // 5 saniye dolma çubuðu
    public Canvas canvas;
    public float disappearTime = 1f; // Þeklin ekranda kalma süresi
    public float maxHoldTime = 5f; // Kaç saniye tutarsak yeni sahneye geçelim?

    private float holdTimer = 0f; // Fareyi kaç saniye tuttuk?
    private int currentShapeIndex = 0; // Þu an hangi þekil açýk?

    void Start()
    {
        progressBar.value = 0;
        ShowNextShape();
    }

    void ShowNextShape()
    {
        // Önce tüm þekilleri kapat
        foreach (var shape in shapes)
        {
            shape.gameObject.SetActive(false);
        }

        // Rastgele bir þekli aç ve belirlenen aralýkta konumlandýr
        if (shapes.Length > 0)
        {
            currentShapeIndex = Random.Range(0, shapes.Length);
            shapes[currentShapeIndex].gameObject.SetActive(true);

            // **Þekli rastgele noktaya yerleþtir**
            shapes[currentShapeIndex].rectTransform.anchoredPosition = GetRandomPosition();

            // 1 saniye sonra yeni bir þekil göster
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

            if (holdTimer >= maxHoldTime)
            {
                LoadNextScene();
            }

            yield return null;
        }
    }

    void LoadNextScene()
    {
        Debug.Log("Yeni sahneye geçiliyor...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}