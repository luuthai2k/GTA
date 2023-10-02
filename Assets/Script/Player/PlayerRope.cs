using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerRope : MonoBehaviour
{
    [SerializeField] private LayerMask pullColliderMask;
    [SerializeField] private LayerMask rushColliderMask;
    public Vector3 currentThreadEnd;
    public LineRenderer lineRenderer;
    public Transform startPoint;
    private Vector3 targetPoint;
    public float shootsilk;
    public GameObject collisionObj;
    public Rigidbody rb;
    public bool isshootsilk;
    public bool ispull;
    public bool isrush;
    public float speed = 10f;
    public float height;
    
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Q))
    //    {
    //        PrepareShoot();
            
    //    }
    //    ShootSilk();
    //}
    public void PrepareShoot()
    {
        Player.ins.animator.Play("web_shoot");
        collisionObj = PointCenterSceenToWorld.ins.CollisionObj;
        targetPoint = PointCenterSceenToWorld.ins.targetTransform.position;
        CheckCollision();
        StartCoroutine(CouroutineRotate());
       
    }
    IEnumerator CouroutineRotate()
    {
        while(transform.rotation!= Quaternion.Euler(new Vector3(0f, Camera.main.transform.eulerAngles.y, 0f)))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0f, Camera.main.transform.eulerAngles.y, 0f)), 20);
            yield return null;
        }
        yield  break;
    }
    public void StartShootSilk()
    {
        lineRenderer.enabled = true;
        currentThreadEnd = startPoint.position;
        
        StartCoroutine(UpdateLineRender());
        

    }
    public void ShootSilk()
    {
        if (!isshootsilk) return;
        {
            Player.ins.animator.SetBool("IsPull", ispull);
            Player.ins.animator.SetBool("IsRush", isrush);
            if (ispull)
            {
                Pull();
                
            }
            //else if(isrush)
            //{
            //    Rush();
               
            //}
            else
            {
                isshootsilk = false;
            }
            

        }
    }
    public void CheckCollision()
    {
        if (pullColliderMask == (pullColliderMask | (1 << collisionObj.layer)))
        {
            ispull = true;
            isrush = false;
           
        }
        else
        {
            ispull = false;
            isrush = true;
        }
    }
    IEnumerator UpdateLineRender()
    {
        lineRenderer.positionCount = 2;
        while (Vector3.Distance(currentThreadEnd, targetPoint) > 0.1f)
        {
            currentThreadEnd = Vector3.MoveTowards(currentThreadEnd, targetPoint, shootsilk * Time.deltaTime);
            lineRenderer.SetPositions(new Vector3[] { startPoint.position, currentThreadEnd });
            yield return null;
        }
        lineRenderer.enabled = false;
        isshootsilk = true;
        StartCoroutine(CollectLineRender());
        yield break;
    }
    IEnumerator CollectLineRender()
    {
      
        while (Vector3.Distance(currentThreadEnd, startPoint.position) > 0.1f)
        {
            if (isrush)
            {
               
                lineRenderer.SetPositions(new Vector3[] { startPoint.position, currentThreadEnd });
            }
            else
            {
                currentThreadEnd = Vector3.MoveTowards(currentThreadEnd, startPoint.position, shootsilk * Time.deltaTime);
                lineRenderer.SetPositions(new Vector3[] { startPoint.position, currentThreadEnd });
            }
            yield return null;
        }
        
        lineRenderer.enabled = false;
        yield break;
    }

    public void Pull()
    {
       
        Vector3 direction = transform.position + Vector3.up * height - targetPoint;
        float angle = Vector3.Angle(new Vector3(direction.x, 0, direction.z), direction);
        float angleInRadians = angle * Mathf.Deg2Rad;
        float sinValue = Mathf.Sin(angleInRadians);
        float v = Mathf.Sqrt(2 * height * 10 / sinValue);
        if (collisionObj.GetComponent<Rigidbody>() != null)
        {
            rb = collisionObj.GetComponent<Rigidbody>();
            float forceMagnitude = rb.mass *v;
            rb.AddForce(direction * forceMagnitude);
            ispull = false;
            //isshootsilk = false;
        }
        ispull = false;

    }
    public void Rush()
    {

        Vector3 direction = targetPoint - transform.position;
        Player.ins.characterController.Move(direction.normalized * speed * Time.deltaTime);
        if (direction.magnitude < 1f)
        {
            isrush = false;
            //isshootsilk = false;
        }
    }
  
}
