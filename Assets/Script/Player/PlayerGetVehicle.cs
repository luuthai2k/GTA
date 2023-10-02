using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PlayerGetVehicle : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public GameObject vehicle;
    public List<Transform> enterFormPos;  
    public LayerMask obstacleMask;
    public int  side;
    public GameObject player;
    private void Start()
    {
        Player.ins.playerControl.characterControl.eventGetInVehicles += MoveToDoor;
    }
    //private void Update()
    //{
    //    navMeshAgent.SetDestination(enterFormPos[side].position);
    //}
    public Transform CheckEnterFromPos()
    {
        
        if (enterFormPos.Count > 1)
        {
            var distance1 = Vector3.Distance(player.transform.position, enterFormPos[0].position);
            var distance2 = Vector3.Distance(player.transform.position, enterFormPos[1].position);
            if (distance1 <= distance2)
            {
                side = 0;
                return enterFormPos[0];


            }
            else
            {
                side = 1;
                return enterFormPos[1];
            }
        }
        
        else if(enterFormPos.Count ==1)
        {
            side = 0;
            return enterFormPos[0];
           
        }
        else
        {
            return null;
        }
    }
    public void MoveToDoor()
    {
        Player.ins.characterController.enabled = false;
        navMeshAgent.enabled = true;
        vehicle = PlayerSensor.ins.VehiclesCollision;
        TakeEnterFromPos();
        Transform enterPos = CheckEnterFromPos();
        if (enterPos == null)
        {
            GetInVehicle();
        }
        else
        {
            StartCoroutine(MoveToDoorCouroutine(enterPos));
        }
      

    }
    public void TakeEnterFromPos()
    {
        if (vehicle.CompareTag("Car"))
        {
            enterFormPos = PlayerSensor.ins.VehiclesCollision.GetComponent<Car>().enterFormPos;
        }
        if (vehicle.CompareTag("Motor"))
        {
            enterFormPos = PlayerSensor.ins.VehiclesCollision.GetComponent<Motor>().enterFormPos;
            vehicle.transform.rotation = Quaternion.identity;
        }
        if (vehicle.CompareTag("Tank"))
        {
            enterFormPos = PlayerSensor.ins.VehiclesCollision.GetComponent<Tank>().enterFormPos;

        }
        if (vehicle.CompareTag("Helicopter"))
        {
            enterFormPos = PlayerSensor.ins.VehiclesCollision.GetComponent<Helicopter>().enterFormPos;

        }
    }
    IEnumerator MoveToDoorCouroutine(Transform enterPos)
    {

        //Quaternion targetRotation = Quaternion.LookRotation((enterPos.position - player.transform.position).normalized, Vector3.up);
        while (Vector3.Distance(new Vector3(player.transform.position.x, 0, player.transform.position.z), new Vector3(enterPos.position.x,0, enterPos.position.z)) > 1f)
        {
            navMeshAgent.SetDestination(enterFormPos[side].position);
            //Debug.Log(enterPos.position + "and" + enterFormPos[side].position);
            //if (Player.ins.joystick.Horizontal != 0 || Player.ins.joystick.Vertical != 0)
            //{
            //    // Thoát khỏi coroutine nếu người chơi đang di chuyển (joystick không ở trạng thái tĩnh)
            //    yield break;
            //}
            //Vector3 direction = (enterPos.position - player.transform.position).normalized;
            //Ray ray = new Ray(player.transform.position + Vector3.up, direction);
            //Debug.DrawRay(ray.origin, ray.direction * 1f, Color.red);
            //RaycastHit hit;
            //Debug.Log("turn");
            //if (!Physics.Raycast(ray, out hit, 0.5f, obstacleMask))
            //{
            //    targetRotation = Quaternion.LookRotation(direction, Vector3.up);            

            //}
            //else
            //{
            //    Debug.Log("collision");
            //    Ray raycheck = new Ray(player.transform.position + Vector3.up, targetRotation * Vector3.forward);
            //    RaycastHit hitcheck;
            //    if (Physics.Raycast(raycheck, out hitcheck, 3f, obstacleMask))
            //    {
            //        Vector3 avoidDirection = FindAvoidDirection(targetRotation * Vector3.forward);
            //        Debug.Log(avoidDirection);
            //        targetRotation = Quaternion.LookRotation(avoidDirection, Vector3.up);
            //        Debug.Log("newdirection");
            //    }
            //}
            //targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
            //player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, 5f * Time.deltaTime);
            Player.ins.animator.SetFloat("Forward", 1);


            yield return null;
        }
        navMeshAgent.enabled = false;
        GetInVehicle();


    }
    private Vector3 FindAvoidDirection(Vector3 direction)
    {
        Vector3 avoidDirection = direction+Vector3.left;
        float minAngleDifference = float.MaxValue; // Lưu góc nhỏ nhất giữa hai hướng
        int raysCount = 8;
        float angleStep = 360f / raysCount;

        for (int i = 0; i < raysCount; i++)
        {
            Vector3 rayDirection = Quaternion.Euler(0f, angleStep * i, 0f) * direction;
            Ray ray = new Ray(player.transform.position + Vector3.up, rayDirection);
            Debug.DrawRay(ray.origin, ray.direction * 1f, Color.green);
            if (!Physics.Raycast(ray, 5f, obstacleMask))
            {
                // Tính góc giữa hai hướng
                float angleDifference = Vector3.Angle(direction, rayDirection);

                // Nếu góc giữa hai hướng nhỏ hơn góc nhỏ nhất đã tìm thấy
                if (angleDifference < minAngleDifference)
                {
                    minAngleDifference = angleDifference;
                    avoidDirection = rayDirection;
                }
            }
        }

        return avoidDirection;
    }
    public void GetInVehicle()
    {

        vehicle.GetComponent<Rigidbody>().isKinematic = false;
        player.transform.parent = vehicle.transform;
        if (vehicle.CompareTag("Car"))
        {
            Player.ins.ChangeControl(1);
            Player.ins.playerDriverCar.car = vehicle.GetComponent<Car>();
            Player.ins.playerDriverCar.GetInCar(enterFormPos[side]);
            player.transform.parent = vehicle.GetComponent<Car>().driverSit;
        }
        if (vehicle.CompareTag("Motor"))
        {
            Player.ins.ChangeControl(2);
            Player.ins.playerDriverMotor.motor = vehicle.GetComponent<Motor>();
            Player.ins.playerDriverMotor.GetInMotor(enterFormPos[side],side);
        }
        if (vehicle.CompareTag("Tank"))
        {
            Player.ins.ChangeControl(3);
            Player.ins.playerDriverTank.tank = vehicle.GetComponent<Tank>();
            Player.ins.playerDriverTank.GetInTank();
        }
        if (vehicle.CompareTag("Helicopter"))
        {
            Player.ins.ChangeControl(4);
            Player.ins.playerDriverHelicopter.helicopter = vehicle.GetComponent<Helicopter>();
            Player.ins.playerDriverHelicopter.GetInHelicopter(enterFormPos[side], side);
        }
    }    
    public void GetOutVehicle()
    {
        CameraManager.ins.ChangeCam(0, transform);
        player.transform.parent = null;
        if (vehicle.CompareTag("Car"))
        {
            Player.ins.playerDriverCar.GetOutCar();
        }
        if (vehicle.CompareTag("Motor"))
        {
            Player.ins.playerDriverMotor.GetOutMotor();
        }
        if (vehicle.CompareTag("Tank"))
        {
            Player.ins.playerDriverTank.GetOutTank();
        }
        if (vehicle.CompareTag("Helicopter"))
        {
            Player.ins.playerDriverHelicopter.GetOutHelicopter();
        }
    }
   
   
}
