using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehiclesHp : MonoBehaviour
{
    [SerializeField]
    private float hp;

    [SerializeField]
    private Car car;

    [SerializeField]
    private ParticleSystem explosion;

    [SerializeField]
    private GameObject vehiclesExplosion;

    [SerializeField]
    private GameObject vehiclesNomal;

    [SerializeField]
    private Collider explosionBox;

    public void Start()
    {
        explosion.Stop();

        vehiclesExplosion.SetActive(false);

        vehiclesNomal.SetActive(true);

        explosionBox.enabled = false;

        car.enabled = true;
    }

    public void DameVehicles(float _hp)
    {
        hp -= _hp;

        if(hp <= 0)
        {
            explosion.Play();

            vehiclesExplosion.SetActive(true);

            vehiclesNomal.SetActive(false);

            explosionBox.enabled = true;

            car.enabled = false;
        }
    }
}
