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

    // Start is called before the first frame update
    void Start()
    {
        weaponSwitch = GetComponentInParent<WeaponSwitching>();
    }

    // Update is called once per frame
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
        rb.AddForce(new Vector3(0, 1, 1) * range,ForceMode.Impulse);
        weaponSwitch.selectedWeapon = 0;
    }


    public void Action(GameObject sender)
    {
        //pickup
    }

}
