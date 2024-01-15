using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
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
[CreateAssetMenu(fileName = "DialogueObject", menuName = "Dialogue System / NPC Dialogue Object", order = 0)]

public class DialogueObject : ScriptableObject
{
    public DialogueSystem.DialogueCondition dialogueCondition;
    public List<string> dialogueText = new List<string>();
    
    [Header("Condition at Resolution")]
    public DialogueSystem.DialogueCondition endCondition;

}
