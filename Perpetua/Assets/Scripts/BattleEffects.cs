using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class BattleEffects : MonoBehaviour
{
    public GameObject AttackEffectsPresenter;
    public GameObject TextDisplay;
    public GameObject TextDisplayHUD;
    public GameObject ShieldImage;
    public GameObject SmokeEffect;
    private Animator _anim;
    //public List<Sprite> SpriteEffects;

    private void Start()
    {
        _anim = AttackEffectsPresenter.GetComponent<Animator>();
    }

    public void DisplayAttackEnemyEffect(Transform enemy)
    {
        SetBetween(Camera.main.transform, enemy, AttackEffectsPresenter.transform);
        AimTowards(AttackEffectsPresenter.transform, Camera.main.transform);
        _anim.SetTrigger("BasicAttack");
        //_anim.ResetTrigger("BasicAttack");
    }

    private void SetBetween(Transform first, Transform second, Transform obj)
    {
        obj.position = (first.position + second.position) / 2;
    }

    private void AimTowards(Transform aimable, Transform target)
    {
        aimable.rotation = Quaternion.LookRotation(target.position - aimable.position);
    }

    public void DisplayDamageValue(Transform target, float value)
    {
        string valueString = Math.Round(value, 1).ToString();
        DisplayFloatingText(target, valueString);
    }

    public void DisplayFloatingText(Transform target, string text)
    {
        GameObject damageValue = Instantiate(TextDisplay);
        SetBetween(Camera.main.transform, target, damageValue.transform);
        Transform MovementTransform = damageValue.transform.GetChild(0);
        MovementTransform.GetChild(0).GetComponent<TextMeshPro>().text = text;
        MovementTransform.GetChild(1).GetComponent<TextMeshPro>().text = text;
    }

    public void DisplayDamageValueHUD(Transform target, float value)
    {
        string valueString = Math.Round(value, 1).ToString();
        DisplayFloatingTextHUD(target, valueString);
    }

    public void DisplayFloatingTextHUD(Transform target, string text)
    {
        GameObject damageValue = Instantiate(TextDisplayHUD, BattleManager.Instance.BattleCanvas.transform);
        damageValue.transform.position = target.position;
        damageValue.transform.GetComponentInChildren<TextMeshProUGUI>().text = text;
    }

    public void DisplayGuardEffect(Transform participant, bool targetOnHUD)
    {
        GameObject guardEffect = Instantiate(ShieldImage, BattleManager.Instance.BattleCanvas.transform);
        guardEffect.transform.position = (targetOnHUD) ? participant.position : Camera.main.WorldToScreenPoint(participant.position);
    }

    // HUD
    public void ShroudedEffect(Transform participant)
    {
        GameObject smokeEffect = Instantiate(SmokeEffect, BattleManager.Instance.BattleCanvas.transform);
        smokeEffect.transform.position = participant.position;
    }
}
