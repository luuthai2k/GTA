using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimb : MonoBehaviour
{
    [SerializeField] Transform topPoint;
    [SerializeField] Transform botPoint;
    public float raydistance = 0.5f;
    public float dis1 = 0.5f;
    public float dis2 = 1f;
    public LayerMask wallMask;
    [SerializeField] Transform wallCheck;
    public float speed;
    public float speedrotate;
    public GameObject rig;
    public bool canclimb;
    public bool isPlane;
    public bool isCornerUp;
    public bool isCornerDown;

    public bool CheckNearWall()
    {
        RaycastHit hit;
        if (Physics.Raycast(wallCheck.position, transform.forward, out hit, raydistance, wallMask))
        {
            transform.rotation = Quaternion.LookRotation(-hit.normal);
            return true;
        }

        return false;

    }

    public void Climb(CharacterControl characterControl)
    {
        Player.ins.animator.applyRootMotion = false;
        if (CanClimb(characterControl.joystick.Horizontal, characterControl.joystick.Vertical))
        {
            Player.ins.animator.SetFloat("Vertical", characterControl.joystick.Vertical);
            Player.ins.animator.SetFloat("Horizontal", characterControl.joystick.Horizontal);
        }
        else
        {
            Player.ins.animator.SetFloat("Vertical", 0);
            Player.ins.animator.SetFloat("Horizontal", 0);

        }
    }

    public bool CanClimb(float h, float v)
    {
        Vector3 _moveDirection = transform.right * h + transform.up * v;
        Vector3 origin = wallCheck.position;
        float _distance = raydistance;
        Vector3 direction = _moveDirection;
        RaycastHit hit;
        Debug.DrawRay(origin, direction, Color.yellow);
        if (Physics.Raycast(origin, direction, out hit, raydistance, wallMask))
        {
            transform.rotation = Quaternion.LookRotation(-hit.normal);
            Player.ins.characterController.Move(_moveDirection * speed * Time.deltaTime);

            return true;
        }

        origin += _moveDirection * Mathf.Sqrt(Mathf.Pow(dis1 * h, 2) + Mathf.Pow(dis2 * v, 2));
        direction = transform.forward;

        Debug.DrawRay(origin, direction, Color.yellow);
        if (Physics.Raycast(origin, direction, out hit, raydistance, wallMask))
        {
            transform.rotation = Quaternion.LookRotation(-hit.normal);
            Player.ins.characterController.Move(_moveDirection * speed * Time.deltaTime);
            return true;
        }

        origin += direction * dis2;
        direction = -Vector3.up;
        Debug.DrawRay(origin, direction, Color.yellow);
        if (Physics.Raycast(origin, direction, out hit, raydistance, wallMask))
        {
            //Player.ins.animator.SetBool("UptoGround", true);
            //transform.position = hit.point+Vector3.up*0.5f;
            //tr/*ansform.rotation = Quaternion.LookRotation(-hit.normal);*/
            //Player.ins.animator.SetBool("UptoGround", true);
            return false;

        }
        direction = -_moveDirection;
        Debug.DrawRay(origin, direction, Color.yellow);
        if (Physics.Raycast(origin, direction, out hit, raydistance, wallMask))
        {
            transform.rotation = Quaternion.LookRotation(-hit.normal);
            transform.position = hit.point + hit.normal * 0.3f;
            //Player.ins.characterController.Move(_moveDirection * speed * Time.deltaTime);
            return true;
        }
        return true;

    }
    IEnumerator JumpUptoGround(Vector3 postarget)
    {
        while (Vector3.Distance(transform.position, postarget) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, postarget, speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
            Player.ins.animator.SetBool("UptoGround", true);

        }
        yield return null;
    }
}
