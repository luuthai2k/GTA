using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Layer", menuName = "Layer/LayerData")]
public class LayerData : ScriptableObject
{
    public LayerMask HumanLayer;
    public LayerMask MetalLayer;
    public LayerMask StoneLayer;
    public LayerMask WoodLayer;
    public LayerMask VehiclesLayer;
    public LayerMask ObstacleLayer;
}
