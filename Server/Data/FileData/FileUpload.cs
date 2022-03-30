using Strona_v2.Shared.File;
using System.Net;

namespace Strona_v2.Server.Data.FileData
{
    public class FileUpload
    {
        public int MaxAlloweFiles { get; set; } = 3; //max ilość plików

        public long MaxFileSize { get; set; } = 1024 * 1024 * 5;// 5MB

        public int FilesProcessed = 0;

        public Uri? ResourcePatch { get; set; }

        private string? TrustedFileName;

        private string UnSaveUploads = "unsave_uploads";

        //główna metoda do pobierania plików
        public async Task<FileModelServer> UploadAsync(IEnumerable<IFormFile> files, int UserId, ILogger _logger, IWebHostEnvironment _webHostEnvironment)
        {
            FileModelServer? _FileModel = new();
            _FileModel.Files = new();

            try
            {
                foreach (var file in files)
                {
                    var UploadResult = new FileSingleModel();
                    string UntrustedFileName = file.FileName;
                    var TrustedFileForDisplay = WebUtility.HtmlEncode(UntrustedFileName);

                    if (FilesProcessed < MaxAlloweFiles)
                    {
                        //sprawdzenie pierwszego błędu
                        UploadResult = FirstError(file, _logger, UploadResult, TrustedFileForDisplay);

                        if (UploadResult.ErrorCode > 0)
                        {
                            return null;
                        }

                        //sprawdzenie drugiego błędu
                        UploadResult = SecondError(file, _logger, UploadResult, TrustedFileForDisplay);
                        if (UploadResult.ErrorCode > 0)
                        {
                            return null;
                        }
                        //sprawdzenie trzeciego błedu i zapisanie pliku
                        UploadResult = await SuccessfullyError(file, UserId, _logger, _webHostEnvironment, UploadResult);
                        if (UploadResult.ErrorCode > 0)
                        {
                            return null;
                        }
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
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }

            _FileModel.Path = Path.Combine(_webHostEnvironment.ContentRootPath, _webHostEnvironment.EnvironmentName, UnSaveUploads);
            
             var combine = CombineString(_FileModel.Files);

            _FileModel.StoredFileName = combine.StoredFileName;
            _FileModel.Type=combine.Type;


            return _FileModel;
        }

        //sprawdznie czy jakieś plik został przesłany 
        private FileSingleModel FirstError(IFormFile file, ILogger _logger, FileSingleModel fileSingleModel, string FileName)
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
        private FileSingleModel SecondError(IFormFile file, ILogger _logger, FileSingleModel fileSingleModel, string FileName)
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
        private async Task<FileSingleModel> SuccessfullyError(IFormFile file, int UserId, ILogger _logger, IWebHostEnvironment _env, FileSingleModel fileSingleModel)
        {
            if (file.Length != 0 && file.Length < MaxFileSize)
            {
                try
                {
                    //bezpieczna nazwa
                    TrustedFileName = Path.GetRandomFileName();
                    //ścieżka do pliku
                    var _Patch = Path.Combine(_env.ContentRootPath, _env.EnvironmentName, "unsafe_uploads", UserId.ToString());
                    CreatFolder(_Patch);
                    _Patch = Path.Combine(_Patch, TrustedFileName);

                    await using FileStream fs = new(_Patch, FileMode.Create);
                    await file.CopyToAsync(fs);

                    _logger.LogInformation("{TrustedFileName} saved at {Patch}", TrustedFileName, _Patch);
                    fileSingleModel.Uploaded = true;
                    fileSingleModel.StoredFileName = TrustedFileName;
                    fileSingleModel.ErrorCode = 0;
                    fileSingleModel.Type = SplitTyp(file.FileName);



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

        //wycięcie typu pliku
        private string SplitTyp(string UnTrusted)
        {
            var type = UnTrusted.Split('.');

            return type[1];
        }

        //Przypisanie wielu plików do jednego stringa
        private FileModelClient CombineString(List<FileSingleModel> files)
        {
            FileModelClient file = new();

            for (int i = 0; i < files.Count; i++)
            {
                file.StoredFileName += files[i].StoredFileName + " / ";
                file.Type += files[i].Type + " / ";
            }
            return file;
        }

        //stworzenie folderu gdzie zapisuje się plik
        private void CreatFolder(string Patch)
        {
            if (!System.IO.File.Exists(Patch))
            {
                Directory.CreateDirectory(Patch);
            }
        }

        //sprawdzenie czy nie przesłało się za dużo plików
        private FileSingleModel FourthError(IFormFile file, ILogger _logger, FileSingleModel fileSingleModel, string FileName)
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
