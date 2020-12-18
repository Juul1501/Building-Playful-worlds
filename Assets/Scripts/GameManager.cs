using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;
    public Transform[] spawnpoint;
    public Transform[] targetPoints;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            if (GetComponent<PhotonView>() == null)
            {
                gameObject.AddComponent<PhotonView>();
            }
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        for (int i = 0; i < targetPoints.Length; i++)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "target"), targetPoints[i].position,targetPoints[i].rotation);
        }
    }
    public void DestroySceneObject(PhotonView photonView)
    {
        if (PhotonNetwork.IsConnected)
        {
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(photonView);
            }
            else
            {
                photonView.RequestOwnership();
                PhotonNetwork.Destroy(photonView);
            }
        }
    }
}
