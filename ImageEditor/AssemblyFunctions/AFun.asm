;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; IMAGE EDITOR
; ASM procedures for applying filters to image
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;

; data section - constants used in procedures
.data

	; contrast filter constants
	align 16
	halfps dd 0.5, 0.5, 0.5, 0.5 ; 0.5 in 4 packed floats

	; brightness and grayscale filter constants
	align 16
	mask0LD oword 00000000ffffffffffffffffffffffffh ; mask for skipping alpha channel

	; contrast, brightness and transparency filter constants
	align 16
	var255ps dd 255.0, 255.0, 255.0, 255.0 ; 255 in 4 packed floats

	; sepia filter constants
	align 16
	multipliersS dd 0.131, 0.534, 0.272, 0.0, 0.168, 0.686, 0.349, 0.0, 0.189, 0.769, 0.393, 0.0 ; multipliers for sepia filter

	; grayscale filter constants
	align 16
	multipliersGS dd 0.114, 0.587, 0.299, 0.0 ; multipliers for grayscale filter

	; macro constants
	align 16
	maskIB oword 000000ff000000ff000000ff000000ffh ; mask for checking values greater than 255
	maskDB db 0, 4, 8, 12, 1, 5, 9, 13, 2, 6, 10, 14, 3, 7, 11, 15 ; mask for converting 4 packed integers to 4 packed bytes in xmm

; code section - macros and procedures
.code

SkipToStartIndexAndCalculateNumberOfPixels macro arrayPtrReg, endIndexReg, startIndex
    ; macro skips to start index and calculate number of pixels to process
    ; macro needs
    ; arrayPtrReg - register with pointer to array
    ; endIndexReg - register with end index of array 
    ; startIndex - start index of array (register or constant)
	; macro uses at least 2 general purpose registers (may use 3)

    add arrayPtrReg, startIndex ; skip to start index
    sub endIndexReg, startIndex ; calculate number of pixels to process
endm

PrepareConstantsForMinMaxAndProcedures macro xmmReg255pd, xmmReg0pd, xmmRegMaskDB, varMem255pd, varMemMaskDB
	; macro prepares constants for min max and procedures
	; macro needs
	; varMem255pd - pointer to 4 packed integers = 255 in memory
	; varMemMaskDB - pointer to mask for converting 4 packed integers to 4 packed bytes, in memory
    ; xmmReg255pd - xmm register for 4 packed integers = 255
    ; xmmReg0pd - xmm register for all bits set to 0
	; macro sets
	; 4 packed integers = 255 in xmmReg255pd (xmm register)
	; 4 packed integers = 0 in xmmReg0pd (xmm register)
	; mask for converting 4 packed integers to 4 packed bytes in xmmRegMaskDB (xmm register)
	; macro uses 3 xmm registers

	vmovdqa xmmReg255pd, oword ptr [varMem255pd] ; prepare mask for checking values greater than 255
	pxor xmmReg0pd, xmmReg0pd ; make xmmReg0pd contain all 0
	vmovdqa xmmRegMaskDB, oword ptr [varMemMaskDB] ; prepare mask for converting 4 packed integers to 4 packed bytes
endm

LoadPixelFromMemory macro xmmProcessReg, arrayPtrReg, xmmReg0pd
    ; macro loads 4 bytes with colors from memory to xmmProcessReg
    ; macro needs
    ; xmmProcessReg - xmm register to load pixel to
    ; arrayPtrReg - register with pointer to array
    ; xmmReg0pd - xmm register with all bits set to 0
    ; macro uses 2 xmm registers and 1 general purpose register

    movd xmmProcessReg, dword ptr [arrayPtrReg] ; copy color bytes to xmmProcessReg
    vpunpcklbw xmmProcessReg, xmmProcessReg, xmmReg0pd
    vpunpcklwd xmmProcessReg, xmmProcessReg, xmmReg0pd ; make 4 packed integers from 4 packed bytes
endm

