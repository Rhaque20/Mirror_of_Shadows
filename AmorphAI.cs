using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmorphAI : MonoBehaviour
{
    public EnemyNav movement;
    public Stats stats;
    public Rigidbody2D rb;
    public BoxCollider2D attackscan;
    public Animator anim;
    public SpriteRenderer sr;
    public LayerMask playerLayers;
    public GameObject slashPrefab;
    public Transform attackPoint;
    public float x,y,posx = 1.02f,posy = 0.84f;
    public float winding = 0f;
    int ran = 0;// Used to offset attack being called twice.
    public float delay = 0f;
    public bool hit = false;
    // Start is called before the first frame update
    void Start()
    {
        attackscan.size = new Vector2(x,y);
    }

    void Neutral()
    {
        anim.SetInteger("Skill",0);
        anim.SetBool("Release",false);
        delay = 1f;
        attackscan.enabled = true;
    }

    void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && delay == 0)
        {
            anim.SetInteger("Skill",1);
            movement.enabled = false;
            winding = 0.5f;
            attackscan.enabled = false;
        }
        //Attack();
    }

/**
    void OnTriggerStay2D (Collider2D collision)
    {
        Debug.Log("Prepared to get spanked!");
    }
**/

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireCube(attackPoint.position,new Vector3(x,y,0f));
    }

    void SlashEffect()
    {
        Vector3 pos;
        if (sr.flipX)
            pos = new Vector3(attackPoint.position.x,attackPoint.position.y + 2f, 0f);
        else
            pos = new Vector3(attackPoint.position.x - 1f,attackPoint.position.y + 2f, 0f);
        
        GameObject slasheffect = Instantiate(slashPrefab,pos,Quaternion.identity);
    }

    void Attack()
    {
        Collider2D player = Physics2D.OverlapBox(attackPoint.position,new Vector2(x,y),0f,playerLayers);
        Shadowpuppetry sp;
        
        if (ran == 0)
            ran++;
        else
        {
            ran = 0;
            return;
        }
        Debug.Log("Calling attack!");
        if (player == null)
        {
            Debug.Log("Nothing in range yet");
            return;
        }

        if(player.gameObject.CompareTag("Player"))
        {
            sp = player.gameObject.GetComponent<Shadowpuppetry>();
            if (sp.staph <= 0.1f)
            {
                sp.puppet.gameObject.GetComponent<Stats>().damagecalc(stats.attack,stats.critdmg,stats.critrate,0.85f,1.1f,1.0f);
                SlashEffect();
            }
            else
                Debug.Log("Evaded");
        }
        movement.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (sr.flipX)
        {
            attackPoint.position = new Vector3(transform.position.x + posx,transform.position.y + posy,0f);
            attackscan.offset = new Vector2(posx,posy);
        }
        else
        {
            attackPoint.position = new Vector3(transform.position.x - posx,transform.position.y + posy,0f);
            attackscan.offset = new Vector2(-posx,posy);
        }

        if (stats.staggertime != 0)
        {
            winding = 0;
            anim.Play("blob_idle");
            Neutral();
        }

        if (winding != 0)
        {
            winding -= Time.deltaTime;

            if (winding <= 0)
            {
                winding = 0;
                anim.SetBool("Release",true);
            }
        }
        
        if (delay != 0)
        {
            delay -= Time.deltaTime;

            if (delay <= 0)
                delay = 0;
        }

        if (stats.staggertime != 0)
        {
            movement.enabled = false;
            rb.drag = 0f;
        }
        else
        {
            movement.enabled = true;
            rb.drag = 1.5f;
        }

        if (stats.state == 2)
            delay = 1f;
    }
}
