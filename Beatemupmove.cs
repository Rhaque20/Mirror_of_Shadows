using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beatemupmove : MonoBehaviour
{
    [SerializeField] private float hspeed = 10f;
    [SerializeField] private float vspeed = 6f;
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public BoxCollider2D box;
    public Animator anim;
    public FollowShadow shadoo;
    public float jumpHeight = 500f;

    [HideInInspector] public float upperlimit;
    [HideInInspector] public float cooldown = 0f;
    [HideInInspector] public float staph = 0f;
    [HideInInspector] public bool onground = true, up = true,left = true;
    [HideInInspector] public bool canmove = true;
    [HideInInspector] public float axisY;
    [HideInInspector] public float randomtimer = 0f;
    [HideInInspector] public float safeguard = 0f;
    //public static Beatemupmove instance;
    float hMove;
    [HideInInspector] public float vMove;
    // Start is called before the first frame update

    void Awake()
    {
        //rb.Sleep();
        //instance = this;
    }

    void Start()
    {
        //Physics2D.IgnoreLayerCollision(6,7,true);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("enemy") && !onground)
        {
            if (axisY < collision.transform.position.y)
            {
                sr.sortingOrder = 1;
            }
        }

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        sr.sortingOrder = 0;
        /**
        if (collision.gameObject.CompareTag("wall"))
        {
            if (transform.position.y >= upperlimit && onground)
            {
                transform.position = new Vector2 (transform.position.x,upperlimit);
            }

            Debug.Log("We leavin!");
            upperlimit = 0f;
        }
        **/
    }

    void OnCollisionEnter2D (Collision2D collision)
    {
        
    }

    void OnCollisionStay2D (Collision2D collision)
    {
        
    }

    float vertmovecheck(float vmove)
    {
        if (!up && vmove > 0)
            vmove = 0f;
        
        return vmove;
    }

    float horimovecheck(float hmove)
    {
        if (!left && hmove < 0)
            hmove = 0f;
        
        return hmove;
    }

    void Update()
    {
        if (randomtimer != 0)
        {
            randomtimer -= Time.deltaTime;
            if (randomtimer <= 0)
                randomtimer = 0;
        }

        if (!canmove && safeguard == 0)
        {
            safeguard = 1f;
        }

        if (safeguard != 0)
        {
            safeguard -= Time.deltaTime;

            if (safeguard <= 0)
            {
                safeguard = 0;
                canmove = true;
            }
        }

        if (canmove)
            safeguard = 0;
        
        hMove = Input.GetAxisRaw("Horizontal");
        vMove = Input.GetAxisRaw("Vertical");
    }
    private void FixedUpdate()
    {
        Move(hMove,vMove);
    }

    // Update is called once per frame
    public void Move(float hmove,float vmove)
    {
        if (cooldown != 0)
        {
            cooldown -= Time.deltaTime;

            if (cooldown <= 0)
            {
                cooldown = 0;
            }
        }

        if (staph != 0)
        {
            staph -= Time.deltaTime;

            if (staph <= 0)
            {
                staph = 0;
                canmove = true;
                rb.Sleep();
            }
        }

        if (canmove)
        {
            if (!onground)
            {
                vmove *= 0.5f;
                if (Input.GetKey("m"))
                    rb.gravityScale = 5f;
            }

            hmove = horimovecheck(hmove);
            vmove = vertmovecheck(vmove);

            // Try and use movement.y to alter shadow position for y axis
            
            Vector3 movement = new Vector3(hmove * hspeed, vmove * vspeed, 0.0f);
            transform.position += (movement * Time.deltaTime);
            
            //rb.velocity = new Vector2(hmove*hspeed,vmove*vspeed);
            if (!onground)
                shadoo.verticality(vmove *vspeed * Time.deltaTime);

            if (Input.GetKey("c") && cooldown == 0 && onground)
            {
                if ((Input.GetKey("left") || Input.GetKey("a")) && left)
                {
                    rb.WakeUp();
                    rb.velocity = new Vector3(-10f,0,0);
                    cooldown = 1.0f;
                    canmove = false;
                    staph = 0.5f;
                }

                if (Input.GetKey("right") || Input.GetKey("d"))
                {
                    rb.WakeUp();
                    rb.velocity = new Vector3(10f,0,0);
                    cooldown = 1.0f;
                    canmove = false;
                    staph = 0.5f;
                }

                if ((Input.GetKey("up")|| Input.GetKey("w"))&& up)
                {
                    rb.WakeUp();
                    rb.velocity = new Vector3(0,10f,0);
                    cooldown = 1.0f;
                    canmove = false;
                    staph = 0.5f;
                    Debug.Log("Zoom!");
                }

                if (Input.GetKey("down") || Input.GetKey("s"))
                {
                    rb.WakeUp();
                    rb.velocity = new Vector3(0,-10f,0);
                    cooldown = 1.0f;
                    canmove = false;
                    staph = 0.5f;
                }

                //transform.position = transform.position + movement;

            }

            if (hmove < 0)
            {
                sr.flipX = true;
                anim.SetBool("moving", true);
            }
            else if (hmove > 0)
            {
                sr.flipX = false;
                anim.SetBool("moving", true);
            }
            else if(vmove != 0)
            {
                anim.SetBool("moving", true);
            }
            else
            {
                anim.SetBool("moving", false);
            }
            
            if (onground && Input.GetKey("space") && cooldown == 0)
            {
                axisY = transform.position.y;
                Physics2D.IgnoreLayerCollision(6,8,true);
                //box.isTrigger = true;
                onground = false;
                rb.gravityScale = 1.5f;
                rb.WakeUp();
                rb.AddForce(new Vector2(0, jumpHeight));
                randomtimer = 0.5f;
            }

        }
        if (transform.position.y <= axisY && randomtimer == 0 && !onground)
        {
            OnLanding();
        }
    }

    void OnLanding()
    {
        //box.isTrigger = false;
        onground = true;
        Physics2D.IgnoreLayerCollision(6,8,false);
        sr.sortingOrder = 0;
        rb.gravityScale = 0f;
        rb.Sleep();
        axisY = transform.position.y;
    }
}
