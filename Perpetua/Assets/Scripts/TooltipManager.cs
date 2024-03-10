using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//Somewhat inspired by https://www.youtube.com/watch?v=HXFoUGw7eKk
public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;
    private TextMeshProUGUI headerText;
    private TextMeshProUGUI descriptionText;
    public GameObject TooltipPresenter;
    private LayoutElement layoutElem;
    public int characterWrap;
    void Start()
    {
        Instance = this;
        headerText = TooltipPresenter.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        descriptionText = TooltipPresenter.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        layoutElem = TooltipPresenter.transform.GetComponent<LayoutElement>();
    }

    public static void Show(string header, string description)
    {
        Instance.headerText.text = header;
        Instance.descriptionText.text = description;
        Instance.TooltipPresenter.SetActive(true);
        Instance.layoutElem.enabled = (Instance.headerText.text.Length > Instance.characterWrap || Instance.descriptionText.text.Length > Instance.characterWrap);
    }

    public static void Hide()
    {
        Instance.headerText.text = "";
        Instance.descriptionText.text = "";
        Instance.TooltipPresenter.SetActive(false);
    }
}
