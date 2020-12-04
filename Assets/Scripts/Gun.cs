using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public ParticleSystem muzzleFlash;

    public Camera cam;

    private void Start()
    {
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
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
        muzzleFlash.Play();

    }
}
