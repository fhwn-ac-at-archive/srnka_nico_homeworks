using Coordinator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace GisApp
{
    public class KmlCreator
    {
        public List<byte[]> GetAllRecordsFromShp(byte[] allBytes)
        {
            List<byte[]> chunks = new List<byte[]>();

            int length;
            int skipValue = 100;

            var header = allBytes.Skip(skipValue).Take(8).ToArray(); //header has 8 bytes
            while (header.Count() != 0)
            {
                // header has 8 bytes -> +8, * 2 because the length is given in words.
                length = (BitConverter.ConvertToInt32(header.Skip(4).Take(4).ToArray(), ByteTypes.BigEndian) * 2) + 8;
                //add all bytes with currente length and given skip value to our chunks list.
                chunks.Add(allBytes.Skip(skipValue).Take(length).ToArray());
                //add length to skip value to start next iteration on new position
                skipValue += length;

                header = allBytes.Skip(skipValue).Take(8).ToArray();
            }

            return chunks;
        }

        public string[] ReturnListOfNamesFromDbf(byte[] dbf)
        {
            // convert now all given bytes to UTF. That returns a very long string which we can fix with regex and replace.
            string convertedString = Encoding.UTF8.GetString(dbf);
            //replace all numbers with "," d means digits from [0-9].
            convertedString = Regex.Replace(convertedString, @"\d", ",");
            //replace multiple spaces with just one space. 	\s Matches any white-space character. + means 1 or more of the preceding expression
            convertedString = convertedString = Regex.Replace(convertedString, @"\s+", " ");
            string[] nameList = convertedString.Split(",");
            // create skippedList because the first line is not needed.
            string[] skippedList = new string[nameList.Length - 1];
            int secCounter = 1;
            for (int i = 0; i < skippedList.Length; i++)
            {
                skippedList[i] = nameList[secCounter];
                secCounter++;
            }
            var names = skippedList.Where(x => x != string.Empty).ToArray();
            return names;
        }

        public void CreateKML(List<PolygonShape> shapes)
        {
            Console.Write("Put in File name for kml: ");
            var filename = Console.ReadLine();
            XmlDocument xmlDocument = new XmlDocument();
            XmlDeclaration xDec = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);

            XmlElement rootNode = xmlDocument.CreateElement("kml");
            rootNode.SetAttribute("xmlns", @"http://www.opengis.net/kml/2.2");
            xmlDocument.InsertBefore(xDec, xmlDocument.DocumentElement);
            xmlDocument.AppendChild(rootNode);


            //iterate trough all the generates shapes and place it into the xml document
            foreach (var shape in shapes)
            {
                var documentElement = xmlDocument.CreateElement("Document");
                var placemarkNode = xmlDocument.CreateElement("Placemark");
                rootNode.AppendChild(documentElement);
                documentElement.AppendChild(placemarkNode);

                var nameElement = xmlDocument.CreateElement("name");
                var nameText = xmlDocument.CreateTextNode(shape.Name);
                var polygonNode = xmlDocument.CreateElement("Polygon");
                placemarkNode.AppendChild(polygonNode);
                placemarkNode.AppendChild(nameElement);
                nameElement.AppendChild(nameText);

                var outerBoundaryNode = xmlDocument.CreateElement("outerBoundaryIs");

                var outerLinearRingNode = xmlDocument.CreateElement("LinearRing");
                XmlElement innerLinearRingNode = null;

                placemarkNode.AppendChild(polygonNode);
                polygonNode.AppendChild(outerBoundaryNode);
                outerBoundaryNode.AppendChild(outerLinearRingNode);

                if (shape.Polygon.InnerRingPoints.Count != 0)
                {
                    var innerBorderNode = xmlDocument.CreateElement("innerBoundaryIs");
                    innerLinearRingNode = xmlDocument.CreateElement("LinearRing");
                    polygonNode.AppendChild(innerBorderNode);
                    innerBorderNode.AppendChild(innerLinearRingNode);
                }

                var outCoordinateNodes = xmlDocument.CreateElement("coordinates");

                //iterate through outer ring points of current shape 
                foreach (var item in shape.Polygon.OuterRingPoints)
                {
                    var coords = new Coordinates(item.X, item.Y);
                    //using provided Coordinator converter for austria lambert
                    var cordWGS84 = Coordinator.Converter.AustriaLambertToWGS84(coords);
                    // replace , with . for coords
                    var coordTextForNode = xmlDocument.CreateTextNode($"\n{cordWGS84.X.ToString().Replace(',', '.')}," +
                        $" {cordWGS84.Y.ToString().Replace(',', '.')}");
                    outCoordinateNodes.AppendChild(coordTextForNode);
                }

                //same thing with the inner ring points
                if (shape.Polygon.InnerRingPoints.Count != 0)
                {
                    var innderCoordNodes = xmlDocument.CreateElement("coordinates");
                    //now iterate through the inner ring with current shape
                    foreach (var item in shape.Polygon.InnerRingPoints)
                    {
                        var coordinates = new Coordinates(item.X, item.Y);
                        //using provided Coordinator converter for austria lambert
                        var cordWGS84 = Coordinator.Converter.AustriaLambertToWGS84(coordinates);
                        // replace , with . for coords
                        var coordTextForNode = xmlDocument.CreateTextNode($"\n{cordWGS84.X.ToString().Replace(',', '.')}, {cordWGS84.Y.ToString().Replace(',', '.')}");
                        innderCoordNodes.AppendChild(coordTextForNode);
                    }

                    innerLinearRingNode.AppendChild(innderCoordNodes);
                }

                outerLinearRingNode.AppendChild(outCoordinateNodes);
            }
            //save the document with the given filename to build folder
            xmlDocument.Save(filename + ".kml");

        }


        public List<PolygonShape> CreateAllPolygons(List<byte[]> records, string[] names)
        {
            List<Polygon> polys = new List<Polygon>();
            List<PolygonShape> polyShapes = new List<PolygonShape>();
            int nameIndex = 0;

            foreach (var byteChunk in records)
            {
                //assign xMin and xMax, yMin and yMax (byte position like in documentation)
                double xMin = BitConverter.ConvertToDouble(byteChunk.Skip(12).Take(8).ToArray());
                double yMin = BitConverter.ConvertToDouble(byteChunk.Skip(20).Take(8).ToArray());
                double xMax = BitConverter.ConvertToDouble(byteChunk.Skip(28).Take(8).ToArray());
                double yMax = BitConverter.ConvertToDouble(byteChunk.Skip(36).Take(8).ToArray());

                // Assign Number of Parts and points (byte position like in documentation with skip and take ...) 
                int numberOfParts = BitConverter.ConvertToInt32(byteChunk.Skip(44).Take(4).ToArray());
                int numberOfPoints = BitConverter.ConvertToInt32(byteChunk.Skip(48).Take(4).ToArray());
                // if current chunk has only one part
                if (numberOfParts == 1)
                {
                    //skip 52 bytes like in definition and take number of parts * 4
                    int parts = BitConverter.ConvertToInt32(byteChunk.Skip(52).Take(numberOfParts * 4).ToArray());
                    // 8 bytes for double x und double y
                    byte[] pointBytes = byteChunk.Skip(52 + 4 * numberOfParts).Take(numberOfPoints * 20).ToArray(); 
                    int skipBytes = 0;
                    List<Point> pointsOuterRing = new List<Point>();
                    //iterate through number of points 
                    for (int i = 0; i < numberOfPoints; i++)
                    {
                        //extract x as double on with current skip. first time skip = 0 and we take 8 bytes
                        double x = BitConverter.ConvertToDouble(pointBytes.Skip(skipBytes).Take(8).ToArray());
                        //skip +8 bytes because we took 8 already double has 8 bytes
                        skipBytes += 8;
                        //extract y as double on with current skip. first time skip = 0 and we take 8 bytes
                        double y = BitConverter.ConvertToDouble(pointBytes.Skip(skipBytes).Take(8).ToArray());
                        //skip +8 bytes because we took 8 already double has 8 bytes
                        skipBytes += 8;
                        Point point = new Point(x, y);
                        //check if point is inside the bounding box and only add if is true
                        if (point.X >= xMin && point.X <= xMax && point.Y <= yMax && point.Y >= yMin ) 
                        {
                            pointsOuterRing.Add(point);
                        }
                    }
                    //add the iteration to the polyShapes if count is not 0 
                    if (pointsOuterRing.Count() != 0)
                    {
                        Polygon polygon = new Polygon(new PolygonBoundingBox(xMin, yMin, xMax, yMax), pointsOuterRing, new List<Point>());
                        polys.Add(polygon);
                        //add the poly with the name of the city
                        polyShapes.Add(new PolygonShape(polygon, names[nameIndex]));
                    }
                }
                // if current chunk has two parts
                else if (numberOfParts == 2)
                {
                    int index = 0;
                    List<Point> pointsOuterRing = new List<Point>();
                    List<Point> pointsInnerRing = new List<Point>();
                    //skip 52 bytes like in definition and first ring start index which has 4 bytes
                    int firstRingStartIndex = BitConverter.ConvertToInt32(byteChunk.Skip(52).Take(4).ToArray());
                    //skip 56 bytes now because we took the next for and take second ring start index which has 4 bytes
                    int secondRingStartIndex = BitConverter.ConvertToInt32(byteChunk.Skip(56).Take(4).ToArray());
                    // skip 52 bytes like in definition and take points of number of parts which is 2 * 4
                    byte[] pointBytes = byteChunk.Skip(52 + 4 * numberOfParts).ToArray();
                    int skipValue = 0;
                    for (int i = 0; i < numberOfPoints; i++)
                    {
                        //extract x as double on with current skip. first time skip = 0 and we take 8 bytes
                        double x = BitConverter.ConvertToDouble(pointBytes.Skip(skipValue).Take(8).ToArray());
                        //skip +8 bytes because we took 8 already double has 8 bytes
                        skipValue += 8;
                        //extract y as double on with current skip. first time skip = 0 and we take 8 bytes
                        double y = BitConverter.ConvertToDouble(pointBytes.Skip(skipValue).Take(8).ToArray());
                        //skip +8 bytes because we took 8 already double has 8 bytes
                        skipValue += 8; 

                        Point point = new Point(x, y);
                        //check if point is inside the bounding box and only add if is true
                        if (point.X >= xMin && point.X <= xMax && point.Y <= yMax && point.Y >= yMin )
                        {
                            // if current index == second ring start index 
                            if (index == secondRingStartIndex)
                            {
                                pointsInnerRing.Add(point);
                            }
                            // else add it to outer ring
                            else
                            {
                                pointsOuterRing.Add(point);
                            }
                        }
                        // if current index is smaler than the second ring start index add 1 to index
                        if (index < secondRingStartIndex)
                        {
                            index++;
                        }
                    }
                    //add the iteration to the polyShapes if count is not 0 
                    if (pointsOuterRing.Count() != 0)
                    {
                        Polygon polygon = new Polygon(new PolygonBoundingBox(xMin, yMin, xMax, yMax), pointsOuterRing, pointsInnerRing);
                        polys.Add(polygon);
                        //add the poly with the name of the city
                        polyShapes.Add(new PolygonShape(polygon, names[nameIndex]));
                        //increase name Index
                        nameIndex++;
                    }
                }
            }
            return polyShapes;
        }
    }
}
