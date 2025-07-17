public class SeedRunner
{
    public static void Main()
    {
        GeographicalDataImporter.ImportFirstFive(
            @"c:\Users\Gebruiker\repos\Invenna\BE\BE\BE.Data\Data\geographicaldata (1).csv",
            @"c:\Users\Gebruiker\repos\Invenna\BE\BE\BE.Data\Data\geodata.db"
        );
    }
}