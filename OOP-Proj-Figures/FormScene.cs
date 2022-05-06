using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Figures.ClassLibraryFigures;
using Rectangle = Figures.ClassLibraryFigures.Rectangle;
using Circle = Figures.ClassLibraryFigures.Circle;
using Triangle = Figures.ClassLibraryFigures.Triangle;

namespace Figures
{
    public partial class FormScene : Form
    {
        public FormScene()
        {
            InitializeComponent();

            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer,
                true);
        }

        private List<Figure> _figures = new List<Figure>();

        private bool _captureMouse;
        private Point _captureMouseLocation;
        private Rectangle _frame;

        private int _mode = 0;

        private Figure _selected;
        private int _mouseMoveDifX = 0;
        private int _mouseMoveDifY = 0;

        private string currentScene;

        //custom
        private void OpenProperties()
        {
            var figure = _figures
                .FirstOrDefault(r => r.Selected);

            if(figure != null)
            {
                if(figure is Circle)
                {
                    var fp = new FormProperties(FormProperties.Style.Circle);
                    fp.MyWidth = figure.Width;
                    fp.MyHeight = figure.Height;
                    fp.MyColor = figure.BackColor;

                    if (fp.ShowDialog() == DialogResult.OK)
                    {
                        figure.Width = fp.MyWidth.Value;
                        figure.Height = fp.MyHeight.Value;
                        figure.BackColor = fp.MyColor;

                        CalculateArea();
                    }
                }
                else
                {
                    var fp = new FormProperties(FormProperties.Style.Rectangle);
                    fp.MyWidth = figure.Width;
                    fp.MyHeight = figure.Height;
                    fp.MyColor = figure.BackColor;

                    if (fp.ShowDialog() == DialogResult.OK)
                    {
                        figure.Width = fp.MyWidth.Value;
                        figure.Height = fp.MyHeight.Value;
                        figure.BackColor = fp.MyColor;

                        CalculateArea();
                    }
                }
            }
            //Invalidate();
        }

        private void Figure_onClick()
        {
            Invalidate();
        }

        private void Figure_onLocationChanged()
        {
            Invalidate();
        }

        private void Figure_onSizeChanged()
        {
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            for (int i = _figures.Count - 1; i >= 0; i--)
                _figures[i].Paint(e.Graphics);

            if (_captureMouse && _frame != null)
                _frame.Paint(e.Graphics);
        }

        private Rectangle CreateReactangelFrame(
            Point location)
        {
            var frame = new Rectangle
            {
                Location = new Point(
                    Math.Min(_captureMouseLocation.X, location.X),
                    Math.Min(_captureMouseLocation.Y, location.Y)),
                Width =
                    Math.Abs(_captureMouseLocation.X - location.X),
                Height =
                    Math.Abs(_captureMouseLocation.Y - location.Y)
            };

            return frame;
        }

        private void CalculateArea()
        {
            labelArea.Text = _figures
                .Sum(s => s.GetArea()).ToString();
        }

        private void DeleteSelected()
        {
            for (int i = _figures.Count - 1; i >= 0; i--)
                if (_figures[i].Selected)
                    _figures.RemoveAt(i);

            CalculateArea();

            Invalidate();
        }
        //custom

        // autogen
        private void FormScene_MouseDown(object sender, MouseEventArgs e)
        {
            _captureMouse = true;
            _captureMouseLocation = e.Location;
            _frame = null;

            foreach (var figure in _figures)
                figure.Selected = false;

            if (e.Button == MouseButtons.Left)
            {
                var selectedFigure = _figures
                    .FirstOrDefault(r => r.ContainsPoint(e.Location));

                if (selectedFigure != null)
                {
                    selectedFigure.Selected = true;
                }
            }

            _selected = _figures
                .FirstOrDefault(r => r.Selected == true);

            if (_selected != null && e.Button == MouseButtons.Left)
            {
                _mouseMoveDifX = e.Location.X - _selected.Location.X;
                _mouseMoveDifY = e.Location.Y - _selected.Location.Y;
            }


            Invalidate();
        }

