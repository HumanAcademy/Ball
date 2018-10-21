using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ItemBlock : MonoBehaviour
{
    [SerializeField] GameObject item;
    [SerializeField] AudioClip itemSound;

    [HideInInspector] public AudioSource audioSource = null;

    bool isPutting = false;

    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (var contact in collision.contacts)
        {
            if (contact.normal.y >= 0.7f)
            {
                if (!isPutting)
                    StartCoroutine(PutItem());
            }
        }
    }

    IEnumerator PutItem()
    {
        isPutting = true;

        GameObject prefab = Instantiate(item);
        prefab.GetComponent<Collider2D>().enabled = false;
        prefab.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        prefab.transform.position = this.transform.position;
        audioSource.PlayOneShot(itemSound);

        for (int i = 0; i < 60; i++)
        {
            prefab.transform.position += new Vector3(0f, 0.9f / 60f, 0f);
            yield return null;
        }

        prefab.GetComponent<Collider2D>().enabled = true;
        prefab.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        prefab.GetComponent<Rigidbody2D>().velocity = new Vector2(3f, 5f);
        Destroy(this);
    }
}
