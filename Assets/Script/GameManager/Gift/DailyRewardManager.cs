using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardManager : MonoBehaviour
{
    [SerializeField]
    private List<DailyReward> dailyRewards;

    [SerializeField]
    private GameObject buttonClaim, buttonClaimX2;

    [SerializeField]
    private DateTime timeDaily;

    [SerializeField]
    private Text timeText;

    [SerializeField]
    private int dailyDay;


    public void Start()
    {
        //InitDaily();
        if (PlayerPrefs.HasKey("TimeDaily"))
        {
            timeDaily = DateTime.Parse(PlayerPrefs.GetString("TimeDaily"));
        }
        else
        {
            timeDaily = DateTime.Now;

            PlayerPrefs.SetString("TimeDaily", timeDaily.ToString());

        }

        dailyDay = PlayerPrefs.GetInt("DailyReward");
    }

    public void InitDaily()
    {
        for (int i = 0; i < dailyRewards.Count; i++)
        {
            if (dailyDay > i)
            {
                dailyRewards[i].LastDaily();
            }
            else if (dailyDay == i)
            {
                dailyRewards[i].NowDaily();
            }
            else
            {
                dailyRewards[i].FutureDaily();
            }
        }
    }

    public void Claim()
    {
        dailyRewards[dailyDay].Claim();

        PlayerPrefs.SetInt("DailyReward", dailyDay + 1);
        dailyDay++;
        if(dailyDay >= 6)
        {
            dailyDay = 0;
            PlayerPrefs.SetInt("DailyReward", 0);
        }

        timeDaily = DateTime.Now.AddDays(1);
        PlayerPrefs.SetString("TimeDaily", timeDaily.ToString());
    }

    public void ClaimX2()
    {
        dailyRewards[PlayerPrefs.GetInt("DailyReward")].Claim();
        dailyRewards[PlayerPrefs.GetInt("DailyReward")].Claim();

        PlayerPrefs.SetInt("DailyReward", PlayerPrefs.GetInt("DailyReward") + 1);

        timeDaily = DateTime.Now.AddDays(1);
        PlayerPrefs.SetString("TimeDaily", timeDaily.ToString());
    }

    public void ButtonClaim()
    {
        if (timeDaily <= DateTime.Now)
        {
            buttonClaim.SetActive(true);
            buttonClaimX2.SetActive(true);
            timeText.enabled = false;
        }
        else
        {
            buttonClaim.SetActive(false);
            buttonClaimX2.SetActive(false);
            timeText.enabled = false;
            TimeClaim();
        }
    }

    public void TimeClaim()
    {
        TimeSpan time = timeDaily - DateTime.Now;

        timeText.text = time.ToString(@"hh\:mm\:ss");

        Invoke("TimeClaim", 1f);
    }

    public void AdsSkipTime()
    {
        timeDaily = DateTime.Now;
    }

}
