using _at = RecordDbMySqlDapper.Tests.ArtistTest;
using _rt = RecordDbMySqlDapper.Tests.RecordTest;
using _st = RecordDbMySqlDapper.Tests.StatisticTest;

namespace RecordDbMySqlDapper
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            #region Artist Methods
            // await _at.GetAllArtistsAsync();
            // await _at.GetAllArtistsSPAsync();

            // await _at.CreateArtistAsync();
            // await _at.CreateArtistSPAsync();

            // await _at.GetArtistByNameAsync("Traffic");
            // await _at.GetArtistByNameSPAsync("Bob Dylan");

            // await _at.UpdateArtistAsync(837);
            // await _at.UpdateArtistSPAsync(838);

            // await _at.GetArtistByIdAsync(114);
            // await _at.GetArtistByIdSPAsync(114);

            // await _at.DeleteArtistAsync(827);
            // await _at.DeleteArtistSPAsync(827);

            // await _at.GetBiographyAsync(114);
            // await _at.GetBiographySPAsync(114);

            // await _at.ArtistHtmlAsync(114);
            // await _at.ArtistHtmlSPAsync(114);

            // await _at.GetArtistIdAsync("Bob", "Dylan");
            // await _at.GetArtistIdSPAsync("Bob", "Dylan");

            // await _at.GetArtistsWithNoBioAsync();
            // await _at.GetArtistsWithNoBioSPAsync();

            // await _at.GetNoBiographyCountAsync();
            // await _at.GetNoBiographyCountSPAsync();

            #endregion

            #region Record Methods
            // await _rt.GetAllRecordsAsync();
            // await _rt.GetAllRecordsSPAsync();

            // await _rt.CreateRecordAsync(835);
            // await _rt.CreateRecordSPAsync(834);

            // await _rt.GetRecordByIdAsync(133);
            // await _rt.GetRecordByIdSPAsync(133);

            // await _rt.UpdateRecordAsync(5260);
            // await _rt.UpdateRecordSPAsync(5261);

            // await _rt.DeleteRecordAsync(5260);
            // await _rt.DeleteRecordSPAsync(5261);

            // await _rt.GetRecordByNameAsync("Cutting Edge");
            // await _rt.GetRecordByNameSPAsync("Cutting Edge");

            // await _rt.GetRecordsByArtistIdAsync(114);
            // await _rt.GetRecordsByArtistIdSPAsync(73);

            // await _rt.GetArtistRecordsMultipleTablesAsync();
            // await _rt.GetArtistRecordsMultipleTablesSPAsync();

            // await _rt.GetRecordsByArtistIdMultipleTablesAsync(114);
            // await _rt.GetRecordsByArtistIdMultipleTablesSPAsync(114);

            // await _rt.GetRecordsByYearAsync(1974);
            // await _rt.GetRecordsByYearSPAsync(1985);

            // await _rt.GetTotalNumberOfCDsAsync(); 
            // await _rt.GetTotalNumberOfCDsSPAsync(); 

            // await _rt.GetTotalNumberOfDiscsAsync(); 
            // await _rt.GetTotalNumberOfDiscsSPAsync(); 

            // await _rt.GetTotalNumberOfRecordsAsync(); 
            // await _rt.GetTotalNumberOfRecordsSPAsync(); 

            // await _rt.GetTotalNumberOfBluraysAsync();
            // await _rt.GetTotalNumberOfBluraysSPAsync();

            // await _rt.GetRecordListAsync();
            // await _rt.GetRecordListSPAsync();

            // await _rt.GetRecordListMultipleTablesAsync();
            // await _rt.GetRecordListMultipleTablesSPAsync();

            // await _rt.CountDiscsAsync("DVD");
            // await _rt.CountDiscsSPAsync(string.Empty);

            // await _rt.CountDiscsAsync("DVD");
            // await _rt.CountDiscsSPAsync("DVD");

            // await _rt.CountDiscsAsync("CD");
            // await _rt.CountDiscsSPAsync("CD");

            // await _rt.CountDiscsAsync("R");
            // await _rt.CountDiscsSPAsync("R");

            // await _rt.GetArtistRecordEntityAsync(2196);
            // await _rt.GetArtistRecordEntitySPAsync(2196);

            // await _rt.GetArtistNumberOfRecordsAsync(114);
            // await _rt.GetArtistNumberOfRecordsSPAsync(114);

            // await _rt.GetRecordDetailsAsync(2196);
            // await _rt.GetRecordDetailsSPAsync(2196);

            // await _rt.GetArtistNameFromRecordAsync(2196);
            // await _rt.GetArtistNameFromRecordSPAsync(2196);

            // await _rt.GetDiscCountForYearAsync(1974);
            // await _rt.GetDiscCountForYearSPAsync(1974);

            // await _rt.GetBoughtDiscCountForYearAsync("2000");
            // await _rt.GetBoughtDiscCountForYearSPAsync("2000");

            // await _rt.GetNoRecordReviewAsync();
            // await _rt.GetNoRecordReviewSPAsync();

            // await _rt.GetNoReviewCountAsync();
            // await _rt.GetNoReviewCountSPAsync();

            // await _rt.GetTotalArtistCostAsync();
            // await _rt.GetTotalArtistCostSPAsync();

            // await _rt.GetTotalArtistDiscsAsync();
            // await _rt.GetTotalArtistDiscsSPAsync();

            // await _rt.RecordHtmlAsync(2196);
            // await _rt.RecordHtmlSPAsync(2196);

            #endregion

            #region Statistic Methods

            await _st.PrintStatisticsAsync();

            #endregion
        }
    }
}