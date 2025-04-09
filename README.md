# INI configuration file [![Nuget Version](https://img.shields.io/nuget/v/Configuration.Ini)](https://www.nuget.org/packages/Configuration.Ini) [![Nuget Download](https://img.shields.io/nuget/dt/Configuration.Ini)](https://www.nuget.org/packages/Configuration.Ini)
Simple INI parser and writer.
## Getting started
Initial ```config.ini``` content:
```ini
# GlobalSection

KeyOne = GlobalSection_ValueOne
KeyTwo = GlobalSection_ValueTwo

[section_one]

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
Import ```Functional``` namespace:
```csharp
global using Functional;
global using static Functional.Prelude;
```
Import ```Configuration``` namespace:
```csharp
using Configuration;
```
Function ```Ini.FromFile(path)``` initializes new ```Ini``` configuration from a file:
```csharp
var ini = Ini.FromFile("config.ini").Unwrap();
```
Property ```Item[key]``` gets the ```Option``` value associated with the specified key:
```csharp
var globalOne = ini["KeyOne"];
var globalTwo = ini["KeyTwo"];
var globalThree = ini["KeyThree"];

Debug.Assert(globalOne == Some("GlobalSection_ValueOne"));
Debug.Assert(globalTwo == Some("GlobalSection_ValueTwo"));
Debug.Assert(globalThree == None);
```
Property ```IsSome``` returns ```true``` if the value exists; Property ```IsNone``` returns ```true``` if the value doesn't exist:
```csharp
Debug.Assert(globalOne.IsSome);
Debug.Assert(globalTwo.IsSome);
Debug.Assert(globalThree.IsNone);
```
Property ```Item[section, key] ``` gets the ```Option``` value associated with the specified section name and key:
```csharp
var oneOne = ini["section_one", "KeyOne"];
var twoTwo = ini["section_two", "KeyTwo"];
var threeThree = ini["section_three", "KeyThree"];

Debug.Assert(oneOne == Some("SectionOne_ValueOne"));
Debug.Assert(twoTwo == Some("SectionTwo_ValueTwo"));
Debug.Assert(threeThree == None);
```
Function ```Match(some, none)``` safely extracts the value:
```csharp
var oneOneValue = oneOne.Match(some => some, "none");
var threeThreeValue = threeThree.Match(some => some, "none");

Debug.Assert(oneOneValue == "SectionOne_ValueOne");
Debug.Assert(threeThreeValue == "none");
```
Set ```Some(value)``` to change the value:
```csharp
ini["section_one", "KeyOne"] = Some("SectionOne_ValueOne_Changed");

var oneOneChanged = ini["section_one", "KeyOne"];

var oneOneChangedValue = oneOneChanged.Match(some => some, "none");

Debug.Assert(oneOneChangedValue == "SectionOne_ValueOne_Changed");
```
Set Some(value) to add a value:
```csharp
ini["section_one", "KeyThree"] = Some("SectionOne_ValueThree_Added");

var oneThreeAdded = ini["section_one", "KeyThree"];

var oneThreeAddedValue = oneThreeAdded.Match(some => some, "none");

Debug.Assert(oneThreeAddedValue == "SectionOne_ValueThree_Added");
```
Set ```None``` to remove the value:
```csharp
ini["section_two", "KeyThree"] = None;

var twoThree = ini["section_two", "KeyThree"];

var twoThreeValue = twoThree.Match(some => some, "none");

Debug.Assert(twoThreeValue == "none");
```
Function ```ToFile(path)``` writes the ```Ini``` configuration to a file:
```csharp
ini.ToFile("config.ini");
```
Final ```config.ini``` content:
```ini
KeyOne = GlobalSection_ValueOne
KeyTwo = GlobalSection_ValueTwo

[section_one]

KeyOne   = SectionOne_ValueOne_Changed
KeyTwo   = SectionOne_ValueTwo
KeyThree = SectionOne_ValueThree_Added

[section_two]

KeyOne = SectionTwo_ValueOne
KeyTwo = SectionTwo_ValueTwo
```
### Sample
See [GettingStarted](https://github.com/glokhov/configuration/blob/main/Configuration/test/Configuration.Tests/README.Tests.cs) test.
