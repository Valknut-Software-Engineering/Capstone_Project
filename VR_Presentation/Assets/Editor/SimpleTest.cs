using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class SimpleTest {

	[Test]
	public void SimpleTest4() {
        // Use the Assert class to test conditions.

        Assert.That(2 + 2, Is.EqualTo(4));
	}

    [Test]
    public void SimpleTest5()
    {
        // Use the Assert class to test conditions.

        Assert.That(2 + 2, Is.Not.EqualTo(5));
    }
}
