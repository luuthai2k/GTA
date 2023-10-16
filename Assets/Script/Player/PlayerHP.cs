using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    private int hp, firstHp;

    public float stamina, firstStamina;

    [SerializeField]
    private int armor, maxArmor;

    [SerializeField]
    private PlayerControl playerControl;

    private bool isDead;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject ragdoll;

    [SerializeField]
    private GameObject ragdollNow;

    [SerializeField]
    private Vector3 ragdollPosition;

    [SerializeField]
    private Vector3 firstPosition;

    [SerializeField]
    private ParticleSystem effectblood;

    private float timeWanter;

    private float timeStamina;

    private float timeHp;

    [SerializeField]
    private CharacterParameter characterParameter;

    [SerializeField]
    private List<int> timeLoseWanter;

    [SerializeField]
    private Rigidbody rb;


    public void Start()
    {
        firstPosition = transform.position;
        effectblood.Stop();
        CheckStarWanter();
        CheckStamina();
        CheckHp();
        firstHp = hp;
        firstStamina = stamina;
        armor = 0;
        maxArmor = 100;
        characterParameter.Armor(maxArmor, armor);
    }


    public void OnHit(HitDameState dameState, bool isRagdoll, int dame, Vector3 pos,float powerRagdoll = 0)
    {
        if (isDead == false)
        {
            if (dameState == HitDameState.Car)
            {
                hp -= dame;
                //OnRagdoll(Vector3.zero,powerRagdoll);
            }
            else if (dameState == HitDameState.Water)
            {
                if (stamina >= 0)
                {
                    ChangeStamina(dame);
                }
                else
                {
                    hp -= dame;
                }
            }
            else if (dameState == HitDameState.Weapon)
            {
                effectblood.Play();
                if (armor <= 0)
                {             
                    hp -= dame;
                }
                else
                {  
                    ChangeArmor(dame);
                }
            }

            if (hp <= 0)
            {
                isDead = true;
                OnDead();
            }
            if (isRagdoll)
            {
                OnRagdoll(Vector3.zero,powerRagdoll);
            }

            characterParameter.HeatIndex(firstHp, hp);
            timeWanter = Time.time;
        }

    }
    public void CheckStarWanter()
    {
        if(PoliceStarManager.ins.IndexWanter() != 0)
        {
            float indexWanter = timeLoseWanter[PoliceStarManager.ins.IndexWanter()];

            if (timeWanter + indexWanter <= Time.time)
            {
                PoliceStarManager.ins.LoseWanterPoint();
                timeWanter = Time.time;
            }
        }
        Invoke("CheckStarWanter", 0.5f);
    }


    public void OnRagdoll(Vector3 pos, float power = 0)
    {
        player.SetActive(false);

        ragdollNow = Instantiate(ragdoll, transform.position, transform.rotation);
        gameObject.GetComponent<CharacterController>().enabled = false;

        rb.AddForce(pos * power);

        Invoke("EndRagdoll", 1.25f);
    }


    public void EndRagdoll()
    {
        transform.position = new Vector3(ragdollNow.transform.position.x, transform.position.y, ragdollNow.transform.position.z);

        player.SetActive(true);

        gameObject.GetComponent<CharacterController>().enabled = true;
        Destroy(ragdollNow);
    }

    public void OnDead()
    {
        player.SetActive(false);
        QuestManager.ins.LoseQuest();
        ragdollNow = Instantiate(ragdoll, transform.position, transform.rotation);
        gameObject.GetComponent<CharacterController>().enabled = false;
        transform.parent = null;
        Player.ins.ChangeControl(0);
        Player.ins.animator.Play("Grounded");
        CameraManager.ins.ChangeCam(0,Player.ins.transform);
        Invoke("EndDead", 1.25f);
    }

    public void EndDead()
    {

        hp = firstHp;
        stamina = firstStamina;
        isDead = false;
        characterParameter.HeatIndex(firstHp, hp);
        characterParameter.StaminaIndex(firstStamina, stamina);
        player.SetActive(true);
        transform.position = firstPosition;
        gameObject.GetComponent<CharacterController>().enabled = true;
        NPCPooling.ins.CheckPlayerDead();
        Destroy(ragdollNow);
     
    }

    public void ChangeStamina(float _stamina)
    {
        stamina -= _stamina;
        if (stamina <= 0)
        {
            stamina = 0;
        }
        characterParameter.StaminaIndex(firstStamina, stamina);

    }

    public void PlusStamina(float _stamina, bool maxStamina = false)
    {
        if (maxStamina)
        {
            stamina = firstStamina;

            if (stamina >= firstStamina)
            {
                stamina = firstStamina;
            }
            characterParameter.StaminaIndex(firstStamina, stamina);
        }
        else
        {
            stamina += _stamina;

            if (stamina >= firstStamina)
            {
                stamina = firstStamina;
            }
            characterParameter.StaminaIndex(firstStamina, stamina);
        }
       
    }


    public void ChangeArmor(int _armor)
    {
        armor -= _armor;

        if(armor <= 0)
        {
            armor = 0;
        }
        else if(armor >= maxArmor)
        {
            armor = maxArmor;   
        }

        characterParameter.Armor(maxArmor, armor);
    }

    public void CheckStamina()
    {
        if (playerControl.playerState == PlayerState.Swimming)
        {

        }
        else
        {
            if (timeStamina + 2 < Time.time)
            {
                timeStamina = Time.time;
                PlusStamina(firstStamina/200);
            }
        }

        Invoke("CheckStamina", 1f);
    }

    public void PlusHp(int _hp)
    {
        hp += _hp;

        if (hp >= firstHp)
        {
            hp = firstHp;
        }
        characterParameter.HeatIndex(firstHp, hp);
    }

    public void CheckHp()
    {
        if (timeHp + 2 < Time.time)
        {
            timeHp = Time.time;
            PlusHp(5);
        }
        Invoke("CheckHp", 1f);
    }

    public void PlusArmor(int _armor)
    {
        armor = maxArmor;

        characterParameter.Armor(maxArmor, armor);
    }


}

public enum HitDameState
{
    Car,
    Water,
    Weapon,
    Explosion
}
