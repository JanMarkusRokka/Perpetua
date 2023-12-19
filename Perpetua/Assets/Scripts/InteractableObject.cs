using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool isActive = true;
    public virtual void Interact() { }
    public virtual void OnEnterRange() { }
    public virtual void OnExitRange() { }

}
