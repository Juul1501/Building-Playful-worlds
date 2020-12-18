using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;

public class Launcher : MonoBehaviourPunCallbacks
{
	public static Launcher instance;

	[SerializeField] TMP_InputField roomNameInputField;
	[SerializeField] TMP_InputField nameInputField;
	[SerializeField] TMP_InputField createRoomNameInputField;
	[SerializeField] TMP_Text errorText;
	[SerializeField] TMP_Text roomNameText;
	[SerializeField] TMP_Text PlayerNameText;
	[SerializeField] Transform roomListContent;
	[SerializeField] GameObject roomListItemPrefab;
	[SerializeField] Transform playerListContent;
	[SerializeField] GameObject PlayerListItemPrefab;
	[SerializeField] GameObject startGameButton;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(this);
		}
	}

	void Start()
	{
		Debug.Log("Connecting to Master");
		PhotonNetwork.ConnectUsingSettings();
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log("Connected to Master");
		PhotonNetwork.JoinLobby();
		PhotonNetwork.AutomaticallySyncScene = true;
	}

	public override void OnJoinedLobby()
	{
		MenuManager.instance.OpenMenu("title");
		Debug.Log("Joined Lobby");
	}

	public void CreateRoom()
	{
		if (string.IsNullOrEmpty(roomNameInputField.text))
		{
			return;
		}
		if (string.IsNullOrEmpty(createRoomNameInputField.text))
		{
			return;
		}

		PhotonNetwork.CreateRoom(roomNameInputField.text);
		PhotonNetwork.NickName = createRoomNameInputField.text;
		MenuManager.instance.OpenMenu("loading");
	}

	public override void OnJoinedRoom()
	{
		MenuManager.instance.OpenMenu("room");
		roomNameText.text = PhotonNetwork.CurrentRoom.Name;

		Player[] players = PhotonNetwork.PlayerList;

		foreach (Transform child in playerListContent)
		{
			Destroy(child.gameObject);
		}

		for (int i = 0; i < players.Count(); i++)
		{
			Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
		}

		startGameButton.SetActive(PhotonNetwork.IsMasterClient);
	}

	public override void OnMasterClientSwitched(Player newMasterClient)
	{
		startGameButton.SetActive(PhotonNetwork.IsMasterClient);
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		errorText.text = "Room Creation Failed: " + message;
		Debug.LogError("Room Creation Failed: " + message);
		MenuManager.instance.OpenMenu("error");
	}

	public void StartGame()
	{
		PhotonNetwork.LoadLevel(1);
	}

	public void LeaveRoom()
	{
		PhotonNetwork.LeaveRoom();
		MenuManager.instance.OpenMenu("loading");
	}

	public void JoinRoom(RoomInfo info)
	{
		if (string.IsNullOrEmpty(nameInputField.text))
		{
			return;
		}
		PhotonNetwork.NickName = nameInputField.text;
		PhotonNetwork.JoinRoom(info.Name);
		MenuManager.instance.OpenMenu("loading");
	}

	public override void OnLeftRoom()
	{
		MenuManager.instance.OpenMenu("title");
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		foreach (Transform trans in roomListContent)
		{
			Destroy(trans.gameObject);
		}

		for (int i = 0; i < roomList.Count; i++)
		{
			if (roomList[i].RemovedFromList)
				continue;
			var roomlistitem = Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>();
			roomlistitem.SetUp(roomList[i]);
			photonView.RPC("SetUp", RpcTarget.OthersBuffered, roomList[i]);
		}
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
	}
}
