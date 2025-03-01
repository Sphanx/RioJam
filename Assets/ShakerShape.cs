using UnityEngine;
using UnityEngine.EventSystems;

public class ShakerShape : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ShakerManaging shakerManager; // UI �ekillerini y�neten nesne

    public void OnPointerEnter(PointerEventData eventData)
    {
        shakerManager.OnPointerEnter(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        shakerManager.OnPointerExit(eventData);
    }
}

