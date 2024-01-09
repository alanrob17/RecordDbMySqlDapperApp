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

namespace DapperDAL
{
    public class RecordDataAccess
    {
        public static List<RecordModel> GetRecords()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return cn.Query<RecordModel>("SELECT * FROM Record ORDER BY Recorded DESC", new DynamicParameters()).ToList();
            }
        }

        public static List<dynamic> GetRecordsSP()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return cn.Query<dynamic>("GetFullRecords", commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public static List<RecordModel> GetRecordsByArtistId(int artistId)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return cn.Query<RecordModel>($"SELECT * FROM Record WHERE ArtistId = {artistId} ORDER BY Recorded DESC").ToList();
            }
        }

        public static List<RecordModel> GetRecordsByArtistIdSP(int artistId)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var parameter = new DynamicParameters();
                parameter.Add("_artistId", artistId);

                return cn.Query<RecordModel>($"GetRecordsByArtistId", parameter, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public static ArtistModel GetRecordsByArtistIdMultipleTables(int artistId)
        {
            string query = "select * from Artist where artistId = @artistId ; select * from Record where artistId = @artistId order by Recorded;";

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var multipleResult = cn.QueryMultiple(query, new { artistId = artistId });

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

        public static ArtistModel GetRecordsByArtistIdMultipleTablesSP(int artistId)
        {

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var parameter = new DynamicParameters();
                parameter.Add("_artistId", artistId);

                var artist = cn.Query<ArtistModel>("GetArtistById", parameter, commandType: CommandType.StoredProcedure).SingleOrDefault();
                var records = cn.Query<RecordModel>("GetRecordsByArtistId", parameter, commandType: CommandType.StoredProcedure).ToList();

                if (artist is ArtistModel)
                {
                    artist.Records = records;
                    return (ArtistModel)artist;
                }
                    
                return new ArtistModel { ArtistId = 0 };
            }
        }

        public static List<ArtistModel> GetArtistRecordsMultipleTables()
        {
            List<ArtistModel> artists = new();

            string query = "select * from Artist order by LastName, FirstName; select * from Record order by ArtistId, Recorded;";

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var multipleResult = cn.QueryMultiple(query);

                artists = multipleResult.Read<ArtistModel>().ToList();
                var records = multipleResult.Read<RecordModel>().ToList();

                foreach (var artist in artists)
                {
                    artist.Records = records.Where(r => r.ArtistId == artist.ArtistId).ToList();
                }
            }

            return artists;
        }

        public static List<ArtistModel> GetArtistRecordsMultipleTablesSP()
        {
            List<ArtistModel> artists = new();


            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                artists = cn.Query<ArtistModel>("GetAllArtists", commandType: CommandType.StoredProcedure).ToList();
                var records = cn.Query<RecordModel>("GetFullRecords", commandType: CommandType.StoredProcedure).ToList();

                foreach (var artist in artists)
                {
                    artist.Records = records.Where(r => r.ArtistId == artist.ArtistId).ToList();
                }
            }

            return artists;
        }

        public static List<RecordModel> GetRecordsByYear(int year)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return cn.Query<RecordModel>($"SELECT * FROM Record WHERE Recorded = {year} ORDER BY ArtistId").ToList();
            }
        }

        public static List<dynamic> GetRecordsByYearSP(int year)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var parameter = new DynamicParameters();
                parameter.Add("_year", year);

                return cn.Query<dynamic>($"GetRecordsByYear", parameter, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public static RecordModel GetRecordById(int recordId)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return cn.Query<RecordModel>($"SELECT * FROM Record WHERE RecordId = {recordId}").FirstOrDefault() ?? new RecordModel { RecordId = 0 };
            }
        }

        public static RecordModel GetRecordByIdSP(int recordId)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var parameter = new DynamicParameters();
                parameter.Add("_recordId", recordId);

                return cn.Query<RecordModel>($"GetRecordById", parameter, commandType: CommandType.StoredProcedure).FirstOrDefault() ?? new RecordModel { RecordId = 0 };
            }
        }

        public static RecordModel GetRecordByName(RecordModel record)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return cn.Query<RecordModel>($"SELECT * FROM Record WHERE Name LIKE @Name", record).FirstOrDefault() ?? new RecordModel { RecordId = 0 };
            }
        }

        public static RecordModel GetRecordByNameSP(RecordModel record)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var parameter = new DynamicParameters();
                parameter.Add("_name", record.Name);

                return cn.Query<RecordModel>($"GetRecordByName", parameter, commandType: CommandType.StoredProcedure).FirstOrDefault() ?? new RecordModel { RecordId = 0 };
            }
        }

        public static int UpdateRecord(RecordModel record)
        {
            var result = 0;
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                result = cn.Execute("UPDATE Record SET Name = @Name, Field = @Field, Recorded = @Recorded, Label = @Label, Pressing = @Pressing, Rating = @Rating, Discs = @Discs, Media = @Media, Bought = @Bought, Cost = @Cost, Review = @Review WHERE RecordId = @RecordId", record);
            }

            return result;
        }

        public static int UpdateRecordSP(RecordModel record)
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

                cn.Execute("UpdateRecord", parameter, commandType: CommandType.StoredProcedure);

                result = parameter.Get<int>("_recordId");
            }
            return result;
        }

        public static int AddRecord(RecordModel record)
        {
            var result = 0;
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var query = @"INSERT INTO Record (ArtistId, Name, Field, Recorded, Label, Pressing, Rating, Discs, Media, Bought, Cost, Review) VALUES (@ArtistId, @Name, @Field, @Recorded, @Label, @Pressing, @Rating, @Discs, @Media, @Bought, @Cost, @Review);";
                result = cn.Execute(query, record);
            }

            return result;
        }

        public static object AddRecordSP(RecordModel record)
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

                cn.Execute("CreateRecord", parameter, commandType: CommandType.StoredProcedure);

                recordId = parameter.Get<int>("_recordId");
            }
            return recordId;
        }

        public static int DeleteRecord(int recordId)
        {
            var result = 0;
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                result = cn.Execute($"DELETE FROM Record WHERE RecordId={recordId}");
            }

            return result;
        }

        public static int DeleteRecordSP(int recordId)
        {
            var result = 0;
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var parameters = new DynamicParameters();
                parameters.Add("_recordId", recordId);

                result = (int)cn.Execute("DeleteRecordById", parameters, commandType: CommandType.StoredProcedure);
            }
            return result;
        }

        public static int GetTotalNumberOfCDs()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return cn.ExecuteScalar<int>("SELECT SUM(Discs) FROM Record WHERE Media = 'CD' OR Media = 'CD/DVD'");
            }
        }

        public static int GetTotalNumberOfCDsSP()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return cn.ExecuteScalar<int>("GetTotalCDCount", commandType: CommandType.StoredProcedure);
            }
        }

        public static int GetTotalNumberOfDiscs()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return cn.ExecuteScalar<int>("SELECT SUM(Discs) FROM Record");
            }
        }

        public static int GetTotalNumberOfDiscsSP()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return cn.ExecuteScalar<int>("GetTotalNumberOfAllRecords", commandType: CommandType.StoredProcedure);
            }
        }

        public static int GetTotalNumberOfRecords()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return cn.ExecuteScalar<int>("SELECT SUM(Discs) FROM Record WHERE Media = 'R'");
            }
        }

        public static int GetTotalNumberOfRecordsSP()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return cn.ExecuteScalar<int>("GetTotalNumberOfRecords", commandType: CommandType.StoredProcedure);
            }
        }

        public static object GetTotalNumberOfBlurays()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return cn.ExecuteScalar<int>("SELECT SUM(Discs) FROM Record WHERE Media = 'CD/Blu-ray' OR Media = 'Blu-ray'");
            }
        }

        public static object GetTotalNumberOfBluraysSP()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return cn.ExecuteScalar<int>("GetTotalNumberOfAllBlurays", commandType: CommandType.StoredProcedure);
            }
        }

        public static List<dynamic> GetArtistRecordList()
        {
            string query = "SELECT a.ArtistId, a.FirstName, a.LastName, a.Name AS Artist, r.RecordId, r.Name, r.Recorded, r.Media, r.Rating " +
                           "FROM Artist a " +
                           "JOIN Record r ON a.ArtistId = r.ArtistId " +
                           "ORDER BY a.LastName, a.FirstName, r.Recorded";

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return cn.Query<dynamic>(query).ToList();
            }
        }

        public static List<dynamic> GetArtistRecordListSP()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return cn.Query<dynamic>("GetAllArtistsAndRecords", commandType: CommandType.StoredProcedure).ToList();
            }
        }

        /// <summary>
        /// Count the number of discs.
        /// </summary>
        public static int CountAllDiscs(string media = "")
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
                return cn.ExecuteScalar<int>($"SELECT SUM(Discs) FROM Record WHERE Media = {mediaType}");
            }
        }

        /// <summary>
        /// Count the number of discs.
        /// </summary>
        public static int CountAllDiscsSP(string media = "")
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

                return cn.ExecuteScalar<int>("GetMediaCountByType", parameter, commandType: CommandType.StoredProcedure);
            }
        }

        public static dynamic? GetArtistRecordEntity(int recordId)
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

                var result = cn.QueryFirstOrDefault<dynamic>(query, new { recordId });

                if (result == null)
                {
                    result = new ExpandoObject();
                    result.RecordId = 0;
                }

                return result;
            }
        }

        public static dynamic? GetArtistRecordEntitySP(int recordId)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var parameter = new DynamicParameters();
                parameter.Add("_recordId", recordId);

                var result = cn.QueryFirstOrDefault<dynamic>("GetArtistRecordByRecordId", parameter, commandType: CommandType.StoredProcedure);

                if (result == null)
                {
                    result = new ExpandoObject();
                    result.RecordId = 0;
                }

                return result;
            }
        }

        /// <summary>
        /// Get number of records for an artist.
        /// </summary>
        public static int GetArtistNumberOfRecords(int artistId)
        {
            var discs = 0;

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return cn.ExecuteScalar<int>("SELECT SUM(Discs) FROM Record WHERE artistId = @artistId;", new { artistId });
            }
        }

        /// <summary>
        /// Get number of records for an artist.
        /// </summary>
        public static int GetArtistNumberOfRecordsSP(int artistId)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var parameter = new DynamicParameters();
                parameter.Add("_artistId", artistId);

                var result = cn.QueryFirstOrDefault<int>("GetArtistNumberOfRecords", parameter, commandType: CommandType.StoredProcedure);

                return result;
            }
        }

        /// <summary>
        /// Get record details from ToString method.
        /// </summary>
        public static RecordModel GetFormattedRecord(int recordId)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return cn.Query<RecordModel>($"SELECT * FROM Record WHERE RecordId = {recordId}").FirstOrDefault() ?? new RecordModel { RecordId = 0 };
            }
        }

        /// <summary>
        /// Get record details from ToString method.
        /// </summary>
        public static RecordModel GetFormattedRecordSP(int recordId)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var parameter = new DynamicParameters();
                parameter.Add("_recordId", recordId);

                return cn.QueryFirstOrDefault<RecordModel>("GetSingleRecord", parameter, commandType: CommandType.StoredProcedure);
            }
        }

        public static string GetArtistNameFromRecord(int recordId)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var query = "SELECT a.Name FROM Artist a INNER JOIN Record r ON a.ArtistId = r.ArtistId WHERE r.RecordId = @recordId";
                var parameters = new { recordId };

                return cn.QuerySingleOrDefault<string>(query, parameters);
            }
        }

        public static string GetArtistNameFromRecordSP(int recordId)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var _recordId = recordId;
                var parameter = new { _recordId };

                return cn.QuerySingleOrDefault<string>("GetArtistNameByRecordId", parameter, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Get the number of discs for a particular year.
        /// </summary>
        public static int GetDiscCountForYear(int year)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return cn.Query<int>($"SELECT SUM(Discs) FROM Record WHERE Recorded = {year};").FirstOrDefault();
            }
        }

        /// <summary>
        /// Get the number of discs for a particular year.
        /// </summary>
        public static int GetDiscCountForYearSP(int year)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var _year = year;   
                var parameter = new { _year };
                return cn.ExecuteScalar<int>($"GetRecordedYearNumber", parameter, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Get the number of discs that I bought for a particular year.
        /// </summary>
        public static int GetBoughtDiscCountForYear(string year)
        {
            var discCount = 0;

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                // get year from bought date
                return cn.Query<int>($"SELECT SUM(Discs) FROM Record WHERE Bought like '%{year}%'").FirstOrDefault();
            }
        }

        /// <summary>
        /// Get the number of discs that I bought for a particular year.
        /// </summary>
        public static int GetBoughtDiscCountForYearSP(string year)
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var _year = year;
                var parameter = new { _year };

                // get year from bought date
                return cn.ExecuteScalar<int>("GetBoughtDiscCountForYear", parameter, commandType: CommandType.StoredProcedure);
            }
        }

        public static List<dynamic> MissingRecordReviews()
        {
            var query = "SELECT a.ArtistId, A.Name AS Artist, r.RecordId, r.Name, r.Recorded, r.Discs, r.Rating, r.Media " +
                "FROM Artist a " +
                "INNER JOIN Record r ON a.ArtistId = r.ArtistId " +
                "WHERE r.Review IS NULL " +
                "ORDER BY a.LastName, a.FirstName;";

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return cn.Query<dynamic>(query).ToList();
            }
        }

        public static List<dynamic> MissingRecordReviewsSP()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return cn.Query<dynamic>("MissingRecordReview", commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public static int GetNoReviewCount()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return cn.Query<int>($"SELECT * FROM Record WHERE Review IS NULL;").Count();
            }
        }

        public static int GetNoReviewCountSP()
        {
            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return cn.ExecuteScalar<int>($"GetNoRecordReviewCount", commandType: CommandType.StoredProcedure);
            }
        }

        public static List<dynamic> GetCostTotals()
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
                return artistList = cn.Query<dynamic>(query).ToList();
            }
        }

        public static List<dynamic> GetCostTotalsSP()
        {
            var artistList = new List<dynamic>();

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return artistList = cn.Query<dynamic>("GetTotalsForEachArtist", commandType: CommandType.StoredProcedure).ToList();
            }
        }

        /// <summary>
        /// Get total number of discs for each artist.
        /// </summary>
        public static IEnumerable<dynamic> GetTotalArtistDiscs()
        {
            var artistList = new List<dynamic>();
            var query = "SELECT a.ArtistId, a.FirstName, a.LastName, a.Name, SUM(r.Discs) AS Discs " +
                        "FROM Artist a " +
                        "JOIN Record r ON a.ArtistId = r.ArtistId " +
                        "GROUP BY a.ArtistId, a.FirstName, a.LastName, a.Name " +
                        "ORDER BY a.LastName, a.FirstName;";

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return artistList = cn.Query<dynamic>(query).ToList();
            }
        }

        /// <summary>
        /// Get total number of discs for each artist.
        /// </summary>
        public static IEnumerable<dynamic> GetTotalArtistDiscsSP()
        {
            var artistList = new List<dynamic>();

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                return artistList = cn.Query<dynamic>("GetTotalsForEachArtist", commandType: CommandType.StoredProcedure).ToList();
            }
        }

        private static string LoadConnectionString(string id = "RecordDB")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
