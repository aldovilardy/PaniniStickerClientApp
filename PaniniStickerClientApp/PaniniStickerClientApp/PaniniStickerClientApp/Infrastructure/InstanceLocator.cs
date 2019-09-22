using PaniniStickerClientApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaniniStickerClientApp.Infrastructure
{
    public class InstanceLocator
    {
        public MainViewModel Main { get; set; }

        public InstanceLocator() => Main = new MainViewModel();
    }
}
