/****************************************************************************
*   PROJECT: Common include
*   FILE:    sq.h
*   CONTENT: 
*
*   AUTHOR:  
*   ADDRESS: 
*   EMAIL:   
*   RCSID:   $Id: sq.h 1283 2005-12-31 00:51:12Z rowledge $
*
*/
#include <math.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>

#include "sqConfig.h"
#include "sqMemoryAccess.h"
#include "sqVirtualMachine.h"

#define true	1
#define false	0
#define null	0  /* using "null" because nil is predefined in Think C */

/* Pluggable primitives macros. */

/* Note: All pluggable primitives are defined as
	EXPORT(int) somePrimitive(void)
   If the platform requires special declaration modifiers, the EXPORT
   macro can be redefined.
*/
#define EXPORT(returnType) returnType

/* Image save/restore macros. */

/* Note: The image file save and restore code uses these macros; they
   can be redefined in sqPlatformSpecific.h if desired. These default
   versions are defined in terms of the ANSI Standard C libraries.
*/
#define sqImageFile					   FILE *
#define sqImageFileClose(f)                  		   fclose(f)
#define sqImageFileOpen(fileName, mode)      		   fopen(fileName, mode)
#define sqImageFilePosition(f)               		   ftell(f)
#define sqImageFileRead(ptr, sz, count, f)   		   fread(ptr, sz, count, f)
#define sqImageFileSeek(f, pos)              		   fseek(f, pos, SEEK_SET)
#define sqImageFileWrite(ptr, sz, count, f)  		   fwrite(ptr, sz, count, f)
#define sqImageFileStartLocation(fileRef, fileName, size)  0

/* Platform-dependent macros for handling object memory. */

/* Note: The grow/shrink macros assume that the object memory can be extended
   continuously at its prior end. The garbage collector cannot deal with
   'holes' in the object memory so the support code needs to reserve the
   virtual maximum of pages that can be allocated beforehand. The amount of
   'extra' memory should describe the amount of memory that can be allocated
   from the OS (including swap space if the flag is set to true) and must not
   exceed the prior reserved memory.
   In other words: don't you dare to report more free space then you can
   actually allocate.
   The default implementation assumes a fixed size memory allocated at startup.
*/
#define sqAllocateMemory(minHeapSize, desiredHeapSize)  malloc(desiredHeapSize)
#define sqGrowMemoryBy(oldLimit, delta)			oldLimit
#define sqShrinkMemoryBy(oldLimit, delta)		oldLimit
#define sqMemoryExtraBytesLeft(includingSwap)		0

/* Platform-dependent memory size adjustment macro. */

/* Note: This macro can be redefined to allows platforms with a
   fixed application memory partition (notably, the Macintosh)
   to reserve extra C heap memory for special applications that need
   it (e.g., for a 3D graphics library). Since most platforms can
   extend their application memory partition at run time if needed,
   this macro is defined as a noop here and redefined if necessary
   in sqPlatformSpecific.h.
*/

#define reserveExtraCHeapBytes(origHeapSize, bytesToReserve) origHeapSize

/* Platform-dependent millisecond clock macros. */

/* Note: The Squeak VM uses two different clock functions for timing, and
   the Cog VMs provide a third.

   The primary one, ioMSecs(), is used to implement Delay and Time
   millisecondClockValue. The resolution of this clock
   determines the resolution of these basic timing functions. For
   doing real-time control of music and MIDI, a clock with resolution
   down to one millisecond is preferred, but a coarser clock (say,
   1/60th second) can be used in a pinch.

   The function ioMicroMSecs() is used only to collect timing statistics
   for the garbage collector and other VM facilities. (The function
   name is meant to suggest that the function is based on a clock
   with microsecond accuracy, even though the times it returns are
   in units of milliseconds.) This clock must have enough precision to
   provide accurate timings, and normally isn't called frequently
   enough to slow down the VM. Thus, it can use a more expensive clock
   than ioMSecs(). This function is listed in the sqVirtualMachine plugin
   support mechanism and thus needs to be a real function, even if a macro is
   use to point to it.

   There was a third form that used to be used for quickly timing primitives in
   order to try to keep millisecond delays up to date. That is no longer used.

   By default, the basic ioMSec() clock function is defined
   here as a macro based on the standard C library function clock().
   Any of this can be overridden in sqPlatformSpecific.h.

   The wall clock is answered by ioSeconds, which answers the number of seconds
   since the start of the 20th century (12pm Dec 31, 1900).

   The Cog VMs depend on a heartbeat to cause the VM to check for interrupts at
   regular intervals (of the order of ever milisecond).  The heartbeat on these
   VMs is responsible for updating a 64-bit microsecond clock with the number
   of microseconds since the start of the 20th century (12pm Dec 31, 1900)
   available via ioUTCMicroseconds() and ioLocalMicroseconds().  For cases
   where exact time is required we provide ioUTCMicrosecondsNow that updates
   the clock to return the time right now, rather than of the last heartbeat.
*/

