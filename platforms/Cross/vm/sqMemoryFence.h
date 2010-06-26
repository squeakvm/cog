/* sqMemoryFence.h
 *	Support for synchronisation above weakly-ordered memory models.
 *
 *	Author: Eliot Miranda
 *
 *	Copyright (C) 2010 by Teleplace, Inc.
 *
 *	All rights reserved.
 *   
 *   This file is part of Squeak.
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
 */

/* sqLowLevelMFence is required to ensure that after completion any stores made
 * by other threads before a fence will be visible to the calling thread, and
 * vice verce.  Writers follow a write with a fence. Readers use a fence before
 * reading.  See e.g. http://en.wikipedia.org/wiki/Memory_barrier
 *
 * The only implementation directly uses mfence on x86.
 * Please add implementations for other ISAs as required.
 */

#if defined(__GNUC__) && (defined(i386) || defined(__i386) || defined(__i386__) || defined(_X86_))
# if defined(__MINGW32__) && !__SSE2__
	/* Andreas is fond of the gcc 2.95 MINGW but it lacks sse2 support */
#	define sqLowLevelMFence() asm volatile (".byte 0x0f;.byte 0xae;.byte 0xf0")
# else
#	define sqLowLevelMFence() asm volatile ("mfence")
# endif
#else
# if !defined(sqLowLevelMFence)
extern void sqLowLevelMFence(void);
# endif
#endif
