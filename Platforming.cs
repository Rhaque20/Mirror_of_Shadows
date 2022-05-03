using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platforming : MonoBehaviour
{
    public BoxCollider2D ground;
    public Rigidbody2D rb;
    public bool onplatform = false,shootoff = false;
    public float elevation = 0f,delay = 0.0f;

    public Shadowpuppetry sp = null;
    // Start is called before the first frame update

    void OnTriggerExit2D(Collider2D col)
    {
        Debug.Log("Premature getting off");
        //delay = 0.1f;
        
        if (onplatform && col.gameObject.CompareTag("obstacle") && delay == 0.0f)
        {
            if (sp != null)
            {
                sp.onground = false;
                sp.falling = true;
                if (sp.dashed)
                {
                    shootoff = true;
                    Debug.Log("Zipping across!");
                }
            }

            Debug.Log("Fallin through!");
            onplatform = false;
            
            
            
            //sp.ShadowDrop();
            
        }
        
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (delay != 0f)
        {
            delay -= Time.deltaTime;
            if (delay <= 0f)
            {
                delay = 0f;
                
            }
        }
        
        
    }
}
