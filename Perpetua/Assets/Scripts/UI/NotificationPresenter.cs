using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationPresenter : MonoBehaviour
{
    // Notification Presenter
    public static NotificationPresenter Instance;

    public GameObject NotificationPrefab;

    public void Awake()
    {
        Events.OnItemReceived += OnItemReceived;
    }

    public void OnDestroy()
    {
        Events.OnItemReceived -= OnItemReceived;
    }

    public void OnItemReceived(ItemData item)
    {
        ShowNotification("Received item: " + item.name, item.type.image);
    }

    private void Start()
    {
        Instance = this;
    }

    public void ShowNotification(string text, Sprite image)
    {
        GameObject notification = Instantiate(NotificationPrefab, transform);
        notification.transform.Find("Preview").GetComponent<Image>().sprite = image;
        notification.GetComponentInChildren<TextMeshProUGUI>().text = text;
    }
}
