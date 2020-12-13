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

    public PlayerController player;

    protected void Start()
    {
        player = GetComponentInParent<PlayerController>();
        fireRate = 60 / fireRate;
    }

    protected virtual void Update()
    {

    }

    abstract protected void Shoot();
    abstract protected void Reload();

    public void Action()
    {
        //Inventory.instance.AddWeapon(this.gameObject.GetComponent<Weapon>(),Inventory.instance.currentSlot);
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
