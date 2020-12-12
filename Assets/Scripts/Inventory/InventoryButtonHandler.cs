using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryButtonHandler : MonoBehaviour
{
    private PlayerController player;
    private InventoryController inventoryController;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        inventoryController = player.GetComponent<InventoryController>();
        player.enabled = false;
    }
    
    public void HandleClick()
    {
        Inventory.instance.removeByName(transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
        player.enabled = true;
        inventoryController.SetInventoryVisible(false);
    }
}
