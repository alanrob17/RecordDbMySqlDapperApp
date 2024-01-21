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
        public static List<ArtistModel> GetArtists()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return cn.Query<ArtistModel>("SELECT * FROM Artist ORDER BY LastName, FirstName", new DynamicParameters()).ToList();
            }
        }

        public static List<ArtistModel> GetArtistsSP()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return cn.Query<ArtistModel>("GetAllArtists", commandType: CommandType.StoredProcedure).ToList(); 
            }
        }

        public static ArtistModel? GetArtistById(int artistId)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return cn.Query<ArtistModel>($"SELECT * FROM Artist WHERE ArtistId = {artistId}").FirstOrDefault() ?? new ArtistModel { ArtistId = 0 };
            }
        }

        public static ArtistModel? GetArtistByIdSP(int artistId)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var parameter = new DynamicParameters();
                parameter.Add("_artistId", artistId);

                return cn.Query<ArtistModel>("GetArtistById", parameter, commandType: CommandType.StoredProcedure).FirstOrDefault() ?? new ArtistModel { ArtistId = 0 };
            }
        }

        public static ArtistModel? GetArtistByName(ArtistModel artist)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return cn.Query<ArtistModel>($"SELECT * FROM Artist WHERE Name = '{artist.Name}'").FirstOrDefault() ?? new ArtistModel { Name = null };
            }
        }

        public static ArtistModel? GetArtistByNameSP(ArtistModel artist)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var parameter = new DynamicParameters();
                parameter.Add("@_name", artist.Name);

                return cn.Query<ArtistModel>("GetArtistByName", parameter, commandType: CommandType.StoredProcedure).FirstOrDefault() ?? new ArtistModel { ArtistId = 0 };
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

        public static int UpdateArtist(ArtistModel artist)
        {
            var i = 0;
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                artist.Name = !string.IsNullOrEmpty(artist.FirstName) ? $"{artist.FirstName} {artist.LastName}" : artist.LastName;

                i = cn.Execute("UPDATE Artist SET FirstName = @FirstName, LastName = @LastName, Name = @Name, Biography = @Biography WHERE ArtistId = @ArtistId", artist);
            }

            return i;
        }

        public static object UpdateArtistSP(ArtistModel artist)
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

                cn.Execute("UpdateArtistById", parameter, commandType: CommandType.StoredProcedure);

                var foundArtist = cn.QueryFirstOrDefault<ArtistModel>("SELECT * FROM Artist WHERE Name LIKE @Name", artist);
                artistId = foundArtist?.ArtistId ?? 0;
            }
            return artistId;
        }

        public static int AddArtist(ArtistModel artist)
        {
            var artistId = 0;

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                artist.Name = !string.IsNullOrEmpty(artist.FirstName) ? $"{artist.FirstName} {artist.LastName}" : artist.LastName;

                var foundArtist = cn.QueryFirstOrDefault<ArtistModel>("SELECT * FROM Artist WHERE Name LIKE @Name", artist);
                if (foundArtist != null)
                {
                    artistId = 9999;
                }
                else
                {
                    var number = cn.Execute("INSERT INTO Artist (FirstName, LastName, Name, Biography) VALUES (@FirstName, @LastName, @Name, @Biography)", artist);
                    if (number == 1)
                    {
                        foundArtist = cn.QueryFirstOrDefault<ArtistModel>("SELECT * FROM Artist WHERE Name LIKE @Name", artist);
                        artistId = foundArtist?.ArtistId ?? 0;
                    }
                }
            }

            return artistId;
        }

        public static int AddArtistSP(ArtistModel artist)
        {

            var artistId = 0;

            artist.Name = !string.IsNullOrEmpty(artist.FirstName) ? $"{artist.FirstName} {artist.LastName}" : artist.LastName;



            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var foundArtist = cn.QueryFirstOrDefault<ArtistModel>("SELECT * FROM Artist WHERE Name LIKE @Name", artist);
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

                    var result = cn.Execute("CreateArtist", parameters, commandType: CommandType.StoredProcedure);

                    foundArtist = cn.QueryFirstOrDefault<ArtistModel>("SELECT * FROM Artist WHERE Name LIKE @Name", artist);
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
