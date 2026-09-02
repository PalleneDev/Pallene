using Avalonia.Controls;
using System;
using Pallene.Lib;
using System.Diagnostics;
using Avalonia.Media;

namespace Pallene.Views
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            IntPtr? handle = this.TryGetPlatformHandle()?.Handle;
            if (handle != null)
                WindowBorder.SetBorderColor((IntPtr)handle, Color.Parse("#1070b0"));
            
        }
    }
}