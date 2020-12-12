using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public float maximumWeight = 10.0f;
    public float totalWeight;
    public Weapon[] weapons = new Weapon[2];
    private List<Item> items;
    private Weapon currentWeapon;
    public GameObject player;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
        items = new List<Item>();
    }

    public bool AddItem(Item item)
    {
        if (totalWeight + item.weight > maximumWeight)
        {
            return false;
        }
        else
        {
            items.Add(item);
            InventoryUI.instance.Add(item);
            totalWeight += item.weight;
            return true;
        }
    }

    public void removeItem(Item item)
    {
        if (items.Remove(item))
        {
            InventoryUI.instance.Remove(item);
            totalWeight -= item.weight;
        }
    }

    public void removeByName(string name)
    {
        foreach (Item i in items)
        {
            if (i.name == name)
            {
                removeItem(i);
                break;
            }
        }
    }

    public bool AddWeapon(Weapon weapon)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] != null)
            {
                return false;
            }
            else
            {
                weapons[i] = weapon;
                Weapon w = weapons[i].GetComponent<Weapon>();
                w.transform.parent = Camera.main.transform;
                w.transform.localPosition = w.offset;
                w.transform.rotation = Camera.main.transform.rotation;

                w.gameObject.SetActive(false);
                return true;
            }
        }
        return false;
    }

    public void SelectWeapon(int slot)
    {
        if(slot < weapons.Length && currentWeapon != weapons[slot] && weapons[slot] != null)
        {
            currentWeapon = weapons[slot];
            currentWeapon.GetComponent<Collider>().enabled = false;
            currentWeapon.GetComponent<Rigidbody>().isKinematic = true;
            currentWeapon.isEquiped = true;
            currentWeapon.gameObject.SetActive(true);
        }
    }

    public void DropWeapon(int slot)
    {
        currentWeapon.Respawn();
        currentWeapon = null;
        weapons[slot] = null;
    }
}
