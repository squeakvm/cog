/*
 *  sqMacUnixCommandLineInterface.c
 *  SqueakVMForCarbon
 *
 *  Created by John M McIntosh on 3/19/05.
 *
 *   
 *   This file was part of Unix Squeak.
 * 
 *   Permission is hereby granted, free of charge, to any person obtaining a
 *   copy of this software and associated documentation files (the "Software"),
 *   to deal in the Software without restriction, including without limitation
 *   the rights to use, copy, modify, merge, publish, distribute, sublicense,
 *   and/or sell copies of the Software, and to permit persons to whom the
 *   Software is furnished to do so, subject to the following conditions:
 * 
 *   The above copyright notice and this permission notice shall be included in
 *   all copies or substantial portions of the Software.
 * 
 *   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *   AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *   LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 *   FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 *   DEALINGS IN THE SOFTWARE.
 * 
 * Much of this code comes from the unix port
 * Ian Piumarta <ian.piumarta@inria.fr>
 * 
 * 3.8.13b4  Oct 16th, 2006 JMM headless
 */

#include "sq.h"
#include "sqMacUnixCommandLineInterface.h"
#include "sqMacEncoding.h"
#include "sqMacFileLogic.h"
#include "sqMacUIConstants.h"
#include "sqMacUnixFileInterface.h"


extern CFStringEncoding gCurrentVMEncoding;
extern Boolean gSqueakHeadless;
static int    vmArgCnt=		0;	/* for getAttributeIntoLength() */
static char **vmArgVec=		0;
static int    squeakArgCnt=	0;
static char **squeakArgVec=	0;
extern       int    argCnt;	/* global copies for access from plugins */
extern       char **argVec;
extern       char **envVec;
extern UInt32 gMaxHeapSize;

static void outOfMemory(void);
static void parseArguments(int argc, char **argv);
static int parseArgument(int argc, char **argv);
static void usage(void);
static void parseEnvironment(void);
static int strtobkm(const char *str);
static void printUsage(void);
static void printUsageNotes(void);
void resolveWhatTheImageNameIs(char *guess);

char *unixArgcInterfaceGetParm(int n) {
	int actual;
	
	if (n < 0) {
		actual = -n;
		return actual < vmArgCnt ? vmArgVec[actual] : nil;
	}
	else {
		actual = n - 2;
		return actual < squeakArgCnt ? squeakArgVec[actual] : nil;
	}
}

void unixArgcInterface(int argc, char **argv, char **envp) {
#pragma unused(envp)
  if ((vmArgVec= calloc(argc + 1, sizeof(char *))) == 0)
    outOfMemory();

  if ((squeakArgVec= calloc(argc + 1, sizeof(char *))) == 0)
    outOfMemory();

  parseEnvironment();
  parseArguments(argc, argv);
}


static void parseArguments(int argc, char **argv)
{
# define skipArg()	(--argc, argv++)
# define saveArg()	(vmArgVec[vmArgCnt++]= *skipArg())

  saveArg();	/* vm name */

  while ((argc > 0) && (**argv == '-'))	/* more options to parse */
    {
      int n= 0;
      
	  if (!strcmp(*argv, "--"))	
		break; /* escape from option processing */
	  else
		 n= parseArgument(argc, argv);
	  
	  if (n == 0)			/* option not recognised */ {
		fprintf(stderr, "unknown option: %s\n", argv[0]);
		usage();
	  }
      while (n--)
	     saveArg();
    }

  if (!argc)
    return;

  if (!strcmp(*argv, "--"))
    skipArg();
  else					/* image name default to normal mac expectations */
	resolveWhatTheImageNameIs(saveArg());
  /* save remaining arguments as Squeak arguments */
  while (argc > 0)
    squeakArgVec[squeakArgCnt++]= *skipArg();

# undef saveArg
# undef skipArg
}

