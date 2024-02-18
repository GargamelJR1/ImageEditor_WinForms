//////////////////////////////////////////////////////////////////////////////////////////////////
// IMAGE EDITOR
//////////////////////////////////////////////////////////////////////////////////////////////////

using CsvHelper;
using System.Diagnostics;
using System.Globalization;

namespace ImageEditor.ProgramLogic
{
    /**
     * Benchmark class is responsible for running benchmark tests on all functions in ImageProcessor classes.
     * Benchmarks measure time of function with every possible number of threads (1-64) and programming language (C#, C++, ASM).
     * Time is measered in most accurate way possible and then converted to microseconds.
     * It uses ImageProcessing class to run functions on a given image.
     * It saves results to a csv file.
     */
    internal class Benchmark
    {
        private delegate long FunctionToBenchmark(int threadCount);
        private delegate long FunctionToBenchmarkParameter(float strength, int threadCount);

        private readonly int MAX_NUMBER_OF_THREADS = 64;

        internal enum FunctionsToBenchmark
        {
            brightnessChange,
            transparencyChange,
            contrastChange,
            grayscale,
            sepia,
            negative
        }

        record class BenchmarkResult(string language, int threadCount, long trimmedAverageTime, string function);

        ImageProcessing imageProcessing;
        ImageProcessorASM imageProcessorASM = new();
        ImageProcessorCSharp imageProcessorCSharp = new();
        ImageProcessorCpp imageProcessorCpp = new();

        List<BenchmarkResult> results = new();
        ProgressBar progressBar;


        public Benchmark(ProgressBar progressBar)
        {
            this.progressBar = progressBar;
            Bitmap bitmap1 = new Bitmap(10, 10);
            imageProcessing = new ImageProcessing(bitmap1, imageProcessorCSharp);
            PrepareFunctions(bitmap1);
        }

        public void RunBenchmark(Bitmap sourceImage, FunctionsToBenchmark function, float strength = 0)
        {
            progressBar.Value = 0;
            float progressBarValue = 0;
            string[] languageHeaders = new string[3] { "C#", "C++", "ASM" };
            FunctionToBenchmark functionParameterless = imageProcessing.MakeSepia;
            FunctionToBenchmarkParameter functionParameter = imageProcessing.ChangeBrightness;
            bool parameter = false;

            switch (function)
            {
                case FunctionsToBenchmark.brightnessChange:
                    functionParameter = imageProcessing.ChangeBrightness;
                    parameter = true;
                    break;
                case FunctionsToBenchmark.contrastChange:
                    functionParameter = imageProcessing.ChangeContrast;
                    parameter = true;
                    break;
                case FunctionsToBenchmark.transparencyChange:
                    functionParameter = imageProcessing.ChangeTransparency;
                    parameter = true;
                    break;
                case FunctionsToBenchmark.grayscale:
                    functionParameterless = imageProcessing.MakeGrayScale;
                    break;
                case FunctionsToBenchmark.sepia:
                    functionParameterless = imageProcessing.MakeSepia;
                    break;
                case FunctionsToBenchmark.negative:
                    functionParameterless = imageProcessing.MakeNegative;
                    break;
            }

            float progressBarStep = 100f / (languageHeaders.Length * MAX_NUMBER_OF_THREADS);
            for (int l = 0; l < languageHeaders.Length; l++)
            {
                switch (languageHeaders[l])
                {
                    case "C#":
                        imageProcessing.SetImageProcessor(imageProcessorCSharp);
                        break;
                    case "C++":
                        imageProcessing.SetImageProcessor(imageProcessorCpp);
                        break;
                    case "ASM":
                        imageProcessing.SetImageProcessor(imageProcessorASM);
                        break;
                }

                for (int i = 0; i < MAX_NUMBER_OF_THREADS; i++)
                {
                    List<long> threadTimes = new List<long>(12);
                    for (int j = 0; j < 12; j++)
                    {
                        imageProcessing.ImageToProcess = sourceImage;
                        if (parameter)
                            threadTimes.Add(functionParameter(strength, i + 1));
                        else
                            threadTimes.Add(functionParameterless(i + 1));
                    }
                    progressBarValue += progressBarStep;
                    progressBar.Value = (int)progressBarValue;
                    threadTimes.Sort();
                    long threadTimeTrimmed = (long)((threadTimes.Skip(3).Take(6).Average() + 0.5f) / Stopwatch.Frequency * 1000000);
                    results.Add(new BenchmarkResult(languageHeaders[l], i + 1, threadTimeTrimmed, function.ToString()));
                }
            }
            progressBar.Value = 100;
            SaveResults();
        }

        void SaveResults()
        {
            using (var writer = new StreamWriter("results.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(results);
            }
        }

        void PrepareFunctions(Bitmap sourceImage)
        {
            for (int i = 0; i < 3; i++)
            {
                imageProcessing.SetImageProcessor(imageProcessorASM);
                RunAllFunctions(imageProcessing);
                imageProcessing.SetImageProcessor(imageProcessorCSharp);
                RunAllFunctions(imageProcessing);
                imageProcessing.SetImageProcessor(imageProcessorCpp);
                RunAllFunctions(imageProcessing);
            }
        }

        void RunAllFunctions(ImageProcessing imageProcessing)
        {
            imageProcessing.MakeSepia(1);
            imageProcessing.MakeGrayScale(1);
            imageProcessing.MakeNegative(1);
            imageProcessing.ChangeBrightness(1, 1);
            imageProcessing.ChangeContrast(1, 1);
            imageProcessing.ChangeTransparency(1, 1);
        }
    }
}
