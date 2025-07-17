using System.Globalization;
using CsvHelper;
using Microsoft.Data.Sqlite;

public static class GeographicalDataImporter
{
    public static void ImportFirstFive(string csvPath, string sqlitePath)
    {
        using var reader = new StreamReader(csvPath);
        using var csv = new CsvReader(reader, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";"
        });

        var records = csv.GetRecords<dynamic>().Take(5).ToList();

        using var connection = new SqliteConnection($"Data Source={sqlitePath}");
        connection.Open();

        // Create table if not exists (simplified: only a few columns for demo)
        var createCmd = connection.CreateCommand();
        createCmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS GeographicalData (
                openbareruimte TEXT,
                huisnummer TEXT,
                postcode TEXT,
                woonplaats TEXT
                -- Add more columns as needed
            );
        ";
        createCmd.ExecuteNonQuery();

        foreach (var record in records)
        {
            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText = @"
                INSERT INTO GeographicalData (openbareruimte, huisnummer, postcode, woonplaats)
                VALUES ($openbareruimte, $huisnummer, $postcode, $woonplaats);
            ";
            insertCmd.Parameters.AddWithValue("$openbareruimte", record.openbareruimte);
            insertCmd.Parameters.AddWithValue("$huisnummer", record.huisnummer);
            insertCmd.Parameters.AddWithValue("$postcode", record.postcode);
            insertCmd.Parameters.AddWithValue("$woonplaats", record.woonplaats);
            insertCmd.ExecuteNonQuery();
        }
    }
}