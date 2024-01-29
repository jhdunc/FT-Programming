using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueObject", menuName = "Dialogue System / NPC Dialogue Object", order = 0)]
public class DxBlock : MonoBehaviour
{
    public DialogueSystem.DialogueCondition dialogueCondition;
    public List<string> dialogueText = new List<string>();

    [Header("Condition at Resolution")]
    public DialogueSystem.DialogueCondition endCondition;
}
