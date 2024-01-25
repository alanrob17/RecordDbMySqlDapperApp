using DapperDAL.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _ad = DapperDAL.ArtistDataAccess;
using _at = RecordDbMySqlDapper.Tests.ArtistTest;
using _rd = DapperDAL.RecordDataAccess;

namespace RecordDbMySqlDapper.Tests
{
    public class RecordTest
    {
        // see GetArtistRecordsMultipleTables() for a better version.
        internal static async Task GetRecordListAsync()
        {
            var artists = await _ad.GetArtistsAsync();
            var records = await _rd.GetRecordsAsync();

            foreach (var artist in artists)
            {
                await Console.Out.WriteLineAsync($"{artist.Name}:\n");

                var ar = from r in records
                         where artist.ArtistId == r.ArtistId
                         orderby r.Recorded descending
                         select r;

                foreach (var rec in ar)
                {
                    await Console.Out.WriteLineAsync($"\t{rec.Recorded} - {rec.Name} ({rec.Media})");
                }

                await Console.Out.WriteLineAsync();
            }
        }

        // see GetArtistRecordsMultipleTablesSP() for a better version.
        internal static async Task GetRecordListSPAsync()
        {
            var artists = await _ad.GetAllArtistsSPAsync();
            var records = await _rd.GetRecordsSPAsync();

            foreach (var artist in artists)
            {
                await Console.Out.WriteLineAsync($"{artist.Name}:\n");

                var ar = from r in records
                         where artist.ArtistId == r.ArtistId
                         orderby r.Recorded descending
                         select r;

                foreach (var rec in ar)
                {
                    await Console.Out.WriteLineAsync($"\t{rec.Recorded} - {rec.Name} ({rec.Media})");
                }

                await Console.Out.WriteLineAsync();
            }
        }

        internal static async Task GetTotalNumberOfBluraysAsync()
        {
            var total = await _rd.GetTotalNumberOfBluraysAsync();
            await Console.Out.WriteLineAsync($"Total number of Blu-rays: {total}");
        }

        internal static async Task GetTotalNumberOfBluraysSPAsync()
        {
            var total = await _rd.GetTotalNumberOfBluraysSPAsync();
            await Console.Out.WriteLineAsync($"Total number of Blu-rays: {total}");
        }

        internal static async Task GetTotalNumberOfRecordsAsync()
        {
            var total = await _rd.GetTotalNumberOfRecordsAsync();
            await Console.Out.WriteLineAsync($"Total number of Records: {total}");
        }

        internal static async Task GetTotalNumberOfRecordsSPAsync()
        {
            var total = await _rd.GetTotalNumberOfRecordsSPAsync();
            await Console.Out.WriteLineAsync($"Total number of Records: {total}");
        }

        internal static async Task GetTotalNumberOfCDsAsync()
        {
            var total = await _rd.GetTotalNumberOfCDsAsync();
            await Console.Out.WriteLineAsync($"Total number of CD's: {total}");
        }

        internal static async Task GetTotalNumberOfCDsSPAsync()
        {
            var total = await _rd.GetTotalNumberOfCDsSPAsync();
            await Console.Out.WriteLineAsync($"Total number of CD's: {total}");
        }

        internal static async Task GetTotalNumberOfDiscsAsync()
        {
            var total = await _rd.GetTotalNumberOfDiscsAsync();
            await Console.Out.WriteLineAsync($"Total number of Discs: {total}");
        }

        internal static async Task GetTotalNumberOfDiscsSPAsync()
        {
            var total = await _rd.GetTotalNumberOfDiscsSPAsync();
            await Console.Out.WriteLineAsync($"Total number of Discs: {total}");
        }

        internal static async Task GetRecordsByYearAsync(int year)
        {
            var records = await _rd.GetRecordsByYearAsync(year);
            if (records.Count == 0)
            {
                await Console.Out.WriteLineAsync($"No records found for {year}");
            }
            else
            {
                foreach (var record in records)
                {
                    await PrintArtistRecordAsync(record);
                }
            }
        }

