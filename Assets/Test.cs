using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Test : MonoBehaviour
{
    public Rigidbody rb;
    public Transform target;
    public float force;
    public float height;
    public float dis;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            AddForce();
        }
    }
    public void AddForce()
    {
        //float dis = Vector3.Distance(transform.position, target.position);
        //Vector3 direction = target.position + Vector3.up * height - transform.position;
        //float angle = Vector3.Angle(new Vector3(direction.x, 0, direction.z), direction);
        //float angleInRadians = angle * Mathf.Deg2Rad;
        //float sinValue = Mathf.Sin(2 * angleInRadians);
        //float _v = Mathf.Sqrt((dis * Physics.gravity.magnitude) / sinValue);
        ////if (collisionObj.GetComponent<Rigidbody>() != null)
        ////{
        ////    rb = collisionObj.GetComponent<Rigidbody>();
        //float forceMagnitude = rb.mass * _v;
        ////    Debug.LogWarning(forceMagnitude);
        ////rb.AddForce(direction.normalized * forceMagnitude, ForceMode.VelocityChange);
        //rb.velocity = direction.normalized * _v;
        ////    ispull = false;
        //    //isshootsilk = false;
        //}
        //ispull = false;
        
        Vector3 _target = target.position - (target.position - transform.position).normalized * dis;
        Vector3 direction = _target - transform.position;
        float distanceToTarget = direction.magnitude;
        float angle = Vector3.Angle(direction, target.forward)-90;

        // Chuyển đổi góc sang radian nếu cần
      
        Debug.Log(angle);
        // Tính toán vận tốc y để ném lên cao 20 đơn vị
        float initialVelocityY = Mathf.Sqrt(2 * height * Mathf.Abs(Physics.gravity.y));

        // Tính toán vận tốc x và z
        float initialVelocityXZ = distanceToTarget / (Mathf.Sqrt(2 * height / Mathf.Abs(Physics.gravity.y)) + Mathf.Sqrt(2 * Mathf.Abs(distanceToTarget - height) / Mathf.Abs(Physics.gravity.y)));

        // Tạo vector vận tốc tổng cộng
        Vector3 throwVelocity = direction.normalized * initialVelocityXZ;
        throwVelocity.y = initialVelocityY;
        float randomRotationX = Random.Range(180f, 360f); // Góc xoay ngẫu nhiên theo trục x
        float randomRotationY = Random.Range(180f, 360f); // Góc xoay ngẫu nhiên theo trục y
        float randomRotationZ = Random.Range(180f, 360f); // Góc xoay ngẫu nhiên theo trục z

        // Tạo một Quaternion từ các góc xoay ngẫu nhiên
        Quaternion randomRotation = Quaternion.Euler(angle, 0, angle-90);
        Vector3 randomTorque = new Vector3(0, 0, angle) *force;
        // Áp dụng góc xoay cho đối tượng
        rb.AddTorque(randomTorque);
        // Gán vận tốc cho Rigidbody
        rb.velocity = throwVelocity;

    }


}


