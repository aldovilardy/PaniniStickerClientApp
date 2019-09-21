using GalaSoft.MvvmLight.Command;
using Plugin.Media;
using Plugin.Media.Abstractions;
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
        public ICommand TakePhotoCommand => new RelayCommand(TakePhoto);
        public ICommand PickPhotoCommand => new RelayCommand(PickPhoto);
        public ImageSource PhotoPreviewSource
        {
            get => photoPreviewSource;
            set
            {
                photoPreviewSource = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName]string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private async void TakePhoto()
        {
            if (await CrossMedia.Current.Initialize() && CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
            {
                MediaFile file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Test",
                        SaveToAlbum = true,
                        CompressionQuality = 75,
                        CustomPhotoSize = 25,
                        PhotoSize = PhotoSize.MaxWidthHeight,
                        MaxWidthHeight = 1000,
                        DefaultCamera = CameraDevice.Front
                    });

                if (file == null) return;
                else
                    await Application.Current.MainPage.DisplayAlert("File Location", file.Path, "OK");

                PhotoPreviewSource = ImageSource.FromStream(() =>
                {
                    Stream stream = file.GetStream();
                    file.Dispose();
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
                MediaFile file = await CrossMedia.Current.PickPhotoAsync(
                    new PickMediaOptions
                    {
                        PhotoSize = PhotoSize.Small
                    });

                if (file == null) return;

                PhotoPreviewSource = ImageSource.FromStream(() =>
                {
                    Stream stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                return;
            }
        }
    }
}
