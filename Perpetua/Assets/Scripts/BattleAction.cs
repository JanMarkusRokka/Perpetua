using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BattleAction
{
    public void CommitAction();
}

public class Attack : BattleAction
{
    public BattleParticipant attacker;
    public BattleParticipant recipient;
    public void CommitAction()
    {
        StatsData attackerStats = attacker.GetStatsData();
        StatsData recipientStats = recipient.GetStatsData();
        EquipmentData attackerEquipment = attacker.GetEquipmentData();
        EquipmentData recipientEquipment = recipient.GetEquipmentData();
        BattleManager battleManager = BattleManager.Instance;
        BattleCanvas battleCanvas = battleManager.BattleCanvas;

        float landChance = attackerStats.Accuracy - attackerStats.Accuracy * recipientStats.Dodge;

        if (Random.Range(0, 100) > landChance)
        {
            Debug.Log(attacker.participant.name + " Miss");
            battleManager.StartCoroutine(animateMiss(battleManager, battleCanvas));
            return;
        }
        Debug.Log(attacker.participant.name + "Hit");

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
        if (attacker.IsPartyMember)
        {
            BattleEffects battleEffects = battleCanvas.battleEffects;
            battleCanvas.SetEnemyHealthBars(true);
            int orderId = battleManager.agilityOrder.IndexOf(attacker);
            battleCanvas.SetPartyMemberColor(orderId, Color.blue);
            Transform recipientTransform = battleManager.Enemies.Find(battleManager.agilityOrder.IndexOf(recipient).ToString());
            battleEffects.DisplayAttackEnemyEffect(recipientTransform);
            yield return new WaitForSeconds(0.5f);
            recipient.GetStatsData().HealthPoints = Mathf.Max(0f, recipient.GetStatsData().HealthPoints - totalDamage);
            Color defaultColor = recipientTransform.GetComponent<SpriteRenderer>().color;
            recipientTransform.GetComponent<SpriteRenderer>().color = Color.red;
            battleCanvas.RefreshEnemyHealthBars();
            // Add damage number

            yield return new WaitForSeconds(0.5f);
            recipientTransform.GetComponent<SpriteRenderer>().color = defaultColor;
            battleCanvas.ResetPartyMemberColor(orderId);
            battleCanvas.SetEnemyHealthBars(false);
        }
        else
        {
            // Enemy attacks player
        }

        battleManager.CommitNextAction();
    }

    IEnumerator animateMiss(BattleManager battleManager, BattleCanvas battleCanvas)
    {
        // Add text "miss" on top of character
        yield return new WaitForSeconds(1f);
        battleManager.CommitNextAction();

    }

    public static Attack New(BattleParticipant _attacker, BattleParticipant _recipient)
    {
        return new Attack() {
        attacker = _attacker,
        recipient = _recipient
        };
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
}