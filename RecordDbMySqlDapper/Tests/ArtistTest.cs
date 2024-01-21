using System;
using System.Collections.Generic;
using System.Configuration;
using Dapper;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperDAL.Models;
using _ad = DapperDAL.ArtistDataAccess;

namespace RecordDbMySqlDapper.Tests
{
    public class ArtistTest
    {
        internal static async Task CreateArtistAsync()
        {
            var artist = new ArtistModel
            {
                FirstName = "Chris",
                LastName = "Robson",
                Biography = "This is the Bio for Chris Robson."
            };

            var artistId = await _ad.AddArtistAsync(artist);

            if (artistId > 0 && artistId < 9999)
            {
                await Console.Out.WriteLineAsync($"Artist added with Id: {artistId}.");
            }
            else if (artistId == 9999)
            {
                await Console.Out.WriteLineAsync("ERROR: Artist already exists in the database!");
            }
            else
            {
                await Console.Out.WriteLineAsync("ERROR: Artist couldn't be added to the database!");
            }
        }

        internal static async Task CreateArtistSPAsync()
        {
            ArtistModel artist = new()
            {
                FirstName = "Ethan James",
                LastName = "Robson",
                Name = "",
                Biography = "Ethan is a Jazz Fusion artist."
            };

            var artistId = await _ad.AddArtistSPAsync(artist);

            if (artistId > 0 && artistId < 9999)
            {
                await Console.Out.WriteLineAsync($"Artist added with Id: {artistId}.");
            }
            else if (artistId == 9999)
            {
                await Console.Out.WriteLineAsync("ERROR: Artist already exists in the database!");
            }
            else
            {
                await Console.Out.WriteLineAsync("ERROR: Artist couldn't be added to the database!");
            }
        }

        internal static async Task GetArtistByNameAsync(string name)
        {
            var artistToFind = new ArtistModel { Name = name };

            var artist = await _ad.GetArtistByNameAsync(artistToFind);
            var message = artist?.ArtistId > 0 ? $"Id: {artist.ArtistId} - {artist.FirstName} {artist.LastName}." : "ERROR: Artist not found!";

            await Console.Out.WriteLineAsync(message);
        }

        internal static async Task GetArtistByNameSPAsync(string name)
        {
            var artistToFind = new ArtistModel { Name = name };

            var artist = await _ad.GetArtistByNameSPAsync(artistToFind);
            var message = artist?.ArtistId > 0 ? $"Id: {artist.ArtistId} - {artist.FirstName} {artist.LastName}." : "ERROR: Artist not found!";

            await Console.Out.WriteLineAsync(message);
        }

        internal static async Task GetAllArtistsAsync()
        {
            var artists = await _ad.GetArtistsAsync();

            foreach (var artist in artists)
            {
                await PrintArtist(artist);
            }
        }


        internal static async Task GetAllArtistsSPAsync()
        {
            var artists = await _ad.GetAllArtistsSPAsync();

            foreach (var artist in artists)
            {
                await PrintArtist(artist);
            }
        }

        internal static async Task UpdateArtistAsync(int artistId)
        {
            ArtistModel artist = new()
            {
                ArtistId = artistId,
                FirstName = "Christopher",
                LastName = "Robson",
                Biography = "Chris is an Australian C&W singer."
            };

            var i = await _ad.UpdateArtistAsync(artist);

            var message = i > 0 ? "Artist updated." : "ERROR: Artist not updated!";
            await Console.Out.WriteLineAsync(message);

        }

        internal static async Task UpdateArtistSPAsync(int artistId)
        {
            ArtistModel artist = new()
            {
                ArtistId = artistId,
                FirstName = "Alan",
                LastName = "Robson",
                Biography = "Alan is an Australian Hip-Hop superstar."
            };

            var result = await _ad.UpdateArtistSPAsync(artist);

            var message = result > 0 ? "Artist updated." : "ERROR: Artist not updated!";
            Console.WriteLine(message);
        }


        internal static async Task GetArtistByIdAsync(int artistId)
        {
            var artist = await _ad.GetArtistByIdAsync(artistId);
            var message = artist?.ArtistId > 0 ? $"Id: {artist.ArtistId} - {artist.FirstName} {artist.LastName}." : "ERROR: Artist not found!";

            await Console.Out.WriteLineAsync(message);
        }

        internal static async Task GetArtistByIdSPAsync(int artistId)
        {
            var artist = await _ad.GetArtistByIdSPAsync(artistId);
            var message = artist?.ArtistId > 0 ? $"Id: {artist.ArtistId} - {artist.FirstName} {artist.LastName}." : "ERROR: Artist not found!";

            await Console.Out.WriteLineAsync(message);
        }

