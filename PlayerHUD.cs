using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    public Slider hpslider;
    public Stats stats;
    public TMP_Text healthtext;
    public GameObject skillpallete;

    // Start is called before the first frame update
    void Start()
    {
        hpslider.value = 1f;
    }

    public void SetHealth(float health)
    {
        healthtext.text = stats.health.ToString()+"/"+stats.maxHP.ToString();
        hpslider.value = health;
    }

    // Update is called once per frame
    void Update()
    {
        SetHealth(stats.health/stats.maxHP);
        if (Input.GetKeyDown("x"))
        {
            Time.timeScale = 0.5f;
            skillpallete.SetActive(true);
        }
        if (Input.GetKeyUp("x"))
        {
            Time.timeScale = 1f;
            skillpallete.SetActive(false);
        }
    }
}
