using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
public enum ItemState {Dropped,Equiped}
public abstract class Weapon : MonoBehaviourPunCallbacks, IInteractable,IPunObservable
{
    public string weaponName;
    public int damage;
    public float range;
    public int ammo;
    public int magSize;
    public float fireRate;
    public float reloadTime;
    public Vector3 offset;
    public ItemState weaponState = ItemState.Equiped;
    protected bool isReloading;
    protected float nextTimeToFire = 0;

    public PlayerController player;

    protected void Start()
    {
        player = GetComponentInParent<PlayerController>();
    }

    protected virtual void Update()
    {

    }

    abstract protected void Shoot();
    abstract protected void Reload();

    public void Action(GameObject sender)
    {
        if(GetComponent<PhotonView>() != null && weaponState == ItemState.Dropped)
        {
            EquipWeapon(sender); 
        }
    }
    [PunRPC]
    public void Respawn()
    {
        if (PhotonNetwork.IsMasterClient &&player.photonView.IsMine)
        {
            var go = PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefabs",weaponName), transform.position + transform.forward.normalized* 2, transform.rotation);
            go.GetComponent<Weapon>().weaponState = ItemState.Dropped;
            go.GetComponent<Rigidbody>().isKinematic = false;
            go.GetComponent<Collider>().enabled = true;
        }
        if (photonView.IsMine)
        {
            gameObject.AddComponent<PhotonView>();
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        switch (weaponState) 
        {
            case ItemState.Dropped:

                if (stream.IsWriting)
                {
                    stream.SendNext(transform.position);
                    stream.SendNext(transform.rotation);
                }
                else
                {
                    transform.position = (Vector3)stream.ReceiveNext();
                    transform.rotation = (Quaternion)stream.ReceiveNext();
                }

            break;
        }
    }

    protected virtual void EquipWeapon(GameObject sender)
    {
        player = sender.GetComponent<PlayerController>();
        WeaponSwitching weaponswitch = player.GetComponentInChildren<WeaponSwitching>();
        transform.parent = weaponswitch.transform;
        transform.localPosition = offset;
        transform.rotation = Camera.main.transform.rotation;
        weaponState = ItemState.Equiped;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = false;
    }
}
