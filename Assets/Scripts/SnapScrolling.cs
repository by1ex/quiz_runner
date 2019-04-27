using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapScrolling : MonoBehaviour
{
    [SerializeField]
    private Timer timer;
    private GameObject timerObj;
    public float startingTime;

    private int count=0;
    [Header("Main")]
    public Transform content;
    private GameObject[] instPans;
    public GameObject QuestionPref;
    private int panOffset;
    public ScrollRect scrollRect;
    private Vector2[] pansPos;
    private RectTransform contentRect;
    private Vector2 contentVector;

    [Header("Box")]
    public GameObject BoxPref;
    public Transform BoxParentTransform;
    private int boxOffset;
    private GameObject[] instBoxes;
    private float[] boxRect;
    private Image[] imagesBoxes;
    private RectTransform boxContentRect;
    private Vector2 boxContentVector;
    private ScrollRect boxScrollRect;

    public Text HeaderText;

    public float snapSpeed;
    private int selectedPanID;
    private int HPPanID;
    private int prevSelectedPanID;

    private bool isScrolling;

    //animator controller for animation Ticket
    public GameObject TicketObj;
    private Animator anim;

    //animator controller for animation Win Panel
    public GameObject winObj;
    private Animator winAnim;

    private Questions[] questions;
    private Questions[] ticket;
    private Ticket[] theme;


    public Color wrongColor;
    public Color rightColor;
    public Color enableWrongColor;
    public Color enableRightColor;
    public Color neutralColor;
    public Color enableNeutralColor;

    //The number of correct and incorrect answers
    private int countWrong;
    private int countRight;

    public bool isExam = false;

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(0.4f);
        Destroy(instPans[selectedPanID]);
        for (int i = 0; i < count; i++)
        {
            Destroy(instBoxes[i]);
        }
        contentRect.anchoredPosition = new Vector2(0, 0);
        prevSelectedPanID = 0;
        boxContentRect.anchoredPosition = new Vector2(0, 0);
        count = 0;
    }

    public void OnThemeUnclick()
    {
        for (int i = 0; i < count; i++)
        {
            if (i == selectedPanID) continue;
            Destroy(instPans[i]);
        }
        anim.SetBool("Active", false);
        winAnim.SetBool("Active", false);
        StartCoroutine("Timer");
    }

    public void Win()
    {
        timer.start = false;
        winObj.GetComponent<StatsContoller>().Change(countRight, count, startingTime - timer.secondsTime - 1);
        winAnim.SetBool("Active", true);
    }

    private void Start()
    {
        theme = Camera.main.GetComponent<JsonParsing>().Theme;
        winAnim = winObj.GetComponent<Animator>();
        TicketObj = this.gameObject;
        anim = TicketObj.GetComponent<Animator>();
        boxContentRect = BoxParentTransform.GetComponent<RectTransform>();
        panOffset = Screen.width;
        boxOffset = Mathf.FloorToInt((float)Screen.width / 500.0f * (float)50);
        boxScrollRect = BoxParentTransform.GetComponentInParent<ScrollRect>();
        contentRect = content.GetComponent<RectTransform>();
        instPans = new GameObject[count];
        pansPos = new Vector2[count];
        timer = GetComponent<Timer>();
        timerObj = timer.TextTimer.GetComponentInParent<Image>().gameObject;
    }

    private void Fill(int leng)
    {
        
        HPPanID = 0;
        for (int i = 0; i < count; i++)
        {
            Destroy(instPans[i]);
            Destroy(instBoxes[i]);
        }
        instPans = new GameObject[leng];
        pansPos = new Vector2[leng];
        instBoxes = new GameObject[leng];
        boxRect = new float[leng];
        imagesBoxes = new Image[leng];
        for (int i = 0; i < leng; i++)
        {
            instPans[i] = (GameObject)Instantiate(QuestionPref, content.transform, false);
            instBoxes[i] = (GameObject)Instantiate(BoxPref, BoxParentTransform, false);
            int tmp = i;
            instBoxes[i].GetComponent<Button>().onClick.AddListener(() => OnClick(tmp));
            instPans[i].GetComponent<QuestionController>().question = questions[i];
            imagesBoxes[i] = instBoxes[i].GetComponent<Image>();
            if (i == 0) continue;
            instBoxes[i].transform.localPosition = new Vector2(instBoxes[i - 1].transform.localPosition.x + boxOffset, instBoxes[i - 1].transform.localPosition.y);
            instBoxes[i].GetComponentInChildren<Text>().text = (i + 1).ToString();
            instPans[i].transform.localPosition = new Vector2(instPans[i - 1].transform.localPosition.x + panOffset, instPans[i - 1].transform.localPosition.y);
            boxRect[i] = instBoxes[i].GetComponent<RectTransform>().anchoredPosition.x;
            pansPos[i] = -instPans[i].transform.localPosition;
        }
        count = leng;
        timerObj.SetActive(isExam);
    }

    private void FixedUpdate()
    {
        if (count <= 0) return;
        if (countRight + countWrong == count)
        {
            Win();
            countRight = 0;
            countWrong = 0;
        }
        if (contentRect.anchoredPosition.x > pansPos[0].x + 1 || contentRect.anchoredPosition.x < pansPos[count - 1].x - 1) scrollRect.horizontal = false;
        else scrollRect.horizontal = true;
        float nearestPos = float.MaxValue;
        for (int i = 0; i < count; i++)
        {
            float distance = Mathf.Abs(contentRect.anchoredPosition.x - pansPos[i].x);
            if (distance < nearestPos)
            {
                nearestPos = distance;
                selectedPanID = i;
            }
        }
        if (isScrolling) return;
        if (boxRect[count - 1] > panOffset - 5)
        {
            if (Mathf.Abs(boxContentRect.anchoredPosition.x) > boxRect[count - 1] - panOffset + boxOffset + 5) 
            {
                boxScrollRect.horizontal = false;
                boxContentVector.x = Mathf.SmoothStep(boxContentRect.anchoredPosition.x, -boxRect[count - 1] + panOffset - boxOffset , snapSpeed * Time.fixedDeltaTime);
                boxContentRect.anchoredPosition = boxContentVector;
            }
            else boxScrollRect.horizontal = true;
        }
        if (boxContentRect.anchoredPosition.x > 5)
        {
            
            boxContentVector.x = Mathf.SmoothStep(boxContentRect.anchoredPosition.x, boxRect[0], snapSpeed * Time.fixedDeltaTime);
            boxContentRect.anchoredPosition = boxContentVector;
        }

        if (imagesBoxes[prevSelectedPanID].color == enableNeutralColor)
        {
            imagesBoxes[prevSelectedPanID].color = neutralColor;
        }
        else if (imagesBoxes[prevSelectedPanID].color == enableWrongColor)
        {
            imagesBoxes[prevSelectedPanID].color = wrongColor;
        }
        else if (imagesBoxes[prevSelectedPanID].color == enableRightColor)
        {
            imagesBoxes[prevSelectedPanID].color = rightColor;
        }
        else
        {
            imagesBoxes[prevSelectedPanID].enabled = false;
        }

        if (boxContentRect.anchoredPosition.x + boxRect[selectedPanID] > panOffset - boxOffset)
        {
            if (prevSelectedPanID != selectedPanID || boxContentRect.anchoredPosition.x == 0)
                boxContentVector.x = -(boxRect[selectedPanID] - panOffset + boxOffset);
            else
                boxContentVector.x = boxContentRect.anchoredPosition.x;
            boxContentRect.anchoredPosition = boxContentVector;
        }
        else if (boxContentRect.anchoredPosition.x < -boxRect[selectedPanID])
        {
            if (prevSelectedPanID != selectedPanID)
                boxContentVector.x = -boxRect[selectedPanID];
            else
                boxContentVector.x = boxContentRect.anchoredPosition.x;
            boxContentRect.anchoredPosition = boxContentVector;
        }
        else
        {
            if (HPPanID != 0)
            {
                contentRect.anchoredPosition = new Vector2(pansPos[HPPanID].x + boxOffset, 0);
                selectedPanID = HPPanID;
                HPPanID = 0;
            }
            else
            {
                contentVector.x = Mathf.SmoothStep(contentRect.anchoredPosition.x, pansPos[selectedPanID].x, snapSpeed * Time.fixedDeltaTime);
                contentRect.anchoredPosition = contentVector;
            }
        }
        prevSelectedPanID = selectedPanID;
        if (imagesBoxes[prevSelectedPanID].color == neutralColor)
        {
            imagesBoxes[prevSelectedPanID].color = enableNeutralColor;
        }
        else if (imagesBoxes[selectedPanID].color == wrongColor)
        {
            imagesBoxes[selectedPanID].color = enableWrongColor;
        }
        else if (imagesBoxes[selectedPanID].color == rightColor)
        {
            imagesBoxes[selectedPanID].color = enableRightColor;
        }
        else
        {
            imagesBoxes[selectedPanID].enabled = true;
        }

    }


    public void ChangeColor(int index)
    {
        if (index == 0)
        {
            if (!isExam)
            {
                imagesBoxes[selectedPanID].color = enableWrongColor;
            }
            else
            {
                imagesBoxes[selectedPanID].color = enableNeutralColor;
                if (selectedPanID < count - 1)
                {
                    contentRect.anchoredPosition -= new Vector2(panOffset - boxOffset * 3, 0);
                    HPPanID = ++selectedPanID;
                }
            }
            countWrong++;
        }
        else if (index == 1)
        {
            if (!isExam)
            {
                imagesBoxes[selectedPanID].color = enableRightColor;
            }
            else
            {
                imagesBoxes[selectedPanID].color = enableNeutralColor;
            }
            countRight++;
            if (selectedPanID < count - 1)
            {
                contentRect.anchoredPosition -= new Vector2(panOffset - boxOffset * 3, 0);
                HPPanID = ++selectedPanID;
            }
        }

    }

    private void OnClick(int index)
    {
        if (index > selectedPanID)
        {
            contentRect.anchoredPosition = new Vector2(pansPos[index].x + boxOffset, 0); 
            if (selectedPanID == 0) HPPanID = index;
        }
        else if (index < selectedPanID)
        {
            contentRect.anchoredPosition = new Vector2(pansPos[index].x - boxOffset, 0);
        }
    }

    public void OnClickToStartExam()
    {
        isExam = true;
        timer.Starting(startingTime);
        int[] indexTheme = new int[29];
        questions = new Questions[20];
        int i = 0;
        while (i < 4)
        {
            int randTheme = Random.Range(1, 5);
            if (indexTheme[randTheme] == 0)
            {
                int randQuestion = Random.Range(0, theme[randTheme].Questions.ToArray().Length - 5);
                indexTheme[randTheme] = 1;
                for (int j = 0; j < 5; j++)
                {
                    questions[i * 5 + j] = theme[randTheme].Questions[randQuestion + j];
                }
                i++;
            }
        }
        Fill(questions.Length);
        anim.SetBool("Active", true);
        HeaderText.text = "Экзамен";
    }

    public void OnClickToStartRand()
    {
        isExam = false;
        int[] indexTheme = new int[29];
        int[] indexQuestions = new int[200];
        questions = new Questions[20];
        int i = 0;
        while (i < 20)
        {
            int randTheme = Random.Range(1, 4);
            int randQuestion = Random.Range(0, theme[randTheme].Questions.ToArray().Length);
            if (indexTheme[randTheme] == 0 || indexQuestions[randQuestion] == 0)
            {
                indexTheme[randTheme] = 1;
                indexQuestions[randQuestion] = 1;
                questions[i] = theme[randTheme].Questions[randQuestion];
                i++;
            }
        }
        Fill(questions.Length);
        anim.SetBool("Active", true);
        HeaderText.text = "Случайный билет";
    }

    public void OnClickToStartTheme(int indexAll)
    {
        isExam = false;
        int indexTheme = indexAll / 10;
        int index = indexAll % 10;
        ticket = theme[indexTheme].Questions.ToArray();
        if (ticket.Length - 20 * index <= 0) return;
        if (ticket.Length <= 20 * (index + 1))
        {
            questions = new Questions[ticket.Length - 20 * index];
            for (int i = index * 20, j = 0; i < ticket.Length; i++, j++)
                questions[j] = ticket[i];
        }
        else if (ticket.Length > 20 * (index + 1))
        {
            questions = new Questions[20];
            for (int i = 20 * (index), j = 0; j < 20; i++, j++)
                questions[j] = ticket[i];
        }
        Fill(questions.Length);
        anim.SetBool("Active", true);
        HeaderText.text = "Тема: " + indexTheme.ToString() + " Билет: " + (index + 1).ToString();
    }

    public void Scrolling(bool scroll)
    {
        isScrolling = scroll;
    }

}
