using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject customerOrderPanel;


    private GameObject currentPanel;

    void Start()
    {
        currentPanel = customerOrderPanel; 
        currentPanel.SetActive(true);
    }

    public void NextPanel(GameObject nextPanel)
    {
        if (currentPanel != null)
        {
            currentPanel.SetActive(false);
        }

        nextPanel.SetActive(true);
        currentPanel = nextPanel;
    }
}