XmmColorsClipping macro xmmProcessReg, xmmReg255pd, xmmReg0pd
	; macro clips colors in xmmProcessReg to 0-255 range
	; macro needs
    ; xmmProcessReg - xmm register with 4 packed integers to clip
	; xmmReg255pd - xmm register with 4 packed integers = 255
	; xmmReg0pd - xmm register with 4 packed integers = 0
	; macro uses 3 xmm registers
		
	pmaxsd xmmProcessReg, xmmReg0pd ; put 0 to each color if it is less than 0
	pminsd xmmProcessReg, xmmReg255pd ; put 255 to each color if it is greater than 255
endm

SavePixelToMemory macro xmmProcessReg, arrayPtrReg, xmmRegMaskDB
	; macro saves pixel bytes to memory
	; macro converts 4 packed integers to 4 packed bytes in xmmProcessReg
	; and then copies them to memory
	; macro needs
    ; xmmProcessReg - xmm register with 4 packed integers to save
	; xmmRegMaskDB - xmm register with mask for converting 4 packed integers to 4 packed bytes
	; arrayPtrReg - register with pointer to array
	; macro uses 3 xmm registers and 1 general purpose register

	vpshufb xmmProcessReg, xmmProcessReg, xmmRegMaskDB ; make 4 packed bytes from 4 integers
	movd dword ptr [arrayPtrReg], xmmProcessReg ; copy color bytes to memory
endm

LoopNextPixel macro arrayPtrReg, arrayRemaindingLengthReg, loopStartLabel, procedureEndLabel
	; loop for next pixel
	; macro needs
	; loopStartLabel - beginning of loop (label)
	; procedureEndLabel - end of procedure (label)
	; arrayPtrReg - register with pointer to array
	; arrayRemaindingLengthReg - register with number of pixels left to process
	; macro uses 2 general purpose registers

	sub arrayRemaindingLengthReg, 4 ; decrement number of pixels left to process
	je procedureEndLabel ; if all pixels processed, jump to end of procedure
	add arrayPtrReg, 4 ; skip to next pixel
	jmp loopStartLabel ; jump to beginning of loop
endm
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;


; procedure apply sepia filter to image
; every pixel is loaded from memory to xmm register, then new colors' values are calculated
; and stored back to memory
; WARNING: original memory array is overwritten
; parameters: end index of array, pointer to array, start index
; at beginning procedure sets rdx to point to start index
; new colors' values are calculated as follows:
; newB = (inputB * 0.131 + inputG * 0.534 + inputR * 0.272)
; newG = (inputB * 0.168 + inputG * 0.686 + inputR * 0.349)
; newR = (inputB * 0.189 + inputG * 0.769 + inputR * 0.393)
; alpha channel is not changed
; procedure uses r8, rdx, rcx, xmm0, xmm1, xmm2, xmm3, xmm6, xmm8, xmm10, xmm11, xmm12, xmm13, xmm14, xmm15
; procedure returns nothing, but array passed as parameter is modified
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
SepiaFilterASM proc endIndexReg: DWORD, array: QWORD, startIndex: DWORD
		; endIndexReg in rcx, array in rdx, startIndex in r8

		SkipToStartIndexAndCalculateNumberOfPixels rdx, rcx, r8 ; skip to start index and calculate number of pixels to process

		; copy multipliers to xmm10, xmm11 and xmm12 registers
		vmovaps xmm10, [multipliersS] ; B multipliers
		vmovaps xmm11, [multipliersS+16] ; G multipliers
		vmovaps xmm12, [multipliersS+32] ; R multipliers

		PrepareConstantsForMinMaxAndProcedures xmm2, xmm3, xmm6, maskIB, maskDB ; prepare constants for min max and procedure: 255 in xmm2, 0 in xmm3, mask for converting 4 packed integers to 4 packed bytes in xmm6

	loopl:
		LoadPixelFromMemory xmm1, rdx, xmm3 ; load pixel from memory to xmm1 as 4 packed integers

		vmovdqa xmm8, xmm1 ; copy pixel from xmm1 to xmm8, it will be used to copy alpha channel to result

		vcvtdq2ps xmm1, xmm1 ; convert color bytes to floats
		
		; multiply colors by multipliers and sum
		; B
		vmulps xmm13, xmm1, xmm10
		; add horizontally packed floats in xmm13
		; it is more efficient than using haddps twice
		vmovshdup    xmm0, xmm13 ; copy first float from xmm13 to xmm0
		addss       xmm13, xmm0 ; add first float to second float in xmm13
		movhlps     xmm0, xmm13 ; copy third float from xmm13 to xmm0
		addss       xmm13, xmm0 ; add third float to subtotal in xmm13

		; G
		vmulps xmm14, xmm1, xmm11
		; add horizontally packed floats in xmm14
		vmovshdup    xmm0, xmm14 ; copy first float from xmm14 to xmm0
		addss       xmm14, xmm0 ; add first float to second float in xmm14
		movhlps     xmm0, xmm14 ; copy third float from xmm14 to xmm0
		addss       xmm14, xmm0 ; add third float to subtotal in xmm14

		; R
		vmulps xmm15, xmm1, xmm12
		; add horizontally packed floats in xmm15
		vmovshdup    xmm0, xmm15 ; copy first float from xmm15 to xmm0
		addss       xmm15, xmm0 ; add first float to second float in xmm15
		movhlps     xmm0, xmm15 ; copy third float from xmm15 to xmm0
		addss       xmm15, xmm0 ; add third float to subtotal in xmm15

		insertps xmm13, xmm14, 10h ; replace second float in xmm13 with first float from xmm14, G color
		insertps xmm13, xmm15, 20h ; replace third float in xmm13 with first float from xmm15, R color

		vcvtps2dq xmm1, xmm13 ; convert floats to integers

		vpblendd xmm1, xmm1, xmm8, 08h ; copy original alpha channel to result

		XmmColorsClipping xmm1, xmm2, xmm3 ; clip colors to 0-255 range
		SavePixelToMemory xmm1, rdx, xmm6 ; save pixel to memory
		LoopNextPixel rdx, rcx, loopl, ende ; loop for next pixel
		
	ende:
		ret
