using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadowpuppetry : MonoBehaviour
{
    // Everything is in reference to the groundpoint (transform)
    [SerializeField] private float hspeed = 4f;
    [SerializeField] private float vspeed = 2f;
    int direction = 0; // 1 = Up 2 = Right 3 = Down 4 = Left
    public Rigidbody2D rb,shadowb;
    public SpriteRenderer sr,shadow;
    public Platforming p;
    public BoxCollider2D b;
    Collision2D bux;
    public Transform shaspot;

    public ShadowPosition pos;

    public Animator anim;
    //public FollowShadow shadoo;
    public GameObject puppet,shadowsp;
    public float jumpHeight = 750f,height = 0f,forcedrop = 0f,dashpower = 250f;

    //public float stopwatch = 0f;
    public bool onground = true,falling = false,dashed = false,canmove = true,iframe = false;
    Coroutine dodge;
    [HideInInspector] private bool runleft = false, runright = false;
    [HideInInspector] public float dubbletap = 0.0f;
    [HideInInspector] public float cooldown = 0f;
    public float staph = 0f;
    public float axisY = 0f;
     public float randomtimer = 0f;
    [HideInInspector] public float safeguard = 0f;
    //public static Beatemupmove instance;
    [HideInInspector] public float vMove,hMove;
    // Start is called before the first frame update

    void Awake()
    {
        //rb.Sleep();
        //instance = this;
    }

    void Start()
    {
        pos.Positioning(transform.position);
        pos.PuppetPositioning(shadow.transform.position);
        //p = groundpoint.gameObject.GetComponent<Platforming>();
    }

/**
    void OnTriggerExit2D(Collider2D collision)
    {
        sr.sortingOrder = 0;
        shadow.sortingOrder = 0;
        shadow.sortingLayerName = "Foreground";
            sr.sortingLayerName = "Foreground";
    }
    **/

    private IEnumerator ManageCooldown(float waitTime, int vardelay)
    {
        yield return new WaitForSeconds(waitTime);

        switch(vardelay)
        {
            // Iframe
            case 1:
                sr.color = new Color32(255,255,255,255);
                iframe = false;
                break;
        }
    }

    public void OnTop(HeightMap h)
    {
        p.onplatform = true;
        //Debug.Log("Getting on Top");
        p.elevation = h.height;
        axisY = shadow.transform.position.y + h.height;
        
        pos.Positioning(shadow.transform.position.x,axisY);
        shadow.sortingOrder = 1;
        sr.sortingOrder = 1;
    }

    void OnCollisionStay2D (Collision2D col)
    {
        HeightMap h;
        
        if (col.gameObject.CompareTag("obstacle"))
        {
            if (!onground)
            {
                //bux.isTrigger = true;

                if (forcedrop != 0f)
                    forcedrop = 0f;

                h = col.gameObject.GetComponent<HeightMap>();
                //b.size = new Vector3(1.3f,0.64f,0f);
                if (height + p.elevation >= h.height && !(p.onplatform))
                {
                    OnTop(h);
                    //Physics2D.IgnoreLayerCollision(6,8,true);
                    p.delay = 0.2f;
                    if (!Physics2D.GetIgnoreLayerCollision(6,8))
                    {
                        Debug.Log("Bux is still pointing to something");
                        Physics2D.IgnoreLayerCollision(6,8,true);
                    }
                    
                }
                
            }   
        }
        if (col.gameObject.CompareTag("enemy"))
        {
            Debug.Log("Look they're kissing");
        }
        
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("vwall"))
        {
            //Debug.Log("Want some Vbucks?");
            shadow.sortingOrder = -1;
            sr.sortingOrder = -1;
            shadow.sortingLayerName = "Background";
            sr.sortingLayerName = "Background";
        }
        if (col.gameObject.CompareTag("obstacle"))
        {
            Debug.Log("On a rock");
        }
    }

    public void Jump(float bonus)
    {
        Debug.Log("Hop!");
        axisY = transform.position.y;
        onground = false;
        rb.gravityScale = 1.5f;
        rb.WakeUp();
        rb.AddForce(new Vector2(0, jumpHeight + bonus));
        randomtimer = 0.7f;
    }

    public void ShadowDrop()
    {
        //axisY = groundpoint.transform.position.y;
        pos.Positioning(transform.position.x,transform.position.y);
        forcedrop = shadow.transform.position.y;
        p.elevation = 0f;
        p.shootoff = false;
        dashed = false;
        if (!p.onplatform)
        {
            //Physics2D.IgnoreCollision(b,bux.collider,false);
            Physics2D.IgnoreLayerCollision(6,8,false);
            bux = null;
        }
    }

    public void Fall()
    {
        rb.WakeUp();
        rb.gravityScale = 1.5f;
        onground = false;
        sr.sortingOrder = 0;
        shadow.sortingOrder = 0;
        ShadowDrop();
        
        //randomtimer = 0.25f;
    }

    void Update()
    {
        /**
        if (!Physics2D.GetIgnoreLayerCollision(6,8) && !p.onplatform)
        {
            Physics2D.IgnoreLayerCollision(6,8,false);
        }
        **/
        shaspot = transform;
        
        height = puppet.transform.position.y - transform.position.y;
        if (randomtimer != 0)
        {
            randomtimer -= Time.deltaTime;
            if (randomtimer <= 0)
            {
                randomtimer = 0;
                falling = false;
            }
        }

        /**
        if (anim.GetBool("run"))
            anim.SetBool("moving",false);
        **/

        if (runleft || runright)
        {
            dubbletap += Time.deltaTime;
            if (dubbletap > 0.2f)
            {
                dubbletap = 0.0f;
                runleft = false;
                runright = false;
            }
        }

        if (Input.GetKeyDown("left"))
        {
            if (dubbletap <= 0.2f && dubbletap != 0.0f && !runright)
            {
                anim.SetBool("run",true);
                dubbletap = 0.0f;
            }
            else
            {
                runleft = true;
                runright = false;
            }
        }
        if (Input.GetKeyDown("right"))
        {
            if (dubbletap <= 0.2f && dubbletap != 0.0f && !runleft)
            {
                anim.SetBool("run",true);
                dubbletap = 0.0f;
                Debug.Log("Running away!");
            }
            else
            {
                runright = true;
                runleft = false;
            }
        }

        if (!p.onplatform && p.elevation != 0f && staph == 0)
            Fall();
        else
            axisY = transform.position.y;
        hMove = Input.GetAxisRaw("Horizontal");
        vMove = Input.GetAxisRaw("Vertical");
    }
    private void FixedUpdate()
    {
        Move(hMove,vMove);
    }

    void Dash(int direction)
    {
        shadowb.WakeUp();

        switch(direction)
        {
            case 1:
                shadowb.velocity = new Vector2(0,dashpower/2f);
                dashed = true;
                break;
            case 2:
                shadowb.velocity = new Vector2(dashpower,0);
                dashed = true;
                break;
            case 3:
                shadowb.velocity = new Vector2(0,-dashpower/2f);
                dashed = true;
                break;
            case 4:
                shadowb.velocity = new Vector2(-dashpower,0);
                dashed = true;
                break;
        }
            
        cooldown = 1.0f;
        canmove = false;
        staph = 0.2f;
        iframe = true;
        sr.color = new Color32(0,0,0,255);
        dodge = StartCoroutine(ManageCooldown(0.5f,1));
        
    }

    // Update is called once per frame
    public void Move(float hmove,float vmove)
    {
        int dashes = 1;
        bool candash = true;
        if (staph != 0)
        {
            staph -= Time.deltaTime;
            if (!p.onplatform)// Dashing either on ground or out of platform
            {
                if (onground)
                {
                    //Debug.Log("Normal Dash");
                    pos.Positioning(transform.position);
                    pos.PuppetPositioning(transform.position);
                }
                if (!onground)
                {
                    if (!p.shootoff)
                    {
                        //Debug.Log("Airdash");
                        pos.PuppetPositioning(new Vector2(transform.position.x,axisY + height));
                        pos.Positioning(transform.position.x,transform.position.y);
                    }
                    else
                    {
                        //Debug.Log("OffGround + Offplatform");
                        pos.Positioning(transform.position.x,transform.position.y);
                        pos.PuppetPositioning(new Vector2(transform.position.x,transform.position.y + p.elevation));
                    }
                }

            }
            else// Dashing on a platform
            {
                
                if (onground)
                {
                    //Debug.Log("Dashing through platform");
                    pos.PuppetPositioning(new Vector2(transform.position.x,transform.position.y + p.elevation));
                    pos.Positioning(puppet.transform.position);
                }
                else
                {
                    pos.PuppetPositioning(new Vector2(transform.position.x,axisY + height));
                    pos.Positioning(transform.position.x,transform.position.y + p.elevation);
                }
            }
            
            if (staph <= 0)
            {
                staph = 0;
                canmove = true;
                shadowb.Sleep();
                direction = 0;
                
            }
        }
        if (!onground && !dashed)
        {
            hmove *= 0.75f;
            vmove *= 0.75f;
        }

        if (canmove)
        {
            if (cooldown != 0)
            {
                cooldown -= Time.deltaTime;

                if (cooldown <= 0)
                {
                    cooldown = 0;
                }
            }
            
            Vector3 movement;
            if (anim.GetBool("run"))
                movement = new Vector3(hmove * hspeed * 1.5f, vmove * vspeed * 1.5f, 0.0f);
            else
                movement = new Vector3(hmove * hspeed, vmove * vspeed, 0.0f);
            transform.position += (movement * Time.deltaTime);
            if (onground)
            {
                
                if (!p.onplatform && !falling)
                {
                    //Debug.Log("Walking on ground");
                    pos.PuppetPositioning(transform.position);
                    pos.Positioning(transform.position);
                }
                else// if on a platform
                {
                    //Debug.Log("Walking on platform");
                    pos.PuppetPositioning(new Vector2(transform.position.x,transform.position.y + p.elevation));
                    pos.Positioning(transform.position.x,transform.position.y + p.elevation);
                }
            }
            else
            {
                axisY += (vmove *vspeed * Time.deltaTime);
                transform.position = new Vector2(transform.position.x,axisY);
                
                if (!(p.onplatform))// bare ground
                {
                    Debug.Log("Bare Ground Jump");
                    pos.Positioning(transform.position);
                }
                else// on a platform
                {
                    Debug.Log("Platform jump");
                    pos.Positioning(transform.position.x,transform.position.y + p.elevation);
                }
                pos.PuppetJumping(new Vector2(transform.position.x,puppet.transform.position.y));
                
            }

            if (Input.GetKey("c") && cooldown == 0)
            {
                if (!onground)
                {
                    if (dashes > 0)
                        dashes--;
                    else
                        candash = false;
                    
                }
                
                if (candash)
                {
                    if ((Input.GetKey("left") || Input.GetKey("a")))
                    {    
                        Dash(4);
                    }

                    if (Input.GetKey("right") || Input.GetKey("d"))
                    {   
                        Dash(2);
                    }

                    if ((Input.GetKey("up")|| Input.GetKey("w")))
                    {
                        Dash(1);
                    }

                    if (Input.GetKey("down") || Input.GetKey("s"))
                    {
                        Dash(3);
                    }
                }
                if (!candash)
                    candash = true;
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
                if (Input.GetKeyDown("left") || Input.GetKeyDown("right"))
                {
                    anim.SetBool("moving", true);
                    Debug.Log("Maintaining it");
                }
                else
                {
                    anim.SetBool("moving", false);
                    anim.SetBool("run",false);
                    Debug.Log("Dropping it");
                }
            }
            
            if (onground && Input.GetKey("space"))
            {
                Jump(0.0f);
            }

        }
        else
        {
            anim.SetBool("moving",false);
            pos.PuppetPositioning(transform.position);
            pos.Positioning(transform.position);
        }
        
        /**
        if (forcedrop != 0f)
        {
            shadow.transform.position = new Vector2(shadow.transform.position.x, forcedrop);
        }
        **/

        if (puppet.transform.position.y <= shadow.transform.position.y && randomtimer == 0 && !onground)
        {
            Debug.Log("Oof");
            OnLanding();
        }
    }

    void OnLanding()
    {
        //bux.isTrigger = false;
        onground = true;
        rb.gravityScale = 0f;
        rb.Sleep();
        axisY = shadow.transform.position.y;
        falling = false;
        forcedrop = 0f;
        
        //Debug.Log("Airborn for "+stopwatch+" seconds");
        //stopwatch = 0f;
    }
}