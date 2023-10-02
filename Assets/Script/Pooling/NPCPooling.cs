using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPooling : MonoBehaviour
{
    public List<GameObject> NPC = new List<GameObject>();
    private List<GameObject> NPCPool = new List<GameObject>();
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
               
                return npc;
            }
        }
        int total = Random.Range(0, NPC.Count);
        GameObject newnpc = Instantiate(NPC[total], pos, Quaternion.identity);
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
        if (npc.GetComponent<NPCDriver>().Vehiclel != null)
        {

            npc.GetComponent<NPCDriver>().Vehiclel.SetActive(false);
        }

        npc.SetActive(false);

    }
}
