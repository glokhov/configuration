# INI configuration file [![Nuget](https://img.shields.io/nuget/v/Configuration.Ini)](https://www.nuget.org/packages/Configuration.Ini) [![Nuget](https://img.shields.io/nuget/dt/Configuration.Ini)](https://www.nuget.org/packages/Configuration.Ini)
Simple INI parser and printer.
## Getting started
Use the ```global using``` directive for the whole project:
```csharp
global using Configuration;
global using static Configuration.Prelude;
```
Or the ```using``` directive in a single file: 
```csharp
using Configuration;
using static Configuration.Prelude;
```
Call ```Ini``` function. Pass the ```IEqualityComparer<string>``` 
that will be used to determine equality of keys in the configuration:
```csharp
var ini = Ini(new FileInfo("Configuration.ini"));
var ini = Ini(new FileInfo("Configuration.ini"), StringComparer.OrdinalIgnoreCase);
```
Initial Configuration.ini content:
```ini
[section_one]

# Comment

KeyOne = SectionOne_ValueOne
KeyTwo = SectionOne_ValueTwo

[section_two]

KeyOne = SectionTwo_ValueOne
KeyTwo = SectionTwo_ValueTwo
KeyThree = SectionTwo_ValueThree

[section_three]

KeyOne = SectionThree_ValueOne
KeyTwo = SectionThree_ValueTwo
```
Get value associated with the ```section```/```key``` combination:
```csharp
var one_one     = ini["section_one", "KeyOne"];
var two_two     = ini["section_two", "KeyTwo"];
var three_three = ini["section_three", "KeyThree"];

Console.WriteLine(one_one);     // Some(SectionOne_ValueOne)
Console.WriteLine(two_two);     // Some(SectionTwo_ValueTwo)
Console.WriteLine(three_three); // None
```
Set ```Some()``` to edit ```value```:
```csharp
ini["section_one", "KeyOne"] = Some("SectionOne_ValueOne_Edited");

one_one = ini["section_one", "KeyOne"];

Console.WriteLine(one_one); // Some(SectionOne_ValueOne_Edited)
```
Set ```Some()``` to add ```value```:
```csharp
ini["section_one", "KeyThree"] = Some("SectionOne_ValueThree_Added");

var one_three = ini["section_one", "KeyThree"];

Console.WriteLine(one_three); // Some(SectionOne_ValueThree_Added)
```
Set ```None``` to remove ```value```:
```csharp
ini["section_two", "KeyThree"] = None;

var two_three = ini["section_two", "KeyThree"];

Console.WriteLine(two_three); // None
```
Set ```None``` to remove ```section```:
```csharp
ini["section_three"] = None;

var three = ini["section_three"];

Console.WriteLine(two_three); // None
```
Save configuration to a file:
```csharp
ini.SaveToFile("Configuration.ini");
```
Final Configuration.ini content:
```ini
[section_one]

KeyOne = SectionOne_ValueOne_Edited
KeyTwo = SectionOne_ValueTwo
KeyThree = SectionOne_ValueThree_Added

[section_two]

KeyOne = SectionTwo_ValueOne
KeyTwo = SectionTwo_ValueTwo
```