using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomListItem : MonoBehaviourPunCallbacks
{
	[SerializeField] TMP_Text text;

	public RoomInfo info;
	[PunRPC]
	public void SetUp(RoomInfo info)
	{
		this.info = info;
		text.text = info.Name;
	}

	public void OnClick()
	{
		Launcher.instance.JoinRoom(info);
	}
}
