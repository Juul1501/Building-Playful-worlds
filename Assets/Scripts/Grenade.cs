using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
public class Grenade : MonoBehaviourPunCallbacks, IInteractable
{
    public float range;
    public float damage;

    private WeaponSwitching weaponSwitch;

    void Start()
    {
        weaponSwitch = GetComponentInParent<WeaponSwitching>();
    }

    void Update()
    {
        if (Input.GetButtonUp("Fire1")&& transform.gameObject.activeSelf)
        {
            ThrowGrenade();
            Destroy(this.gameObject);
        }
    }

    void ThrowGrenade()
    {
        GameObject grenade = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "grenade"), transform.position, Quaternion.identity);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        grenade.AddComponent<PhotonView>();
        rb.AddForce(Camera.main.transform.forward.normalized * range,ForceMode.Impulse);
        weaponSwitch.selectedWeapon = 0;
    }


    public void Action(GameObject sender)
    {
        //pickup
    }

}
