using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float movespeed = 20f;
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public Animator anim;
    public float jumpHeight = 0.001f;
    public float checkRadius;
    private bool onGround = false;
    public Transform feetpos;
    public LayerMask whatIsGround;
    private float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping = true;
    // Start is called before the first frame update

    void OnCollisionEnter(Collision collider)
    {
        if (collider.collider.tag == "ground")
        {
            onGround = true;
            Debug.Log("on the ground");
        }

    }

    void Start()
    {
        
    }

    bool groundcheck()
    {
        return Physics2D.OverlapCircle(feetpos.position,checkRadius,whatIsGround);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector2 direction = new Vector2(horizontal, vertical).normalized;

        onGround = groundcheck();
        /**
        if (direction.magnitude >= 0.1f)
        {
            controller.Move(horizontal * Time.fixedDeltaTime, false, false);
        }
        **/
/**
        if (direction.magnitude >= 0.1f)
        {
            anim.SetBool("moving", true);
            if (horizontal < 0)
                sr.flipX = true;
            if (horizontal > 0)
                sr.flipX = false;
        }
        if (direction.magnitude < 0.1f)
        {
            Debug.Log("Eggroll");
            anim.SetBool("moving", false);
        }
**/
        if (Input.GetKey("left")||Input.GetKey("a"))
        {
            anim.SetBool("moving", true);
            sr.flipX = true;
        }
        else if (Input.GetKey("right")||Input.GetKey("d"))
        {
            anim.SetBool("moving", true);
            sr.flipX = false;
        }
        else
        {
            anim.SetBool("moving", false);
        }

        if (Input.GetKey("z"))
        {
            anim.Play("sample_attack");
        }

        if (onGround && Input.GetKeyDown("space"))
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpHeight;
        }

        if (Input.GetKey("space") && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity +=Vector2.up * jumpHeight;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
                isJumping = false;
        }

        if (Input.GetKeyUp("space"))
            isJumping = false;

        rb.velocity = new Vector2(horizontal * movespeed, rb.velocity.y);

    }
}
