using System;
using System.Collections.Generic;
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

namespace CarSensX.Components
{
    /// <summary>
    /// Interaktionslogik für UserControl1.xaml
    /// </summary>
    public partial class ControlButton : UserControl
    {
        private Brush returnColor;
        public FrameworkElement ButtonName;

        public ControlButton()
        {
            InitializeComponent();
            if (this.Name == "BtnEdit")
            {
                this.SetIcon(global::CarSensX.Properties.Resources.iconEdit);
            }
            else if (this.Name == "BtnFullscreen")
            {
                this.SetIcon(global::CarSensX.Properties.Resources.iconFullScreen);
            }

            this.MouseEnter += new MouseEventHandler(MouseEnterEvt);
            this.MouseLeave += new MouseEventHandler(MouseLeaveEvt);
        }

        private void MouseEnterEvt(object sender, MouseEventArgs e)
        {
           SolidColorBrush scb = new SolidColorBrush(SystemColors.InactiveCaptionColor);
            this.returnColor = mainGrid.Background;
            this.mainGrid.Background = scb;
        }

        private void MouseLeaveEvt(object sender, MouseEventArgs e)
        {
            this.mainGrid.Background = returnColor;
        }

        public void SetIcon(System.Drawing.Bitmap bitmap)
        {
            Icon.Source = Static.getBitmapSourceFromBitmap(bitmap);
        }

        public void SetLabel(String name)
        {
            this.LblName.Content = name;
        }
    }
}
