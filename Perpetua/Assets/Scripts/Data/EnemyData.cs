using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Enemies/Enemy")]
public class EnemyData : ScriptableObject
{
    public Sprite image;
    public new string name;
    public string description;
    public StatsData stats;
    public List<ItemData> loot;
    public Sprite gonerSprite;

    private void Init(Sprite _image, string _name, string _description, StatsData _stats, List<ItemData> _loot)
    {
        image = _image;
        name = _name;
        description = _description;
        stats = _stats;
        loot = _loot;
    }

    public static EnemyData Clone(EnemyData character)
    {
        var enemyData = ScriptableObject.CreateInstance<EnemyData>();

        enemyData.Init(character.image, character.name, character.description, character.stats, character.loot);
        return enemyData;
    }
}
