using MazeGenerator;
using NUnit.Framework;
using System.Linq;

namespace MazeTesting
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestIfAlgoLinksCorrect()
        {
            Wilson wilson = new Wilson();
            var maze = wilson.Create(20, 20);

            foreach (var cell in maze.Cells)
            {
                if (cell.Links.Count == 0)
                {
                    Assert.Fail();
                }
            }
            Assert.Pass();
        }

        [Test]
        public void TestIfAlgoContainsCorrectCountOfCells()
        {
            Wilson wilson = new Wilson();
            var maze = wilson.Create(40, 40);

            
            Assert.That(maze.Rows == 40 && maze.Cols == 40);
        }

    }
}