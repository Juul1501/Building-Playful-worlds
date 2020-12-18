using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviourPunCallbacks,IInteractable
{
    float health = 25;
    public int i;
    public void Action(GameObject sender)
    {
        sender.GetComponent<Health>().health += health;
        GameManager.instance.photonView.RPC("RespawnPickup",RpcTarget.All,i);
        GameManager.instance.DestroySceneObject(photonView);
    }
}
