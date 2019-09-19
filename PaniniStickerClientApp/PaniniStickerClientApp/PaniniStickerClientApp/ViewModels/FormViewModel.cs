using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PaniniStickerClientApp.ViewModels
{
    public class FormViewModel
    {
        public string Frame { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public DateTime DoB { get; set; }
        public string Club { get; set; }
        public string Debut { get; set; }
        public ICommand TakePhotoCommand => new RelayCommand(TakePhoto);
        public ICommand PickPhotoCommand => new RelayCommand(PickPhoto);
        public ImageSource PhotoPreview { get; set; }

        private void TakePhoto()
        {
            throw new NotImplementedException();
        }
        private void PickPhoto()
        {
            throw new NotImplementedException();
        }
    }
}
