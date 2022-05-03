using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCecile : MonoBehaviour
{
    public Animator anim;
    public Transform attackPoint,player;
    public Stats stats;
    public float attackRange = 1f;
    public string natk_name = "TestAttack";
    public LayerMask enemyLayers;
    //public Beatemupmove bm;
    public Shadowpuppetry sp;
    public bool delay,airborne;
    [HideInInspector]public int attack = 0;
    [HideInInspector]public int part = 1,charges = 2;
    [HideInInspector]public float power,force;
    public float timer = 0f,airheight = 0f;
    public float x,y,height = 0f,hoffset = 1.02f,voffset = 0f,lowheight,highheight;

    Coroutine aerial = null;

    public bool zoneoverride = false;
    public static AttackCecile instance;
    public GameObject slashPrefab,kunai;
    private Vector3 slap;
    // Start is called before the first frame update

    void Start()
    {
        delay = false;
    }

    private void Awake()
    {
        instance = this;
    }

    public void Mobility()
    {
        sp.canmove = true;
    }

    public void Readyup()
    {
        delay = false;
    }

    Vector2 positioning()
    {
        if (!sp.sr.flipX)
            return new Vector2(attackPoint.position.x,attackPoint.position.y + 2f);
        else
            return new Vector2(attackPoint.position.x - 1f,attackPoint.position.y + 2f);
    }

    void SlashEffect(Vector3 position)
    {  
        GameObject slasheffect = Instantiate(slashPrefab,position,Quaternion.identity);
    }

    void FireKunai(int kunais)
    {
        int i;
        float start = 0.25f;
        Vector2 basepos = positioning(),startpos;
        ProjectileScript ps = kunai.GetComponent<ProjectileScript>();

        if (sp.sr.flipX)
            ps.rightward = false;
        else
            ps.rightward = true;
        
        for (i = 0; i < kunais; i++)
        {
            startpos = new Vector2(basepos.x,basepos.y + start);
            Instantiate(kunai,startpos,Quaternion.identity);
            start -= 0.25f;
        }
    }

    void AttackForce(float power)
    {
        // change ALL shadowb to rb if plan goes south
        if (!sp.sr.flipX)
            sp.shadowb.velocity = new Vector2(power,0f);
        else
            sp.shadowb.velocity = new Vector2(-power,0f);
        timer = 0.1f;
        if (aerial != null)
            StopCoroutine(aerial);
        aerial = StartCoroutine(Cooldown(1.0f,1));
    }

    void RelocateZone(bool back)
    {
        //back = false means center, back = true means behind
        Debug.Log("Relocating");
        zoneoverride = true;
        if (back)
        {
            if (!sp.sr.flipX)
                attackPoint.position = new Vector3(player.position.x - hoffset,player.position.y + voffset,0f);
            else
                attackPoint.position = new Vector3(player.position.x + hoffset,player.position.y + voffset,0f);
        }
    }

    void Attack(int chain)
    {
        Collider2D[] hitEnemies;
        Forces jedi;

        sp.shadowb.WakeUp();
            if (sp.onground)
                sp.shadowb.gravityScale = 0f;

        switch(chain)
        {
            case 1:
                power = 0.9f;
                break;
            case 2:
                if (part == 1)
                {
                    power = 0.75f;
                    part++;
                }
                else
                {
                    power = 1.2f;
                    part = 1;
                }
                break;
            case 3:
                power = 1.1f;
                break;
            case 4:
                if (part == 1)
                {
                    power = 1.1f;
                    part++;
                }
                else
                {
                    power = 1.5f;
                    part = 1;
                }
                break;
            case 5:
                if (part == 1)
                {
                    power = 1.1f;
                    part++;
                }
                else
                {
                    part = 1;
                    power = 1.1f;
                    RelocateZone(true);
                    aerial = StartCoroutine(Cooldown(0.5f,2));
                }
                break;
            case 6:
                if (part == 1)
                {
                    power = 1.2f;
                    part++;
                }
                else
                {
                    power = 1.7f;
                    part = 1;
                    sp.rb.WakeUp();
                    airborne = false;
                    StopCoroutine(aerial);
                }
            break;
        }

        hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position,new Vector2(x,y),0f,enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            
            if (enemy.gameObject.CompareTag("enemy"))
            {
                
                jedi = enemy.gameObject.GetComponent<Forces>();
                Debug.Log("Cecile hits: "+enemy.tag);

                if (jedi.height + jedi.elevation >= lowheight || jedi.height + jedi.elevation <= highheight)
                {
                    SlashEffect(jedi.puppet.transform.position);
                    if (attack == 1)
                        force = 50f;
                    if (attack == 2)
                    {
                        force = 500f;
                    }
                    if (jedi == null)
                        Debug.Log("Failure!");
                    
                    if (jedi.stats.armor <= 0f)
                        jedi.Stagger(sp.sr.flipX,attack,force);
                    
                    jedi.stats.damagecalc(stats.attack,stats.critdmg,stats.critrate,stats.low,stats.high,power);
                }
            }
            else
                Debug.Log("Whiff!");
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireCube(attackPoint.position,new Vector3(x,y,0f));
    }

    private IEnumerator Cooldown(float waitTime, int vardelay)
    {
        yield return new WaitForSeconds(waitTime);

        switch(vardelay)
        {
            // Airborne
            case 1:
            Debug.Log("Called!");
                sp.rb.WakeUp();
                airborne = false;
                break;
            case 2:
                zoneoverride = false;
            break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        lowheight = sp.height - 1f;
        highheight = sp.height + 1f;

        if (!zoneoverride)
        {
            if (!sp.sr.flipX)
                attackPoint.position = new Vector3(player.position.x + hoffset,player.position.y + voffset,0f);
            else
                attackPoint.position = new Vector3(player.position.x - hoffset,player.position.y + voffset,0f);
        }
        else
            Debug.Log("We got overriden");
        
        if (timer != 0)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                timer = 0;
            }
        }

        if (airborne)
        {
            sp.puppet.transform.position = new Vector2(sp.puppet.transform.position.x,airheight);
        }

        //Use this block for ground check
        if (sp.onground)
            charges = 2;

        if (timer == 0 && !delay)
        {
            if (sp.onground && sp.staph == 0)
                sp.shadowb.Sleep();
        }

        if (Input.GetKeyDown("z") && !delay && charges > 0)
        {
            delay = true;
            attack = 1;
            Debug.Log("And swing!");
            sp.canmove = false;
            if (!sp.onground)
            {
                airborne = true;
                airheight = sp.puppet.transform.position.y;
            }
            if (!sp.rb.IsSleeping())
            {
                sp.rb.Sleep();
            }
        }

        if (Input.GetKeyDown("x") && !delay && charges != 0)
        {
            if (Input.GetKey("up"))
            {
                delay = true;
                attack = 2;
                sp.canmove = false;
                if (sp.onground)
                    charges--;
                else
                    charges = 0;
                sp.Jump(5.0f);
            }
        }
        
    }
}
