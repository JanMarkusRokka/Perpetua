using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BattleAction
{
    public BattleParticipant GetParticipant();
    public void CommitAction();
}

public class Attack : BattleAction
{
    public BattleParticipant participant;
    public BattleParticipant recipient;
    public void CommitAction()
    {
        StatsData attackerStats = participant.GetStatsData();
        StatsData recipientStats = recipient.GetStatsData();
        EquipmentData attackerEquipment = participant.GetEquipmentData();
        EquipmentData recipientEquipment = recipient.GetEquipmentData();
        BattleManager battleManager = BattleManager.Instance;
        BattleCanvas battleCanvas = battleManager.BattleCanvas;

        float landChance = attackerStats.Accuracy - attackerStats.Accuracy * recipientStats.Dodge;

        if (Random.Range(0, 100) > landChance)
        {
            Debug.Log(participant.participant.name + " Miss");
            battleManager.StartCoroutine(animateMiss(battleManager, battleCanvas));
            return;
        }
        Debug.Log(participant.participant.name + "Hit");

        float baseDamage = attackerStats.PhysicalDamage;
        float baseMagicDamage = attackerStats.MagicDamage;
        if (attackerEquipment)
        {
            if (attackerEquipment.weapon)
            {
                baseDamage += attackerEquipment.weapon.WeaponVariables.WeaponDamage;
                baseMagicDamage += attackerEquipment.weapon.WeaponVariables.WeaponMagicDamage;
            }
        }

        baseDamage += Random.Range(-0.15f, 0.15f) * baseDamage;
        baseMagicDamage += Random.Range(-0.15f, 0.15f) * baseMagicDamage;

        float criticalMultiplier = 1f;

        if (Random.Range(0, 100) < attackerStats.CriticalChance)
        {
            //Critical Strike
            criticalMultiplier = attackerStats.CriticalMultiplier;
        }

        float physicalDefense = recipientStats.PhysicalDefense;
        float magicDefense = recipientStats.MagicDefense;
        if (recipientEquipment)
        {
            if (recipientEquipment.armour)
            {
                physicalDefense += recipientEquipment.armour.ArmorVariables.ArmorDefense;
                magicDefense += recipientEquipment.armour.ArmorVariables.ArmorMagicDefense;
            }
        }

        float totalDamage = Mathf.Max(0f, baseDamage * criticalMultiplier - physicalDefense) + 
            Mathf.Max(0f, baseMagicDamage * criticalMultiplier - magicDefense);

        //Later add ailments here (if skill only, then not?)


        battleManager.StartCoroutine(animateAttack(battleManager, battleCanvas, totalDamage));
        
    }

    IEnumerator animateAttack(BattleManager battleManager, BattleCanvas battleCanvas, float totalDamage)
    {
        if (participant.IsPartyMember)
        {
            BattleEffects battleEffects = battleCanvas.battleEffects;
            battleCanvas.SetEnemyHealthBars(true);
            int orderId = battleManager.agilityOrder.IndexOf(participant);
            battleCanvas.SetPartyMemberColor(orderId, Color.blue);
            Transform recipientTransform = battleManager.Enemies.Find(battleManager.agilityOrder.IndexOf(recipient).ToString());
            battleEffects.DisplayAttackEnemyEffect(recipientTransform);

            yield return new WaitForSeconds(0.5f);
            recipient.GetStatsData().HealthPoints = Mathf.Max(0f, recipient.GetStatsData().HealthPoints - totalDamage);

            Color defaultColor = recipientTransform.GetComponent<SpriteRenderer>().color;
            recipientTransform.GetComponent<SpriteRenderer>().color = Color.red;
            battleCanvas.RefreshEnemyHealthBars();

            battleEffects.DisplayDamageValue(recipientTransform, totalDamage);

            yield return new WaitForSeconds(0.5f);
            recipientTransform.GetComponent<SpriteRenderer>().color = defaultColor;
            battleCanvas.ResetPartyMemberColor(orderId);
            battleCanvas.SetEnemyHealthBars(false);
        }
        else
        {
            // Enemy attacks player
            BattleEffects battleEffects = battleCanvas.battleEffects;
            Transform attackerTransform = battleManager.Enemies.Find(battleManager.agilityOrder.IndexOf(participant).ToString());
            attackerTransform.GetComponent<BattleEnemyAnimator>().PlayAttackAnimation();
            int orderId = battleManager.agilityOrder.IndexOf(recipient);
            battleCanvas.SetPartyMemberColor(orderId, Color.red);
            yield return new WaitForSeconds(0.75f);

            battleEffects.DisplayDamageValueHUD(battleCanvas.PartyPresenter.transform.Find(orderId.ToString()).transform, totalDamage);
            recipient.GetStatsData().HealthPoints = Mathf.Max(0f, recipient.GetStatsData().HealthPoints - totalDamage);
            battleCanvas.UpdatePartyTabStats();
            yield return new WaitForSeconds(0.5f);
            battleCanvas.ResetPartyMemberColor(orderId);

        }

        battleManager.CommitNextAction();
    }

    IEnumerator animateMiss(BattleManager battleManager, BattleCanvas battleCanvas)
    {
        if (participant.IsPartyMember)
        {
            BattleEffects battleEffects = battleCanvas.battleEffects;
            int orderId = battleManager.agilityOrder.IndexOf(participant);
            battleCanvas.SetPartyMemberColor(orderId, Color.blue);
            Transform recipientTransform = battleManager.Enemies.Find(battleManager.agilityOrder.IndexOf(recipient).ToString());
            yield return new WaitForSeconds(0.5f);
            battleEffects.DisplayFloatingText(recipientTransform, "Miss");
            yield return new WaitForSeconds(0.5f);
            battleCanvas.ResetPartyMemberColor(orderId);
        }

        battleManager.CommitNextAction();

    }

    public static Attack New(BattleParticipant _attacker, BattleParticipant _recipient)
    {
        return new Attack() {
        participant = _attacker,
        recipient = _recipient
        };
    }

    public BattleParticipant GetParticipant()
    {
        return participant;
    }
}

public class Guard : BattleAction
{
    public BattleParticipant participant;
    public void CommitAction()
    {
        Debug.Log("Guard action");
        BattleManager battleManager = BattleManager.Instance;
        BattleCanvas battleCanvas = battleManager.BattleCanvas;
        battleManager.CommitNextAction();
    }
    public static Guard New(BattleParticipant _participant)
    {
        return new Guard()
        {
            participant = _participant
        };
    }
    public BattleParticipant GetParticipant()
    {
        return participant;
    }
}