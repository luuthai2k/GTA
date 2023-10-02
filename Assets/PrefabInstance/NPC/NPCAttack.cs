using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAttack : MonoBehaviour
{
    [Header("Control")]
    public NPCControl npcControl;
    public NPCMovement npcMovement;
    public bool isAttack;
    public bool isCombat;
    public bool isRanged;
    [Header("Weapon")]
    public GameObject meleeWeaponPrefab;
    private MeleeWeapon meleeWeapon;
    public GameObject rangedWeaponPrefab;
    private RangedWeapon rangedWeapon;
    public Transform addWeapon;
    public bool isfinish;
    float time;
    public float offset;
    Ray ray;
    public void SeclectWeapon()
    {

    }

    public void Combat(SelectWeapon currenWeapon)
    {
        bool isGrounded = npcControl.animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded");
        //npcControl.animator.SetBool("DoMelee", false);
        if ( !npcControl.npcMovement.ismove)
        {
            if(meleeWeaponPrefab == null)
            {
                meleeWeaponPrefab = Instantiate(npcControl.chacractorData.meleeWeapon, addWeapon);
                meleeWeaponPrefab.transform.localPosition = Vector3.zero;
                meleeWeaponPrefab.transform.localRotation = Quaternion.identity;
                meleeWeapon = meleeWeaponPrefab.GetComponent<MeleeWeapon>();
            }
            else
            {
                meleeWeaponPrefab.SetActive(true);
            }
            if (isGrounded)
            {
                npcControl.animator.SetBool("Strafe", true);
            }
            npcControl.animator.SetInteger("MeleeWeaponType", (int)currenWeapon);
            transform.LookAt(npcControl.enemy.transform);
            if (currenWeapon == SelectWeapon.Bat)
            {
                npcControl.animator.SetInteger("BatType", Random.Range(0, 4));
            }
            else if (currenWeapon == SelectWeapon.Knife)
            {
                npcControl.animator.SetInteger("KnifeType", Random.Range(0, 3));
            }
            else
            {
                npcControl.animator.SetInteger("UnarmedType", Random.Range(0, 9));
            }
        }
    }
    public bool CheckObstacle()
    {
        ray.origin =transform.position+Vector3.up;
        ray.direction = ((npcControl.enemy.transform.position+ Vector3.up) - ray.origin).normalized;
        float distance = Vector3.Distance(npcControl.enemy.transform.position,transform.position)-1f;
        if (Physics.Raycast(ray, distance, ~npcControl.obstacleMask))
        {
            return false;
        }
        else return true;
    }
    public void Ranged(SelectWeapon currenWeapon)
    {
        bool isGrounded = npcControl.animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded");
        if (!npcControl.npcMovement.ismove)
        {
           
            npcControl.animator.applyRootMotion = true;
            npcControl.navMeshAgent.enabled = false;
            if (rangedWeaponPrefab == null)
            {
                rangedWeaponPrefab = Instantiate(npcControl.chacractorData.rangedWeapon, addWeapon);
                rangedWeaponPrefab.transform.localPosition = Vector3.zero;
                rangedWeaponPrefab.transform.localRotation = Quaternion.identity;
                rangedWeapon = rangedWeaponPrefab.GetComponent<RangedWeapon>();
            }
            else
            {
                rangedWeaponPrefab.SetActive(true);
            }
            npcControl.animator.SetInteger("MeleeWeaponType", (int)currenWeapon);
           
            if (isGrounded )
            {
                npcControl.animator.SetBool("Strafe", true);
                //npcControl.animator.SetBool("DoMelee", true);
            }
            if (currenWeapon == SelectWeapon.Pistol)
            {
                Vector3 randomOffset = new Vector3(Random.Range(-offset, offset), Random.Range(-offset, offset), 0f);
                Vector3 directionShoot = transform.forward+ randomOffset;
                
                rangedWeapon.StartShooting(directionShoot);
            }
            if (currenWeapon == SelectWeapon.Rifle)
            {

            }
            if (currenWeapon == SelectWeapon.Shotgun)
            {
                //_weapon.GetComponent<ShotGun>().StartShooting();
            }
            if (currenWeapon == SelectWeapon.Minigun)
            {

            }
        }
    }
    public void Attack(SelectWeapon meleeWeapon, SelectWeapon rangedWeapon)
    {
        StartCoroutine(ActackCouroutine(meleeWeapon, rangedWeapon));
    }
    IEnumerator ActackCouroutine(SelectWeapon meleeWeapon, SelectWeapon rangedWeapon)
    {
        while (isAttack)
        {
            float distance = Vector3.Distance(npcControl.enemy.transform.position, transform.position);
            Debug.Log(distance);
            npcControl.pointtarget = npcControl.enemy.transform;
            if (rangedWeapon != SelectWeapon.Unarmed )
            {
                if (distance > 15)
                {
                    if (isRanged )
                    {
                        if (distance > 40 || !CheckObstacle() || !CheckDirection())
                        {
                            if (!isfinish)
                            {
                                FinishActack(1f);
                            }
                           
                            npcMovement.canmove = true;
                            npcMovement.Move(1f);
                        }
                        else
                        {
                            npcMovement.canmove = false;
                            Ranged(rangedWeapon);
                        }
                    }
                    else
                    {
                       
                        npcMovement.canmove = true;
                        npcMovement.Move(1f);
                    }
                  

                }
                else if (distance <= 15 && distance >= 10)
                {
                    CheckDistanceShoot(distance);
                    if (CheckObstacle() && CheckDirection())
                    {
                        if (!isRanged)
                        {
                            isRanged = true;
                        }
                        npcMovement.canmove = false;
                        Ranged(rangedWeapon);
                    }
                    else
                    {
                        if (!isfinish)
                        {
                            FinishActack(1f);
                        }
                       
                        npcMovement.canmove = true;
                        npcMovement.Move(1f);
                    }
                }
                else
                {
                    if (distance > 1f)
                    {
                        if (isRanged)
                        {
                            CheckDistanceShoot(distance);
                            if (distance <7 || !CheckObstacle() || !CheckDirection())
                            {
                                if (!isfinish)
                                {
                                    FinishActack(1f);
                                }
                               
                                npcMovement.canmove = true;
                                npcMovement.Move(1f);
                            }
                            else
                            {
                                npcMovement.canmove = false;
                                Ranged(rangedWeapon);
                            }
                        }
                        else if (isCombat)
                        {
                            if (distance > 1.2f)
                            {
                                if (!isfinish)
                                {
                                    FinishActack(2f);
                                }

                                npcMovement.canmove = true;
                                npcMovement.Move(1f);
                            }
                            else
                            {
                                if (!isCombat)
                                {
                                    isCombat = true;
                                }
                                npcMovement.canmove = false;
                                Combat(meleeWeapon);
                            }
                        }
                        else
                        {
                            if (!isfinish)
                            {
                                FinishActack(2f);
                            }
                          
                            npcMovement.canmove = true;
                            npcMovement.Move(1f);
                        }
                       
                    }
                    else 
                    {
                       
                        if (!isCombat)
                        {
                            isCombat = true;
                        }
                        npcMovement.canmove = false;
                        Combat(meleeWeapon);

                    }
                }
            }
            else
            {
                if (distance > 1f)
                {
                    if (!isfinish)
                    {
                        FinishActack(2f);
                    }
                    npcMovement.canmove = true;
                   
                    npcMovement.Move(1f);
                }
                else 
                {
                    if (!isCombat)
                    {
                        isCombat = true;
                    }
                    npcMovement.canmove = false;
                    Combat(meleeWeapon);
                }
            }
            yield return null;
        }
    }
    public void FinishActack(float delaytime)
    {
        isfinish = true;
        StartCoroutine(CouroutineFinishActack(delaytime));
    }
    IEnumerator CouroutineFinishActack(float delaytime)
    {
        yield return new WaitForSeconds(delaytime);
        if (isRanged || isCombat)
        {
            npcControl.animator.SetBool("Strafe", false);
            if (meleeWeaponPrefab != null)
            {
                meleeWeaponPrefab.SetActive(false);
            }
            if (rangedWeaponPrefab != null)
            {
                rangedWeaponPrefab.SetActive(false);
            }

            
           
        }
        isfinish = false;
       
    }
    public void CheckDistanceShoot(float distance)
    {
       
        if (distance >= 10)
        {
            npcControl.animator.SetFloat("Vertical", 0);

        }
        else
        {
            npcControl.animator.SetFloat("Vertical", -1 - (5 - distance) * 0.2f);
          
        }
    }
    public bool CheckDirection()
    {
        float angle = Vector3.SignedAngle(transform.forward, npcControl.enemy.transform.position-transform.position,Vector3.up);
       
        if (angle <= 10 && angle >= -10)
        {
            if (angle < 2 && angle > -2)
            {
                npcControl.animator.SetFloat("Horizontal", 0);
               
            }
            else
            {

                npcControl.animator.SetFloat("Horizontal", angle * 0.1f);
                
               
                time += Time.deltaTime;
                if (time >= 0.2f)
                {
                    transform.LookAt(npcControl.enemy.transform);
                    time = 0;
                }
            }
            return true;
        }
       
        else
        {
            npcControl.animator.SetFloat("Horizontal", 0);
           
            return false;
        }
        
    }
}
