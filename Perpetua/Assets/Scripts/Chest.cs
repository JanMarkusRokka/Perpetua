using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public ChestData chestData;
    private SpriteRenderer _sr;
    private Animator _anim;
    public bool isOpened;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        if (chestData.items.Count > 0)
        {
            _sr.sprite = chestData.ClosedSprite;
            isOpened = false;
        }
        else
        {
            _sr.sprite = chestData.OpenSprite;
            isOpened = true;
        }
    }

    public void GetItems()
    {
        foreach (ItemData item in chestData.items)
        {
            Events.ReceiveItem(item);
        }
        _sr.sprite = chestData.OpenSprite;
        chestData.items = new List<ItemData>();
        isOpened = true;
    }
    public void StartIndicating()
    {
        if (!isOpened)
        {
            _anim.enabled = true;
            _anim.Play("ChestBlinking", 0, 0f);
        }
    }

    public void StopIndicating()
    {
        if (_anim.enabled)
        {
            _sr.color = Color.white;
            _anim.enabled = false;
        }
    }
}