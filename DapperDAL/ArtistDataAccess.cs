using DapperDAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection.Metadata;
using MySql.Data.MySqlClient;
using _ap = DapperDAL.Components.AppSettings;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DapperDAL
{
    public class ArtistDataAccess
    {
        public static async Task<List<ArtistModel>> GetArtistsAsync()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var result = await cn.QueryAsync<ArtistModel>("SELECT * FROM Artist ORDER BY LastName, FirstName", new DynamicParameters());
                return result.ToList();
            }
        }

        public static async Task<List<ArtistModel>> GetAllArtistsSPAsync()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var result = await cn.QueryAsync<ArtistModel>("GetAllArtists", commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public static async Task<ArtistModel>? GetArtistByIdAsync(int artistId)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var result = await cn.QueryAsync<ArtistModel>($"SELECT * FROM Artist WHERE ArtistId = {artistId}");
                return result.FirstOrDefault() ?? new ArtistModel { ArtistId = 0 };
            }
        }

        public static async Task<ArtistModel>? GetArtistByIdSPAsync(int artistId)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var parameter = new DynamicParameters();
                parameter.Add("_artistId", artistId);

                var result = await cn.QueryAsync<ArtistModel>("GetArtistById", parameter, commandType: CommandType.StoredProcedure);
                return result.FirstOrDefault() ?? new ArtistModel { ArtistId = 0 };
            }
        }

        public static async Task<ArtistModel>? GetArtistByNameAsync(ArtistModel artist)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var result = await cn.QueryAsync<ArtistModel>($"SELECT * FROM Artist WHERE Name = '{artist.Name}'");
                return result.FirstOrDefault() ?? new ArtistModel { Name = null };
            }
        }

        public static async Task<ArtistModel>? GetArtistByNameSPAsync(ArtistModel artist)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var parameter = new DynamicParameters();
                parameter.Add("@_name", artist.Name);

                var result = await cn.QueryAsync<ArtistModel>("GetArtistByName", parameter, commandType: CommandType.StoredProcedure);
                return result.FirstOrDefault() ?? new ArtistModel { ArtistId = 0 };
            }
        }
        public static ArtistModel? GetArtistByFirstLastName(ArtistModel artist)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return cn.Query<ArtistModel>("SELECT * FROM Artist WHERE FirstName LIKE @FirstName AND LastName LIKE @LastName", artist).FirstOrDefault() ?? new ArtistModel { ArtistId = 0 };
            }
        }

        public static ArtistModel? GetArtistByFirstLastNameSP(ArtistModel artist)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var parameter = new DynamicParameters();
                parameter.Add("_firstName", artist.FirstName);
                parameter.Add("_lastName", artist.LastName);

                return cn.Query<ArtistModel>("GetArtistIdByNames", parameter, commandType: CommandType.StoredProcedure).FirstOrDefault() ?? new ArtistModel { ArtistId = 0 };
            }
        }

        public static async Task<int> UpdateArtistAsync(ArtistModel artist)
        {
            var i = 0;
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                artist.Name = !string.IsNullOrEmpty(artist.FirstName) ? $"{artist.FirstName} {artist.LastName}" : artist.LastName;

                i = await cn.ExecuteAsync("UPDATE Artist SET FirstName = @FirstName, LastName = @LastName, Name = @Name, Biography = @Biography WHERE ArtistId = @ArtistId", artist);
            }

            return i;
        }

        public static async Task<int> UpdateArtistSPAsync(ArtistModel artist)
        {
            var artistId = 0;
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                if (string.IsNullOrEmpty(artist.Name))
                {
                    artist.Name = !string.IsNullOrEmpty(artist.FirstName) ? $"{artist.FirstName} {artist.LastName}" : artist.LastName;
                }

                var parameter = new DynamicParameters();
                parameter.Add("_artistId", artist.ArtistId);
                parameter.Add("_firstName", artist.FirstName);
                parameter.Add("_lastName", artist.LastName);
                parameter.Add("_name", artist.Name);
                parameter.Add("_biography", artist.Biography);

                await cn.ExecuteAsync("UpdateArtistById", parameter, commandType: CommandType.StoredProcedure);

                var foundArtist = await cn.QueryFirstOrDefaultAsync<ArtistModel>("SELECT * FROM Artist WHERE Name LIKE @Name", artist);
                artistId = foundArtist?.ArtistId ?? 0;
            }
            return artistId;
        }

        public static async Task<int> AddArtistAsync(ArtistModel artist)
        {
            var artistId = 0;

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                artist.Name = !string.IsNullOrEmpty(artist.FirstName) ? $"{artist.FirstName} {artist.LastName}" : artist.LastName;

                var foundArtist = await cn.QueryFirstOrDefaultAsync<ArtistModel>("SELECT * FROM Artist WHERE Name LIKE @Name", artist);
                if (foundArtist != null)
                {
                    artistId = 9999;
                }
                else
                {
                    var number = await cn.ExecuteAsync("INSERT INTO Artist (FirstName, LastName, Name, Biography) VALUES (@FirstName, @LastName, @Name, @Biography)", artist);
                    if (number == 1)
                    {
                        foundArtist = await cn.QueryFirstOrDefaultAsync<ArtistModel>("SELECT * FROM Artist WHERE Name LIKE @Name", artist);
                        artistId = foundArtist?.ArtistId ?? 0;
                    }
                }
            }

            return artistId;
        }

        public static async Task<int> AddArtistSPAsync(ArtistModel artist)
        {

            var artistId = 0;

            artist.Name = !string.IsNullOrEmpty(artist.FirstName) ? $"{artist.FirstName} {artist.LastName}" : artist.LastName;



            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var foundArtist = await cn.QueryFirstOrDefaultAsync<ArtistModel>("SELECT * FROM Artist WHERE Name LIKE @Name", artist);
                if (foundArtist != null)
                {
                    artistId = 9999;
                }
                else
                {

                    var parameters = new DynamicParameters();
                    parameters.Add("@_firstName", artist.FirstName);
                    parameters.Add("@_lastName", artist.LastName);
                    parameters.Add("@_name", artist.Name);
                    parameters.Add("@_biography", artist.Biography);

                    var result = await cn.ExecuteAsync("CreateArtist", parameters, commandType: CommandType.StoredProcedure);

                    foundArtist = await cn.QueryFirstOrDefaultAsync<ArtistModel>("SELECT * FROM Artist WHERE Name LIKE @Name", artist);
                    artistId = foundArtist?.ArtistId ?? 0;
                }
            }

            return artistId;
        }

        public static int DeleteArtist(int artistId)
        {
            var result = 0;
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                // Delete records before deleting artist
                var records = cn.Query<RecordModel>($"SELECT * FROM Record WHERE artistId = {artistId}", new DynamicParameters());
                foreach (var record in records)
                {
                    cn.Execute($"DELETE FROM Record WHERE ArtistId={artistId}");
                }

                result = cn.Execute($"DELETE FROM Artist WHERE ArtistId={artistId}");
            }

            return result;
        }

        public static int DeleteArtistSP(int artistId)
        {
            var result = 0;
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var parameters = new DynamicParameters();
                parameters.Add("_artistId", artistId);

                result = (int)cn.Execute("DeleteArtistById", parameters, commandType: CommandType.StoredProcedure);
            }
            return result;
        }

        /// <summary>
        /// Get biography from the current Artist Id.
        /// </summary>
        public static string GetBiography(int artistId)
        {
            var biography = new StringBuilder();

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var artist = cn.Query<ArtistModel>("SELECT * FROM Artist WHERE artistId = @artistId", new { artistId }).FirstOrDefault();
                if (artist is ArtistModel)
                {
                    biography.Append($"Name: {artist.Name}\n");
                    biography.Append($"Biography:\n{artist.Biography}");
                }
            }

            return biography.ToString();
        }

        /// <summary>
        /// Get biography from the current Artist Id.
        /// </summary>
        public static string GetBiographySP(int artistId)
        {
            var biography = new StringBuilder();

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var parameters = new DynamicParameters();
                parameters.Add("_artistId", artistId);

                var artist = cn.QueryFirstOrDefault<ArtistModel>("GetArtistById", parameters, commandType: CommandType.StoredProcedure);
                if (artist is ArtistModel)
                {
                    biography.Append($"Name: {artist.Name}\n");
                    biography.Append($"Biography:\n{artist.Biography}");
                }
            }

            return biography.ToString();
        }

        public static List<ArtistModel> GetArtistsWithNoBio()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return cn.Query<ArtistModel>("SELECT * FROM Artist WHERE Biography IS NULL;").ToList();
            }
        }

        public static List<ArtistModel> GetArtistsWithNoBioSP()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return cn.Query<ArtistModel>("GetArtistsWithNoBio", commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public static int NoBiographyCount()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return cn.ExecuteScalar<int>("SELECT Count(*) FROM Artist WHERE Biography IS NULL;");
            }
        }

        public static int NoBiographyCountSP()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return cn.ExecuteScalar<int>("GetNoBiographyCount", commandType: CommandType.StoredProcedure);
            }
        }

        public static string LoadConnectionString()
        {
            return _ap.Instance.ConnectionString;
        }
    }
}
