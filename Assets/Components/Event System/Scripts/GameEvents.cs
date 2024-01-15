using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake()
    {
        current = this;
    }
    public event Action onItemPickup;
    public event Action<GameObject> onSpriteSwapUp;

    public void ItemPickup()
    {
        if (onItemPickup != null)
        {
            onItemPickup();
        }
    }
    public void SpriteSwapUp(GameObject targetObject)
    {
        if (onSpriteSwapUp != null)
        {
            onSpriteSwapUp(targetObject);
        }
    }

}