        internal static void DeleteArtist(int artistId)
        {
            int result = _ad.DeleteArtist(artistId);
            var message = result > 0 ? "Artist deleted." : "ERROR: Artist not deleted!";
            Console.WriteLine(message);
        }

        internal static void DeleteArtistSP(int artistId)
        {
            int result = _ad.DeleteArtistSP(artistId);
            var message = result > 0 ? "Artist deleted." : "ERROR: Artist not deleted!";
            Console.WriteLine(message);
        }

        internal static void GetBiography(int artistid)
        {
            var biography = _ad.GetBiography(artistid);

            if (biography.Length > 5)
            {
                Console.WriteLine(biography);
            }
        }

        internal static void GetBiographySP(int artistid)
        {
            var biography = _ad.GetBiographySP(artistid);

            if (biography.Length > 5)
            {
                Console.WriteLine(biography);
            }
        }

        // TODO: Change to an Async method
        //internal static void ArtistHtml(int artistId)
        //{
        //    var artist = _ad.GetArtistById(artistId);
        //    var message = artist?.ArtistId > 0 ? $"<p><strong>Id:</strong> {artist.ArtistId}</p>\n<p><strong>Name:</strong> {artist.FirstName} {artist.LastName}</p>\n<p><strong>Biography:</strong></p>\n<div>{artist.Biography}</p></div>" : "ERROR: Artist not found!";

        //    Console.WriteLine(message);
        //}

        // TODO: Change to an Async method
        //internal static void ArtistHtmlSP(int artistId)
        //{
        //    var artist = _ad.GetArtistByIdSP(artistId);
        //    var message = artist?.ArtistId > 0 ? $"<p><strong>Id:</strong> {artist.ArtistId}</p>\n<p><strong>Name:</strong> {artist.FirstName} {artist.LastName}</p>\n<p><strong>Biography:</strong></p>\n<div>{artist.Biography}</p></div>" : "ERROR: Artist not found!";

        //    Console.WriteLine(message);
        //}

        internal static void GetArtistId(string firstName, string lastName)
        {
            var artistToFind = new ArtistModel { FirstName = firstName, LastName = lastName };

            var artist = _ad.GetArtistByFirstLastName(artistToFind);
            var message = artist?.ArtistId > 0 ? $"Id: {artist.ArtistId} - {artist.FirstName} {artist.LastName}." : "ERROR: Artist not found!";

            Console.WriteLine(message);

        }

        internal static void GetArtistIdSP(string firstName, string lastName)
        {
            var artistToFind = new ArtistModel { FirstName = firstName, LastName = lastName };

            var artist = _ad.GetArtistByFirstLastNameSP(artistToFind);
            var message = artist?.ArtistId > 0 ? $"Id: {artist.ArtistId}." : "ERROR: Artist not found!";

            Console.WriteLine(message);

        }

        internal static void GetArtistsWithNoBio()
        {
            List<ArtistModel> artists = _ad.GetArtistsWithNoBio();

            foreach (var artist in artists)
            {
                Console.WriteLine($"Id: {artist.ArtistId} - {artist.Name}");
            }
        }

        internal static void GetArtistsWithNoBioSP()
        {
            List<ArtistModel> artists = _ad.GetArtistsWithNoBioSP();

            foreach (var artist in artists)
            {
                Console.WriteLine($"Id: {artist.ArtistId} - {artist.Name}");
            }
        }

        internal static void GetNoBiographyCount()
        {
            var number = _ad.NoBiographyCount();

            Console.WriteLine($"The total number of artists with missing biographies: {number}.");
        }

        internal static void GetNoBiographyCountSP()
        {
            var number = _ad.NoBiographyCountSP();

            Console.WriteLine($"The total number of artists with missing biographies: {number}.");
        }

        internal static async Task PrintArtist(ArtistModel artist)
        {
            try
            {
                var bio = string.IsNullOrEmpty(artist.Biography) ? "No Biography" : (artist.Biography.Length > 30 ? artist.Biography.Substring(0, 30) + "..." : artist.Biography);
                await Console.Out.WriteLineAsync($"Id: {artist.ArtistId} - {artist.FirstName} {artist.LastName}\n{bio}\n");
            }
            catch (Exception ex)
            {

                await Console.Out.WriteLineAsync($"No artist was found.\n\n{ex.Message}");
            }
        }
    }
}