sqInt ioMSecs(void);
/* deprecated out of existence sqInt ioLowResMSecs(void); */
sqInt ioMicroMSecs(void);

/* duplicate the generated definition in the interpreter.  If they differ the
 * compiler will complain and catch it for us.  We use 0x1FFFFFFF instead of
 * 0x3FFFFFFF so that twice the maximum clock value remains in the positive
 * SmallInteger range.  This assumes 31 bit SmallIntegers; 0x3FFFFFFF is
 * SmallInteger maxVal.
 */
#define MillisecondClockMask 0x1FFFFFFF

#if STACKVM
usqLong ioUTCMicrosecondsNow();
usqLong ioUTCMicroseconds();
usqLong ioLocalMicroseconds();
void	ioUpdateVMTimezone();
void	ioSynchronousCheckForEvents();
void	checkHighPriorityTickees(usqLong);
# if ITIMER_HEARTBEAT		/* Hack; allow heartbeat to avoid */
extern int numAsyncTickees; /* prodHighPriorityThread unless necessary */
# endif						/* see platforms/unix/vm/sqUnixHeartbeat.c */
void	ioGetClockLogSizeUsecsIdxMsecsIdx(sqInt*,void**,sqInt*,void**,sqInt*);
#endif

/* this function should return the value of the high performance
   counter if there is such a thing on this platform (otherwise return 0) */
sqLong ioHighResClock(void);

/* New filename converting function; used by the interpreterProxy function 
  ioFilenamefromStringofLengthresolveAliases. Most platforms can ignore the
  resolveAlias boolean - it seems to only be of use by OSX but is crucial there.
*/
sqInt sqGetFilenameFromString(char * aCharBuffer, char * aFilenameString, sqInt filenameLength, sqInt aBoolean);

/* Macro to provide default null behaviour for ftruncate - a non-ansi call
   used in FilePlugin.
   Override in sqPlatformSpecific.h for each platform that implements a
   file truncate, or consider replacing the
   ../Cross/plugins/FilePlugin/sqFilePluginBasicPrims.c
   file with a platform specific version as Win32 and RISC OS do. 
*/
#define sqFTruncate(filenum, fileoffset) true

/* Macros to support Mac browser plugin without ugly code in Interpreter. */

#define insufficientMemorySpecifiedError()	error("Insufficient memory for this image")
#define insufficientMemoryAvailableError()	error("Failed to allocate memory for the heap")
#define unableToReadImageError()		error("Read failed or premature end of image file")
#define browserPluginReturnIfNeeded()
#define browserPluginInitialiseIfNeeded()

/* Platform-specific header file may redefine earlier definitions and macros. */

#include "sqPlatformSpecific.h"

/* Interpreter entry points. */

/* Disable Intel compiler inlining of error which is used for breakpoints */
#pragma auto_inline off
void error(char *s);
#pragma auto_inline on
sqInt checkedByteAt(sqInt byteAddress);
sqInt checkedByteAtput(sqInt byteAddress, sqInt byte);
sqInt checkedLongAt(sqInt byteAddress);
sqInt checkedLongAtput(sqInt byteAddress, sqInt a32BitInteger);
sqInt fullDisplayUpdate(void);
sqInt interpret(void);
sqInt primitiveFail(void);
sqInt signalSemaphoreWithIndex(sqInt semaIndex);
sqInt success(sqInt);

