//////////////////////////////////////////////////////////////////////////////////////////////////
// IMAGE EDITOR
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms.DataVisualization.Charting;

namespace ImageEditor.ProgramLogic
{
    /**
     * Class that represents a histogram of an image.
     * It is used to calculate the histogram of an image and to display it in a chart.
     */
    internal class Histogram
    {
        Bitmap image;

        int[] red;
        int[] green;
        int[] blue;
        int[] alpha;

        public Histogram(Bitmap image)
        {
            this.image = image;
            red = new int[256];
            green = new int[256];
            blue = new int[256];
            alpha = new int[256];
            calculate();
        }

        private void calculate()
        {
            BitmapData bitmapData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
            int arraySize = bitmapData.Stride * image.Height;
            byte[] imageBytesArray = new byte[arraySize];
            Marshal.Copy(bitmapData.Scan0, imageBytesArray, 0, arraySize);
            image.UnlockBits(bitmapData);

            for (int i = 0; i < arraySize; i += 4)
            {
                blue[imageBytesArray[i]]++;
                green[imageBytesArray[i + 1]]++;
                red[imageBytesArray[i + 2]]++;
                alpha[imageBytesArray[i + 3]]++;
            }
        }

        public int[][] getHistogramData()
        {
            return new int[][] { red, green, blue, alpha };
        }

        public void showHistogram(string windowTitle)
        {
            // Create a new form
            Form form = new Form();
            form.Size = new System.Drawing.Size(800, 600);
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Text = windowTitle;

            // Create a new chart and add it to the form
            Chart histOutput = new Chart();
            histOutput.Parent = form;
            histOutput.Dock = DockStyle.Fill;

            int[][] histogramData = this.getHistogramData();
            int[] x = Enumerable.Range(0, 256).ToArray();
            histOutput.Series.Clear();
            histOutput.Series.Add("Red");
            histOutput.Series.Add("Green");
            histOutput.Series.Add("Blue");
            histOutput.Series["Red"].ChartType = SeriesChartType.Column;
            histOutput.Series["Green"].ChartType = SeriesChartType.Column;
            histOutput.Series["Blue"].ChartType = SeriesChartType.Column;

            // Create a new chart area for each color
            ChartArea areaRed = new ChartArea("AreaRed");
            ChartArea areaGreen = new ChartArea("AreaGreen");
            ChartArea areaBlue = new ChartArea("AreaBlue");

            // Add the chart areas to the chart
            histOutput.ChartAreas.Add(areaRed);
            histOutput.ChartAreas.Add(areaGreen);
            histOutput.ChartAreas.Add(areaBlue);

            // Set the title of each chart area
            histOutput.ChartAreas["AreaRed"].AxisX.Title = "Red";
            histOutput.ChartAreas["AreaGreen"].AxisX.Title = "Green";
            histOutput.ChartAreas["AreaBlue"].AxisX.Title = "Blue";

            // Assign each series to a different chart area
            histOutput.Series["Red"].ChartArea = "AreaRed";
            histOutput.Series["Green"].ChartArea = "AreaGreen";
            histOutput.Series["Blue"].ChartArea = "AreaBlue";

            // Set the maximum and minimum for each X axis
            foreach (ChartArea area in histOutput.ChartAreas)
            {
                area.AxisX.Maximum = 255;
                area.AxisX.Minimum = 0;
            }

            // Set the interval for each X axis
            histOutput.ChartAreas["AreaRed"].AxisX.Interval = 5;
            histOutput.ChartAreas["AreaGreen"].AxisX.Interval = 5;
            histOutput.ChartAreas["AreaBlue"].AxisX.Interval = 5;

            // Set the maximum for each Y axis
            histOutput.ChartAreas["AreaRed"].AxisY.Maximum = histogramData[0].Max();
            histOutput.ChartAreas["AreaGreen"].AxisY.Maximum = histogramData[1].Max();
            histOutput.ChartAreas["AreaBlue"].AxisY.Maximum = histogramData[2].Max();

            // Hide Y axis labels
            histOutput.ChartAreas["AreaRed"].AxisY.LabelStyle.Enabled = false;
            histOutput.ChartAreas["AreaGreen"].AxisY.LabelStyle.Enabled = false;
            histOutput.ChartAreas["AreaBlue"].AxisY.LabelStyle.Enabled = false;

            // Set data for each series
            histOutput.Series["Red"].Points.DataBindXY(x, histogramData[0]);
            histOutput.Series["Green"].Points.DataBindXY(x, histogramData[1]);
            histOutput.Series["Blue"].Points.DataBindXY(x, histogramData[2]);

            // Set the color of each series
            histOutput.Series["Red"].Color = Color.Red;
            histOutput.Series["Green"].Color = Color.Green;
            histOutput.Series["Blue"].Color = Color.Blue;

            histOutput.Update();
            histOutput.Show();

            // Set form icon to the same as the main form
            form.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            // Show the form
            form.Show();
            form.Activate();
        }
    }
}
