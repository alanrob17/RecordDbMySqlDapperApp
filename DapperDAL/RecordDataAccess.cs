using Dapper;
using DapperDAL.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using _ap = DapperDAL.Components.AppSettings;

namespace DapperDAL
{
    public class RecordDataAccess
    {
        public static async Task<List<RecordModel>> GetRecordsAsync()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var result = await cn.QueryAsync<RecordModel>("SELECT * FROM Record ORDER BY Recorded DESC", new DynamicParameters());
                return result.ToList();
            }
        }

        public static async Task<List<RecordModel>> GetRecordsSPAsync()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var result = await cn.QueryAsync<RecordModel>("GetFullRecords", commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public static async Task<List<RecordModel>> GetRecordsByArtistIdAsync(int artistId)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var result = await cn.QueryAsync<RecordModel>($"SELECT * FROM Record WHERE ArtistId = {artistId} ORDER BY Recorded DESC");
                return result.ToList();
            }
        }

        public static async Task<List<RecordModel>> GetRecordsByArtistIdSPAsync(int artistId)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var parameter = new DynamicParameters();
                parameter.Add("_artistId", artistId);

                var result = await cn.QueryAsync<RecordModel>($"GetRecordsByArtistId", parameter, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public static async Task<ArtistModel> GetRecordsByArtistIdMultipleTablesAsync(int artistId)
        {
            string query = "select * from Artist where artistId = @artistId ; select * from Record where artistId = @artistId order by Recorded;";

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var multipleResult = await cn.QueryMultipleAsync(query, new { artistId = artistId });

                var artist = multipleResult.Read<ArtistModel>().SingleOrDefault();
                if (artist is ArtistModel)
                {
                    var records = multipleResult.Read<RecordModel>().ToList();
                    artist.Records = records;
                    return artist;
                }
                else
                {
                    return new ArtistModel { ArtistId = 0 };
                }
            }
        }

        public static async Task<ArtistModel> GetRecordsByArtistIdMultipleTablesSPAsync(int artistId)
        {

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var parameter = new DynamicParameters();
                parameter.Add("_artistId", artistId);

                var result = await cn.QueryAsync<ArtistModel>("GetArtistById", parameter, commandType: CommandType.StoredProcedure);
                var artist = result.SingleOrDefault();
                var results = await cn.QueryAsync<RecordModel>("GetRecordsByArtistId", parameter, commandType: CommandType.StoredProcedure);
                var records = results.ToList();

                if (artist is ArtistModel)
                {
                    artist.Records = records;
                    return (ArtistModel)artist;
                }
                    
                return new ArtistModel { ArtistId = 0 };
            }
        }

        public static async Task<List<ArtistModel>> GetArtistRecordsMultipleTablesAsync()
        {
            List<ArtistModel> artists = new();

            string query = "select * from Artist order by LastName, FirstName; select * from Record order by ArtistId, Recorded;";

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var multipleResult = await cn.QueryMultipleAsync(query);

                artists = multipleResult.Read<ArtistModel>().ToList();
                var records = multipleResult.Read<RecordModel>().ToList();

                foreach (var artist in artists)
                {
                    artist.Records = records.Where(r => r.ArtistId == artist.ArtistId).ToList();
                }
            }

            return artists;
        }

        public static async Task<List<ArtistModel>> GetArtistRecordsMultipleTablesSPAsync()
        {
            List<ArtistModel> artists = new();


            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var result = await cn.QueryAsync<ArtistModel>("GetAllArtists", commandType: CommandType.StoredProcedure);
                artists = result.ToList();
                var results = await cn.QueryAsync<RecordModel>("GetFullRecords", commandType: CommandType.StoredProcedure);
                var records = results.ToList();

                foreach (var artist in artists)
                {
                    artist.Records = records.Where(r => r.ArtistId == artist.ArtistId).ToList();
                }
            }

            return artists;
        }

        public static async Task<List<RecordModel>> GetRecordsByYearAsync(int year)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var records = await cn.QueryAsync<RecordModel>($"SELECT * FROM Record WHERE Recorded = {year} ORDER BY ArtistId");
                return records.ToList();
            }
        }

        public static async Task<List<RecordModel>> GetRecordsByYearSPAsync(int year)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var parameter = new DynamicParameters();
                parameter.Add("_year", year);

                var records = await cn.QueryAsync<RecordModel>($"GetRecordsByYear", parameter, commandType: CommandType.StoredProcedure);
                return records.ToList();
            }
        }

        public static async Task<RecordModel> GetRecordByIdAsync(int recordId)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var result = await cn.QueryAsync<RecordModel>($"SELECT * FROM Record WHERE RecordId = {recordId}");
                return result.FirstOrDefault() ?? new RecordModel { RecordId = 0 };
            }
        }

        public static async Task<RecordModel> GetRecordByIdSPAsync(int recordId)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var parameter = new DynamicParameters();
                parameter.Add("_recordId", recordId);

                var result = await cn.QueryAsync<RecordModel>($"GetRecordById", parameter, commandType: CommandType.StoredProcedure);
                return result.FirstOrDefault() ?? new RecordModel { RecordId = 0 };
            }
        }

        public static async Task<RecordModel> GetRecordByNameAsync(RecordModel record)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var result = await cn.QueryAsync<RecordModel>($"SELECT * FROM Record WHERE Name LIKE @Name", record);
                return result.FirstOrDefault() ?? new RecordModel { RecordId = 0 };
            }
        }

        public static async Task<RecordModel> GetRecordByNameSPAsync(RecordModel record)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var parameter = new DynamicParameters();
                parameter.Add("_name", record.Name);

                var result = await cn.QueryAsync<RecordModel>($"GetRecordByName", parameter, commandType: CommandType.StoredProcedure);
                return  result.FirstOrDefault() ?? new RecordModel { RecordId = 0 };
            }
        }

        public static async Task<int> UpdateRecordAsync(RecordModel record)
        {
            var result = 0;
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                result = await cn.ExecuteAsync("UPDATE Record SET Name = @Name, Field = @Field, Recorded = @Recorded, Label = @Label, Pressing = @Pressing, Rating = @Rating, Discs = @Discs, Media = @Media, Bought = @Bought, Cost = @Cost, Review = @Review WHERE RecordId = @RecordId", record);
            }

            return result;
        }

        public static async Task<int> UpdateRecordSPAsync(RecordModel record)
        {
            var result = 0;
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var parameter = new DynamicParameters();
                parameter.Add("_recordId", record.RecordId);
                parameter.Add("_name", record.Name);
                parameter.Add("_field", record.Field);
                parameter.Add("_recorded", record.Recorded);
                parameter.Add("_label", record.Label);
                parameter.Add("_pressing", record.Pressing);
                parameter.Add("_rating", record.Rating);
                parameter.Add("_discs", record.Discs);
                parameter.Add("_media", record.Media);
                parameter.Add("_bought", record.Bought);
                parameter.Add("_cost", record.Cost);
                parameter.Add("_review", record.Review);

                await cn.ExecuteAsync("UpdateRecord", parameter, commandType: CommandType.StoredProcedure);

                result = parameter.Get<int>("_recordId");
            }
            return result;
        }

        public static async Task<int> AddRecordAsync(RecordModel record)
        {
            var result = 0;
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var query = @"INSERT INTO Record (ArtistId, Name, Field, Recorded, Label, Pressing, Rating, Discs, Media, Bought, Cost, Review) VALUES (@ArtistId, @Name, @Field, @Recorded, @Label, @Pressing, @Rating, @Discs, @Media, @Bought, @Cost, @Review);";
                result = await cn.ExecuteAsync(query, record);
            }

            return result;
        }

        public static async Task<int> AddRecordSPAsync(RecordModel record)
        {
            var recordId = 0;
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var parameter = new DynamicParameters();
                parameter.Add("_recordId", record.RecordId, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
                parameter.Add("_artistId", record.ArtistId);
                parameter.Add("_name", record.Name);
                parameter.Add("_field", record.Field);
                parameter.Add("_recorded", record.Recorded);
                parameter.Add("_label", record.Label);
                parameter.Add("_pressing", record.Pressing);
                parameter.Add("_rating", record.Rating);
                parameter.Add("_discs", record.Discs);
                parameter.Add("_media", record.Media);
                parameter.Add("_bought", record.Bought);
                parameter.Add("_cost", record.Cost);
                parameter.Add("_review", record.Review);

                await cn.ExecuteAsync("CreateRecord", parameter, commandType: CommandType.StoredProcedure);

                recordId = parameter.Get<int>("_recordId");
            }
            return recordId;
        }

        public static async Task<int> DeleteRecordAsync(int recordId)
        {
            var result = 0;
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                result = await cn.ExecuteAsync($"DELETE FROM Record WHERE RecordId={recordId}");
            }

            return result;
        }

        public static async Task<int> DeleteRecordSPAsync(int recordId)
        {
            var result = 0;
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var parameters = new DynamicParameters();
                parameters.Add("_recordId", recordId);

                result = (int)await cn.ExecuteAsync("DeleteRecordById", parameters, commandType: CommandType.StoredProcedure);
            }
            return result;
        }

        public static async Task<int> GetTotalNumberOfCDsAsync()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return await cn.ExecuteScalarAsync<int>("SELECT SUM(Discs) FROM Record WHERE Media = 'CD' OR Media = 'CD/DVD'");
            }
        }

        public static async Task<int> GetTotalNumberOfCDsSPAsync()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return await cn.ExecuteScalarAsync<int>("GetTotalCDCount", commandType: CommandType.StoredProcedure);
            }
        }

        public static async Task<int> GetTotalNumberOfDiscsAsync()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return await cn.ExecuteScalarAsync<int>("SELECT SUM(Discs) FROM Record");
            }
        }

        public static async Task<int> GetTotalNumberOfDiscsSPAsync()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return await cn.ExecuteScalarAsync<int>("GetTotalNumberOfAllRecords", commandType: CommandType.StoredProcedure);
            }
        }

        public static async Task<int> GetTotalNumberOfRecordsAsync()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return await cn.ExecuteScalarAsync<int>("SELECT SUM(Discs) FROM Record WHERE Media = 'R'");
            }
        }

        public static async Task<int> GetTotalNumberOfRecordsSPAsync()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return await cn.ExecuteScalarAsync<int>("GetTotalNumberOfRecords", commandType: CommandType.StoredProcedure);
            }
        }

        public static async Task<int> GetTotalNumberOfBluraysAsync()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return await cn.ExecuteScalarAsync<int>("SELECT SUM(Discs) FROM Record WHERE Media = 'CD/Blu-ray' OR Media = 'Blu-ray'");
            }
        }

        public static async Task<int> GetTotalNumberOfBluraysSPAsync()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return await cn.ExecuteScalarAsync<int>("GetTotalNumberOfAllBlurays", commandType: CommandType.StoredProcedure);
            }
        }

        public static async Task<List<dynamic>> GetArtistRecordListAsync()
        {
            string query = "SELECT a.ArtistId, a.FirstName, a.LastName, a.Name AS Artist, r.RecordId, r.Name, r.Recorded, r.Media, r.Rating " +
                           "FROM Artist a " +
                           "JOIN Record r ON a.ArtistId = r.ArtistId " +
                           "ORDER BY a.LastName, a.FirstName, r.Recorded";

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var result = await cn.QueryAsync<dynamic>(query);
                return result.ToList();
            }
        }

        public static async Task<List<dynamic>> GetArtistRecordListSPAsync()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var result = await cn.QueryAsync<dynamic>("GetAllArtistsAndRecords", commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public static async Task<int> CountAllDiscsAsync(string media = "")
        {
            var mediaType = string.Empty;

            switch (media)
            {
                case "":
                    mediaType = "'DVD' OR Media = 'CD/DVD' OR Media = 'Blu-ray' OR Media = 'CD/Blu-ray' OR Media = 'CD' OR Media = 'R'";
                    break;
                case "DVD":
                    mediaType = "'DVD' OR Media = 'CD/DVD' OR Media = 'Blu-ray' OR Media = 'CD/Blu-ray'";
                    break;
                case "CD":
                    mediaType = "'CD'";
                    break;
                case "R":
                    mediaType = "'R'";
                    break;
                default:
                    break;
            }

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return await cn.ExecuteScalarAsync<int>($"SELECT SUM(Discs) FROM Record WHERE Media = {mediaType}");
            }
        }

        public static async Task<int> CountAllDiscsSPAsync(string media = "")
        {
            var mediaType = 0;

            switch (media)
            {
                case "":
                    mediaType = 0;
                    break;
                case "DVD":
                    mediaType = 1;
                    break;
                case "CD":
                    mediaType = 2;
                    break;
                case "R":
                    mediaType = 3;
                    break;
                default:
                    break;
            }

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var parameter = new DynamicParameters();
                parameter.Add("_mediaType", mediaType);

                return await cn.ExecuteScalarAsync<int>("GetMediaCountByType", parameter, commandType: CommandType.StoredProcedure);
            }
        }

        public static async Task<dynamic>? GetArtistRecordEntityAsync(int recordId)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var query = @"
                    SELECT 
                        r.*,
                        a.ArtistId as ArtistId,
                        a.FirstName as FirstName,
                        a.LastName as LastName,
                        a.Name as Artist
                    FROM Record r
                    JOIN Artist a ON a.ArtistId = r.ArtistId
                    WHERE r.RecordId = @recordId";

                var result = await cn.QueryFirstOrDefaultAsync<dynamic>(query, new { recordId });

                if (result == null)
                {
                    result = new ExpandoObject();
                    result.RecordId = 0;
                }

                return result;
            }
        }

        public static async Task<dynamic>? GetArtistRecordEntitySPAsync(int recordId)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var parameter = new DynamicParameters();
                parameter.Add("_recordId", recordId);

                var result = await cn.QueryFirstOrDefaultAsync<dynamic>("GetArtistRecordByRecordId", parameter, commandType: CommandType.StoredProcedure);

                if (result == null)
                {
                    result = new ExpandoObject();
                    result.RecordId = 0;
                }

                return result;
            }
        }

        public static async Task<int> GetArtistNumberOfRecordsAsync(int artistId)
        {
            var discs = 0;

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return cn.ExecuteScalar<int>("SELECT SUM(Discs) FROM Record WHERE artistId = @artistId;", new { artistId });
            }
        }

        public static async Task<int> GetArtistNumberOfRecordsSPAsync(int artistId)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var parameter = new DynamicParameters();
                parameter.Add("_artistId", artistId);

                var result = await cn.QueryFirstOrDefaultAsync<int>("GetArtistNumberOfRecords", parameter, commandType: CommandType.StoredProcedure);

                return result;
            }
        }

        /// <summary>
        /// Get record details from ToString method.
        /// </summary>
        public static async Task<RecordModel> GetFormattedRecordAsync(int recordId)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var result = await cn.QueryAsync<RecordModel>($"SELECT * FROM Record WHERE RecordId = {recordId}");
                return result.FirstOrDefault() ?? new RecordModel { RecordId = 0 };
            }
        }

        /// <summary>
        /// Get record details from ToString method.
        /// </summary>
        public static async Task<RecordModel> GetFormattedRecordSPAsync(int recordId)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var parameter = new DynamicParameters();
                parameter.Add("_recordId", recordId);

                return await cn.QueryFirstOrDefaultAsync<RecordModel>("GetSingleRecord", parameter, commandType: CommandType.StoredProcedure);
            }
        }

        public static async Task<string> GetArtistNameFromRecordAsync(int recordId)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var query = "SELECT a.Name FROM Artist a INNER JOIN Record r ON a.ArtistId = r.ArtistId WHERE r.RecordId = @recordId";
                var parameters = new { recordId };

                return await cn.QuerySingleOrDefaultAsync<string>(query, parameters);
            }
        }

        public static async Task<string> GetArtistNameFromRecordSPAsync(int recordId)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var _recordId = recordId;
                var parameter = new { _recordId };

                return await cn.QuerySingleOrDefaultAsync<string>("GetArtistNameByRecordId", parameter, commandType: CommandType.StoredProcedure);
            }
        }

        public static async Task<int> GetDiscCountForYearAsync(int year)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var result = await cn.QueryAsync<int>($"SELECT SUM(Discs) FROM Record WHERE Recorded = {year};");
                return result.FirstOrDefault();
            }
        }

        public static async Task<int> GetDiscCountForYearSPAsync(int year)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var _year = year;   
                var parameter = new { _year };
                return await cn.ExecuteScalarAsync<int>($"GetRecordedYearNumber", parameter, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Get the number of discs that I bought for a particular year.
        /// </summary>
        public static async Task<int> GetBoughtDiscCountForYearAsync(string year)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                // get year from bought date
                var result = await cn.QueryAsync<int>($"SELECT SUM(Discs) FROM Record WHERE Bought like '%{year}%'");
                return result.FirstOrDefault();
            }
        }

        /// <summary>
        /// Get the number of discs that I bought for a particular year.
        /// </summary>
        public static async Task<int> GetBoughtDiscCountForYearSPAsync(string year)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var _year = year;
                var parameter = new { _year };

                // get year from bought date
                return await cn.ExecuteScalarAsync<int>("GetBoughtDiscCountForYear", parameter, commandType: CommandType.StoredProcedure);
            }
        }

        public static async Task<List<dynamic>> MissingRecordReviewsAsync()
        {
            var query = "SELECT a.ArtistId, A.Name AS Artist, r.RecordId, r.Name, r.Recorded, r.Discs, r.Rating, r.Media " +
                "FROM Artist a " +
                "INNER JOIN Record r ON a.ArtistId = r.ArtistId " +
                "WHERE r.Review IS NULL " +
                "ORDER BY a.LastName, a.FirstName;";

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var result = await cn.QueryAsync<dynamic>(query);
                return result.ToList();
            }
        }

        public static async Task<List<dynamic>> MissingRecordReviewsSPAsync()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var result = await cn.QueryAsync<dynamic>("MissingRecordReview", commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public static async Task<int> GetNoReviewCountAsync()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var result = await cn.QueryAsync<int>($"SELECT * FROM Record WHERE Review IS NULL;");
                return result.Count();
            }
        }

        public static async Task<int> GetNoReviewCountSPAsync()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return cn.ExecuteScalar<int>($"GetNoRecordReviewCount", commandType: CommandType.StoredProcedure);
            }
        }

        public static async Task<List<dynamic>> GetCostTotalsAsync()
        {
            var artistList = new List<dynamic>();
            var query = "SELECT " +
                        "a.ArtistId, " +
                        "TRIM(IFNULL(CONCAT(a.FirstName, ' ', a.LastName), a.LastName)) AS Name, " +
                        "SubQuery.TotalDiscs, " +
                        "SubQuery.TotalCost " +
                        "FROM " +
                        "Artist a " +
                        "JOIN ( " +
                        "SELECT " +
                        "r.ArtistId, " +
                        "SUM(Discs) AS TotalDiscs, " +
                        "SUM(Cost) AS TotalCost " +
                        "FROM " +
                        "Record r " +
                        "GROUP BY " +
                        "r.ArtistId " +
                        ") AS SubQuery ON a.ArtistId = SubQuery.ArtistId " +
                        "ORDER BY " +
                        "SubQuery.TotalCost DESC;";

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                artistList = (dynamic)await cn.QueryAsync<dynamic>(query);
                return artistList.ToList();
            }
        }

        public static async Task<List<dynamic>> GetCostTotalsSPAsync()
        {
            var artistList = new List<dynamic>();

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                artistList = (dynamic)await cn.QueryAsync<dynamic>("GetTotalsForEachArtist", commandType: CommandType.StoredProcedure);
                return artistList.ToList();
            }
        }

        public static async Task<IEnumerable<dynamic>> GetTotalArtistDiscsAsync()
        {
            var artistList = new List<dynamic>();
            var query = "SELECT a.ArtistId, a.FirstName, a.LastName, a.Name, SUM(r.Discs) AS Discs " +
                        "FROM Artist a " +
                        "JOIN Record r ON a.ArtistId = r.ArtistId " +
                        "GROUP BY a.ArtistId, a.FirstName, a.LastName, a.Name " +
                        "ORDER BY a.LastName, a.FirstName;";

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                artistList = (dynamic)await cn.QueryAsync<dynamic>(query);
                return artistList.ToList();
            }
        }

        public static async Task<IEnumerable<dynamic>> GetTotalArtistDiscsSPAsync()
        {
            var artistList = new List<dynamic>();

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                artistList = (dynamic)await cn.QueryAsync<dynamic>("GetTotalsForEachArtist", commandType: CommandType.StoredProcedure);
                return artistList.ToList();
            }
        }

        private static string LoadConnectionString()
        {
            return _ap.Instance.ConnectionString;
        }
    }
}