        internal static async Task GetRecordsByYearSPAsync(int year)
        {
            var records = await _rd.GetRecordsByYearSPAsync(year);

            if (records.Count > 0)
            {
                foreach (var record in records)
                {
                    await Console.Out.WriteLineAsync($"{record.Name} - {record.Recorded} ({record.Media})");
                }
            }
            else
            {
                await Console.Out.WriteLineAsync($"No records found for {year}");
            }
        }

        internal static async Task GetRecordsByArtistIdAsync(int artistId)
        {
            var artist = await _ad.GetArtistByIdAsync(artistId);

            if (artist is ArtistModel)
            {
                await _at.PrintArtistAsync(artist);

                await Console.Out.WriteLineAsync("\n----------------------------\n");

                var records = await _rd.GetRecordsByArtistIdAsync(artistId);

                foreach (var record in records)
                {
                    await PrintRecordAsync(record);
                }
            }
        }

        internal static async Task GetRecordsByArtistIdSPAsync(int artistId)
        {
            var artist = await _ad.GetArtistByIdSPAsync(artistId);

            if (artist is ArtistModel)
            {
                await _at.PrintArtistAsync(artist);

                await Console.Out.WriteLineAsync("\n----------------------------\n");

                var records = await _rd.GetRecordsByArtistIdSPAsync(artistId);

                foreach (var record in records)
                {
                    await PrintRecordAsync(record);
                }
            }
        }

        internal static async Task GetRecordsByArtistIdMultipleTablesAsync(int artistId)
        {
            var artist = await _rd.GetRecordsByArtistIdMultipleTablesAsync(artistId);

            if (artist.ArtistId > 0)
            {
                await _at.PrintArtistAsync(artist);

                await Console.Out.WriteLineAsync("\n----------------------------\n");

                var records = await _rd.GetRecordsByArtistIdAsync(artistId);

                foreach (var record in records)
                {
                   await PrintRecordAsync(record);
                }
            }
        }

        internal static async Task GetRecordsByArtistIdMultipleTablesSPAsync(int artistId)
        {
            var artist = await _rd.GetRecordsByArtistIdMultipleTablesSPAsync(artistId);

            if (artist.ArtistId > 0)
            {
                await _at.PrintArtistAsync(artist);

                await Console.Out.WriteLineAsync("\n----------------------------\n");

                var records = await _rd.GetRecordsByArtistIdSPAsync(artistId);

                foreach (var record in records)
                {
                    await PrintRecordAsync(record);
                }
            }
        }

        internal static async Task GetArtistRecordsMultipleTablesAsync()
        {
            var artists = await _rd.GetArtistRecordsMultipleTablesAsync();

            foreach (var artist in artists)
            {
                await Console.Out.WriteLineAsync($"\n{artist.Name}");

                foreach (var record in artist.Records)
                {
                    await Console.Out.WriteLineAsync($"\t{record.Name}, {record.Recorded} ({record.Media})");
                }
            }
        }

        internal static async Task GetArtistRecordsMultipleTablesSPAsync()
        {
            var artists = await _rd.GetArtistRecordsMultipleTablesSPAsync();

            foreach (var artist in artists)
            {
                await Console.Out.WriteLineAsync($"\n{artist.Name}");

                foreach (var record in artist.Records)
                {
                    await Console.Out.WriteLineAsync($"\t{record.Name}, {record.Recorded} ({record.Media})");
                }
            }
        }

        internal static async Task GetRecordByNameAsync(string name)
        {
            // Note: you can use %% to get a partial name. 
            RecordModel record = new()
            {
                Name = $"%{name}%"
            };

            var newRecord = await _rd.GetRecordByNameAsync(record);

            await PrintArtistRecordAsync(newRecord);
        }

