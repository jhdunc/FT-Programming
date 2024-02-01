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
    public event Action<GameObject> onIncrSpriteUp;
    public event Action<GameObject, int> onUpdateSprite;

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

    public void IncrSpriteUp(GameObject targetObject)
    {
        if (onIncrSpriteUp != null)
        {
            onIncrSpriteUp(targetObject);
        }
    }
    public void UpdateSprite(GameObject targetObject, int index)
    {
        if (onUpdateSprite != null)
        {
            onUpdateSprite(targetObject, index);
        }
    }

}