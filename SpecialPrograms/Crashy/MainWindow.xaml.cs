using System;
using System.Windows;

namespace Crashy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static MainWindow()
        {
            AppDomain.CurrentDomain.UnhandledException += handleUnhandled;
        }

        private static void handleUnhandled(object sender, UnhandledExceptionEventArgs e)
        {
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void handleCrashClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
