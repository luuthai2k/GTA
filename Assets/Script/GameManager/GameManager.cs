using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager ins;
    public LayerData layerData;
    void Awake()
    {
        ins = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
