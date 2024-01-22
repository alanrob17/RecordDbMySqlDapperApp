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
                FirstName = "Alan",
                LastName = "Robson",
                Name = "",
                Biography = "Alan is a Jazz Fusion artist."
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
                FirstName = "Ethan",
                LastName = "Robson",
                Biography = "Ethan is an Australian Techno superstar."
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

        internal static async Task DeleteArtistAsync(int artistId)
        {
            int result = await _ad.DeleteArtistAsync(artistId);
            var message = result > 0 ? "Artist deleted." : "ERROR: Artist not deleted!";
            await Console.Out.WriteLineAsync(message);
        }

        internal static async Task DeleteArtistSPAsync(int artistId)
        {
            int result = await _ad.DeleteArtistSPAsync(artistId);
            var message = result > 0 ? "Artist deleted." : "ERROR: Artist not deleted!";
            await Console.Out.WriteLineAsync(message);
        }

        internal static async Task GetBiographyAsync(int artistid)
        {
            var biography = await _ad.GetBiographyAsync(artistid);

            if (biography.Length > 5)
            {
                await Console.Out.WriteLineAsync(biography);
            }
        }

        internal static async Task GetBiographySPAsync(int artistid)
        {
            var biography = await _ad.GetBiographySPAsync(artistid);

            if (biography.Length > 5)
            {
                await Console.Out.WriteLineAsync(biography);
            }
        }

        internal static async Task ArtistHtmlAsync(int artistId)
        {
            var artist = await _ad.GetArtistByIdAsync(artistId);
            var message = artist?.ArtistId > 0 ? $"<p><strong>Id:</strong> {artist.ArtistId}</p>\n<p><strong>Name:</strong> {artist.FirstName} {artist.LastName}</p>\n<p><strong>Biography:</strong></p>\n<div>{artist.Biography}</p></div>" : "ERROR: Artist not found!";

            Console.WriteLine(message);
        }

        internal static async Task ArtistHtmlSPAsync(int artistId)
        {
            var artist = await _ad.GetArtistByIdSPAsync(artistId);
            var message = artist?.ArtistId > 0 ? $"<p><strong>Id:</strong> {artist.ArtistId}</p>\n<p><strong>Name:</strong> {artist.FirstName} {artist.LastName}</p>\n<p><strong>Biography:</strong></p>\n<div>{artist.Biography}</p></div>" : "ERROR: Artist not found!";

            await Console.Out.WriteLineAsync(message);
        }

        internal static async Task GetArtistIdAsync(string firstName, string lastName)
        {
            var artistToFind = new ArtistModel { FirstName = firstName, LastName = lastName };

            var artist = await _ad.GetArtistByFirstLastNameAsync(artistToFind);
            var message = artist?.ArtistId > 0 ? $"Id: {artist.ArtistId} - {artist.FirstName} {artist.LastName}." : "ERROR: Artist not found!";

            await Console.Out.WriteLineAsync(message);

        }

        internal static async Task GetArtistIdSPAsync(string firstName, string lastName)
        {
            var artistToFind = new ArtistModel { FirstName = firstName, LastName = lastName };

            var artist = await _ad.GetArtistByFirstLastNameSPAsync(artistToFind);
            var message = artist?.ArtistId > 0 ? $"Id: {artist.ArtistId}." : "ERROR: Artist not found!";

            await Console.Out.WriteLineAsync(message);

        }

        internal static async Task GetArtistsWithNoBioAsync()
        {
            List<ArtistModel> artists = await _ad.GetArtistsWithNoBioAsync();

            foreach (var artist in artists)
            {
                await Console.Out.WriteLineAsync($"Id: {artist.ArtistId} - {artist.Name}");
            }
        }

        internal static async Task GetArtistsWithNoBioSPAsync()
        {
            List<ArtistModel> artists = await _ad.GetArtistsWithNoBioSPAsync();

            foreach (var artist in artists)
            {
                await Console.Out.WriteLineAsync($"Id: {artist.ArtistId} - {artist.Name}");
            }
        }

        internal static async Task GetNoBiographyCountAsync()
        {
            var number = await _ad.NoBiographyCountAsync();

            await Console.Out.WriteLineAsync($"The total number of artists with missing biographies: {number}.");
        }

        internal static async Task GetNoBiographyCountSPAsync()
        {
            var number = await _ad.NoBiographyCountSPAsync();

            await Console.Out.WriteLineAsync($"The total number of artists with missing biographies: {number}.");
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