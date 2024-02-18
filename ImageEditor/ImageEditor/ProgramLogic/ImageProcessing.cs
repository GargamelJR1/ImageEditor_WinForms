//////////////////////////////////////////////////////////////////////////////////////////////////
// IMAGE EDITOR
//////////////////////////////////////////////////////////////////////////////////////////////////

namespace ImageEditor.ProgramLogic
{
    /**
     * ImageProcessing class is responsible for processing images.
     * Original image is stored in ImageToProcess property and copy of it is stored in ProcessedImage property.
     * It uses ImageProcessor class to run functions on a given image and stores results in ProcessedImage property.
     * ImageProcessor class object can be changed to ImageProcessorASM or ImageProcessorCpp to run functions written in C++ or ASM.
     */
    public class ImageProcessing
    {
        ImageProcessor processor;

        private Bitmap imageToProcess;
        public Bitmap ImageToProcess
        {
            get
            {
                return imageToProcess;
            }
            set
            {
                imageToProcess = value;
                ProcessedImage = (Bitmap)value.Clone();
            }
        }

        public void SetImageToProcess(Bitmap image)
        {
            imageToProcess = image;
            ProcessedImage = (Bitmap)image.Clone();
        }

        public Bitmap ProcessedImage
        {
            get; private set;
        }

        public ImageProcessing(Bitmap imageToProcess, ImageProcessor processor)
        {
            if (imageToProcess == null)
                throw new NullReferenceException();

            this.processor = processor;
            this.ImageToProcess = imageToProcess;
            this.ProcessedImage = imageToProcess;
        }

        public void SetImageProcessor(ImageProcessor _processor)
        {
            processor = _processor;
        }

        public long ChangeBrightness(float strength, int threadsNumber)
        {
            long processTime = 0;
            processor.ChangeImageBrightness(ProcessedImage, strength, threadsNumber, ref processTime);
            return processTime;
        }

        public long ChangeContrast(float strength, int threadsNumber)
        {
            long processTime = 0;
            processor.ChangeImageContrast(ProcessedImage, strength, threadsNumber, ref processTime);
            return processTime;
        }

        public long ChangeTransparency(float strength, int threadsNumber)
        {
            long processTime = 0;
            processor.ChangeImageTransparency(ProcessedImage, strength, threadsNumber, ref processTime);
            return processTime;
        }

        public long MakeGrayScale(int threadsNumber)
        {
            long processTime = 0;
            processor.MakeImageGrayScale(ProcessedImage, threadsNumber, ref processTime);
            return processTime;
        }

        public long MakeSepia(int threadsNumber)
        {
            long processTime = 0;
            processor.MakeImageSepia(ProcessedImage, threadsNumber, ref processTime);
            return processTime;
        }

        public long MakeNegative(int threadsNumber)
        {
            long processTime = 0;
            processor.MakeImageNegative(ProcessedImage, threadsNumber, ref processTime);
            return processTime;
        }
    }
}
