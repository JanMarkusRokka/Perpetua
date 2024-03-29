using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Objectives/Boolean Objective")]

public class BooleanObjective : Objective
{
    public bool Completed = false;
    public override bool IsCompleted()
    {
        if (Completed)
            return true;
        return false;
    }

    public override Objective Clone()
    {
        BooleanObjective booleanObjective = ScriptableObject.CreateInstance<BooleanObjective>();
        booleanObjective.Completed = Completed;
        booleanObjective.name = name;
        booleanObjective.id = id;
        booleanObjective.description = description;
        return booleanObjective;
    }

    public override void Advance()
    {
        Completed = true;
    }
}
