using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Habanero : Item
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.isFire = true;
            Destroy(this.gameObject);
        }
    }
}
