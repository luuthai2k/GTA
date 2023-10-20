using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public static ItemManager ins;

    [SerializeField]
    private ItemData itemData;

    [SerializeField]
    private Text hpKitText, armorText, spinText;

    private void Awake()
    {
        ins = this;
    }
    public void Start()
    {
        //Init();
    }
    private void Init()
    {
        hpKitText.text = itemData.HpKit + "";
        armorText.text = itemData.Armor + "";
        spinText.text = itemData.Spin + "";
    }

    public void AddHPKit(int _index)
    {
        itemData.HpKit += _index;
    }

    public void AddArmor(int _index)
    {
        itemData.Armor += _index;
    }

    public void AddSpin(int _index)
    {
        itemData.Spin += _index;
    }

    public void AddMoney(int _index)
    {
        itemData.money += _index;
    }
}


public enum Gift
{
    hpKit,
    armor,
    spin,
    money,
    skin
}