void resolveWhatTheImageNameIs(char *guess)  
{
	char possibleImageName[DOCUMENT_NAME_SIZE+1],  fullPath [DOCUMENT_NAME_SIZE+1],  lastPath [SHORTIMAGE_NAME_SIZE+1];
	FSRef		theFSRef;
	OSErr		err;
	
	strncpy(possibleImageName, guess,DOCUMENT_NAME_SIZE);
	err = getFSRef(possibleImageName,&theFSRef,kCFStringEncodingUTF8);
	if (err) {
		SetImageNameViaString("",gCurrentVMEncoding);
		SetShortImageNameViaString("",gCurrentVMEncoding);
		return;
	}
	PathToFileViaFSRef(fullPath,DOCUMENT_NAME_SIZE, &theFSRef,gCurrentVMEncoding);
	getLastPathComponentInCurrentEncoding(fullPath,lastPath,gCurrentVMEncoding);
	SetImageNameViaString(fullPath,gCurrentVMEncoding);
	SetShortImageNameViaString(lastPath,gCurrentVMEncoding);
}


static int parseArgument(int argc, char **argv)
{
   /* vm arguments */
  
  if      (!strcmp(argv[0], "-help"))		{ 
	usage();
	return 1; }
  else if (!strcmp(argv[0], "-version")) {
	extern char *getVersionInfo(int verbose);
	printf("%s\n", getVersionInfo(0));
	exit(0);
  }
  else if (!strncmp(argv[0], "-psn_", 5)) { return 1; }
  else if (!strcmp(argv[0], "-headless")) { gSqueakHeadless = true; return 1; }
  else if (!strcmp(argv[0], "-headfull")) { gSqueakHeadless = false; return 1; }
#if (STACKVM || NewspeakVM) && !COGVM
  else if (!strcmp(argv[0], "-sendtrace")) { extern sqInt sendTrace; sendTrace = 1; return 1; }
#endif
  else if (argc > 1) {
	  if (!strcmp(argv[0], "-memory"))	{ 
		gMaxHeapSize = strtobkm(argv[1]);	 
		return 2; }
#if STACKVM || NewspeakVM
      else if (!strcmp(argv[0], "-breaksel")) { 
		extern void setBreakSelector(char *);
		setBreakSelector(argv[1]);
		return 2; }
#endif
#if STACKVM
      else if (!strcmp(argv[0], "-eden")) { 
		extern sqInt desiredEdenBytes;
		desiredEdenBytes = strtobkm(argv[1]);	 
		return 2; }
      else if (!strcmp(argv[0], "-leakcheck")) { 
		extern sqInt checkForLeaks;
		checkForLeaks = atoi(argv[1]);	 
		return 2; }
      else if (!strcmp(argv[0], "-stackpages")) { 
		extern sqInt desiredNumStackPages;
		desiredNumStackPages = atoi(argv[1]);	 
		return 2; }
      else if (!strcmp(argv[0], "-numextsems")) { 
		ioSetMaxExtSemTableSize(atoi(argv[1]));
		return 2; }
      else if (!strcmp(argv[0], "-noheartbeat")) { 
		extern sqInt suppressHeartbeatFlag;
		suppressHeartbeatFlag = 1;
		return 1; }
      else if (!strcmp(argv[0], "-pollpip")) { 
		extern sqInt pollpip;
		pollpip = atoi(argv[1]);	 
		return 2; }
#endif /* STACKVM */
#if COGVM
      else if (!strcmp(argv[0], "-codesize")) { 
		extern sqInt desiredCogCodeSize;
		desiredCogCodeSize = strtobkm(argv[1]);	 
		return 2; }
# define TLSLEN (sizeof("-sendtrace")-1)
      else if (!strncmp(argv[0], "-sendtrace", TLSLEN)) { 
		extern int traceLinkedSends;
		char *equalsPos = strchr(argv[0],'=');

		if (!equalsPos) {
			traceLinkedSends = 1;
			return 1;
		}
		if (equalsPos - argv[0] != TLSLEN
		  || (equalsPos[1] != '-' && !isdigit(equalsPos[1])))
			return 0;

		traceLinkedSends = atoi(equalsPos + 1);
		return 1; }
      else if (!strcmp(argv[0], "-tracestores")) { 
		extern sqInt traceStores;
		traceStores = 1;
		return 1; }
      else if (!strcmp(argv[0], "-dpcso")) { 
		extern unsigned long debugPrimCallStackOffset;
		debugPrimCallStackOffset = (unsigned long)strtobkm(argv[1]);	 
		return 2; }
      else if (!strcmp(argv[0], "-cogmaxlits")) { 
		extern sqInt maxLiteralCountForCompile;
		maxLiteralCountForCompile = strtobkm(argv[1]);	 
		return 2; }
      else if (!strcmp(argv[0], "-cogminjumps")) { 
		extern sqInt minBackwardJumpCountForCompile;
		minBackwardJumpCountForCompile = strtobkm(argv[1]);	 
		return 2; }
#endif /* COGVM */
      else if (!strcmp(argv[0], "-pathenc")) { 
		setEncodingType(argv[1]); 
		return 2; }
      else if (!strcmp(argv[0], "-browserPipes")) {
		extern int		 gSqueakBrowserPipes[]; /* read/write fd for browser communication */
		extern Boolean gSqueakBrowserSubProcess;
		
		if (!argv[2]) return 0;
		sscanf(argv[1], "%i", &gSqueakBrowserPipes[0]);
		sscanf(argv[2], "%i", &gSqueakBrowserPipes[1]);
		gSqueakBrowserSubProcess = true;
		return 3;
	}
  }
  return 0;	/* option not recognised */
}

