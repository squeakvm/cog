/****************************************************************************
*   PROJECT: Squeak foreign function interface support routines
*   FILE:    sqFFI.h
*   CONTENT: Declarations for the foreign function interface support routines
*
*   AUTHORS: Andreas Raab (ar), Eliot Miranda (eem), Josh Gargus (jcg)
*   ADDRESS: Teleplace, Inc.
*   EMAIL:   {andreas.raab,eliot,josh}@teleplace.com
*
*****************************************************************************/
#ifndef SQ_FFI_H
#define SQ_FFI_H

/* Set the log file name for logging call-outs */
int ffiLogFileNameOfLength(void *nameIndex, int nameLength);
int ffiLogCallOfLength(void *nameIndex, int nameLength);

/* The following are for creating, manipulating, and destroying "manual surfaces".          
   These are surfaces that are managed by Squeak code, which has manual control
   over the memory location where the image data is stored (the pointer used may
   be obtained via FFI calls, or other means).

   Upon creation, no memory is allocated for the surface.  Squeak code is
   responsible for passing in a pointer to the memory to use.  It is OK to set
   the pointer to different values, or to NULL.  If the pointer is NULL, then
   BitBlt calls to ioLockSurface() will fail.

   createManualFunction() returns a non-negative surface ID if successful, and
   -1 otherwise.  The other return true for success, and false for failure.
*/
#include "../SurfacePlugin/SurfacePlugin.h"
void initManualSurfaceFunctionPointers(fn_ioRegisterSurface reg, fn_ioUnregisterSurface unreg, fn_ioFindSurface find);
int createManualSurface(int width, int height, int rowPitch, int depth, int isMSB);
int destroyManualSurface(int surfaceID);
int setManualSurfacePointer(int surfaceID, void* ptr);

#endif /* SQ_FFI_H */