        internal static async Task GetRecordByNameSPAsync(string name)
        {
            // Note: you can use %% to get a partial name. 
            RecordModel record = new()
            {
                Name = $"%{name}%"
            };

            var newRecord = await _rd.GetRecordByNameSPAsync(record);

            await PrintArtistRecordAsync(newRecord);
        }

        internal static async Task GetRecordByIdAsync(int recordId)
        {
            var record = await _rd.GetRecordByIdAsync(recordId);

            if (record.RecordId > 0)
            {
                await PrintArtistRecordAsync(record);
            }
            else
            {
                await Console.Out.WriteLineAsync("ERROR: Record not found!");
            }
        }

        internal static async Task GetRecordByIdSPAsync(int recordId)
        {
            var record = await _rd.GetRecordByIdSPAsync(recordId);

            if (record.RecordId > 0)
            {
                await PrintArtistRecordAsync(record);
            }
            else
            {
                await Console.Out.WriteLineAsync("ERROR: Record not found!");
            }
        }

        internal static async Task GetAllRecordsAsync()
        {
            var records = await _rd.GetRecordsAsync();

            foreach (var record in records)
            {
                await PrintArtistRecordAsync(record);
            }
        }

        internal static async Task GetAllRecordsSPAsync()
        {
            var records = await _rd.GetRecordsSPAsync();

            foreach (var record in records)
            {
                await PrintArtistRecordAsync(record);
            }
        }

        internal static async Task GetArtistByIdAsync(int artistId)
        {
            var artist = await _ad.GetArtistByIdAsync(artistId);

            if (artist is ArtistModel)
            {
                await _at.PrintArtistAsync(artist);

                var rec = new RecordModel()
                {
                    ArtistId = artistId
                };

                var records = await _rd.GetRecordsByArtistIdAsync(rec.ArtistId);

                foreach (var record in records)
                {
                    await PrintArtistRecordAsync(record);
                }
            }
        }

        internal static async Task PrintRecordAsync(RecordModel record, bool review = false)
        {
            var rev = string.IsNullOrEmpty(record.Review) ? "No Review" : (record.Review.Length > 30 ? record.Review.Substring(0, 30) + "..." : "No review");
            try
            {
                await Console.Out.WriteLineAsync($"Id: {record.RecordId} - {record.ArtistId} {record.Name} - {record.Field}, {record.Recorded}, {record.Label}, {record.Pressing}, {record.Rating}, {record.Discs}, {record.Media}, {record.Bought}, ${record.Cost}.\n {rev}\n");
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"No record found.\n\n{ex.Message}");
            }
        }

        // TODO: refactor this to remove multiple Db calls.
        internal static async Task PrintArtistRecordAsync(RecordModel record)
        {
            var artist = await _ad.GetArtistByIdAsync(record.ArtistId);

            var message = artist is ArtistModel ? $"{artist.Name} - {record.Recorded}: {record.Name} - {record.Field}." : $"Artist or record not found!\n\n";
            await Console.Out.WriteLineAsync(message);
        }

        internal static async Task CreateRecordAsync(int artistId)
        {
            RecordModel record = new()
            {
                ArtistId = artistId,
                Name = "No Fun Allowed",
                Field = "Rock",
                Recorded = 1986,
                Label = "Wobble",
                Pressing = "Aus",
                Rating = "***",
                Discs = 1,
                Media = "CD",
                Bought = "2022-01-16",
                Cost = 19.95m,
                Review = "This is James\'s first album."
            };

            var result = await _rd.AddRecordAsync(record);
            var message = result > 0 ? $"Record added to database." : "ERROR: Record not added to database.";
            await Console.Out.WriteLineAsync(message);
        }

        internal static async Task CreateRecordSPAsync(int artistId)
        {
            RecordModel record = new()
            {
                ArtistId = artistId,
                Name = "Way More Fun Allowed",
                Field = "Rock",
                Recorded = 1990,
                Label = "Rabble",
                Pressing = "Ger",
                Rating = "****",
                Discs = 1,
                Media = "CD",
                Bought = "2022-01-17",
                Cost = 29.95m,
                Review = "This is Charley\'s second album."
            };

            var recordId = await _rd.AddRecordSPAsync(record);

            await Console.Out.WriteLineAsync(recordId.ToString());
        }


