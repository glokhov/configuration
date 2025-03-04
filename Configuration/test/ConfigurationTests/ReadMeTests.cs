// ReSharper disable SuggestVarOrType_Elsewhere
// ReSharper disable SuggestVarOrType_SimpleTypes
// ReSharper disable SuggestVarOrType_BuiltInTypes

namespace ConfigurationTests;

public sealed class ReadMeTests : IDisposable
{
    private static readonly string InitialContent = "[section_one]" + Environment.NewLine +
                                                    "" + Environment.NewLine +
                                                    "# Comment" + Environment.NewLine +
                                                    "" + Environment.NewLine +
                                                    "KeyOne = SectionOne_ValueOne" + Environment.NewLine +
                                                    "KeyTwo = SectionOne_ValueTwo" + Environment.NewLine +
                                                    "" + Environment.NewLine +
                                                    "[section_two]" + Environment.NewLine +
                                                    "" + Environment.NewLine +
                                                    "KeyOne = SectionTwo_ValueOne" + Environment.NewLine +
                                                    "KeyTwo = SectionTwo_ValueTwo" + Environment.NewLine +
                                                    "KeyThree = SectionTwo_ValueThree" + Environment.NewLine +
                                                    "" + Environment.NewLine +
                                                    "[section_three]" + Environment.NewLine +
                                                    "" + Environment.NewLine +
                                                    "KeyOne = SectionThree_ValueOne" + Environment.NewLine +
                                                    "KeyTwo = SectionThree_ValueTwo" + Environment.NewLine;

    private static readonly string FinalContent = "[section_one]" + Environment.NewLine +
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

    public ReadMeTests()
    {
        File.WriteAllText(_tempFile, InitialContent);
    }

    [Fact]
    public void GettingStarted()
    {
        // Call Ini function. Pass the IEqualityComparer<string> that will be used to determine equality of keys in the configuration:

        Result<Ini, string> result = Ini(new FileInfo(_tempFile));

        // Use Match() function to extract the configuration:

        Ini ini = result.Match(ini => ini, err => throw new ApplicationException(err));

        // Use Item[section] property to get a section:

        Option<Section> one = ini["section_one"];
        Option<Section> four = ini["section_four"];

        bool oneIsSome = one.IsSome;
        bool fourIsNone = four.IsNone;

        Assert.True(oneIsSome);
        Assert.True(fourIsNone);

        // Use Item[section, key] property to get a value:

        Option<string> oneOne = ini["section_one", "KeyOne"];
        Option<string> twoTwo = ini["section_two", "KeyTwo"];
        Option<string> threeThree = ini["section_three", "KeyThree"];

        Assert.Equal("Some(SectionOne_ValueOne)", oneOne.ToString());
        Assert.Equal("Some(SectionTwo_ValueTwo)", twoTwo.ToString());
        Assert.Equal("None", threeThree.ToString());

        // Use IsSome and IsNone properties to check, if the value exists:

        bool oneOneIsSome = oneOne.IsSome;
        bool threeThreeIsSome = threeThree.IsSome;
        bool threeThreeIsNone = threeThree.IsNone;

        Assert.True(oneOneIsSome);
        Assert.False(threeThreeIsSome);
        Assert.True(threeThreeIsNone);

        // Call Match() function to extract the value, if exists:

        string oneOneValue = oneOne.Match(some => some, "none");
        string threeThreeValue = threeThree.Match(some => some, "none");

        Assert.Equal("SectionOne_ValueOne", oneOneValue);
        Assert.Equal("none", threeThreeValue);

        // Set Some(value) to change the value:

        ini["section_one", "KeyOne"] = Some("SectionOne_ValueOne_Changed");

        Option<string> oneOneChanged = ini["section_one", "KeyOne"];

        string oneOneValueChanged = oneOneChanged.Match(some => some, "none");

        Assert.Equal("SectionOne_ValueOne_Changed", oneOneValueChanged);

        // Set Some(value) to add new value:

        ini["section_one", "KeyThree"] = Some("SectionOne_ValueThree_Added");

        Option<string> oneThree = ini["section_one", "KeyThree"];

        string oneThreeValue = oneThree.Match(some => some, "none");

        Assert.Equal("SectionOne_ValueThree_Added", oneThreeValue);

        // Set None to remove the value:

        ini["section_two", "KeyThree"] = None;

        Option<string> twoThree = ini["section_two", "KeyThree"];

        string twoThreeValue = twoThree.Match(some => some, "none");
        
        Assert.Equal("none", twoThreeValue);

        // Set None to remove the section:

        ini["section_three"] = None;

        Option<Section> three = ini["section_three"];

        bool threeIsNone = three.IsNone;
        
        Assert.True(threeIsNone);

        // Write configuration to the file:

        ini.WriteTo(new FileInfo(_tempFile));

        var finalContent = File.ReadAllText(_tempFile);

        Assert.Equal(FinalContent, finalContent);
    }

    public void Dispose()
    {
        File.Delete(_tempFile);
    }
}