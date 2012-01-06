using System;

namespace KmlTimelineBuilder {
  public class Reading {
    public Reading(string line) {
      string[] pieces = line.Split(',');
      DateTime = DateTime.Parse(pieces[0]);
      Validity = pieces[1];
      Latitude = Decimal.Parse(pieces[2]);
      Longitude = Decimal.Parse(pieces[3]);
      Speed = Decimal.Parse(pieces[4]);
      TrueCourse = Decimal.Parse(pieces[5]);
      Variation = pieces[6];
      Checksum = pieces[7];
      Elevation = Decimal.Parse(pieces[8].Replace("M", string.Empty));
    }

    public Reading() {}

    public DateTime DateTime { get; set; }
    public string Validity { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public decimal Speed { get; set; }
    public decimal TrueCourse { get; set; }
    public string Variation { get; set; }
    public string Checksum { get; set; }
    public decimal Elevation { get; set; }
  }
}