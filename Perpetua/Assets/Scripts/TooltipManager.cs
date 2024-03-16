using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    void Awake()
    {
        if (FindObjectsOfType(typeof(TooltipManager)).Count() > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetPresenter(TooltipPresenter);
        }

    }

    private void SetPresenter(GameObject presenter)
    {
        TooltipPresenter = presenter;
        headerText = TooltipPresenter.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        descriptionText = TooltipPresenter.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        layoutElem = TooltipPresenter.transform.GetComponent<LayoutElement>();
    }

    public static void Show(string header, string description)
    {
        Instance.headerText.text = header;
        Instance.descriptionText.text = description;
        Instance.layoutElem.enabled = (Instance.headerText.text.Length > Instance.characterWrap || Instance.descriptionText.text.Length > Instance.characterWrap);
        Instance.StartCoroutine(Instance.SlowReveal());
    }

    private IEnumerator SlowReveal()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        Image panel = TooltipPresenter.GetComponent<Image>();

        Color color = headerText.color;
        color.a = 0;
        headerText.color = color;
        descriptionText.color = color;
        panel.color = color;

        TooltipPresenter.SetActive(true);

        for (float i = 0; i < 1; i+=0.1f)
        {
            color.a = i;
            headerText.color = color;
            descriptionText.color = color;
            panel.color = color;
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }

    public static void Hide()
    {
        Instance.headerText.text = "";
        Instance.descriptionText.text = "";
        Instance.TooltipPresenter.SetActive(false);
    }
}
