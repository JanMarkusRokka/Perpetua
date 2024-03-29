using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    // Dialogue Manager
    public static DialogueManager Instance;

    public GameObject DialoguePresenter;
    public TextMeshProUGUI NamePresenter;
    public TextMeshProUGUI MessagePresenter;
    public Image LeftSprite;
    public Image RightSprite;

    private Image _bg;

    private Queue<Dialogue> dialogues;
    private Dialogue lastDialogue;

    public void Awake()
    {
        Events.OnDialogueEnded += OnDialogueEnded;
        lastDialogue = null;
    }

    public void OnDestroy()
    {
        Events.OnDialogueEnded -= OnDialogueEnded;
    }

    public void OnDialogueEnded(int value)
    {
        dialogues = new Queue<Dialogue>();
        DialoguePresenter.SetActive(false);
    }

    void Start()
    {
        Instance = this;
        dialogues = new Queue<Dialogue>();
        _bg = GetComponent<Image>();
    }

    public void StartDialogue(Dialogue[] dialogues1)
    {
        foreach (Dialogue dialogue in dialogues1)
        {
            dialogues.Enqueue(dialogue);
        }
        DialoguePresenter.SetActive(true);
        ShowNextDialogue();
    }
    
    public void ShowNextDialogue()
    {
        if (lastDialogue != null)
        {
            if (lastDialogue.triggerAction != null)
            {
                if (lastDialogue.triggerAction.GetType() == typeof(TriggerBattle))
                {
                    TriggerBattle triggerBattleAction = (TriggerBattle)lastDialogue.triggerAction;
                    triggerBattleAction.overworldEnemy = lastDialogue.triggerer.GetComponent<OverworldEnemy>();
                }
                lastDialogue.triggerAction.DoAction();
            }
        }

        if (dialogues.Count > 0)
        {
            Dialogue dialogue = dialogues.Dequeue();
            NamePresenter.text = dialogue.name;
            MessagePresenter.text = dialogue.message;
            if (dialogue.spriteLeft)
            {
                LeftSprite.enabled = true;
                LeftSprite.sprite = dialogue.spriteLeft;
            }
            else
            {
                LeftSprite.enabled = false;
            }
            if (dialogue.spriteRight)
            {
                RightSprite.enabled = true;
                RightSprite.sprite = dialogue.spriteRight;
            }
            else
            {
                RightSprite.enabled = false;
            }
            lastDialogue = dialogue;
        }
        else
        {
            lastDialogue = null;
            Events.EndDialogue(0);
        }
    }
}
