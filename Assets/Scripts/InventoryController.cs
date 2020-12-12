using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SetInventoryVisible(!InventoryUI.instance.gameObject.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Inventory.instance.SelectWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Inventory.instance.SelectWeapon(1);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            Inventory.instance.DropWeapon(0);
        }
    }
    public void SetInventoryVisible(bool value)
    {
        InventoryUI.instance.gameObject.SetActive(value);
        GetComponent <PlayerController>().enabled = !value;
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
