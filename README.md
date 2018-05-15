# ED.Net
A C# version of the line editor used by the Pick OS made popular in the 1980s.

## Background

Pick OS and Pick Basic was a language invented by Richard Pick, originaly for the purpose of tracking all kinds of inventory in the US military.
The operating system's database and version of the Basic programming language were tightly coupled and the same editor was used for editing code and for editing data. 'ed' was the verb used for editing and thus the name for the executable in this project.
###
Ed.net is a C# version of the Pick 'ed' editor and works exactly the same way. 

## Installation

Either clone or download the contents of the 'src' folder and open the solution with Visual Studio. The binaries are not on this repository so you will need to compile the solution. Note that there no external dependencies. To avoid having to continuously refer to the executable folder, ensure you add a the 'bin/debug' folder or whatever folder you put the executable in to your path environment variable. This will allow you access the 'ed.exe' executable file from anywhere on your computer.
###
The 'PickEd' project is a .NET Standard 2.0 project and can be consumed by any kind of project. The solution includes a sample console in .NET Framework and one in .NET Core (cross-platform).

## Usage

Editing a file will read into the editor's memory and position the line pointer at the top with a "." prompt ready for a command. Commands are not case sensitive but the documentation will show them in upper case for clarity.
ED.net commands (note the [] signs only indicate a content and should not be literally typed:

- T
  Sets the current line counter to the top of the file.
- L[xx]
  ###
  Lists the next page of text lines based on the current page size (20 by default). If [xx] is specified, then [xx] number of lines are listed and the current page size is reset to [xx] number of lines.
- R
- D
- I
- X
- F
- FS
- FI
- EX
- ?