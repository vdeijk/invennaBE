// using System.Globalization;
// using CsvHelper;
// using CsvHelper.Configuration;
// using CsvHelper.TypeConversion;
// using BE.Data;
// using BE.Domain.Models;

// namespace BE.Import
// {
//     // Converter for nullable int fields
//     public class NullableIntConverter : DefaultTypeConverter
//     {
//         public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
//         {
//             if (string.IsNullOrWhiteSpace(text)) return null;
//             text = text.Replace(".", "").Replace(",", "."); // Remove thousands sep, fix decimal sep
//             if (int.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
//                 return result;
//             if (double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out var d))
//                 return (int)d;
//             return null;
//         }
//     }

//     // Converter for nullable double fields
//     public class NullableDoubleConverter : DefaultTypeConverter
//     {
//         public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
//         {
//             if (string.IsNullOrWhiteSpace(text)) return null;
//             text = text.Replace(".", "").Replace(",", ".");
//             if (double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
//                 return result;
//             return null;
//         }
//     }

//     // CSV mapping for GeographicalData
//     public class GeographicalDataMap : ClassMap<GeographicalData>
//     {
//         public GeographicalDataMap()
//         {
//             Map(m => m.Openbareruimte).Name("openbareruimte");
//             Map(m => m.Huisnummer).Name("huisnummer").TypeConverter<NullableIntConverter>();
//             Map(m => m.Huisletter).Name("huisletter");
//             Map(m => m.Huisnummertoevoeging).Name("huisnummertoevoeging").TypeConverter<NullableIntConverter>();
//             Map(m => m.Postcode).Name("postcode");
//             Map(m => m.Woonplaats).Name("woonplaats");
//             Map(m => m.Gemeente).Name("gemeente");
//             Map(m => m.Provincie).Name("provincie");
//             Map(m => m.Nummeraanduiding).Name("nummeraanduiding");
//             Map(m => m.Verblijfsobjectgebruiksdoel).Name("verblijfsobjectgebruiksdoel");
//             Map(m => m.Oppervlakteverblijfsobject).Name("oppervlakteverblijfsobject").TypeConverter<NullableIntConverter>();
//             Map(m => m.Verblijfsobjectstatus).Name("verblijfsobjectstatus");
//             Map(m => m.ObjectId).Name("object_id");
//             Map(m => m.ObjectType).Name("object_type");
//             Map(m => m.Nevenadres).Name("nevenadres");
//             Map(m => m.Pandid).Name("pandid");
//             Map(m => m.Pandstatus).Name("pandstatus");
//             Map(m => m.Pandbouwjaar).Name("pandbouwjaar").TypeConverter<NullableIntConverter>();
//             Map(m => m.X).Name("x").TypeConverter<NullableIntConverter>();
//             Map(m => m.Y).Name("y").TypeConverter<NullableIntConverter>();
//             Map(m => m.Lon).Name("lon").TypeConverter<NullableDoubleConverter>();
//             Map(m => m.Lat).Name("lat").TypeConverter<NullableDoubleConverter>();
//         }
//     }

//     // Import logic
//     public static class ImportGeographicalData
//     {
//         public static int ImportFromCsv(GeographicalDataContext db, string csvPath)
//         {
//             if (!File.Exists(csvPath))
//             {
//                 Console.WriteLine($"CSV file not found: {csvPath}");
//                 return 0;
//             }

//             var config = new CsvConfiguration(CultureInfo.InvariantCulture)
//             {
//                 Delimiter = ";",
//                 BadDataFound = badData => Console.WriteLine($"Bad data found: {badData}"),
//                 MissingFieldFound = null,
//                 HeaderValidated = null,
//                 IgnoreBlankLines = true
//             };

//             using var reader = new StreamReader(csvPath);
//             using var csv = new CsvReader(reader, config);
//             csv.Context.RegisterClassMap<GeographicalDataMap>();
//             var records = new List<GeographicalData>();
//             int failed = 0;
//             try
//             {
//                 records = csv.GetRecords<GeographicalData>().ToList();
//             }
//             catch (Exception ex)
//             {
//                 Console.WriteLine($"Error during CSV import: {ex.Message}");
//                 failed++;
//             }
//             db.GeographicalData.AddRange(records);
//             db.SaveChanges();
//             Console.WriteLine($"Imported {records.Count} records from {csvPath} into the database. Failed records: {failed}");
//             return records.Count;
//         }
//     }
// }