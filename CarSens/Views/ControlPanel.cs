using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarSens.Views
{
    /// <summary>
    /// Control Panel for the Application
    /// </summary>
    public partial class ControlPanel : UserControl
    {
        private MainScreen mainScreen;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="screen"></param>
        public ControlPanel(MainScreen screen)
        {
            this.mainScreen = screen;
            InitializeComponent();

        }

        /// <summary>
        /// This will close the application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
           mainScreen.Close();
        }

        /// <summary>
        /// Opens the edit window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            mainScreen.OpenEditView(btnEdit);
        }

        /// <summary>
        /// sets the application to fullscreen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFullScreen_Click(object sender, EventArgs e)
        {
            mainScreen.FullScreen();
            int btnWidth = (int)(0.18f * Program.appWidth);
            int btnHeight = (int)(0.12f * Program.appHeight);

            //this.btnEdit.Width = btnWidth;
            //this.btnEdit.Height = btnHeight;
        }
    }
}
