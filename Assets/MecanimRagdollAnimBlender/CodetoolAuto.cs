using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodetoolAuto : MonoBehaviour
{
    [SerializeField]
    private List<PhysicsBodyPart> physicsBodyParts;

    [SerializeField]
    private List<PhysicsBodyPart> physicsBodyPartsOne, physicsBodyPartsTwo;

    [ContextMenu("check")]
    public void Check()
    {
        //for (int i = 0; i < physicsBodyParts.Count; i++)
        //{
        //    if(physicsBodyParts[i].m_animator == null)
        //    {
        //        physicsBodyPartsOne.Add(physicsBodyParts[i]);
        //    }
        //    else
        //    {
        //        physicsBodyPartsTwo.Add(physicsBodyParts[i]);
        //    }
        //}

        for (int i = 0; i < physicsBodyPartsOne.Count; i++)
        {
            physicsBodyPartsOne[i].m_blend = physicsBodyPartsTwo[i].m_blend;
            physicsBodyPartsOne[i].m_animBone = physicsBodyPartsTwo[i].m_animBone;
            physicsBodyPartsOne[i].m_animator = physicsBodyPartsTwo[i].m_animator;
            physicsBodyPartsOne[i].m_connectedParts = physicsBodyPartsTwo[i].m_connectedParts;
            physicsBodyPartsOne[i].m_defaultControlRegain = physicsBodyPartsTwo[i].m_defaultControlRegain;
            physicsBodyPartsOne[i].m_connectedPartsWeight = physicsBodyPartsTwo[i].m_connectedPartsWeight;
            physicsBodyPartsOne[i].m_balanceRatio = physicsBodyPartsTwo[i].m_balanceRatio;
            physicsBodyPartsOne[i].m_blendMax = physicsBodyPartsTwo[i].m_blendMax;
            physicsBodyPartsOne[i].m_rigidness = physicsBodyPartsTwo[i].m_rigidness;
            physicsBodyPartsOne[i].m_blendSpeed = physicsBodyPartsTwo[i].m_blendSpeed;
            physicsBodyPartsOne[i].mBounceFactor = physicsBodyPartsTwo[i].mBounceFactor;


        }
    }
}
