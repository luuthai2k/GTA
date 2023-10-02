using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using DG.Tweening;
using Cinemachine;

public class PlayerSwing : MonoBehaviour
{
    public CinemachineFreeLook freeLookCamera;
    private Tween myTween;
    float turnCalmVelocity;
    [SerializeField] float turnCalmTime = 1f;
    public PlayerControl playerControl;
    public float heightMax;
    public LineRenderer lineRenderer;
    SpringJoint joint;
    [Header("Player Swing")]
    public float Gravity;
    public float force;
    public float jumpHeight;
    public float _verticalVelocity;
    public float currenforce;
    public float foward;
    public float currenfoward;
    [SerializeField] Transform swingStartPoint;
    [SerializeField] Vector3 swingPoint;
    public bool startSwing;
    public PlayerMove playerMove;
    [SerializeField] Transform swingPivot;
    public bool isjump;
    public float shootsilk;
    public float swing;
    public bool isSwing;
    public bool isfall;
    public bool canswing = true;
    public float speedrotateswing;
    public bool isfinish;
    public float currentdamping;
    public float damping;
    public float maxAngle;
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
        //if (isSwing) return;
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
        //_verticalVelocity = 0;
        myTween = DOTween.To(() => currentdamping, x => currentdamping = x, 3, damping)
           .SetEase(Ease.InCubic);
        lineRenderer.enabled = true;
        isjump = false;
       

    }
    public void HandleInput()
    {
        
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
            //transform.up = directionMove;
            if (isfall)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
                currenforce = 3*force * Mathf.Clamp(swing,1,2);
                currenfoward = 2*foward * Mathf.Clamp(swing, 1, 1.5f);
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
                currenforce = force;
                _verticalVelocity = 0;
                directionMove = (swingPoint - swingStartPoint.position).normalized * currenforce * currentdamping + transform.forward * 3 * foward;
            }
            Player.ins.characterController.Move(directionMove*Time.deltaTime);
            return;
        }
        else
        {
           
            Vector3 direction = new Vector3(characterControl.joystick.Horizontal, 0f, characterControl.joystick.Vertical);
            currenspeed = direction.magnitude * foward;
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
    public void DelaySwing(float time)
    {
      
        StartCoroutine(CouroutineDelaySwing(time));
    }
   IEnumerator CouroutineDelaySwing(float time)
    {
        yield return new WaitForSeconds(time);
        canswing = true;
        startSwing = false;
    }

    private bool CheckForAngle(Vector3 pos, Vector3 source, Vector3 direction) //calculates the angle between the car and the waypoint 
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