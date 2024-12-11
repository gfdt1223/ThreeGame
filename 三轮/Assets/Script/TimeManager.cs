using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    // Start is called before the first frame update
    public float TimeScale;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = TimeScale;
    }
}
