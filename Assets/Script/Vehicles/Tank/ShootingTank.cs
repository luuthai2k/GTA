using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTank : MonoBehaviour
{
    public GameObject bullet;
    public List<GameObject> bulletPool = new List<GameObject>();
    public Transform barrelOut;
    public float speed;
  
    Ray ray;
    void Start()
    {
        
    }
    private void Update()
    {
        
        ray.origin = barrelOut.transform.position;
        ray.direction = barrelOut.transform.forward;

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
        {
           

        }
    }
    public void Shooting()
    {
        GameObject bulletvalue = GetBullet(barrelOut.position);
        //bulletvalue.transform.parent = barrelOut.transform;
        bulletvalue.GetComponent<Rigidbody>().AddForce(barrelOut.transform.forward * speed);
    }
    public GameObject GetBullet(Vector3 pos)
    {
        foreach (GameObject bullet in bulletPool)
        {
            GameObject prefabbullet = bullet.GetComponent<RocketShoot>().gameObject;
            if (!prefabbullet.activeInHierarchy)
            {
                prefabbullet.SetActive(true);
                bullet.transform.position = pos;
                return bullet;
            }
        }
        GameObject newbag = Instantiate(bullet, pos, Quaternion.identity);
        bulletPool.Add(newbag);
        return newbag;
    }

    public void ReturnBullet(GameObject bag, float time)
    {
        
    }
}
