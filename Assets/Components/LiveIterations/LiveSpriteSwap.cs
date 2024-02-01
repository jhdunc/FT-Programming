using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveSpriteSwap : MonoBehaviour
{
    public bool spriteUp;
    private SpriteRenderer sr;
    private Animator anim;
    public List<Sprite> spriteList = new List<Sprite>();
    public List<RuntimeAnimatorController> animList = new List<RuntimeAnimatorController>();
    private int sprIndex;
    private int animIndex;

    bool nearPlayer;

    private void Start()
    {
        spriteUp = false;
        nearPlayer = false;

        sprIndex = 0;
        animIndex = 0;
        sr = gameObject.GetComponent<SpriteRenderer>();
        if (spriteList.Count > 0)
        { sr.sprite = spriteList[sprIndex]; }
        anim = gameObject.GetComponent<Animator>();

        GameEvents.current.onIncrSpriteUp += IncrSpriteUp;
        GameEvents.current.onUpdateSprite += UpdateSprite;
    }
    #region Change Sprite by List
    public void IncrSpriteUp(GameObject targetObject)
    {
        Debug.Log("attempt update: " + targetObject);
        var tarobj = targetObject.GetComponent<LiveSpriteSwap>();
        if (tarobj.sprIndex < (tarobj.spriteList.Count - 1))
        {
            tarobj.sprIndex++;
            tarobj.sr.sprite = tarobj.spriteList[sprIndex];
            if (tarobj.animList != null)
            {
                tarobj.animIndex++;
                tarobj.anim.runtimeAnimatorController = tarobj.animList[animIndex];
            }
        }
        else
        {
            Debug.Log("Cannot run IncrSpriteUp: end of the list.");
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

    public void UpdateSprite(GameObject targetObject, int spriteIndex)
    {
        
        var tarobj = targetObject.GetComponent<LiveSpriteSwap>();
        if (tarobj.animList.Count > 0)
        { tarobj.anim.runtimeAnimatorController = tarobj.animList[spriteIndex]; }
        else if (tarobj.spriteList.Count > 0)
        { tarobj.sr.sprite = tarobj.spriteList[spriteIndex]; }

    }
    #region Method Call to Enable Swap
    public void SpriteSwapUp(GameObject targetObject)
    {
        targetObject.GetComponent<LiveSpriteSwap>().spriteUp = true;
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
        /*        if (spriteUp)
                {
                    if (Input.GetKeyDown(KeyCode.E) && nearPlayer)
                    {
                        IncrSpriteUp();
                    }
                }*/

    }
}