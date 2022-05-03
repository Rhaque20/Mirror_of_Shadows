using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statuses : MonoBehaviour
{
    public float id,stack;
    public float effect;
    public int type;
    public float interval = 2.5f;
    public bool plotting = false;

    Coroutine plot;
    Stats parameters;
    
    
    public Statuses(float id, float effect, int type, int stack,Stats para)
    {
        this.id = id;
        this.effect = effect;
        this.type = type;
        this.stack = stack;
        parameters = para;

    }

    public void CancelPlot()
    {
        if (plot != null)
            StopCoroutine(plot);
    }
    IEnumerator DoT()
    {
        yield return new WaitForSeconds(5f);
        if (id == 4)
        {
            parameters.health -= this.effect;
        }
        if (id == 7)
        {
            parameters.curSP -= this.effect;
        }
        plot = StartCoroutine(DoT());
    }

    IEnumerator Healing()
    {
        yield return new WaitForSeconds(5f);
        if (id == 4)
        {
            parameters.health += this.effect;
        }
        if (id == 7)
        {
            parameters.curSP += this.effect;
        }
        plot = StartCoroutine(Healing());
    }

    void Update()
    {
        if (!plotting)
        {
            if (id == 4 || id == 7)
            {
                plotting = true;
                if (type == 1)
                    plot = StartCoroutine(Healing());
                else
                    plot = StartCoroutine(DoT());
            }
            if (id == 8)
            {
                plotting = true;
                plot = StartCoroutine(DoT());
            }
            
        }
    }
}