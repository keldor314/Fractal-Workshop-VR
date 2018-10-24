using System;
using System.Windows;
using Fractal_Workshop_VR_CS.VR;

namespace Fractal_Workshop_VR_CS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IntPtr SwapChainHost_hWnd;
        public MainWindow()
        {
            InitializeComponent();
        }
        protected override void OnInitialized(EventArgs e)
        {
            SwapChainHost_hWnd = ((System.Windows.Forms.Control)FindName("SwapChainHost")).Handle;
            var control = new VR.Control();
            var index = control.GetDXGIOutputDevice();
            base.OnInitialized(e);
        }
    }
}