        private void FormScene_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_captureMouse)//exit if mouse isn't captured
                return;
            if (_selected == null)
            {
                _frame = CreateReactangelFrame(e.Location);//create frame
                _frame.Filled = false;
                _frame.BackColor = Color.LightGray;

                if (_mode == 2 && e.Button == MouseButtons.Right)
                    _frame.Height = _frame.Width;

                if(e.Button == MouseButtons.Left)
                    foreach (var rectangle in _figures
                        .Where(r => r.Intersects(_frame)))
                        rectangle.Selected = true;
            }
            else if(e.Button == MouseButtons.Left)
            {
                _selected.Move(e.Location.X - _mouseMoveDifX, e.Location.Y - _mouseMoveDifY);

            }

            Invalidate();
        }

        private void FormScene_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_captureMouse)//exit if mouse isn't captured
                return;
            if (_selected == null)
            {
                if (_mode == 0)
                {
                    if (e.Button == MouseButtons.Right && e.Location.X != _captureMouseLocation.X && _frame != null)
                    {
                        _frame.Filled = true;
                        _frame.Selected = true;
                        _frame.BackColor = Color.Blue;
                        _frame.onClick += Figure_onClick;
                        _frame.onLocationChanged += Figure_onLocationChanged;
                        _frame.onSizeChanged += Figure_onSizeChanged;

                        _figures.Insert(0, _frame);
                    }
                }
                else if (_mode == 1)
                {
                    if (e.Button == MouseButtons.Right && e.Location.X != _captureMouseLocation.X && _frame != null)
                    {
                        Triangle tr = new Triangle();
                        tr.Filled = true;
                        tr.Selected = true;
                        tr.BackColor = Color.Blue;
                        tr.Location = _frame.Location;
                        tr.Width = _frame.Width;
                        tr.Height = _frame.Height;
                        tr.onClick += Figure_onClick;
                        tr.onLocationChanged += Figure_onLocationChanged;
                        tr.onSizeChanged += Figure_onSizeChanged;

                        _figures.Insert(0, tr);
                    }
                }
                else if (_mode == 2)
                {
                    if (e.Button == MouseButtons.Right && e.Location.X != _captureMouseLocation.X && _frame != null)
                    {
                        Circle cr = new Circle();
                        cr.Filled = true;
                        cr.Selected = true;
                        cr.BackColor = Color.Blue;
                        cr.Location = _frame.Location;
                        cr.Width = _frame.Width;
                        cr.Height = _frame.Height;
                        cr.onClick += Figure_onClick;
                        cr.onLocationChanged += Figure_onLocationChanged;
                        cr.onSizeChanged += Figure_onSizeChanged;

                        _figures.Insert(0, cr);
                    }
                }
            }
            else
            {
                _selected = null;
            }

            CalculateArea();

            _captureMouse = false;

            Invalidate();
        }

        private void FormScene_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete)
                return;

            DeleteSelected();
        }

        private void rectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mode = 0;
        }

        private void triangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mode = 1;
        }

        private void circleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mode = 2;
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSelected();
        }

        private void editToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            var selectedCount = _figures
                .Count(r => r.Selected);

            deleteToolStripMenuItem.Enabled = _figures
                .Any(r => r.Selected);

            propertiesToolStripMenuItem.Enabled = selectedCount == 1;
        }

        private void FormScene_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OpenProperties();
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenProperties();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSaveLoadScene fsl = new FormSaveLoadScene(FormSaveLoadScene.Style.Save);
            fsl.Figures = _figures;
            if (fsl.ShowDialog() == DialogResult.OK)
            {
                _figures = fsl.Figures;
                currentScene = fsl.scenePath;
            }
            Invalidate();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSaveLoadScene.SaveFile(currentScene, _figures);
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSaveLoadScene fsl = new FormSaveLoadScene(FormSaveLoadScene.Style.Load);
            if (fsl.ShowDialog() == DialogResult.OK)
            {
                _figures = fsl.Figures;
                currentScene = fsl.scenePath;
            }

            CalculateArea();

            Invalidate();
        }

        private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            saveToolStripMenuItem.Enabled = currentScene != null;

            saveAsToolStripMenuItem.Enabled = _figures.Any();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_figures.Any())
                if (MessageBox.Show("Are you sure you want to create a new scene?","Start New Scene",MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

            _figures.Clear();
            currentScene = null;

            Invalidate();
        }
        //autogen
    }
}
