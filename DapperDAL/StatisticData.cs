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
                var disks2017 = await cn.ExecuteScalarAsync<int>("SELECT COALESCE(SUM(discs), 0) FROM Record WHERE bought > '2016-12-31' AND bought < '2018-01-01'");

                statistics.Disks2017 = disks2017;
            }

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                decimal cost2017 = await cn.ExecuteScalarAsync<decimal>("SELECT COALESCE(SUM(Cost), 0) FROM Record WHERE bought > '2016-12-31' AND bought < '2018-01-01'");

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

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                // query for Number of CD's bought in 2018              
                int disks2018 = await cn.ExecuteScalarAsync<int>("SELECT COALESCE(SUM(discs), 0) FROM Record WHERE bought > '2017-12-31' AND bought < '2019-01-01'");

                statistics.Disks2018 = disks2018;
            }

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                // query for amount spent on CD's in 2018                
                decimal cost2018 = await cn.ExecuteScalarAsync<decimal>("SELECT COALESCE(SUM(Cost), 0) FROM Record WHERE bought > '2017-12-31' and bought < '2019-01-01'");

                    statistics.Cost2018 = cost2018;

                // this is to stop a divide by zero error if nothing has been bought
                if (statistics.Cost2018 > 1)
                {
                    var av2018 = statistics.Cost2018 / (decimal)statistics.Disks2018;
                    statistics.Av2018 = av2018;
                }
                else
                {
                    statistics.Cost2018 = 0.00m;
                    statistics.Av2018 = 0.00m;
                }
            }

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                int disks2019 = await cn.ExecuteScalarAsync<int>("SELECT COALESCE(SUM(discs), 0) FROM Record WHERE bought > '2018-12-31' and bought < '2020-01-01'");

                statistics.Disks2019 = disks2019;
            }

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                decimal cost2019 = await cn.ExecuteScalarAsync<decimal>("SELECT COALESCE(SUM(Cost), 0) FROM Record WHERE bought > '2018-12-31' and bought < '2020-01-01'");

                statistics.Cost2019 = cost2019;

                // this is to stop a divide by zero error if nothing has been bought
                if (statistics.Cost2019 > 1)
                {
                    var av2019 = statistics.Cost2019 / (decimal)statistics.Disks2019;
                    statistics.Av2019 = av2019;
                }
                else
                {
                    statistics.Cost2018 = 0.00m;
                    statistics.Av2018 = 0.00m;
                }
            }

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                int disks2020 = await cn.ExecuteScalarAsync<int>("SELECT COALESCE(SUM(discs), 0) FROM Record WHERE bought > '2019-12-31' and bought < '2021-01-01'");

                statistics.Disks2020 = disks2020;
            }

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                decimal cost2020 = await cn.ExecuteScalarAsync<decimal>("SELECT COALESCE(SUM(Cost), 0) FROM Record WHERE bought > '2019-12-31' and bought < '2021-01-01'");

                statistics.Cost2020 = cost2020;

                // this is to stop a divide by zero error if nothing has been bought
                if (statistics.Cost2020 > 1)
                {
                    var av2020 = statistics.Cost2020 / (decimal)statistics.Disks2020;
                    statistics.Av2020 = av2020;
                }
                else
                {
                    statistics.Cost2020 = 0.00m;
                    statistics.Av2020 = 0.00m;
                }
            }

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                int disks2021 = await cn.ExecuteScalarAsync<int>("SELECT COALESCE(SUM(discs), 0) FROM Record WHERE bought > '2020-12-31' and bought < '2022-01-01'");

                statistics.Disks2021 = disks2021;
            }

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                decimal cost2021 = await cn.ExecuteScalarAsync<decimal>("SELECT COALESCE(SUM(Cost), 0) FROM Record WHERE bought > '2020-12-31' and bought < '2022-01-01'");

                statistics.Cost2021 = cost2021;

                // this is to stop a divide by zero error if nothing has been bought
                if (statistics.Cost2021 > 1)
                {
                    var av2021 = statistics.Cost2021 / (decimal)statistics.Disks2021;
                    statistics.Av2021 = av2021;
                }
                else
                {
                    statistics.Cost2021 = 0.00m;
                    statistics.Av2021 = 0.00m;
                }
            }

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                int disks2022 = await cn.ExecuteScalarAsync<int>("SELECT COALESCE(SUM(discs), 0) FROM Record WHERE bought > '2021-12-31' and bought < '2023-01-01'");

                statistics.Disks2022 = disks2022;
            }

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                decimal cost2022 = await cn.ExecuteScalarAsync<decimal>("SELECT COALESCE(SUM(Cost), 0) FROM Record WHERE bought > '2021-12-31' and bought < '2023-01-01'");

                statistics.Cost2022 = cost2022;

                // this is to stop a divide by zero error if nothing has been bought
                if (statistics.Cost2022 > 1)
                {
                    var av2022 = statistics.Cost2022 / (decimal)statistics.Disks2022;
                    statistics.Av2022 = av2022;
                }
                else
                {
                    statistics.Cost2022 = 0.00m;
                    statistics.Av2022 = 0.00m;
                }
            }

            using (IDbConnection cn = new MySqlConnection(LoadConnectionString()))
            {
                var totalRecords = await cn.ExecuteScalarAsync<int>("SELECT COALESCE(SUM(discs), 0) FROM Record WHERE media = 'R'");

                statistics.TotalRecords = totalRecords;
            }

            return statistics;
        }

        private static string LoadConnectionString()
        {
            return _ap.Instance.ConnectionString;
        }

        #endregion
    }
}
