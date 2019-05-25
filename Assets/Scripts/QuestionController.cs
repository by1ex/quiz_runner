using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class QuestionController : MonoBehaviour
{
    public Sprite Right;
    public Sprite Wrong;

    public int indexTheme;
    public int indexQuestions;
    private JsonParsing jsonP;

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

    private Texture2D texture;
    private Sprite sprite;

    public int answerInt;

    private string path;
    public string folder = "images";

    private void OnDestroy()
    {
        Destroy(texture);
        Destroy(sprite);
    }

    IEnumerator LoadSprite(string imageName)
    {
        byte[] bytes = null;
        string pathFile = Path.Combine(path, imageName + ".jpg");

#if UNITY_EDITOR
        var file = new FileInfo(pathFile);
        if (file.Exists)
        {
            bytes = File.ReadAllBytes(pathFile);
            texture = new Texture2D(604, 225, TextureFormat.DXT1, false, true);
            texture.LoadImage(bytes);
            sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }
        else
        {
        }
#elif UNITY_ANDROID && !UNITY_EDITOR
        UnityWebRequest www = UnityWebRequest.Get(pathFile);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Error");
        }
        else
        {
            bytes = www.downloadHandler.data;
            Texture2D texture = new Texture2D(750, 290, TextureFormat.DXT1, false, true);
            texture.LoadImage(bytes);
            sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }
#elif !UNITY_EDITOR
        var file = new FileInfo(pathFile);
        if (file.Exists)
        {
            bytes = File.ReadAllBytes(pathFile);
            texture = new Texture2D(750, 290, TextureFormat.DXT1, false, true);
            texture.LoadImage(bytes);
            sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }
        else
        {
        }
#endif
        QuestionImage.sprite = sprite;
        yield break;
    }



    void Start()
    {
        jsonP = Camera.main.GetComponent<JsonParsing>();
        path = Path.Combine(Application.streamingAssetsPath, folder);
        snap = GetComponentInParent<SnapScrolling>();
        answerInt = question.answer;
        answer[0].SetActive(false);
        answer[1].SetActive(false);
        answer[2].SetActive(false);
        answer[3].SetActive(false);
        offsetText *= snap.kh;
        offsetBetweenText = snap.kh;
        if (question.url != "001")
        {
            StartCoroutine(LoadSprite(question.url));
            Question.GetComponent<RectTransform>().offsetMin = new Vector2(0, (3 - question.text.Length / numToTransfer) * offsetText);
        }
        else
        {
            Destroy(QuestionImage.gameObject);
            Question.GetComponent<RectTransform>().offsetMax = new Vector2(0, 180 * snap.kh);
            Question.GetComponent<RectTransform>().offsetMin = new Vector2(0, (3 - question.text.Length / numToTransfer) * offsetText + (180 * snap.kh));
        }
        Question.text = question.text;
        Question.fontSize = Mathf.RoundToInt((float)Question.fontSize * snap.kw);

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
            answer[i].GetComponentInChildren<Text>().fontSize = Mathf.RoundToInt((float)answer[i].GetComponentInChildren<Text>().fontSize * snap.kw);
            int t = i + 1;
            answer[i].GetComponent<Button>().onClick.AddListener(() => OnClick(t));
        }
    }

    public int OnClickTest(int index)
    {
        if (index > answer.Length || index < 0)
        {
            return 0;
        }
        else if (index == answerInt)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }

    public void OnClick(int index)
    {
        if (snap.isScrolling)
        {
            return;
        }
        if (index == answerInt)
        {
            if (indexTheme != -1 && indexQuestions != -1)
                jsonP.Theme[indexTheme].Questions.ToArray()[indexQuestions].stats = 1;

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
            if (indexTheme != -1 && indexQuestions != -1)
                jsonP.Theme[indexTheme].Questions.ToArray()[indexQuestions].stats = 2;

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
