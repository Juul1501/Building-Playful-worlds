using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    public Camera cam;

    void Update()
    {
        if(Input.GetButtonDown("Fire 1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range);
        if (hit.transform.GetComponent<IDamageable<float>>() != null )
        {
            IDamageable<float> hitObj = hit.transform.GetComponent<IDamageable<float>>();
            hitObj.Damage(damage);
        }

    }
}
