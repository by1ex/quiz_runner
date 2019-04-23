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
        }
    }
}
