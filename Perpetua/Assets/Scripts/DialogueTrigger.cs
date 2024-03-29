using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public List<TriggerAction> StartActions;
    public Dialogue[] dialogues;

    public void TriggerDialogues()
    {
        if (StartActions != null)
        {
            if (StartActions.Count > 0)
            {
                foreach (TriggerAction action in StartActions)
                {
                    if (action.GetType() == typeof(TriggerBattle))
                    {
                        TriggerBattle triggerBattleAction = ScriptableObject.CreateInstance<TriggerBattle>();
                        triggerBattleAction.overworldEnemy = GetComponent<OverworldEnemy>();
                    }
                    action.DoAction();
                }
            }
        }
        foreach (Dialogue dialogue in dialogues)
        {
            dialogue.triggerer = this;
        }
        DialogueManager.Instance.StartDialogue(dialogues);
    }
}
