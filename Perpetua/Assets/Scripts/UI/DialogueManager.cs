using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using Image = UnityEngine.UI.Image;

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
    private bool isTextBeingRevealed;
    private string textBeingRevealed;

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
        SoundManager.Instance.PlayUISelect2Sound();
        foreach (Dialogue dialogue in dialogues1)
        {
            dialogues.Enqueue(dialogue);
        }
        DialoguePresenter.SetActive(true);
        ShowNextDialogue();
    }
    private IEnumerator RevealTextDialogue(string message)
    {
        isTextBeingRevealed = true;
        MessagePresenter.text = "";
        foreach (char character in message)
        {
            yield return new WaitForSecondsRealtime(0.05f);
            MessagePresenter.text += character;
        }
        isTextBeingRevealed = false;
    }
    public void ShowNextDialogue()
    {
        if (isTextBeingRevealed)
        {
            StopAllCoroutines();
            MessagePresenter.text = textBeingRevealed;
            isTextBeingRevealed = false;
            return;
        }
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
            textBeingRevealed = dialogue.message;
            StartCoroutine(RevealTextDialogue(dialogue.message));
            
            if (dialogue.spriteLeft)
            {
                LeftSprite.enabled = true;
                LeftSprite.sprite = dialogue.spriteLeft;
                LeftSprite.GetComponent<RectTransform>().sizeDelta = dialogue.spriteLeft.textureRect.size * 4;
            }
            else
            {
                LeftSprite.enabled = false;
            }
            if (dialogue.spriteRight)
            {
                RightSprite.enabled = true;
                RightSprite.sprite = dialogue.spriteRight;
                RightSprite.GetComponent<RectTransform>().sizeDelta = dialogue.spriteRight.textureRect.size * 4;
            }
            else
            {
                RightSprite.enabled = false;
            }
            lastDialogue = dialogue;
        }
        else
        {
            SoundManager.Instance.PlayUIDeselect2Sound();
            lastDialogue = null;
            Events.EndDialogue(0);
        }
    }
}
