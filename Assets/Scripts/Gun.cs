using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    public Camera cam;
    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
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
            hitObj.Damage(damage, player);
        }
        Debug.Log(hit.collider.gameObject.name);

    }
}
