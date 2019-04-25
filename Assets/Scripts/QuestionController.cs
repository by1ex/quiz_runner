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

    public int numToTransfer;
    public float offsetText;
    public float offsetBetweenText;

    public Color wrongColor;
    public Color rightColor;
    public Color neutralColor;

    private int answerInt;


    void Start()
    {
        snap = GetComponentInParent<SnapScrolling>();
        answerInt = question.answer;
        answer[0].SetActive(false);
        answer[1].SetActive(false);
        answer[2].SetActive(false);
        answer[3].SetActive(false);
        if (question.url != "001")
        {
            QuestionImage.sprite = Resources.Load<Sprite>(question.url);
            Question.GetComponent<RectTransform>().offsetMin = new Vector2(0, (3 - question.text.Length / numToTransfer) * offsetText);
        }
        else
        {
            Destroy(QuestionImage.gameObject);
            Question.GetComponent<RectTransform>().offsetMax = new Vector2(0, 180);
            Question.GetComponent<RectTransform>().offsetMin = new Vector2(0, (3 - question.text.Length / numToTransfer) * offsetText+180);
        }
        Question.text = question.text;


        for (int i = 0; i < question.opt.Length; i++)
        {
            answer[i].SetActive(true);
            if (i == 0)
            {
                answer[i].GetComponent<RectTransform>().offsetMax = new Vector2(0, Question.GetComponent<RectTransform>().offsetMin.y);
                answer[i].GetComponent<RectTransform>().offsetMin = new Vector2(0, (3 - question.opt[i].Length / numToTransfer) * offsetText + Question.GetComponent<RectTransform>().offsetMin.y);
            }
            if (i != 0)
            {
                answer[i].GetComponent<RectTransform>().offsetMax = new Vector2(0, (answer[i - 1].GetComponent<RectTransform>().offsetMin.y) - offsetBetweenText);
                answer[i].GetComponent<RectTransform>().offsetMin = new Vector2(0, (3 - question.opt[i].Length / numToTransfer) * offsetText + (answer[i - 1].GetComponent<RectTransform>().offsetMin.y) - offsetBetweenText);
            }
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
            if (!snap.isExam)
            {
                answer[index - 1].GetComponent<Image>().color = rightColor;
                answer[index - 1].transform.GetChild(1).GetComponent<Image>().sprite = Right;
            }
            else
            {
                answer[index - 1].GetComponent<Image>().color = neutralColor;
            }
            snap.ChangeColor(1);
        }
        else
        {
            for (int i = 0; i < question.opt.Length; i++)
            {
                answer[i].GetComponent<Button>().interactable = false;
                if (i == answerInt - 1) continue;
                if (!snap.isExam)
                {
                    answer[i].GetComponent<Image>().color = wrongColor;
                    answer[i].transform.GetChild(1).GetComponent<Image>().sprite = Wrong;
                }
            }
            if (!snap.isExam)
            {
                answer[answerInt - 1].GetComponent<Image>().color = rightColor;
                answer[answerInt - 1].transform.GetChild(1).GetComponent<Image>().sprite = Right;
            }
            else
            {
                answer[index - 1].GetComponent<Image>().color = neutralColor;
            }
            snap.ChangeColor(0);
        }
    }
}