static void usage(void)
{
  printf("Usage: %s [<option>...] [<imageName> [<argument>...]]\n", argVec[0]);
  printf("       %s [<option>...] -- [<argument>...]\n", argVec[0]);
  printUsage();
  printf("\nNotes:\n");
  printf("  <imageName> defaults to `" DEFAULT_IMAGE_NAME "'.\n");
  printUsageNotes();
  exit(1);
}

static void printUsage(void)
{
  printf("\nCommon <option>s:\n");
  printf("  -help                 print this help message, then exit\n");
  printf("  -memory <size>[mk]    use fixed heap size (added to image size)\n");
#if STACKVM || NewspeakVM
  printf("  -breaksel selector    set breakpoint on send of selector\n");
#endif
#if STACKVM
  printf("  -eden <size>[mk]      set eden memory to bytes\n");
  printf("  -leakcheck num        check for leaks in the heap\n");
  printf("  -stackpages num       use n stack pages\n");
#endif
#if COGVM
  printf("  -codesize <size>[mk]  set machine code memory to bytes\n");
  printf("  -sendtrace[=num]      enable send tracing (optionally to a specific value)\n");
  printf("  -tracestores          enable store tracing (assert check stores)\n");
  printf("  -cogmaxlits <n>       set max number of literals for methods compiled to machine code\n");
  printf("  -cogminjumps <n>      set min number of backward jumps for interpreted methods to be considered for compilation to machine code\n");
#endif
  printf("  -pathenc <enc>        set encoding for pathnames (default: macintosh)\n");
  printf("  -headless             run in headless (no window) mode (default: false)\n");
  printf("  -version              print version information, then exit\n");
}

static void printUsageNotes(void)
{
  printf("  If `-memory' is not specified then the heap will grow dynamically.\n");
  printf("  <argument>s are ignored, but are processed by the " IMAGE_DIALECT_NAME " image.\n");
  printf("  The first <argument> normally names a " IMAGE_DIALECT_NAME " `script' to execute.\n");
  printf("  Precede <arguments> by `--' to use default image.\n");
}

static void outOfMemory(void)
{
  fprintf(stderr, "out of memory\n");
  exit(1);
}

static int strtobkm(const char *str)
{
  char *suffix;
  int value= strtol(str, &suffix, 10);
  switch (*suffix)
    {
    case 'k': case 'K':
      value*= 1024;
      break;
    case 'm': case 'M':
      value*= 1024*1024;
      break;
    }
  return value;
}

static void parseEnvironment(void)
{
	char *ev;

	if ((ev= getenv(IMAGE_ENV_NAME)))		
		resolveWhatTheImageNameIs(ev);
	if ((ev= getenv("SQUEAK_MEMORY")))
		gMaxHeapSize= strtobkm(ev);
	if ((ev= getenv("SQUEAK_PATHENC")))
		setEncodingType(ev);
}
