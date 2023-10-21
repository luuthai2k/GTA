using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAIMove : MonoBehaviour
{
  
    public SelectPoint selectPoint;
    public List<Transform> nextpoint;
    public NPCPooling npcPooling;
    public float maxdistance;
    public float mindistance;
    public float mintime;
    public float maxtime;
    private float time;
    public float timedelay;
    public Transform _nextpoint;
    void Start()
    {
        Init();      
    }

    public void Init()
    {
        _nextpoint = RandomNextPoint();
        time = Random.Range(mintime, maxtime);
        StartCoroutine(SpawnNPCCouroutine());
    }

    public Transform RandomNextPoint()
    {
        if (nextpoint.Count > 0)
        {
            
            int randomIndex = Random.Range(0, nextpoint.Count);

          
            return nextpoint[randomIndex];
        }
        else
        {
           
            return null;
        }
    }
    IEnumerator SpawnNPCCouroutine()
    {
        yield return new WaitForSeconds(time);
        float dis = Vector3.Distance(transform.position, Player.ins.transform.position);
        if (dis < mindistance || dis > maxdistance||!NPCManager.ins.CheckCanSpawn())
        {
            yield break;
        }
        GameObject newnpc = npcPooling.GetPool(transform.position);
        
        NPCControl npcControl = newnpc.GetComponent<NPCControl>();
        npcControl.pointtarget = RandomNextPoint();
        newnpc.transform.LookAt(_nextpoint);
        npcControl.lastpoint = transform;
      
        if (selectPoint == SelectPoint.Walk)
        {
            npcControl.npcState.ChangeState(SelectState.Move);
        }
        else if (selectPoint == SelectPoint.Drive)
        {
            npcControl.npcState.ChangeState(SelectState.Driver);
            npcControl.transform.parent.GetComponent<Car>().npcDrive = npcControl;
        }
        yield return new WaitForSeconds(timedelay);
        StartCoroutine(SpawnNPCCouroutine());
        yield break;
    }

}
public enum SelectPoint
{
    Walk,
    Drive
}