using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwap : MonoBehaviour
{
    public SpriteRenderer sr;
    public Sprite firstSprite;
    public Sprite newSprite;

    private void Start()
    {
        sr.sprite = firstSprite;
    }
    public void ChangeSpriteNew()
    {
        if (sr.sprite == firstSprite)
        { sr.sprite = newSprite; }
    }

    public void ChangeSpriteOld()
    {
        if (sr.sprite == newSprite)
        { sr.sprite = firstSprite; }
    }

}