/* Display, mouse, keyboard, time. */

sqInt ioBeep(void);
sqInt ioExit(void);
sqInt ioExitWithErrorCode(int);
sqInt ioForceDisplayUpdate(void);
sqInt ioFormPrint(sqInt bitsAddr, sqInt width, sqInt height, sqInt depth,
		  double hScale, double vScale, sqInt landscapeFlag);
sqInt ioSetFullScreen(sqInt fullScreen);
sqInt ioRelinquishProcessorForMicroseconds(sqInt microSeconds);
sqInt ioScreenSize(void);
sqInt ioScreenDepth(void);
sqInt ioSeconds(void);
sqInt ioSetCursor(sqInt cursorBitsIndex, sqInt offsetX, sqInt offsetY);
sqInt ioSetCursorWithMask(sqInt cursorBitsIndex, sqInt cursorMaskIndex, sqInt offsetX, sqInt offsetY);
sqInt ioShowDisplay(sqInt dispBitsIndex, sqInt width, sqInt height, sqInt depth,
		    sqInt affectedL, sqInt affectedR, sqInt affectedT, sqInt affectedB);
sqInt ioHasDisplayDepth(sqInt depth);
sqInt ioSetDisplayMode(sqInt width, sqInt height, sqInt depth, sqInt fullscreenFlag);

#if STACKVM
/* thread subsystem support for e.g. sqExternalSemaphores.c */
void ioInitThreads();

/* Event polling via periodic heartbeat thread. */
void  ioInitHeartbeat(void);
int   ioHeartbeatMilliseconds(void);
void  ioSetHeartbeatMilliseconds(int);
unsigned long ioHeartbeatFrequency(int);

/* Management of the external semaphore table (max size set at startup) */
#if !defined(INITIAL_EXT_SEM_TABLE_SIZE)
# define INITIAL_EXT_SEM_TABLE_SIZE 256
#endif
int   ioGetMaxExtSemTableSize(void);
void  ioSetMaxExtSemTableSize(int);

/* these are used both in the STACKVM & the COGMTVM */
# if !defined(ioCurrentOSThread)
sqOSThread ioCurrentOSThread(void);
# endif
# if !defined(ioOSThreadsEqual)
int  ioOSThreadsEqual(sqOSThread,sqOSThread);
# endif
# if !COGMTVM
extern sqOSThread ioVMThread;
# define getVMOSThread() ioVMThread
# endif
#endif /* STACKVM */
#if COGMTVM
#define THRLOGSZ 256
extern int thrlogidx;
extern char *thrlog[];

/* Debug logging that defers printing.  Use like printf, e.g.
 * TLOG("tryLockVMToIndex vmOwner = %d\n", vmOwner);
 * Requires #include "sqAtomicOps.h"
 * N.B. The following still isn't safe.  If enough log entries are made by other
 * threads after myindex is obtained but before asprintf completes we can get
 * two threads using the same entry.  But this is good enough for now.
 */
#define THRLOG(...) do { int myidx, oldidx; \
	do { myidx = thrlogidx; \
		sqCompareAndSwapRes(thrlogidx,myidx,(myidx+1)&(THRLOGSZ-1),oldidx); \
	} while (myidx != oldidx); \
	if (thrlog[myidx]) free(thrlog[myidx]); \
	asprintf(thrlog + myidx, __VA_ARGS__); \
} while (0)

extern sqOSThread getVMOSThread();
/* Please read the comment for CogThreadManager in the VMMaker package for
 * documentation of this API.  N.B. code is included from sqPlatformSpecific.h
 * before the code here.  e.g.
 * # include <pthread.h>
 * # define sqOSThread pthread_t
 * # define sqOSSemaphore pthread_cond_t
 * # define ioOSThreadsEqual(a,b) pthread_equal(a,b)
 */
# if !defined(ioGetThreadLocalThreadIndex)
long ioGetThreadLocalThreadIndex(void);
# endif
# if !defined(ioSetThreadLocalThreadIndex)
void ioSetThreadLocalThreadIndex(long);
# endif

