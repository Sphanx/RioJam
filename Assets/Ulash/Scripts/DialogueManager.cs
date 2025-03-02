using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [System.Serializable]
    public class Customer
    {
        public string customerName;      // Müşteri adı
        public string customerDialogue;  // Müşteri konuşması
        public string playerResponse;    // Bizim cevabımız
        public string customerFinalReply; // Müşterinin son cevabı
        public string successDialogue;   // Başarı diyaloğu
        public string failDialogue;      // Başarısızlık diyaloğu
        public Sprite playerSprite;      // Barmen resmi
        public Sprite customerSprite;    // Müşteri resmi
        public Cocktail_SO customerCocktail; // Müşterinin istediği kokteyl

        // Ses dosyaları
        public AudioClip customerDialogueSound; // Müşteri konuşma sesi
        public AudioClip playerResponseSound;   // Barmen cevap sesi
        public AudioClip customerFinalReplySound; // Müşteri kapanış sesi
        public AudioClip successDialogueSound;   // Başarı sesi
        public AudioClip failDialogueSound;      // Başarısızlık sesi
    }

    [Header("UI Referansları")]
    public Image customerImageUI;       // UI'daki müşteri resmi
    public Image playerImageUI;         // UI'daki barmen resmi
    public TMP_Text dialogueText;       // Sohbet metni
    public TMP_Text speakerNameText;    // Konuşmacı adı
    public Button nextDialogueButton;   // Diyalog ilerletme butonu
    public Button sceneChangeButton;    // Sahne değişim butonu
    public GameObject dialoguePanel;    // Diyalog paneli

    [Header("Diyalog Ayarları")]
    public float typingSpeed = 0.05f;   // Yazı yazma hızı
    public Customer[] customers;        // Müşteri listesi
    public int currentCustomerIndex = 0; // Şu anki müşteri indeksi

    [Header("Ses Efektleri")]
    public AudioClip typingSound;       // Yazı yazma sesi
    public AudioClip buttonClickSound;  // Buton tıklama sesi

    private int dialogueStage = 0;      // 0 = müşteri, 1 = biz, 2 = müşteri kapanış
    private bool isTyping = false;      // Yazı yazılıyor mu?
    private AudioSource audioSource;    // Ses kaynağı
    private Coroutine typingCoroutine;  // Yazı yazma coroutine'i

    private void Awake()
    {
        // Ses kaynağı oluştur
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = 0.5f;
    }

    private void Start()
    {
        // Buton dinleyicilerini ekle
        nextDialogueButton.onClick.AddListener(AdvanceDialogue);
        sceneChangeButton.onClick.AddListener(ChangeScene);

        // Sonraki sahne butonunu başlangıçta devre dışı bırak
        sceneChangeButton.gameObject.SetActive(false);

        // Diyalog panelini başlangıçta gizle
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }

        // Start butonunu bul ve dinleyici ekle
        Button startButton = GameObject.Find("StartButton").GetComponent<Button>(); // Start butonunun adını doğru şekilde ayarlayın
        startButton.onClick.AddListener(OnStartButtonClicked);
    }

    // Start butonuna tıklandığında çağrılır
    private void OnStartButtonClicked()
    {
        // Diyalog panelini göster
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
        }

        // İlk diyaloğu başlat
        StartDialogue();
    }

    // Diyaloğu başlat
    public void StartDialogue()
    {
        dialogueStage = 0;
        DisplayCurrentDialogue();
    }

    // Diyaloğu ilerlet
    public void AdvanceDialogue()
    {
        // Eğer yazı yazılıyorsa, yazıyı tamamla
        if (isTyping)
        {
            CompleteTyping();
            return;
        }
        // Ses çal
        PlaySound(buttonClickSound);
        // Diyalog aşamasını ilerlet
        dialogueStage++;

        // Diyalog aşamasına göre işlem yap
        if (dialogueStage > 2)
        {
            // Diyalog tamamlandı, kokteyli gönder ve sonraki sahne butonunu aktifleştir
            CompleteCocktailOrder();
        }
        else
        {
            // Sonraki diyaloğu göster
            DisplayCurrentDialogue();
        }
    }

    // Mevcut diyaloğu göster
    private void DisplayCurrentDialogue()
    {
        // Müşteri listesi boş kontrolü
        if (customers == null || customers.Length == 0 || currentCustomerIndex >= customers.Length)
        {
            Debug.LogError("Müşteri listesi boş veya geçersiz indeks!");
            return;
        }

        Customer currentCustomer = customers[currentCustomerIndex];
        string dialogueContent = "";
        string speakerName = "";
        AudioClip dialogueSound = null;

        // Diyalog aşamasına göre içeriği belirle
        switch (dialogueStage)
        {
            case 0: // Müşteri konuşması
                dialogueContent = currentCustomer.customerDialogue;
                speakerName = currentCustomer.customerName;
                dialogueSound = currentCustomer.customerDialogueSound;
                if (customerImageUI != null)
                {
                    customerImageUI.sprite = currentCustomer.customerSprite;
                    customerImageUI.gameObject.SetActive(true);
                }
                if (playerImageUI != null)
                {
                    playerImageUI.gameObject.SetActive(false);
                }
                break;
            case 1: // Barmen (oyuncu) cevabı
                dialogueContent = currentCustomer.playerResponse;
                speakerName = "Barmen";
                dialogueSound = currentCustomer.playerResponseSound;
                if (playerImageUI != null)
                {
                    playerImageUI.sprite = currentCustomer.playerSprite;
                    playerImageUI.gameObject.SetActive(true);
                }
                if (customerImageUI != null)
                {
                    customerImageUI.gameObject.SetActive(false);
                }
                break;
            case 2: // Müşteri kapanış konuşması
                dialogueContent = currentCustomer.customerFinalReply;
                speakerName = currentCustomer.customerName;
                dialogueSound = currentCustomer.customerFinalReplySound;
                if (customerImageUI != null)
                {
                    customerImageUI.sprite = currentCustomer.customerSprite;
                    customerImageUI.gameObject.SetActive(true);
                }
                if (playerImageUI != null)
                {
                    playerImageUI.gameObject.SetActive(false);
                }
                break;
        }

        // Konuşmacı adını ayarla
        if (speakerNameText != null)
        {
            speakerNameText.text = speakerName;
        }

        // Ses çal
        if (dialogueSound != null)
        {
            PlaySound(dialogueSound);
        }

        // Yazı animasyonunu başlat
        StartTypingEffect(dialogueContent);
    }

    // Yazı yazma efekti
    private void StartTypingEffect(string text)
    {
        // Eğer önceki yazı yazma işlemi devam ediyorsa, durdur
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        // Yeni yazı yazma işlemini başlat
        typingCoroutine = StartCoroutine(TypeText(text));
    }

    // Yazı yazma coroutine'i
    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;

            // Her harf için ses çal
            if (typingSound != null && letter != ' ')
            {
                PlaySound(typingSound);
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        typingCoroutine = null;
    }

    // Yazı yazma işlemini tamamla
    private void CompleteTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        Customer currentCustomer = customers[currentCustomerIndex];

        // Diyalog aşamasına göre tam metni göster
        switch (dialogueStage)
        {
            case 0:
                dialogueText.text = currentCustomer.customerDialogue;
                break;
            case 1:
                dialogueText.text = currentCustomer.playerResponse;
                break;
            case 2:
                dialogueText.text = currentCustomer.customerFinalReply;
                break;
        }

        isTyping = false;
    }

    // Kokteyl siparişini tamamla
    private void CompleteCocktailOrder()
    {
        Customer currentCustomer = customers[currentCustomerIndex];

        // Müşterinin istediği kokteyli GameManager'a gönder
        if (currentCustomer.customerCocktail != null)
        {
            GameManager.Instance.AddCocktail(currentCustomer.customerCocktail);
            Debug.Log(currentCustomer.customerName + " için " + currentCustomer.customerCocktail.cocktailName + " hazırlandı!");
        }
        else
        {
            Debug.LogWarning(currentCustomer.customerName + " için kokteyl tanımlanmamış!");
        }

        // Sonraki sahne butonunu aktifleştir
        sceneChangeButton.gameObject.SetActive(true);
        nextDialogueButton.gameObject.SetActive(false);

        // Diyalog panelini gizle (opsiyonel)
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }

    // Ses çal
    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    // Sonraki sahneye geç
    void ChangeScene()
    {
        UIManager.Instance.NextPanel();
    }

    // Sonraki müşteriye geç
    public void NextCustomer()
    {
        if (customers != null && customers.Length > 0)
        {
            currentCustomerIndex = (currentCustomerIndex + 1) % customers.Length;
            dialogueStage = 0;

            // Diyalog panelini tekrar göster
            if (dialoguePanel != null)
            {
                dialoguePanel.SetActive(true);
            }

            // Butonları ayarla
            nextDialogueButton.gameObject.SetActive(true);
            sceneChangeButton.gameObject.SetActive(false);

            // Yeni diyaloğu başlat
            DisplayCurrentDialogue();
        }
    }

    // Belirli bir müşteriye geç
    public void SetCustomer(int index)
    {
        if (customers != null && index >= 0 && index < customers.Length)
        {
            currentCustomerIndex = index;
            dialogueStage = 0;

            // Diyalog panelini tekrar göster
            if (dialoguePanel != null)
            {
                dialoguePanel.SetActive(true);
            }

            // Butonları ayarla
            nextDialogueButton.gameObject.SetActive(true);
            sceneChangeButton.gameObject.SetActive(false);

            // Yeni diyaloğu başlat
            DisplayCurrentDialogue();
        }
        else
        {
            Debug.LogError("Geçersiz müşteri indeksi: " + index);
        }
    }

    public void TriggerSuccessDialogue()
    {
        if (currentCustomerIndex < customers.Length)
        {
            dialoguePanel.SetActive(true);
            dialogueText.text = customers[currentCustomerIndex].successDialogue;
            PlaySound(customers[currentCustomerIndex].successDialogueSound);
        }
    }

    public void TriggerFailDialogue()
    {
        if (currentCustomerIndex < customers.Length)
        {
            dialoguePanel.SetActive(true);
            dialogueText.text = customers[currentCustomerIndex].failDialogue;
            PlaySound(customers[currentCustomerIndex].failDialogueSound);
        }
    }

    public string GetSuccessDialogue()
    {
        if (currentCustomerIndex < customers.Length)
            return customers[currentCustomerIndex].successDialogue;
        return "Başarıyla tamamlandı!";
    }

    public string GetFailDialogue()
    {
        if (currentCustomerIndex < customers.Length)
            return customers[currentCustomerIndex].failDialogue;
        return "Başarısızlık!";
    }
}