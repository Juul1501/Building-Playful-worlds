using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEngine.UI;

public class Health : Target, IPunObservable,IDamageable<RaycastHit>
{
    public Camera hudCamera;
    public Slider healthBar;

    protected override void Die()
    {
        PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefabs", "explosion"), transform.position, transform.rotation);
        health = 100f;
        transform.position = GameManager.instance.spawnpoint[Random.Range(0, GameManager.instance.spawnpoint.Length)].position;
    }

    public void Damage(float damage, RaycastHit hit)
    {
        photonView.RPC("TakeDamage", RpcTarget.AllBuffered, damage, hit.normal);
        photonView.RPC("Hit", RpcTarget.Others);
    }
    public void Start()
    {
        healthBar = GetComponentInChildren<Slider>();
        healthBar.maxValue = health;
    }
    public void Update()
    {
        healthBar.value = health;
        if (health < 100)
            health += 2 * Time.deltaTime;
    }

    [PunRPC]
    void Hit()
    {
        StartCoroutine(indicateDamage());
    }

    private IEnumerator indicateDamage()
    {
        hudCamera.enabled = true;
        yield return new WaitForSeconds(0.2f);
        hudCamera.enabled = false;
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