# if !defined(ioNewOSThread)
int  ioNewOSThread(void (*func)(void *), void *);
# endif
# if !defined(ioExitOSThread)
void ioExitOSThread(sqOSThread thread);
# endif
# if !defined(ioReleaseOSThreadState)
void ioReleaseOSThreadState(sqOSThread thread);
# endif
# if !defined(ioOSThreadIsAlive)
int  ioOSThreadIsAlive(sqOSThread);
# endif
int  ioNewOSSemaphore(sqOSSemaphore *);
# if !defined(ioDestroyOSSemaphore)
void ioDestroyOSSemaphore(sqOSSemaphore *);
# endif
void ioSignalOSSemaphore(sqOSSemaphore *);
void ioWaitOnOSSemaphore(sqOSSemaphore *);
int  ioNumProcessors(void);
# if !defined(ioTransferTimeslice)
void ioTransferTimeslice(void);
# endif
#endif /* COGMTVM */

/* Profiling. */
void  ioProfileStatus(sqInt *running, void **exestartpc, void **exelimitpc,
					  void **vmhst, long *nvmhbin, void **eahst, long *neahbin);
void  ioControlProfile(int on, void **vhp, long *nvb, void **ehp, long *neb);
long  ioControlNewProfile(int on, unsigned long buffer_size);
void  ioNewProfileStatus(sqInt *running, long *buffersize);
long  ioNewProfileSamplesInto(void *sampleBuffer);
void  ioClearProfile(void);

/* Power management. */

sqInt ioDisablePowerManager(sqInt disableIfNonZero);

/* User input recording I:
   In general, either set of input function can be supported,
   depending on the platform. This (first) set is state based
   and should be supported even on platforms that make use
   of the newer event driven API to support older images 
   without event support.
*/

sqInt ioGetButtonState(void);
sqInt ioGetKeystroke(void);
sqInt ioMousePoint(void);
sqInt ioPeekKeystroke(void);
/* Note: In an event driven architecture, ioProcessEvents is obsolete.
   It can be implemented as a no-op since the image will check for
   events in regular intervals. */
sqInt ioProcessEvents(void);


/* User input recording II:
   The following functions and definition can be used on
   platform supporting events directly.
*/

/* Event types. */
#define EventTypeNone		0
#define EventTypeMouse		1
#define EventTypeKeyboard	2
#define EventTypeDragDropFiles	3
#define EventTypeMenu		4
#define EventTypeWindow		5
#define	EventTypeComplex	6

/* Keypress state for keyboard events. */
#define EventKeyChar	0
#define EventKeyDown	1
#define EventKeyUp	2

/* Button definitions. */
#define RedButtonBit	4
#define YellowButtonBit	2
#define BlueButtonBit	1

/* Modifier definitions. */
#define ShiftKeyBit	1
#define CtrlKeyBit	2
#define OptionKeyBit	4
#define CommandKeyBit	8

/* generic input event */
typedef struct sqInputEvent
{
  int type;			/* type of event; either one of EventTypeXXX */
  unsigned int timeStamp;	/* time stamp */
  /* the interpretation of the following fields depend on the type of the event */
  int unused1;
  int unused2;
  int unused3;
  int unused4;
  int unused5;
  int windowIndex;		/* SmallInteger used in image to identify a host window structure */
} sqInputEvent;

/* mouse input event */
typedef struct sqMouseEvent
{
  int type;			/* EventTypeMouse */
  unsigned int timeStamp;	/* time stamp */
  int x;			/* mouse position x */
  int y;			/* mouse position y */
  int buttons;			/* combination of xxxButtonBit */
  int modifiers;		/* combination of xxxKeyBit */
  int nrClicks;			/* number of clicks in button downs - was reserved1 */
  int windowIndex;		/* host window structure */
} sqMouseEvent;

