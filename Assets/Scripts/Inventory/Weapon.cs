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
        if(GetComponent<PhotonView>() != null)
        {
            transform.parent = sender.GetComponentInChildren<WeaponSwitching>().transform;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Collider>().enabled = false;
            transform.localPosition = offset;
            transform.rotation = Camera.main.transform.rotation;
            Destroy(GetComponent<PhotonView>());
            weaponState = ItemState.Equiped;
        }
    }
    public void Respawn()
    {
        if (!player.photonView.IsMine)
            return;
        gameObject.AddComponent<PhotonView>();
        GameObject weapon = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", weaponName),transform.position,transform.rotation);
        weapon.transform.position = Camera.main.transform.position + Camera.main.transform.forward*2;
        weapon.transform.GetComponent<Rigidbody>().isKinematic = false;
        weapon.transform.GetComponent<Collider>().enabled = true;
        weapon.transform.GetComponent<Weapon>().weaponState = ItemState.Dropped;
        Destroy(this.gameObject);

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        switch (weaponState) {

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
}
