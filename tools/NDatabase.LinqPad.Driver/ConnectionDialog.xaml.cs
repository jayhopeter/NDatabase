using System;
using System.IO;
using System.Windows;
using LINQPad.Extensibility.DataContext;

namespace NDatabase.LinqPad.Driver
{
    /// <summary>
    /// Interaction logic for ConnectionDialog.xaml
    /// </summary>
    public partial class ConnectionDialog : Window
    {
        private readonly IConnectionInfo _cxInfo;
        
        public ConnectionDialog(IConnectionInfo cxInfo)
        {
            _cxInfo = cxInfo;
            DataContext = cxInfo.CustomTypeInfo;
            Background = SystemColors.ControlBrush;

            InitializeComponent();
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void BrowseCustomAssemblyClick(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
                             {
                                 Title = "Choose custom assembly",
                                 DefaultExt = ".dll",
                             };

            if (dialog.ShowDialog() == true)
                _cxInfo.CustomTypeInfo.CustomAssemblyPath = dialog.FileName;
        }

        private void ChooseClick(object sender, RoutedEventArgs e)
        {
            var assemPath = _cxInfo.CustomTypeInfo.CustomAssemblyPath;
            if (assemPath.Length == 0)
            {
                MessageBox.Show("First enter a path to an assembly.");
                return;
            }

            if (!File.Exists(assemPath))
            {
                MessageBox.Show("File '" + assemPath + "' does not exist.");
                return;
            }

            string[] customTypes;
            try
            {
                customTypes = _cxInfo.CustomTypeInfo.GetCustomTypesInAssembly();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error obtaining custom types: " + ex.Message);
                return;
            }
            if (customTypes.Length == 0)
            {
                MessageBox.Show("There are no public types in that assembly.");  // based on.........
                return;
            }

            var result =
                (string) LINQPad.Extensibility.DataContext.UI.Dialogs.PickFromList("Choose Custom Type", customTypes);

            if (result != null) 
                _cxInfo.CustomTypeInfo.CustomTypeName = result;
        }

        private void BrowseAppConfig(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
                             {
                                 Title = "Choose application config file",
                                 DefaultExt = ".config",
                             };

            if (dialog.ShowDialog() == true)
                _cxInfo.AppConfigPath = dialog.FileName;
        }
    }
}
