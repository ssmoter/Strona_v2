using Microsoft.AspNetCore.Components.Forms;

namespace Strona_v2.Client.Data.File.Upload
{
    public class UploadFileBaseError
    {
        public string[] TableString { get; set; }  //lista błędów
        public bool[] TableBool { get; set; }

        public UploadFileBaseError(int Size)
        {
            TableString = new string[Size];
            TableBool = new bool[Size];
            TrueTable(Size);
        }

        private void TrueTable(int n)
        {
            for (int i = 0; i < n; i++)
            {
                TableBool[i] = true;
            }
        }

    }

    public class UploadFileBase //Klasa sprawdza czy zostały wysłane prawidłowe pliki oraz wyświetla podgląd
    {
        public UploadFileBase(int maxFileSizeMb) //nadpisanie wielkości pliku
        {
            MaxFileSizeMb = maxFileSizeMb;
            MaxFileSize = FileSize * MaxFileSizeMb;

        }
        public UploadFileBase() //podstawowa wielkość pliku
        {
            MaxFileSize = FileSize * MaxFileSizeMb;

        }
        public List<string>? ImgDataUri { get; set; } //lista do podglądu
        public InputFileChangeEventArgs? InputFileChange { get; set; } //przesłane dane
        public UploadFileBaseError? Error { get; set; } //Błędy

        public string[] FileTypAccess = {
            "png",
            "jpg",
             }; //dopuszczane pliki

        public int MaxFileSizeMb { get; set; }
        public void SetMaxFileSizeMb(int value)
        {
            MaxFileSizeMb = value;
            MaxFileSize = FileSize * MaxFileSizeMb;
        }

        private long MaxFileSize;
        public long FileSize { get; } = 1024 * 1024;
        public int MaxAllowedFiles { get; set; } = 3;

        public int SizeOfList(InputFileChangeEventArgs e) //metoda sprawdzająca ilośc przesłanych plików
        {
            int n = 0;
            foreach (var item in e.GetMultipleFiles())
            {
                n++;
            }
            return n;
        }

        public void CheckFileType(InputFileChangeEventArgs e) //sprawdzenie czy pliki moją dopuszczone rozszerzenie
        {
            for (int i = 0; i < e.GetMultipleFiles().Count; i++)
            {
                bool CorrectType = false;
                var fileName = e.GetMultipleFiles()[i].Name.Split('.');
                foreach (var item in FileTypAccess)
                {
                    if (item == fileName[fileName.Count() - 1])
                    {
                        CorrectType = true;
                    }
                }
                if (!CorrectType)
                {
                    Error.TableString[i] = "Error: Niewspierane rozszerzenie pliku";
                    Error.TableBool[i] = false;
                }
            }
        }

        public void CheckFileSize(InputFileChangeEventArgs e) // sprawdzenie czy pliki mają dopuszczony rozmiar
        {
            int n = 0;
            foreach (var file in e.GetMultipleFiles())
            {
                if (file.Size > MaxFileSize)
                {
                    Error.TableString[n] = "Error:Za duży plik\nDopuszczalna wartość:" + MaxFileSizeMb + "Mb";
                    Error.TableBool[n] = false;
                }
                n++;
            }
        }

        public int CheckAllowedFiles(InputFileChangeEventArgs e) //sprawdzenie ilości plików czy wartość jest przekroczona czy nie
        {
            int n = SizeOfList(e);
            if (n > MaxAllowedFiles)
            {
                Error.TableString[n] = "Error:Przesłano za dużo plików\nDopuszczalna liczba:" + MaxAllowedFiles;
                Error.TableBool[n] = false;
            }
            int result = MaxAllowedFiles;
            if (n < MaxAllowedFiles)
            {
                result = n;
            }
            return result;
        }


        public async Task CastToString(InputFileChangeEventArgs e) //konwertowanie img pod stringa do wyświetlenia w podglądzie
        {
            ImgDataUri = new();
            string result;
            for (int i = 0; i < CheckAllowedFiles(e); i++)
            {
                var fileName = e.GetMultipleFiles(MaxAllowedFiles)[i].Name.Split('.');
                var ImgFile = await e.GetMultipleFiles(MaxAllowedFiles)[i].RequestImageFileAsync("image/" + fileName[fileName.Count() - 1], maxWidth: 1920, maxHeight: 1080);
                if (Error.TableBool[i])
                {
                    InputFileChange = e; //przypisanie sprawdzonych plików 

                    using Stream fileStream = ImgFile.OpenReadStream(MaxFileSize);
                    {
                        using MemoryStream ms = new MemoryStream();
                        {
                            await fileStream.CopyToAsync(ms);
                            result = $"data:image/" + fileName[fileName.Count() - 1] + ";base64," + Convert.ToBase64String(ms.ToArray());

                            ImgDataUri.Add(result);
                        }
                    }
                }
            }
        }
    }
}