/* keyboard input event */
typedef struct sqKeyboardEvent
{
  int type;			/* EventTypeKeyboard */
  unsigned int timeStamp;	/* time stamp */
  int charCode;			/* character code in Mac Roman encoding */
  int pressCode;		/* press code; any of EventKeyXXX */
  int modifiers;		/* combination of xxxKeyBit */
  int utf32Code;		/* UTF-32 unicode value */
  int reserved1;		/* reserved for future use */
  int windowIndex;		/* host window structure */
} sqKeyboardEvent;

/* drop files event */
typedef struct sqDragDropFilesEvent
{
  int type;			/* EventTypeDropFiles */
  unsigned int timeStamp;	/* time stamp */
  int dragType;			/* one of DragXXX (see below) */
  int x;			/* mouse position x */
  int y;			/* mouse position y */
  int modifiers;		/* combination of xxxKeyBit */
  int numFiles;			/* number of files in transaction */
  int windowIndex;		/* host window structure */
} sqDragDropFilesEvent;

#define DragEnter	1 /* drag operation from OS entered Squeak window	 */
#define DragMove	2 /* drag operation from OS moved within Squeak window */
#define DragLeave	3 /* drag operation from OS left Squeak window	 */
#define DragDrop	4 /* drag operation dropped contents onto Squeak.      */
#define DragRequest	5 /* data request from other app. */

/* menu event */
typedef struct sqMenuEvent
{
  int type;			/* type of event; EventTypeMenu */
  unsigned int timeStamp;	/* time stamp */
  /* the interpretation of the following fields depend on the type  of the event */
  int menu;			/* platform-dependent to indicate which menu was picked */
  int menuItem;			/* given a menu having 1 to N items this maps to the menu item number */
  int reserved1;		/* reserved for future use */
  int reserved2;		/* reserved for future use */
  int reserved3;		/* reserved for future use */
  int windowIndex;		/* host window structure */
} sqMenuEvent;

/* window action event */
typedef struct sqWindowEvent
{
  int type;			/* type of event;  EventTypeWindow */
  unsigned int timeStamp;	/* time stamp */
  /* the interpretation of the following fields depend on the type  of the event */
  int action;		        /* one of WindowEventXXX (see below) */
  int value1;			/* used for rectangle edges */
  int value2;			/* used for rectangle edges */
  int value3;			/* used for rectangle edges */
  int value4;			/* used for rectangle edges */
  int windowIndex;		/* host window structure */
} sqWindowEvent;

#define WindowEventMetricChange	1 /* size or position of window changed - value1-4 are left/top/right/bottom values */
#define WindowEventClose	2 /* window close icon pressed */
#define WindowEventIconise	3 /* window iconised or hidden etc */
#define WindowEventActivated	4 /* window made active - some platforms only - do not rely upon this */
#define WindowEventPaint	5 /* window area (in value1-4) needs updating. Some platforms do not need to send this, do not rely on it in image */
#define WindowEventStinks	6 /* this window stinks (just to see if people read this stuff) */

typedef struct sqComplexEvent
	{
		int type;			/* type of event;  EventTypeComplex */
		unsigned int timeStamp;	/* time stamp */
		/* the interpretation of the following fields depend on the type  of the event */
		int action;		        /* one of ComplexEventXXX (see below) */
		usqInt objectPointer;	/* used to point to object */
		int unused1;			/*  */
		int unused2;			/*  */
		int unused3;			/*  */
		int windowIndex;	/* host window structure */
	} sqComplexEvent;

#define ComplexEventTypeTouchsDown	1 /*  */
#define ComplexEventTypeTouchsUp	2 /*  */
#define ComplexEventTypeTouchsMoved	3 /*  */
#define ComplexEventTypeTouchsStationary 4 /*  */
#define ComplexEventTypeTouchsCancelled	5 /*  */
#define ComplexEventTypeAccelerationData	6 /*  */
#define ComplexEventTypeLocationData	7 /*  */
#define ComplexEventTypeApplicationData	8 /*  */


/* Set an asynchronous input semaphore index for events. */
sqInt ioSetInputSemaphore(sqInt semaIndex);
/* Retrieve the next input event from the OS. */
sqInt ioGetNextEvent(sqInputEvent *evt);

