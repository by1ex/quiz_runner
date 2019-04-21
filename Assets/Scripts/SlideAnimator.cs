using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SlideAnimator : MonoBehaviour
{
    public Animator anim;
    public GameObject Theme;
    public GameObject Exams;
    public GameObject Rand;
    public GameObject Rule;

    IEnumerator TimerTheme()
    {
        yield return new WaitForSeconds(0.4f);
        Theme.SetActive(false);
    }
    IEnumerator TimerExams()
    {
        yield return new WaitForSeconds(0.4f);
        Exams.SetActive(false);
    }
    IEnumerator TimerRand()
    {
        yield return new WaitForSeconds(0.4f);
        Rand.SetActive(false);
    }
    IEnumerator TimerRule()
    {
        yield return new WaitForSeconds(0.4f);
        Rule.SetActive(false);
    }
    //Theme
    public void OnThemeClick() 
    {
        Theme.SetActive(true);
        anim.SetBool("Active", true);
    }
    public void OnThemeUnclick()
    {
        anim.SetBool("Active", false);
        StartCoroutine("TimerTheme");
    }
    //Exams
    public void OnExamClick()
    {
        Exams.SetActive(true);
        anim.SetBool("Active", true);
    }
    public void OnExamUnclick()
    {
        anim.SetBool("Active", false);
        StartCoroutine("TimerExams");
    }
    //Rand
    public void OnRandClick()
    {
        Rand.SetActive(true);
        anim.SetBool("Active", true);
    }
    public void OnRandUnclick()
    {
        anim.SetBool("Active", false);
        StartCoroutine("TimerRand");
    }
    //Rule
    public void OnRuleClick()
    {
        Rule.SetActive(true);
        anim.SetBool("Active", true);
    }
    public void OnRuleUnclick()
    {
        anim.SetBool("Active", false);
        StartCoroutine("TimerRule");
    }
    public void OnExitClick()
    {
        Application.Quit();
    }
}
