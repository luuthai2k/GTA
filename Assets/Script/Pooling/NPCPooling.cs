using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class NPCPooling : MonoBehaviour
{
    public static NPCPooling ins;

    [SerializeField]
    private List<GameObject> NPC = new List<GameObject>();

    [SerializeField]
    private List<GameObject> NPCPool = new List<GameObject>();

    [SerializeField]
    private List<PointAIMove> NPCAIMove = new List<PointAIMove>();

    [SerializeField]
    private List<GameObject> NPCStar;

    [SerializeField]
    private List<GameObject> NPCStarPool;

    [SerializeField]
    private List<Transform> transformSpawnPoint;

    [SerializeField]
    private int maxPoolNpcStar;

    [SerializeField]
    private int indexNpcStar;

    [SerializeField]
    private List<GameObject> NPCPickup;

    [SerializeField]
    private List<GameObject> NPCPickupPool;

    [SerializeField]
    private int maxNpcPickUp;

    [SerializeField]
    private List<GameObject> NPCQuest;

    [SerializeField]
    private List<GameObject> NPCQuestPool;

    [SerializeField]
    private int maxNpcQuest;

    [SerializeField]
    private int indexNpcQuest;

    [SerializeField]
    private List<GameObject> QuestCar;

    [SerializeField]
    private List<GameObject> QuestCarPool;

    [SerializeField]
    private int maxCarQuest;

    [SerializeField]
    private int indexCarQuest;

    public void Awake()
    {
        ins = this;
    }

    public void ReStartGame()
    {
        for (int i = 0; i < NPCAIMove.Count; i++)
        {
            NPCAIMove[i].Init();
        }
    }


    public GameObject GetPool(Vector3 pos)
    {
        NPCManager.ins.totalnpc++;
        foreach (var npc in NPCPool)
        {
            if (!npc.gameObject.activeInHierarchy)
            {
                npc.gameObject.SetActive(true);
                npc.gameObject.transform.position = pos;
                //particle.gameObject.transform.rotation = Quaternion.identity;
                npc.GetComponent<NPCControl>().npcHp.hp = 100;
                npc.GetComponent<NPCControl>().npcHp.isdead = false;
                return npc;
            }
        }
        int total = Random.Range(0, NPC.Count);
        GameObject newnpc = Instantiate(NPC[total], pos, Quaternion.identity);
        newnpc.GetComponent<NPCControl>().npcHp.isPolice = false;
        NPCPool.Add(newnpc);
        return newnpc;
    }

    public void ReturnPool(GameObject npc, float time)
    {
        StartCoroutine(CouroutineReturnPool(npc, time));
    }
    IEnumerator CouroutineReturnPool(GameObject npc, float time)
    {
        yield return new WaitForSeconds(time);
        NPCManager.ins.totalnpc--;
        npc.transform.parent = null;
        npc.gameObject.GetComponent<Animator>().enabled = true;
        if (npc.GetComponent<NPCDriver>().Vehiclel != null)
        {
            npc.GetComponent<NPCDriver>().Vehiclel.SetActive(false);
        }
        npc.SetActive(false);
    }

    public void ReturnStarNpc(GameObject NPC)
    {
        NPC.SetActive(false);
        NPC.GetComponent<Animator>().enabled = false;
    }

    public void ResetNPCStar()
    {
        for (int i = 0; i < NPCStarPool.Count; i++)
        {
            if (NPCStarPool[i].activeSelf == false)
            {
                NPCStarPool[i].GetComponent<Animator>().enabled = true;
                NPCStarPool[i].SetActive(true);
                int indexSpawnPoint = Random.Range(0, transformSpawnPoint.Count);
                NPCStarPool[i].transform.position = transformSpawnPoint[indexSpawnPoint].position;
                NPCStarPool[i].GetComponent<NPCControl>().pointtarget = Player.ins.transform;
                NPCStarPool[i].GetComponent<NPCControl>().npcHp.hp = 100;
                NPCStarPool[i].GetComponent<NPCControl>().npcHp.isdead = false;
                return;

            }
        }
    }
    public void SpawnStarNPC(int index)
    {

        if (index == 0)
        {
            if (indexNpcStar < maxPoolNpcStar)
            {
                int indexSpawnPoint = Random.Range(0, transformSpawnPoint.Count);

                GameObject newnpc = Instantiate(NPCStar[0], transformSpawnPoint[indexSpawnPoint].position, Quaternion.identity);
                NPCStarPool.Add(newnpc);
                newnpc.GetComponent<NPCState>().ChangeState(SelectState.Attack);
                newnpc.GetComponent<NPCControl>().npcHp.isPolice = true;
                indexNpcStar++;
            }
            else
            {
                ResetNPCStar();
            }

        }
        else if (index == 1)
        {
            NPCStar[1].GetComponent<Animator>().enabled = true;
            NPCStar[1].SetActive(true);
            int indexSpawnPoint = Random.Range(0, transformSpawnPoint.Count);
            NPCStar[1].transform.position = transformSpawnPoint[indexSpawnPoint].position;

        }
        else if (index == 2)
        {
            NPCStar[2].GetComponent<Animator>().enabled = true;
            NPCStar[2].SetActive(true);
            int indexSpawnPoint = Random.Range(0, transformSpawnPoint.Count);
            NPCStar[2].transform.position = transformSpawnPoint[indexSpawnPoint].position;
        }

    }

    public void SpawnPickUp(Vector3 posPickup, bool _isMoney = false)
    {
        if (_isMoney == false)
        {
            if (NPCPickupPool.Count >= maxNpcPickUp)
            {
                for (int i = 0; i < NPCPickupPool.Count; i++)
                {
                    if (NPCPickupPool[i].activeSelf == false)
                    {
                        NPCPickupPool[i].SetActive(true);

                        NPCPickupPool[i].transform.position = posPickup;
                        return;
                    }
                }
            }
            else
            {
                int index = Random.Range(1, 3);
                GameObject pickupNew = Instantiate(NPCPickup[index], posPickup, Quaternion.identity);

                NPCPickupPool.Add(pickupNew);
            }
        }

        else
        {
            if (NPCPickupPool.Count >= maxNpcPickUp)
            {
                for (int i = 0; i < NPCPickupPool.Count; i++)
                {
                    if (NPCPickupPool[i].activeSelf == false && NPCPickupPool[i].GetComponent<MoneyPickup>() != null)
                    {
                        NPCPickupPool[i].SetActive(true);

                        NPCPickupPool[i].transform.position = posPickup;
                        return;
                    }
                }
            }
            else
            {
                GameObject pickupNew = Instantiate(NPCPickup[0], posPickup, Quaternion.identity);

                NPCPickupPool.Add(pickupNew);
            }
        }
    }

    public void SpawnNPCQuest(Vector3 pos)
    {
        if (indexNpcQuest < maxNpcQuest)
        {
            GameObject newnpc = Instantiate(NPCQuest[0], pos, Quaternion.identity);
            NPCQuestPool.Add(newnpc);
            newnpc.GetComponent<NPCState>().ChangeState(SelectState.Attack);
            newnpc.GetComponent<NPCControl>().npcHp.isNpcQuest = true;
            indexNpcQuest++;
        }
        else
        {
            ResetNPCQuest(pos);
        }
    }

    public void ReturnNpcQuest(GameObject NPC)
    {

        NPC.SetActive(false);
        NPC.GetComponent<Animator>().enabled = false;

    }

    public void ResetNPCQuest(Vector3 pos)
    {
        for (int i = 0; i < NPCStarPool.Count; i++)
        {
            if (NPCQuestPool[i].activeSelf == false)
            {
                NPCQuestPool[i].GetComponent<Animator>().enabled = true;
                NPCQuestPool[i].SetActive(true);

                NPCQuestPool[i].transform.position = pos;
                NPCQuestPool[i].GetComponent<NPCControl>().pointtarget = Player.ins.transform;
                NPCQuestPool[i].GetComponent<NPCControl>().npcHp.hp = 100;
                NPCQuestPool[i].GetComponent<NPCControl>().npcHp.isdead = false;
                return;
            }
        }
    }

    public void SpawnCarQuest(Vector3 pos)
    {
        if (indexCarQuest < maxCarQuest)
        {
            GameObject newnpc = Instantiate(QuestCar[0], pos, Quaternion.identity);
            QuestCarPool.Add(newnpc);
            //newnpc.GetComponent<NPCState>().ChangeState(SelectState.Attack);
            //newnpc.GetComponent<NPCControl>().npcHp.isNpcQuest = true;
            indexCarQuest++;
        }
        else
        {
            ResetNPCQuest(pos);
        }
    }

    public void ReturnCarQuest(GameObject Car)
    {
        Car.SetActive(false);
        //NPC.GetComponent<Animator>().enabled = false;
    }

    public void ResetCarQuest(Vector3 pos)
    {
        for (int i = 0; i < QuestCarPool.Count; i++)
        {
            if (QuestCarPool[i].activeSelf == false)
            {
                QuestCarPool[i].GetComponent<Animator>().enabled = true;
                QuestCarPool[i].SetActive(true);

                QuestCarPool[i].transform.position = pos;
                QuestCarPool[i].GetComponent<NPCControl>().pointtarget = Player.ins.transform;
                QuestCarPool[i].GetComponent<NPCControl>().npcHp.hp = 100;
                QuestCarPool[i].GetComponent<NPCControl>().npcHp.isdead = false;
                return;
            }
        }
    }

    public void CheckPlayerDead()
    {
        for (int i = 0; i < NPCPool.Count; i++)
        {
            NPCPool[i].GetComponent<NPCControl>().npcState.ChangeState(SelectState.Move);
            NPCPool[i].GetComponent<NPCControl>().pointtarget = NPCPool[i].GetComponent<NPCControl>().lastpoint;
        }
        if (NPCStarPool.Count != 0)
        {
            for (int i = 0; i < NPCStarPool.Count; i++)
            {
                ReturnStarNpc(NPCStarPool[i]);
            }
        }

        if (NPCQuestPool.Count != 0)
        {
            for (int i = 0; i < NPCQuestPool.Count; i++)
            {
                ReturnNpcQuest(NPCQuestPool[i]);
            }
        }

        if (QuestCarPool.Count != 0)
        {
            for (int i = 0; i < NPCQuestPool.Count; i++)
            {
                ReturnCarQuest(QuestCarPool[i]);
            }
        }
    }

    public void CheckEndQuest()
    {
        if (NPCQuestPool.Count != 0)
        {
            for (int i = 0; i < NPCQuestPool.Count; i++)
            {
                ReturnNpcQuest(NPCQuestPool[i]);
            }
        }

        if (QuestCarPool.Count != 0)
        {
            for (int i = 0; i < NPCQuestPool.Count; i++)
            {
                ReturnCarQuest(QuestCarPool[i]);
            }
        }
    }

}
