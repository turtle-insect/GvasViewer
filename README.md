![DL Count](https://img.shields.io/github/downloads/turtle-insect/GvasViewer/total.svg)

![image](https://github.com/user-attachments/assets/c3a04c40-d767-44a7-93a4-b7699708dee0)  
ex) Dragon Quest VII Reimagined  

# What is ?
GvasViewer  
[Unreal Engine](https://www.unrealengine.com/) Gvas Savedata Format File Viewer

# Advance
* Windows 11(or 10)
* [.NET 10](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
* ※ oo2core_9_win64.dll

# Usage
* Prepare GVAS files
* File -> Open
* Using the Tree View

## ArrayProperty's Import
You must have at least one property.  
Character encoding is UTF-8.  
If the first character is a #, skip insertion.  
Skip insertion for empty lines.  
```
# skip sample

# insert sample
abc
```

## MapProperty's Import
You must have at least one property.  
Character encoding is UTF-8.  
If the first character is a #, skip insertion.  
Skip insertion for empty lines.  
Treat tabs as delimiters.  
Skip insertion when the split by the delimiter does not result in exactly two elements.  
```
# skip sample
abc
abc	def	ghi

# insert sample
abc	def
```

## oo2core_9_win64.dll
* Download `EpicInstaller` from the [Epic Games Store](https://www.epicgames.com/)
* Install the Epic Games Launcher
* Install Unreal Engine 5.7.4
* Search for `oo2core_9_win64.dll` in the Unreal Engine installation folder
* Copy `oo2core_9_win64.dll` to the same folder as the executable file

# Build
* [Visual Studio 2026](https://visualstudio.microsoft.com/)

# Developer
unique File's Format
[IFileFormat](https://github.com/turtle-insect/GvasViewer/blob/main/GvasViewer/FileFormat/IFileFormat.cs)  
implementation this Interface

[SaveData](https://github.com/turtle-insect/GvasViewer/blob/main/GvasViewer/SaveData.cs) `Load` method  
If you want to add a unique structure, you can do so in this file as well  
