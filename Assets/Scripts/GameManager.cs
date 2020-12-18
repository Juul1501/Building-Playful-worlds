using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public enum GameState {WarmUp,Play,End}
public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameManager instance;
    public GameState state;
    public Transform[] spawnpoint;
    public Transform[] targetPoints;
    public float warmupTime = 30;
    public float roundTime = 180;
    public float endScreenTime = 20;

    public Transform[] pickupSpawnPoints;

    public List<GameObject> players;

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
        players = new List<GameObject>();
        if (PhotonNetwork.IsMasterClient)
        {

            for (int i = 0; i < targetPoints.Length; i++)
            {
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "target"), targetPoints[i].position, targetPoints[i].rotation);
            }
            for (int i = 0; i < pickupSpawnPoints.Length; i++)
            {
                var g = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "pickup"), pickupSpawnPoints[i].position, pickupSpawnPoints[i].rotation);
                g.GetComponent<PickupItem>().i = i;
            }
        }
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            switch (state)
            {
                case GameState.WarmUp:
                    warmupTime -= 1 * Time.deltaTime;
                    if(warmupTime <= 0)
                    {
                        StartGame();
                        state = GameState.Play;  
                    }
                    break;
                case GameState.Play:
                    roundTime -= 1 * Time.deltaTime;
                    if(roundTime <= 0)
                    {
                        state = GameState.End;
                    }
                    break;
                case GameState.End:
                    endScreenTime -= 1 * Time.deltaTime;
                    if(endScreenTime <= 0)
                    {
                        //restart
                    }
                    break;
            }
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(roundTime);
            stream.SendNext(warmupTime);
            stream.SendNext(endScreenTime);
            stream.SendNext(state);
        }
        if (stream.IsReading)
        {
            roundTime = (float)stream.ReceiveNext();
            warmupTime = (float)stream.ReceiveNext();
            endScreenTime = (float)stream.ReceiveNext();
            state = (GameState)stream.ReceiveNext();
        }
    }
    public void StartGame()
    {

    }
    [PunRPC]
    public void RespawnPickup(int i)
    {
        StartCoroutine(RespawnPickupItem(i));
    }

    IEnumerator RespawnPickupItem(int i)
    {
        yield return new WaitForSeconds(5);
        if (PhotonNetwork.IsMasterClient)
        {
           var g = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "pickup"), pickupSpawnPoints[i].position, pickupSpawnPoints[i].rotation);
            g.GetComponent<PickupItem>().i = i;
        }
    }
}
