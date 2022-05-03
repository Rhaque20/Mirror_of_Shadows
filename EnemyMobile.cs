using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMobile : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;

    public Transform target = null;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position,target.position) > 3f)
        {
            transform.position = Vector2.MoveTowards(transform.position,target.position,speed * Time.deltaTime);
        }
    }
}
