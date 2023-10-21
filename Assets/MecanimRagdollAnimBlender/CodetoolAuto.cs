using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodetoolAuto : MonoBehaviour
{
    [SerializeField]
    private List<PhysicsBodyPart> physicsBodyParts1,physicsBodyParts2,physicsBodyParts;

    [ContextMenu("check")]
    public void Check()
    {
        for (int i = 0; i < physicsBodyParts1.Count; i++)
        {
            physicsBodyParts1[i].m_blend = physicsBodyParts2[i].m_blend;
            physicsBodyParts1[i].m_animBone = physicsBodyParts2[i].m_animBone;
            physicsBodyParts1[i].m_animator = physicsBodyParts2[i].m_animator;
            physicsBodyParts1[i].m_connectedParts = physicsBodyParts2[i].m_connectedParts;
            physicsBodyParts1[i].m_controlRegain = physicsBodyParts2[i].m_controlRegain;
            physicsBodyParts1[i].m_defaultControlRegain = physicsBodyParts2[i].m_defaultControlRegain;
            physicsBodyParts1[i].m_connectedPartsWeight = physicsBodyParts2[i].m_connectedPartsWeight;
            physicsBodyParts1[i].m_balanceRatio = physicsBodyParts2[i].m_balanceRatio;
            physicsBodyParts1[i].m_blendMax = physicsBodyParts2[i].m_blendMax;
            physicsBodyParts1[i].m_rigidness = physicsBodyParts2[i].m_rigidness;
            physicsBodyParts1[i].m_blendSpeed = physicsBodyParts2[i].m_blendSpeed;
            physicsBodyParts1[i].mBounceFactor = physicsBodyParts2[i].mBounceFactor;

        }

        //for (int i = 0; i < physicsBodyParts.Count; i++)
        //{
        //    if (physicsBodyParts[i].m_animator != null)
        //    {
        //        physicsBodyParts2.Add(physicsBodyParts[i]);
        //    }
        //    else
        //    {
        //        physicsBodyParts1.Add(physicsBodyParts[i]);
        //    }
        //}
    }
}
