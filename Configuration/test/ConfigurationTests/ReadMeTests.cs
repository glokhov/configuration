// ReSharper disable All

namespace ConfigurationTests;

public sealed class ReadMeTests : IDisposable
{
    private readonly static string InitialContent = "[section_one]" + Environment.NewLine +
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

    private readonly static string FinalContent = "[section_one]" + Environment.NewLine +
                                                  "" + Environment.NewLine +
                                                  "KeyOne   = SectionOne_ValueOne_Edited" + Environment.NewLine +
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

        Result<Configuration.Configuration, string> ini = Ini(new FileInfo(_tempFile));

        // Use Match() function to extract Configuration:

        Configuration.Configuration conf = ini.Match(conf => conf, err => throw new ApplicationException(err));

        // Get value associated with the section/key combination:

        Option<string> one_one = conf["section_one", "KeyOne"];
        Option<string> two_two = conf["section_two", "KeyTwo"];
        Option<string> three_three = conf["section_three", "KeyThree"];

        Console.WriteLine(one_one); // Some(SectionOne_ValueOne)
        Console.WriteLine(two_two); // Some(SectionTwo_ValueTwo)
        Console.WriteLine(three_three); // None

        // Use IsSome and IsNone properties to check, if value exists:

        bool one_one_is_some = one_one.IsSome; // true
        bool three_three_is_some = three_three.IsSome; // false
        bool three_three_is_none = three_three.IsNone; // true

        // Use Match() function, to extract value:

        string one_one_value = one_one.Match(some => some, "none"); // "SectionOne_ValueOne"
        string three_three_value = three_three.Match(some => some, "none"); // "none"

        // Set Some() to edit value:

        conf["section_one", "KeyOne"] = Some("SectionOne_ValueOne_Edited");

        one_one = conf["section_one", "KeyOne"];

        one_one_value = one_one.Match(some => some, "none"); // "SectionOne_ValueOne_Edited"

        // Set Some() to add value:

        conf["section_one", "KeyThree"] = Some("SectionOne_ValueThree_Added");

        Option<string> one_three = conf["section_one", "KeyThree"];

        string one_three_value = one_three.Match(some => some, "none"); // "SectionOne_ValueThree_Added"

        // Set None to remove value:

        conf["section_two", "KeyThree"] = None;

        Option<string> two_three = conf["section_two", "KeyThree"];

        string two_three_value = two_three.Match(some => some, "none"); // "none"

        // Set None to remove section:

        conf["section_three"] = None;

        Option<Section> three = conf["section_three"];

        bool three_is_none = three.IsNone; // true

        // Write configuration to a file:

        conf.WriteTo(new FileInfo(_tempFile));

        // Assert

        var finalContent = File.ReadAllText(_tempFile);

        Assert.Equal(FinalContent, finalContent);
    }

    public void Dispose()
    {
        File.Delete(_tempFile);
    }
}