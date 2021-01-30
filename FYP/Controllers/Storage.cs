using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Controllers
{
    public class Storage
    {
        public CloudStorageAccount StorageAccount;

        public Storage() {
            this.StorageAccount = CloudStorageAccount.Parse(Constant.AzureConnectionString);
        }

        public async void CreateNewFolder(string FolderName) {
            var folder = StorageAccount.CreateCloudFileClient().GetShareReference(FolderName.ToLower());
            folder.CreateIfNotExistsAsync().Wait();
        }

        public string ReplaceSpace(string input) {
            return input.Replace(" ", "-");
        }

        public string ReplaceDash(string input) {
            return input.Replace("-", " ");
        }

    }
}
