using TMPro;
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
    public GameObject dialogueBox; // Diyalog kutusunu aç/kapatmak için
    public TMP_Text dialogueText;
    private Rigidbody2D rb;
    public DialogueManager dialogueManager; // Diyalog yöneticisi referansı
    public GameObject nextButton; // Next butonu referansı,
    public GameObject throwCupPanel;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        successImage.SetActive(false);
        failImage.SetActive(false);
        dialogueBox.SetActive(false); // Diyalog kutusu başta kapalı olsun
    }

    void Update()
    {
         if (powerbarScript.hasGameStarted)
        {
            if (rb.linearVelocity.magnitude < 0.1f)
            {
                timer += Time.deltaTime;
                if (timer >= stationaryTime)
                {
                    hasStopped = true;
                }
            }
            else
            {
                timer = 0f;
                hasStopped = false;
            }

            if (hasStopped)
            {
                ShowDialogue(isInTarget);
            }
        }
    }

    void ShowDialogue(bool success)
    {
        dialogueBox.SetActive(true);
        if (success)
        {
            dialogueText.text = dialogueManager.GetSuccessDialogue();
        }
        else
        {
            dialogueText.text = dialogueManager.GetFailDialogue();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TargetZone"))
        {
            isInTarget = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("TargetZone"))
        {
            isInTarget = false;
        }
    }
    
    // Next butonu için fonksiyon
    public void NextButton()
    {
        // GameManager'ı temizle
        GameManager.Instance.selectedDrinks.Clear();
        GameManager.Instance.cocktail = null;
        Debug.Log("GameManager temizlendi.");
        
        // Bulunduğu sahneyi deaktif et
        throwCupPanel.SetActive(false);
        // UIManager'da element1'e dön
        UIManager.Instance.ActivatePanel(1);
        
        // currentCustomerIndex değerini 1 artır
        dialogueManager.currentCustomerIndex++;
        
        // Debug log
        Debug.Log("Next butonu tıklandı. Müşteri indeksi: " + dialogueManager.currentCustomerIndex);
        DialogueManager.Instance.StartDialogue();
    }
}
