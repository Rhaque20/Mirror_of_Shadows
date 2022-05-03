using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StatusChanges : MonoBehaviour
{
    public bool debuffed = false, buffed = false, dot = false;
    public int debuffct = 0,buffct = 0,val = 0, counter = 0;
    //Statuses[] effects;
    Dictionary<float,Statuses> effects;
    //List<Statuses> effects;
    Stats parameters;
    IEnumerator Applied(float duration,float id)
    {
        Statuses temp;
        yield return new WaitForSeconds(duration * 0.9f);

        yield return new WaitForSeconds(duration * 0.1f);
        temp = effects[id];

        if (temp.plotting)
        {
            temp.CancelPlot();
        }
        if (temp.type == 1)
            buffct--;
        else
            debuffct--;
        
        effects.Remove(id);
    }

    void ApplyEffect(float id, float duration, float effect, int type, int stack)
    {
        Statuses temp;

        if (buffct + debuffct >= 10)
            return;
        
        if (effects.ContainsKey(id))
        {
            temp = effects[id];
            if (temp.plotting)
            {
                temp.CancelPlot();
            }

            effects.Remove(id);

        }

        StartCoroutine(Applied(duration,id));
        effects.Add(id,new Statuses(id,effect,type,stack,parameters));
        if (type == 1)
            buffct++;
        if (type == 2)
            debuffct++;
    }



    void Start()
    {
        effects = new Dictionary<float,Statuses>();
    }

    void Update()
    {
        
    }
}