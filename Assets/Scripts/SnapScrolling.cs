using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapScrolling : MonoBehaviour
{
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

    public Text HeaderText;

    public float snapSpeed;
    private int selectedPanID;
    private bool isScrolling;

    private GameObject TicketObj;
    private Animator anim;

    private Questions[] questions;
    private Questions[] ticket;
    private Ticket[] theme;


    IEnumerator Timer()
    {
        yield return new WaitForSeconds(0.4f);
        Destroy(instPans[selectedPanID]);
        count = 0;
        contentRect.anchoredPosition = new Vector2(0, 0);
    }

    public void OnThemeUnclick()
    {
        for (int i = 0; i < count; i++)
        {
            if (i == selectedPanID) continue;
            Destroy(instPans[i]);
        }
        anim.SetBool("Active", false);
        StartCoroutine("Timer");
    }

    private void Start()
    {
        theme = Camera.main.GetComponent<JsonParsing>().Theme;
        TicketObj = this.gameObject;
        anim = TicketObj.GetComponent<Animator>();
        panOffset = Screen.width;
        contentRect = content.GetComponent<RectTransform>();
        instPans = new GameObject[count];
        pansPos = new Vector2[count];
    }

    private void Fill(int leng)
    {
        for (int i = 0; i < count; i++)
        {
            Destroy(instPans[i]);
        }
        instPans = new GameObject[leng];
        pansPos = new Vector2[leng];
        for (int i = 0; i < leng; i++)
        {
            instPans[i] = (GameObject)Instantiate(QuestionPref, content.transform, false);
            int tmp = i;
            if (i == 0) continue;
            instPans[i].transform.localPosition = new Vector2(instPans[i - 1].transform.localPosition.x + panOffset, instPans[i - 1].transform.localPosition.y);
            pansPos[i] = -instPans[i].transform.localPosition;
        }
        count = leng;
    }

    private void FixedUpdate()
    {
        if (count <= 0) return;
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
        contentVector.x = Mathf.SmoothStep(contentRect.anchoredPosition.x, pansPos[selectedPanID].x, snapSpeed * Time.fixedDeltaTime);
        contentRect.anchoredPosition = contentVector;
        
    }

    public void OnClickToStartExam()
    {
        anim.SetBool("Active", true);
    }

    public void OnClickToStartRand()
    {
        anim.SetBool("Active", true);
    }

    public void OnClickToStartTheme(int indexAll)
    {
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
