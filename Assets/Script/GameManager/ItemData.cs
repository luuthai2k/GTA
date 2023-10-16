using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataItem", menuName = "DataItem/Item")]
public class ItemData : ScriptableObject
{
    public int HpKit;
    public int Armor;
    public int Spin;
    public int money;
}

