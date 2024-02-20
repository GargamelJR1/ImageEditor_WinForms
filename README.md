# ImageEditor (C++/C#/x86-64 Assembly)
Simple Image Editor with:
* sepia filter
* grayscale filter
* negative filter
* image adjustments
    * brightness
    * contrast
    * transparency

All functions are implemented in C++, C# and x86-64 Assembly (with SSE2 instructions). Main program uses Windows Forms GUI. User could choose numbers of threads (in range 1-64) which will be used to process them image. It is also possible to generate csv file with all combinations of programming languages and number of threads with processing times, which file can be used to generate plot.


## How to run
Download zip with release and open file ImageEditor.exe then load chosen image and process it

### Options and buttons description
* Load - load image file
* Save - save processed image
* Process - process image by chosen filter
* Apply - load processed image (after) as basic image (before)
* Histogram - show histogram of chosen image (before, after, both)
* P language - select function variant (programming language)
* No Threads - select number of threads used to process image
* Benchmark - generate csv file results.csv with average processing time of all combinations of programming language and number of threads

## Screenshots

## Compilation
Open project with Visual Studio 2022 and run it

## License
Program code is shared under GPLv3 license

This program uses
* WinForms.DataVisualization 1.9.1 sharer under MIT license
* CsvHelper 30.0.1 shared under Apache-2.0 license

MIT and Apache-2.0 licenses are in ./licenses subdirectory