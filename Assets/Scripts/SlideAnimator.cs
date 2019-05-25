using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SlideAnimator : MonoBehaviour
{
    public Animator anim;
    public GameObject Theme;

    IEnumerator TimerTheme()
    {
        yield return new WaitForSeconds(0.4f);
        Theme.SetActive(false);
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


    //Rule
    public void OnRuleClick()
    {
        Application.OpenURL("http://www.pdd24.com");
    }
    public void OnExitClick()
    {
        Application.Quit();
    }
}
