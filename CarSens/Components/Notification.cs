using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarSens.Components
{
    /// <summary>
    /// NotificationClass.
    /// </summary>
    public partial class Notification : UserControl
    {
        public static Form mainView;

        private static int index;

        /// <summary>
        /// Constructor.
        /// This will open a new Notification.
        /// Beware, that this Singleton must know the mainView.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="text"></param>
        public Notification(String type, String text)
        {
            InitializeComponent();
            if (type.ToLower().Equals("info"))
            {
                this.BackColor = Color.Green;
                this.txtContent.BackColor = Color.Green;
            }
            this.lblType.Text = type;
            this.txtContent.Text = text;
            Notification.mainView.Controls.Add(this);
            Notification.index = Notification.mainView.Controls.IndexOf(this);
            this.Location = new System.Drawing.Point(Notification.mainView.Width - 150 - 12, Notification.mainView.Height - 100 - 12);
            this.tick.Enabled = true;
            this.BringToFront();

            this.Click += new System.EventHandler(tick_Tick);
        }

       
        /// <summary>
        /// UpdateTick to remove Notification after a few seconds.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tick_Tick(object sender, EventArgs e)
        {
            Notification.mainView.Controls.Remove(this);
            this.tick.Enabled = false;
        }
    }
}
