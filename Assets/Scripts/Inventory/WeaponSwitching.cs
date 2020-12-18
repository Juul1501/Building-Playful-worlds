using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

public class WeaponSwitching : MonoBehaviourPunCallbacks
{
    public int selectedWeapon = 0;
    private PlayerController player;
    public GameObject selectedWeaponObj;
    void Start()
    {
        player = GetComponentInParent<PlayerController>();
        SelectedWeapon();   
    }



    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;
            else
                selectedWeapon++;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = transform.childCount - 1;
            else
                selectedWeapon--;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)&& transform.childCount >= 2)
        {
            selectedWeapon = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)&& transform.childCount >= 3)
        {
            selectedWeapon = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && transform.childCount >= 4)
        {
            selectedWeapon = 3;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            selectedWeaponObj.GetComponent<Weapon>().Respawn();
        }

        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectedWeapon();
        }
    }

    private void SelectedWeapon()
    {
        int i = 0;
        foreach(Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
                selectedWeaponObj = weapon.gameObject;
                if (player.photonView.IsMine)
                {
                    Hashtable hash = new Hashtable();
                    hash.Add("itemIndex", i);
                    PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
                }
            }
            else 
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(!player.photonView.IsMine && targetPlayer == player.photonView.Owner)
        {
            selectedWeapon = (int)changedProps["itemIndex"];
            SelectedWeapon();
        }
    }
    
}
