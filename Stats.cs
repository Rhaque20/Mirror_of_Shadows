using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Stats : MonoBehaviour
{
    public float health,maxHP,attack,defense,critrate,critdmg,stress = 0f,curSP = 0f,maxSP = 0f,
    maxArmor = 0f,armor = 0f, potency = 0f, resistance = 0f;
    public int level;
    [HideInInspector] public float damage;
    public int id,state = 0;
    [HideInInspector]public float mercy = 0f,relax = 0f;
    public float staggertime = 0f;
    public float low,high,armorregen = 0f;
    public bool natarmor = false;
    public SpriteRenderer sr;
    public float height;


    // DamageText Variables
    public GameObject damageTextPrefab, enemyInstance,criticalTextPrefab;
    public Vector3 positioning;
    // Start is called before the first frame update
    void Start()
    {
        height = sr.bounds.size.y;
        switch(id)
        {
            case 1:
            health = 145f + ((1073f/99f)*level);
            health = Mathf.Round(health);
            attack = 45f + ((480f/99f)*(level));
            defense = 36f + ((322f/99f)*level);
            critrate = 5f;
            critdmg = 50f;
            curSP = 100f;
            break;
        }
        maxHP = health;
        maxSP = curSP;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
    }

    void DamagePrint(int type)
    {
        GameObject DamageText;
        positioning = enemyInstance.transform.position;
        positioning = new Vector3(positioning.x + 2f,positioning.y,positioning.z);
        if (type == 1)
            DamageText = Instantiate(damageTextPrefab, positioning,Quaternion.identity);
        else
            DamageText = Instantiate(criticalTextPrefab, positioning,Quaternion.identity);
        
        DamageText.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(damage.ToString());
    }

    public void damagecalc(float atk,float cdmg,float crate,float min,float max,float mod)
    {
        float rng = Random.Range(1f,100f);
        if (mercy == 0)
        {
            damage = atk/(defense/300+1);
            damage *= mod * Random.Range(min,max);
            if (rng <= crate)
                damage *= (1 + cdmg/100);
            damage = Mathf.Round(damage);
            health -= damage;

            if (rng <= crate)
            {
                DamagePrint(2);
                //Debug.Log("CRITICAL!");
            }
            else
                DamagePrint(1);
            mercy = 0.01f;
            if (armor <= 0)
                staggertime = 1f;
            
            if (armor > 0)
                armor -= damage * 0.2f;
            
            if (stress < 1f && state != 2)
            {
                stress += 0.01f;
                relax = 10f;
            }
        }
    }

    void FixedUpdate()
    {
        /**
        if (Input.GetKeyDown("p") && id == 1)
        {
            health -= 30;
            Debug.Log("Cecile took 30 Damage!");
        }

        if (Input.GetKeyDown("o") && id == 1)
        {
            health += 30;
            Debug.Log("Cecile restored 30 HP!");
        }
        **/
    }

    // Update is called once per frame
    void Update()
    {
        if (mercy != 0)
        {
            mercy -= Time.deltaTime;
            if (mercy <= 0)
            {
                mercy = 0;
            }
        }

        if (stress >= 0.5 && state == 0)
        {
            if (natarmor)
                armor = maxArmor * 2;
            else
                armor = 1000f;
            state = 1;
            attack *= 1.5f;
            Debug.Log("Stressed!");
        }

        if (stress >= 1f && state == 1)
        {
            armor = 0f;
            state = 2;
            defense *= 0.5f;
            relax = 0;
            Debug.Log("Overwhelmed!");
        }

        if (armorregen != 0)
        {
            armorregen -= Time.deltaTime;
            if (armorregen <= 0)
            {
                armorregen = 0;
                if (natarmor)
                    armor = maxArmor;
            }
        }

        if (staggertime != 0)
        {
            staggertime -= Time.deltaTime;
            if (staggertime <= 0)
            {
                staggertime = 0;
            }
        }

        if (relax != 0)
        {
            relax -= Time.deltaTime;
            if (relax <= 0)
            {
                relax = 0;
            }
        }

        if (relax == 0 && stress != 0 && state != 0)
        {
            stress -= Time.deltaTime * 0.1f;
            if (stress <= 0)
            {
                stress = 0;
                if (state > 0)
                {
                    attack /= 1.5f;
                    if (state == 2)
                        defense /= 0.5f;
                }
                state = 0;
            }
        }
        
        if (health <= 0)
            health = 0;
        if (health >= maxHP)
            health = maxHP;

    }
}
