using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHUD : MonoBehaviour
{
    public Image healthbar;
    public Stats stats;
    public Image stress;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthbar.fillAmount = stats.health/stats.maxHP;
        stress.fillAmount = stats.stress/1f;
        
        if (stats.stress < 0.5f)
        {
            stress.color = new Color32(63,63,12,255);
        }
        if (stats.stress >= 0.5f && stats.stress < 1f)
        {
            stress.color = new Color32(180,40,40,255);
        }

        if (stats.stress >= 1f && stats.state == 2)
        {
            stress.color = new Color32(255,0,0,255);
        }
        
    }
}
