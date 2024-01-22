using _at = RecordDbMySqlDapper.Tests.ArtistTest;
using _rt = RecordDbMySqlDapper.Tests.RecordTest;


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

            _rt.UpdateRecord(5260);
            _rt.UpdateRecordSP(5261);

            // _rt.DeleteRecord(5258);
            // _rt.DeleteRecordSP(5259);

            // _rt.GetRecordByName("Cutting Edge");
            // _rt.GetRecordByNameSP("Cutting Edge");

            // _rt.GetRecordsByArtistId(114);
            // _rt.GetRecordsByArtistIdSP(114);

            // _rt.GetArtistRecordsMultipleTables();
            // _rt.GetArtistRecordsMultipleTablesSP();

            // _rt.GetRecordsByArtistIdMultipleTables(114);
            // _rt.GetRecordsByArtistIdMultipleTablesSP(114);

            // _rt.GetRecordsByYear(1974);
            // _rt.GetRecordsByYearSP(1974);

            // _rt.GetTotalNumberOfCDs(); 
            // _rt.GetTotalNumberOfCDsSP(); 

            // _rt.GetTotalNumberOfDiscs(); 
            // _rt.GetTotalNumberOfDiscsSP(); 

            // _rt.GetTotalNumberOfRecords(); 
            // _rt.GetTotalNumberOfRecordsSP(); 

            // _rt.GetTotalNumberOfBlurays();
            // _rt.GetTotalNumberOfBluraysSP();

            // _rt.GetRecordList();
            // _rt.GetRecordListSP();

            // _rt.GetRecordListMultipleTables();
            // _rt.GetRecordListMultipleTablesSP();

            // _rt.CountDiscs("DVD");

            // _rt.CountDiscsSP(string.Empty);

            // _rt.CountDiscs("DVD");
            // _rt.CountDiscsSP("DVD");

            // _rt.CountDiscs("CD");

            // _rt.CountDiscsSP("CD");

            // _rt.CountDiscs("R");

            // _rt.CountDiscsSP("R");

            // _rt.GetArtistRecordEntity(2196);
            // _rt.GetArtistRecordEntitySP(2196);

            // _rt.GetArtistNumberOfRecords(114);
            // _rt.GetArtistNumberOfRecordsSP(114);

            // _rt.GetRecordDetails(2196);
            // _rt.GetRecordDetailsSP(2196);

            // _rt.GetArtistNameFromRecord(2196);
            // _rt.GetArtistNameFromRecordSP(2196);

            // _rt.GetDiscCountForYear(1974);
            // _rt.GetDiscCountForYearSP(1974);

            // _rt.GetBoughtDiscCountForYear("2000");
            // _rt.GetBoughtDiscCountForYearSP("2000");

            // _rt.GetNoRecordReview();
            // _rt.GetNoRecordReviewSP();

            // _rt.GetNoReviewCount();
            // _rt.GetNoReviewCountSP();

            // _rt.GetTotalArtistCost();
            // _rt.GetTotalArtistCostSP();

            // _rt.GetTotalArtistDiscs();
            // _rt.GetTotalArtistDiscsSP();

            // _rt.RecordHtml(2196);
            // _rt.RecordHtmlSP(2196);
            #endregion
        }
    }
}