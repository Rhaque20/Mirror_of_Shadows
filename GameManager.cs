using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    int m_frameCounter = 0;
    float m_timeCounter = 0.0f;
    float m_lastFramerate = 0.0f;
    public float m_refreshTime = 0.5f;
    public TMP_Text fps;
    // Update is called once per frame

    void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;
    }

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        
    }
}
