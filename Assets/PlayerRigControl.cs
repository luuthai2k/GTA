using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerRigControl : MonoBehaviour
{
    public RigBuilder rigBuilder;
    public HeadLookAt headLookAt;
    public MultiAimConstraint multiAimConstraint_Spine;
    void Start()
    {
        multiAimConstraint_Spine.weight = 0;
    }

   public void HeadLookAt()
    {
        if (!rigBuilder.layers[0].active)
        {
            rigBuilder.layers[0].active = true;
        }
      
        headLookAt.HeadLookAtToCenter();
    }
    public void ReturnHeadLookAt()
    {
        if (!rigBuilder.layers[0].active) return;
        
            rigBuilder.layers[0].active = false;
      
    }
    public void ShootLaser()
    {
        multiAimConstraint_Spine.weight = 1;
    }
    public void ReturnShootLaser()
    {
        multiAimConstraint_Spine.weight = 0;
    }
}
