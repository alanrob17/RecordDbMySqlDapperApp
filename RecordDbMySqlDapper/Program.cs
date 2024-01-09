using _at = RecordDbMySqlDapper.Tests.ArtistTest;
using _rt = RecordDbMySqlDapper.Tests.RecordTest;


namespace RecordDbMySqlDapper
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            #region Artist Methods
            // _at.GetAllArtists();
            // _at.GetAllArtistsSP();

            // _at.CreateArtist();

            // _at.CreateArtistSP();

            // _at.GetArtistByName("Traffic");
            // _at.GetArtistByNameSP("Bob Dylan");

            // _at.UpdateArtist(837);
            // _at.UpdateArtistSP(837);

            // _at.GetArtistById(114);
            // _at.GetArtistByIdSP(114);

            // _at.DeleteArtist(837);
            // _at.DeleteArtistSP(838);

            // _at.GetBiography(114);
            // _at.GetBiographySP(114);

            // _at.ArtistHtml(114);
            // _at.ArtistHtmlSP(114);

            // _at.GetArtistId("Bob", "Dylan");
            // _at.GetArtistIdSP("Bob", "Dylan");

            // _at.GetArtistsWithNoBio();
            // _at.GetArtistsWithNoBioSP();

            // _at.GetNoBiographyCount();
            // _at.GetNoBiographyCountSP();
            #endregion

            #region Record Methods
            // _rt.GetAllRecords();
            // _rt.GetAllRecordsSP();

            // _rt.CreateRecord(838);
            // _rt.CreateRecordSP(827);

            // _rt.GetRecordById(133);
            // _rt.GetRecordByIdSP(133);

            // _rt.UpdateRecord(5258);
            // _rt.UpdateRecordSP(5257);

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