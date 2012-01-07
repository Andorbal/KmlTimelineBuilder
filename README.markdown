# KML Timeline Builder

This is a tool that will convert a directory full of .csv files into .kml files that will show as a timeline in [Google Earth](http://www.google.com/earth/download/ge/).

## Requirements

[Microsoft .NET 4.0 client profile](http://www.microsoft.com/download/en/details.aspx?id=24872) is required to use this tool.
It can also run on Linux using Mono.  It has been tested against version 2.10.

## Usage

To build the tool, run ``build.bat`` if you are building on Windows or ``./build.sh`` if you are building on Linux.

To use the tool, run it with the following syntax:

``KmlTimelineBuilder c:\path\to\data``

After the .kml files are built, just open them up in Google Earth and use the built in tools to view and edit the data.  You can change the line color, names, and any other piece of information in Google Earth.  If you do this, ensure that you move the .kml files out of the original path, because re-running the tool will overwrite the files.

## Expected Input Format

``Date/Time,Validity,Latitude,Longitude,Speed,True Course,Variation,Checksum,Elevation``

## License

KmlTimelineBuilder is licensed under the [BSD license](http://www.opensource.org/licenses/bsd-license.php).
