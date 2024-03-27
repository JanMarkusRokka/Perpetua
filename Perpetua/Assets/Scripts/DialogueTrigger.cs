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
                foreach(TriggerAction action in StartActions)
                {
                    Debug.Log(action.name);
                    action.DoAction();
                }
            }
        }
        DialogueManager.Instance.StartDialogue(dialogues);
    }
}
