using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyNav : MonoBehaviour
{
    public Transform target;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public SpriteRenderer shadow,puppet;
    public Stats stats;
    public bool canMove = true;
    public int mode = 1;
    public Animator anim;

    Path path;
    int currentWaypoint = 0;
    bool endofpath = false;

    Seeker seeker;
    public Rigidbody2D rigy;
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath()
    {
        Vector2 goal;
        if (seeker.IsDone())
        {
            switch(mode)
            {
                case 1:
                    if (target.position.x >= transform.position.x)
                        goal = new Vector2(target.position.x - 1f, target.position.y);
                    else
                        goal = new Vector2(target.position.x + 1f,target.position.y);
                    
                    seeker.StartPath(rigy.position, goal, OnPathComplete);
                    break;
            }
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void Update()
    {
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null)
            return;
        if (currentWaypoint >= path.vectorPath.Count)
        {
            endofpath = true;
            return;
        }
        else
        {
            endofpath = false;
        }

        float distanceTarget = Vector3.Distance(transform.position,target.position);
        
        if (stats.staggertime == 0)
        {
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rigy.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;
            rigy.AddForce(force * rigy.mass);
            //transform.position += (Vector3)(force * Time.deltaTime);
        }
        float distance = Vector2.Distance(rigy.position,path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (rigy.velocity.x >= 0.01f)
        {
            puppet.flipX = true;
        }

        if (rigy.velocity.x <= 0.01f)
        {
            puppet.flipX = false;
        }

        if (rigy.velocity.magnitude != 0)
            anim.SetBool("moving",true);
        else
            anim.SetBool("moving",false);
    }
}
