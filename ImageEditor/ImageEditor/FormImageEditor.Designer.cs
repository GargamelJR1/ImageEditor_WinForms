namespace ImageEditor
{
    partial class FormImageEditor
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormImageEditor));
            imageBefore = new PictureBox();
            imageAfter = new PictureBox();
            labelBefore = new Label();
            labelAfter = new Label();
            buttonSave = new Button();
            trackBar = new TrackBar();
            labelTrackBar = new Label();
            bindingSource1 = new BindingSource(components);
            openFileDialog1 = new OpenFileDialog();
            buttonLoad = new Button();
            labelTrackBarValue = new Label();
            rButtonBrightness = new RadioButton();
            rButtonContrast = new RadioButton();
            rButtonTransparency = new RadioButton();
            buttonProcess = new Button();
            panelRButtons = new Panel();
            RButtonNegative = new RadioButton();
            rButtonSepia = new RadioButton();
            rButtonGrayScale = new RadioButton();
            buttonApply = new Button();
            saveFileDialog1 = new SaveFileDialog();
            textBoxFilename = new TextBox();
            textBoxProcessTime = new TextBox();
            labelProcessTime = new Label();
            comboBoxProgLang = new ComboBox();
            comboBoxNoThreads = new ComboBox();
            buttonBenchmark = new Button();
            progressBarBenchmark = new ProgressBar();
            labelProgLang = new Label();
            labelNoThreads = new Label();
            buttonHistogram = new Button();
            comboBoxHistogram = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)imageBefore).BeginInit();
            ((System.ComponentModel.ISupportInitialize)imageAfter).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).BeginInit();
            panelRButtons.SuspendLayout();
            SuspendLayout();
            // 
            // imageBefore
            // 
            imageBefore.BorderStyle = BorderStyle.FixedSingle;
            imageBefore.ImageLocation = "";
            imageBefore.Location = new Point(13, 143);
            imageBefore.Name = "imageBefore";
            imageBefore.Size = new Size(575, 575);
            imageBefore.SizeMode = PictureBoxSizeMode.Zoom;
            imageBefore.TabIndex = 0;
            imageBefore.TabStop = false;
            // 
            // imageAfter
            // 
            imageAfter.BorderStyle = BorderStyle.FixedSingle;
            imageAfter.Location = new Point(623, 143);
            imageAfter.Name = "imageAfter";
            imageAfter.Size = new Size(575, 575);
            imageAfter.SizeMode = PictureBoxSizeMode.Zoom;
            imageAfter.TabIndex = 1;
            imageAfter.TabStop = false;
            // 
            // labelBefore
            // 
            labelBefore.AutoSize = true;
            labelBefore.Location = new Point(14, 120);
            labelBefore.Name = "labelBefore";
            labelBefore.Size = new Size(53, 20);
            labelBefore.TabIndex = 6;
            labelBefore.Text = "before";
            // 
            // labelAfter
            // 
            labelAfter.AutoSize = true;
            labelAfter.Location = new Point(623, 120);
            labelAfter.Name = "labelAfter";
            labelAfter.Size = new Size(40, 20);
            labelAfter.TabIndex = 7;
            labelAfter.Text = "after";
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(1107, 52);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(91, 28);
            buttonSave.TabIndex = 8;
            buttonSave.Text = "Save";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // trackBar
            // 
            trackBar.AutoSize = false;
            trackBar.Location = new Point(13, 52);
            trackBar.Maximum = 100;
            trackBar.Name = "trackBar";
            trackBar.Size = new Size(401, 56);
            trackBar.TabIndex = 9;
            trackBar.Scroll += trackBar_Scroll;
            // 
            // labelTrackBar
            // 
            labelTrackBar.AutoSize = true;
            labelTrackBar.Location = new Point(178, 83);
            labelTrackBar.Name = "labelTrackBar";
            labelTrackBar.Size = new Size(66, 20);
            labelTrackBar.TabIndex = 10;
            labelTrackBar.Text = "strength:";
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // buttonLoad
            // 
            buttonLoad.Location = new Point(1107, 19);
            buttonLoad.Name = "buttonLoad";
            buttonLoad.Size = new Size(91, 28);
            buttonLoad.TabIndex = 12;
            buttonLoad.Text = "Load";
            buttonLoad.UseVisualStyleBackColor = true;
            buttonLoad.Click += buttonLoad_Click;
            // 
            // labelTrackBarValue
            // 
            labelTrackBarValue.AutoSize = true;
            labelTrackBarValue.Location = new Point(235, 83);
            labelTrackBarValue.Name = "labelTrackBarValue";
            labelTrackBarValue.Size = new Size(17, 20);
            labelTrackBarValue.TabIndex = 15;
            labelTrackBarValue.Text = "0";
            // 
            // rButtonBrightness
            // 
            rButtonBrightness.AutoSize = true;
            rButtonBrightness.Location = new Point(3, 3);
            rButtonBrightness.Name = "rButtonBrightness";
            rButtonBrightness.Size = new Size(95, 24);
            rButtonBrightness.TabIndex = 16;
            rButtonBrightness.TabStop = true;
            rButtonBrightness.Text = "Brightness";
            rButtonBrightness.UseVisualStyleBackColor = true;
            rButtonBrightness.Click += RadioClick;
            // 
            // rButtonContrast
            // 
            rButtonContrast.AutoSize = true;
            rButtonContrast.Location = new Point(223, 3);
            rButtonContrast.Name = "rButtonContrast";
            rButtonContrast.Size = new Size(82, 24);
            rButtonContrast.TabIndex = 17;
            rButtonContrast.TabStop = true;
            rButtonContrast.Text = "Contrast";
            rButtonContrast.UseVisualStyleBackColor = true;
            rButtonContrast.Click += RadioClick;
            // 
            // rButtonTransparency
            // 
            rButtonTransparency.AutoSize = true;
            rButtonTransparency.Location = new Point(104, 3);
            rButtonTransparency.Name = "rButtonTransparency";
            rButtonTransparency.Size = new Size(113, 24);
            rButtonTransparency.TabIndex = 18;
            rButtonTransparency.TabStop = true;
            rButtonTransparency.Text = "Transparency";
            rButtonTransparency.UseVisualStyleBackColor = true;
            rButtonTransparency.Click += RadioClick;
            // 
            // buttonProcess
            // 
            buttonProcess.Location = new Point(913, 19);
            buttonProcess.Name = "buttonProcess";
            buttonProcess.Size = new Size(91, 28);
            buttonProcess.TabIndex = 19;
            buttonProcess.Text = "Process";
            buttonProcess.UseVisualStyleBackColor = true;
            buttonProcess.Click += buttonProcess_Click;
            // 
            // panelRButtons
            // 
            panelRButtons.Controls.Add(RButtonNegative);
            panelRButtons.Controls.Add(rButtonTransparency);
            panelRButtons.Controls.Add(rButtonBrightness);
            panelRButtons.Controls.Add(rButtonSepia);
            panelRButtons.Controls.Add(rButtonContrast);
            panelRButtons.Controls.Add(rButtonGrayScale);
            panelRButtons.Location = new Point(13, 13);
            panelRButtons.Name = "panelRButtons";
            panelRButtons.Size = new Size(571, 33);
            panelRButtons.TabIndex = 21;
            // 
            // RButtonNegative
            // 
            RButtonNegative.AutoSize = true;
            RButtonNegative.Location = new Point(477, 3);
            RButtonNegative.Name = "RButtonNegative";
            RButtonNegative.Size = new Size(87, 24);
            RButtonNegative.TabIndex = 21;
            RButtonNegative.TabStop = true;
            RButtonNegative.Text = "Negative";
            RButtonNegative.UseVisualStyleBackColor = true;
            RButtonNegative.Click += RadioClick;
            // 
            // rButtonSepia
            // 
            rButtonSepia.AutoSize = true;
            rButtonSepia.Location = new Point(407, 3);
            rButtonSepia.Name = "rButtonSepia";
            rButtonSepia.Size = new Size(64, 24);
            rButtonSepia.TabIndex = 20;
            rButtonSepia.TabStop = true;
            rButtonSepia.Text = "Sepia";
            rButtonSepia.UseVisualStyleBackColor = true;
            rButtonSepia.Click += RadioClick;
            // 
            // rButtonGrayScale
            // 
            rButtonGrayScale.AutoSize = true;
            rButtonGrayScale.Location = new Point(311, 3);
            rButtonGrayScale.Name = "rButtonGrayScale";
            rButtonGrayScale.Size = new Size(90, 24);
            rButtonGrayScale.TabIndex = 19;
            rButtonGrayScale.TabStop = true;
            rButtonGrayScale.Text = "Grayscale";
            rButtonGrayScale.UseVisualStyleBackColor = true;
            rButtonGrayScale.Click += RadioClick;
            // 
            // buttonApply
            // 
            buttonApply.Location = new Point(1010, 19);
            buttonApply.Name = "buttonApply";
            buttonApply.Size = new Size(91, 28);
            buttonApply.TabIndex = 22;
            buttonApply.Text = "Apply";
            buttonApply.UseVisualStyleBackColor = true;
            buttonApply.Click += buttonApply_Click;
            // 
            // textBoxFilename
            // 
            textBoxFilename.Location = new Point(623, 87);
            textBoxFilename.Name = "textBoxFilename";
            textBoxFilename.ReadOnly = true;
            textBoxFilename.Size = new Size(370, 27);
            textBoxFilename.TabIndex = 23;
            textBoxFilename.Text = "filename";
            // 
            // textBoxProcessTime
            // 
            textBoxProcessTime.BorderStyle = BorderStyle.None;
            textBoxProcessTime.Location = new Point(917, 53);
            textBoxProcessTime.Name = "textBoxProcessTime";
            textBoxProcessTime.ReadOnly = true;
            textBoxProcessTime.Size = new Size(87, 20);
            textBoxProcessTime.TabIndex = 24;
            textBoxProcessTime.Text = "0s";
            // 
            // labelProcessTime
            // 
            labelProcessTime.AutoSize = true;
            labelProcessTime.Location = new Point(816, 53);
            labelProcessTime.Name = "labelProcessTime";
            labelProcessTime.Size = new Size(95, 20);
            labelProcessTime.TabIndex = 25;
            labelProcessTime.Text = "Process time:";
            // 
            // comboBoxProgLang
            // 
            comboBoxProgLang.FormattingEnabled = true;
            comboBoxProgLang.Items.AddRange(new object[] { "C#", "C++", "Assembly" });
            comboBoxProgLang.Location = new Point(709, 19);
            comboBoxProgLang.Name = "comboBoxProgLang";
            comboBoxProgLang.Size = new Size(102, 28);
            comboBoxProgLang.TabIndex = 26;
            // 
            // comboBoxNoThreads
            // 
            comboBoxNoThreads.FormattingEnabled = true;
            comboBoxNoThreads.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50", "51", "52", "53", "54", "55", "56", "57", "58", "59", "60", "61", "62", "63", "64" });
            comboBoxNoThreads.Location = new Point(709, 52);
            comboBoxNoThreads.Name = "comboBoxNoThreads";
            comboBoxNoThreads.Size = new Size(102, 28);
            comboBoxNoThreads.TabIndex = 27;
            // 
            // buttonBenchmark
            // 
            buttonBenchmark.Location = new Point(816, 19);
            buttonBenchmark.Name = "buttonBenchmark";
            buttonBenchmark.Size = new Size(91, 28);
            buttonBenchmark.TabIndex = 28;
            buttonBenchmark.Text = "Benchmark";
            buttonBenchmark.UseVisualStyleBackColor = true;
            buttonBenchmark.Click += buttonBenchmark_Click;
            // 
            // progressBarBenchmark
            // 
            progressBarBenchmark.Location = new Point(816, 52);
            progressBarBenchmark.Name = "progressBarBenchmark";
            progressBarBenchmark.Size = new Size(285, 28);
            progressBarBenchmark.TabIndex = 29;
            progressBarBenchmark.UseWaitCursor = true;
            progressBarBenchmark.Visible = false;
            // 
            // labelProgLang
            // 
            labelProgLang.AutoSize = true;
            labelProgLang.Location = new Point(619, 21);
            labelProgLang.Name = "labelProgLang";
            labelProgLang.Size = new Size(83, 20);
            labelProgLang.TabIndex = 30;
            labelProgLang.Text = "P language";
            // 
            // labelNoThreads
            // 
            labelNoThreads.AutoSize = true;
            labelNoThreads.Location = new Point(619, 55);
            labelNoThreads.Name = "labelNoThreads";
            labelNoThreads.Size = new Size(85, 20);
            labelNoThreads.TabIndex = 31;
            labelNoThreads.Text = "No Threads";
            // 
            // buttonHistogram
            // 
            buttonHistogram.Location = new Point(1107, 87);
            buttonHistogram.Margin = new Padding(3, 4, 3, 4);
            buttonHistogram.Name = "buttonHistogram";
            buttonHistogram.Size = new Size(91, 28);
            buttonHistogram.TabIndex = 33;
            buttonHistogram.Text = "Histogram";
            buttonHistogram.UseVisualStyleBackColor = true;
            buttonHistogram.Click += buttonHistogram_Click;
            // 
            // comboBoxHistogram
            // 
            comboBoxHistogram.FormattingEnabled = true;
            comboBoxHistogram.Items.AddRange(new object[] { "Before", "After", "Both" });
            comboBoxHistogram.Location = new Point(999, 87);
            comboBoxHistogram.Name = "comboBoxHistogram";
            comboBoxHistogram.Size = new Size(102, 28);
            comboBoxHistogram.TabIndex = 34;
            // 
            // FormImageEditor
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1211, 727);
            Controls.Add(comboBoxHistogram);
            Controls.Add(buttonHistogram);
            Controls.Add(labelNoThreads);
            Controls.Add(labelProgLang);
            Controls.Add(progressBarBenchmark);
            Controls.Add(buttonBenchmark);
            Controls.Add(comboBoxNoThreads);
            Controls.Add(comboBoxProgLang);
            Controls.Add(labelProcessTime);
            Controls.Add(textBoxProcessTime);
            Controls.Add(textBoxFilename);
            Controls.Add(buttonApply);
            Controls.Add(panelRButtons);
            Controls.Add(buttonProcess);
            Controls.Add(labelTrackBarValue);
            Controls.Add(buttonLoad);
            Controls.Add(labelTrackBar);
            Controls.Add(trackBar);
            Controls.Add(buttonSave);
            Controls.Add(labelAfter);
            Controls.Add(labelBefore);
            Controls.Add(imageAfter);
            Controls.Add(imageBefore);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "FormImageEditor";
            Text = "ImageEditorProPlus2024+++";
            Load += FormImageEditor_Load;
            ((System.ComponentModel.ISupportInitialize)imageBefore).EndInit();
            ((System.ComponentModel.ISupportInitialize)imageAfter).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).EndInit();
            panelRButtons.ResumeLayout(false);
            panelRButtons.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox imageBefore;
        private PictureBox imageAfter;
        private Label labelBefore;
        private Label labelAfter;
        private Button buttonSave;
        private TrackBar trackBar;
        private Label labelTrackBar;
        private BindingSource bindingSource1;
        private OpenFileDialog openFileDialog1;
        private Button buttonLoad;
        private Label labelTrackBarValue;
        private RadioButton rButtonBrightness;
        private RadioButton rButtonContrast;
        private RadioButton rButtonTransparency;
        private Button buttonProcess;
        private Panel panelRButtons;
        private Button buttonApply;
        private SaveFileDialog saveFileDialog1;
        private RadioButton rButtonGrayScale;
        private RadioButton RButtonNegative;
        private RadioButton rButtonSepia;
        private TextBox textBoxFilename;
        private TextBox textBoxProcessTime;
        private Label labelProcessTime;
        private ComboBox comboBoxProgLang;
        private ComboBox comboBoxNoThreads;
        private Button buttonBenchmark;
        private ProgressBar progressBarBenchmark;
        private Label labelProgLang;
        private Label labelNoThreads;
        private Button buttonHistogram;
        private ComboBox comboBoxHistogram;
    }
}