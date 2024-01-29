using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueInteract : MonoBehaviour
{
    private DialogueSystem.DialogueCondition npcState;
    public int dialogueDelay;

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

    [SerializeField] InventoryManager inventory;
    [SerializeField] ItemClass objectiveItem;
    [SerializeField] GameObject[] worldObject;

    private bool nearPlayer;

    public void Start()
    {
        nearPlayer = false;
        // default NPC state to First Contact
        npcState = DialogueSystem.DialogueCondition.FirstContact;

        // safety check: if the dialogue delay isn't set when game is run, set automatically
        if(dialogueDelay == 0)
        { dialogueDelay = 3; }

        // hide UI
        UI.SetActive(false);

    }
    public void DialogueStart()
    {
        StartCoroutine(DisplayDialogue());
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && nearPlayer)
        {
            if (UI.activeSelf)
            { UI.SetActive(false); }
            else
            { DialogueStart(); }
        }
    }
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
            nearPlayer = false;
        }
        
    }
    IEnumerator DisplayDialogue()
    {
        switch (npcState) // determine which dialogue script to show based on NPC's active state
        {
            case DialogueSystem.DialogueCondition.FirstContact:
                for (int i = 0; i < firstContact.dialogueText.Count; i++) // for each string of dialogue in firstcontact's scriptable object, run the following:
                {
                    Debug.Log(firstContact.dialogueText[i]); // print text to console for testing purposes

                    UI.SetActive(true); // enable the UI window
                    dialogueTextUI.text = firstContact.dialogueText[i]; // set the UI text to read the dialogue text
                    yield return new WaitForSeconds(dialogueDelay); // wait for the delay before showing the next string 
                }
                npcState = firstContact.endCondition;
                
                break;

            case DialogueSystem.DialogueCondition.QuestAvailable:
                for (int i = 0; i < questAvailable.dialogueText.Count; i++)
                {
                    Debug.Log(questAvailable.dialogueText[i]);

                    UI.SetActive(true);
                    dialogueTextUI.text = questAvailable.dialogueText[i];
                    yield return new WaitForSeconds(dialogueDelay);
                }
                npcState = questAvailable.endCondition; 
                break;
            case DialogueSystem.DialogueCondition.QuestActive:
                for (int i = 0; i < questActive.dialogueText.Count; i++)
                {
                    if (objectiveItem != null) // if there is a quest objective item
                    {
                        SlotClass slot = inventory.Contains(objectiveItem); 
                        if (slot != null)
                        {
                            Debug.Log(questEnd.dialogueText[i]);

                            UI.SetActive(true);
                            dialogueTextUI.text = questEnd.dialogueText[i];
                            if (worldObject != null)
                            {
                                for (int x = 0; x < worldObject.Length; x++)
                                {
                                    GameEvents.current.SpriteSwapUp(worldObject[x]);
                                } // Make sprite swap possible if this is an objective
                            }
                                    
                            npcState = questEnd.endCondition; // advance to next dialogue set
                        }
                        else
                        {
                            Debug.Log(questActive.dialogueText[i]);

                            UI.SetActive(true);
                            dialogueTextUI.text = questActive.dialogueText[i];
                            yield return new WaitForSeconds(dialogueDelay);
                        }
                    }
                }
                break;
            case DialogueSystem.DialogueCondition.QuestEnd:
                    for (int i = 0; i < questEnd.dialogueText.Count; i++)
                    {
                        Debug.Log(questEnd.dialogueText[i]);

                        UI.SetActive(true);
                        dialogueTextUI.text = questEnd.dialogueText[i];
                    yield return new WaitForSeconds(dialogueDelay);
                    
                    }
                    npcState = questEnd.endCondition;
                break;
            case DialogueSystem.DialogueCondition.Idle:
                for (int i = 0; i < idle.dialogueText.Count; i++)
                {
                    Debug.Log(idle.dialogueText[i]);

                    UI.SetActive(true);
                    dialogueTextUI.text = idle.dialogueText[i];
                    yield return new WaitForSeconds(dialogueDelay);

                    if (GetComponent<SpriteSwap>() != null)
                    {
                        GetComponent<SpriteSwap>().UpdateSprite(1);
                    }
                }
                npcState = idle.endCondition;
                break;
            default:
                break;
        }

    }

}
