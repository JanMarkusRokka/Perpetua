using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyMembersPresenter : MonoBehaviour
{
    private PartyData party;
    public Button PartyMemberPresenter;

    private void OnEnable()
    {
        party = PartyManager.Instance.party;
        Debug.Log(PartyManager.Instance);
        RefreshMembers();
    }

    private void RefreshMembers()
    {
        int children = transform.childCount;

        for (int i = children - 1; i >= 0; i--)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }
        foreach (PartyCharacterData member in party.PartyMembers)
        {
            Button pres = Instantiate(PartyMemberPresenter, transform);
            pres.GetComponentInChildren<Image>().sprite = member.image;
            pres.GetComponentInChildren<TextMeshProUGUI>().text = member.name;
        }
    }
}
