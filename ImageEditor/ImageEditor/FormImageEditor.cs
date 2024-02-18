using ImageEditor.ProgramLogic;
using System.Diagnostics;
using System.Windows.Forms.DataVisualization.Charting;

namespace ImageEditor
{
    public partial class FormImageEditor : Form
    {
        ImageProcessing? imageProcessing;

        readonly ImageProcessorCSharp imageProcessorCSharp = new ImageProcessorCSharp();
        readonly ImageProcessorASM? imageProcessorASM;
        readonly ImageProcessorCpp? imageProcessorCpp;

        public FormImageEditor()
        {
            InitializeComponent();

            // Check if ASM and Cpp dlls are available
            bool isAssemblyAvailable = false;
            bool isCppAvailable = false;
            try
            {
                imageProcessorASM = new ImageProcessorASM();
                isAssemblyAvailable = true;
            }
            catch (DllNotFoundException)
            {
                imageProcessorASM = null;
                comboBoxProgLang.Items.Remove("Assembly");
            }
            try
            {
                imageProcessorCpp = new ImageProcessorCpp();
                isCppAvailable = true;
            }
            catch (DllNotFoundException)
            {
                imageProcessorCpp = null;
                comboBoxProgLang.Items.Remove("C++");
            }
            if (!isAssemblyAvailable && !isCppAvailable)
            {
                MessageBox.Show("No assembly nor C++ dll found." + Environment.NewLine + "Only C# processing will be available" +
                    Environment.NewLine + "Benchmark will be also unavailable", "Error: no dll found");
                buttonBenchmark.Enabled = false;
            }
            else if (!isAssemblyAvailable)
            {
                MessageBox.Show("No assembly dll found." + Environment.NewLine + "Only C# and C++ processing will be available" +
                    Environment.NewLine + "Benchmark will be also unavailable", "Error: no dll found");
                buttonBenchmark.Enabled = false;
            }
            else if (!isCppAvailable)
            {
                MessageBox.Show("No C++ dll found." + Environment.NewLine + "Only C# and Assembly processing will be available" +
                    Environment.NewLine + "Benchmark will be also unavailable", "Error: no dll found");
                buttonBenchmark.Enabled = false;
            }

        }

        private void FormImageEditor_Load(object sender, EventArgs e)
        {
            comboBoxProgLang.Text = "C#";
            comboBoxNoThreads.Text = String.Format("{0}", Environment.ProcessorCount);
            comboBoxHistogram.SelectedIndex = 0;
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image Files(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    imageBefore.Load(openFileDialog1.FileName);
                }
                catch
                {
                    MessageBox.Show("Please select another file", "Error: file cannot be loaded");
                }
                textBoxFilename.Text = openFileDialog1.SafeFileName;
            }
            else
            {
                MessageBox.Show("Please select a file", "Error");
            }
        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
            labelTrackBarValue.Text = (trackBar.Value / 100.0f).ToString();
        }

        private void buttonProcess_Click(object sender, EventArgs e)
        {
            if (imageBefore.Image == null)
            {
                MessageBox.Show("Please load an image first", "Error: no image");
                return;
            }

            try
            {
                imageProcessing ??= new ImageProcessing(new Bitmap(imageBefore.Image), imageProcessorCSharp);
                if (imageBefore.Image != imageProcessing.ImageToProcess)
                    imageProcessing.ImageToProcess = new Bitmap(imageBefore.Image);
            }
            catch
            {
                MessageBox.Show("This image cannot be processed", "Error");
                return;
            }

            switch (comboBoxProgLang.Text)
            {
                case "C++":
                    imageProcessing.SetImageProcessor(imageProcessorCpp);
                    break;
                case "C#":
                    imageProcessing.SetImageProcessor(imageProcessorCSharp);
                    break;
                case "Assembly":
                    imageProcessing.SetImageProcessor(imageProcessorASM);
                    break;
                default:
                    MessageBox.Show("Please select a programming language", "Error: no programming language selected");
                    return;
            }

            var selected = panelRButtons.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            if (selected == null)
            {
                MessageBox.Show("Please select a filter", "Error: no filter selected");
                return;
            }

            int numberOfThreads = Environment.ProcessorCount;
            if (comboBoxNoThreads.SelectedIndex != -1)
                numberOfThreads = comboBoxNoThreads.SelectedIndex + 1;

            long processTime = 0;
            switch (selected.Text)
            {
                case "Brightness":
                    processTime = imageProcessing.ChangeBrightness(trackBar.Value / 100.0f, numberOfThreads);
                    break;
                case "Contrast":
                    processTime = imageProcessing.ChangeContrast(trackBar.Value / 10.0f, numberOfThreads);
                    break;
                case "Transparency":
                    processTime = imageProcessing.ChangeTransparency(1.0f - (trackBar.Value / 100.0f), numberOfThreads);
                    break;
                case "Grayscale":
                    processTime = imageProcessing.MakeGrayScale(numberOfThreads);
                    break;
                case "Sepia":
                    processTime = imageProcessing.MakeSepia(numberOfThreads);
                    break;
                case "Negative":
                    processTime = imageProcessing.MakeNegative(numberOfThreads);
                    break;
            }

            processTime = (long)((double)processTime / Stopwatch.Frequency * 1000000);
            textBoxProcessTime.Text = processTime.ToString() + "us";
            imageAfter.Image = imageProcessing.ProcessedImage;
        }

