using System;
using System.Collections.Generic;
using System.Text;

namespace PaniniStickerClientApp.ViewModels
{
    public class MainViewModel
    {
        public FormViewModel Form { get; set; }
        public MainViewModel() => Form = new FormViewModel();
    }
}
