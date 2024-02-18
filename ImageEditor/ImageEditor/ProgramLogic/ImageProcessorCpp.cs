//////////////////////////////////////////////////////////////////////////////////////////////////
// IMAGE EDITOR
//////////////////////////////////////////////////////////////////////////////////////////////////

using System.Runtime.InteropServices;

namespace ImageEditor.ProgramLogic
{
    /**
     * ImageProcessorCpp class is used withing ImageProcessing class to process images.
     * Class loads C++ functions from CppFunctions.dll and uses them to process images.
     */
    public class ImageProcessorCpp : ImageProcessor
    {
        private readonly DllLoader dllLoader;

        // delegates for methods that apply graphic filters to the image
        private ImageFilterParameter BrightnessFilterCpp;
        private ImageFilterParameter ContrastFilterCpp;
        private ImageFilterParameterless GrayScaleFilterCpp;
        private ImageFilterParameterless SepiaFilterCpp;
        private ImageFilterParameterless NegativeFilterCpp;
        private ImageFilterParameter TransparencyFilterCpp;

        public ImageProcessorCpp()
        {
            // load proper version (Debug/Release) of C++ functions dll and get function pointers
            try
            {
#if DEBUG
                dllLoader = new DllLoader(@"../../../../x64/Debug/CppFunctions.dll");
#else
                dllLoader = new DllLoader(@"../../../../x64/Release/CppFunctions.dll");
#endif
                BrightnessFilterCpp = Marshal.GetDelegateForFunctionPointer<ImageFilterParameter>(dllLoader.GetFunctionPointer("BrightnessFilterCpp"));
                ContrastFilterCpp = Marshal.GetDelegateForFunctionPointer<ImageFilterParameter>(dllLoader.GetFunctionPointer("ContrastFilterCpp"));
                GrayScaleFilterCpp = Marshal.GetDelegateForFunctionPointer<ImageFilterParameterless>(dllLoader.GetFunctionPointer("GrayScaleFilterCpp"));
                SepiaFilterCpp = Marshal.GetDelegateForFunctionPointer<ImageFilterParameterless>(dllLoader.GetFunctionPointer("SepiaFilterCpp"));
                NegativeFilterCpp = Marshal.GetDelegateForFunctionPointer<ImageFilterParameterless>(dllLoader.GetFunctionPointer("NegativeFilterCpp"));
                TransparencyFilterCpp = Marshal.GetDelegateForFunctionPointer<ImageFilterParameter>(dllLoader.GetFunctionPointer("TransparencyFilterCpp"));
            }
            catch
            {
                throw new DllNotFoundException("CppFunctions.dll not found");
            }
        }

        override public Bitmap ChangeImageBrightness(Bitmap image, float strength, int threadCount, ref long executionTime)
        {
            return FilterParameter(BrightnessFilterCpp, image, strength, threadCount, ref executionTime);
        }

        override public Bitmap ChangeImageContrast(Bitmap image, float strength, int threadCount, ref long executionTime)
        {
            return FilterParameter(ContrastFilterCpp, image, strength, threadCount, ref executionTime);
        }

        override public Bitmap ChangeImageTransparency(Bitmap image, float strength, int threadCount, ref long executionTime)
        {
            return FilterParameter(TransparencyFilterCpp, image, strength, threadCount, ref executionTime);
        }

        override public Bitmap MakeImageGrayScale(Bitmap image, int threadCount, ref long executionTime)
        {
            return FilterParameterless(GrayScaleFilterCpp, image, threadCount, ref executionTime);
        }

        override public Bitmap MakeImageSepia(Bitmap image, int threadCount, ref long executionTime)
        {
            return FilterParameterless(SepiaFilterCpp, image, threadCount, ref executionTime);
        }

        override public Bitmap MakeImageNegative(Bitmap image, int threadCount, ref long executionTime)
        {
            return FilterParameterless(NegativeFilterCpp, image, threadCount, ref executionTime);
        }
    }
}
