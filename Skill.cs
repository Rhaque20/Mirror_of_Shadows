using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill")]
public class Skill : ScriptableObject
{
    public new string name;
    public float power;
    public Sprite Icon;
    public float SPcost;
    public int element;
    public int [] skillchain = new int[3];
    public Sprite[] effects;
    public string [] effectname = new string[3];
    //public Statuses[] chains = new Statuses[3];
}