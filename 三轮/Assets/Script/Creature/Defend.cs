using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defend : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isdefend;//是否可以抵抗
    public float defendtime;//抵抗作用时间
    public float defendtimer;//计时器
    void Start()
    {
        defendtimer=0;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.GetComponent<Character>() .Age>=this.GetComponent<Character>().MultiplyAge&&this.GetComponent<Character>().Age<= this.GetComponent<Character>().OldAge)
        {
            isdefend = true;
        }
        defendtimer-=Time.deltaTime;
        if (defendtimer > 0)
        {
            this.GetComponent<Character>().isCanBeEat = false;
        }
        else
        {
            this.GetComponent<Character>().isCanBeEat = true;
        }
    }
}
