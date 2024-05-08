using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem // create a list that can be accessed from other scripts: DialogueSystem.DialogueCondition.
{
    public enum DialogueCondition
    {
        FirstContact,
        QuestAvailable,
        QuestActive,
        QuestEnd,
        Idle
    }
}
// create the ability to make this in the Create menu in your right click asset folders
[CreateAssetMenu(fileName = "DialogueObject", menuName = "Dialogue System / NPC Dialogue Object", order = 0)]

public class DxObject : ScriptableObject // scriptable object means you use this as a template for other scripts
{
    public List<string> dialogueText = new List<string>();
    
    [Header("Condition at Resolution")] // header to display in the Inspector (does not impact function).
    public DialogueSystem.DialogueCondition endCondition; // I use this to determine where the code goes next.
}


