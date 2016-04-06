using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CarSensX.Views;
using Microsoft.Win32;

namespace CarSensX
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        static public Boolean fullscreen = false;
        static public int appWidth = 1024;
        static public int appHeight = 600;
        private UIElement View;
        private ViewMode Mode = ViewMode.View;
        private OpenFileDialog ofDialog;
        public const int sensorTimeout = 3;

        public const String project = "CarSens 1.2";

        public MainWindow()
        {
            InitializeComponent();

            BtnEdit.SetIcon(global::CarSensX.Properties.Resources.iconEdit);
            BtnFullScreen.SetIcon(global::CarSensX.Properties.Resources.iconFullScreen);
            BtnExit.SetIcon(global::CarSensX.Properties.Resources.iconBack);

            BtnExit.SetLabel("Exit");
            BtnFullScreen.SetLabel("Fullscreen");
            BtnEdit.SetLabel("Configure");
            BtnBackground.SetLabel("Set Background");

            BtnEdit.MouseLeftButtonUp += new MouseButtonEventHandler(BtnEditClick);
            BtnFullScreen.MouseLeftButtonUp += new MouseButtonEventHandler(BtnFullScreenClick);
            BtnBackground.MouseLeftButtonUp += new MouseButtonEventHandler(BtnBackgroundClick);
            BtnExit.MouseLeftButtonUp += new MouseButtonEventHandler(BtnExitClick);

            if (global::CarSensX.Properties.Settings.Default.BackgroundImage != null)
            {
                String fileName = global::CarSensX.Properties.Settings.Default.BackgroundImage;
                try
                {
                    BitmapImage iSource = new BitmapImage(new Uri(fileName));
                    BackgroundGrid.Background = new ImageBrush(iSource);
                }
                catch (Exception ex)
                {
                }
            }
        }

        public void OpenWindow(UIElement uiElement)
        {
            this.MainView.Children.Clear();
            this.MainView.Children.Add(uiElement);
        }

        private void BtnEditClick(object sender, MouseButtonEventArgs e)
        {
            if (Mode == ViewMode.View)
            {
                this.View = this.MainView.Children[0];
            }
            this.MainView.Children.Clear();
            if (Mode == ViewMode.View)
            {
                this.MainView.Children.Add(new SensorConfig(this));
                this.Mode = ViewMode.Edit;
            }
            else if (Mode == ViewMode.Edit)
            {
                ((SensorView)this.View).UpdateUI();
                this.MainView.Children.Add(this.View);
                this.Mode = ViewMode.View;
            }
        }

        private void BtnFullScreenClick(object sender, MouseButtonEventArgs e)
        {
            System.Console.Out.WriteLine("Not implemented");
        }

        private void BtnBackgroundClick(object sender, MouseButtonEventArgs e)
        {
           ofDialog = new OpenFileDialog();
           ofDialog.Filter = "JPEGs (*.jpg) | *.jpg";

            ofDialog.Multiselect = false;
            //ofDialog.ShowDialog();
            //ofDialog.FileOk += new CancelEventHandler(openFileDialog1_FileOk);

            bool? userClickedOK = ofDialog.ShowDialog();

            // Process input if the user clicked OK.
            if (userClickedOK == true)
            {
                String fileName = ofDialog.FileName;
                BitmapImage iSource = new BitmapImage(new Uri(fileName));
                // System.Drawing.Image img = System.Drawing.Image.FromFile(fileName);
                global::CarSensX.Properties.Settings.Default.BackgroundImage = fileName;
                global::CarSensX.Properties.Settings.Default.Save();


                ImageBrush iBrush = new ImageBrush(iSource);
                BackgroundGrid.Background = iBrush;
            }
        }

        private void BtnExitClick(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }

    public enum ViewMode
    {
        View, Edit
    }
}
