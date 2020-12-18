using Coordinator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;

namespace GisApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var kmlCreator = new KmlCreator();
            //read in both files
            byte[] allShpBytes = File.ReadAllBytes("C:\\Users\\nicos\\Downloads\\OGDEXT_GEM_1_STATISTIK_AUSTRIA_20200101\\STATISTIK_AUSTRIA_GEM_20200101Polygon.shp").ToArray();
            byte[] alldbfBytes = File.ReadAllBytes("C:\\Users\\nicos\\Downloads\\OGDEXT_GEM_1_STATISTIK_AUSTRIA_20200101\\STATISTIK_AUSTRIA_GEM_20200101Polygon.dbf").ToArray();

            //get names from dbf file
            var names = kmlCreator.ReturnListOfNamesFromDbf(alldbfBytes);
            //get all records from shp file
            List<byte[]> records = kmlCreator.GetAllRecordsFromShp(allShpBytes);
            //create shapes for the kml creator and xml creator with given recors and names
            var shapes = kmlCreator.CreateAllPolygons(records, names);
            kmlCreator.CreateKML(shapes);
            Console.WriteLine("Created shape file");
            Console.ReadLine();
        }


    }
}
