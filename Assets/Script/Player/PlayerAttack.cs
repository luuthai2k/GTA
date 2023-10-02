using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public SelectWeapon selectWeapon;
    public GameObject _weapon;
    public List<Collider> hitBoxs;
    public PlayerSensor playerSensor;
    public GameObject enemy;
    public float dame;
    public bool ishit;
    public void Attack(bool actack)
    {
        //if (actack)
        //{
        Animator animator = Player.ins.animator;
        animator.SetBool("Strafe", true);
        animator.SetInteger("MeleeWeaponType", (int)selectWeapon);
        if (selectWeapon == SelectWeapon.Unarmed)
        {
            animator.SetInteger("UnarmedType", Random.Range(0, 13));
            if (playerSensor.enemy != null)
            {
                enemy = playerSensor.enemy;

                Vector3 directionToEnemy = enemy.transform.position - transform.position;
                directionToEnemy.y = 0;
                transform.rotation = Quaternion.LookRotation(directionToEnemy);

            }
        }
        if (selectWeapon == SelectWeapon.Bat)
        {
            animator.SetInteger("BatType", Random.Range(0, 4));
        }
        if (selectWeapon == SelectWeapon.Knife)
        {
            animator.SetInteger("KnifeType", Random.Range(0, 3));
        }
        if (selectWeapon == SelectWeapon.Pistol)
        {
            _weapon.GetComponent<Gun>().StartShooting();
        }
        if (selectWeapon == SelectWeapon.Rifle)
        {

        }
        if (selectWeapon == SelectWeapon.Shotgun)
        {
            _weapon.GetComponent<ShotGun>().StartShooting();
        }
        if (selectWeapon == SelectWeapon.Minigun)
        {

        }
        //}
        //else
        //{
        //    OffHitBox(); 
        //    if (selectWeapon == SelectWeapon.Unarmed)
        //    {
        //        FinishActack(0f);
        //    }
        //    if (selectWeapon == SelectWeapon.Bat)
        //    {
        //        FinishActack(2f);
        //    }
        //    if (selectWeapon == SelectWeapon.Knife)
        //    {
        //        FinishActack(2f);
        //    }
        //    if (selectWeapon == SelectWeapon.Pistol)
        //    {
        //        FinishActack(2f);
        //        _weapon.GetComponent<Gun>().FinishShooting();
        //    }
        //    if (selectWeapon == SelectWeapon.Rifle)
        //    {
        //        FinishActack(2f);
        //    }
        //    if (selectWeapon == SelectWeapon.Shotgun)
        //    {
        //        FinishActack(2f);
        //        _weapon.GetComponent<ShotGun>().FinishShooting();
        //    }
        //    if (selectWeapon == SelectWeapon.Minigun)
        //    {
        //        FinishActack(2f);
        //    }
        //}
    }
    public void FinishActack(float delaytime)
    {
        StartCoroutine(CouroutineFinishActack(delaytime));
    }
    IEnumerator CouroutineFinishActack(float delaytime)
    {
        yield return new WaitForSeconds(delaytime);
        Animator animator = Player.ins.animator;
        animator.SetBool("Strafe", false);
    }
    public void OffHitBox()
    {
        
        foreach (var hitbox in hitBoxs)
        {
            hitbox.enabled = false;
        }
    }
    public void OnHitBox()
    {
        foreach (var hitbox in hitBoxs)
        {
            hitbox.enabled = true;
        }
    }
}
public enum SelectWeapon
{
    Unarmed,
    Bat,
    Knife,  
    Pistol,
    Rifle,
    Shotgun,
    Minigun
   

}
