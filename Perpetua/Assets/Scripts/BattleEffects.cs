using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEffects : MonoBehaviour
{
    public GameObject AttackEffectsPresenter;
    private Animator _anim;
    //public List<Sprite> SpriteEffects;

    private void Start()
    {
        _anim = AttackEffectsPresenter.GetComponent<Animator>();
    }

    public void DisplayAttackEnemyEffect(Transform enemy)
    {
        SetEffectsBetweenCameraAndEnemy(enemy);
        _anim.SetTrigger("BasicAttack");
        //_anim.ResetTrigger("BasicAttack");
        // Make enemy different colour for a bit:

    }

    private void SetEffectsBetweenCameraAndEnemy(Transform enemy)
    {
        AttackEffectsPresenter.transform.position = (Camera.main.transform.position + enemy.position) / 2;
        AttackEffectsPresenter.transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - AttackEffectsPresenter.transform.position);
    }

    public void DisplayDamageValue(float value)
    {
        
    }
}
