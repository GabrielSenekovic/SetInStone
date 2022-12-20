using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaneSlash : MonoBehaviour
{
    public LayerMask whatIsGround;
    public LayerMask whatIsWater;
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (1 << collision.gameObject.layer == whatIsWater.value)
        {
            Destroy(gameObject);
        }
    }
}
