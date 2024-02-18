//////////////////////////////////////////////////////////////////////////////////////////////////
// IMAGE EDITOR
//////////////////////////////////////////////////////////////////////////////////////////////////

using System.Diagnostics;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ImageEditor.ProgramLogic
{
    /**
     * ImageProcessor class is used withing ImageProcessing class to process images.
     * It contains methods for changing brightness, contrast, transparency, making image grayscale, sepia and negative.
     * Class is abstract and can is inherited by ImageProcessorASM, ImageProcessorCpp and ImageProcessorCSharp to run functions written in ASM, C++ and C#.
     */
    abstract public class ImageProcessor
    {
        protected int originalImageArraySize;
        protected Stopwatch stopwatch = new();

        // delegates for methods that apply graphic filters to the image
        protected delegate void ImageFilterParameter(int endIndex, byte[] imageBytesArray, int startIndex, float strength);
        protected delegate void ImageFilterParameterless(int endIndex, byte[] imageBytesArray, int startIndex);

        abstract public Bitmap ChangeImageBrightness(Bitmap image, float strength, int ThreadsCount, ref long executionTime);

        abstract public Bitmap ChangeImageContrast(Bitmap image, float strength, int ThreadsCount, ref long executionTime);

        abstract public Bitmap ChangeImageTransparency(Bitmap image, float strength, int ThreadsCount, ref long executionTime);

        abstract public Bitmap MakeImageGrayScale(Bitmap image, int ThreadsCount, ref long executionTime);

        abstract public Bitmap MakeImageSepia(Bitmap image, int ThreadsCount, ref long executionTime);

        abstract public Bitmap MakeImageNegative(Bitmap image, int ThreadsCount, ref long executionTime);

        /**
         * Method that converts Bitmap image to byte array.
         * @param image Bitmap image to be converted.
         * @param modulSize number of threads to be used.
         * @return byte array of the image made from Bitmap image.
         */
        protected byte[] ConvertBitmapToBytesArray(Bitmap image, int moduloSize)
        {
            BitmapData bitmapData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat);
            int arraySize = bitmapData.Stride * image.Height;
            originalImageArraySize = arraySize;
            arraySize /= 4;
            if (arraySize % moduloSize != 0)
            {
                arraySize += moduloSize - arraySize % moduloSize;
            }
            arraySize *= 4;
            byte[] imageBytesArray = new byte[arraySize];
            Marshal.Copy(bitmapData.Scan0, imageBytesArray, 0, originalImageArraySize);
            image.UnlockBits(bitmapData);
            return imageBytesArray;
        }

        /**
         * Method that converts byte array to Bitmap image.
         * @param imageBytesArray byte array to be converted.
         * @param image original Bitmap image from which the byte array was created.
         * @return Bitmap image made from byte array.
         */
        protected Bitmap ConvertBytesArrayToBitmap(byte[] imageBytesArray, Bitmap image)
        {
            BitmapData bitmapData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat);
            Marshal.Copy(imageBytesArray, 0, bitmapData.Scan0, originalImageArraySize);
            image.UnlockBits(bitmapData);
            return image;
        }

        /**
         * Method that applies graphic filter with parameter to the image.
         * @param method delegate to specific filter method, which applies graphic filter to the image.
         * @param image Bitmap image to be processed.
         * @param strength strength of the filter.
         * @param threadCount number of threads to be used.
         * @param executionTime reference to long variable, which will be set to execution time of the filter.
         * @return Bitmap image after applying graphic filter.
         */
        protected Bitmap FilterParameter(ImageFilterParameter method, Bitmap image, float strength, int threadCount, ref long executionTime)
        {
            byte[] array = ConvertBitmapToBytesArray(image, threadCount);
            Thread[] threads = new Thread[threadCount];
            int threadArrayIndex = array.Length / threadCount;
            stopwatch.Restart();
            for (int i = 0; i < threadCount; i++)
            {
                int startIndex = i * threadArrayIndex;
                int endIndex = (i + 1) * threadArrayIndex;

                threads[i] = new Thread(() => method(endIndex, array, startIndex, strength));
                threads[i].Start();
            }
            for (int i = 0; i < threadCount; i++)
            {
                threads[i].Join();
            }
            stopwatch.Stop();
            executionTime = stopwatch.ElapsedTicks;
            return ConvertBytesArrayToBitmap(array, image);
        }

        /**
        * Method that applies graphic filter without parameter to the image.
        * @param method delegate to specific filter method, which applies graphic filter to the image.
        * @param image Bitmap image to be processed.
        * @param threadCount number of threads to be used.
        * @param executionTime reference to long variable, which will be set to execution time of the filter.
        * @return Bitmap image after applying graphic filter.
        */
        protected Bitmap FilterParameterless(ImageFilterParameterless method, Bitmap image, int threadCount, ref long executionTime)
        {
            byte[] array = ConvertBitmapToBytesArray(image, threadCount);
            Thread[] threads = new Thread[threadCount];
            int threadArrayIndex = array.Length / threadCount;
            stopwatch.Restart();
            for (int i = 0; i < threadCount; i++)
            {
                int startIndex = i * threadArrayIndex;
                int endIndex = (i + 1) * threadArrayIndex;

                threads[i] = new Thread(() => method(endIndex, array, startIndex));
                threads[i].Start();
            }
            for (int i = 0; i < threadCount; i++)
            {
                threads[i].Join();
            }
            stopwatch.Stop();
            executionTime = stopwatch.ElapsedTicks;
            return ConvertBytesArrayToBitmap(array, image);
        }
    }
}
