//////////////////////////////////////////////////////////////////////////////////////////////////
// IMAGE EDITOR
// C++ functions for image processing
//////////////////////////////////////////////////////////////////////////////////////////////////

#include "pch.h"
#include "CppFun.h"

// macro for checking if value is in range 0-255 and if not, then set it to 0 or 255, depending on which side of range it is
#define bound0to255(value) if (value > 255) value = 255; else if (value < 0) value = 0;

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
void BrightnessFilterCpp(int endIndex, unsigned char* data, int startIndex, float strength) {
	// if strength is 0, then we don't need to do anything
	if (strength == 0)
		return;

	// precalculated component is being added to each color component
	float componentFloat = 255.0f * strength;
	componentFloat > 0 ? componentFloat += 0.5f : componentFloat -= 0.5f;
	int component = static_cast<int>(componentFloat);

	// loop through every pixel
	// blue, green and red color components are calculated separately in each iteration
	for (int i = startIndex; i < endIndex; i += 4) {
		int result = data[i] + component;
		bound0to255(result);
		data[i] = result;

		result = data[i + 1] + component;
		bound0to255(result);
		data[i + 1] = result;

		result = data[i + 2] + component;
		bound0to255(result);
		data[i + 2] = result;
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
void ContrastFilterCpp(int endIndex, unsigned char* data, int startIndex, float strength) {
	// new value is calculated for each color component by formula: ((((old color / 255.0) - 0.5) * strength) + 0.5) * 255.0

	// loop through every pixel
	// blue, green and red color components are calculated separately in each iteration
	for (int i = startIndex; i < endIndex; i += 4) {
		float result = ((((data[i] / 255.0f) - 0.5f) * strength) + 0.5f) * 255.0f;
		bound0to255(result);
		data[i] = result + 0.5f;

		result = ((((data[i + 1] / 255.0f) - 0.5f) * strength) + 0.5f) * 255.0f;
		bound0to255(result);
		data[i + 1] = result + 0.5f;

		result = ((((data[i + 2] / 255.0f) - 0.5f) * strength) + 0.5f) * 255.0f;
		bound0to255(result);
		data[i + 2] = result + 0.5f;
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
void TransparencyFilterCpp(int endIndex, unsigned char* data, int startIndex, float multiplier) {
	// calculate new alpha value and set it for each pixel
	int alphaByte = (255.0f * multiplier) + 0.5f;
	// loop through every 4th byte, which is alpha byte
	for (int i = startIndex + 3; i < endIndex; i += 4) {
		data[i] = alphaByte;
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
void GrayScaleFilterCpp(int endIndex, unsigned char* data, int startIndex) {
	// new value is calculated for each color component by formula: 0.114 * b + 0.587 * g + 0.299 * r
	// every color in pixel is set to this value
	for (int i = startIndex; i < endIndex; i += 4) {
		int newValue = (data[i] * 0.114f + data[i + 1] * 0.587f + data[i + 2] * 0.299f) + 0.5f;
		bound0to255(newValue);

		data[i] = newValue;
		data[i + 1] = newValue;
		data[i + 2] = newValue;
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
void NegativeFilterCpp(int endIndex, unsigned char* data, int startIndex) {
	// new value is calculated for each color component by formula: 255 - old value
	for (int i = startIndex; i < endIndex; i += 4) {
		data[i] = 255 - data[i];
		data[i + 1] = 255 - data[i + 1];
		data[i + 2] = 255 - data[i + 2];
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
void SepiaFilterCpp(int endIndex, unsigned char* data, int startIndex) {
	// new value is calculated separately for each color component
	// B = 0.131 * b + 0.534 * g + 0.272 * r
	// G = 0.168 * b + 0.686 * g + 0.349 * r
	// R = 0.189 * b + 0.769 * g + 0.393 * r
	for (int i = startIndex; i < endIndex; i += 4) {
		// i is b, i+1 is g, i+2 is r

		// calculate new blue color value
		float newValueB = (data[i] * 0.131f + data[i + 1] * 0.534f + data[i + 2] * 0.272f);
		bound0to255(newValueB);

		// calculate new green color value
		float newValueG = (data[i] * 0.168f + data[i + 1] * 0.686f + data[i + 2] * 0.349f);
		bound0to255(newValueG);

		// calculate new red color value
		float newValueR = (data[i] * 0.189f + data[i + 1] * 0.769f + data[i + 2] * 0.393f);
		bound0to255(newValueR);

		data[i] = newValueB + 0.5f;
		data[i + 1] = newValueG + 0.5f;
		data[i + 2] = newValueR + 0.5f;
	}
}
