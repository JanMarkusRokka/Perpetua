using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class Objective : ScriptableObject
{
    public new string name;
    public string description;
    public int id;
    public abstract bool IsCompleted();
    public abstract Objective Clone();
    public abstract void Advance();
}
