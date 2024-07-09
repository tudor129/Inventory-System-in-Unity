using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeReference] public List<ItemSlotInfo> items = new List<ItemSlotInfo>();

    [Space]
    [Header("Inventory Menu Components")]
    public GameObject inventoryMenu;
    public GameObject itemPanel;
    public GameObject itemPanelGrid;

    List<ItemPanel> existingPanels = new List<ItemPanel>();

    [Space]
    public int invetorySize = 24;

    void Start()
    {
        for (int i = 0; i < invetorySize; i++)
        {
            items.Add(new ItemSlotInfo(null, 0));
        }
        RefreshInventory();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (inventoryMenu.activeSelf)
            {
                inventoryMenu.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                inventoryMenu.SetActive(true);
                Cursor.lockState = CursorLockMode.Confined;
                RefreshInventory();
            }
        }
    }

    public void RefreshInventory()
    {
        existingPanels = itemPanelGrid.GetComponentsInChildren<ItemPanel>().ToList();

        if (existingPanels.Count < invetorySize)
        {
            int amountToCreate = invetorySize - existingPanels.Count;
            for (int i = 0; i < amountToCreate; i++)
            {
                GameObject newPanel = Instantiate(itemPanel, itemPanelGrid.transform);
                existingPanels.Add(newPanel.GetComponent<ItemPanel>());
            }

            int index = 0;
            foreach (ItemSlotInfo i in items)
            {
                // Name our list elements
                i.name = "" + (index + 1);
                if (i.item != null)
                {
                    i.name += ": " + i.item.GiveName();
                }
                else
                {
                    i.name += ": -";
                }
                
                // Update our panels
                ItemPanel panel = existingPanels[index];
                if (panel != null)
                {
                    panel.name = i.name + " Panel";
                    panel.inventory = this;
                    panel.itemSlot = i;
                    if (i.item != null)
                    {
                        panel.itemImage.gameObject.SetActive(true);
                        panel.itemImage.sprite = i.item.GiveItemImage();
                        panel.stacksText.gameObject.SetActive(true);
                        panel.stacksText.text = "" + i.stacks;
                    }
                }
                else
                {
                    panel.itemImage.gameObject.SetActive(false);
                    panel.stacksText.gameObject.SetActive(false);
                }
                index++;
            }
        }
    }

    public int AddItem(Item item, int amount)
    {
        // Check for open spaces in existing slots
        foreach (ItemSlotInfo i in items)
        {
            if (i.item != null)
            {
                if (i.item.GiveName() == item.GiveName())
                {
                    if (amount > i.item.MaxStacks() - i.stacks)
                    {
                        amount -= i.item.MaxStacks() - i.stacks;
                        i.stacks = i.item.MaxStacks();
                    }
                    else
                    {
                        i.stacks += amount;
                        if (inventoryMenu.activeSelf)
                        {
                            RefreshInventory();
                        }
                        return 0;
                    }
                }
            }
        }
        // Fill empty slots with leftover items
        foreach (ItemSlotInfo i in items)
        {
            if (i.item == null)
            {
                if (amount > item.MaxStacks())
                {
                    i.item = item;
                    i.stacks = item.MaxStacks();
                    amount -= item.MaxStacks();
                }
                else
                {
                    i.item = item;
                    i.stacks = amount;
                    if (inventoryMenu.activeInHierarchy)
                    {
                        RefreshInventory();
                    }
                    return 0;
                }
            }
        }
        // No space in the inventory, return remainder items
        Debug.Log("No space in Inventory for: " + item.GiveName());

        if (inventoryMenu.activeSelf)
        {
            RefreshInventory();
        }
        return amount;
    }

    public void ClearSlot(ItemSlotInfo slot)
    {
        slot.item = null;
        slot.stacks = 0;
    }
}
