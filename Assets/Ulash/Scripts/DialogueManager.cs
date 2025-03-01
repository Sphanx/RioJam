using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [System.Serializable]
    public class Customer
    {
        public string customerDialogue;  // Müşteri konuşması
        public string playerResponse;    // Bizim cevabımız
        public string customerFinalReply; // Müşterinin son cevabı
        public Sprite customerSprite;    // Müşteri resmi
    }

    public Image customerImageUI; // UI'daki müşteri resmi
    public TMP_Text dialogueText; // Sohbet metni
    public Button nextDialogueButton; // Diyalog ilerletme butonu
    public Button sceneChangeButton; // Sahne değişim butonu

    public Customer[] customers; // Müşteri listesi
    public CustomerData customerData; // Müşteri sırasını saklayan ScriptableObject

    private int dialogueStage = 0; // 0 = müşteri, 1 = biz, 2 = müşteri kapanış

    private void Start()
    {
        nextDialogueButton.onClick.AddListener(NextDialogueStage);
        sceneChangeButton.onClick.AddListener(ChangeScene);
        ShowNewCustomer();
    }

    void ShowNewCustomer()
    {
        if (customerData.currentCustomerIndex < customers.Length)
        {
            dialogueStage = 0; // Diyalog başlangıç aşaması
            customerImageUI.sprite = customers[customerData.currentCustomerIndex].customerSprite;
            dialogueText.text = customers[customerData.currentCustomerIndex].customerDialogue;

            nextDialogueButton.gameObject.SetActive(true);
            sceneChangeButton.gameObject.SetActive(false); // Sahne geçiş butonu kapalı başlasın
        }
        else
        {
            dialogueText.text = "Tüm müşterilere hizmet verdin!";
            customerImageUI.gameObject.SetActive(false);
            nextDialogueButton.gameObject.SetActive(false);
            sceneChangeButton.gameObject.SetActive(true);
        }
    }

    void NextDialogueStage()
    {
        if (customerData.currentCustomerIndex >= customers.Length)
            return;

        if (dialogueStage == 0) // Müşteri konuşuyor
        {
            dialogueText.text = customers[customerData.currentCustomerIndex].playerResponse;
            dialogueStage = 1;
        }
        else if (dialogueStage == 1) // Bizim cevabımız çıktı, müşteri son yanıtını veriyor
        {
            dialogueText.text = customers[customerData.currentCustomerIndex].customerFinalReply;
            dialogueStage = 2;
        }
        else if (dialogueStage == 2) // Diyalog tamamlandı, sahne butonu aç
        {
            customerData.currentCustomerIndex++; // Bir sonraki müşteriye geç
            nextDialogueButton.gameObject.SetActive(false); // Diyalog butonunu kapat
            sceneChangeButton.gameObject.SetActive(true); // Sahne değiştirme butonunu aç
        }
    }

    void ChangeScene()
    {
        SceneManager.LoadScene("NextScene"); // Yeni sahneye geç
    }
}