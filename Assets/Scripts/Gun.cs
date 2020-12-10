using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public float timeBetweenFire = 0.2f;
    private float nextTimeToFire = 0;
    public Camera cam;

    private void Start()
    {
    }

    void Update()
    {
        if(Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + timeBetweenFire;
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range);
        if (hit.transform.GetComponent<IDamageable>() != null )
        {
            IDamageable hitObj = hit.transform.GetComponent<IDamageable>();
            hitObj.Damage(damage, hit);

        }
        Debug.Log(hit.collider.gameObject.name);

        GameObject obj = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(obj, 2f);
        muzzleFlash.Play();
    }
}
