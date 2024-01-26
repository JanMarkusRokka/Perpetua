using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEnemyAnimator : MonoBehaviour
{
    private Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void PlayAttackAnimation()
    {
        _anim.SetTrigger("Attack");
    }
    public void PlayDeathAnimation()
    {
        _anim.SetTrigger("Death");
    }
}
