using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Objectives/Sub Objectives Objective")]

public class SubObjectivesObjective : Objective
{
    public List<Objective> objectives;
    public override bool IsCompleted()
    {
        foreach (Objective objective in objectives)
        {
            if (!objective.IsCompleted()) return false;
        }
        return true;
    }

    public override Objective Clone()
    {
        SubObjectivesObjective subObjObj = ScriptableObject.CreateInstance<SubObjectivesObjective>();
        subObjObj.name = name;
        subObjObj.id = id;
        subObjObj.description = description;
        subObjObj.objectives = new();
        subObjObj.NextObjective = NextObjective;
        subObjObj.isVisible = isVisible;
        foreach(Objective objective in objectives)
        {
            subObjObj.objectives.Add(objective.Clone());
        }
        return subObjObj;
    }

    public override void AdvanceSpecific()
    {
        
    }
}
