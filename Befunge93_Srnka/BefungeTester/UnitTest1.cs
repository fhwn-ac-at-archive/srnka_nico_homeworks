using Befunge93_Srnka;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BefungeTester
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestCaseFileOne()
        {
            Befunge s = new Befunge("E:\\FH\\semester-3\\Algodat Übung\\Befunge93_Srnka\\Befunge93_Srnka\\bin\\Debug\\testcaseone.bf");
            s.CalcBefunge();
            Assert.That(s.Output[0] == "H");
        }
        [Test]
        public void TestCaseFileTwo()
        {
            Befunge s = new Befunge("E:\\FH\\semester-3\\Algodat Übung\\Befunge93_Srnka\\Befunge93_Srnka\\bin\\Debug\\testcasetwo.bf");
            s.CalcBefunge();
            string line = "";
            foreach (var item in s.Output)
            {
                if(item != "\n")
                {
                    line += item;
                }
            }
            Assert.That(line == "Hello, World!");
        }
        [Test]
        public void Test_MathOperations()
        {
            Befunge s = new Befunge("E:\\FH\\semester-3\\Algodat Übung\\Befunge93_Srnka\\Befunge93_Srnka\\bin\\Debug\\mathoperations.bf");
            s.CalcBefunge();
            var stack = new List<int> { 2, 0, 6, -2, 10 };
            
            Assert.That(s.BefungeStack.ToList().SequenceEqual(stack));
        }
    }
}