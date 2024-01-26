using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _sd = DapperDAL.StatisticData;

namespace RecordDbMySqlDapper.Tests
{
    public class StatisticTest
    {
        internal static async Task PrintStatisticsAsync()
        {
            var statistics = await _sd.GetStatisticsAsync();

            // collect data
            var dtnow = DateTime.Now;
            var date = dtnow.ToLongDateString();
            await Console.Out.WriteLineAsync($"Record Database Statistics for {date}\n\n");
            var disks2022 = statistics.Disks2022.ToString(CultureInfo.InvariantCulture);
            await Console.Out.WriteLineAsync($"Number of Discs bought in 2022: {disks2022}");
            var cost2022 = statistics.Cost2022.ToString("C");
            await Console.Out.WriteLineAsync($"Total Amount spent on Discs in 2022: {cost2022}");
            var av2022 = statistics.Av2022.ToString("C");
            await Console.Out.WriteLineAsync($"Average cost of a Disc in 2022: {av2022}");
            var disks2021 = statistics.Disks2021.ToString(CultureInfo.InvariantCulture);
            await Console.Out.WriteLineAsync($"Number of Discs bought in 2021: {disks2021}");
            var cost2021 = statistics.Cost2021.ToString("C");
            await Console.Out.WriteLineAsync($"Total Amount spent on Discs in 2021: {cost2021}");
            var av2021 = statistics.Av2021.ToString("C");
            await Console.Out.WriteLineAsync($"Average cost of a Disc in 2021: {av2021}");
            var disks2020 = statistics.Disks2020.ToString(CultureInfo.InvariantCulture);
            await Console.Out.WriteLineAsync($"Number of Discs bought in 2020: {disks2020}");
            var cost2020 = statistics.Cost2020.ToString("C");
            await Console.Out.WriteLineAsync($"Total Amount spent on Discs in 2020: {cost2020}");
            var av2020 = statistics.Av2020.ToString("C");
            await Console.Out.WriteLineAsync($"Average cost of a Disc in 2020: {av2020}");
            var disks2019 = statistics.Disks2019.ToString(CultureInfo.InvariantCulture);
            await Console.Out.WriteLineAsync($"Number of Discs bought in 2019: {disks2019}");
            var cost2019 = statistics.Cost2019.ToString("C");
            await Console.Out.WriteLineAsync($"Total Amount spent on Discs in 2019: {cost2019}");
            var av2019 = statistics.Av2019.ToString("C");
            await Console.Out.WriteLineAsync($"Average cost of discs for 2019: {av2019}");
            var disks2018 = statistics.Disks2018.ToString(CultureInfo.InvariantCulture);
            await Console.Out.WriteLineAsync($"Number of Discs bought in 2018: {disks2018}");
            var cost2018 = statistics.Cost2018.ToString("C");
            await Console.Out.WriteLineAsync($"Total Amount spent on Discs in 2018: {cost2018}");
            var av2018 = statistics.Av2018.ToString("C");
            await Console.Out.WriteLineAsync($"Average cost of a Disc in 2018: {av2018}");
            var disks2017 = statistics.Disks2017.ToString(CultureInfo.InvariantCulture);
            await Console.Out.WriteLineAsync($"Number of Discs bought in 2017: {disks2017}");
            var cost2017 = statistics.Cost2017.ToString("C");
            await Console.Out.WriteLineAsync($"Total Amount spent on Discs in 2017: {cost2017}");
            var av2017 = statistics.Av2017.ToString("C");
            await Console.Out.WriteLineAsync($"Average cost of a Disc in 2017: {av2017}");
            var totalCDs = statistics.TotalCDs.ToString(CultureInfo.InvariantCulture);
            await Console.Out.WriteLineAsync($"Total number of CD's: {totalCDs}");
            var cDCost = statistics.CDCost.ToString("C");
            await Console.Out.WriteLineAsync($"Total Amount spent on CD's: {cDCost}");
            var avCDCost = statistics.AvCDCost.ToString("C");
            await Console.Out.WriteLineAsync($"Average spent on a CD: {avCDCost}");
            var totalRecords = statistics.TotalRecords.ToString(CultureInfo.InvariantCulture);
            await Console.Out.WriteLineAsync($"Total number of Records: {totalRecords}");
            var recordCost = statistics.RecordCost.ToString("C");
            await Console.Out.WriteLineAsync($"Amount spent on Records: {recordCost}");
            var totalCost = statistics.TotalCost.ToString("C");
            await Console.Out.WriteLineAsync($"Total amount spent: {totalCost}");
            var rockDisks = statistics.RockDisks.ToString(CultureInfo.InvariantCulture);
            await Console.Out.WriteLineAsync($"Number of Rock albums: {rockDisks}");
            var folkDisks = statistics.FolkDisks.ToString(CultureInfo.InvariantCulture);
            await Console.Out.WriteLineAsync($"Number of Folk albums: {folkDisks}");
            var acousticDisks = statistics.AcousticDisks.ToString(CultureInfo.InvariantCulture);
            await Console.Out.WriteLineAsync($"Number of Acoustic albums: {acousticDisks}");
            var jazzDisks = statistics.JazzDisks.ToString(CultureInfo.InvariantCulture);
            await Console.Out.WriteLineAsync($"Number of Jazz/Fusion albums: {jazzDisks}");
            var bluesDisks = statistics.BluesDisks.ToString(CultureInfo.InvariantCulture);
            await Console.Out.WriteLineAsync($"Number of Blues albums: {bluesDisks}");
            var countryDisks = statistics.CountryDisks.ToString(CultureInfo.InvariantCulture);
            await Console.Out.WriteLineAsync($"Number of Country albums: {countryDisks}");
            var classicalDisks = statistics.ClassicalDisks.ToString(CultureInfo.InvariantCulture);
            await Console.Out.WriteLineAsync($"Number of Classical albums: {classicalDisks}");
            var soundtrackDisks = statistics.SoundtrackDisks.ToString(CultureInfo.InvariantCulture);
            await Console.Out.WriteLineAsync($"Number of Soundtrack albums: {soundtrackDisks}");
            var fourStarDisks = statistics.FourStarDisks.ToString(CultureInfo.InvariantCulture);
            await Console.Out.WriteLineAsync($"Number of Indispensible albums: {fourStarDisks}");
            var threeStarDisks = statistics.ThreeStarDisks.ToString(CultureInfo.InvariantCulture);
            await Console.Out.WriteLineAsync($"Number of Slightly flawed albums: {threeStarDisks}");
            var twoStarDisks = statistics.TwoStarDisks.ToString(CultureInfo.InvariantCulture);
            await Console.Out.WriteLineAsync($"Number of Average albums: {twoStarDisks}");
            var oneStarDisks = statistics.OneStarDisks.ToString(CultureInfo.InvariantCulture);
            await Console.Out.WriteLineAsync($"Number of Mediocre albums: {oneStarDisks}");
        }
    }
}
