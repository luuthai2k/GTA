using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyGift : MonoBehaviour
{
    [SerializeField]
    private List<GiftBonus> gifts;

    public void Claim()
    {
        for (int i = 0; i < gifts.Count; i++)
        {
            if (gifts[i].gift == Gift.hpKit)
            {
                ItemManager.ins.AddHPKit(gifts[i].indexGift);
            }
            if (gifts[i].gift == Gift.armor)
            {
                ItemManager.ins.AddArmor(gifts[i].indexGift);
            }
            if (gifts[i].gift == Gift.spin)
            {
                ItemManager.ins.AddSpin(gifts[i].indexGift);
            }
            if (gifts[i].gift == Gift.money)
            {
                ItemManager.ins.AddMoney(gifts[i].indexGift);
            }

        }
    }
}
