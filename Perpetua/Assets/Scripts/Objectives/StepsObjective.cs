using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Objectives/Steps Objective")]

public class StepsObjective : Objective
{
    public int CompletedSteps = 0;
    public int GoalSteps;
    public override bool IsCompleted()
    {
        if (CompletedSteps == GoalSteps)
            return true;
        return false;
    }

    public override Objective Clone()
    {
        StepsObjective stepsObjective = ScriptableObject.CreateInstance<StepsObjective>();
        stepsObjective.CompletedSteps = CompletedSteps;
        stepsObjective.name = name;
        stepsObjective.id = id;
        stepsObjective.GoalSteps = GoalSteps;
        stepsObjective.description = description;
        return stepsObjective;
    }

    public override void Advance()
    {
        CompletedSteps += 1;
    }
}
