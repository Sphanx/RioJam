using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Panel listesi - Unity Inspector'da düzenlenebilir
    [SerializeField] private List<GameObject> panels = new List<GameObject>();

    // Şu anki aktif panel indeksi
    private int currentPanelIndex = 0;

    // Singleton instance
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Başlangıçta tüm panelleri deaktif et
        DeactivateAllPanels();

        // Eğer panel listesi boş değilse, ilk paneli aktif et
        if (panels.Count > 0)
        {
            ActivatePanel(0);
        }
        else
        {
            Debug.LogWarning("Panel listesi boş! UIManager'a panel ekleyin.");
        }
    }

    // Tüm panelleri deaktif et
    private void DeactivateAllPanels()
    {
        foreach (GameObject panel in panels)
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }
    }

    // Belirli bir indeksteki paneli aktif et
    public void ActivatePanel(int index)
    {
        // Geçerli indeks kontrolü
        if (index < 0 || index >= panels.Count)
        {
            Debug.LogError("Geçersiz panel indeksi: " + index);
            return;
        }

        // Şu anki paneli deaktif et
        if (currentPanelIndex >= 0 && currentPanelIndex < panels.Count)
        {
            panels[currentPanelIndex].SetActive(false);
        }

        // Yeni paneli aktif et
        panels[index].SetActive(true);
        currentPanelIndex = index;

        Debug.Log("Panel aktif edildi: " + index);
    }

    // Bir sonraki panele geç
    public void NextPanel()
    {
        int nextIndex = currentPanelIndex + 1;
        ActivatePanel(nextIndex);

    }

    // Bir önceki panele geç
    public void PreviousPanel()
    {
        int prevIndex = currentPanelIndex - 1;
        ActivatePanel(prevIndex);
    }

    // Belirli bir paneli aktif et (GameObject referansı ile)
    public void ActivatePanel(GameObject panel)
    {
        int index = panels.IndexOf(panel);
        if (index != -1)
        {
            ActivatePanel(index);
        }
        else
        {
            Debug.LogError("Panel listede bulunamadı: " + panel.name);
        }
    }

    // Panel sırasını değiştir (iki indeks arasında)
    public void SwapPanels(int index1, int index2)
    {
        // Geçerli indeks kontrolü
        if (index1 < 0 || index1 >= panels.Count || index2 < 0 || index2 >= panels.Count)
        {
            Debug.LogError("Geçersiz panel indeksi: " + index1 + " veya " + index2);
            return;
        }

        // Panellerin yerini değiştir
        GameObject temp = panels[index1];
        panels[index1] = panels[index2];
        panels[index2] = temp;

        Debug.Log("Paneller yer değiştirdi: " + index1 + " <-> " + index2);

        // Eğer aktif panel değiştiyse, görünümü güncelle
        if (currentPanelIndex == index1)
        {
            currentPanelIndex = index2;
        }
        else if (currentPanelIndex == index2)
        {
            currentPanelIndex = index1;
        }
    }

    // Bir paneli belirli bir indekse taşı
    public void MovePanel(int sourceIndex, int targetIndex)
    {
        // Geçerli indeks kontrolü
        if (sourceIndex < 0 || sourceIndex >= panels.Count || targetIndex < 0 || targetIndex >= panels.Count)
        {
            Debug.LogError("Geçersiz panel indeksi: " + sourceIndex + " veya " + targetIndex);
            return;
        }

        // Aynı indeks ise işlem yapma
        if (sourceIndex == targetIndex)
        {
            return;
        }

        // Taşınacak paneli al
        GameObject panelToMove = panels[sourceIndex];

        // Şu anki aktif panel indeksini takip et
        int newCurrentIndex = currentPanelIndex;

        // Eğer aktif panel taşınıyorsa, yeni indeksini güncelle
        if (currentPanelIndex == sourceIndex)
        {
            newCurrentIndex = targetIndex;
        }
        // Eğer aktif panel, taşınan panelin yolunda ise, indeksini güncelle
        else if ((sourceIndex < currentPanelIndex && currentPanelIndex <= targetIndex) ||
                 (targetIndex <= currentPanelIndex && currentPanelIndex < sourceIndex))
        {
            if (sourceIndex < targetIndex)
            {
                newCurrentIndex--;
            }
            else
            {
                newCurrentIndex++;
            }
        }

        // Paneli listeden çıkar
        panels.RemoveAt(sourceIndex);

        // Hedef indekse ekle
        if (targetIndex > panels.Count)
        {
            panels.Add(panelToMove);
        }
        else
        {
            panels.Insert(targetIndex, panelToMove);
        }

        // Aktif panel indeksini güncelle
        currentPanelIndex = newCurrentIndex;

        Debug.Log("Panel taşındı: " + sourceIndex + " -> " + targetIndex);
    }

    // Yeni panel ekle
    public void AddPanel(GameObject panel)
    {
        if (panel != null)
        {
            panels.Add(panel);
            panel.SetActive(false);
            Debug.Log("Yeni panel eklendi: " + panel.name);
        }
    }

    // Panel kaldır
    public void RemovePanel(int index)
    {
        // Geçerli indeks kontrolü
        if (index < 0 || index >= panels.Count)
        {
            Debug.LogError("Geçersiz panel indeksi: " + index);
            return;
        }

        // Kaldırılacak paneli deaktif et
        panels[index].SetActive(false);

        // Eğer aktif panel kaldırılıyorsa, başka bir panele geç
        if (currentPanelIndex == index)
        {
            if (panels.Count > 1)
            {
                // Bir sonraki panele geç (veya listenin sonundaysa ilk panele)
                int nextIndex = (index + 1) % panels.Count;
                panels[nextIndex].SetActive(true);
                currentPanelIndex = nextIndex;
            }
            else
            {
                // Başka panel kalmadı
                currentPanelIndex = -1;
            }
        }
        // Eğer kaldırılan panel, aktif panelden önceyse, aktif panel indeksini güncelle
        else if (index < currentPanelIndex)
        {
            currentPanelIndex--;
        }

        // Paneli listeden kaldır
        panels.RemoveAt(index);

        Debug.Log("Panel kaldırıldı: " + index);
    }

    // Şu anki aktif panel indeksini döndür
    public int GetCurrentPanelIndex()
    {
        return currentPanelIndex;
    }

    // Panel sayısını döndür
    public int GetPanelCount()
    {
        return panels.Count;
    }

    // Belirli bir indeksteki paneli döndür
    public GameObject GetPanel(int index)
    {
        if (index >= 0 && index < panels.Count)
        {
            return panels[index];
        }
        return null;
    }

    // Şu anki aktif paneli döndür
    public GameObject GetCurrentPanel()
    {
        if (currentPanelIndex >= 0 && currentPanelIndex < panels.Count)
        {
            return panels[currentPanelIndex];
        }
        return null;
    }
}