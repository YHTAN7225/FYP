using Azure.Storage.Files.Shares;
using FYP.Models;
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
        private readonly CloudStorageAccount StorageAccount;

        public Storage() {
            this.StorageAccount = CloudStorageAccount.Parse(Constant.AzureConnectionString);
        }

        public void CreateNewFolder(string FolderName)
        {
            var folder = StorageAccount.CreateCloudFileClient().GetShareReference(FolderName.ToLower());
            folder.CreateIfNotExistsAsync().Wait();
        }

        public CloudFileShare GetFileShare(string AdminId) {
            return StorageAccount.CreateCloudFileClient().GetShareReference(AdminId);
        }

        public async Task<List<CloudFile>> GetFileList(string AdminId) {
            CloudFileShare FileShare = GetFileShare(AdminId);
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

        public CloudFile GetFile(string AdminId, string FileName) {
            CloudFileShare FileShare = GetFileShare(AdminId);
            CloudFile file = FileShare.GetRootDirectoryReference().GetFileReference(FileName);
            return file;
        }

        public Boolean UploadFile(string AdminId, IFormFile file) {
            CloudFileShare FileShare = GetFileShare(AdminId);

            CloudFile FileToUpload = FileShare.GetRootDirectoryReference().GetFileReference(file.FileName);

            if (FileToUpload.ExistsAsync().Result) {
                string FileNameIfExist = Path.GetFileNameWithoutExtension(file.FileName) + "(2)" + Path.GetExtension(file.FileName);
                FileToUpload = FileShare.GetRootDirectoryReference().GetFileReference(FileNameIfExist);

                Task result = FileToUpload.UploadFromStreamAsync(file.OpenReadStream());
                result.Wait();
                return result.IsCompletedSuccessfully;
            }
            else {
                Task result = FileToUpload.UploadFromStreamAsync(file.OpenReadStream());
                result.Wait();
                return result.IsCompletedSuccessfully;
            }
        }

        public async Task<Stream> DownloadFileAsync(string AdminId, string FileName) {
            CloudFileShare FileShare = GetFileShare(AdminId);
            Stream stream = null;

            CloudFile File = FileShare.GetRootDirectoryReference().GetFileReference(FileName);
            await File.DownloadToStreamAsync(stream);

            return stream;
 
        }

        public Boolean DeleteFile(string AdminId, string FileName) {
            CloudFileShare FileShare = GetFileShare(AdminId);
            CloudFile File = FileShare.GetRootDirectoryReference().GetFileReference(FileName);
            var result = File.DeleteIfExistsAsync();
            result.Wait();

            return result.IsCompletedSuccessfully;
        }

        public CloudFile GetSpecificFile(string AdminId, string FileName) {
            CloudFileShare FileShare = GetFileShare(AdminId);
            CloudFile File = FileShare.GetRootDirectoryReference().GetFileReference(FileName);
            File.FetchAttributesAsync().Wait();

            return File;
        }

        public Boolean CheckFile(string AdminId, string FileName) {
            CloudFileShare FileShare = GetFileShare(AdminId);
            CloudFile File = FileShare.GetRootDirectoryReference().GetFileReference(FileName);

            var result = File.ExistsAsync();
            result.Wait();

            return result.Result;
        }

        public List<RetrievedFileViewModel> GetFileListBasedOnUser(string AdminId, List<string> FileList)
        {
            CloudFileShare FileShare = GetFileShare(AdminId);
            List<RetrievedFileViewModel> ModelList = new List<RetrievedFileViewModel>();

            foreach (var item in FileList)
            {
                RetrievedFileViewModel Model = new RetrievedFileViewModel();
                Model.RetrievedFile = GetSpecificFile(AdminId, item);
                ModelList.Add(Model);
            }

            return ModelList;
        }

    }
}
