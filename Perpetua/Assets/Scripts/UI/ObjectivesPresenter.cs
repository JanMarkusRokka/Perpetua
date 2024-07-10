using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectivesPresenter : MonoBehaviour
{
    [SerializeField]
    private GameObject ObjectivePresenter;
    [SerializeField]
    private GameObject Title;

    void Start()
    {
        UpdateObjectives(0);
    }

    private void Awake()
    {
        Events.OnUpdateObjectives += UpdateObjectives;
    }

    private void OnDestroy()
    {
        Events.OnUpdateObjectives -= UpdateObjectives;
    }

    private void UpdateObjectives(int num)
    {
        TabsController.ClearTab(gameObject);

        List<Objective> objectives = PartyManager.Instance.party.objectives;

        bool setTitle = false;

        for (int i = objectives.Count-1; i > -1; i--)
        {
            Objective objective = objectives[i];
            if (objective.IsCompleted())
            {
                PartyManager.Instance.party.completedObjectives.Add(objective);
                objectives.RemoveAt(i);
                Objective nextObj = objective.GetNextObjective();
                if (nextObj) 
                {
                    if (!PartyManager.Instance.party.isObjectiveAlreadyAdded(nextObj))
                    {
                        PartyManager.Instance.party.objectives.Add(nextObj);
                        setTitle = true;
                        DisplayObjective(nextObj);
                    }
                }
            }
            else if (objective.isVisible)
            {
                setTitle = true;
                DisplayObjective(objective);
            }
        }
        Title.SetActive(setTitle);
    }

    private void DisplayObjective(Objective objective)
    {
        GameObject ObjectivePres = Instantiate(ObjectivePresenter, transform);
        ObjectivePres.GetComponentInChildren<TextMeshProUGUI>().text = objective.description;

        if (objective.GetType() == typeof(SubObjectivesObjective))
        {
            DisplaySubObjectivesObjective((SubObjectivesObjective)objective);
        }
    }

    private void DisplaySubObjectivesObjective(SubObjectivesObjective objective)
    {
        foreach (Objective subObj in objective.objectives)
        {
            GameObject ObjectivePres = Instantiate(ObjectivePresenter, transform);
            TextMeshProUGUI text = ObjectivePres.GetComponentInChildren<TextMeshProUGUI>();

            text.fontSize -= 3;
            ObjectivePres.transform.GetChild(1).GetComponent<RectTransform>().position += new Vector3(25, 0, 0);
            if (subObj.IsCompleted())
            {
                text.text = "   <s>" + subObj.description + "</s>";
                text.color /= 2;
                ObjectivePres.transform.GetChild(1).GetComponent<Image>().color /= 2;
            }
            else text.text = "   " + subObj.description;
        }
    }
}
