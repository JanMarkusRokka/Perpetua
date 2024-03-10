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
        StartCoroutine(WaitAndShow());
    }

    IEnumerator WaitAndShow()
    {
        yield return new WaitForSeconds(0.5f);
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
        this.StopAllCoroutines();
        TooltipManager.Hide();
    }
}
