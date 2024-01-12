using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chest : Interactable
{
    public ChestData chestData;
    private SpriteRenderer _sr;
    private Animator _anim;

    private void Start()
    {
        chestData = ChestData.Clone(chestData);
        _anim = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        if (chestData.items.Count > 0)
        {
            _sr.sprite = chestData.ClosedSprite;
            isActive = true;
        }
        else
        {
            _sr.sprite = chestData.OpenSprite;
            isActive = false;
        }
    }

    public override void Interact()
    {
        foreach (ItemData item in chestData.items)
        {
            Events.ReceiveItem(item);
        }
        _sr.sprite = chestData.OpenSprite;
        chestData.items = new List<ItemData>();
        isActive = false;
        OnExitRange();
    }
    public override void OnEnterRange()
    {
        if (isActive)
        {
            _anim.enabled = true;
            _anim.Play("ChestBlinking", 0, 0f);
        }
    }

    public override void OnExitRange()
    {
        if (_anim.enabled)
        {
            _sr.color = Color.white;
            _anim.enabled = false;
        }
    }
}