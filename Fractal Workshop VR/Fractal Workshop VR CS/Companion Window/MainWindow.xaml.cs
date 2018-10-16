using System;
using System.Windows;

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
            base.OnInitialized(e);
        }
    }
}
