// ReSharper disable SuggestVarOrType_Elsewhere
// ReSharper disable SuggestVarOrType_SimpleTypes
// ReSharper disable SuggestVarOrType_BuiltInTypes

namespace ConfigurationTests;

public sealed class ReadMeTests : IDisposable
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
                                                    "KeyThree = SectionTwo_ValueThree" + Environment.NewLine +
                                                    "" + Environment.NewLine +
                                                    "[section_three]" + Environment.NewLine +
                                                    "" + Environment.NewLine +
                                                    "KeyOne = SectionThree_ValueOne" + Environment.NewLine +
                                                    "KeyTwo = SectionThree_ValueTwo" + Environment.NewLine;

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
        
        // Use ```Item[key]``` property to get or set a global section value:
        
        Option<string> globalOne = ini["KeyOne"];
        Option<string> globalTwo = ini["KeyTwo"];
        Option<string> globalThree = ini["KeyThree"];

        Assert.Equal("Some(GlobalSection_ValueOne)", globalOne.ToString());
        Assert.Equal("Some(GlobalSection_ValueTwo)", globalTwo.ToString());
        Assert.Equal("None", globalThree.ToString());

        // Use Item[section, key] property to get or set a value:

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

        string oneOneChangedValue = oneOneChanged.Match(some => some, "none");

        Assert.Equal("SectionOne_ValueOne_Changed", oneOneChangedValue);

        // Set Some(value) to add new value:

        ini["section_one", "KeyThree"] = Some("SectionOne_ValueThree_Added");

        Option<string> oneThreeAdded = ini["section_one", "KeyThree"];

        string oneThreeAddedValue = oneThreeAdded.Match(some => some, "none");

        Assert.Equal("SectionOne_ValueThree_Added", oneThreeAddedValue);

        // Set None to remove the value:

        ini["section_two", "KeyThree"] = None;

        Option<string> twoThree = ini["section_two", "KeyThree"];

        string twoThreeValue = twoThree.Match(some => some, "none");

        Assert.Equal("none", twoThreeValue);

        // Call GetSection function to get a section:

        Option<SectionDictionary> one = ini.GetSection("section_one");
        Option<SectionDictionary> four = ini.GetSection("section_four");

        bool oneIsSome = one.IsSome;
        bool fourIsNone = four.IsNone;

        Assert.True(oneIsSome);
        Assert.True(fourIsNone);

        // Call SetSection function and pass None to remove the section:

        ini.SetSection("section_three", None);

        Option<SectionDictionary> three = ini.GetSection("section_three");

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