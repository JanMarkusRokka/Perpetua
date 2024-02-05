using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour, ISelectHandler
{
    private Button button;
    private void Awake()
    {
        if (GetComponent<Button>())
        GetComponent<Button>().onClick.AddListener(OnSubmit);
    }
    public void OnSelect(BaseEventData eventData)
    {
        SoundManager.Instance.PlayUINavigationSound();
    }

    public void OnSubmit()
    {
        SoundManager.Instance.PlaySubmitSound();
    }
}
