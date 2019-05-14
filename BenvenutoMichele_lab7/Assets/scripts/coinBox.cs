using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinBox : MonoBehaviour
{
    public Sprite emptyCoinBox;
    public AudioClip hit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && collision.collider.transform.position.y < transform.position.y ) {
            if (GetComponent<SpriteRenderer>().sprite != emptyCoinBox)
            {
                collision.collider.GetComponent<AudioSource>().PlayOneShot(hit);
            }
            GetComponent<SpriteRenderer>().sprite = emptyCoinBox;
        }
    }
}
