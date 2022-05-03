using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public GameObject player;
    public Stats stats;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Chase()
    {
        if (Vector3.Distance(transform.position,player.transform.position) > 2)
        {
            Vector3 moveTowards = player.transform.position - transform.position;
            moveTowards.Normalize();
            float dampening = 75f;
            transform.position = new Vector2(transform.position.x+moveTowards.x/dampening,transform.position.y + moveTowards.y/dampening);
        }
    }

    void Strafe()
    {

    }

    void Flee()
    {
        Vector3 awayFromPlayer = transform.position - player.transform.position;
        awayFromPlayer.Normalize();
        float dampening = 75f;
        transform.position = new Vector2(transform.position.x+awayFromPlayer.x/dampening,transform.position.y + awayFromPlayer.y/dampening);
    }

    // Update is called once per frame
    void Update()
    {
        if (stats.staggertime == 0)
            Chase();
    }
}