/* Image file and VM path names. */
extern char imageName[];
char *getImageName(void);
sqInt imageNameGetLength(sqInt sqImageNameIndex, sqInt length);
sqInt imageNamePutLength(sqInt sqImageNameIndex, sqInt length);
sqInt imageNameSize(void);
sqInt vmPathSize(void);
sqInt vmPathGetLength(sqInt sqVMPathIndex, sqInt length);
char* ioGetLogDirectory(void);
char* ioGetWindowLabel(void);

/* Image security traps. */
sqInt ioCanRenameImage(void);
sqInt ioCanWriteImage(void);
sqInt ioDisableImageWrite(void);

/* Save/restore. */
/* Read the image from the given file starting at the given image offset */
sqInt readImageFromFileHeapSizeStartingAt(sqImageFile f, usqInt desiredHeapSize, squeakFileOffsetType imageOffset);

/* Clipboard (cut/copy/paste). */
sqInt clipboardSize(void);
sqInt clipboardReadIntoAt(sqInt count, sqInt byteArrayIndex, sqInt startIndex);
sqInt clipboardWriteFromAt(sqInt count, sqInt byteArrayIndex, sqInt startIndex);


/* Interpreter entry points needed by compiled primitives. */
void *arrayValueOf(sqInt arrayOop);
sqInt checkedIntegerValueOf(sqInt intOop);
void *fetchArrayofObject(sqInt fieldIndex, sqInt objectPointer);
double fetchFloatofObject(sqInt fieldIndex, sqInt objectPointer);
sqInt fetchIntegerofObject(sqInt fieldIndex, sqInt objectPointer);
double floatValueOf(sqInt floatOop);
sqInt pop(sqInt nItems);
sqInt pushInteger(sqInt integerValue);
sqInt sizeOfSTArrayFromCPrimitive(void *cPtr);
sqInt storeIntegerofObjectwithValue(sqInt fieldIndex, sqInt objectPointer, sqInt integerValue);

/* System attributes. */
sqInt attributeSize(sqInt indexNumber);
sqInt getAttributeIntoLength(sqInt indexNumber, sqInt byteArrayIndex, sqInt length);

/*** Pluggable primitive support. ***/

/* NOTE: The following functions are those implemented by sqNamedPrims.c */
void *ioLoadExternalFunctionOfLengthFromModuleOfLength(sqInt functionNameIndex, sqInt functionNameLength,
						       sqInt moduleNameIndex, sqInt moduleNameLength);
sqInt  ioUnloadModuleOfLength(sqInt moduleNameIndex, sqInt moduleNameLength);
void  *ioLoadFunctionFrom(char *functionName, char *pluginName);
sqInt  ioShutdownAllModules(void);
sqInt  ioUnloadModule(char *moduleName);
sqInt  ioUnloadModuleOfLength(sqInt moduleNameIndex, sqInt moduleNameLength);
char  *ioListBuiltinModule(sqInt moduleIndex);
char  *ioListLoadedModule(sqInt moduleIndex);
/* The next two are FFI entries! (implemented in sqNamedPrims.c as well) */
void  *ioLoadModuleOfLength(sqInt moduleNameIndex, sqInt moduleNameLength);
void  *ioLoadSymbolOfLengthFromModule(sqInt functionNameIndex, sqInt functionNameLength, void *moduleHandle);

/* The next three functions must be implemented by sqXYZExternalPrims.c */
/* ioLoadModule:
	Load a module from disk.
	WARNING: this always loads a *new* module. Don't even attempt to find
	a loaded one.
	WARNING: never primitiveFail() within, just return 0
*/
void *ioLoadModule(char *pluginName);

/* ioFindExternalFunctionIn:
	Find the function with the given name in the moduleHandle.
	WARNING: never primitiveFail() within, just return 0.
*/
void *ioFindExternalFunctionIn(char *lookupName, void *moduleHandle);

/* ioFreeModule:
	Free the module with the associated handle.
	WARNING: never primitiveFail() within, just return 0.
*/
sqInt ioFreeModule(void *moduleHandle);

/* The Squeak version from which this interpreter was generated. */
extern const char *interpreterVersion;
