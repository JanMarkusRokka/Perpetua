using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillsPresenter : MonoBehaviour
{
    BattleManager battleManager;
    BattleCanvas battleCanvas;
    public GameObject SkillPresenter;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            battleCanvas.LeftTC.SetTab(battleCanvas.ActionsPresenter);
            battleCanvas.ActionsPresenter.transform.GetChild(0).GetComponent<Button>().Select();
        }
    }

    private void OnEnable()
    {
        battleManager = BattleManager.Instance;
        battleCanvas = battleManager.BattleCanvas;
        //battleCanvas.ActionOptionPresenterPrefab;
        RefreshSkills();
    }
    private void RefreshSkills()
    {
        battleCanvas.ClearTab(gameObject);
        GameObject backButton = Instantiate(battleCanvas.ActionOptionPresenterPrefab, transform);
        backButton.GetComponentInChildren<TextMeshProUGUI>().text = "Back";
        backButton.GetComponent<Button>().onClick.AddListener( delegate { battleCanvas.LeftTC.SetTab(battleCanvas.ActionsPresenter); battleCanvas.ActionsPresenter.transform.GetChild(0).GetComponent<Button>().Select(); } );
        backButton.GetComponent<Button>().Select();
        BattleParticipant participant = battleManager.GetCurrentTurnTaker();
        if (participant.IsPartyMember)
        {
            PartyCharacterData member = participant.GetPartyMember();
            foreach (BattleAction action in member.skills)
            {
                GameObject button = Instantiate(SkillPresenter, transform);
                button.GetComponentInChildren<TextMeshProUGUI>().text = action.GetName();
                TooltipTrigger tooltipTrigger = button.GetComponent<TooltipTrigger>();
                tooltipTrigger.header = action.GetName();
                tooltipTrigger.description = action.tooltip;
                if (action.SelectEnemy())
                {
                    button.GetComponent<Button>().onClick.AddListener(delegate { battleCanvas.StartSelectEnemy(action); });
                }
                else
                {
                    button.GetComponent<Button>().onClick.AddListener(delegate { battleManager.AddActionToQueue(action.CreateFromUI(new List<BattleParticipant> {
                        participant 
                    }
                    )); });
                }
            }
        }
    }
}
