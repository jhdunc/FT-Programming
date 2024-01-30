using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DxInteractPress : MonoBehaviour
{
    private DialogueSystem.DialogueCondition npcState;

    [Header("UI Game Objects")]
    [SerializeField] GameObject UI;
    [SerializeField] TextMeshProUGUI dialogueTextUI;

    [Header("Dialogue Objects")]
    [SerializeField] DxObject firstContact;
    [SerializeField] DxObject questAvailable;
    [SerializeField] DxObject questActive;
    [SerializeField] DxObject questEnd;
    [SerializeField] DxObject idle;

    [Header("Item Management: Quests")]
    [SerializeField] ItemClass objectiveItem; // item required to complete quest
    [SerializeField] GameObject[] worldObject; // object in world affected when quest completed
    private InventoryManager inventory;

    private bool nearPlayer;
    private DxObject activeDx;
    private int activeDxIndex;

    private void Start()
    {
        inventory = GameObject.Find("Inventory").GetComponent<InventoryManager>(); //find the inventory

        nearPlayer = false;
        // default NPC state to First Contact
        npcState = DialogueSystem.DialogueCondition.FirstContact;
        activeDx = firstContact;
        activeDxIndex = 0;

        // hide UI
        UI.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(activeDx);
            Debug.Log("Index: " + activeDxIndex);
        }
        if (Input.GetKeyDown(KeyCode.E) && nearPlayer)
        {
            CheckActiveDx();
            switch (npcState)
            {
                case DialogueSystem.DialogueCondition.FirstContact:
                    InitiateDialogue(activeDx);
                    break;
                case DialogueSystem.DialogueCondition.QuestAvailable:
                    InitiateDialogue(activeDx);
                    break;
                case DialogueSystem.DialogueCondition.QuestActive:
                    InitiateDialogue(activeDx);
                    break;
                case DialogueSystem.DialogueCondition.QuestEnd:
                    InitiateDialogue(activeDx);
                    inventory.Remove(objectiveItem); // remove quest objective
                    if (GetComponent<SpriteSwap>() != null)
                    {
                        GetComponent<SpriteSwap>().UpdateSprite(1); // update NPC sprite if sprite changes
                    }

                    if (worldObject != null) // check if a world object is affected
                    {
                        for (int x = 0; x < worldObject.Length; x++) // loop through all world objects in list
                        {
                            GameEvents.current.SpriteSwapUp(worldObject[x]); // enable player interaction with world object sprite swap
                        }
                    }
                    break;
                case DialogueSystem.DialogueCondition.Idle:
                    InitiateDialogue(activeDx);
                    break;
            }
        }
    }
    #region Collision Logic
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            nearPlayer = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            UI.SetActive(false);
            activeDxIndex = 0;
            nearPlayer = false;
        }
    }
    #endregion
    void InitiateDialogue(DxObject dialogue)
    {
        UI.SetActive(true);
        Debug.Log("dxObj: " + dialogue + "is at index: " + activeDxIndex);

        if (dialogue != null && activeDxIndex < (dialogue.dialogueText.Count - 1))
        {
            dialogueTextUI.text = dialogue.dialogueText[activeDxIndex]; // set the UI text to read the dialogue text
            activeDxIndex += 1; // increase the counter 
        }
        else if (activeDxIndex == dialogue.dialogueText.Count - 1)
        {
            dialogueTextUI.text = dialogue.dialogueText[activeDxIndex]; // set the UI text to read the dialogue text
            activeDxIndex += 1;
        }
        else if (activeDxIndex == dialogue.dialogueText.Count)
        {
            npcState = dialogue.endCondition;
            UI.SetActive(false);
            activeDxIndex = 0;
        }
    }


    void CheckActiveDx()
    {   // makes certain that the current dialogue variable matches the current 
        // NPC state and checks for objectives

        switch (npcState)
        {
            case DialogueSystem.DialogueCondition.FirstContact:
                activeDx = firstContact;
                break;
            case DialogueSystem.DialogueCondition.QuestAvailable:
                activeDx = questAvailable;
                break;
            case DialogueSystem.DialogueCondition.QuestActive:
                {
                    activeDx = questActive;
                    if (objectiveItem != null)
                    {
                        SlotClass slot = inventory.Contains(objectiveItem);
                        if (slot != null)
                        {
                            activeDx = questEnd;
                            npcState = DialogueSystem.DialogueCondition.QuestEnd;
                        }
                    }
                }
                break;
            case DialogueSystem.DialogueCondition.QuestEnd:
                activeDx = questEnd;
                break;
            case DialogueSystem.DialogueCondition.Idle:
                activeDx = idle;
                break;
        }
    }
}
