using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenü : MonoBehaviour
{
    public GameObject sesAyarPanel; // Ses ayarları paneli

    public void PlayGame()
    {
        SceneManager.LoadScene("sahne1"); // Oyun sahnesine geçiş yap
    }

    public void OpenSesAyar()
    {
        sesAyarPanel.SetActive(true); // Ses ayarı panelini aç
    }

    public void CloseSesAyar()
    {
        sesAyarPanel.SetActive(false); // Ses ayarı panelini kapat
    }
}