SepiaFilterASM endp

; procedure change brightness of image
; every pixel is loaded from memory to xmm register, then new colors' values are calculated
; and stored back to memory
; WARNING: original memory array is overwritten
; parameters: end index of array, pointer to array, strength (float; any float number, because final pixels color results are clipped to 0-255 range), start index
; at beginning procedure sets rdx to point to start index
; before loading bytes from memory, procedure calculate component as packed integers in xmm0
; new colors' values are calculated as follows:
; old value + precalculated component
; alpha channel is not changed
; procedure uses r8, rdx, rcx, xmm0, xmm1, xmm2, xmm3, xmm6
; procedure returns nothing, but array passed as parameter is modified
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
BrightnessFilterASM proc endIndexReg: DWORD, array: QWORD, startIndex: DWORD, strength: REAL4
		; endIndexReg in rcx, array in rdx, startIndex in r8, strength in xmm3

		; jump to end if strength is 0
		ptest xmm3, xmm3
		je ende

		SkipToStartIndexAndCalculateNumberOfPixels rdx, rcx, r8 ; skip to start index and calculate number of pixels to process

		; prepare component as packed integers in xmm0; component = 255 * stength
		vmulps xmm0, xmm3, oword ptr [var255ps] ; multiply 255 by strength
		vcvtps2dq xmm0, xmm0 ; convert float to int
		pshufd xmm0, xmm0, 00h ; copy first int to all 4 ints in xmm0
						
		PrepareConstantsForMinMaxAndProcedures xmm2, xmm3, xmm6, maskIB, maskDB ; prepare constants for min max and procedure: 255 in xmm2, 0 in xmm3, mask for converting 4 packed integers to 4 packed bytes in xmm6

		pand xmm0, [mask0LD] ; put 0 to alpha channel in xmm0, component is ready to use

	loopl:
		LoadPixelFromMemory xmm1, rdx, xmm3

		paddd xmm1, xmm0 ; add prepared component to each color
 
		XmmColorsClipping xmm1, xmm2, xmm3 ; clip colors to 0-255 range
		SavePixelToMemory xmm1, rdx, xmm6 ; save pixel to memory
		LoopNextPixel rdx, rcx, loopl, ende ; loop for next pixel

	ende:
		ret
BrightnessFilterASM endp

