using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IInteractable
{
    public int damage;
    public float range;
    public int ammo;
    public int magSize;
    public float fireRate;
    public float reloadTime;
    public Vector3 offset;

    protected bool isReloading;
    protected float nextTimeToFire = 0;
    public bool isEquiped;

    protected void Start()
    {

    }

    protected virtual void Update()
    {
        if (!isEquiped)
            return;

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + fireRate;
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    abstract protected void Shoot();
    abstract protected void Reload();

    public void Action()
    {
        Inventory.instance.AddWeapon(this.gameObject.GetComponent<Weapon>(),Inventory.instance.currentSlot);
    }
    public void Respawn()
    {
        transform.position = Camera.main.transform.position + Camera.main.transform.forward*2;
        gameObject.SetActive(true);
        transform.parent = null;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Collider>().enabled = true;
        isEquiped = false;
    }
}
