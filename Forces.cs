using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forces : MonoBehaviour
{
    public Stats stats;
    public float axisY;
    public bool yeet = false, airslap = false,onplatform = false;
    public float pushtimer = 0f,height = 0f,airtime = 0f;

    public float heightslap = 0f;
    public float floorcheck = 0f,elevation = 0f,bonus = 0f;
    public SpriteRenderer shadow,puppet;
    public Rigidbody2D rigid;
    public Rigidbody2D groundrigid;
    public BoxCollider2D body;
    Collision2D bux;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("obstacle"))
        {
            //Debug.Log("I hit a block by trigger");
        }
    }

    public void ShadowDrop()
    {
        //axisY = groundpoint.transform.position.y;
        shadow.transform.position = new Vector2(transform.position.x,transform.position.y);
        elevation = 0f;
        if (!onplatform)
        {
            Physics2D.IgnoreCollision(body,bux.collider,false);
            //Physics2D.IgnoreLayerCollision(6,8,false);
            bux = null;
        }
    }

    public void Fall()
    {
        rigid.WakeUp();
        floorcheck = 0.1f;
        rigid.gravityScale = 1.5f;
        puppet.sortingOrder = 0;
        shadow.sortingOrder = 0;
        ShadowDrop();
        
        //randomtimer = 0.25f;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        //delay = 0.1f;
        
        
        if (onplatform && col.gameObject.CompareTag("obstacle"))
        {

            yeet = true;
            Debug.Log("Fallin through!");
            onplatform = false;
            elevation -= bonus;
            bonus = 0f;
            Fall();
            
            
            //sp.ShadowDrop();
           
            
        }
        
    }

/**
    void OnCollisionExit2D (Collision2D col)
    {
        if (onplatform && col.gameObject.CompareTag("obstacle"))
        {
            yeet = true;
            Debug.Log("Fallin through!");
            onplatform = false;
            //Fall();
        }
    }
**/

    void OnCollisionEnter2D (Collision2D col)
    {
        HeightMap h;
        if (col.gameObject.CompareTag("obstacle"))
        {
            if (!yeet && !onplatform)
            {
                Jump(750f);
                h = col.gameObject.GetComponent<HeightMap>();
                
                    onplatform = true;
        //Debug.Log("Getting on Top");
                    bonus = h.height;
                    elevation += bonus;
                    axisY = transform.position.y + h.height;
                    
                    shadow.transform.position = new Vector2(transform.position.x,axisY);
                    shadow.sortingOrder = 1;
                    puppet.sortingOrder = 1;

                    if (bux != null)
                    {
                        Debug.Log("Bux is still pointing to something");
                        Physics2D.IgnoreCollision(body,bux.collider,false);
                        bux = col;
                        Physics2D.IgnoreCollision(body,bux.collider,true);
                    }
                    else
                    {
                        bux = col;
                        Physics2D.IgnoreCollision(body,bux.collider,true);
                    }
                    //Debug.Log("bux is "+bux.name);
                //floorcheck = 0.2f;
            }
        }
        
        //Attack();
    }

    public void Stagger(bool left,int attack,float knock)
    {
        rigid.WakeUp();
        

        if (attack == 1)
        {
              
            if (left)
                groundrigid.AddForce(new Vector2(-1f * knock * groundrigid.mass,0f));
            else
                groundrigid.AddForce(new Vector2(knock * groundrigid.mass,0f));

            //stats.staggertime = 0.5f;
            
            if (heightslap == 0f)
            {
                heightslap = puppet.transform.position.y; 
                pushtimer = 0.1f;
            }
            airtime = 0.5f;
        }

        if (attack == 2)
        {
            if (!yeet)
            {
                Debug.Log("Launched!");
                Jump(knock);
            }
        }
    }

    void Jump(float knock)
    {
        if (rigid.IsSleeping())
            rigid.WakeUp();
        Debug.Log("Jump!");
        //axisY = shadow.transform.position.y;
        rigid.gravityScale = 1.5f;
        rigid.AddForce(new Vector2(0f,knock));
        yeet = true;
        floorcheck = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
        elevation = puppet.transform.position.y - transform.position.y;
        if (elevation < 0f)
            elevation = 0f;
        if (Input.GetKey("o"))
        {
            if (!yeet)
                Jump(500f);
        }
        if (pushtimer != 0)
        {
            pushtimer -= Time.deltaTime;
            if (pushtimer <= 0)
            {
                pushtimer = 0;
                
                if (!yeet)
                    rigid.Sleep();
            }
        }

        if (airtime != 0)
        {
            airtime -= Time.deltaTime;
            if (airtime <= 0f)
            {
                airtime = 0f;
                heightslap = 0f;
            }
        }

        if (floorcheck != 0)
        {
            floorcheck -= Time.deltaTime;
            if (floorcheck <= 0)
                floorcheck = 0;
        }

        if (yeet)
        {
            if (!onplatform)
                shadow.transform.position = transform.position;
            else
                shadow.transform.position = new Vector2(transform.position.x,transform.position.y + elevation);
                if (heightslap == 0f)
                    puppet.transform.position = new Vector2(transform.position.x,puppet.transform.position.y);
                else
                    puppet.transform.position = new Vector2(transform.position.x,heightslap);
            if (floorcheck == 0)
            {
                if (puppet.transform.position.y <= shadow.transform.position.y)
                {
                    Debug.Log("Touch down!");
                    rigid.gravityScale = 0f;
                    rigid.Sleep();
                    yeet = false;
                }
            }
        }
        else
        {
            if (!onplatform)
            {
                shadow.transform.position = transform.position;
                puppet.transform.position = transform.position;
            }
            else
            {
                shadow.transform.position = new Vector3(transform.position.x,transform.position.y + elevation);
                puppet.transform.position = shadow.transform.position;
            }
            
        }
    }
}
