using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    public BoxCollider2D bounds;
    public bool phasable = false;
    public Beatemupmove bm;
    public float timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (bounds.isTrigger == true)
            bounds.isTrigger = false;
        
        if (Input.GetKey("space"))
            Debug.Log("Registered Space input");
        if (collision.gameObject.CompareTag("Player") && Input.GetKey("space"))
        {
            Debug.Log("Hello");
            bounds.isTrigger = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
