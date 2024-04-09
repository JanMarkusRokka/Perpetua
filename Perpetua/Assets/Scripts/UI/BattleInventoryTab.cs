using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleInventoryTab : MonoBehaviour
{
    public GameObject ItemPresenterPrefab;
    private BattleCanvas battleCanvas;
    private BattleManager battleManager;
    private void Awake()
    {
        battleManager = BattleManager.Instance;
        battleCanvas = battleManager.BattleCanvas;
    }
    private void OnEnable()
    {
        UpdateItems();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            battleCanvas.LeftTC.SetTab(battleCanvas.ActionsPresenter);
            battleCanvas.ActionsPresenter.transform.GetChild(0).GetComponent<Button>().Select();
        }
    }

    private void UpdateItems()
    {
        battleCanvas.ClearTab(gameObject);
        GameObject backButton = Instantiate(battleCanvas.ActionOptionPresenterPrefab, transform);
        backButton.GetComponentInChildren<TextMeshProUGUI>().text = "Back";
        backButton.GetComponent<Button>().onClick.AddListener(delegate { battleCanvas.LeftTC.SetTab(battleCanvas.ActionsPresenter); battleCanvas.ActionsPresenter.transform.GetChild(0).GetComponent<Button>().Select(); });
        backButton.GetComponent<Button>().Select();
        List<ItemData> consumables = InventoryManager.Instance.inventory.items.FindAll(item => item.type.name.Equals("Consumable"));
        foreach(ItemData item in consumables)
        {
            GameObject itemPresenter = Instantiate(ItemPresenterPrefab, transform);
            itemPresenter.transform.Find("Image").GetComponentInChildren<Image>().sprite = item.image;
            itemPresenter.GetComponentInChildren<TextMeshProUGUI>().text = item.name;
            TooltipTrigger tooltip = itemPresenter.GetComponent<TooltipTrigger>();
            tooltip.header = item.name;
            tooltip.description = item.GetDescription();
            itemPresenter.GetComponent<Button>().onClick.AddListener( delegate { AddConsumeToQueue(item); } );
        }
        
    }

    private void AddConsumeToQueue(ItemData item)
    {
        Consume consume = ScriptableObject.CreateInstance<Consume>();
        consume.participant = BattleManager.Instance.GetCurrentTurnTaker();
        consume.item = item;
        InventoryManager.Instance.inventory.items.Remove(item);
        BattleManager.Instance.AddActionToQueue(consume);
    }
}
