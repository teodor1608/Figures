using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Figures.ClassLibraryFigures;

namespace Figures
{
    public partial class FormSaveLoadScene : Form
    {
        public enum Style
        {
            Load,
            Save
        }

        public FormSaveLoadScene(Style style)
        {
            InitializeComponent();
            switch (style)
            {
                case Style.Load:
                    Text = "Load Scene";
                    buttonSave.Hide();
                    break;
                case Style.Save:
                    Text = "Save Scene";
                    buttonLoad.Hide();
                    break;
            }
        }

        public List<Figure> Figures = new List<Figure>();

        public string scenePath;

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.FileName = "myScene";
            sfd.DefaultExt = "gita";//useless ext - makes no diff
            sfd.AddExtension = true;
            sfd.Filter =
            "Gita files (*.gita)|*.gita|All files (*.*)|*.*";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string filePath = sfd.FileName;

                buttonCancel.Enabled = false;

                textBoxFileName.Text = sfd.FileName;

                scenePath = filePath;

                textBoxStatus.AppendText(Environment.NewLine + $"Saved Scene of {Figures.Count} figures at {filePath}...");

                SaveFile(filePath, Figures);
            }
        }

        public static void SaveFile(string filePath, List<Figure> fileFigures)
        {
            var formatter = new BinaryFormatter();

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                formatter.Serialize(stream, fileFigures);
            }
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            //dialog
            var ofd = new OpenFileDialog();

            ofd.Filter =
            "Gita files (*.gita)|*.gita|All files (*.*)|*.*";

            string filePath = "";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                filePath = ofd.FileName;

                scenePath = ofd.FileName;

                textBoxFileName.Text = ofd.FileName;
            }
            //

            if (!File.Exists(filePath))
                return;

            LoadFile(filePath);

            textBoxStatus.AppendText(Environment.NewLine + $"Loaded File: {filePath}");
        }

        private void LoadFile(string filePath)
        {
            var formatter = new BinaryFormatter();

            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                Figures = (List<Figure>)formatter.Deserialize(stream);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Figures = null;
            DialogResult = DialogResult.Cancel;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
