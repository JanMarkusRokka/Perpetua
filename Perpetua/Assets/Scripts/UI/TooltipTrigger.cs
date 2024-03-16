using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string header;
    public string description;

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.Show(header, description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Hide();
    }
    public void OnDisable()
    {
        Hide();
    }

    public void Hide()
    {
        TooltipManager.Instance.StopAllCoroutines();
        TooltipManager.Hide();
    }
}