        internal static async Task DeleteRecordAsync(int recordId)
        {
            int result = await _rd.DeleteRecordAsync(recordId);
            var message = result > 0 ? "Record deleted." : "ERROR: Record not deleted!";
            await Console.Out.WriteLineAsync(message);
        }

        internal static async Task DeleteRecordSPAsync(int recordId)
        {
            int result = await _rd.DeleteRecordSPAsync(recordId);
            var message = result > 0 ? "Record deleted." : "ERROR: Record not deleted!";
            await Console.Out.WriteLineAsync(message);
        }

        internal static async Task UpdateRecordAsync(int recordId)
        {
            RecordModel record = new()
            {
                RecordId = recordId,
                Name = "Plenty Of Fun Allowed",
                Field = "Jazz",
                Recorded = 1988,
                Label = "Wibble",
                Pressing = "Ger",
                Rating = "****",
                Discs = 2,
                Media = "CD",
                Bought = "2022-08-17",
                Cost = 29.95m,
                Review = "This is Charley\'s second album."
            };

            var i = await _rd.UpdateRecordAsync(record);

            var message = i > 0 ? "Record updated." : "ERROR: Record not updated!";
            await Console.Out.WriteLineAsync(message);
        }

        internal static async Task UpdateRecordSPAsync(int recordId)
        {
            RecordModel record = new()
            {
                RecordId = recordId,
                Name = "Too Much Fun Allowed",
                Field = "Hip-Hop",
                Recorded = 2019,
                Label = "Rebel",
                Pressing = "Aus",
                Rating = "***",
                Discs = 1,
                Media = "CD",
                Bought = "2023-09-18",
                Cost = 19.95m,
                Review = "This is James\'s thirty-third album."
            };

            var i = await _rd.UpdateRecordSPAsync(record);

            var message = i > 0 ? "Record updated." : "ERROR: Record not updated!";
            await Console.Out.WriteLineAsync(message);
        }

        // Single record view
        internal static async Task GetRecordListMultipleTablesAsync()
        {
            List<dynamic> records = await _rd.GetArtistRecordListAsync();

            foreach (dynamic r in records)
            {
                await Console.Out.WriteLineAsync($"Artist: {r.Artist} - {r.Name} - {r.Recorded} ({r.Media}) {r.Rating}.");
            }
        }

        // Single record view
        internal static async Task GetRecordListMultipleTablesSPAsync()
        {
            List<dynamic> records = await _rd.GetArtistRecordListSPAsync();

            foreach (dynamic r in records)
            {
                await Console.Out.WriteLineAsync($"Artist: {r.Artist} - {r.Name} - {r.Recorded} ({r.Media}) {r.Rating}.");
            }
        }

        internal static async Task CountDiscsAsync(string media)
        {
            var discs = await _rd.CountAllDiscsAsync(media);

            switch (media)
            {
                case "":
                    await Console.Out.WriteLineAsync($"The total number of all discs is: {discs}");
                    break;
                case "DVD":
                    await Console.Out.WriteLineAsync($"The total number of all DVD, CD/DVD Blu-ray or CD/Blu-ray discs is: {discs}");
                    break;
                case "CD":
                    await Console.Out.WriteLineAsync($"The total number of audio discs is: {discs}");
                    break;
                case "R":
                    await Console.Out.WriteLineAsync($"The total number of vinyl discs is: {discs}");
                    break;
                default:
                    break;
            }
        }

