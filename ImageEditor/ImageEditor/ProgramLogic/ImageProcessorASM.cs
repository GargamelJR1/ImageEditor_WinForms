//////////////////////////////////////////////////////////////////////////////////////////////////
// IMAGE EDITOR
//////////////////////////////////////////////////////////////////////////////////////////////////

using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;

namespace ImageEditor.ProgramLogic
{
    /**
     * ImageProcessorASM class is used withing ImageProcessing class to process images.
     * Class loads ASM functions from AssemblyFunctions.dll and uses them to process images.
     */
    public class ImageProcessorASM : ImageProcessor
    {
        private readonly DllLoader dllLoader;

        // delegates for methods that apply graphic filters to the image
        private ImageFilterParameter BrightnessFilterASM;
        private ImageFilterParameter ContrastFilterASM;
        private ImageFilterParameterless GrayScaleFilterASM;
        private ImageFilterParameterless SepiaFilterASM;
        private ImageFilterParameterless NegativeFilterASM;
        private ImageFilterParameter TransparencyFilterASM;

        public ImageProcessorASM()
        {
            // load proper version (Debug/Release) of ASM functions dll and get function pointers
            try
            {
#if DEBUG
                dllLoader = new DllLoader(@"../../../../x64/Debug/AssemblyFunctions.dll");
#else
                dllLoader = new DllLoader(@"../../../../x64/Release/AssemblyFunctions.dll");
#endif
                BrightnessFilterASM = Marshal.GetDelegateForFunctionPointer<ImageFilterParameter>(dllLoader.GetFunctionPointer("BrightnessFilterASM"));
                ContrastFilterASM = Marshal.GetDelegateForFunctionPointer<ImageFilterParameter>(dllLoader.GetFunctionPointer("ContrastFilterASM"));
                GrayScaleFilterASM = Marshal.GetDelegateForFunctionPointer<ImageFilterParameterless>(dllLoader.GetFunctionPointer("GrayScaleFilterASM"));
                SepiaFilterASM = Marshal.GetDelegateForFunctionPointer<ImageFilterParameterless>(dllLoader.GetFunctionPointer("SepiaFilterASM"));
                NegativeFilterASM = Marshal.GetDelegateForFunctionPointer<ImageFilterParameterless>(dllLoader.GetFunctionPointer("NegativeFilterASM"));
                TransparencyFilterASM = Marshal.GetDelegateForFunctionPointer<ImageFilterParameter>(dllLoader.GetFunctionPointer("TransparencyFilterASM"));
            }
            catch
            {
                throw new DllNotFoundException("AssemblyFunctions.dll not found");
            }
        }


        override public Bitmap ChangeImageBrightness(Bitmap image, float strength, int threadCount, ref long executionTime)
        {
            return FilterParameter(BrightnessFilterASM, image, strength, threadCount, ref executionTime);
        }

        override public Bitmap ChangeImageContrast(Bitmap image, float strength, int threadCount, ref long executionTime)
        {
            return FilterParameter(ContrastFilterASM, image, strength, threadCount, ref executionTime);
        }

        override public Bitmap ChangeImageTransparency(Bitmap image, float strength, int threadCount, ref long executionTime)
        {
            return FilterParameter(TransparencyFilterASM, image, strength, threadCount, ref executionTime);
        }

        override public Bitmap MakeImageGrayScale(Bitmap image, int threadCount, ref long executionTime)
        {
            return FilterParameterless(GrayScaleFilterASM, image, threadCount, ref executionTime);
        }

        override public Bitmap MakeImageSepia(Bitmap image, int threadCount, ref long executionTime)
        {
            return FilterParameterless(SepiaFilterASM, image, threadCount, ref executionTime);
        }

        override public Bitmap MakeImageNegative(Bitmap image, int threadCount, ref long executionTime)
        {
            return FilterParameterless(NegativeFilterASM, image, threadCount, ref executionTime);
        }
    }
}
