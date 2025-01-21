using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class inventory_item : ScriptableObject
{   
    // define the item properties
    public Sprite Icon;
    public string Name;
    public int ID;
    [TextArea(5, 10)]
    public string itemType;
    public int maxValue;
}
