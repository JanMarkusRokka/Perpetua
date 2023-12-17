using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePresenter : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) DialogueManager.Instance.ShowNextDialogue();
    }
}
