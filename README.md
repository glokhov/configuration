# INI configuration file [![Nuget Version](https://img.shields.io/nuget/v/Configuration.Ini)](https://www.nuget.org/packages/Configuration.Ini) [![Nuget Download](https://img.shields.io/nuget/dt/Configuration.Ini)](https://www.nuget.org/packages/Configuration.Ini)
Simple INI parser and writer.
## Getting started
Use ```global using``` directive for the whole project:
```csharp
global using Functional;
global using Configuration;
global using static Functional.Prelude;
global using static Configuration.Prelude;
```
Or ```using``` directive in the single file: 
```csharp
using Functional;
using Configuration;
using static Functional.Prelude;
using static Configuration.Prelude;
```
Initial Configuration.ini content:
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
Call ```Ini``` function. Pass the ```IEqualityComparer<string>``` that will be used to determine equality of keys in the configuration:
```csharp
Result<Ini, string> result = Ini(new FileInfo("Configuration.ini"));
Result<Ini, string> result = Ini(new FileInfo("Configuration.ini"), StringComparer.OrdinalIgnoreCase);
```
Use ```Match()``` function to extract the configuration:
```csharp
Ini ini = result.Match(ini => ini, err => throw new ApplicationException(err));
```
Use ```Item[key]``` property to get or set a global section value:
```csharp
Option<string> globalOne = ini["KeyOne"];
Option<string> globalTwo = ini["KeyTwo"];
Option<string> globalThree = ini["KeyThree"];

Assert.Equal("Some(GlobalSection_ValueOne)", globalOne.ToString());
Assert.Equal("Some(GlobalSection_ValueTwo)", globalTwo.ToString());
Assert.Equal("None", globalThree.ToString());
```
Use ```Item[section, key]``` property to get or set a value:
```csharp
Option<string> oneOne = ini["section_one", "KeyOne"];
Option<string> twoTwo = ini["section_two", "KeyTwo"];
Option<string> threeThree = ini["section_three", "KeyThree"];

Assert.Equal("Some(SectionOne_ValueOne)", oneOne.ToString());
Assert.Equal("Some(SectionTwo_ValueTwo)", twoTwo.ToString());
Assert.Equal("None", threeThree.ToString());
```
Use ```IsSome``` and ```IsNone``` properties to check, if the value exists:
```csharp
bool oneOneIsSome = oneOne.IsSome;
bool threeThreeIsSome = threeThree.IsSome;
bool threeThreeIsNone = threeThree.IsNone;

Assert.True(oneOneIsSome);
Assert.False(threeThreeIsSome);
Assert.True(threeThreeIsNone);
```
Call ```Match()``` function to extract the value, if exists:
```csharp
string oneOneValue = oneOne.Match(some => some, "none");
string threeThreeValue = threeThree.Match(some => some, "none");

Assert.Equal("SectionOne_ValueOne", oneOneValue);
Assert.Equal("none", threeThreeValue);
```
Set ```Some(value)``` to change the value:
```csharp
ini["section_one", "KeyOne"] = Some("SectionOne_ValueOne_Changed");

Option<string> oneOneChanged = ini["section_one", "KeyOne"];

string oneOneChangedValue = oneOneChanged.Match(some => some, "none");

Assert.Equal("SectionOne_ValueOne_Changed", oneOneChangedValue);
```
Set ```Some(value)``` to add new value:
```csharp
ini["section_one", "KeyThree"] = Some("SectionOne_ValueThree_Added");

Option<string> oneThreeAdded = ini["section_one", "KeyThree"];

string oneThreeAddedValue = oneThreeAdded.Match(some => some, "none");

Assert.Equal("SectionOne_ValueThree_Added", oneThreeAddedValue);
```
Set ```None``` to remove the value:
```csharp
ini["section_two", "KeyThree"] = None;

Option<string> twoThree = ini["section_two", "KeyThree"];

string twoThreeValue = twoThree.Match(some => some, "none");

Assert.Equal("none", twoThreeValue);
```
Call ```GetSection``` function to get a section:
```csharp
Option<SectionDictionary> one = ini.GetSection("section_one");
Option<SectionDictionary> four = ini.GetSection("section_four");

bool oneIsSome = one.IsSome;
bool fourIsNone = four.IsNone;

Assert.True(oneIsSome);
Assert.True(fourIsNone);
```
Call ```SetSection``` function and pass ```None``` to remove the section:
```csharp
ini.SetSection("section_three", None);

Option<SectionDictionary> three = ini.GetSection("section_three");

bool threeIsNone = three.IsNone;

Assert.True(threeIsNone);
```
Write configuration to the file:
```csharp
ini.WriteTo(new FileInfo("Configuration.ini"));
```
Final Configuration.ini content:
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
See [GettingStarted](https://github.com/glokhov/configuration/blob/main/Configuration/test/ConfigurationTests/ReadMeTests.cs) test.
