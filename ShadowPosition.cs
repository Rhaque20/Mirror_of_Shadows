using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPosition : MonoBehaviour
{
    [SerializeField] private float[] shadowoffset = new float[2];
    [SerializeField] private float[] puppetoffset = new float[2];
    public SpriteRenderer puppet;

    public void Positioning(Vector2 pos)
    {

        transform.position = new Vector2(pos.x + shadowoffset[0], pos.y + shadowoffset[1]);
    }
    public void Positioning(float x, float y)
    {

        transform.position = new Vector2(x + shadowoffset[0], y + shadowoffset[1]);
    }

    public void PuppetPositioning(Vector2 pos)
    {
        puppet.transform.position = new Vector2(pos.x + puppetoffset[0], pos.y + puppetoffset[1]);
    }
    public void PuppetPositioning(float x, float y)
    {

        puppet.transform.position = new Vector2(x + puppetoffset[0], y + puppetoffset[1]);
    }

    public void PuppetJumping(Vector2 pos)
    {
        Debug.Log("Called!");
        puppet.transform.position = new Vector2(pos.x + puppetoffset[0], pos.y);
    }
    
}