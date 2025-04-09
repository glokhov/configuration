using System.Diagnostics;
using Functional;
using static Functional.Prelude;

namespace Configuration.Tests;

public sealed class ReadmeTests : IDisposable
{
    private static readonly string InitialContent = "# GlobalSection" + Environment.NewLine +
                                                    "" + Environment.NewLine +
                                                    "KeyOne = GlobalSection_ValueOne" + Environment.NewLine +
                                                    "KeyTwo = GlobalSection_ValueTwo" + Environment.NewLine +
                                                    "" + Environment.NewLine +
                                                    "[section_one]" + Environment.NewLine +
                                                    "" + Environment.NewLine +
                                                    "KeyOne = SectionOne_ValueOne" + Environment.NewLine +
                                                    "KeyTwo = SectionOne_ValueTwo" + Environment.NewLine +
                                                    "" + Environment.NewLine +
                                                    "[section_two]" + Environment.NewLine +
                                                    "" + Environment.NewLine +
                                                    "KeyOne = SectionTwo_ValueOne" + Environment.NewLine +
                                                    "KeyTwo = SectionTwo_ValueTwo" + Environment.NewLine +
                                                    "KeyThree = SectionTwo_ValueThree" + Environment.NewLine;

    private static readonly string FinalContent = "KeyOne = GlobalSection_ValueOne" + Environment.NewLine +
                                                  "KeyTwo = GlobalSection_ValueTwo" + Environment.NewLine +
                                                  "" + Environment.NewLine +
                                                  "[section_one]" + Environment.NewLine +
                                                  "" + Environment.NewLine +
                                                  "KeyOne   = SectionOne_ValueOne_Changed" + Environment.NewLine +
                                                  "KeyTwo   = SectionOne_ValueTwo" + Environment.NewLine +
                                                  "KeyThree = SectionOne_ValueThree_Added" + Environment.NewLine +
                                                  "" + Environment.NewLine +
                                                  "[section_two]" + Environment.NewLine +
                                                  "" + Environment.NewLine +
                                                  "KeyOne = SectionTwo_ValueOne" + Environment.NewLine +
                                                  "KeyTwo = SectionTwo_ValueTwo" + Environment.NewLine;

    private readonly string _tempFile = Path.GetTempFileName();

    public ReadmeTests()
    {
        File.WriteAllText(_tempFile, InitialContent);
    }

    public void Dispose()
    {
        File.Delete(_tempFile);
    }

    [Fact]
    public void GettingStarted()
    {
        // Import ```Functional``` namespace:
        // using Functional;
        // using static Functional.Prelude;

        // Import ```Configuration``` namespace:
        // using Configuration;

        // Function ```Ini.FromFile(path)``` initializes new ```Ini``` configuration from a file:

        var ini = Ini.FromFile(_tempFile).Unwrap();

        // Property ```Item[key]``` gets the ```Option``` value associated with the specified key:

        var globalOne = ini["KeyOne"];
        var globalTwo = ini["KeyTwo"];
        var globalThree = ini["KeyThree"];

        Debug.Assert(globalOne == Some("GlobalSection_ValueOne"));
        Debug.Assert(globalTwo == Some("GlobalSection_ValueTwo"));
        Debug.Assert(globalThree == None);

        // Property ```IsSome``` returns ```true``` if the value exists; Property ```IsNone``` returns ```true``` if the value doesn't exist:

        Debug.Assert(globalOne.IsSome);
        Debug.Assert(globalTwo.IsSome);
        Debug.Assert(globalThree.IsNone);

        // Property ```Item[section, key] ``` gets the ```Option``` value associated with the specified section name and key:

        var oneOne = ini["section_one", "KeyOne"];
        var twoTwo = ini["section_two", "KeyTwo"];
        var threeThree = ini["section_three", "KeyThree"];

        Debug.Assert(oneOne == Some("SectionOne_ValueOne"));
        Debug.Assert(twoTwo == Some("SectionTwo_ValueTwo"));
        Debug.Assert(threeThree == None);

        // Function ```Match(some, none)``` safely extracts the value:

        var oneOneValue = oneOne.Match(some => some, "none");
        var threeThreeValue = threeThree.Match(some => some, "none");

        Debug.Assert(oneOneValue == "SectionOne_ValueOne");
        Debug.Assert(threeThreeValue == "none");

        // Set ```Some(value)``` to change the value:

        ini["section_one", "KeyOne"] = Some("SectionOne_ValueOne_Changed");

        var oneOneChanged = ini["section_one", "KeyOne"];

        var oneOneChangedValue = oneOneChanged.Match(some => some, "none");

        Debug.Assert(oneOneChangedValue == "SectionOne_ValueOne_Changed");

        // Set Some(value) to add a value:

        ini["section_one", "KeyThree"] = Some("SectionOne_ValueThree_Added");

        var oneThreeAdded = ini["section_one", "KeyThree"];

        var oneThreeAddedValue = oneThreeAdded.Match(some => some, "none");
        
        Debug.Assert(oneThreeAddedValue == "SectionOne_ValueThree_Added");

        // Set ```None``` to remove the value:

        ini["section_two", "KeyThree"] = None;

        var twoThree = ini["section_two", "KeyThree"];

        var twoThreeValue = twoThree.Match(some => some, "none");

        Debug.Assert(twoThreeValue == "none");

        // Function ```ToFile(path)``` writes the <c>Ini</c> configuration to a file:

        ini.ToFile(_tempFile);

        var finalContent = File.ReadAllText(_tempFile);

        Debug.Assert(finalContent == FinalContent);
    }
}