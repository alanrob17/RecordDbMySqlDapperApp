using Dapper;
using DapperDAL.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _ap = DapperDAL.Components.AppSettings;

namespace DapperDAL
{
    public class StatisticData
    {
        #region " Methods "

        public static async Task<StatisticModel> GetStatisticsAsync()
        {
            StatisticModel statistics = new();

            using (var cn = new MySqlConnection(LoadConnectionString()))
            {
                var totalCds = await cn.ExecuteScalarAsync<int>("SELECT COALESCE(SUM(Discs), 0) FROM Record WHERE Media = 'CD'");

                statistics.TotalCDs = totalCds;
            }

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var rockDisks = await cn.ExecuteScalarAsync<int>("SELECT COALESCE(SUM(Discs), 0) FROM Record WHERE field = 'Rock'");

                statistics.RockDisks = rockDisks;
            }

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var folkDisks = await cn.ExecuteScalarAsync<int>("SELECT COALESCE(SUM(Discs), 0) FROM Record WHERE field = 'Folk'");

                statistics.FolkDisks = folkDisks;
            }

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var acousticDisks = await cn.ExecuteScalarAsync<int>("SELECT COALESCE(SUM(Discs), 0) FROM Record WHERE field = 'Acoustic'");

                statistics.AcousticDisks = acousticDisks;
            }

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var jazzDisks = await cn.ExecuteScalarAsync<int>("SELECT COALESCE(SUM(Discs), 0) FROM Record WHERE field = 'Jazz' OR field = 'Fusion'");

                statistics.JazzDisks = jazzDisks;
            }

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var bluesDisks = await cn.ExecuteScalarAsync<int>("SELECT COALESCE(SUM(Discs), 0) FROM Record WHERE field = 'Blues'");

                statistics.BluesDisks = bluesDisks;
            }

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var countryDisks = await cn.ExecuteScalarAsync<int>("SELECT COALESCE(SUM(Discs), 0) FROM Record WHERE field = 'Country'");

                statistics.CountryDisks = countryDisks;
            }

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var classicalDisks = await cn.ExecuteScalarAsync<int>("SELECT COALESCE(SUM(Discs), 0) FROM Record WHERE field = 'Classical'");

                statistics.ClassicalDisks = classicalDisks;
            }

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var soundtrackDisks = await cn.ExecuteScalarAsync<int>("SELECT COALESCE(SUM(Discs), 0) FROM Record WHERE field = 'Soundtrack'");

                statistics.SoundtrackDisks = soundtrackDisks;
            }

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var fourStarDisks = await cn.ExecuteScalarAsync<int>("SELECT COALESCE(COUNT(Rating), 0) FROM Record WHERE Rating = '****'");

                statistics.FourStarDisks = fourStarDisks;
            }

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var threeStarDisks = await cn.ExecuteScalarAsync<int>("SELECT COALESCE(COUNT(Rating), 0) FROM Record WHERE Rating = '***'");

                statistics.ThreeStarDisks = threeStarDisks;
            }

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var twoStarDisks = await cn.ExecuteScalarAsync<int>("SELECT COALESCE(COUNT(Rating), 0) FROM Record WHERE Rating = '**'");

                statistics.TwoStarDisks = twoStarDisks;
            }

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var oneStarDisks = await cn.ExecuteScalarAsync<int>("SELECT COALESCE(COUNT(Rating), 0) FROM Record WHERE Rating = '*'");

                statistics.OneStarDisks = oneStarDisks;
            }

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                decimal recordCost = await cn.ExecuteScalarAsync<decimal>("SELECT COALESCE(SUM(Cost), 0) FROM Record WHERE media = 'R'");

                statistics.RecordCost = recordCost;
            }

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                decimal cdCost = await cn.ExecuteScalarAsync<decimal>("SELECT COALESCE(SUM(Cost), 0) FROM Record WHERE media = 'CD'");

                statistics.CDCost = cdCost;
            }

            decimal avCdCost = statistics.CDCost / (decimal)statistics.TotalCDs;
            statistics.AvCDCost = avCdCost;

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                decimal totalCost = await cn.ExecuteScalarAsync<decimal>("SELECT COALESCE(SUM(Cost), 0) FROM Record");

                statistics.TotalCost = totalCost;
            }

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var disks2017 = await cn.ExecuteScalarAsync<int>("SELECT COALESCE(SUM(disc), 0) FROM Record WHERE bought > '31-Dec-2016' AND bought < '01-Jan-2018'");

                statistics.Disks2017 = disks2017;
            }

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                decimal cost2017 = await cn.ExecuteScalarAsync<decimal>("SELECT COALESCE(SUM(Cost), 0) FROM Record WHERE bought > '31-Dec-2016' AND bought < '01-Jan-2018'");

                // this is to stop a divide by zero error if nothing has been bought
                if (cost2017 > 1)
                {
                    statistics.Cost2017 = cost2017;
                    statistics.Av2017 = cost2017 / (decimal)statistics.Disks2017;
                }
                else
                {
                    statistics.Cost2017 = 0.00m;
                    statistics.Av2017 = 0.00m;
                }
            }

        //    int? disks2018 = null;
        //    using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
        //    {
        //        // query for Number of CD's bought in 2018              
        //        var getValue = new SqlCommand("select sum(discs) from record where bought > '31-Dec-2017' and bought < '01-Jan-2019'", cn);

        //        await cn.OpenAsync();

        //        var result = await getValue.ExecuteScalarAsync();
        //        if (result != DBNull.Value)
        //        {
        //            disks2018 = (int?)result;
        //        }

        //        cn.Close();
        //        statistics.Disks2018 = disks2018 ?? 0;
        //    }

        //    var cost2018 = 0.0m;
        //    using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
        //    {
        //        // query for amount spent on CD's in 2018                
        //        var getValue = new SqlCommand("select sum(cost) from record where bought > '31-Dec-2017' and bought < '01-Jan-2019'", cn);

        //        await cn.OpenAsync();

        //        var result = await getValue.ExecuteScalarAsync();
        //        if (result != DBNull.Value)
        //        {
        //            cost2018 = (decimal)result;
        //        }

        //        // this is to stop a divide by zero error if nothing has been bought
        //        if (cost2018 > 1)
        //        {
        //            statistics.Cost2018 = cost2018;
        //            var av2018 = cost2018 / (decimal)disks2018;
        //            statistics.Av2018 = av2018;
        //        }
        //        else
        //        {
        //            statistics.Cost2018 = 0.00m;
        //            statistics.Av2018 = 0.00m;
        //        }

        //        cn.Close();
        //        statistics.Cost2018 = cost2018;
        //    }

        //    int? disks2019 = null;
        //    using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
        //    {
        //        // query for Number of CD's bought in 2019
        //        var getValue = new SqlCommand("select sum(discs) from record where bought > '31-Dec-2018' and bought < '01-Jan-2020'", cn);

        //        await cn.OpenAsync();

        //        var result = await getValue.ExecuteScalarAsync();
        //        if (result != DBNull.Value)
        //        {
        //            disks2019 = (int?)result;
        //        }

        //        cn.Close();
        //        statistics.Disks2019 = disks2019 ?? 0;
        //    }

        //    using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
        //    {
        //        // query for amount spent on CD's in 2019
        //        var cost2019 = 0.0m;
        //        var getValue = new SqlCommand("select sum(cost) from record where bought > '31-Dec-2018' and bought < '01-Jan-2020'", cn);

        //        await cn.OpenAsync();

        //        var result = await getValue.ExecuteScalarAsync();
        //        if (result != DBNull.Value)
        //        {
        //            cost2019 = (decimal)result;
        //        }

        //        // this is to stop a divide by zero error if nothing has been bought
        //        if (cost2019 > 1)
        //        {
        //            statistics.Cost2019 = cost2019;
        //            var av2019 = cost2019 / (decimal)disks2019;
        //            statistics.Av2019 = av2019;
        //        }
        //        else
        //        {
        //            statistics.Cost2019 = 0.00m;
        //            statistics.Av2019 = 0.00m;
        //        }

        //        cn.Close();
        //    }

        //    int? disks2020 = null;
        //    using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
        //    {
        //        // query for Number of CD's bought in 2020
        //        var getValue = new SqlCommand("select sum(discs) from record where bought > '31-Dec-2019' and bought < '01-Jan-2021'", cn);

        //        await cn.OpenAsync();

        //        var result = await getValue.ExecuteScalarAsync();
        //        if (result != DBNull.Value)
        //        {
        //            disks2020 = (int?)result;
        //        }

        //        cn.Close();
        //        statistics.Disks2020 = disks2020 ?? 0;
        //    }

        //    var cost2020 = 0.0m;
        //    using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
        //    {
        //        // query for amount spent on CD's in 2020
        //        var getValue = new SqlCommand("select sum(cost) from record where bought > '31-Dec-2019' and bought < '01-Jan-2021'", cn);

        //        await cn.OpenAsync();

        //        var result = await getValue.ExecuteScalarAsync();
        //        if (result != DBNull.Value)
        //        {
        //            cost2020 = (decimal)result;
        //        }

        //        var av2020 = cost2020 / (decimal)disks2020;
        //        statistics.Av2020 = av2020;

        //        cn.Close();
        //        statistics.Cost2020 = cost2020;
        //    }

        //    int? disks2021 = null;
        //    using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
        //    {
        //        // query for Number of CD's bought in 2021
        //        var getValue = new SqlCommand("select sum(discs) from record where bought > '31-Dec-2020' and bought < '01-Jan-2022'", cn);

        //        await cn.OpenAsync();

        //        var result = await getValue.ExecuteScalarAsync();
        //        if (result != DBNull.Value)
        //        {
        //            disks2021 = (int?)result;
        //        }

        //        cn.Close();
        //        statistics.Disks2021 = disks2021 ?? 0;
        //    }

        //    using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
        //    {
        //        // query for amount spent on CD's in 2021
        //        var cost2021 = 0.0m;
        //        var getValue = new SqlCommand("select sum(cost) from record where bought > '31-Dec-2020' and bought < '01-Jan-2022'", cn);

        //        await cn.OpenAsync();

        //        var result = await getValue.ExecuteScalarAsync();
        //        if (result != DBNull.Value)
        //        {
        //            cost2021 = (decimal)result;
        //        }

        //        // this is to stop a divide by zero error if nothing has been bought
        //        if (cost2021 > 1)
        //        {
        //            statistics.Cost2021 = cost2021;
        //            var av2021 = cost2021 / (decimal)disks2021;
        //            statistics.Av2021 = av2021;
        //        }
        //        else
        //        {
        //            statistics.Cost2021 = 0.00m;
        //            statistics.Av2021 = 0.00m;
        //        }

        //        cn.Close();
        //    }

        //    using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
        //    {
        //        // query for Number of records
        //        int? totalRecords = null;
        //        var getValue = new SqlCommand("select sum(discs) from record where media='R'", cn);

        //        await cn.OpenAsync();

        //        var result = await getValue.ExecuteScalarAsync();
        //        if (result != DBNull.Value)
        //        {
        //            totalRecords = (int?)result;
        //        }

        //        cn.Close();
        //        statistics.TotalRecords = totalRecords ?? 0;
        //    }

        //    int? disks2022 = null;
        //    using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
        //    {
        //        // query for Number of CD's bought in 2022
        //        var getValue = new SqlCommand("select sum(discs) from record where bought > '31-Dec-2021' and bought < '01-Jan-2023'", cn);

        //        await cn.OpenAsync();

        //        var result = await getValue.ExecuteScalarAsync();
        //        if (result != DBNull.Value)
        //        {
        //            disks2022 = (int?)result;
        //        }

        //        cn.Close();
        //        statistics.Disks2022 = disks2022 ?? 0;
        //    }

        //    using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
        //    {
        //        // query for amount spent on CD's in 2022
        //        var cost2022 = 0.0m;
        //        var getValue = new SqlCommand("select sum(cost) from record where bought > '31-Dec-2021' and bought < '01-Jan-2023'", cn);

        //        await cn.OpenAsync();

        //        var result = await getValue.ExecuteScalarAsync();
        //        if (result != DBNull.Value)
        //        {
        //            cost2022 = (decimal)result;
        //        }

        //        // this is to stop a divide by zero error if nothing has been bought
        //        if (cost2022 > 1)
        //        {
        //            statistics.Cost2022 = cost2022;
        //            var av2022 = cost2022 / (decimal)disks2022;
        //            statistics.Av2022 = av2022;
        //        }
        //        else
        //        {
        //            statistics.Cost2022 = 0.00m;
        //            statistics.Av2022 = 0.00m;
        //        }

        //        cn.Close();
        //    }

            return statistics;
        }

        private static string LoadConnectionString()
        {
            return _ap.Instance.ConnectionString;
        }
        #endregion
    }
}
