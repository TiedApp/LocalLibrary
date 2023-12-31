using GlobalShared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using FileIO = System.IO.File;

namespace TiedAppLocal.Controllers
{
    public class ValidationController : Controller
    {
        private string CLDir { get; set; }
        private string ArDir { get; set; }
        private string ComDir { get; set; }

        private readonly IWebHostEnvironment _env;
        public ValidationController(IWebHostEnvironment env)
        {
            _env = env;
            CLDir = Path.Combine(_env.ContentRootPath, LibVariables.CompanyLibrary);
            ArDir = Path.Combine(_env.ContentRootPath, LibVariables.ActionRequest);
            ComDir = Path.Combine(_env.ContentRootPath, LibVariables.InternalCom);
        }

        [HttpPost]
        public async Task<IActionResult> SaveFile([FromBody] LocalFileInfo fileInfo)
        {
            try
            {
                if (string.IsNullOrEmpty(fileInfo.RequestCodeJson))
                    return ReturnOnResponseFailure("Request code missing");

                string result = await HandleHttpRequest.Get(LibVariables.ValidateCodeURL, fileInfo.RequestCodeJson);
                var response = JsonConvert.DeserializeObject<Response>(result);
                if (!response.Success)
                    return ReturnOnResponseFailure(response.Message);

                string errorConstraint = CheckGeneralConstraints(fileInfo, true);
                if (!string.IsNullOrEmpty(errorConstraint))
                    return ReturnOnResponseFailure(errorConstraint);

                if (fileInfo.DefautlFileProcess)
                    return DefautlFilePost(fileInfo);
                else if (fileInfo.ActionRequestProcess)
                    return ActionRequestPost(fileInfo);
                else if (fileInfo.InternalComProcess)
                    return InternalComPost(fileInfo);
                else
                    return ReturnOnResponseFailure("Cannot proceed");
            }
            catch (Exception err)
            {
                return ReturnOnResponseFailure(err.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFileTask([FromBody] LocalFileInfo fileInfo)
        {
            try
            {
                if (string.IsNullOrEmpty(fileInfo.RequestCodeJson))
                    return ReturnOnResponseFailure("Request code missing");

                string result = await HandleHttpRequest.Get(LibVariables.ValidateCodeURL, fileInfo.RequestCodeJson);
                var response = JsonConvert.DeserializeObject<Response>(result);
                if (!response.Success)
                    return ReturnOnResponseFailure(response.Message);

                string errorConstraint = CheckGeneralConstraints(fileInfo, false);
                if (!string.IsNullOrEmpty(errorConstraint))
                    return ReturnOnResponseFailure(errorConstraint);

                return DeleteFile(fileInfo);
            }
            catch (Exception err)
            {
                return ReturnOnResponseFailure(err.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetFile([FromBody] LocalFileInfo fileInfo)
        {
            try
            {
                string result = await HandleHttpRequest.Get(LibVariables.ValidateCodeURL, fileInfo.RequestCodeJson);
                var response = JsonConvert.DeserializeObject<Response>(result);
                if (!response.Success)
                {
                    return ReturnOnDownloadFailure(response.Message);
                }

                string errorConstraint = CheckGeneralConstraints(fileInfo, false);
                if (!string.IsNullOrEmpty(errorConstraint))
                    return ReturnOnDownloadFailure(errorConstraint);

                if (fileInfo.DefautlFileProcess)
                    return DefaultFileGet(fileInfo);
                else if (fileInfo.ActionRequestProcess)
                    return ActionRequestGet(fileInfo);
                else if (fileInfo.InternalComProcess)
                    return InternalComGet(fileInfo);
                else
                    return ReturnOnDownloadFailure("File cannot be found");
            }
            catch (Exception err)
            {
                return ReturnOnDownloadFailure(err.Message);
            }
        }

        #region On post private processes
        private IActionResult DefautlFilePost(LocalFileInfo fileInfo)
        {
            string department = fileInfo.Department;
            string activity = fileInfo.Activity;
            string foldername = fileInfo.Foldername;
            string fileFolder = fileInfo.FileFolder;
            string filename = fileInfo.Filename;
            byte[] fileByte = fileInfo.FileByte;
            string dirPath = Path.Combine(CLDir, department, activity, foldername, fileFolder);
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            return ReturnPostResult(dirPath, filename, fileByte);
        }

        private IActionResult ActionRequestPost(LocalFileInfo fileInfo)
        {
            string filename = fileInfo.Filename;
            string requestReference = fileInfo.RequestReference;
            byte[] fileByte = fileInfo.FileByte;

            string dirPath = Path.Combine(ArDir, requestReference);
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            return ReturnPostResult(dirPath, filename, fileByte);
        }

        private IActionResult InternalComPost(LocalFileInfo fileInfo)
        {
            string filename = fileInfo.Filename;
            string comReference = fileInfo.ComReference;
            byte[] fileByte = fileInfo.FileByte;

            string dirPath = Path.Combine(ComDir, comReference);
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            return ReturnPostResult(dirPath, filename, fileByte);
        }

        private IActionResult ReturnPostResult(string dirPath, string filename, byte[] fileByte)
        {
            string filePath = Path.Combine(dirPath, filename);
            bool fileCreate = CreateFile(filePath, fileByte);
            if (!fileCreate)
                return ReturnOnResponseFailure("File not saved");

            return Ok(new Response { Success = true });
        }

        private bool CreateFile(string filePath, byte[] fileByte)
        {
            StaticFunctions.CreateFileByte(filePath, fileByte);
            return FileIO.Exists(filePath);
        }

        private IActionResult DeleteFile(LocalFileInfo fileInfo)
        {
            string department = fileInfo.Department;
            string activity = fileInfo.Activity;
            string foldername = fileInfo.Foldername;
            string fileFolder = fileInfo.FileFolder;
            string filename = fileInfo.Filename;
            string filepath = Path.Combine(CLDir, department, activity, foldername, fileFolder, filename);
            if (!FileIO.Exists(filepath))
                return ReturnOnResponseFailure("File does not exist");

            string message = StaticFunctions.DeleteFile(filepath);
            if (!string.IsNullOrEmpty(message))
                return ReturnOnResponseFailure(message);

            return Ok(new Response { Success = true,});
        }
        #endregion

        #region On get private processes
        private IActionResult DefaultFileGet(LocalFileInfo fileInfo)
        {
            string department = fileInfo.Department;
            string activity = fileInfo.Activity;
            string foldername = fileInfo.Foldername;
            string fileFolder = fileInfo.FileFolder;
            string filename = fileInfo.Filename;
            string filePath = Path.Combine(CLDir, department, activity, foldername, fileFolder, filename);
            return ReturnTheFile(filePath);
        }

        private IActionResult ActionRequestGet(LocalFileInfo fileInfo)
        {
            string filename = fileInfo.Filename;
            string comReference = fileInfo.RequestReference;
            string filePath = Path.Combine(ArDir, comReference, filename);
            return ReturnTheFile(filePath);
        }

        private IActionResult InternalComGet(LocalFileInfo fileInfo)
        {
            string filename = fileInfo.Filename;
            string comReference = fileInfo.ComReference;
            string filePath = Path.Combine(ComDir, comReference, filename);
            return ReturnTheFile(filePath);
        }

        private IActionResult ReturnTheFile(string filePath)
        {
            if (!FileIO.Exists(filePath))
                return ReturnOnDownloadFailure("File does not exist");

            byte[] fileByte = StaticFunctions.ReadFileByte(filePath);
            if (fileByte == null)
                return ReturnOnDownloadFailure("File is empty");

            return Ok(new FileByteDownload { FileByte = fileByte, });
        }
        #endregion

        #region Check all constraints
        private string CheckGeneralConstraints(LocalFileInfo fileInfo, bool save)
        {
            if (fileInfo.DefautlFileProcess)
                return CheckDefaultConstraints(fileInfo, save);
            else if (fileInfo.ActionRequestProcess)
                return CheckActionRequestConstraints(fileInfo, save);
            else if (fileInfo.InternalComProcess)
                return CheckInternalComConstraints(fileInfo, save);
            else
                return "Cannot proceed";
        }

        private string CheckDefaultConstraints(LocalFileInfo fileInfo, bool save)
        {
            if (string.IsNullOrEmpty(fileInfo.Department))
                return "Department name is mandatory";
            else if (string.IsNullOrEmpty(fileInfo.Activity))
                return "Department's activity is mandatory";
            else if (string.IsNullOrEmpty(fileInfo.Foldername))
                return "Folder name is mandatory";
            else if (string.IsNullOrEmpty(fileInfo.FileFolder))
                return "'FileFolder' is mandatory";
            else if (string.IsNullOrEmpty(fileInfo.Filename))
                return "File name is mandatory";
            else if (fileInfo.FileByte == null && save)
                return "No file to save";

            return string.Empty;
        }

        private string CheckActionRequestConstraints(LocalFileInfo fileInfo, bool save)
        {
            if (string.IsNullOrEmpty(fileInfo.Filename))
                return "File name is mandatory";
            else if (string.IsNullOrEmpty(fileInfo.RequestReference))
                return "'RequestReference' missing";
            else if (fileInfo.FileByte == null && save)
                return "No file to save";

            return string.Empty;
        }

        private string CheckInternalComConstraints(LocalFileInfo fileInfo, bool save)
        {
            if (string.IsNullOrEmpty(fileInfo.Filename))
                return "File name is mandatory";
            else if (string.IsNullOrEmpty(fileInfo.ComReference))
                return "'ComReference' missing";
            else if (fileInfo.FileByte == null && save)
                return "No file to save";

            return string.Empty;
        }
        #endregion

        #region On error processes
        public IActionResult ReturnOnResponseFailure(string message)
        {
            return Ok(new Response { Success = false, Message = message });
        }

        public IActionResult ReturnOnDownloadFailure(string message)
        {
            return Ok(new FileByteDownload { ErrorMessage = message });
        }
        #endregion
    }
}
