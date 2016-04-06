using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CarSens.Views;
using CarSens.Components;

using System.Runtime.InteropServices;

namespace CarSens
{
    public partial class MainScreen : Form
    {
        private ControlPanel ctrlPanel;
        private SensorView sensorView;
        private EditView editView;

        private Boolean fullscreen = false;

        // Needed for Moving the Application Window
        public const int WM_NCLBUTTONDOWN = 0xA1;
        // Needed for moving the Application Window
        public const int HT_CAPTION = 0x2;

        // Needed for moving the application window
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        // Needed for moving the application window
        [DllImportAttribute("user32.dll")]

        // Needed for moving the application window
        public static extern bool ReleaseCapture();


        /// <summary>
        /// Constructor
        /// </summary>
        public MainScreen()
        {
            InitializeComponent();
            this.appInfo.Text = Program.project;
            String file = global::CarSens.Properties.Settings.Default.BackgroundImage;
            if (!file.Equals(""))
            {
                Image img = Image.FromFile(file);
                this.BackgroundImage = img;
            }
            Notification.mainView = this;
            this.initView();
        }

        /// <summary>
        /// Initializes the View.
        /// </summary>
        private void initView()
        {
            this.Opacity = 0;
            ctrlPanel = new ControlPanel(this);
            this.Controls.Add(ctrlPanel);
           

            this.sensorView = new SensorView();
            sensorView.Height = this.Height;
            sensorView.Width = this.Width;
            this.editView = new EditView(this);
            this.Controls.Add(sensorView);

            this.setPosition();
        }

        /// <summary>
        /// Moves the Application window.
        /// </summary>
        private void setPosition()
        {
            int width = this.Size.Width;
            ctrlPanel.Location = new System.Drawing.Point((width - ctrlPanel.Size.Width - 5), 45);
            sensorView.Location = new System.Drawing.Point(0,0);
        }

        /// <summary>
        /// Opens the Edit Window or the mainView, depending which is currently opened.
        /// </summary>
        /// <param name="btnEdit"></param>
        internal void OpenEditView(Button btnEdit)
        {

            if (Controls.Contains(sensorView))
            {
                CarSens.Sensors.Sensor[] sens = CarSens.Sensors.SensorManager.getAllSensors();
                foreach (CarSens.Sensors.Sensor s in sens)
                {
                    s.disconnect();
                }
                this.Controls.Remove(sensorView);
                this.Controls.Add(editView);
                btnEdit.Image = global::CarSens.Properties.Resources.iconBack;
                this.appInfo.Text = Program.project + " - Properties";
                
            }
            else
            {
                sensorView.update();
                this.Controls.Remove(editView);
                this.Controls.Add(sensorView);
                btnEdit.Image = global::CarSens.Properties.Resources.iconEdit;
                this.appInfo.Text = Program.project;
                
            }
            btnEdit.ImageAlign = ContentAlignment.MiddleCenter;
        }

        /// <summary>
        /// Catcher for the musedown.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainScreen_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        /// <summary>
        /// Sets the application to Fullscreen
        /// </summary>
        internal void FullScreen()
        {

            int height; 
            int width;
            if (fullscreen)
            {
                width = global::CarSens.Properties.Settings.Default.OriginWidth;
                height = global::CarSens.Properties.Settings.Default.OriginHeight;

                this.panel1.Width = width;

                this.Left = global::CarSens.Properties.Settings.Default.LastLeft;
                this.Top = global::CarSens.Properties.Settings.Default.LastTop;
                fullscreen = false;
                lblCopy.Top = height - 15;
            }
            else
            {
                height = Screen.FromControl(this).Bounds.Height;
                width = Screen.FromControl(this).Bounds.Width;

                

                this.panel1.Width = width;

                global::CarSens.Properties.Settings.Default.LastTop = this.Top;
                global::CarSens.Properties.Settings.Default.LastLeft = this.Left;
                global::CarSens.Properties.Settings.Default.Save();

                this.Left = 0;
                this.Top = 0;
                fullscreen = true;
                lblCopy.Top = height - 15;
            }

            this.Width = width;
            this.Height = height;

            Program.appWidth = width;
            Program.appHeight = height;

            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.setPosition();

            if (!Controls.Contains(sensorView))
            {
                this.Controls.Remove(editView);
                sensorView.Height = height;
                sensorView.Width = width;
                sensorView.update();
                this.Controls.Add(sensorView);
                this.appInfo.Text = Program.project;

            }

        }

        /// <summary>
        /// Blends in the application on load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainScreen_Load(object sender, EventArgs e)
        {
            opcaity.Interval = 50;
            opcaity.Enabled = true;
            
        }

        /// <summary>
        /// Timer for blending in and out
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void opacity_Tick(object sender, EventArgs e)
        {
            if (this.Opacity < 1)
            {
                this.Opacity += 0.1f;
            }
        }
    }
}
