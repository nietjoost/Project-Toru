using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private GameObject inventory;

    public int allSlots;
    private int enabledSlots;
    private GameObject[] slots;

    public GameObject slotHolder;
	
	static InventoryUI _instance = null;
	
	public static InventoryUI Instance() {
		if (_instance == null) {
			_instance = (InventoryUI)FindObjectOfType(typeof(InventoryUI));
		}
		Debug.Assert(_instance != null, "InventoryUI is still null after init");
		
		return _instance;
	}
	
	public void Awake() {
		//Get the objects needed to create the UI
		inventory = GameObject.FindGameObjectWithTag("Inventory");
		slotHolder = GameObject.FindGameObjectWithTag("InventorySlotHolder");
		
		allSlots = slotHolder.transform.childCount;
		slots = new GameObject[allSlots];

		//For loop to set the GameObjects
		for (int i = 0; i < allSlots; i++)
		{
			slots[i] = slotHolder.transform.GetChild(i).gameObject;
		}
		
		// Set instance before it can't be found
		Instance(); 
		
		// Hide Inventory
		inventory.SetActive(false);
	}

    public void showInv(HashSet<Item> inv)
    {
        //Character has his own inventory - Call function to set the inventory to the Inventory
        addInventoryToUI(inv);

		//Update money
		UpdateMoneyUI();

		//Show inventory
		inventory?.SetActive(true);
    }

    public void hideInv(HashSet<Item> inv)
    {
        //Character has his own inventory - Call function to set the inventory to the Inventory
        addInventoryToUI(inv);

        //Hide inventory
        inventory?.SetActive(false);
    }

    public void addInventoryToUI(HashSet<Item> inv)
    {
        List<Item> inv2 = new List<Item>(inv);
        for (int i = 0; i < inv2.Count; i++)
        {
            slots[i].GetComponentInChildren<Image>().sprite = inv2[i].UIIcon;
            slots[i].GetComponent<Mask>().showMaskGraphic = true;
        }
    }

	public void UpdateMoneyUI()
	{
		GameObject.FindGameObjectWithTag("InventoryMoney").GetComponent<TMPro.TextMeshProUGUI>().text = "$" + GetTotalMoney();
	}

	public float GetTotalMoney()
	{
		float totalCash = 0;

		foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player"))
		{
			totalCash += p.GetComponent<Character>().inventory.getMoney();
		}
		return totalCash;
	}
}
