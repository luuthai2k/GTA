using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerRope : MonoBehaviour
{
    [SerializeField] private LayerMask pullColliderMask;
    [SerializeField] private LayerMask rushColliderMask;
    [SerializeField] Transform ropeTarget;
    public Vector3 currentThreadEnd;
    public LineRenderer lineRenderer;
    public Transform startPoint;
    public float shootsilk;
    public GameObject collisionObj;
    public Rigidbody rb;
    public bool isshootsilk;
    public bool ispull;
    public bool isrush;
    public bool canpull;
    public bool canrush;
    public float speed = 10f;
    private float currentspeed;
    public float height;
    public float dis;
    CharacterControl characterControl;
    float timedelay;

    public void Rope(CharacterControl _characterControl)
    {
        characterControl = _characterControl;
        if (!isshootsilk) return;
        UpdateLineRender();
        if (Vector3.Distance(currentThreadEnd, ropeTarget.position) >= 0.1f) return;
        if (ispull)
        {
            timedelay += Time.deltaTime;
            if (timedelay >= 1)
            {
                Pull();
                timedelay = 0;
            }

        }
        if (isrush)
        {
            Rush();
            return;

        }
    }
    public void PrepareShoot()
    {

        Player.ins.animator.Play("rope");
        collisionObj = PointCenterSceenToWorld.ins.CollisionObj;
        rb = collisionObj.GetComponent<Rigidbody>();
        ropeTarget.position = PointCenterSceenToWorld.ins.targetTransform.position;
        ropeTarget.parent = collisionObj.transform;
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
        currentThreadEnd = startPoint.position;
        lineRenderer.enabled = true;
        isshootsilk = true;

        if (isrush)
        {
            Player.ins.animator.SetBool("IsJump", true);

        }
    }

    public void CheckCollision()
    {
        if (pullColliderMask == (pullColliderMask | (1 << collisionObj.layer)))
        {
            ispull = true;
            isrush = false;
            Player.ins.animator.SetBool("IsRope", false);
        }
        else
        {
            Player.ins.animator.SetBool("IsRope", true);
            ispull = false;
            isrush = true;
        }
    }
    public void UpdateLineRender()
    {
        lineRenderer.positionCount = 2;
        currentThreadEnd = Vector3.MoveTowards(currentThreadEnd, ropeTarget.position, shootsilk * Time.deltaTime);
        lineRenderer.SetPositions(new Vector3[] { startPoint.position, currentThreadEnd });

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
        float angle = Vector3.Angle(transform.position - ropeTarget.position, collisionObj.transform.forward);
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
        FinishRope();
    }
    public void Rush()
    {
        Player.ins.animator.applyRootMotion = false;
        Vector3 direction = ropeTarget.position - transform.position;
        currentspeed = Mathf.MoveTowards(currentspeed, speed, 2f);
        Player.ins.characterController.Move(direction.normalized * currentspeed * Time.deltaTime);
        Quaternion rot = Quaternion.LookRotation(direction);
        transform.rotation = rot;
        CollisionCheck(direction);
    }
    public void FinishRope()
    {
        isrush = false;
        isshootsilk = false;
        ispull = false;
        characterControl.isRope = false;
        lineRenderer.enabled = false;
        Player.ins.animator.SetBool("IsJump", false);
        Player.ins.animator.SetBool("IsRope", false);
    }
    public void RandomRopeType()
    {
        int value = Random.Range(1, 4);
        Player.ins.animator.SetInteger("RopeType", value);
    }
    public void CollisionCheck(Vector3 direction)
    {
        RaycastHit hit;
        Debug.DrawRay(startPoint.position, direction * 5f, Color.green);
        if (Physics.Raycast(startPoint.position, direction, out hit, 2f))
        {
            FinishRope();
            Debug.Log("xxx");
        }

    }
}
