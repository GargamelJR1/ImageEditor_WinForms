//////////////////////////////////////////////////////////////////////////////////////////////////
// IMAGE EDITOR
//////////////////////////////////////////////////////////////////////////////////////////////////

namespace ImageEditor.ProgramLogic
{
    /**
     * ImageProcessorCSharp class is used withing ImageProcessing class to process images.
     * Class uses C# functions to process images.
     */
    public class ImageProcessorCSharp : ImageProcessor
    {
        // delegates for methods that apply graphic filters to the image

        override public Bitmap ChangeImageBrightness(Bitmap image, float strength, int threadCount, ref long executionTime)
        {
            return FilterParameter(BrightnessFilter, image, strength, threadCount, ref executionTime);
        }

        override public Bitmap ChangeImageContrast(Bitmap image, float strength, int threadCount, ref long executionTime)
        {
            return FilterParameter(ContrastFilter, image, strength, threadCount, ref executionTime);
        }

        override public Bitmap ChangeImageTransparency(Bitmap image, float strength, int threadCount, ref long executionTime)
        {
            return FilterParameter(TransparencyFilter, image, strength, threadCount, ref executionTime);
        }

        override public Bitmap MakeImageGrayScale(Bitmap image, int threadCount, ref long executionTime)
        {
            return FilterParameterless(GrayScaleFilter, image, threadCount, ref executionTime);
        }

        override public Bitmap MakeImageSepia(Bitmap image, int threadCount, ref long executionTime)
        {
            return FilterParameterless(SepiaFilter, image, threadCount, ref executionTime);
        }

        override public Bitmap MakeImageNegative(Bitmap image, int threadCount, ref long executionTime)
        {
            return FilterParameterless(NegativeFilter, image, threadCount, ref executionTime);
        }

        /**
         * Function for applying brightness filter on image
         * @warning - this function overwrites original image data
         * @param endIndex - index of one after last byte, which will be processed
         * @param data - pointer to array with image data
         * @param startIndex - index of first byte in image, which will be processed
         * @param strength - strength of filter
         * @brief precalculated component is being added to each color component
         * @return function return nothing (void) but it changes data array, which is passed by reference
         */
        private void BrightnessFilter(int endIndex, byte[] imageBytesArray, int startIndex, float strength)
        {
            // if strength is 0, then we don't need to do anything
            if (strength == 0)
                return;

            // precalculated component is being added to each color component
            float componentFloat = 255.0f * strength;
            componentFloat += (componentFloat > 0) ? 0.5f : -0.5f;
            int component = (int)componentFloat;
            for (int i = startIndex; i < endIndex; i++)
            {
                // skip alpha channel
                if ((i + 1) % 4 == 0) continue;

                // calculate new color value
                int result = component + imageBytesArray[i];
                if (result > 255)
                    result = 255;
                else if (result < 0)
                    result = 0;
                imageBytesArray[i] = (byte)result;
            }
        }

        /**
         * Function for applying contrast filter on image
         * @warning - this function overwrites original image data
         * @param endIndex - index of one after last byte, which will be processed
         * @param data - pointer to array with image data
         * @param startIndex - index of first byte in image, which will be processed
         * @param strength - strength of filter
         * @brief new value is calculated for each color component by formula: ((((old color / 255.0) - 0.5) * strength) + 0.5) * 255.0
         * @return function return nothing (void) but it changes data array, which is passed by reference
         */
        private void ContrastFilter(int endIndex, byte[] imageBytesArray, int startIndex, float strength)
        {
            // new value is calculated for each color component by formula: ((((old color / 255.0) - 0.5) * strength) + 0.5) * 255.0
            for (int i = startIndex; i < endIndex; i++)
            {
                // skip alpha channel
                if ((i + 1) % 4 == 0) continue;

                // calculate new color value
                float result = ((((imageBytesArray[i] / 255.0f) - 0.5f) * strength) + 0.5f) * 255.0f;
                if (result > 255)
                    result = 255;
                else if (result < 0)
                    result = 0;
                imageBytesArray[i] = (byte)(result + 0.5f);
            }
        }

        /**
         * Function for applying transparency filter on image
         * @warning - this function overwrites original image data
         * @param endIndex - index of one after last byte, which will be processed
         * @param data - pointer to array with image data
         * @param startIndex - index of first byte in image, which will be processed
         * @param multiplier - multiplier for alpha value
         * @brief new alpha value is calculated for each pixel by formula: 255 * multiplier
         * @return function return nothing (void) but it changes data array, which is passed by reference
         */
        private void TransparencyFilter(int endIndex, byte[] imageBytesArray, int startIndex, float multiplier)
        {
            // calculate new alpha value and set it for each pixel
            int alphaByte = (int)((255.0f * multiplier) + 0.5f);

            // loop through every 4th byte, which is alpha channel
            for (int i = startIndex + 3; i < endIndex; i += 4)
            {
                imageBytesArray[i] = (byte)alphaByte;
            }
        }