        private void RadioClick(object sender, EventArgs e)
        {
            var selected = panelRButtons.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            if (selected == null)
                return;
            switch (selected.Text)
            {
                case "Brightness":
                    showTrackBarAndLabels();
                    trackBar.Minimum = -100;
                    trackBar.Maximum = 100;
                    trackBar.Value = 0;
                    break;
                case "Contrast":
                    showTrackBarAndLabels();
                    trackBar.Minimum = 1;
                    trackBar.Maximum = 100;
                    trackBar.Value = 1;
                    break;
                case "Transparency":
                    showTrackBarAndLabels();
                    trackBar.Minimum = 0;
                    trackBar.Maximum = 100;
                    trackBar.Value = 0;
                    break;
                case "Grayscale":
                    trackBar.Visible = false;
                    labelTrackBarValue.Visible = false;
                    labelTrackBar.Visible = false;
                    break;
                case "Negative":
                    trackBar.Visible = false;
                    labelTrackBarValue.Visible = false;
                    labelTrackBar.Visible = false;
                    break;
                case "Sepia":
                    trackBar.Visible = false;
                    labelTrackBarValue.Visible = false;
                    labelTrackBar.Visible = false;
                    break;
            }
            labelTrackBarValue.Text = (trackBar.Value / 100.0f).ToString();
        }

        private void showTrackBarAndLabels()
        {
            trackBar.Visible = true;
            labelTrackBarValue.Visible = true;
            labelTrackBar.Visible = true;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.DefaultExt = "png";
            saveFileDialog1.Filter = "Image Files(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG|All files (*.*)|*.*";
            saveFileDialog1.Title = "Save an Image File";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                imageAfter.Image.Save(saveFileDialog1.FileName);
            }
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            if (imageAfter.Image == null)
            {
                MessageBox.Show("Please process input image first", "Error: no processed image");
                return;
            }
            imageBefore.Image = imageAfter.Image;
        }

        private void buttonBenchmark_Click(object sender, EventArgs e)
        {
            if (imageBefore.Image == null)
            {
                MessageBox.Show("Please load an image first", "Error: no image");
                return;
            }

            var selected = panelRButtons.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            if (selected == null)
            {
                MessageBox.Show("Please select a filter", "Error: no filter selected");
                return;
            }

            try
            {
                imageProcessing ??= new ImageProcessing(new Bitmap(imageBefore.Image), imageProcessorCSharp);
            }
            catch
            {
                MessageBox.Show("This image cannot be processed", "Error");
                return;
            }

            Benchmark benchmark = new Benchmark(progressBarBenchmark);
            progressBarBenchmark.Visible = true;

            switch (selected.Text)
            {
                case "Brightness":
                    benchmark.RunBenchmark(new Bitmap(imageBefore.Image), Benchmark.FunctionsToBenchmark.brightnessChange, trackBar.Value / 100.0f);
                    break;
                case "Contrast":
                    benchmark.RunBenchmark(new Bitmap(imageBefore.Image), Benchmark.FunctionsToBenchmark.contrastChange, trackBar.Value / 10.0f);
                    break;
                case "Transparency":
                    benchmark.RunBenchmark(new Bitmap(imageBefore.Image), Benchmark.FunctionsToBenchmark.transparencyChange, 1.0f - (trackBar.Value / 100.0f));
                    break;
                case "Grayscale":
                    benchmark.RunBenchmark(new Bitmap(imageBefore.Image), Benchmark.FunctionsToBenchmark.grayscale);
                    break;
                case "Sepia":
                    benchmark.RunBenchmark(new Bitmap(imageBefore.Image), Benchmark.FunctionsToBenchmark.sepia);
                    break;
                case "Negative":
                    benchmark.RunBenchmark(new Bitmap(imageBefore.Image), Benchmark.FunctionsToBenchmark.negative);
                    break;
            }

            MessageBox.Show("Benchmark finished", "Benchmark");
            progressBarBenchmark.Visible = false;
            textBoxProcessTime.Text = "0us";
        }

        private void buttonHistogram_Click(object sender, EventArgs e)
        {
            if (comboBoxHistogram.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a histogram of which image you want", "Error: no histogram selected");
                return;
            }

            try
            {
                imageProcessing ??= new ImageProcessing(new Bitmap(imageBefore.Image), imageProcessorCSharp);
            }
            catch
            {
                MessageBox.Show("This image cannot be processed", "Error");
                return;
            }

            if (comboBoxHistogram.Text == "Both")
            {
                if (imageBefore.Image == null || imageAfter.Image == null)
                {
                    MessageBox.Show("Please load and process image first", "Error: no image");
                    return;
                }
                Histogram histogramBefore = new Histogram(new Bitmap(imageBefore.Image));
                histogramBefore.showHistogram("Histogram Image Before");
                Histogram histogramAfter = new Histogram(new Bitmap(imageAfter.Image));
                histogramAfter.showHistogram("Histogram Image After");
                return;
            }
            else
            {
                switch (comboBoxHistogram.Text)
                {
                    case "Before":
                        if (imageBefore.Image == null)
                        {
                            MessageBox.Show("Please load image first", "Error: no image");
                            return;
                        }
                        Histogram histogramBefore = new Histogram(new Bitmap(imageBefore.Image));
                        histogramBefore.showHistogram("Histogram Image Before");
                        break;
                    case "After":
                        if (imageAfter.Image == null)
                        {
                            MessageBox.Show("Please process image first", "Error: no image");
                            return;
                        }
                        Histogram histogramAfter = new Histogram(new Bitmap(imageAfter.Image));
                        histogramAfter.showHistogram("Histogram Image After");
                        break;
                }
            }
        }
    }
}