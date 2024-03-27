using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Characters/Party")]
public class PartyData : ScriptableObject
{
    public List<PartyCharacterData> PartyMembers;
    public List<Objective> objectives;
}
