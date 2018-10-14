using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected bool isGround = false;

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        isGround = false;

        for (int i = 0; i < collision.contacts.Length; i++)
        {
            if (collision.contacts[i].normal.y > 0.5f)
            {
                isGround = true;
            }
        }
    }

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        OnCollisionEnter2D(collision);
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        OnCollisionEnter2D(collision);
    }
}
