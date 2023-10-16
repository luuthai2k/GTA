using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class PoliceStarManager : MonoBehaviour
{
    public static PoliceStarManager ins;

    [SerializeField]
    private List<Image> images;

    [SerializeField]
    private int indexWanter;

    [SerializeField]
    private NPCPooling npcPooling;

    [SerializeField]
    private int wanterPoint;

    public bool isQuest;

    public void Awake()
    {
        ins = this;
    }
    public void Start()
    {
        indexWanter = 0;
        wanterPoint = 0;
        ChangeStarWanter(0);
    }

    public void ChangeWanterPoint(int _wanterPoint)
    {
        if (isQuest == false)
        {
            wanterPoint += _wanterPoint;
            if (wanterPoint <= 400)
            {
                if (wanterPoint >= 10 && wanterPoint < 50)
                {
                    ChangeStarWanter(1);
                }
                else if (wanterPoint >= 50 && wanterPoint < 125)
                {
                    ChangeStarWanter(2);
                }
                else if (wanterPoint >= 125 && wanterPoint < 300)
                {
                    ChangeStarWanter(3);
                }
                else if (wanterPoint >= 300 && wanterPoint < 400)
                {
                    ChangeStarWanter(4);
                }
                else if (wanterPoint >= 400)
                {
                    wanterPoint = 500;
                    ChangeStarWanter(4);
                }
            }
        }
    }

    public int IndexWanter()
    {
        return indexWanter;
    }

    public void ChangeStarWanter(int indexStar)
    {
        indexWanter = indexStar;

        for (int i = 0; i < images.Count; i++)
        {
            if (i < indexWanter)
            {
                images[i].fillAmount = 1;
            }
            else
            {
                images[i].fillAmount = 0;
            }
        }

        if (indexWanter == 1)
        {
            npcPooling.SpawnStarNPC(0);

        }
        else if (indexWanter == 2)
        {
            npcPooling.SpawnStarNPC(0);
            npcPooling.SpawnStarNPC(0);
        }
        else if (indexWanter == 3)
        {
            npcPooling.SpawnStarNPC(0);
            npcPooling.SpawnStarNPC(0);
            npcPooling.SpawnStarNPC(0);
        }
        else if (indexWanter == 4)
        {
            npcPooling.SpawnStarNPC(1);
        }
        else if (indexWanter == 5)
        {
            npcPooling.SpawnStarNPC(2);
        }
        else
        {

        }
    }

    public void LoseWanterPoint()
    {
        indexWanter = 0;
        wanterPoint = 0;
    }
}
