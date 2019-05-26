using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;


public class JsonParsing : MonoBehaviour
{
    private string json;
    private string pathDownload;
    private string path;

    private Stats stats;

    public string fileName = "questions.json";

    [SerializeField]
    private Ticket ticket = new Ticket();

    public Ticket[] Theme = new Ticket[26];

    IEnumerator load()
    {
        UnityWebRequest www = UnityWebRequest.Get(pathDownload);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Error");
        }
        else
        {
            json = www.downloadHandler.text;
            File.WriteAllText(path, json);
        }

        ticket = JsonUtility.FromJson<Ticket>(json);
        foreach (Questions question in ticket.Questions)
        {
            Theme[question.index - 1].Questions.Add(question);
        }

        yield return new WaitForSeconds(0.1f);
    }


    public void WriteStats()
    {
        Ticket ticketToStats = new Ticket();
        for (int i = 0; i < 26; i++)
        {
            foreach (Questions question in Theme[i].Questions)
            {
                ticketToStats.Questions.Add(question);
            }
        }
        string ToJson = JsonUtility.ToJson(ticketToStats, true);
        File.WriteAllText(path, ToJson);
        stats.Upload(Theme);
    }

    public int PathJsonTest(string name)
    {
        path = Path.Combine(Application.streamingAssetsPath, name);
        if (File.Exists(path))
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }

    private void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        path = Path.Combine(Application.persistentDataPath, fileName);
#else
        path = Path.Combine(Application.dataPath, fileName);
#endif
        stats = GetComponent<Stats>();
        pathDownload = Path.Combine(Application.streamingAssetsPath, fileName);

        if (File.Exists(path))
        {
            json = File.ReadAllText(path);
            ticket = JsonUtility.FromJson<Ticket>(json);
            foreach (Questions question in ticket.Questions)
            {
                Theme[question.index - 1].Questions.Add(question);
            }
        }
        else
        {

#if UNITY_ANDROID && !UNITY_EDITOR
        StartCoroutine("load");
#else
            json = File.ReadAllText(pathDownload);
            File.WriteAllText(path, json);
            ticket = JsonUtility.FromJson<Ticket>(json);
            foreach (Questions question in ticket.Questions)
            {
                Theme[question.index - 1].Questions.Add(question);
            }
#endif
        }
        WriteStats();
    }
}
