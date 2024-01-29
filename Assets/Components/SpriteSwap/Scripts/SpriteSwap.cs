using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwap : MonoBehaviour
{
    public bool spriteUp;
    public SpriteRenderer sr;
    public List<Sprite> spriteList = new List<Sprite>();
    private int sprIndex;

    bool nearPlayer;

    private void Start()
    {
        spriteUp = false;
        nearPlayer = false;

        sprIndex = 0;
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.sprite = spriteList[sprIndex];


        GameEvents.current.onSpriteSwapUp += SpriteSwapUp;
    }
    #region Change Sprite by List
    public void IncrSpriteUp()
    {
        
        if (sprIndex < (spriteList.Count - 1))
        {
            Debug.Log("the object: " + gameObject + "Index: " + sprIndex);
            sprIndex++;
            sr.sprite = spriteList[sprIndex];
        }
        else
        {
            Debug.Log("Cannot run IncrSpriteUp: end of the list.");
            Debug.Log("the object: " + gameObject + "Index: " + sprIndex);
        }
    }
    public void IncrSpriteDown()
    {
        if (sprIndex != 0)
        {
            sprIndex--;
            sr.sprite = spriteList[sprIndex];
        }
        else
            Debug.Log("Cannot run IncrSpriteDown: beginning of the list.");
    }
    #endregion

    public void UpdateSprite(int spriteIndex)
    {
        sr.sprite = spriteList[spriteIndex];
    }
    #region Method Call to Enable Swap
    public void SpriteSwapUp(GameObject targetObject)
    {
            targetObject.GetComponent<SpriteSwap>().spriteUp = true;
    }
    
    #endregion
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            nearPlayer = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            nearPlayer = false;
    }

    private void Update()
    {
        if (spriteUp)
        {
            if (Input.GetKeyDown(KeyCode.E) && nearPlayer)
            {
                IncrSpriteUp();
            }
        }

    }
}