        internal static async Task CountDiscsSPAsync(string media)
        {
            var discs = await _rd.CountAllDiscsSPAsync(media);

            switch (media)
            {
                case "":
                    await Console.Out.WriteLineAsync($"The total number of all discs is: {discs}");
                    break;
                case "DVD":
                    await Console.Out.WriteLineAsync($"The total number of all DVD, CD/DVD Blu-ray or CD/Blu-ray discs is: {discs}");
                    break;
                case "CD":
                    await Console.Out.WriteLineAsync($"The total number of audio discs is: {discs}");
                    break;
                case "R":
                    await Console.Out.WriteLineAsync($"The total number of vinyl discs is: {discs}");
                    break;
                default:
                    break;
            }
        }

        internal static async Task GetArtistRecordEntityAsync(int recordId)
        {
            var r = await _rd.GetArtistRecordEntityAsync(recordId);

            if (r.RecordId > 0)
            {
                await Console.Out.WriteLineAsync($"{r.Artist}\n");
                await Console.Out.WriteLineAsync($"\t{r.Recorded} - {r.Name} ({r.Media}) - Rating: {r.Rating}");
            }
        }

        internal static async Task GetArtistRecordEntitySPAsync(int recordId)
        {
            var r = await _rd.GetArtistRecordEntitySPAsync(recordId);

            if (r.RecordId > 0)
            {
                await Console.Out.WriteLineAsync($"{r.ArtistName}\n");
                await Console.Out.WriteLineAsync($"\t{r.Recorded} - {r.Name} ({r.Media}) - Rating: {r.Rating}");
            }
        }

        internal static async Task GetArtistNumberOfRecordsAsync(int artistId)
        {
            var artist = await _ad.GetArtistByIdAsync(artistId);
            var discs = await _rd.GetArtistNumberOfRecordsAsync(artistId);

            if (artist is ArtistModel)
            {
                await Console.Out.WriteLineAsync($"{artist.Name} has {discs} discs.");
            }
        }

        internal static async Task GetArtistNumberOfRecordsSPAsync(int artistId)
        {
            var artist = await _ad.GetArtistByIdAsync(artistId);
            int result = await _rd.GetArtistNumberOfRecordsSPAsync(artistId);

            if (result > 0)
            {
                await Console.Out.WriteLineAsync($"{artist.Name} has {result} discs.");
            }
        }

        internal static async Task GetRecordDetailsAsync(int recordId)
        {
            var record = await _rd.GetFormattedRecordAsync(recordId);

            if (record is RecordModel)
            {
                await Console.Out.WriteLineAsync(record.ToString());
            }
        }

        internal static async Task GetRecordDetailsSPAsync(int recordId)
        {
            var record = await _rd.GetFormattedRecordSPAsync(recordId);

            if (record is RecordModel)
            {
                await Console.Out.WriteLineAsync(record.ToString());
            }
        }

        internal static async Task GetArtistNameFromRecordAsync(int recordId)
        {
            var name = await _rd.GetArtistNameFromRecordAsync(recordId);
            await Console.Out.WriteLineAsync(name);
        }

        internal static async Task GetArtistNameFromRecordSPAsync(int recordId)
        {
            var name = await _rd.GetArtistNameFromRecordSPAsync(recordId);
            await Console.Out.WriteLineAsync(name);
        }

        internal static async Task GetDiscCountForYearAsync(int year)
        {
            var count = await _rd.GetDiscCountForYearAsync(year);

            await Console.Out.WriteLineAsync($"The total number of discs for {year} are {count}.");
        }

        internal static async Task GetDiscCountForYearSPAsync(int year)
        {
            var count = await _rd.GetDiscCountForYearSPAsync(year);

            await Console.Out.WriteLineAsync($"The total number of discs for {year} are {count}.");
        }

        internal static async Task GetBoughtDiscCountForYearAsync(string year)
        {
            var count = await _rd.GetBoughtDiscCountForYearAsync(year);

            await Console.Out.WriteLineAsync($"The total number of discs bought in {year} is {count}.");
        }

