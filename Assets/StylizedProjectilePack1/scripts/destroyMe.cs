using UnityEngine;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;

public class destroyMe : MonoBehaviourPunCallbacks{

    float timer;
    public float deathtimer = 10;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;

        if(timer >= deathtimer)
        {
            DestroySceneObject(photonView);
        }
	
	}
    

    public static void DestroySceneObject(PhotonView photonView)
    {
        if (PhotonNetwork.IsConnected)
        {
            if (photonView.isRuntimeInstantiated) // instantiated at runtime
            {
                if (photonView.IsMine)
                {
                    PhotonNetwork.Destroy(photonView);
                }
                else
                {
                    photonView.RequestOwnership();
                }
            }
            else // scene view loaded in the scene
            {
                photonView.RPC("LocalSelfDestroy", RpcTarget.AllBuffered);
                //otherPhotonView.RPC("LocalDestroy", RpcTarget.AllBuffered, photonView.ViewID); // another option
            }
        }
        else
        {
            GameObject.Destroy(photonView.gameObject); // photonView.LocalSelfDestroy();
        }
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
    }


    // on the same PhotonView (on all?)

    [PunRPC]
    public void LocalSelfDestroy()
    {
        GameObject.Destroy(photonView);
    }

    // [...] another option if you want to destroy from a single PhotonView available on all clients, similar to M4TT's DestroyRPC

    [PunRPC]
    private void LocalDestroy(int viewId)
    {
        GameObject.Destroy(PhotonView.Find(viewId).gameObject);
    }
}
