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
    public bool canpull;
    public bool canrush;
    public float speed = 10f;
    public float height;
    public float dis;
    public bool isRope;

    public void Rope(CharacterControl characterControl)
    {

        PrepareShoot();
        ShootSilk();
    }
    public void PrepareShoot()
    {
        if (isRope) return;
        isRope = true;
        Player.ins.animator.Play("rope");
        collisionObj = PointCenterSceenToWorld.ins.CollisionObj;
        targetPoint = PointCenterSceenToWorld.ins.targetTransform.position;
        CheckCollision();
        StartCoroutine(CouroutineRotate());

    }
    IEnumerator CouroutineRotate()
    {
        while (transform.rotation != Quaternion.Euler(new Vector3(0f, Camera.main.transform.eulerAngles.y, 0f)))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0f, Camera.main.transform.eulerAngles.y, 0f)), 20);
            yield return null;
        }
        yield break;
    }
    public void StartShootSilk()
    {
        lineRenderer.enabled = true;
        currentThreadEnd = startPoint.position;

        StartCoroutine(UpdateLineRender());


    }
    public void ShootSilk()
    {

        if (ispull)
        {
            Pull();
            return;
        }
        else if (isrush)
        {
            Rush();
            return;

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

        yield return new WaitForSeconds(0.5f);
        lineRenderer.enabled = false;
        isshootsilk = true;
        CheckPullorRush();
        StartCoroutine(CollectLineRender());
        yield break;
    }
    public void CheckPullorRush()
    {
        if (ispull)
        {
            canpull = true;
        }
        if (ispull)
        {
            canrush = false;
        }
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
        Vector3 _target = transform.position + (collisionObj.transform.position - transform.position).normalized * dis;
        Vector3 direction = _target - collisionObj.transform.position;
        float distanceToTarget = direction.magnitude;
        float angle = Vector3.Angle(transform.position - targetPoint, collisionObj.transform.forward);
        Debug.Log(angle);
        float initialVelocityY = Mathf.Sqrt(2 * height * Mathf.Abs(Physics.gravity.y));
        float initialVelocityXZ = distanceToTarget / (Mathf.Sqrt(2 * height / Mathf.Abs(Physics.gravity.y)) + Mathf.Sqrt(2 * Mathf.Abs(distanceToTarget - height) / Mathf.Abs(Physics.gravity.y)));
        Vector3 throwVelocity = direction.normalized * initialVelocityXZ;
        throwVelocity.y = initialVelocityY;
        Vector3 randomTorque = new Vector3(angle - 90, 0, angle);

        if (collisionObj.GetComponent<Rigidbody>() != null)
        {
            rb = collisionObj.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddTorque(randomTorque * 10, ForceMode.VelocityChange);
            rb.velocity = throwVelocity;

        }
        canpull = false;
        isshootsilk = false;
        ispull = false;
        isRope = false;

    }
    public void Rush()
    {

        Vector3 direction = targetPoint - transform.position;
        Player.ins.characterController.Move(direction.normalized * speed * Time.deltaTime);
        if (direction.magnitude < 1f)
        {
            isrush = false;
            isRope = false;
        }
    }

}
