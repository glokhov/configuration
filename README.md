# INI configuration file [![Nuget Version](https://img.shields.io/nuget/v/Configuration.Ini)](https://www.nuget.org/packages/Configuration.Ini) [![Nuget Download](https://img.shields.io/nuget/dt/Configuration.Ini)](https://www.nuget.org/packages/Configuration.Ini)
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
Result<Configuration, string> ini = Ini(new FileInfo("Configuration.ini"));
Result<Configuration, string> ini = Ini(new FileInfo("Configuration.ini"), StringComparer.OrdinalIgnoreCase);
```
Use ```Match()``` function to extract ```Configuration```:
```csharp
Configuration conf = ini.Match(conf => conf, err => throw new ApplicationException(err));
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
Option<string> one_one     = conf["section_one", "KeyOne"];
Option<string> two_two     = conf["section_two", "KeyTwo"];
Option<string> three_three = conf["section_three", "KeyThree"];

Console.WriteLine(one_one);     // Some(SectionOne_ValueOne)
Console.WriteLine(two_two);     // Some(SectionTwo_ValueTwo)
Console.WriteLine(three_three); // None
```
Use ```IsSome``` and ```IsNone``` properties to check, if ```value``` exists:
```csharp
bool one_one_is_some     = one_one.IsSome;     // true
bool three_three_is_some = three_three.IsSome; // false
bool three_three_is_none = three_three.IsNone; // true
```
Use ```Match()``` function, to extract ```value```:
```csharp
string one_one_value     = one_one.Match(some => some, "none");     // "SectionOne_ValueOne"
string three_three_value = three_three.Match(some => some, "none"); // "none"
```
Set ```Some()``` to edit ```value```:
```csharp
conf["section_one", "KeyOne"] = Some("SectionOne_ValueOne_Edited");

one_one = conf["section_one", "KeyOne"];

one_one_value = one_one.Match(some => some, "none"); // "SectionOne_ValueOne_Edited"
```
Set ```Some()``` to add ```value```:
```csharp
conf["section_one", "KeyThree"] = Some("SectionOne_ValueThree_Added");

Option<string> one_three = conf["section_one", "KeyThree"];

string one_three_value = one_three.Match(some => some, "none"); // "SectionOne_ValueThree_Added"
```
Set ```None``` to remove ```value```:
```csharp
conf["section_two", "KeyThree"] = None;

Option<string> two_three = conf["section_two", "KeyThree"];

string two_three_value = two_three.Match(some => some, "none"); // "none"
```
Set ```None``` to remove ```section```:
```csharp
conf["section_three"] = None;

Option<Section> three = conf["section_three"];

bool three_is_none = three.IsNone; // true
```
Write configuration to a file:
```csharp
conf.WriteTo(new FileInfo("Configuration.ini"));
```
Final Configuration.ini content:
```ini
[section_one]

KeyOne   = SectionOne_ValueOne_Edited
KeyTwo   = SectionOne_ValueTwo
KeyThree = SectionOne_ValueThree_Added

[section_two]

KeyOne = SectionTwo_ValueOne
KeyTwo = SectionTwo_ValueTwo
```
### Sample
See [GettingStarted](https://github.com/glokhov/configuration/blob/main/Configuration/test/ConfigurationTests/ReadMeTests.cs) test.
