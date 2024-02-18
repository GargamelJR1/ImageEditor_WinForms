//////////////////////////////////////////////////////////////////////////////////////////////////
// IMAGE EDITOR
// C++ functions for image processing
//////////////////////////////////////////////////////////////////////////////////////////////////

#pragma once

#ifdef CppFun_EXPORTS
#define CppFun_API __declspec(dllexport)
#else
#define CppFun_API __declspec(dllimport)
#endif

/**
 * Function that changes brightness of image
 * @param endIndex - index of last pixel in data array tha will be changed by this function
 * @param data - array of pixels, each pixel is represented by 4 bytes (RGBA)
 * @param startIndex - index of first pixel in data array that will be changed by this function
 * @param strength - strength of brightness change
 */
extern "C" CppFun_API void BrightnessFilterCpp(int endIndex, unsigned char* data, int startIndex, float strength);

/**
 * Function that changes contrast of image
 * @param endIndex - index of last pixel in data array that will be changed by this function
 * @param data - array of pixels, each pixel is represented by 4 bytes (RGBA)
 * @param startIndex - index of first pixel in data array that will be changed by this function
 * @param strength - strength of contrast change
 */
extern "C" CppFun_API void ContrastFilterCpp(int endIndex, unsigned char* data, int startIndex, float strength);

/**
 * Function that changes transparency of image
 * @param endIndex - index of last pixel in data array that will be changed by this function
 * @param data - array of pixels, each pixel is represented by 4 bytes (RGBA)
 * @param startIndex - index of first pixel in data array that will be changed by this function
 * @param multiplier - multiplier of transparency (range 0-1, range is not checked within function)
 */
extern "C" CppFun_API void TransparencyFilterCpp(int endIndex, unsigned char* data, int startIndex, float multiplier);

/**
 * Function that applies grayscale filter to image
 * @param endIndex - index of last pixel in data array that will be changed by this function
 * @param data - array of pixels, each pixel is represented by 4 bytes (RGBA)
 * @param startIndex - index of first pixel in data array that will be changed by this function
 */
extern "C" CppFun_API void GrayScaleFilterCpp(int endIndex, unsigned char* data, int startIndex);

/**
 * Function that applies negative filter to image
 * @param endIndex - index of last pixel in data array that will be changed by this function
 * @param data - array of pixels, each pixel is represented by 4 bytes (RGBA)
 * @param startIndex - index of first pixel in data array that will be changed by this function
 */
extern "C" CppFun_API void NegativeFilterCpp(int endIndex, unsigned char* data, int startIndex);

/**
 * Function that applies sepia filter to image
 * @param endIndex - index of last pixel in data array that will be changed by this function
 * @param data - array of pixels, each pixel is represented by 4 bytes (RGBA)
 * @param startIndex - index of first pixel in data array that will be changed by this function
 */
extern "C" CppFun_API void SepiaFilterCpp(int endIndex, unsigned char* data, int startIndex);
