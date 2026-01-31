![DL Count](https://img.shields.io/github/downloads/turtle-insect/GvasViewer/total.svg)

![image](https://github.com/user-attachments/assets/bb3ec407-16cb-4e7b-a75c-b9aa9c36e8ab)  
ex) Romancing Saga 2  

# What is ?
GvasViewer  
[Unreal Engine](https://www.unrealengine.com/) Gvas Savedata Format File Viewer

# Advance
* Windows 10(or 11)
* [.NET 10](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)

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

# Developer
unique File's Format
[IFileFormat](https://github.com/turtle-insect/GvasViewer/blob/main/GvasViewer/FileFormat/IFileFormat.cs)  
implementation this Interface

[SaveData](https://github.com/turtle-insect/GvasViewer/blob/main/GvasViewer/SaveData.cs) `Load` method  
If you want to add a unique structure, you can do so in this file as well  

# Build
* [Visual Studio 2026](https://visualstudio.microsoft.com/)
 
