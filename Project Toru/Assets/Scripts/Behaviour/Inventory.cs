using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory
{
    //Assign a Item List
    private HashSet<Item> inv;

    //Create's a static InventoryUI manager
    //private static InventoryUI INVUI = new InventoryUI();

    public float MaxWeight;

    private Character ch;
	
    //Constructor - Creates a Item List
    public Inventory(float MaxWeight)
    {
        this.MaxWeight = MaxWeight;
        inv = new HashSet<Item>();
    }

    public void SetMaxWeight(float MaxWeight)
    {
        this.MaxWeight = MaxWeight;
    }

    //Add an item to the Inventory
    public void addItem(Item item)
    {
        if (inv.Count < InventoryUI.Instance().allSlots && (getWeightOfInventory() + item.Weight) <= MaxWeight)
        {
            bool Found = false;
            if (item.isStackable)
            {
                foreach (Item i in inv)
                {
                    if (i.GetType().Equals(item.GetType()) && !Found)
                    {
                        i.value += item.value;
                        Found = true;
                    }
                }
            }

            if (!Found)
            {
                inv.Add(item);
            }

            UpdateUI();
        }
    }

    //Removes an item from the inventory
    public void removeItem(Item item)
    {
        inv.Remove(item);
        UpdateUI();
    }

    //Get the list of Items
    public HashSet<Item> getItemsList()
    {
        return inv;
    }

    //If needed show the UI
    public void UpdateUI()
    {
        if (inv.Count == 0)
        {
            InventoryUI.Instance().hideInv(inv);
        }
        else
        {
            InventoryUI.Instance().showInv(inv);
        }
    }

    private float getWeightOfInventory()
    {
        float currentWeight = 0;

        foreach (Item i in inv)
        {
            currentWeight += i.Weight;
        }

        return currentWeight;
    }

    public float getMoney()
    {
        foreach (Item i in inv)
        {
            if (i.GetType().Name == "Money")
            {
                return i.value;
            }
        }
        return 0;
    }
}
