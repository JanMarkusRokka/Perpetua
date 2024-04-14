using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "StatusEffect/Types/Poison")]
public class Poison : StatusEffect
{
    private int turnsLeft = 3;
    public float damagePerTurn = 0.05f;
    public void SetTurnsLeft(int value)
    {
        turnsLeft = value;
    }
    public override int GetTurnsLeft()
    {
        return turnsLeft;
    }

    public override IEnumerator InflictActiveStatusEffect(BattleParticipant participant)
    {
        StatsData stats = participant.participant.stats;
        int damage = Mathf.RoundToInt(stats.MaxHealthPoints * damagePerTurn);
        stats.HealthPoints = Mathf.Max(0, stats.HealthPoints - damage);
        turnsLeft -= 1;
        yield return BattleManager.Instance.StartCoroutine(DisplayStatusEffect(damage, participant));
        yield break;
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
            recipientTransform.GetComponent<SpriteRenderer>().color = Color.green;
            battleCanvas.RefreshEnemyHealthBars();
            battleCanvas.SetEnemyHealthBars(true);
            battleCanvas.battleEffects.DisplayDamageValue(recipientTransform, (float)damage);
            yield return new WaitForSeconds(0.5f);
            recipientTransform.GetComponent<SpriteRenderer>().color = defaultColor;
            battleCanvas.SetEnemyHealthBars(false);
        }
    }

    public override StatusEffect Clone()
    {
        Poison poison = ScriptableObject.CreateInstance<Poison>();
        poison.turnsLeft = turnsLeft;
        poison.damagePerTurn = damagePerTurn;
        poison.CopyBase(this);
        return poison;
    }

}