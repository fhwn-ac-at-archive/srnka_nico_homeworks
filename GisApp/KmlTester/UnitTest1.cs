using GisApp;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KmlTester
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [Test]
        public void TestGetAllNamesFromDbf()
        {
            byte[] alldbfBytes = File.ReadAllBytes("C:\\Users\\nicos\\Downloads\\OGDEXT_GEM_1_STATISTIK_AUSTRIA_20200101\\STATISTIK_AUSTRIA_GEM_20200101Polygon.dbf").ToArray();
            KmlCreator kmlCreator = new KmlCreator();
            var names = kmlCreator.ReturnListOfNamesFromDbf(alldbfBytes);

            Assert.That(names.Length == 2117);
        }

        [Test]
        public void TestGetAllRecords()
        {
            var kmlCreator = new KmlCreator();
            byte[] allShpBytes = File.ReadAllBytes("C:\\Users\\nicos\\Downloads\\OGDEXT_GEM_1_STATISTIK_AUSTRIA_20200101\\STATISTIK_AUSTRIA_GEM_20200101Polygon.shp").ToArray();
            List<byte[]> records = kmlCreator.GetAllRecordsFromShp(allShpBytes);

            Assert.That(records.Count == 2117);
        }

        [Test]
        public void TestGetAllShapes()
        {
            var kmlCreator = new KmlCreator();
            byte[] alldbfBytes = File.ReadAllBytes("C:\\Users\\nicos\\Downloads\\OGDEXT_GEM_1_STATISTIK_AUSTRIA_20200101\\STATISTIK_AUSTRIA_GEM_20200101Polygon.dbf").ToArray();
            byte[] allShpBytes = File.ReadAllBytes("C:\\Users\\nicos\\Downloads\\OGDEXT_GEM_1_STATISTIK_AUSTRIA_20200101\\STATISTIK_AUSTRIA_GEM_20200101Polygon.shp").ToArray();
            List<byte[]> records = kmlCreator.GetAllRecordsFromShp(allShpBytes);

            var names = kmlCreator.ReturnListOfNamesFromDbf(alldbfBytes);
            var shapes = kmlCreator.CreateAllPolygons(records, names);

            Assert.That(shapes.Count == 2117);
        }

    }
}