using Azure.Storage.Files.Shares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Controllers
{
    public class Storage
    {
        private CloudStorageAccount StorageAccount;

        public Storage() {
            this.StorageAccount = CloudStorageAccount.Parse(Constant.AzureConnectionString);
        }

        public void CreateNewFolder(string FolderName)
        {
            var folder = StorageAccount.CreateCloudFileClient().GetShareReference(FolderName.ToLower());
            folder.CreateIfNotExistsAsync().Wait();
        }

        public CloudFileShare GetFileShare(string UserId) {
            return StorageAccount.CreateCloudFileClient().GetShareReference(UserId);
        }

        public async Task<List<CloudFile>> GetFileList(string UserId) {
            CloudFileShare FileShare = GetFileShare(UserId);
            FileContinuationToken continuationToken = null;
            List<IListFileItem> results = new List<IListFileItem>();
            do
            {
                FileResultSegment response = await FileShare.GetRootDirectoryReference().ListFilesAndDirectoriesSegmentedAsync(continuationToken);
                results.AddRange(response.Results);
                continuationToken = response.ContinuationToken;

            } while (continuationToken != null);

            var FileList = results.ToList().Where(x => x.GetType() ==
            typeof(CloudFile)).Cast<CloudFile>().ToList();

            foreach (var item in FileList) {
                item.FetchAttributesAsync().Wait();
            }

            return FileList;
        }

        public CloudFile GetFile(string UserId, string FileName) {
            CloudFileShare FileShare = GetFileShare(UserId);
            CloudFile file = FileShare.GetRootDirectoryReference().GetFileReference(FileName);
            return file;
        }

        public Boolean UploadFile(string UserId, IFormFile file) {
            CloudFileShare FileShare = GetFileShare(UserId);

            CloudFile FileToUpload = FileShare.GetRootDirectoryReference().GetFileReference(file.FileName);
            Task result = FileToUpload.UploadFromStreamAsync(file.OpenReadStream());
            result.Wait();

            var tag = FileToUpload.Properties.ETag;

            return result.IsCompletedSuccessfully;
        }

        public Stream DownloadFile(string UserId, string FileName) {
            CloudFileShare FileShare = GetFileShare(UserId);
            Stream stream = null;

            CloudFile File = FileShare.GetRootDirectoryReference().GetFileReference(FileName);
            Task result = File.DownloadToStreamAsync(stream);
            if (result.IsCompletedSuccessfully)
            {
                return stream;
            }
            else {
                return null;
            }  
        }

        public Boolean DeleteFile(string UserId, string FileName) {
            CloudFileShare FileShare = GetFileShare(UserId);
            CloudFile File = FileShare.GetRootDirectoryReference().GetFileReference(FileName);
            var result = File.DeleteIfExistsAsync();
            result.Wait();

            return result.IsCompletedSuccessfully;
        }

        public CloudFile GetSpecificFile(string UserId, string FileName) {
            CloudFileShare FileShare = GetFileShare(UserId);
            CloudFile File = FileShare.GetRootDirectoryReference().GetFileReference(FileName);
            File.FetchAttributesAsync().Wait();

            return File;
        }
    }
}
