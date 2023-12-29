using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public static PartyManager Instance;
    public PartyData party;

    public void Awake()
    {
        Instance = this;
    }
}
