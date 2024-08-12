using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public string header;
    public string description;

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.Show(header, description);
    
    }

    public void OnSelect(BaseEventData eventData)
    {
        TooltipManager.KeysSelectShow(header, description);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Hide();
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
