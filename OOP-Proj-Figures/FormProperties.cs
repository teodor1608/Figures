using System;
using System.Drawing;
using System.Windows.Forms;

namespace Figures
{
    public partial class FormProperties : Form
    {
        public enum Style
        {
            Rectangle,
            Triangle,
            Circle
        }

        private int mode = 0;

        public FormProperties(Style style)
        {
            InitializeComponent();
            switch(style)
            {
                case Style.Rectangle:
                    mode = 0;
                    break;
                case Style.Triangle:
                    mode = 1;
                    break;
                case Style.Circle:
                    mode = 2;
                    textBoxH.Enabled = false;
                    break;
            }
        }

        private int? _width;
        public int? MyWidth
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                textBoxW.Text = _width.ToString();
            }
        }

        private int? _height;
        public int? MyHeight
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
                textBoxH.Text = _height.ToString();
            }
        }

        private Color _color;
        public Color MyColor
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                buttonColor.BackColor = _color;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            _width = int.TryParse(textBoxW.Text, out int width)
                ? width
                : (int?)null;

            if(mode == 2)
                _height = int.TryParse(textBoxW.Text, out int height)
                ? height
                : (int?)null;
            else
                _height = int.TryParse(textBoxH.Text, out int height)
                ? height
                : (int?)null;
        }

        private void buttonColor_Click(object sender, EventArgs e)
        {
            var cd = new ColorDialog();
            MyColor = cd.ShowDialog() == DialogResult.OK
                ? cd.Color
                : Color.Blue;
        }

        private void textBoxW_TextChanged(object sender, EventArgs e)
        {
            if (textBoxW.Text == "" || textBoxH.Text == "" || textBoxW.Text == "0" || textBoxH.Text == "0")
                buttonOK.Enabled = false;
            else
                buttonOK.Enabled = true;

            if (mode == 2)
                textBoxH.Text = textBoxW.Text;
        }

        private void textBoxH_TextChanged(object sender, EventArgs e)
        {
            if (textBoxW.Text == "" || textBoxH.Text == "" || textBoxW.Text == "0" || textBoxH.Text == "0")
                buttonOK.Enabled = false;
            else
                buttonOK.Enabled = true;
        }
    }
}
