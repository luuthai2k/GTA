using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public static NPCManager ins;
    public NPCPooling npcPooling;
    public float maxdistance=200;
    public float maxNpc;
    public float totalnpc;
    
    void Awake()
    {
        ins = this;
    }

    public bool CheckCanSpawn()
    {
        if (totalnpc >= maxNpc)
        {
            return false;
        }
        else return true;
    }
}
