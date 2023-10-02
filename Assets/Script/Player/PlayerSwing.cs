using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using DG.Tweening;
using Cinemachine;

public class PlayerSwing : MonoBehaviour
{
    private Tween myTween;
    float turnCalmVelocity;
    [SerializeField] float turnCalmTime = 1f;
    [SerializeField] private PlayerControl playerControl;
    [SerializeField] private float heightMax;
    [SerializeField] private LineRenderer lineRenderer;
    SpringJoint joint;
    [Header("Player Jump")]
    public float speed;
    public float Gravity;
    public float jumpHeight;
    private float _verticalVelocity;
    private bool isjump;
    [Header("Player Swing")]
    [SerializeField] Transform swingStartPoint;
    [SerializeField] Vector3 swingPoint;
    [SerializeField] Transform swingPivot;
    private bool startSwing;
    public float force;
    public float foward;
    public float shootsilk;
    public float damping;
    public float maxAngle;
    private bool isSwing;
    private bool isfall;
    private bool canswing = true;
    private float swing;
    private float currentdamping;
    public void Jump()
    {
        Player.ins.animator.SetBool("IsJump", true);
        _verticalVelocity = Mathf.Sqrt(Mathf.Abs(jumpHeight * Gravity));
    }
    public void StartSwing(CharacterControl characterControl)
    {
        
        if (Input.GetKey(KeyCode.LeftControl) && canswing || characterControl.isSwing && canswing)
        {
            if (transform.position.y > heightMax) return;
            Player.ins.animator.SetBool("IsSwing", true);
            if (startSwing) return;
            swing = 0;
            isfall = false;
            if (myTween != null)
            {
                myTween.Kill();
               
            }
            swingPoint = swingPivot.position;
            currentdamping = 0f;
            if (playerControl.onSurface)
            {
                Jump();
                isSwing = false;
            }
            startSwing = true;
        }
        else
        {
            if (startSwing)
            {
                startSwing = false;
                canswing = false;
                FinishSwing(0.5f);
            }
            lineRenderer.enabled = false;
            Player.ins.animator.SetBool("IsJump", false);
            Player.ins.animator.SetBool("IsSwing", false);
        }
    }
    public void CanSwing()
    {
        isSwing = true;
        if (GetComponent<SpringJoint>() == null)
        {
            joint = gameObject.AddComponent<SpringJoint>();
        }
        else
        {
            joint.enableCollision = true;
        }
     
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = swingPoint;
        float dis = Vector3.Distance(swingPoint, swingStartPoint.position);
        joint.maxDistance = dis * 0.8f;
        joint.minDistance = dis * 0.5f; ;
        joint.damper = 2;
        joint.massScale = 100f;
        joint.spring = 999f;
        myTween = DOTween.To(() => currentdamping, x => currentdamping = x, 2, damping)
        .SetEase(Ease.InQuad);
        lineRenderer.enabled = true;
        isjump = false;
       

    }
    public void Swing(CharacterControl characterControl)
    {
        StartSwing(characterControl);
        float currenspeed = 0;
        if (isSwing)
        {
            Player.ins.animator.applyRootMotion = false;
            IsSwing();
            Vector3 direction = new Vector3(characterControl.joystick.Horizontal, 0f,characterControl.joystick.Vertical);
            Vector3 directionMove = Vector3.zero;
            Player.ins.animator.SetFloat("Forward", Mathf.Clamp01(direction.magnitude));
            Player.ins.animator.SetFloat("Turn", characterControl.joystick.Horizontal);
            float targetAngle = Mathf.Clamp(Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg,-90,90) + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
            Debug.Log(Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg);
            transform.rotation = Quaternion.Euler(0, angle, 0);
            if (isfall)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
               float currenforce = force *0.6f* Mathf.Clamp(swing,1,2);
                float currenfoward = foward *0.6f* Mathf.Clamp(swing, 1, 1.5f);
                directionMove = Vector3.up  * currenforce  + transform.forward * currenfoward + new Vector3(0.0f, _verticalVelocity, 0.0f);

            }
            else
            {
                swing += Time.deltaTime;

                if (CheckForAngle(swingStartPoint.position,swingPoint,transform.forward))
                {
                    startSwing = false;
                    canswing = false;
                    FinishSwing(1f);

                }
                _verticalVelocity = 0;
                directionMove = (swingPoint - swingStartPoint.position).normalized * force * currentdamping + transform.forward * foward;

            }
            Player.ins.characterController.Move(directionMove*Time.deltaTime);
            return;
        }
        else
        {
           
            Vector3 direction = new Vector3(characterControl.joystick.Horizontal, 0f, characterControl.joystick.Vertical);
            currenspeed = direction.magnitude * speed;
            _verticalVelocity += Gravity * Time.deltaTime;

        }
        Player.ins.characterController.Move(( transform.forward * currenspeed + new Vector3(0.0f, _verticalVelocity, 0.0f)) * Time.deltaTime);
    }
    public void IsSwing()
    {
        if (!isSwing) return;
       
        UpdateLineRender();
       
        Player.ins.animator.SetFloat("Swing", swing);
       
       
    }
    public void UpdateLineRender()
    {

       
        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(new Vector3[] { swingStartPoint.position, swingPoint });
    }
    public void FinishSwing(float time)
    {

        lineRenderer.enabled = false;
        isfall = true;
        StartCoroutine(CouroutineDelaySwing(time));
    }
   IEnumerator CouroutineDelaySwing(float time)
    {
        yield return new WaitForSeconds(time);
        canswing = true;
        startSwing = false;
    }

    private bool CheckForAngle(Vector3 pos, Vector3 source, Vector3 direction) 
    {
        Vector3 distance = (pos - source).normalized;
        float CosAngle = Vector3.Dot(distance, direction);
        float Angle = Mathf.Acos(CosAngle) * Mathf.Rad2Deg;

        if (Angle < maxAngle)
            return true;
        else
            return false;
    }
}