using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeXpManager : MonoBehaviour
{
    public static UpgradeXpManager ins;

    [SerializeField]
    private float _Hp, _dame, _stamina, _rechargeStamina, _speed, _laserDame, _laserRange, _rocketDame, _rocketRange, _backHole;

    //[SerializeField]
    //private float _Hp,_dame,_stamina, _rechargeStamina, _speed, _laserDame, _laserRange, _rocketDame, _rocketRange, _backHole;

    [SerializeField]
    private CharacterParameter characterParameter;

    public DataGame dataGame;

    [SerializeField]
    private string dataname = "datagame";

    public void Awake()
    {
        ins = this;
        dataGame = new DataGame();
        if (PlayerPrefs.HasKey(dataname))
        {
            string data = PlayerPrefs.GetString(dataname);
            dataGame = JsonConvert.DeserializeObject<DataGame>(data);

        }
        else
        {
            SaveData();
        }
    }


    public void UpgradeHp()
    {
        dataGame.maxHp += _Hp;
        SaveData();
    }

    public void UpgradeStamina()
    {
        dataGame.maxStamina += _stamina;
        SaveData();
    }

    public void UpgradeMeleeDame()
    {
        dataGame.maxMeleeDame += _dame;
        SaveData();
    }

    public void UpgradeStaminaRecharge()
    {
        dataGame.maxStaminaRecharge += _rechargeStamina;
        SaveData();
    }

    public void UpgradeSprintSpeed()
    {
        dataGame.maxSprintSpeed += _speed;
        SaveData();
    }

    public void UpgradeLaserDamage()
    {
        dataGame.maxLaserDamage += _laserDame;
        SaveData();
    }
    public void UpgradeLaserRange()
    {
        dataGame.maxLaserRange += _laserRange;
        SaveData();
    }
    public void UpgradeRocketDame()
    {
        dataGame.maxRocketDame += _rocketDame;
        SaveData();
    }
    public void UpgradeRocketRange()
    {
        dataGame.maxRocketRange += _rocketRange;
        SaveData();
    }
    public void UpgradeBackHole()
    {
        dataGame.maxBackHole += _backHole;
        SaveData();
    }


    public void SaveData()
    {
        PlayerPrefs.SetString(dataname, JsonConvert.SerializeObject(dataGame));
    }
}

public class DataGame
{
    [SerializeField]
    private float _Hp, _dame, _stamina, _rechargeStamina, _speed, _laserDame, _laserRange, _rocketDame, _rocketRange, _backHole;

    public float maxHp { get => _Hp; set => _Hp = value; }
    public float maxMeleeDame { get => _dame; set => _dame = value; }
    public float maxStamina { get => _stamina; set => _stamina = value; }
    public float maxStaminaRecharge { get => _rechargeStamina; set => _rechargeStamina = value; }
    public float maxSprintSpeed { get => _speed; set => _speed = value; }
    public float maxLaserDamage { get => _laserDame; set => _laserDame = value; }
    public float maxLaserRange { get => _laserRange; set => _laserRange = value; }
    public float maxRocketDame { get => _rocketDame; set => _rocketDame = value; }
    public float maxRocketRange { get => _rocketRange; set => _rocketRange = value; }
    public float maxBackHole { get => _backHole; set => _backHole = value; }


    public DataGame()
    {
        maxHp = 500;
        maxMeleeDame = 20;
        maxStamina = 100;
        maxStaminaRecharge = 2.22f;
        maxSprintSpeed = 0.5f;
        maxLaserDamage = 30;
        maxLaserRange = 50;
        maxRocketDame = 150;
        maxRocketRange = 10;
        maxBackHole = 25;

    }
}

