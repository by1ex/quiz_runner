using System.IO;
using UnityEngine;

public class JsonParsing : MonoBehaviour
{
    private string json;

    [SerializeField]
    private Ticket ticket = new Ticket();

    public Ticket[] Theme = new Ticket[28];

    private void Start()
    {
        json = Application.streamingAssetsPath + "/questions.json";

        if (File.Exists(json))
        {
            ticket = JsonUtility.FromJson<Ticket>(File.ReadAllText(json));
            foreach (Questions question in ticket.Questions)
            {
                Theme[question.index - 1].Questions.Add(question);

            }
        }
        else Debug.Log("NO load");
    }

}
