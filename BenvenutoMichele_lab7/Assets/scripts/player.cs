using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public GameObject flame;
    private Rigidbody2D rigidbody2D;
    private int runningAnimationCounter = 0;
    //change sprites for animations
    public Sprite standingStill;
    public Sprite running;
    public Sprite jumping;
    public Sprite falling;
    public Sprite deathAnimation;
    private int changeOrientation = 1;
    private bool tookShrooms = false;
    //change sprites for animations in shroomed state
    public Sprite shroomedUp;
    public Sprite shroomedUpRunning;
    public Sprite shroomedUpjumping;
    public Sprite shroomedUpfalling;
    public Sprite tossingFlame;
    //sounds
    public AudioClip[] sounds;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rigidbody2D)
        {
            
            //Cambia la orientacion a la izquierda
            if (Input.GetAxis("Horizontal") < 0 && changeOrientation > 0)
            {
                Vector2 turn = transform.localScale;
                turn.x = turn.x * -1;
                transform.localScale = turn;
                changeOrientation = changeOrientation * -1;
            }
            //Cambia la orientacion a la derecha
            else if (Input.GetAxis("Horizontal") > 0 && changeOrientation < 0)
            {
                Vector2 turn = transform.localScale;
                turn.x = turn.x * -1;
                transform.localScale = turn;
                changeOrientation = changeOrientation * -1;
            }
            //brincos
            if (Input.GetButtonDown("Vertical"))
            {
                Jump();
            }
            // shoots fireballs
            if (Input.GetButtonDown("Jump")&& tookShrooms)
            {
                GetComponent<SpriteRenderer>().sprite = tossingFlame;
                Vector2 currentPosition = transform.position;
                currentPosition.x += 1f*changeOrientation;
                GameObject newFlame =Instantiate(flame,currentPosition, Quaternion.identity);
                newFlame.GetComponent<Rigidbody2D>().AddForce(new Vector2(250 * changeOrientation, transform.position.y));
                GetComponent<AudioSource>().PlayOneShot(sounds[4]);
            }
            changeSprite();
        }
    }
    private void FixedUpdate()
    {
        if (rigidbody2D) {
            rigidbody2D.AddForce(new Vector2(Input.GetAxis("Horizontal")*10, 0));
        }
    }
    private void Jump()
    {
        if (rigidbody2D)
        {
            if (Mathf.Abs(rigidbody2D.velocity.y) < 0.05f)
            {
                rigidbody2D.AddForce(new Vector2(0,500));
                GetComponent<AudioSource>().PlayOneShot(sounds[0]);
            }
        }
    }
    //Cambia el sprite dependiendo de acciones
    private void changeSprite()
    {
        if (Mathf.Abs(rigidbody2D.velocity.x) < 0.05f && Mathf.Abs(rigidbody2D.velocity.y) < 0.05f)
        {
            GetComponent<SpriteRenderer>().sprite = standingStill;
        }
        else if (Mathf.Abs(rigidbody2D.velocity.x) > 0.005f && Mathf.Abs(rigidbody2D.velocity.y) < 0.05f)
        {
            runningAnimationCounter +=1;
            if (runningAnimationCounter % 20 == 10)
            {
                GetComponent<SpriteRenderer>().sprite = running;
            }
            else if (runningAnimationCounter %20 ==0)
            {
                GetComponent<SpriteRenderer>().sprite = standingStill;
            }
        }
        else if (rigidbody2D.velocity.y > 0.05f)
        {
            GetComponent<SpriteRenderer>().sprite = jumping;
        }
        else if (rigidbody2D.velocity.y < 0.05f)
        {
            GetComponent<SpriteRenderer>().sprite = falling;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("shroom"))
        {
            Destroy(collision.gameObject);
            tookShrooms = true;
            standingStill = shroomedUp;
            running = shroomedUpRunning;
            jumping = shroomedUpjumping;
            falling = shroomedUpfalling;
            GetComponent<BoxCollider2D>().size = new Vector2(0.14f, 0.27377f);
            GetComponent<AudioSource>().PlayOneShot(sounds[1]);
        }
        if (collision.collider.CompareTag("enemy") && !tookShrooms) {
            GetComponent<AudioSource>().clip = sounds[3];
            GetComponent<AudioSource>().loop = false;
            GetComponent<AudioSource>().Play();
            Destroy(GetComponent<BoxCollider2D>());
            GetComponent<SpriteRenderer>().sprite = deathAnimation;
            falling = deathAnimation;
            jumping = deathAnimation;
        }
        else if (collision.collider.CompareTag("enemy") && tookShrooms)
        {
            Destroy(collision.gameObject);
            GetComponent<AudioSource>().PlayOneShot(sounds[2]);
        }
    }
}
