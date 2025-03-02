using UnityEngine;

public class ShelvesPanelManager : MonoBehaviour
{
    public GameObject dialogueBox;           // Hata mesajı için DialogueBox objesi
    public GameObject nextButton;
    
    public void Next()
    {
        nextButton.SetActive(false);
        dialogueBox.SetActive(false);
        UIManager.Instance.NextPanel()
;    }
}
