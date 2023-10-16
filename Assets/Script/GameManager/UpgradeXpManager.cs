using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeXpManager : MonoBehaviour
{
    public static UpgradeXpManager ins;

    [SerializeField]
    private int xp,maxXp;
    [SerializeField]
    private int lv,maxLv;

    [SerializeField]
    private DataLvCharacter dataUpgrades;

    [SerializeField]
    private CharacterParameter characterParameter;

    public void Awake()
    {
        ins = this;
    }

    public void Start()
    {
        xp = PlayerPrefs.GetInt("Xp");
        lv = PlayerPrefs.GetInt("Lv");
        maxXp = dataUpgrades.maxXp;
        maxLv = dataUpgrades.maxLv;

        characterParameter.XpIndex(maxXp, xp);
        characterParameter.LvIndex(lv);
    }

    public void UpgradeHp(int _Hp)
    {
        dataUpgrades.maxHp += _Hp;
        PlayerPrefs.SetInt("IndexUpgrade", PlayerPrefs.GetInt("IndexUpgrade") - 1);
    }

    public void UpgradeStamina(int _stamina)
    {
        dataUpgrades.maxStamina += _stamina;
        PlayerPrefs.SetInt("IndexUpgrade", PlayerPrefs.GetInt("IndexUpgrade") - 1);
    }

    public void UpgradeArmor(int _armor)
    {
        dataUpgrades.maxArmor += _armor;
        PlayerPrefs.SetInt("IndexUpgrade", PlayerPrefs.GetInt("IndexUpgrade") - 1);
    }

    public void CheckLevel(int _Xp)
    {
        xp += _Xp;
        if(xp >= maxXp)
        {
            lv += 1;
            maxLv += 1;
            PlayerPrefs.SetInt("Lv", lv);
            dataUpgrades.maxLv = maxLv;
            UpgradeMaxXp();
        }
        else
        {
            PlayerPrefs.SetInt("Xp", xp);        
        }

        characterParameter.XpIndex(maxXp, xp);
        characterParameter.LvIndex(lv);

    }

    public void UpgradeMaxXp()
    {
        xp = 0;
        PlayerPrefs.SetInt("Xp", 0);
        if (lv == 1)
        {
            maxXp = 2048;
            dataUpgrades.maxXp = maxXp;
        }
        else
        {
            maxXp = maxXp * 2;
            dataUpgrades.maxXp = maxXp;
        }

        PlayerPrefs.SetInt("IndexUpgrade", PlayerPrefs.GetInt("IndexUpgrade") + 1);
        
    }

   
}
