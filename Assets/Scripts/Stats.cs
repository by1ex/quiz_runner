using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    public Image[] imagesStats = new Image[26];
    public int[] allResult = new int[26];
    public int[] wrongResult = new int[26];
    public int[] indexArray1 = new int[26];
    public int[] indexArray2 = new int[26];
    public GameObject[] gameObjects = new GameObject[26];

    public List<int> tmp = new List<int>();

    public void Upload(Ticket[] tickets)
    {
        tmp = new List<int>();
        for (int i = 0; i < 26; i++)
        {
            allResult[i] = 0;
            wrongResult[i] = 0;
        }
        for (int i = 0; i < tickets.Length; i++)
        {
            int j = 0, t = 0;
            foreach (Questions question in tickets[i].Questions)
            {
                if (j % 20 != 0 || j == 0)
                {
                    if (question.stats == 1)
                        t++;
                }
                else
                {
                    gameObjects[indexArray1[i]].transform.GetChild(j / 20 - 1).GetChild(1).GetComponentInChildren<Text>().text = t.ToString();
                    gameObjects[indexArray1[i]].transform.GetChild(j / 20 - 1).GetChild(1).GetComponentInChildren<Image>().fillAmount = (float)t / 20.0f;
                    tmp.Add(t);
                    if (question.stats == 1)
                        t = 1;
                    else
                        t = 0;
                }

                if (question.stats != 0)
                {
                    allResult[i]++;
                    if (question.stats == 2)
                    {
                        wrongResult[i]++;
                    }
                }
                j++;
            }
            j--;
            gameObjects[indexArray1[i]].transform.GetChild(j / 20).GetChild(1).GetComponentInChildren<Text>().text = t.ToString();
            gameObjects[indexArray1[i]].transform.GetChild(j / 20).GetChild(1).GetComponentInChildren<Image>().fillAmount = (float)t / (float)(j % 20);
            tmp.Add(t);
        }
        for (int i = 0; i < 26; i++)
        {
            imagesStats[i].fillAmount = (float)wrongResult[indexArray2[i]] / (float)allResult[indexArray2[i]];
        }
    }
}