        internal static async Task GetBoughtDiscCountForYearSPAsync(string year)
        {
            var count = await _rd.GetBoughtDiscCountForYearSPAsync(year);

            await Console.Out.WriteLineAsync($"The total number of discs bought in {year} is {count}.");
        }

        internal static async Task GetNoRecordReviewAsync()
        {
            List<dynamic> records = await _rd.MissingRecordReviewsAsync();

            foreach (var record in records)
            {
                await Console.Out.WriteLineAsync($"{record.Artist} - Id: {record.RecordId} - {record.Name} - {record.Recorded}");
            }
        }

        internal static async Task GetNoRecordReviewSPAsync()
        {
            List<dynamic> records = await _rd.MissingRecordReviewsSPAsync();

            foreach (var record in records)
            {
                await Console.Out.WriteLineAsync($"{record.Artist} - Id: {record.RecordId} - {record.Name} - {record.Recorded}");
            }
        }

        internal static async Task GetNoReviewCountAsync()
        {
            var count = await _rd.GetNoReviewCountAsync();

            await Console.Out.WriteLineAsync($"The total number of empty Reviews is {count}.");
        }

        internal static async Task GetNoReviewCountSPAsync()
        {
            var count = await _rd.GetNoReviewCountSPAsync();

            await Console.Out.WriteLineAsync($"The total number of empty Reviews is {count}.");
        }

        internal static async Task GetTotalArtistCostAsync()
        {
            var list = await _rd.GetCostTotalsAsync();

            foreach (var item in list)
            {
                await Console.Out.WriteLineAsync($"Total cost for {item.Name} with {item.TotalDiscs} discs is ${item.TotalCost:F2}.");
            }
        }

        internal static async Task GetTotalArtistCostSPAsync()
        {
            var list = await _rd.GetCostTotalsSPAsync();

            foreach (var item in list)
            {
                await Console.Out.WriteLineAsync($"Total cost for {item.Name} with {item.TotalDiscs} discs is ${item.TotalCost:F2}.");
            }
        }

        internal static async Task GetTotalArtistDiscsAsync()
        {
            var list = await _rd.GetTotalArtistDiscsAsync();

            foreach (var item in list)
            {
                await Console.Out.WriteLineAsync($"Total number of discs for {item.Name} is {item.Discs}.");
            }
        }

        internal static async Task GetTotalArtistDiscsSPAsync()
        {
            var list = await _rd.GetTotalArtistDiscsSPAsync();

            foreach (var item in list)
            {
                await Console.Out.WriteLineAsync($"Total number of discs for {item.Name} is {item.TotalDiscs}.");
            }
        }

        internal static async Task RecordHtmlAsync(int recordId)
        {
            var r = await _rd.GetArtistRecordEntityAsync(recordId);

            if (r != null)
            {
                await Console.Out.WriteLineAsync($"<p><strong>ArtistId:</strong> {r.ArtistId}</p>\n<p><strong>Artist:</strong> {r.Artist}</p>\n<p><strong>RecordId:</strong> {r.RecordId}</p>\n<p><strong>Recorded:</strong> {r.Recorded}</p>\n<p><strong>Name:</strong> {r.Name}</p>\n<p><strong>Rating:</strong> {r.Rating}</p>\n<p><strong>Media:</strong> {r.Media}</p>\n");
            }
        }

        internal static async Task RecordHtmlSPAsync(int recordId)
        {
            var r = await _rd.GetArtistRecordEntitySPAsync(recordId);

            if (r != null)
            {
                await Console.Out.WriteLineAsync($"<p><strong>ArtistId:</strong> {r.ArtistId}</p>\n<p><strong>Artist:</strong> {r.Artist}</p>\n<p><strong>RecordId:</strong> {r.RecordId}</p>\n<p><strong>Recorded:</strong> {r.Recorded}</p>\n<p><strong>Name:</strong> {r.Name}</p>\n<p><strong>Rating:</strong> {r.Rating}</p>\n<p><strong>Media:</strong> {r.Media}</p>\n");
            }
        }
    }
}
