using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject DialoguePresenter;
    public TextMeshProUGUI NamePresenter;
    public TextMeshProUGUI MessagePresenter;
    public Image LeftSprite;
    public Image RightSprite;

    private Image _bg;

    private Queue<Dialogue> dialogues;
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
        }
        else
        {
            DialoguePresenter.SetActive(false);
        }
    }
}
