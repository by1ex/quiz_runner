using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLoadingSprite
{
    [Test]
    public void LoadSuccessTest()
    {
        QuestionController question = new QuestionController();
        int expectedAnswer = question.LoadSpriteTest("002");
        Assert.AreEqual(1, expectedAnswer);
    }
    [Test]
    public void LoadUnsuccessTest()
    {
        QuestionController question = new QuestionController();
        int expectedAnswer = question.LoadSpriteTest("999");
        Assert.AreEqual(2, expectedAnswer);
    }
}
