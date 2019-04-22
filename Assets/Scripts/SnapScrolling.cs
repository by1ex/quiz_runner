using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapScrolling : MonoBehaviour
{
    [Range(1,20)]
    public int count=0;
    [Header("Main")]
    public Transform content;
    private GameObject[] instPans;
    public GameObject QuestionPref;
    private int panOffset;
    public ScrollRect scrollRect;
    private Vector2[] pansPos;
    private RectTransform contentRect;
    private Vector2 contentVector;


    public float snapSpeed;
    private int selectedPanID;
    private bool isScrolling;



    private void Start()
    {
        panOffset = Screen.width;
        contentRect = content.GetComponent<RectTransform>();
        instPans = new GameObject[count];
        pansPos = new Vector2[count];
        for (int i = 0; i < count; i++)
        {
            instPans[i] = (GameObject)Instantiate(QuestionPref, content.transform, false);
            if (i == 0) continue;
            instPans[i].transform.localPosition = new Vector2(instPans[i - 1].transform.localPosition.x + panOffset, instPans[i - 1].transform.localPosition.y);
            pansPos[i] = -instPans[i].transform.localPosition;
        }
    }
    private void FixedUpdate()
    {
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

    public void Scrolling(bool scroll)
    {
        isScrolling = scroll;
    }

}
