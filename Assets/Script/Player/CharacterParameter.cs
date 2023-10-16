using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterParameter : MonoBehaviour
{
    [SerializeField]
    private Slider heat, stamina, xp, armor;

    [SerializeField]
    private Text lv;

    public void HeatIndex(int _maxheat, int _heat)
    {
        heat.maxValue = _maxheat;
        heat.value = _heat;
    }
    public void StaminaIndex(float _maxstamina,float _stamina)      
    {
        stamina.maxValue = _maxstamina;
        stamina.value = _stamina;   
    }    

    public void Armor(int _maxarmor,int _armor)
    {
        armor.maxValue = _maxarmor;
        armor.value = _armor;
    }

    public void XpIndex(int _maxXp,int _xp)
    {
        xp.maxValue = _maxXp;
        xp.value = _xp;
    }

    public void LvIndex(int _lv)
    {
        lv.text = "Lv " + _lv;
    }
}
