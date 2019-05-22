using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsContoller : MonoBehaviour
{
    public Text Header;
    public Text Stats;
    public Text Time;

    private Sprite tmpSprite;
    public Image Result;

    public void Change(int positive, int count, float time)
    {
        if (count - positive <= 2)
        {
            Header.text = "Билет решен";
            tmpSprite = Resources.Load<Sprite>("win");
            Result.sprite = tmpSprite;
        }
        else
        {
            Header.text = "Билет не решен";
            tmpSprite = Resources.Load<Sprite>("lose");
            Result.sprite = tmpSprite;
        }
        Stats.text = "Результат: " + positive.ToString() + " / " + count.ToString();
        Time.text =  "Время: " + string.Format("{00:00}:{01:00}", (int)(time / 60), (int)(time % 60));
    }
}
