namespace FYP.Controllers
{
    public class Constant
    {
        public const string AzureConnectionString = "DefaultEndpointsProtocol=https;AccountName=storedocstorage;AccountKey=wbGMdw5TonDvzSUwFCk7y8AP7l9LyhUgUxnR55M4ya4Hmc40KPNKipayLuC3Hasbb1sPlcyAuFccBynW1ERwag==;BlobEndpoint=https://storedocstorage.blob.core.windows.net/;TableEndpoint=https://storedocstorage.table.core.windows.net/;QueueEndpoint=https://storedocstorage.queue.core.windows.net/;FileEndpoint=https://storedocstorage.file.core.windows.net/";

        public string GeneratedLinkURL(string LinkId) {
            return "https://localhost:44326/Home/FileSubmissionViaLinkView?LinkId=" + LinkId;
        }


        public string GetMessage(string ActionName, string Sender, string Receiver) {
            switch (ActionName) {
                default:
                    return Sender + " has done something to " + Receiver + ".";
            }
        }
    }
}
