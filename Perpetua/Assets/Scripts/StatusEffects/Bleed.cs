using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "StatusEffect/Types/Bleed")]
public class Bleed : StatusEffect
{
    private int turnsLeft = 10000;
    public float damagePerTurn = 0.05f;
    public void SetTurnsLeft(int value)
    {
        turnsLeft = value;
    }
    public override int GetTurnsLeft()
    {
        return turnsLeft;
    }

    public override void InflictActiveStatusEffect(BattleParticipant participant)
    {
        StatsData stats = participant.participant.stats;
        int damage = Mathf.RoundToInt(stats.MaxHealthPoints * damagePerTurn);
        stats.HealthPoints = Mathf.Max(0, stats.HealthPoints - damage);
        turnsLeft -= 1;
        BattleManager.Instance.StartCoroutine(DisplayStatusEffect(damage, participant));
    }

    private IEnumerator DisplayStatusEffect(int damage, BattleParticipant battleParticipant)
    {
        BattleManager battleManager = BattleManager.Instance;
        BattleCanvas battleCanvas = battleManager.BattleCanvas;
        Transform recipientTransform = battleParticipant.transform;

        if (battleParticipant.IsPartyMember)
        {
            battleCanvas.battleEffects.DisplayDamageValueHUD(recipientTransform, (float) damage);
            battleManager.StartCoroutine(AttackAction.ShowNegativeStatusEffectColor(battleParticipant, 0.5f));
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            Color defaultColor = recipientTransform.GetComponent<SpriteRenderer>().color;
            recipientTransform.GetComponent<SpriteRenderer>().color = Color.red;
            battleCanvas.RefreshEnemyHealthBars();
            battleCanvas.SetEnemyHealthBars(true);
            battleCanvas.battleEffects.DisplayDamageValue(recipientTransform, (float)damage);
            yield return new WaitForSeconds(0.5f);
            recipientTransform.GetComponent<SpriteRenderer>().color = defaultColor;
            battleCanvas.SetEnemyHealthBars(false);
        }
    }

    private void Init(int _turnsLeft, float _damagePerTurn, Sprite _image, string _tooltip)
    {
        turnsLeft = _turnsLeft;
        damagePerTurn = _damagePerTurn;
        image = _image;
        tooltip = _tooltip;
    }

    public override StatusEffect Clone()
    {
        Bleed bleed = ScriptableObject.CreateInstance<Bleed>();
        bleed.Init(turnsLeft, damagePerTurn, image, tooltip);
        return bleed;
    }

}