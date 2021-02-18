namespace FYP.Controllers
{
    public class Constant
    {
        public const string AzureConnectionString = "DefaultEndpointsProtocol=https;AccountName=storedocstorage;AccountKey=wbGMdw5TonDvzSUwFCk7y8AP7l9LyhUgUxnR55M4ya4Hmc40KPNKipayLuC3Hasbb1sPlcyAuFccBynW1ERwag==;BlobEndpoint=https://storedocstorage.blob.core.windows.net/;TableEndpoint=https://storedocstorage.table.core.windows.net/;QueueEndpoint=https://storedocstorage.queue.core.windows.net/;FileEndpoint=https://storedocstorage.file.core.windows.net/";

        public string GeneratedLinkURL(string LinkId) {
            return "https://localhost:44326/Home/FileSubmissionViaLinkView?LinkId=" + LinkId;
        }


        public string AdminGetMessage(string ActionName, string PrimaryName, string SecondaryName, string FileName) {

            switch (ActionName) {
                case "CREATE_USER":
                    return "You have create a new user {" + SecondaryName + "}.";
                case "DELETE_USER":
                    return "You have delete an user {" + PrimaryName + "}.";
                case "ADD_FILE":
                    return "You have added a new file {" + FileName + "}.";
                case "DELETE_FILE":
                    return "You have deleted a file {" + FileName + "}.";
                case "ACCESS_REQUEST":
                    return PrimaryName + " has requested you to share file access of {" + FileName + "} to " + SecondaryName + ".";
                case "APPROVED":
                    return "You have approved the request of " + SecondaryName + " to share the access of file {" + FileName + "}.";
                case "REJECTED":
                    return "You have rejected the request of " + SecondaryName + " to share the access of file {" + FileName + "}.";
                case "SHARE":
                    return "You have shared access of file {" + FileName + "} to " + PrimaryName + ".";
                case "UPLOAD":
                    return "You have uploaded a file {" + FileName + "}.";
                case "LINK_UPLOAD":
                    return "A client has uploaded a file {" + FileName + "} to " + PrimaryName + ".";
                default:
                    return "";
            }
        }

        public string UserGetMessage(string ActionName, string PrimaryName, string SecondaryName, string FileName)
        {
            switch (ActionName)
            {
                case "SIGNATURE":
                    return SecondaryName + " has signed a document (" + FileName + ") requested by you.";
                case "REQUEST":
                    return "You have requested access of file {" + FileName + "} to " + SecondaryName + " from admin.";
                case "APPROVED":
                    return "Admin has approved access of file {" + FileName + "} as per request from " + PrimaryName + " to " + SecondaryName + ".";
                case "REJECTED":
                    return "Admin has rejected access of file {" + FileName + "} as per request from " + PrimaryName + " to " + SecondaryName + ".";
                case "SHARE":
                    return "Admin has shared access of file {" + FileName + "} to you.";
                case "LINK_UPLOAD":
                    return "A client has uploaded a file {" + FileName + "} to your account.";
                default:
                    return "";
            }
        }

      
    }
}
