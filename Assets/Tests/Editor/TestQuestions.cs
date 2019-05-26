using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestQuestions
{
    [Test]
    public void TestRightAnswer()
    {
        QuestionController question = new QuestionController();
        question.answer = new GameObject[4];
        for(int i = 0; i < 4; i++)
        {
            question.answer[i] = new GameObject();
        }
        question.answerInt = 1;
        int expectedAnswer = question.OnClickTest(1);
        Assert.AreEqual(1, expectedAnswer);
    }
    [Test]
    public void TestRightWrong()
    {
        QuestionController question = new QuestionController();
        question.answer = new GameObject[4];
        for (int i = 0; i < 4; i++)
        {
            question.answer[i] = new GameObject();
        }
        question.answerInt = 1;
        int expectedAnswer = question.OnClickTest(2);
        Assert.AreEqual(2, expectedAnswer);
    }
    [Test]
    public void TestRightOverflowPositive()
    {
        QuestionController question = new QuestionController();
        question.answer = new GameObject[4];
        for (int i = 0; i < 4; i++)
        {
            question.answer[i] = new GameObject();
        }
        question.answerInt = 1;
        int expectedAnswer = question.OnClickTest(5);
        Assert.AreEqual(0, expectedAnswer);
    }
    [Test]
    public void TestRightOverflowNegative()
    {
        QuestionController question = new QuestionController();
        question.answer = new GameObject[4];
        for (int i = 0; i < 4; i++)
        {
            question.answer[i] = new GameObject();
        }
        question.answerInt = 1;
        int expectedAnswer = question.OnClickTest(-1);
        Assert.AreEqual(0, expectedAnswer);
    }
}
