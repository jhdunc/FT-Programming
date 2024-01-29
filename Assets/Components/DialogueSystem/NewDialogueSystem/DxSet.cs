using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DxSet : ScriptableObject
{
    [Header("First Set")]
    public List<DxBlock> firstText;
    
    [Header("Does Player Have Dx Choice?")]
    public bool playerReact;

    [Header("Dialogue Option A")]
    public List<DxBlock> optionA;

    [Header("Dialogue Option B")]
    public List<DxBlock> optionB;

}
