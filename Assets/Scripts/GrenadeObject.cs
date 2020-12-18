using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GrenadeObject : MonoBehaviourPunCallbacks
{
    public float damageRange;
    public void Start()
    {
        StartCoroutine(Explode(4, 100));
    }
    public IEnumerator Explode(float explodeTime,float damage)
    {
        yield return new WaitForSeconds(explodeTime);
        PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefabs", "explosion"), transform.position, transform.rotation);
        ExplodeGrenade(damage);
        Destroy(this.gameObject);
    }

    public void ExplodeGrenade(float damage)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRange);
        foreach (Collider collider in colliders)
        {
            if (collider.GetComponent<IDamageable<GameObject>>() != null)
            {
                IDamageable <GameObject> hitobj = collider.GetComponent<IDamageable<GameObject>>();
                hitobj.Damage(damage, this.gameObject);
            }
        }
    }
}
