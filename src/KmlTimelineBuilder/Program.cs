using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace KmlTimelineBuilder {
  class Program {
    static void Main(string[] args) {
      var inputFileName = args[0];


      var files = Directory.GetFiles(inputFileName, "*.csv");
      foreach (var filePath in files) {
        var outputFileName = Path.Combine(inputFileName, Path.GetFileNameWithoutExtension(filePath) + ".kml");

        using (var output = File.OpenWrite(outputFileName)) {
          using (var xml = new XmlTextWriter(output, Encoding.UTF8)) {
            xml.Formatting = Formatting.Indented;
            xml.WriteStartDocument();
            xml.WriteStartElement("kml", "http://earth.google.com/kml/2.2");
            xml.WriteStartElement("Document");
            xml.WriteElementString("name", Path.GetFileNameWithoutExtension(filePath));
            WriteCsvFile(xml, filePath);
            xml.WriteEndElement();  // Document
            xml.WriteEndDocument();
          }
        }
      }
    }

    private static void WriteCsvFile(XmlTextWriter xml, string inputFileName) {
      using (var input = File.OpenRead(inputFileName)) {
        using (var reader = new StreamReader(input)) {
          xml.WriteStartElement("Folder");

          var lineList = new List<Reading>();

          string line;
          reader.ReadLine();
          while ((line = reader.ReadLine()) != null) {
            var reading = new Reading(line);
            if (!lineList.Any()
                || lineList.Last().Latitude != reading.Latitude
                || lineList.Last().Longitude != reading.Longitude) {
              lineList.Add(reading);
            }

            WriteTimelinePlacemark(xml, reading);
          }

          WriteLookAtElement(lineList.First(), xml);

          xml.WriteEndElement(); // Folder

          WriteLookAtElement(lineList.First(), xml);

          xml.WriteStartElement("Style");
          xml.WriteAttributeString("id", "seeadler-dot-icon");
          xml.WriteStartElement("IconStyle");
          xml.WriteStartElement("Icon");
          xml.WriteElementString("href", "http://www.seeadlerpost.com/images/KML/dot.png");
          xml.WriteEndElement(); // Icon
          xml.WriteEndElement(); // IconStyle
          xml.WriteEndElement(); // Style

          xml.WriteStartElement("Placemark");
          xml.WriteElementString("name", "Travel path");

          xml.WriteStartElement("Style");
          xml.WriteStartElement("LineStyle");
          xml.WriteElementString("color", "ff0000ff");
          xml.WriteElementString("width", "2");
          xml.WriteEndElement(); // LineStyle
          xml.WriteEndElement(); // Style

          xml.WriteStartElement("LineString");
          xml.WriteElementString("tessellate", "1");
          xml.WriteElementString("altitudeMode", "clampToGround");
          xml.WriteStartElement("coordinates");
          foreach (var reading in lineList) {
            xml.WriteString(string.Format("{0},{1} ", reading.Longitude, reading.Latitude));
          }
          xml.WriteEndElement(); // coordinates
          xml.WriteEndElement(); // LineString
          xml.WriteEndElement(); // Placemark
        }
      }
    }

    private static void WriteTimelinePlacemark(XmlTextWriter xml, Reading reading) {
      xml.WriteStartElement("Placemark");
      xml.WriteStartElement("TimeStamp");
      xml.WriteElementString("when", reading.DateTime.ToString("u").Replace(' ', 'T'));
      xml.WriteEndElement(); // TimeStamp
      xml.WriteElementString("styleUrl", "#seeadler-dot-icon");
      xml.WriteStartElement("Point");
      xml.WriteElementString("coordinates", string.Format("{0},{1}", reading.Longitude, reading.Latitude));
      xml.WriteEndElement(); // Point
      xml.WriteEndElement(); // Placemark
    }

    private static void WriteLookAtElement(Reading reading, XmlTextWriter xml) {
      xml.WriteStartElement("LookAt");
      xml.WriteElementString("longitude", reading.Longitude.ToString(CultureInfo.InvariantCulture));
      xml.WriteElementString("latitude", reading.Latitude.ToString(CultureInfo.InvariantCulture));
      xml.WriteElementString("range", "250");
      xml.WriteElementString("altitude", reading.Elevation.ToString(CultureInfo.InvariantCulture));
      xml.WriteElementString("tilt", "0");
      xml.WriteEndElement(); // LookAt
    }
  }
}
