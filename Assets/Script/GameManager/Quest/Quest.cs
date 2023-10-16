using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public List<Transform> checkPoint;


    public void Start()
    {
        InitStart();
    }
    public void InitStart()
    {
        for (int i = 0; i < checkPoint.Count; i++)
        {
            checkPoint[i].gameObject.SetActive(false);
        }
    }
}
