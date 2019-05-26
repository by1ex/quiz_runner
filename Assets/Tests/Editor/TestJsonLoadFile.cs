using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestJsonLoadFile
{
    [Test]
    public void LoadSuccessTest()
    {
        JsonParsing JsonParser = new JsonParsing();
        int expectedAnswer = JsonParser.PathJsonTest("questions.json");
        Assert.AreEqual(1, expectedAnswer);
    }

    [Test]
    public void LoadUnsuccessTest()
    {
        JsonParsing JsonParser = new JsonParsing();
        int expectedAnswer = JsonParser.PathJsonTest("questions1.json");
        Assert.AreEqual(2, expectedAnswer);
    }
}
