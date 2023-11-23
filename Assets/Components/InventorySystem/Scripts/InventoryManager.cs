using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    // these variables are Serialized Fields
    // this means they can be private since they are not accessed by other scripts
    // but still be visible in the inspector

    [SerializeField] private GameObject slotHolder;
    [SerializeField] private ItemClass itemToAdd;
    [SerializeField] private ItemClass itemToRemove;

    // array for starting items (for player or for testing)
    [SerializeField] private SlotClass[] startingItems;

    private SlotClass[] items;

    private GameObject[] slots;

    #region Moving Items Variables
    [SerializeField] private GameObject itemCursor;
    private SlotClass movingSlot;
    private SlotClass tempSlot;
    private SlotClass originSlot;
    bool isMovingItem;

    #endregion Moving Items Variables

    private void Start()
    {
        slots = new GameObject[slotHolder.transform.childCount];
        items = new SlotClass[slots.Length];



        // setup starting items
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = new SlotClass();
        }

        // initialize all of the slots
        for (int i = 0; i < startingItems.Length; i++)
        {
            items[i] = startingItems[i];
        }

        // set all the slots
        for (int i = 0; i < slotHolder.transform.childCount; i++)
            slots[i] = slotHolder.transform.GetChild(i).gameObject;

        RefreshUI();

        //testing add and remove functions
        Add(itemToAdd, 1);
        Remove(itemToRemove);
    }
    private void Update()
    {
        itemCursor.SetActive(isMovingItem);
        itemCursor.transform.position = Input.mousePosition;
        if (isMovingItem)
        {
            itemCursor.GetComponent<Image>().sprite = movingSlot.GetItem().itemIcon;
        }
        if (Input.GetMouseButtonDown(0)) // LMB click!
        {
            if (isMovingItem)
            {
                EndItemMove();
            }

            else
            {
                BeginItemMove();
            }
        }

    }

    #region Inventory Utility
    public void RefreshUI()
    {
        //check items in inventory and update the on-screen inventory to match
        for (int i = 0; i < slots.Length; i++)
        {
            try
            {
                // if there is an item stored in the item slot, enable the image and set it to the correct
                // item icon from the scriptable object.
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].GetItem().itemIcon;

                // if the item is a stackable item (a toggle in the scriptable object), then
                // show quantity of item
                if (items[i].GetItem().isStackable)
                {
                    slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().enabled = true;
                    slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = items[i].GetQuantity() + "";
                }
                // otherwise do not show quantity
                else { slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().enabled = false; }
            }
            catch
            {
                // if the above gives an error (no item stored in the slot), then do not show
                // a sprite, and disable the image component
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().enabled = false;
            }
        }
    }
    public bool Add(ItemClass item, int quantity)
    {
        SlotClass slot = Contains(item);

        //check if inventory contains item and is stackable
        if (slot != null && slot.GetItem().isStackable)
        {
            slot.AddQuantity(1);
        }
        else
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].GetItem() == null) // this is an empty slot
                {
                    items[i].AddItem(item, quantity);
                    break;
                }
            }
        }
        RefreshUI();
        return true;
    }

    public bool Remove(ItemClass item)
    {
        //check if item exists in the inventory
        SlotClass temp = Contains(item);
        if (temp != null)
        {
            // if more than one item is in the item slot (stackable), remove one but leave the rest
            if (temp.GetQuantity() > 1)
                temp.SubQuantity(1);
            else
            {
                // if item is not stackable, and item exists, remove the item and slot.
                int subSlotIndex = 0;
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].GetItem() == item)
                    {
                        subSlotIndex = i;
                        break;
                    }
                }
                items[subSlotIndex].Clear();
            }
        }
        else
        {
            // if item does not exist
            return false;
        }

        RefreshUI();
        return true;
    }
    public SlotClass Contains(ItemClass item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].GetItem() == item)
            { return items[i]; }
        }
        return null;
    }
    #endregion Inventory Utility

    #region Moving Items
    private bool BeginItemMove()
    {
        originSlot = GetClosestSlot();
        if (originSlot == null || originSlot.GetItem() == null)
        {
            return false; // there is no item to move
        }
        movingSlot = new SlotClass(originSlot.GetItem(), originSlot.GetQuantity());
        originSlot.Clear();
        isMovingItem = true;
        RefreshUI();
        return true;
    }
    private bool EndItemMove()
    {
        originSlot = GetClosestSlot();
        if (originSlot == null)
        {
            Add(movingSlot.GetItem(), movingSlot.GetQuantity());
            movingSlot.Clear();
        }
        else
        {
            if (originSlot.GetItem() != null)
            {
                if (originSlot.GetItem() == movingSlot.GetItem())
                {
                    if (originSlot.GetItem().isStackable)
                    {
                        originSlot.AddQuantity(movingSlot.GetQuantity());
                        movingSlot.Clear();
                    }
                    else
                        return false;
                }
                else
                {
                    tempSlot = new SlotClass(originSlot.GetItem(), originSlot.GetQuantity()); // a = b
                    originSlot.AddItem(movingSlot.GetItem(), movingSlot.GetQuantity()); // b = c
                    movingSlot.AddItem(tempSlot.GetItem(), tempSlot.GetQuantity()); // a = c
                    RefreshUI();
                    return true;
                }
            }
            else //place item as usual
            {
                originSlot.AddItem(movingSlot.GetItem(), movingSlot.GetQuantity());
                movingSlot.Clear();
            }
        }
        isMovingItem = false;
        RefreshUI();
        return true;


    }
    private SlotClass GetClosestSlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (Vector2.Distance(slots[i].transform.position, Input.mousePosition) <= 28)
            {
                return items[i];
            }
        }
        return null;
    }
    #endregion Moving Items
}

