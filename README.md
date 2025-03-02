# INI configuration file

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
that will be used to determine equality of keys in the configuration.

```csharp
var ini = Ini("Configuration.ini");
var ini = Ini("Configuration.ini", StringComparer.OrdinalIgnoreCase);
```

Sample ```Configuration.ini``` content:

```ini
[section_one]

# Comment

KeyOne = SectionOne_ValueOne
KeyTwo = SectionOne_ValueTwo

[section_two]

KeyOne = SectionTwo_ValueOne
KeyTwo = SectionTwo_ValueTwo
```

Get value associated with the ```section```/```key``` combination.

```csharp
var ini = Ini("Configuration.ini");

var one   = ini["section_one", "KeyOne"];
var two   = ini["section_two", "KeyTwo"];
var three = ini["section_three", "KeyThree"];

Console.WriteLine(one);   // Some(SectionOne_ValueOne)
Console.WriteLine(two);   // Some(SectionTwo_ValueTwo)
Console.WriteLine(three); // None
```