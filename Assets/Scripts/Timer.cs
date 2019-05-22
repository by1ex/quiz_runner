using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [HideInInspector]
    public float secondsTime;

    public Text TextTimer;
    private SnapScrolling win;

    bool flag = true;
    public bool start = false;

    public Color clrStart;
    public Color clrEnd;

    private float tempTime;

    void Start()
    {
        win = GetComponent<SnapScrolling>();
    }

    public void Starting(float startingTime)
    {
        tempTime = startingTime;
        flag = true;
        start = true;
        secondsTime = startingTime;
        TextTimer.color = clrStart;
    }

    void Update()
    {
        if (start)
        {
            TextTimer.color = Color.Lerp(clrStart, clrEnd, (tempTime - secondsTime)/ tempTime);
            secondsTime -= 1 * Time.deltaTime;
            TextTimer.text = string.Format("{00:00}:{01:00}", (int)(secondsTime / 60), (int)(secondsTime % 60));

            if (secondsTime <= 0)
            {
                if (flag)
                {
                    start = false;
                    flag = false;
                    win.Win();
                }

            }
        }
    }
}
