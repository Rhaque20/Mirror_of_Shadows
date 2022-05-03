using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowShadow : MonoBehaviour
{
    public GameObject player;
    public Beatemupmove bm;
    public BoxCollider2D shadowbox;
    public float yOffset = 0f, jumptime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void verticality(float movement)
    {
        yOffset = movement;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("wall"))
        {
            Debug.Log("Entry!");
            //collision.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            if (bm.staph != 0 && Input.GetKey("up"))
            {
                bm.rb.velocity = new Vector3(0,0,0);
                bm.rb.angularVelocity = 0f;
            }
            bm.up = false;
        }

        if (collision.gameObject.CompareTag("vwall"))
        {
            bm.left = false;
            if (bm.staph != 0 && Input.GetKey("left"))
            {
                bm.rb.velocity = new Vector3(0,0,0);
                bm.rb.angularVelocity = 0f;
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("wall"))
        {
            Debug.Log("Get out!");
            if (bm.staph != 0 && Input.GetKey("up"))
            {
                bm.rb.velocity = new Vector3(0,0,0);
                bm.rb.angularVelocity = 0f;
            }
            bm.up = false;
        }
        if (collision.gameObject.CompareTag("vwall"))
        {
            bm.left = false;
            if (bm.staph != 0)
            {
                bm.rb.velocity = new Vector3(0,0,0);
                bm.rb.angularVelocity = 0f;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("wall"))
        {
            Debug.Log("Leaving boundaries");
            bm.up = true;
            //collision.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        }
        if (collision.gameObject.CompareTag("vwall"))
        {
            bm.left = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (jumptime != 0)
        {
            jumptime -= Time.deltaTime;

            if (jumptime <= 0)
            {
                jumptime = 0;
                //shadowbox.isTrigger = false;
            }
        }
        if (!(bm.onground))
        {
            transform.position = new Vector2(player.transform.position.x,bm.axisY + yOffset);
            bm.axisY += yOffset;

            if (jumptime == 0)
                jumptime = 0.1f;

        }
        else
        {
            transform.position = new Vector2(player.transform.position.x,player.transform.position.y);
            yOffset = 0f;
            shadowbox.isTrigger = true;
        } 
    }
}
