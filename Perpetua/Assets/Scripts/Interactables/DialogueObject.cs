using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueTrigger))]
[RequireComponent(typeof(Collider))]
public class DialogueObject : Interactable
{
    public bool ActivatesOnce = false;
    public bool HasBubble = false;
    public GameObject SpeechBubble;
    private DialogueTrigger dialogueTrigger;
    public void Awake()
    {
        Events.OnDialogueEnded += OnDialogueEnded;
    }

    public void OnDestroy()
    {
        Events.OnDialogueEnded -= OnDialogueEnded;
    }

    public void OnDialogueEnded(int value)
    {
        if (!ActivatesOnce)
        {
            isActive = true;
        }
    }

    private void Start()
    {
        dialogueTrigger = GetComponent<DialogueTrigger>();
    }
    public override void Interact()
    {
        dialogueTrigger.TriggerDialogues();
        isActive = false;
    }

    public override void OnEnterRange()
    {
        if (HasBubble)
            SpeechBubble.SetActive(true);
    }

    public override void OnExitRange()
    {
        Events.EndDialogue(0);
        if (HasBubble)
            SpeechBubble.SetActive(false);
    }
}
