/*
	Copyright (c) 2011, Andrew Benz
	All rights reserved.
	
	Redistribution and use in source and binary forms, with or without 
	modification, are permitted provided that the following conditions are met:
	
	Redistributions of source code must retain the above copyright notice, this 
	list of conditions and the following disclaimer.
	Redistributions in binary form must reproduce the above copyright notice, 
	this list of conditions and the following disclaimer in the documentation 
	and/or other materials provided with the distribution.
	THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
	AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
	IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
	ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE 
	LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
	CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
	SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
	INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
	CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
	ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF 
	THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace KmlTimelineBuilder {
  class Program {
    static void Main(string[] args) {
      var inputPath = args[0];

      var files = Directory.GetFiles(inputPath, "*.csv");
      foreach (var filePath in files) {
        var outputFileName = Path.Combine(inputPath, Path.GetFileNameWithoutExtension(filePath) + ".kml");

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
          xml.WriteElementString("name", "Timeline data");

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

          var sampleReading = new Reading()
                              {
                                Latitude = GetMiddleValue(lineList, x => x.Latitude),
                                Longitude = GetMiddleValue(lineList, x => x.Longitude), 
                                Elevation = lineList.Select(x => x.Elevation).Max()
                              };

          WriteLookAtElement(sampleReading, xml);

          xml.WriteEndElement(); // Folder

          WriteLookAtElement(sampleReading, xml);
          WriteIconStyle(xml);
          WriteLineString(xml, lineList);
        }
      }
    }

    private static decimal GetMiddleValue(List<Reading> lineList, Func<Reading, decimal> selector) {
      return (new[] {
                      lineList.Select(selector).Max(),
                      lineList.Select(selector).Min()
                    }).Average();
    }

    private static void WriteLineString(XmlTextWriter xml, List<Reading> lineList) {
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

    private static void WriteIconStyle(XmlTextWriter xml) {
      xml.WriteStartElement("Style");
      xml.WriteAttributeString("id", "seeadler-dot-icon");
      xml.WriteStartElement("IconStyle");
      xml.WriteStartElement("Icon");
      xml.WriteElementString("href", "http://www.seeadlerpost.com/images/KML/dot.png");
      xml.WriteEndElement(); // Icon
      xml.WriteEndElement(); // IconStyle
      xml.WriteEndElement(); // Style
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
