using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json.Linq;
using Plugin.Media;
using Plugin.Media.Abstractions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PaniniStickerClientApp.ViewModels
{
    public class FormViewModel : INotifyPropertyChanged
    {
        private ImageSource photoPreviewSource;

        public string Frame { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public DateTime DoB { get; set; }
        public string Club { get; set; }
        public string Debut { get; set; }
        private string PhotoUrl { get; set; }
        public ICommand TakePhotoCommand => new RelayCommand(TakePhoto);
        public ICommand PickPhotoCommand => new RelayCommand(PickPhoto);
        public ICommand GenerateStickerCommand => new RelayCommand(GenerateSticker);
        public ImageSource PhotoPreviewSource
        {
            get => photoPreviewSource;
            set
            {
                photoPreviewSource = value;
                OnPropertyChanged();
            }
        }
        private MediaFile File { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName]string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private async void TakePhoto()
        {
            if (await CrossMedia.Current.Initialize() && CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
            {
                File = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "PaniniStickerClient",
                        SaveToAlbum = true,
                        CompressionQuality = 75,
                        CustomPhotoSize = 25,
                        PhotoSize = PhotoSize.MaxWidthHeight,
                        MaxWidthHeight = 1000,
                        DefaultCamera = CameraDevice.Front
                    });

                if (File == null) return;
                else
                    await Application.Current.MainPage.DisplayAlert("File Location", File.Path, "OK");

                PhotoPreviewSource = ImageSource.FromStream(() =>
                {
                    Stream stream = File.GetStream();
                    //File.Dispose();
                    return stream;
                });
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }
        }
        private async void PickPhoto()
        {
            if (CrossMedia.Current.IsPickPhotoSupported)
            {
                File = await CrossMedia.Current.PickPhotoAsync(
                    new PickMediaOptions
                    {
                        PhotoSize = PhotoSize.Medium
                    });

                if (File == null) return;

                PhotoPreviewSource = ImageSource.FromStream(() =>
                {
                    Stream stream = File.GetStream();
                    //File.Dispose();
                    return stream;
                });
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                return;
            }
        }
        private void GenerateSticker()
        {
            if (File == null) return;
            JObject imgUploaded = JObject.Parse(UploadImage(File));
            PhotoUrl = (string)imgUploaded["image"]["url"];

            RestClient client = new RestClient();
            RestRequest request = new RestRequest("https://paninistickerwebapi.azurewebsites.net/api/StickerGenerator", Method.POST, DataFormat.Json);
            request.AddJsonBody(
                new
                {
                    photoUrl = PhotoUrl,
                    frame = Frame,
                    fullName = FullName,
                    position = Position,
                    DoB = DoB.ToString("yyyy-MM-ddThh:mm:ss"),
                    club = Club,
                    country = Frame,
                    debut = Debut
                });
            IRestResponse response = client.Execute(request);
            JObject jObjectSticker = JObject.Parse(response.Content);
            Uri stickerImageUri = new Uri((string)jObjectSticker["image"]["url"]);

            PhotoPreviewSource = ImageSource.FromUri(stickerImageUri);          

        }
        private string UploadImage(MediaFile file)
        {
            RestClient client = new RestClient("https://imgbb.com/json");
            RestRequest request = new RestRequest(Method.POST);
            request.AddCookie("PHPSESSID", "e1337f77d57c68fc747a8cb52e3325ca");
            request.AddFile("source", file.Path);
            request.AddParameter("type", "file");
            request.AddParameter("action", "upload");
            request.AddParameter("privacy", "undefined");
            request.AddParameter("timestamp", "1527541040686");
            request.AddParameter("auth_token", "c08e59c27aeeb1cb0f126e3ed3856cf1fd675d06");
            request.AddParameter("nsfw", "0");
            IRestResponse response = client.Execute(request);
            return response.Content;
        }
    }
}
