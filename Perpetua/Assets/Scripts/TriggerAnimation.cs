using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    private Animator _anim;
    public string TriggerName;
    void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void TriggerAnim()
    {
        TriggerAnimationByName(TriggerName);
    }

    public void TriggerAnimationByName(string triggerName)
    {
        _anim.SetTrigger(triggerName);
    }
}
