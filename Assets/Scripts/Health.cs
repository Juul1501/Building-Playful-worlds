using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class Health : Target, IPunObservable,IDamageable<RaycastHit>
{
    protected override void Die()
    {
        PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefabs", "explosion"), transform.position, transform.rotation);
        health = 100f;
        transform.position = GameManager.instance.spawnpoint.position;
    }

    public void Damage(float damage, RaycastHit hit)
    {
        photonView.RPC("TakeDamage", RpcTarget.AllBuffered, damage, hit.normal);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
        }
        else
        {
            health = (float)stream.ReceiveNext();
        }
    }
}
