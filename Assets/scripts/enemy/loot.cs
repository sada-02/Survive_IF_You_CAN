using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class loot : ScriptableObject
{
    public Sprite lootSprite;
    public int dropChance;

    public inventory_item item;

    public loot(int val, int chance)
    {
        dropChance = chance;
    }

}