; procedure change contrast of image
; every pixel is loaded from memory to xmm register, then new colors' values are calculated
; and stored back to memory
; WARNING: original memory array is overwritten
; parameters: end index of array, pointer to array, strength (float; any float number, because final pixels color results are clipped to 0-255 range), start index
; at beginning procedure sets rdx to point to start index
; before loading bytes from memory, procedure put packed floats = 255 to xmm0 and 0.5 to xmm8
; new colors' values are calculated as follows:
; color = ((((old value / 255.0) - 0.5) * strength) + 0.5) * 255.0
; alpha channel is not changed
; procedure uses r8, rdx, rcx, xmm0, xmm1, xmm2, xmm3, xmm6, xmm8, xmm10, xmm11
; procedure returns nothing, but array passed as parameter is modified
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
ContrastFilterASM proc endIndexReg: DWORD, array: QWORD, startIndex: DWORD, strength: REAL4
		;endIndexReg in rcx, array in rdx, startIndex in r8, strength in xmm3

		SkipToStartIndexAndCalculateNumberOfPixels rdx, rcx, r8

		; prepare constants packed floats: 255 in xmm0, 0.5 in xmm8
		vmovaps xmm0, oword ptr [var255ps] ; 255 in 4 packed floats in xmm0
		vmovaps xmm8, oword ptr [halfps] ; 0.5 in 4 packed floats in xmm8
		vshufps xmm10, xmm3, xmm3, 00h ; copy strength to all 4 floats in xmm10

		PrepareConstantsForMinMaxAndProcedures xmm2, xmm3, xmm6, maskIB, maskDB ; prepare constants for min max and procedure: 255 in xmm2, 0 in xmm3, mask for converting 4 packed integers to 4 packed bytes in xmm6

	loopl:
		LoadPixelFromMemory xmm1, rdx, xmm3 ; load pixel from memory to xmm1 as 4 packed integers

		vmovdqa xmm11, xmm1 ; copy pixel from xmm1 to xmm11, it will be used to copy alpha channel to result
						
		vcvtdq2ps xmm1, xmm1 ; convert color bytes to floats
		
		; calculate ((((color / 255.0) - 0.5) * strength) + 0.5) * 255.0
		divps xmm1, xmm0
		subps xmm1, xmm8
		mulps xmm1, xmm10
		addps xmm1, xmm8
		mulps xmm1, xmm0

		vcvtps2dq xmm1, xmm1 ; convert floats to integers

		vpblendd xmm1, xmm1, xmm11, 08h ; copy original alpha channel to result
				
		XmmColorsClipping xmm1, xmm2, xmm3 ; clip colors to 0-255 range
		SavePixelToMemory xmm1, rdx, xmm6 ; save pixel to memory
		LoopNextPixel rdx, rcx, loopl, ende ; loop for next pixel

	ende:
		ret
ContrastFilterASM endp

; procedure applying negative filter to image
; every pixel is loaded from memory to xmm register, then new colors' values are calculated
; and stored back to memory
; WARNING: original memory array is overwritten
; parameters: end index of array, pointer to array, strength, start index
; at beginning procedure sets rdx to point to start index
; new colors' values are calculated as follows:
; color = 255 - old value
; alpha channel is not changed
; procedure uses r8, rdx, rcx, xmm1, xmm2, xmm3, xmm6, xmm8
; procedure returns nothing, but array passed as parameter is modified
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
NegativeFilterASM proc endIndexReg: DWORD, array: QWORD, startIndex: DWORD
		; endIndexReg in rcx, array in rdx, startIndex in r8

		SkipToStartIndexAndCalculateNumberOfPixels rdx, rcx, r8 ; skip to start index and calculate number of pixels to process

		PrepareConstantsForMinMaxAndProcedures xmm2, xmm3, xmm6, maskIB, maskDB ; prepare constants for min max and procedure: 255 in xmm2, 0 in xmm3, mask for converting 4 packed integers to 4 packed bytes in xmm6

	loopl:
		LoadPixelFromMemory xmm1, rdx, xmm3 ; load pixel from memory to xmm1 as 4 packed integers

		vmovdqa xmm8, xmm1 ; copy pixel from xmm1 to xmm8, it will be used to copy alpha channel to result

		vpsubd xmm1, xmm2, xmm1 ; subtract 255 from each color

		vpblendd xmm1, xmm1, xmm8, 08h ; copy original alpha channel to result

		XmmColorsClipping xmm1, xmm2, xmm3 ; clip colors to 0-255 range
		SavePixelToMemory xmm1, rdx, xmm6 ; save pixel to memory
		LoopNextPixel rdx, rcx, loopl, ende ; loop for next pixel

	ende:
		ret
