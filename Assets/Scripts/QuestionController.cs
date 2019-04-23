using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionController : MonoBehaviour
{
    public Sprite Right;
    public Sprite Wrong;

    public Questions question;
    public Text Question;
    public GameObject[] answer;
    public Image QuestionImage;
    private SnapScrolling snap;

    public Color wrongColor;
    public Color rightColor;

    private int answerInt;

    void Start()
    {
        snap = GetComponentInParent<SnapScrolling>();
        answerInt = question.answer;
        answer[0].SetActive(false);
        answer[1].SetActive(false);
        answer[2].SetActive(false);
        answer[3].SetActive(false);
        QuestionImage.sprite = Resources.Load<Sprite>(question.url);
        Question.text = question.text;
        for (int i = 0; i < question.opt.Length; i++)
        {
            answer[i].SetActive(true);
            answer[i].GetComponentInChildren<Text>().text = question.opt[i];
            int t = i + 1;
            answer[i].GetComponent<Button>().onClick.AddListener(() => OnClick(t));
        }
    }

    public void OnClick(int index)
    {
        if (index == answerInt)
        {
            for (int i = 0; i < question.opt.Length; i++)
                answer[i].GetComponent<Button>().interactable = false;

            answer[index - 1].GetComponent<Image>().color = rightColor;
            answer[index - 1].transform.GetChild(1).GetComponent<Image>().sprite = Right;
            snap.ChangeColor(1);
        }
        else
        {
            for (int i = 0; i < question.opt.Length; i++)
            {
                answer[i].GetComponent<Button>().interactable = false;
                if (i == answerInt - 1) continue;
               
                    answer[i].GetComponent<Image>().color = wrongColor;
                    answer[i].transform.GetChild(1).GetComponent<Image>().sprite = Wrong;
                
            }
         
            answer[answerInt - 1].GetComponent<Image>().color = rightColor;
            answer[answerInt - 1].transform.GetChild(1).GetComponent<Image>().sprite = Right;
            snap.ChangeColor(0);
        }
    }
}
