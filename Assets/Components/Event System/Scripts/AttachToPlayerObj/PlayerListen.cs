using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerListen : MonoBehaviour
{

    void Update()
    {
        GameEvents.current.ItemPickup();
    }
}