        /**
         * Function for applying grayscale filter on image
         * @warning - this function overwrites original image data
         * @param endIndex - index of one after last byte, which will be processed
         * @param data - pointer to array with image data
         * @param startIndex - index of first byte in image, which will be processed
         * @param strength - strength of filter
         * @brief new value is calculated for each color component in every pixel by formula: 0.114 * b + 0.587 * g + 0.299 * r
         * @return function return nothing (void) but it changes data array, which is passed by reference
         */
        private void GrayScaleFilter(int endIndex, byte[] imageBytesArray, int startIndex)
        {
            // new value is calculated for each color component by formula: 0.114 * b + 0.587 * g + 0.299 * r
            // every color in pixel is set to this value
            for (int i = startIndex; i < endIndex; i += 4)
            {
                int newValue = (int)(imageBytesArray[i] * 0.114f + imageBytesArray[i + 1] * 0.587f + imageBytesArray[i + 2] * 0.299f + 0.5f);
                if (newValue > 255)
                    newValue = 255;
                else if (newValue < 0)
                    newValue = 0;
                byte newValueByte = (byte)newValue;

                imageBytesArray[i] = newValueByte;
                imageBytesArray[i + 1] = newValueByte;
                imageBytesArray[i + 2] = newValueByte;
            }
        }

        /**
         * Function for applying sepia filter on image
         * @warning - this function overwrites original image data
         * @param endIndex - index of one after last byte, which will be processed
         * @param data - pointer to array with image data
         * @param startIndex - index of first byte in image, which will be processed
         * @brief new value is calculated separately for each color component by multiplying each component by special values and adding then adding them together
         * @return function return nothing (void) but it changes data array, which is passed by reference
         */
        private void SepiaFilter(int endIndex, byte[] imageBytesArray, int startIndex)
        {
            // new value is calculated separately for each color component
            // B = 0.131 * b + 0.534 * g + 0.272 * r
            // G = 0.168 * b + 0.686 * g + 0.349 * r
            // R = 0.189 * b + 0.769 * g + 0.393 * r
            for (int i = startIndex; i < endIndex; i += 4)
            {
                // i is b, i+1 is g, i+2 is r

                // calculate new blue color value
                float newValueB = imageBytesArray[i] * 0.131f + imageBytesArray[i + 1] * 0.534f + imageBytesArray[i + 2] * 0.272f;
                if (newValueB > 255)
                    newValueB = 255;
                else if (newValueB < 0)
                    newValueB = 0;

                // calculate new green color value
                float newValueG = imageBytesArray[i] * 0.168f + imageBytesArray[i + 1] * 0.686f + imageBytesArray[i + 2] * 0.349f;
                if (newValueG > 255)
                    newValueG = 255;
                else if (newValueG < 0)
                    newValueG = 0;

                // calculate new red color value
                float newValueR = imageBytesArray[i] * 0.189f + imageBytesArray[i + 1] * 0.769f + imageBytesArray[i + 2] * 0.393f;
                if (newValueR > 255)
                    newValueR = 255;
                else if (newValueR < 0)
                    newValueR = 0;

                // add 0.5f to each value to round and then save new values to memory
                imageBytesArray[i] = (byte)(newValueB + 0.5f);
                imageBytesArray[i + 1] = (byte)(newValueG + 0.5f);
                imageBytesArray[i + 2] = (byte)(newValueR + 0.5f);
            }
        }

        /**
         * Function for applying negative filter on image
         * @warning - this function overwrites original image data
         * @param endIndex - index of one after last byte, which will be processed
         * @param data - pointer to array with image data
         * @param startIndex - index of first byte in image, which will be processed
         * @brief new value is calculated for each color component by formula: 255 - old value
         * @return function return nothing (void) but it changes data array, which is passed by reference
         */
        private void NegativeFilter(int endIndex, byte[] imageBytesArray, int startIndex)
        {
            // new value is calculated for each color component by formula: 255 - old value
            for (int i = startIndex; i < endIndex; i += 4)
            {
                imageBytesArray[i] = (byte)(255 - imageBytesArray[i]);
                imageBytesArray[i + 1] = (byte)(255 - imageBytesArray[i + 1]);
                imageBytesArray[i + 2] = (byte)(255 - imageBytesArray[i + 2]);
            }
        }
    }
}
