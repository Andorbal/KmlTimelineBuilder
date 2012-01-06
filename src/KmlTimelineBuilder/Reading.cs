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