using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class NPCDriver : MonoBehaviour
{

    [Header("Control")]
    public NPCControl npcControl;
    public FindThePath findThePath;
    public bool ischangedirection;
    [Header("State")]
    public bool candriver;
    public bool runaway;
    public bool pursue;
    float forward = 1;
    [Header("Vehicle")]
    public GameObject Vehiclel;
    private Motor motor;
    private Car car;
    public float currentspeed;
    public float steeringAngle;
    public VehicleSensor sensor;
    private float timedelay;


    public void DriverVehicle()
    {
        transform.LookAt(npcControl.pointtarget);
        if (npcControl.chacractorData.vehicle == SelectVehicles.Car)
        {
            GetInCar();
            Debug.Log(" Motor");
        }
        else if (npcControl.chacractorData.vehicle == SelectVehicles.Motor)
        {
            GetInMotor();

        }
        //else
        //{
        //    npcControl.npcState.ChangeState(SelectState.Move);
        //}
        Driver();
    }

    public void GetInCar()
    {
        if (Vehiclel == null)
        {

            Vehiclel = Instantiate(npcControl.chacractorData.vehiclePrefab, transform.position, transform.rotation);
            //Vehiclel.transform.LookAt(npcControl.pointtarget);
            car = Vehiclel.GetComponent<Car>();
            sensor = car.sensor;

        }
        else
        {
            Vehiclel.SetActive(true);
            Vehiclel.transform.position = transform.position;
            Vehiclel.transform.LookAt(npcControl.pointtarget);

        }
        transform.parent = car.transform;
        transform.position = car.enterFormPos[0].position;
        transform.eulerAngles = car.transform.eulerAngles + new Vector3(0f, 90f, 0f);
        npcControl.animator.Play("GetCarLeft");

    }
    public void GetInMotor()
    {
        if (motor == null)
        {
            Vehiclel = Instantiate(npcControl.chacractorData.vehiclePrefab, transform.position, Quaternion.LookRotation(npcControl.pointtarget.position));
            motor = Vehiclel.GetComponent<Motor>();
            sensor = motor.sensor;
        }
        else
        {
            motor.gameObject.SetActive(true);
        }
        transform.parent = motor.transform;
        transform.position = motor.enterFormPos[0].position;
        transform.eulerAngles = motor.transform.eulerAngles + new Vector3(0f, 90f, 0f);
        npcControl.animator.Play("GetCarLeft");

    }
    public void Driver()
    {

        StartCoroutine(DriverCourotine());
    }
    IEnumerator DriverCourotine()
    {

        while (candriver)
        {
            DriverManager();
            findThePath.PathProgress(npcControl.pointtarget, Vehiclel.transform);
            if (npcControl.chacractorData.vehicle == SelectVehicles.Car)
            {
                DriverCar();
            }
            else if (npcControl.chacractorData.vehicle == SelectVehicles.Motor)
            {
                DriverMotor();
            }
            //if (Vector3.Distance(transform.position, Player.ins.transform.position) >= NPCManager.ins.maxdistance)
            //{

            //    NPCManager.ins.npcPooling.ReturnPool(gameObject, 0.5f);
            //    candriver = false;
            //    yield break;
            //}
            if (Vector3.Distance(transform.position, npcControl.pointtarget.position) < 5f)
            {
                NextPoint();
            }

            yield return null;
        }
    }
    public void DriverManager()
    {

        if (sensor.collisionwithhuman || sensor.collisionwithvehicles || sensor.collisionwithobject)
        {

            if (runaway)
            {
                currentspeed = npcControl.chacractorData.maxspeed;

                timedelay += Time.deltaTime;
                if (timedelay > 2 && !ischangedirection)
                {

                    timedelay = 0;
                    StartCoroutine(ChangeDirection());
                }


            }
            else
            {
                currentspeed = 0;
            }
        }
        else
        {
            if (runaway)
            {
                currentspeed = npcControl.chacractorData.maxspeed;
            }
            else
            {
                currentspeed = npcControl.chacractorData.speed;
            }




        }
        Vector3 relativeVector = Vehiclel.transform.InverseTransformPoint(findThePath.PostionToFollow);
        float dis = Vector3.Distance(Vehiclel.transform.position, npcControl.pointtarget.position);
        steeringAngle = (relativeVector.x / relativeVector.magnitude);

        currentspeed -= currentspeed * Mathf.Clamp(steeringAngle, 0, 0.5f);
        if (pursue)
        {
            runaway = true;
        }


    }

    public void DriverCar()
    {
        car.MoveVehicle(forward, currentspeed);
        car.UpdateVehicleSteering();
        car.VehicleSteering(steeringAngle * npcControl.chacractorData.speedRotate * forward);
    }
    public void DriverMotor()
    {
        motor.VerticalMove(forward, currentspeed);
        motor.HorizontalMove(steeringAngle * npcControl.chacractorData.speedRotate * forward);
        motor.TiltingToMotorcycle(0, steeringAngle * 0.5f * forward);

    }
    public void NextPoint()
    {
        Debug.Log("nextpoint");

        if (npcControl.pointtarget.gameObject.GetComponent<PointAIMove>() != null)
        {
            npcControl.pointtarget = npcControl.pointtarget.gameObject.GetComponent<PointAIMove>()._nextpoint;
        }
        else if (pursue)
        {

            GetOutVehicle();
        }
    }
    IEnumerator ChangeDirection()
    {
        ischangedirection = true;
        findThePath.ClearWayPoint();
        forward = -1;
        yield return new WaitForSeconds(2f);
        forward = 1;

        ischangedirection = false;

    }
    public void GetOutVehicle()
    {

        candriver = false;
        StartCoroutine(StopVehicles());

    }
    IEnumerator StopVehicles()
    {
        while (pursue)
        {
            if (npcControl.chacractorData.vehicle == SelectVehicles.Car)
            {
                car.MoveVehicle(forward, 0);
                if (car.rigidbodycar.velocity.magnitude <= 0.1f)
                {
                    pursue = false;
                }
            }
            else if (npcControl.chacractorData.vehicle == SelectVehicles.Motor)
            {
                motor.VerticalMove(forward, 0);
                if (motor.rigidbodymotor.velocity.magnitude <= 0.1f)
                {
                    pursue = false;
                }
            }
            yield return null;
        }
        transform.parent = null;
        npcControl.animator.SetTrigger("GetOutVehicle");
       

        //Invoke("EndDeadVehicle", 4f);
    }

    public void EndDeadVehicle()
    {       
       
        npcControl.animator.SetBool("DeadOutVehicle", false);

        npcControl.npcState.ChangeState(SelectState.Attack);
    }


}

