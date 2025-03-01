using UnityEngine;
using UnityEngine.EventSystems;

public class ShakerShape : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ShakerManaging shakerManager;

    public void OnPointerEnter(PointerEventData eventData)
    {
        shakerManager.OnPointerEnter(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        shakerManager.OnPointerExit(eventData);
    }
}