NegativeFilterASM endp

; procedure apply grayscale filter to image
; every pixel is loaded from memory to xmm register, then new colors' values are calculated
; and stored back to memory
; WARNING: original memory array is overwritten
; parameters: end index of array, pointer to array, start index
; at beginning procedure sets rdx to point to start index
; new colors' values are calculated as follows:
; newB = newG = newR = (inputB * 0.114 + inputG * 0.587 + inputR * 0.299)
; alpha channel is not changed
; procedure uses r8, rdx, rcx, xmm0, xmm1, xmm2, xmm3, xmm6, xmm7, xmm10
; procedure returns nothing, but array passed as parameter is modified
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
GrayScaleFilterASM proc endIndexReg: DWORD, array: QWORD, startIndex: DWORD
		; endIndexReg in rcx, array in rdx, startIndex in r8

		SkipToStartIndexAndCalculateNumberOfPixels rdx, rcx, r8

		; bgra multipliers in xmm0
		vmovaps xmm0, multipliersGS

		PrepareConstantsForMinMaxAndProcedures xmm2, xmm3, xmm6, maskIB, maskDB ; prepare constants for min max and procedure: 255 in xmm2, 0 in xmm3, mask for converting 4 packed integers to 4 packed bytes in xmm6

		vmovdqa xmm7, [mask0LD] ; mask for skipping alpha channel

	loopl:
		LoadPixelFromMemory xmm1, rdx, xmm3 ; load pixel from memory to xmm1 as 4 packed integers

		vmovdqa xmm10, xmm1 ; copy pixel from xmm1 to xmm10, it will be used to copy alpha channel to result
		pand xmm1, xmm7 ; put 0 to alpha channel in xmm1

		cvtdq2ps xmm1, xmm1 ; convert color bytes to floats

		mulps xmm1, xmm0 ; multiply each color by multiplier

		haddps xmm1, xmm1 ; add horizontally packed floats in xmm1, sum all computed values
		haddps xmm1, xmm1 ; add horizontally packed floats in xmm1

		vcvtps2dq xmm1, xmm1 ; convert floats to integers

		vpblendd xmm1, xmm1, xmm10, 08h ; copy original alpha channel to result

		XmmColorsClipping xmm1, xmm2, xmm3 ; clip colors to 0-255 range
		SavePixelToMemory xmm1, rdx, xmm6 ; save pixel to memory
		LoopNextPixel rdx, rcx, loopl, ende ; loop for next pixel

	ende:
		ret
GrayScaleFilterASM endp

; procedure apply transparency filter to image
; alpha channel of every pixel is loaded from memory, then new alpha channel value is calculated
; and stored back to memory
; WARNING: original memory array is overwritten
; parameters: end index of array, pointer to array, strength (float; value should be in range [0,1], higher and lower values will have same effect as 1 and 0), start index
; at beginning procedure sets rdx to point to start index
; new alpha value is calculated as follows:
; old value * strength
; procedure uses r8, rdx, rcx, eax, xmm0, xmm3
; procedure returns nothing, but array passed as parameter is modified
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
TransparencyFilterASM proc endIndexReg: DWORD, array: QWORD, startIndex: DWORD, strength: REAL4
		; endIndexReg in rcx, array in rdx, startIndex in r8, strength in xmm3

		SkipToStartIndexAndCalculateNumberOfPixels rdx, rcx, r8 ; skip to start index and calculate number of pixels to process

		vmulss xmm0, xmm3, dword ptr [var255ps] ; multiply xmm0 by strength
		vcvtss2si eax, xmm0 ; convert xmm0 to an integer and move it to rax
		add rdx, 3 ; skip rdx to alpha channel

	loopl:
		; copy result to memory
		mov [rdx], al ; copy alpha channel to memory

		LoopNextPixel rdx, rcx, loopl, ende ; loop for next pixel
	ende:
		ret
TransparencyFilterASM endp
end