using Strona_v2.Shared.File;
using System.Net;

namespace Strona_v2.Server.Data.FileData
{
    public class FileUpload
    {
        public int MaxAlloweFiles { get; set; } = 3; //max ilość plików
        public long MaxFileSize { get; set; } = 1024 * 1024 * 5;// 5MB

        public int FilesProcessed  = 0;

        public FileModel? _FileModel { get; set; }

        public Uri? ResourcePatch { get; set; }
        string? TrustedFileName;

        //główna metoda do pobierania plików
        public async Task<FileModel> UploadAsync(IEnumerable<IFormFile> files, ILogger _logger, IWebHostEnvironment _env)
        {
            _FileModel = new();

            foreach (var file in files)
            {
                var UploadResult = new FileSingleModel();
                string UntrustedFileName = file.FileName;
                var TrustedFileForDisplay = WebUtility.HtmlEncode(UntrustedFileName);

                if (FilesProcessed < MaxAlloweFiles)
                {
                    //sprawdzenie pierwszego błędu
                    UploadResult = FirstError(file, _logger, UploadResult, TrustedFileForDisplay);

                    //sprawdzenie drugiego błędu
                    UploadResult = SecondError(file, _logger, UploadResult, TrustedFileForDisplay);

                    //sprawdzenie trzeciego błedu i zapisanie pliku
                    UploadResult = await ThirdError(file, _logger, _env, UploadResult);

                    //błędy są opisane niżej

                    //sprawdzenie kolejnego pliku
                    FilesProcessed++;
                }
                else
                {
                    //sprawdzenie czwartego błedu
                    UploadResult = FourthError(file, _logger, UploadResult, TrustedFileForDisplay);
                }
                _FileModel.Files.Add(UploadResult);
            }
            return _FileModel;
        }

        //sprawdznie czy jakieś plik został przesłany 
        protected FileSingleModel FirstError(IFormFile file, ILogger _logger, FileSingleModel fileSingleModel, string FileName)
        {
            if (file.Length == 0)
            {
                _logger.LogInformation("{FileName} length is 0 (Error 1)", FileName);
                fileSingleModel.ErrorCode = 1;
                return fileSingleModel;
            }
            return fileSingleModel;
        }

        //sprawdzenie czy nie przesłano za dużego pliku (czy mb się zgadza)
        protected FileSingleModel SecondError(IFormFile file, ILogger _logger, FileSingleModel fileSingleModel, string FileName)
        {
            if (file.Length > MaxFileSize)
            {
                _logger.LogInformation("{FileName} of {Length} bytes is larger than the limit of {MaxSize} bytes (Error 2)",
                    FileName, file.Length, MaxFileSize);
                fileSingleModel.ErrorCode = 2;
                return fileSingleModel;
            }
            return fileSingleModel;
        }

        //wszystko się zgadza plik został zapisany
        protected async Task<FileSingleModel> ThirdError(IFormFile file, ILogger _logger, IWebHostEnvironment _env, FileSingleModel fileSingleModel)
        {
            if (file.Length != 0 && file.Length < MaxFileSize)
            {
                try
                {
                    //bezpieczna nazwa
                    TrustedFileName = Path.GetRandomFileName();
                    string UnSave_Uploads = "unsave_uploads";

                    //ścieżka do pliku
                    var _Patch = Path.Combine(_env.ContentRootPath, _env.EnvironmentName, UnSave_Uploads);
                    CreatFolder(_Patch);
                    _Patch = Path.Combine(_Patch, TrustedFileName);

                    await using FileStream fs = new(_Patch, FileMode.Create);
                    await file.CopyToAsync(fs);

                    _logger.LogInformation("{TrustedFileName} saved at {Patch}", TrustedFileName, _Patch);
                    fileSingleModel.Uploaded = true;
                    fileSingleModel.StoredFileName = TrustedFileName;

                    return fileSingleModel;
                }
                catch (IOException ex)
                {
                    _logger.LogInformation("{FileName} error on upload (Error 3):{Message}", TrustedFileName, ex.Message);
                    fileSingleModel.ErrorCode = 3;

                    return fileSingleModel;
                }
            }
            return fileSingleModel;
        }

        //stworzenie folderu gdzie zapisuje się plik
        protected void CreatFolder(string Patch)
        {
            if (!System.IO.File.Exists(Patch))
            {
                Directory.CreateDirectory(Patch);
            }
        }

        //sprawdzenie czy nie przesłało się za dużo plików
        protected FileSingleModel FourthError(IFormFile file, ILogger _logger, FileSingleModel fileSingleModel, string FileName)
        {
            if (file.Length == 0)
            {
                _logger.LogInformation("{FileName} not uploaded becouse the request exceede the allowed {count} of files (Error 4)", FileName, MaxAlloweFiles);
                fileSingleModel.ErrorCode = 4;
                return fileSingleModel;
            }
            return fileSingleModel;
        }
    }
}
