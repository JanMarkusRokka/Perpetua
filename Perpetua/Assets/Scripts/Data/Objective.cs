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
    public bool isVisible;
    [SerializeField]
    public Objective NextObjective;
    public abstract bool IsCompleted();
    public Objective GetNextObjective()
    {
        return NextObjective.Clone();
    }
    public abstract Objective Clone();
    public void Advance()
    {
        AdvanceSpecific();
        Events.UpdateObjectives(0);
    }
    public abstract void AdvanceSpecific();
    public void MakeVisible()
    {
        isVisible = true;
        Events.UpdateObjectives(0);
    }
}
