using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SavePoint : Interactable
{
    public bool ActivatesOnce = false;
    public bool HasBubble = true;
    public bool RestoresWillPower;
    public GameObject SpeechBubble;
    public GameObject RestEffect;
    public AudioClipGroup SoundEffect;

    private void Start()
    {
        if (PartyManager.Instance.party.lastSave == null) Events.Save(transform.position);
    }

    public override void Interact()
    {
        if (SoundEffect) SoundEffect.Play();
        isActive = false;
        foreach (PartyCharacterData member in PartyManager.Instance.party.PartyMembers)
        {
            member.stats.WillPower = member.stats.MaxWillPower;
        }
        Instantiate(RestEffect, transform);
        Events.Save(transform.position);
    }

    public override void OnEnterRange()
    {
        if (HasBubble)
            SpeechBubble.SetActive(true);
    }

    public override void OnExitRange()
    {
        if (HasBubble)
            SpeechBubble.SetActive(false);
    }
}
