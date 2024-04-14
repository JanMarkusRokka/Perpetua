using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Dialogue {
    public string name;
    public Sprite spriteLeft;
    public Sprite spriteRight;
    [TextArea(3, 100)]
    public string message;
    public TriggerAction triggerAction;
    [System.NonSerialized]
    public DialogueTrigger triggerer;
}