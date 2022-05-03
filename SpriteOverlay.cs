using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOverlay : MonoBehaviour
{
    public SpriteRenderer sr;
    public BoxCollider2D box;

    public Shadowpuppetry sp;
    void OnTriggerStay2D(Collider2D collision)
    {
        SpriteRenderer s;
        if (collision.gameObject.CompareTag("puppet"))
        {
            if (collision.gameObject.transform.position.y > sp.axisY && !(sp.onground))
            {
                s = collision.gameObject.GetComponent<SpriteRenderer>();
                if (s != null)
                    sr.sortingOrder = s.sortingOrder + 1;
            }
            if (sp.onground)
            {
                if (collision.gameObject.transform.position.y < sp.shaspot.position.y)
                {
                    s = collision.gameObject.GetComponent<SpriteRenderer>();
                    if (s != null)
                        sr.sortingOrder = s.sortingOrder - 1;
                }
                else
                {
                    sr.sortingOrder = 0;
                }
            }
        }

        if (collision.gameObject.CompareTag("obstacle"))
        {
            if (collision.gameObject.transform.position.y < sr.transform.position.y && !(sp.onground))
            {
                Debug.Log("Overlapping");
                s = collision.gameObject.GetComponent<SpriteRenderer>();

                if (s != null)
                {
                    
                    sr.sortingOrder = s.sortingOrder + 1;
                    //sp.shadow.sortingOrder = 1;
                }
            }
            else
            {
                Debug.Log("Height of Object: "+collision.gameObject.transform.position.y+" AxisY is: "+sp.axisY);
            }
            
        }
    }

    void onTriggerExit2D(Collider2D collision)
    {
        if (sp.onground)
        {
            sr.sortingOrder = 0;

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
