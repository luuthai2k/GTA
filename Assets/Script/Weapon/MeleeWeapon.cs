using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public SelectWeapon meleeWeapons;
    public LayerMask HumanLayer;
    private ParticleSystem hitEffect;
    private void Start()
    {
        HumanLayer = GameManager.ins.layerData.HumanLayer;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (HumanLayer == (HumanLayer | (1 << other.gameObject.layer)))
        {
            hitEffect = FxPooling.ins.GetbloodEffectPool(other.transform.position);         
            hitEffect.gameObject.transform.parent = other.gameObject.transform;
        }
    }
}
