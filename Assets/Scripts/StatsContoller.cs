using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsContoller : MonoBehaviour
{
    public Text Header;
    public Text Stats;

    private Sprite tmpSprite;
    public Image ResImage;

    public void Change(int positive, int count)
    {
        if (count - positive <= 2)
        {
            Header.text = "Билет решен";
            tmpSprite = Resources.Load<Sprite>("win");
            ResImage.sprite = tmpSprite;
        }
        else
        {
            Header.text = "Билет не решен";
            tmpSprite = Resources.Load<Sprite>("lose");
            ResImage.sprite = tmpSprite;
        }
        Stats.text = "Результат: " + positive.ToString() + " / " + count.ToString();
    }
}
