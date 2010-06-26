'From Squeak3.3alpha of 12 January 2002 [latest update: #4934] on 16 August 2002 at 10:56:49 pm'!
"Change Set:		RePlugin3.3
Date:			16 August 2002
Author:			acg

Perl-Style Regular Expressions in Smalltalk
by Andrew C. Greenberg

Version 3.3beta

I.  Regular Expressions in General

	Regular expressions are a language for specifying text to ease the searching and manipulation of text.  A complete discussion of regular expressions is beyond the scope of this document.  See Jeffrey Friedl, Mastering Regular Expressions, by O'Reilly for a relatively complete.  The regular expressions supported by this package are similar to those presently used in Perl 5.05 and Python, and are based upon Philip Hazel's excellent PCRE libraries (incorporated almost without change, subject to a free license described in Re aLicenseComment.  Thanks are due to Markus Kohler and Stephen Pair for their assistance in the initial ports of early versions of the Plugin.

An explanation of the expressions available in this package are summarized in Re aRegexComment, Re anOptionsComment and Re aGlobalSearchComment.

A more detailed description of RePlugin is available downloading the file 'RePluginDoco,' which can be obtained from http://www.gate.net/~werdna/RePlugin.html, into your default directory, and then executing

		Utilities reconstructTextWindowsFromFileNamed: 'RePluginDoco'

II. Overview of the 'Package.'

	The following new classes are provided:

		Class					Description of Instances
		----------------------		-------------------------------------------------------------------
		Re						A regular expression matching engine
		ReMatch				Result of a search using Re
		RePattern				Deprecated engine class from earlier plugin versions
		RePlugin				The Plugin 'Glue' to the PCRE Library.

		String					Various new messages were added to String, which are
								the principal means for users to access the package. 

PluginCodeGenerator has been deleted from the packgage.


III. Some Examples.

	A. Simple Matching and Querying of Matches

	To search a string for matches in a regular expression, use String reMatch:

		'just trying to catch some zzz''s before noon' matchRe: 'z+'

which returns true if matched, and false otherwise.  If more information from a positive search result is desired, the method reMatch will return a ReMatch object corresponding to the result.

		'just trying to catch some zzz''s before noon' reMatch: 'z+'

The match object can be queried in various ways.  For example, to obtain details when parenthetical phrases of a regular expression are captured:

		|m|
		m _ 'Andy was born on 10/02/1957, and not soon enough!!'
			reMatch: '(\d\d)/(\d\d)/((19|20)?\d\d)'.
		m matches

answers with:
	
		('10' '02' '1957' '19' )

The first message answers a ReMatch m representing the result of a search of the string for matches of re (nil would be returned if no match was found).  The third message answered a collection of the parenthetical subgroups matched, each showing the day, month and year as extracted from the string.

	B. Global Matching and String Substitutions

	You can perform global searches to repeatedly search a string for non-overlapping occurrences of a pattern by using reMatch:collect:  For example,

		'this is a test' collectRe: '\w+'

can be used to gather a collection of all words in the search string, answering:

		OrderedCollection ('this' 'is' 'a' 'test' )

For slightly more complex collections, you can use #reMatch:andCollect:  Additionally, you can perform global searches with text substitutions using reMatch:sub:  For example,

		'this is a test' reMatch: '\w+' andReplace: [:m | '<', (m match), '>']  

can be used to replace every word in the search string with the word enclosed by matching brackets, answering:

		'<this> <is> <a> <test>'

Further examples and documentation can be found in the references above, and in the comments and definitions set forth in ReMatch, RePattern and String.
"!

Object subclass: #Re
	instanceVariableNames: 'pattern compiledPattern isAnchored isCaseSensitive isDollarEndOnly isDotIncludesNewline isExtended isExtra isMultiline isBeginningOfLine isEndOfLine isGreedy '
	classVariableNames: ''
	module: #(Werdna Re)!

!Re commentStamp: '<historical>' prior: 0!
Perl-Style Regular Expressions in Smalltalk

Documentation

The documentation category of this method contains substantial documentation on the operation of this Class.

	Re aGeneralComment
	Re aGlobalSearchComment
	Re aRegexComment
	Re aRegexGoryDetailsComment
	Re aVersionsComment
	Re anReComment
	Re anReOverviewComment

�	Re aLicenseComment	


Examples:

	(Re on: 'a.*y') search: 'Candy is dandy.'
	'a.*y' asRe search: 'Candy is dandy.'
	'Candy is dandy' reMatch: 'a.*y'

	(Re on: '\w+') searchAndCollect: 'Candy is dandy.'
	'\w+' asRe searchAndCollect: 'Candy is dandy.'
	'Candy is dandy.' reMatch: '\w+' andCollect: [:m | m match]

Structure:
 pattern 		String -- the string with the regular expression source code
 compiledPattern RePlugin representing a compiled pattern
 isAnchored		Boolean -- representing an option setting
 is ...			Booleans -- for the other options below

List ofcommon public methods:

#opt:

	sets options using Perl-style string

#beAnchored 			#beNotAnchored				#isAnchored			#isAnchored:
#beBeginningOfLine 	#beNotBeginningOfLine 		#isBeginningOfLine	#isBeginningOfLine:
#beCaseSensitive 		#beNotCaseSensitive 			#isCaseSensitive		#isCaseSensitive:
#beDollarEndOnly 		#beNotDollarEndOnly 		#isDollarEndOnly	#isDollarEndOnly:
#beDotIncludesNewline 	#beNotDotIncludesNewline 	#isDotIncludesNewLine #isDotIncludesNewline:
#beEndOfLine 			#beNotEndOfLine 			#isEndOfLine		#isEndOfLine:
#beExtended 			#beNotExtended 				#isExtended			#isExtended:
#beExtra 				#beNotExtra 				#isExtra				#isNotExtra:
#beGreedy 				#beNotGreedy 				#isGreedy			#isGreedy:
#beMultiline 			#beNotMultiline 			#isMultiline			#isMultiline:

	Getters and setters for options in traditional Smalltalk style

search: aTargetString
search aTargetString from: startInteger to: endInteger

	Compiling the pattern, if necessary, search a string (or substring) using the pattern.  Answers nil if no match.  

searchAndCollect: aTargetString
search: aTargetString andCollect: aBlock
search: aTargetString andCollect: aBlock matchCount: anInteger

	Compiling the pattern, if necessary, gather all (or, if specified, the first anInteger) non-overlapping matches to me in aTargetString. Answer a collection of the results of applying aBlock to each ReMatch result.

search: aTargetString andReplace: aBlock
search: aTargetString andReplace: aBlock matchCount: anInteger

	Compiling the pattern, if necessary, find all (or, if specified, the first anInteger) non-overlapping matches to me in aTargetString.  Answer a new string, created by substituting the results of applying aBlock to each ReMatch result for the matched substring.

	
!
]style[(44 16 109 1 1 18 2 23 2 16 2 27 2 19 2 14 2 22 3 1 19 1 12 280 11 236 30 1 6 40 687 66 8 13 8 13 7 12 5 10 1 118 18 13 9 13 13 6 9 13 13 6 13 9 1 217 8 13 13 6 9 13 13 6 13 9 1 266)bf3,bf1,f1,bf1,f1,f1LRe aGeneralComment;,f1,f1LRe aGlobalSearchComment;,f1,f1LRe aRegexComment;,f1,f1LRe aRegexGoryDetailsComment;,f1,f1LRe aVersionsComment;,f1,f1LRe anReComment;,f1,f1LRe anReOverviewComment;,bf1,f1,f1LRe aLicenseComment;,f1,bf1,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1!

Re class
	instanceVariableNames: ''!
Object subclass: #ReMatch
	instanceVariableNames: 'matchArray re searchString pos endpos '
	classVariableNames: ''
	module: #(Werdna Re)!

!ReMatch commentStamp: '<historical>' prior: 0!
ReMatch: Perl-Style Regular Expression Search Results

I. Introduction

This Class is part of a package of classes providing a Smalltalk wrapper to Philip Hazel's excellent PCRE library.  The Plugin interface and Smalltalk wrapper was written by Andrew C. Greenberg.  As discussed in RePattern aGeneralComment, the functionality is essentially embodied in this class, Class RePattern and certain new messages in Class String.  A summary of the regular expression syntax can be found in RePattern aRegexComment and a summary of the compile option codes available can be found in RePattern anOptionsComment.

A more detailed description of RePlugin is available downloading the file 'RePluginDoco,' which can be obtained from http://www.gate.net/~werdna/RePlugin.html, into your default directory, and then executing

		Utilities reconstructTextWindowsFromFileNamed: 'RePluginDoco'

II. Principal Match Results

The substring of searchString matched by re is given by:

		m match

which can be derived from searchString as follows:

		m searchString
			copyFrom: (m from)
			to: (m to)

III. Captured Groups (and Collections of Captured Groups)

The number of substrings capturable by a parenthetical grouping in an re (regardless of the number actually matched to create m) is given by:

		m numGroups

The string captured by parenthetical grouping i, where 1<=i<=(m numGroups) is given by

		m matchAt: i

and this can be generated as follows:

		m searchString
			copyFrom: (m fromAt: i)
			to: (m toAt: i)

And an array of size (m numGroups) can be generated from strings and indices accordingly:

		m matches
		m froms
		m tos
!
]style[(53 2 15 214 25 65 9 103 23 69 26 120 41 53 61 2 27 177 57 488)bf3,f1,bf2,f1,f1LRePattern aGeneralComment;,f1,f1LRePattern Comment;,f1,f1LRePattern aRegexComment;,f1,f1LRePattern anOptionsComment;,f1,f1Rhttp://www.gate.net/~werdna/RePlugin.html;,f1,f1dUtilities reconstructTextWindowsFromFileNamed: 'RePluginDoco';;,f1,bf2,f1,bf2,f1!

ReMatch class
	instanceVariableNames: ''!
Object subclass: #RePattern
	instanceVariableNames: 'pattern compileOptions pcrePointer extraPointer errorString offset matchOptions matchSpace lastMatchResult '
	classVariableNames: ''
	module: #(Werdna Re)!

!RePattern commentStamp: '<historical>' prior: 0!
RePattern: Compiled Perl-Style Regular Expressions

I.  Introduction.

This Smalltalk implementation of modern Perl-Style regular expressions was compiled by Andrew Greenberg <werdna@gate.net> and contributors, based upon the excellent PCRE library by Philip Hazel. As discussed in RePattern aGeneralComment, the functionality is essentially embodied in this class, Class ReMatch and certain new messages in Class String.  A summary of the regular expression syntax can be found in RePattern aRegexComment and a summary of the compile option codes available can be found in RePattern anOptionsComment.

A substantially more detailed description of RePlugin is available downloading the file "RePluginDoco," which can be obtained from http://www.gate.net/~werdna/RePlugin.html, into your default directory, and then executing

		Utilities reconstructTextWindowsFromFileNamed: 'RePluginDoco'

II.  To Search a String or Substring For Pattern Matches (Once Only):

Examples:

		'Squeak or Squawk!!' reMatch: '^Squ(ea|aw)k'
		'Squeak or Squawk!!' reMatch: '^Squ(ea|aw)k' opt: 'imsxABEXZ'
		'Squeak or Squawk!!' reMatch: '^Squ(ea|aw)k!!' from: 11

more generally,
		
		srchStr reMatch: patStr [opt: oStr] [from: start] [to: stop]

For a one-time search of a string (or substring) for occurences of a match pattern.  The message will be answered with nil (if there is no match) or an instance of ReMatch, which can then be queried for further details about the match.

III. Global Searching and Replacing

	The re package provides rudimentary facilities for global searches and replacements on a string.  The following expressions

		'\w+' reMatch: 'this is a test' collect: [:m | m]
		(RePattern on: '\w+') search: 'this is a test' collect: [:m | m]

return an ordered collection of the results of repeated non-overlapping applications of the pattern to the string, or nil if there are no matches in the string.  To produce a list of matched strings, you can for example execute the following:

		'\w+' reMatch: 'this is a test' collect: [:m| m match]
		(RePattern on: '\w+') search: 'this is a test' collect: [:m | m match]

You can also perform global search and string replacements, where the answer is a string with unmatched text left alone, and matched text replaced by the result of a call to a Block passed the ReMatch object as a single parameter.  For example,

		('\w+' reMatch: 'this is a test' sub: [:m| '<', (m match), '>']
and
		(RePattern on: '\w+') search: 'this is a test' sub: [:m| '<', (m match), '>']

return a string with each nonblank word surrounded by angle brackets.  For more details, see RePattern aGlobalSearchComment.

IV. To Create Compiled Regular Expression Objects (For Repeated Matching):

		'^Squ(ea|aw)k!!$' asRePattern
		'^Squ(ea|aw)k!!$' asRePatternOpt: 'imsxAEX'
		'^Squ(ea|aw)k!!$' asRePatternOpt: 'imsxAEX' onErrorRun: aBlock

		RePattern on: '^Squ(ea|aw)k!!$'
		RePattern on: '^Squ(ea|aw)k!!$' opt: 'imsxAEX'
		RePattern 
			on: '^Squ(ea|aw)k!!$' 
			opt: 'imsxAEX' 
			onErrorRun: [:pat :offset :message | "your code here" ]

	Each of the preceding expressions returns an instance of RePattern, compiled for efficient  matching when the pattern is repeatedly searched against different strings.  RePattern ordinarily caches a dozen or so of the most recently compiled patterns, but nevertheless invokes a cost for the table lookup.  To avoid compile and lookup costs, use the above messages.  To perform a one-time search, see above.

V. To Search a Compiled Regexp Against A String or Substring for Matches:

		searchString reMatch: re [from: from] [to: to] [opt: optStr]
or
		re search: searchString [from: from] [to: to] [opt: optStr]

Examples:

		'Squeak or Squawk' reMatch: re.
		re search: 'Squeak or Squawk!!'.
		re search: 'Squeak or Squawk!!' opt: 'ABZ'.

If no match is found, these messages answer nil.  Otherwise, they answer with a corresponding instance of ReMatch.!
]style[(50 1 1 17 2 211 25 65 7 103 23 69 26 1 2 131 41 53 61 2 69 499 36 2 1110 30 3 75 749 73 362 7 1)bf3,f3,f1b,bf2,f1b,f1,f1LRePattern aGeneralComment;,f1,f1LReMatch Comment;,f1,f1LRePattern aRegexComment;,f1,f1LRePattern anOptionsComment;,f1,f1b,f1,f1Rhttp://www.gate.net/~werdna/RePlugin.html;,f1,f1dUtilities reconstructTextWindowsFromFileNamed: 'RePluginDoco';;,f1,bf2,f1,bf2,bf1,f1,f1LRePattern aGlobalSearchComment;,f1,f1b,f1,f1b,f1,f1LReMatch Comment;,f1!

RePattern class
	instanceVariableNames: 'Patterns Options CompileObjects Front '!
TestInterpreterPlugin subclass: #RePlugin
	instanceVariableNames: 'netMemory numAllocs numFrees lastAlloc patternStr rcvr compileFlags pcrePtr extraPtr errorStr errorOffset matchFlags patternStrPtr errorStrBuffer '
	classVariableNames: ''
	module: #(Werdna Re)!

!RePlugin commentStamp: '<historical>' prior: 0!
/*	Regular Expression Plugin (This class comment becomes part of rePlugin.c)

	RePlugin translate: 'RePlugin.c' doInlining: true.

See documentation and source code for the PCRE C Library Code.  This plugin is designed to serve an object such as RePattern:

	patternStr		A 0-terminated string comprising the pattern to be compiled.
	compileFlags	An Integer representing re compiler options
	PCREBuffer		A ByteArray of regular expression bytecodes
	extraPtr			A ByteArray of match optimization data (or nil)
	errorString		A String Object For Holding an Error Message (when compile failed)
	errorOffset		The index in patternStr (0-based) where the error ocurred (when compile failed)
	matchFlags		An Integer representing re matcher options
	matchSpaceObj	An Integer array for match results and workspace during matching.

The instance variables must appear in the preceding order.  MatchSpaceObj must be allocated by the calling routine and contain at least 6*(numGroups+1) bytes.
*/
#include "pcre.h"
#include "internal.h"

/* Slight machine-specific hack for MacOS Memory Management */
#ifdef TARGET_OS_MAC
#define	malloc(ptr) NewPtr(ptr)
#define free(ptr) DisposePtr(aPointer)
#endif

/* Adjust malloc and free routines as used by PCRE */
void rePluginFree(void * aPointer);
void * rePluginMalloc(size_t anInteger);
void *(*pcre_malloc)(size_t) = rePluginMalloc;
void  (*pcre_free)(void *) = rePluginFree;
!

RePlugin class
	instanceVariableNames: ''!
TestCase subclass: #ReTest
	instanceVariableNames: ''
	classVariableNames: ''
	module: #(Werdna Re)!
ReTest class
	instanceVariableNames: ''!

!Re methodsFor: 'documentation' stamp: 'acg 8/3/2002 22:57'!
aGeneralComment "

Perl-Style Regular Expressions in Smalltalk
by Andrew C. Greenberg

I.  Regular Expressions in General

	Regular expressions are a language for specifying text to ease the searching and manipulation of text.  A complete discussion of regular expressions is beyond the scope of this document.  See Jeffrey Friedl, Mastering Regular Expressions, by O'Reilly for a relatively complete.  The regular expressions supported by this package are similar to those presently used in Perl 5.05 and Python, and are based upon Philip Hazel's excellent PCRE libraries (incorporated almost without change, subject to a free license described in Re aLicenseComment.  Thanks are due to Markus Kohler and Stephen Pair for their assistance in the initial ports of early versions of the Plugin.

An explanation of the expressions available in this package are summarized in Re aRegexComment, Re anOptionsComment and Re aGlobalSearchComment.

A more detailed description of RePlugin is available downloading the file 'RePluginDoco,' which can be obtained from http://www.gate.net/~werdna/RePlugin.html, into your default directory, and then executing

		Utilities reconstructTextWindowsFromFileNamed: 'RePluginDoco'

II. Overview of the 'Package.'

	The following new classes are provided:

		Class					Description of Instances
		----------------------		-------------------------------------------------------------------
		Re						A regular expression matching engine
		ReMatch				Result of a search using Re
		RePattern				Deprecated engine class from earlier plugin versions
		RePlugin				The Plugin 'Glue' to the PCRE Library.

		String					Various new messages were added to String, which are
								the principal means for users to access the package. 

PluginCodeGenerator has been deleted from the packgage.


III. Some Examples.

	A. Simple Matching and Querying of Matches

	To search a string for matches in a regular expression, use String reMatch:

		'just trying to catch some zzz''s before noon' matchRe: 'z+'

which returns true if matched, and false otherwise.  If more information from a positive search result is desired, the method reMatch will return a ReMatch object corresponding to the result.

		'just trying to catch some zzz''s before noon' reMatch: 'z+'

The match object can be queried in various ways.  For example, to obtain details when parenthetical phrases of a regular expression are captured:

		|m|
		m _ 'Andy was born on 10/02/1957, and not soon enough!!'
			reMatch: '(\d\d)/(\d\d)/((19|20)?\d\d)'.
		m matches

answers with:
	
		('10' '02' '1957' '19' )

The first message answers a ReMatch m representing the result of a search of the string for matches of re (nil would be returned if no match was found).  The third message answered a collection of the parenthetical subgroups matched, each showing the day, month and year as extracted from the string.

	B. Global Matching and String Substitutions

	You can perform global searches to repeatedly search a string for non-overlapping occurrences of a pattern by using reMatch:collect:  For example,

		'this is a test' collectRe: '\w+'

can be used to gather a collection of all words in the search string, answering:

		OrderedCollection ('this' 'is' 'a' 'test' )

For slightly more complex collections, you can use #reMatch:andCollect:  Additionally, you can perform global searches with text substitutions using reMatch:sub:  For example,

		'this is a test' reMatch: '\w+' andReplace: [:m | '<', (m match), '>']  

can be used to replace every word in the search string with the word enclosed by matching brackets, answering:

		'<this> <is> <a> <test>'

Further examples and documentation can be found in the references above, and in the comments and definitions set forth in ReMatch, RePattern and String.
"!
]style[(19 44 24 34 211 29 289 18 206 16 2 19 5 24 119 41 53 61 2 30 224 7 34 9 59 8 46 6 179 65 291 7 442 7 268 43 832 7 2 9 5 7 2)f1b,bf3,bf2,bf1,f1,f1i,f1,f1LRe aLicenseComment;,f1,f1LRe aRegexComment;,f1,f1LRe anOptionsComment;,f1,f1LRe aGlobalSearchComment;,f1,f1Rhttp://www.gate.net/~werdna/RePlugin.html;,f1,f1dUtilities reconstructTextWindowsFromFileNamed: 'RePluginDoco';;,f1,f1b,f1,f1LReMatch Comment;,f1,f1LRePattern Comment;,f1,f1LRePlugin Comment;,f1,f1LString Comment;,f1,f1b,f1,f1LReMatch Comment;,f1,f1LReMatch Comment;,f1,f1b,f1,f1LReMatch Comment;,f1,f1LRePattern Comment;,f1,f1LString Comment;,f1! !

!Re methodsFor: 'documentation' stamp: 'acg 8/3/2002 23:06'!
aGlobalSearchComment "

Global Searching

Introduction

	RePattern provides facilities to support global searching and global searching and replacement of search strings with semantics quite similar to that of Perl 5.004.  Global searching means that the search string is repeatedly searched for matches, beginning at the beginning of the string, and subsequently beginning the next match immediately after the preceding match terminated.  For example, if we wanted to find all words in the subject string, we could execute:

	'this is a test' reMatch: '\w+' collect: [:m | m match]

which returns

	OrderedCollection ('this' 'is' 'a' 'test' ).

The collect: keyword directs PCRE to repeat the search for '\w+', and to return a collection of the result of applying each ReMatch to the block.  (In this case, the block simply returns the string that was matched.)  To do global searching and string substitution, we could execute:

	'this is a test' reMatch: '\w+' sub: [:m | '<', m match, '>']

which return

	 '<this> <is> <a> <test>'

The sub: keyword directs PCRE to repeat the search, and to return the original string, but with each matched substring replaced by the result of applying the block to the corresponding ReMatch object.

Global Matching Functions

RePattern convenience functions provide the following general global functions:

	searchString reMatch: pattern [opt: oStr] collect: aBlock [num: anInteger]
	searchString reMatch: pattern [opt: oStr] sub: aBlock [num: anInteger]

Optionally, you may specify search and compile options with oStr, and you may specify a maximum number of searches performed in the global search with anInteger.  If anInteger is less than 0, then as many searches as can be performed, will be performed.

Special Case of the Empty Match

Finally, the definition given above would infinite loop if the pattern matches an empty string.  For example:

	'abcdef' reMatch: 'x*' sub: [:m| '<', m match, '>'] 

will actually match the empty string just before and after each letter of the string, even though there is no x there.  ('x+' would return nil).  Since the string ends where it begins, at the beginning of the string, repeating the search from that point would simply infinite loop.  Accordingly, RePattern gives the pattern a one-character 'bump' after matching an empty string, at which point the block is applied.  For example, the preceding would answer

	'<>a<>b<>c<>d<>e<>f<>'

In the case of global searching (but not replacement), an empty string will not result in the ReMatch being applied to the block if the empty match immediately follows a match that has already been made.  Accordingly,

	'123' reMatch: '\d*' collect: [:m| m match]  

answers

	OrderedCollection ('123' )

and not 
	
	OrderedCollection ('123' '')

However, this last caveat does not apply to substitutions, so

	'123' reMatch: '\d*' sub: [:m| '<', m match, '>']   

answers

	'<123><>'



"!
]style[(21 3 18 12 1 2 9 460 1 243 7 154 1 290 7 10 25 487 31 1138 1 1)f1b,f1,bf3,bf2,bf3,bf2,f1LRePattern Comment;,f1,f1u,f1,f1LReMatch Comment;,f1,f1u,f1,f1LReMatch Comment;,f1,bf2,f1,bf2,f1,bf2,f1! !

!Re methodsFor: 'documentation' stamp: 'acg 8/3/2002 23:01'!
aLicenseComment "

RePlugin is Open Source Software

As noted earlier, the non-Smalltalk code on which these classes are based is Philip Hazel's excellent PCRE Package, which is distributed subject to the following license.  The Smalltalk wrapper and plugin interface is written by Andrew C. Greenberg <werdna@gate.net> and other contributors, and is distributed subject to the same terms.

PCRE LICENCE
------------

PCRE is a library of functions to support regular expressions whose syntax
and semantics are as close as possible to those of the Perl 5 language.

Written by: Philip Hazel <ph10@cam.ac.uk>

University of Cambridge Computing Service,
Cambridge, England. Phone: +44 1223 334714.

Copyright (c) 1997-1999 University of Cambridge

Permission is granted to anyone to use this software for any purpose on any
computer system, and to redistribute it freely, subject to the following
restrictions:

1. This software is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

2. The origin of this software must not be misrepresented, either by
   explicit claim or by omission.

3. Altered versions must be plainly marked as such, and must not be
   misrepresented as being the original software.

4. If PCRE is embedded in any software that is released under the GNU
   General Purpose Licence (GPL), then the terms of that licence shall
   supersede any condition above with which it is incompatible.
"!
]style[(16 3 32 340 12 1127)f1b,f1,bf3,f1,bf2,f1! !

!Re methodsFor: 'documentation' stamp: 'acg 8/3/2002 23:01'!
aRegexComment "

Regular Expressions

A more detailed description of RePlugin is available downloading the file 'RePluginDoco,' which can be obtained from http://www.gate.net/~werdna/RePlugin.html, into your default directory, and then executing

		Utilities reconstructTextWindowsFromFileNamed: 'RePluginDoco'


Basic MetaCharacters

The regular expressions recognized in this package generally track those of Perl 5.05, and are set forth in greater detail in the PCRE documentation accompanying the package.  A summary follows:

	\	General escape character with several uses
	^	Assert start of subjct (or line, in multiline mode)
		Also used to negate class definitions
	$	Assert end of subject (or line, in multiline mode)
	.	match any character but newline (by default)
	[]	class definitions
	|	start of alternative branch
	()	subpattern
	?	extends the meaning of '('  (see below)
		quantifies previous extension (1 or 0 occurrences) (e.g. a?)
		minimizes previous quantifier (e.g.  a*?)
	*	0 or more quantifier
	+	1 or more quantifier
	{}	Min/Max Quantifier  {3} {3,} {3,5}

Inside Character Classes

	\	general escape character
	^	negates the class, if the first character
	-	indicates character range, if not escaped or the last character
	
Special Escape Sequences

	\a	alarm (hex 7)
	\cx	control-x, where x is any character
	\e	escape (hex 1b)
	\f	formfeed (hex 0c)
	\n	newline
	\r	carriage return
	\t	tab
	\xhh	Character with hexcode hh
	\ddd		Character with octal code ddd, or a backreference

	\d	matches decimal digit
	\D	non-decimal digit
	\s	whitespace
	\S	non-whitespace
	\w	any 'word' character
	\W	any non-word character

	\b	asserts a word boundary
	\B	asserts not a non-word boundary
	\A	asserts start of subject (regardless of mode)
	\Z	asserts end of subject (regardless of mode)

Internal Option Setting

	Letters enclosed within a pattern and appearing between '(?' and ')' can be used to change the imsx options.  For example.
(?im-sx) sets caseless and multiline modes, and unsets dotall and extended modes.  See the PCRE documentation for further details.

Non-Grouping Subpatterns

	Groupings can be enclosed by parentheses without text being captured by following the leading parenthesis with a question mark and colon.  for example: 'abc(?:def)*' repeats the 'def', but does not capture matches in a grouping.

Assertions

	An assertion is a test on characters that does not actually consume any characters.  There are two kinds, those that look ahead of the current position, and those that look behind.  Consider the following example:

	\w+(?=;)

which matches a word followed by a semicolon, but doesn't include the semicolon in the match.  Another example:

	(?<!!foo)bar

finds occurences of bar not preceded by foo.  All lookbehind assertions must be of fixed length, but not all alternatives in such an assertion need be of the same length.

Once-Only Subpatterns

	(?>\d+)bar

Once only subpatterns 'lock up' after finding a match, to prevent backtracking in various cases.  Essentially, a subpattern ofthis type matches the string that an identical standalone pattern would match if anchored at the current point in the subject string first encountering the expression.

Conditional Subpatterns

	(?(condition)yes-pattern)
	(?(condition)yes-pattern|no-pattern) 

These permit one of two subpatterns to be matched, depending upon a preceding condition.  There are two kinds of conditions: (1) a sequence of digits, specifying that a numbered subpattern has been matched; and (2) an assertion, either positive, negative, lookahead or lookbehind.

Comments

	(?# This is a comment)

Also, in extended mode, comments may be inserted between a '#' and a newline.

"!
]style[(17 21 117 41 53 61 2 1 20 1 1 194 2 547 28 139 29 528 26 255 27 230 13 353 1 170 38 293 94 280 36 78 3)f1b,bf3,f1,f1Rhttp://www.gate.net/~werdna/RePlugin.html;,f1,f1dUtilities reconstructTextWindowsFromFileNamed: 'RePluginDoco';;,f1,bf3,bf2,bf3,f1b,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1,f1b! !

!Re methodsFor: 'documentation' stamp: 'acg 8/4/2002 11:13'!
aRegexGoryDetailsComment "

Regular Expression Syntax -- the Gory Details

Introduction

RePlugin is a Squeak Plugin providing modern regular expression matching operations similar to those found in Perl. It was written by Andrew C. Greenberg (werdna@gate.net), with contributions by Markus Kohler, Stephen Pair and others. RePlugin 2.3b (and 'the Gory Details' portion of this document) is directly taken from Version 2.04 of the excellent PCRE library by Philip Hazel with only minor modifications. 

The syntax and semantics of the regular expressions supported by PCRE are described below. Regular expressions are also described in the Perl documentation and in a number of other books, some of which have copious examples. Jeffrey Friedl's 'Mastering Regular Expressions', published by O'Reilly (ISBN 1-56592-257-3), covers them in great detail. The description here is intended as reference documentation. 

Regular Expressions

A regular expression is a pattern that is matched against a subject string from left to right. Most characters stand for themselves in a pattern, and match the corresponding characters in the subject. As a trivial example, the pattern 

	The quick brown fox

matches a portion of a subject string that is identical to itself. The power of regular expressions comes from the ability to include alternatives and repetitions in the pattern. These are encoded in the pattern by the use of meta-characters, which do not stand for themselves but instead are interpreted in some special way. 

There are two different sets of meta-characters: those that are recognized anywhere in the pattern except within square brackets, and those that are recognized in square brackets. Outside square brackets, the meta-characters are as follows: 

  \      general escape character with several uses
  ^      assert start of subject (or line, in multiline mode)
  $      assert end of subject (or line, in multiline mode)
  .      match any character except newline (by default)
  [      start character class definition
  |      start of alternative branch
  (      start subpattern
  )      end subpattern
  ?      extends the meaning of (
         also 0 or 1 quantifier
         also quantifier minimizer
  *      0 or more quantifier
  +      1 or more quantifier
  {      start min/max quantifier

Part of a pattern that is in square brackets is called a 'character class'. In a character class the only meta-characters are: 

  \      general escape character
  ^      negate the class, but only if the first character
  -      indicates character range
  ]      terminates the character class

The following sections describe the use of each of the meta-characters. 


BACKSLASH

The backslash character has several uses. Firstly, if it is followed by a non-alphameric character, it takes away any special meaning that character may have. This use of backslash as an escape character applies both inside and outside character classes. 

For example, if you want to match a '*' character, you write '\*' in the pattern. This applies whether or not the following character would otherwise be interpreted as a meta-character, so it is always safe to precede a non-alphameric with '\' to specify that it stands for itself. In particular, if you want to match a backslash, you write '\\'. 

If a pattern is compiled with the 'x' (beExtended) option, whitespace in the pattern (other than in a character class) and characters between a '#' outside a character class and the next newline character are ignored. An escaping backslash can be used to include a whitespace or '#' character as part of the pattern. 

A second use of backslash provides a way of encoding non-printing characters in patterns in a visible manner. There is no restriction on the appearance of non-printing characters, apart from the binary zero that terminates a pattern, but when a pattern is being prepared by text editing, it is usually easier to use one of the following escape sequences than the binary character it represents: 

  \a     alarm, that is, the BEL character (hex 07)
  \cx    'control-x', where x is any character
  \e     escape (hex 1B)
  \f     formfeed (hex 0C)
  \n     newline (hex 0A)
  \r     carriage return (hex 0D)
  \t     tab (hex 09)
  \xhh   character with hex code hh
  \ddd   character with octal code ddd, or backreference

The precise effect of '\cx' is as follows: if 'x' is a lower case letter, it is converted to upper case. Then bit 6 of the character (hex 40) is inverted. Thus '\cz' becomes hex 1A, but '\c{' becomes hex 3B, while '\c;' becomes hex 7B. 

After '\x', up to two hexadecimal digits are read (letters can be in upper or lower case). 

After '\0' up to two further octal digits are read. In both cases, if there are fewer than two digits, just those that are present are used. Thus the sequence '\0\x\07' specifies two binary zeros followed by a BEL character. Make sure you supply two digits after the initial zero if the character that follows is itself an octal digit. 

The handling of a backslash followed by a digit other than 0 is complicated. Outside a character class, PCRE reads it and any following digits as a decimal number. If the number is less than 10, or if there have been at least that many previous capturing left parentheses in the expression, the entire sequence is taken as a back reference. A description of how this works is given later, following the discussion of parenthesized subpatterns. 

Inside a character class, or if the decimal number is greater than 9 and there have not been that many capturing subpatterns, PCRE re-reads up to three octal digits following the backslash, and generates a single byte from the least significant 8 bits of the value. Any subsequent digits stand for themselves. For example: 

  \040   is another way of writing a space
  \40    is the same, provided there are fewer than 40
            previous capturing subpatterns
  \7     is always a back reference
  \11    might be a back reference, or another way of
            writing a tab
  \011   is always a tab
  \0113  is a tab followed by the character '3'
  \113   is the character with octal code 113 (since there
            can be no more than 99 back references)
  \377   is a byte consisting entirely of 1 bits
  \81    is either a back reference, or a binary zero
            followed by the two characters '8' and '1'

Note that octal values of 100 or greater must not be introduced by a leading zero, because no more than three octal digits are ever read. 

All the sequences that define a single byte value can be used both inside and outside character classes. In addition, inside a character class, the sequence '\b' is interpreted as the backspace character (hex 08). Outside a character class it has a different meaning (see below). 

The third use of backslash is for specifying generic character types: 

  \d     any decimal digit
  \D     any character that is not a decimal digit
  \s     any whitespace character
  \S     any character that is not a whitespace character
  \w     any 'word' character
  \W     any 'non-word' character

Each pair of escape sequences partitions the complete set of characters into two disjoint sets. Any given character matches one, and only one, of each pair. 

A 'word' character is any letter or digit or the underscore character, that is, any character which can be part of a Perl 'word'. The definition of letters and digits is controlled by PCRE's character tables, and may vary if locale- specific matching is taking place (see 'Locale support' above). For example, in the 'fr' (French) locale, some character codes greater than 128 are used for accented letters, and these are matched by \w. 

These character type sequences can appear both inside and outside character classes. They each match one character of the appropriate type. If the current matching point is at the end of the subject string, all of them fail, since there is no character to match. 

The fourth use of backslash is for certain simple assertions. An assertion specifies a condition that has to be met at a particular point in a match, without consuming any characters from the subject string. The use of subpatterns for more complicated assertions is described below. The backslashed assertions are 

  \b     word boundary
  \B     not a word boundary
  \A     start of subject (independent of multiline mode)
  \Z     end of subject or newline at end (independent of multiline mode)
  \z     end of subject (independent of multiline mode)

These assertions may not appear in character classes (but note that '\b' has a different meaning, namely the backspace character, inside a character class). 

A word boundary is a position in the subject string where the current character and the previous character do not both match \w or \W (i.e. one matches \w and the other matches \W), or the start or end of the string if the first or last character matches \w, respectively. 

The \A, \Z, and \z assertions differ from the traditional circumflex and dollar (described below) in that they only ever match at the very start and end of the subject string, whatever options are set. They are not affected by the 'B' (beNotBeginningOfLine) or 'Z' (beNotEndOfLine) options. The difference between \Z and \z is that \Z matches before a newline that is the last character of the string as well as at the end of the string, whereas \z matches only at the end. 


CIRCUMFLEX AND DOLLAR

Outside a character class, in the default matching mode, the circumflex character is an assertion which is true only if the current matching point is at the start of the subject string. Inside a character class, circumflex has an entirely different meaning (see below). 

Circumflex need not be the first character of the pattern if a number of alternatives are involved, but it should be the first thing in each alternative in which it appears if the pattern is ever to match that branch. If all possible alternatives start with a circumflex, that is, if the pattern is constrained to match only at the start of the subject, it is said to be an 'anchored' pattern. (There are also other constructs that can cause a pattern to be anchored.) 

A dollar character is an assertion which is true only if the current matching point is at the end of the subject string, or immediately before a newline character that is the last character in the string (by default). Dollar need not be the last character of the pattern if a number of alternatives are involved, but it should be the last item in any branch in which it appears. Dollar has no special meaning in a character class. 

The meaning of dollar can be changed so that it matches only at the very end of the string, by setting the 'E' ('beDollarEndOnly') option at compile or matching time. This does not affect the \Z assertion. 

The meanings of the circumflex and dollar characters are changed if the 'm' (beMultiline) option is set. When this is the case, they match immediately after and immediately before an internal '\n' character, respectively, in addition to matching at the start and end of the subject string. For example, the pattern /^abc$/ matches the subject string 'def\nabc' in multiline mode, but not otherwise. Consequently, patterns that are anchored in single line mode because all branches start with '^' are not anchored in multiline mode. The 'E' (beExtended) option is ignored if 's' is set. 

Note that the sequences \A, \Z, and \z can be used to match the start and end of the subject in both modes, and if all branches of a pattern start with \A is it always anchored, whether 's' (beDotIncludesNewline) is set or not. 


PERIOD, DOT

Outside a character class, a dot in the pattern matches any one character in the subject, including a non-printing character, but not (by default) newline. If the 's' (beDotIncludesNewline) option is set, then dots match newlines as well. The handling of dot is entirely independent of the handling of circumflex and dollar, the only relationship being that they both involve newline characters. Dot has no special meaning in a character class. 


SQUARE BRACKETS

An opening square bracket introduces a character class, terminated by a closing square bracket. A closing square bracket on its own is not special. If a closing square bracket is required as a member of the class, it should be the first data character in the class (after an initial circumflex, if present) or escaped with a backslash. 

A character class matches a single character in the subject; the character must be in the set of characters defined by the class, unless the first character in the class is a circumflex, in which case the subject character must not be in the set defined by the class. If a circumflex is actually required as a member of the class, ensure it is not the first character, or escape it with a backslash. 

For example, the character class [aeiou] matches any lower case vowel, while [^aeiou] matches any character that is not a lower case vowel. Note that a circumflex is just a convenient notation for specifying the characters which are in the class by enumerating those that are not. It is not an assertion: it still consumes a character from the subject string, and fails if the current pointer is at the end of the string. 

When caseless matching is set, any letters in a class represent both their upper case and lower case versions, so for example, a caseless [aeiou] matches 'A' as well as 'a', and a caseless [^aeiou] does not match 'A', whereas a caseful version would. 

The newline character is never treated in any special way in character classes, whatever the setting of the 's' (beDotIncludesNewline) or 'm' (beMultiline) options is. A class such as [^a] will always match a newline. 

The minus (hyphen) character can be used to specify a range of characters in a character class. For example, [d-m] matches any letter between d and m, inclusive. If a minus character is required in a class, it must be escaped with a backslash or appear in a position where it cannot be interpreted as indicating a range, typically as the first or last character in the class. 

It is not possible to have the literal character ']' as the end character of a range. A pattern such as [W-]46] is interpreted as a class of two characters ('W' and '-') followed by a literal string '46]', so it would match 'W46]' or '-46]'. However, if the ']' is escaped with a backslash it is interpreted as the end of range, so [W-\]46] is interpreted as a single class containing a range followed by two separate characters. The octal or hexadecimal representation of ']' can also be used to end a range. 

Ranges operate in ASCII collating sequence. They can also be used for characters specified numerically, for example [\000-\037]. If a range that includes letters is used when caseless matching is set, it matches the letters in either case. For example, [W-c] is equivalent to [][\^_`wxyzabc], matched caselessly, and if character tables for the 'fr' locale are in use, [\xc8-\xcb] matches accented E characters in both cases. 

The character types \d, \D, \s, \S, \w, and \W may also appear in a character class, and add the characters that they match to the class. For example, [\dABCDEF] matches any hexadecimal digit. A circumflex can conveniently be used with the upper case character types to specify a more restricted set of characters than the matching lower case type. For example, the class [^\W_] matches any letter or digit, but not underscore. 

All non-alphameric characters other than \, -, ^ (at the start) and the terminating ] are non-special in character classes, but it does no harm if they are escaped. 


VERTICAL BAR

Vertical bar characters are used to separate alternative patterns. For example, the pattern 

  gilbert|sullivan

matches either 'gilbert' or 'sullivan'. Any number of alternatives may appear, and an empty alternative is permitted (matching the empty string). The matching process tries each alternative in turn, from left to right, and the first one that succeeds is used. If the alternatives are within a subpattern (defined below), 'succeeds' means matching the rest of the main pattern as well as the alternative in the subpattern. 


INTERNAL OPTION SETTING

The settings of caseless, multiline, dotall and extended options can be changed from within the pattern by a sequence of Perl option letters enclosed between '(?' and ')'. The option letters are 

  i  for Caseless Matching Mode
  m  for Multiline Mode
  s  for Dotall Mode (Dot matches newlines)
  x  for Extended Mode (whitespace not meaningful, comments permitted)

For example, (?im) sets caseless, multiline matching. It is also possible to unset these options by preceding the letter with a hyphen, and a combined setting and unsetting such as (?im-sx), which sets caseless and multiline while unsetting dotall and extended, is also permitted. If a letter appears both before and after the hyphen, the option is unset. 

The scope of these option changes depends on where in the pattern the setting occurs. For settings that are outside any subpattern (defined below), the effect is the same as if the options were set or unset at the start of matching. The following patterns all behave in exactly the same way: 

  (?i)abc
  a(?i)bc
  ab(?i)c
  abc(?i)

which in turn is the same as compiling the pattern abc with 'i' set. In other words, such 'top level' settings apply to the whole pattern (unless there are other changes inside subpatterns). If there is more than one setting of the same option at top level, the rightmost setting is used. 

If an option change occurs inside a subpattern, the effect is different. This is a change of behaviour in Perl 5.005. An option change inside a subpattern affects only that part of the subpattern that follows it, so 

  (a(?i)b)c

matches abc and aBc and no other strings (assuming 'i' is not used). By this means, options can be made to have different settings in different parts of the pattern. Any changes made in one alternative do carry on into subsequent branches within the same subpattern. For example, 

  (a(?i)b|c)

matches 'ab', 'aB', 'c', and 'C', even though when matching 'C' the first branch is abandoned before the option setting. This is because the effects of option settings happen at compile time. There would be some very weird behaviour otherwise. 

The PCRE-specific options 'U' and 'X' can be changed in the same way as the Perl-compatible options. The (?X) flag setting is special in that it must always occur earlier in the pattern than any of the additional features it turns on, even when it is at top level. It is best put at the start. 


SUBPATTERNS

Subpatterns are delimited by parentheses (round brackets), which can be nested. Marking part of a pattern as a subpattern does two things: 

1. It localizes a set of alternatives. For example, the pattern 

  cat(aract|erpillar|)

matches one of the words 'cat', 'cataract', or 'caterpillar'. Without the parentheses, it would match 'cataract', 'erpillar' or the empty string. 

2. It sets up the subpattern as a capturing subpattern (as defined above). When the whole pattern matches, that portion of the subject string that matchedOpening parentheses are counted from left to right (starting from 1) to obtain the numbers of the capturing subpatterns. 

For example, if the string 'the red king' is matched against the pattern 

  the ((red|white) (king|queen))

the captured substrings are 'red king', 'red', and 'king', and are numbered 1, 2, and 3. 

The fact that plain parentheses fulfil two functions is not always helpful. There are often times when a grouping subpattern is required without a capturing requirement. If an opening parenthesis is followed by '?:', the subpattern does not do any capturing, and is not counted when computing the number of any subsequent capturing subpatterns. For example, if the string 'the white queen' is matched against the pattern 

  the ((?:red|white) (king|queen))

the captured substrings are 'white queen' and 'queen', and are numbered 1 and 2. The maximum number of captured substrings is 99, and the maximum number of all subpatterns, both capturing and non-capturing, is 200. 

As a convenient shorthand, if any option settings are required at the start of a non-capturing subpattern, the option letters may appear between the '?' and the ':'. Thus the two patterns 

  (?i:saturday|sunday)
  (?:(?i)saturday|sunday)
match exactly the same set of strings. Because alternative branches are tried from left to right, and options are not reset until the end of the subpattern is reached, an option setting in one branch does affect subsequent branches, so the above patterns match 'SUNDAY' as well as 'Saturday'. 


REPETITION

Repetition is specified by quantifiers, which can follow any of the following items: 

  a single character, possibly escaped
  the . metacharacter
  a character class
  a back reference (see next section)
  a parenthesized subpattern (unless it is an assertion - see below)

The general repetition quantifier specifies a minimum and maximum number of permitted matches, by giving the two numbers in curly brackets (braces), separated by a comma. The numbers must be less than 65536, and the first must be less than or equal to the second. For example: 

  z{2,4}

matches 'zz', 'zzz', or 'zzzz'. A closing brace on its own is not a special character. If the second number is omitted, but the comma is present, there is no upper limit; if the second number and the comma are both omitted, the quantifier specifies an exact number of required matches. Thus 

  [aeiou]{3,}

matches at least 3 successive vowels, but may match many more, while 

  \d{8}

matches exactly 8 digits. An opening curly bracket that appears in a position where a quantifier is not allowed, or one that does not match the syntax of a quantifier, is taken as a literal character. For example, {,6} is not a quantifier, but a literal string of four characters. 

The quantifier {0} is permitted, causing the expression to behave as if the previous item and the quantifier were not present. 

For convenience (and historical compatibility) the three most common quantifiers have single-character abbreviations: 

  *    is equivalent to {0,}
  +    is equivalent to {1,}
  ?    is equivalent to {0,1}

It is possible to construct infinite loops by following a subpattern that can match no characters with a quantifier that has no upper limit, for example: 

  (a?)*

Earlier versions of Perl and PCRE used to give an error at compile time for such patterns. However, because there are cases where this can be useful, such patterns are now accepted, but if any repetition of the subpattern does in fact match no characters, the loop is forcibly broken. 

By default, the quantifiers are 'greedy', that is, they match as much as possible (up to the maximum number of permitted times), without causing the rest of the pattern to fail. The classic example of where this gives problems is in trying to match comments in C programs. These appear between the sequences /* and */ and within the sequence, individual * and / characters may appear. An attempt to match C comments by applying the pattern 

  /\*.*\*/
to the string 

  /* first command */  not comment  /* second comment */

fails, because it matches the entire string due to the greediness of the .* item. 

However, if a quantifier is followed by a question mark, then it ceases to be greedy, and instead matches the minimum number of times possible, so the pattern 

  /\*.*?\*/

does the right thing with the C comments. The meaning of the various quantifiers is not otherwise changed, just the preferred number of matches. Do not confuse this use of question mark with its use as a quantifier in its own right. Because it has two uses, it can sometimes appear doubled, as in 

  \d??\d

which matches one digit by preference, but can match two if that is the only way the rest of the pattern matches. 

If the 'U' option is set (an option which is not available in Perl) then the quantifiers are not greedy by default, but individual ones can be made greedy by following them with a question mark. In other words, it inverts the default behaviour. 

When a parenthesized subpattern is quantified with a minimum repeat count that is greater than 1 or with a limited maximum, more store is required for the compiled pattern, in proportion to the size of the minimum or maximum. 

If a pattern starts with .* then it is implicitly anchored, since whatever follows will be tried against every character position in the subject string. PCRE treats this as though it were preceded by \A. 

When a capturing subpattern is repeated, the value captured is the substring that matched the final iteration. For example, after 

  (tweedle[dume]{3}\s*)+

has matched 'tweedledum tweedledee' the value of the captured substring is 'tweedledee'. However, if there are nested capturing subpatterns, the corresponding captured values may have been set in previous iterations. For example, after 

  /(a|(b))+/

matches 'aba' the value of the second captured substring is 'b'. 

BACK REFERENCES

Outside a character class, a backslash followed by a digit greater than 0 (and possibly further digits) is a back reference to a capturing subpattern earlier (i.e. to its left) in the pattern, provided there have been that many previous capturing left parentheses. 

However, if the decimal number following the backslash is less than 10, it is always taken as a back reference, and causes an error only if there are not that many capturing left parentheses in the entire pattern. In other words, the parentheses that are referenced need not be to the left of the reference for numbers less than 10. See the section entitled 'Backslash' above for further details of the handling of digits following a backslash. 

A back reference matches whatever actually matched the capturing subpattern in the current subject string, rather than anything matching the subpattern itself. So the pattern 

  (sens|respons)e and \1ibility

matches 'sense and sensibility' and 'response and responsibility', but not 'sense and responsibility'. If caseful matching is in force at the time of the back reference, then the case of letters is relevant. For example, 

  ((?i)rah)\s+\1

matches 'rah rah' and 'RAH RAH', but not 'RAH rah', even though the original capturing subpattern is matched caselessly. 

There may be more than one back reference to the same subpattern. If a subpattern has not actually been used in a particular match, then any back references to it always fail. For example, the pattern 

  (a|(bc))\2

always fails if it starts to match 'a' rather than 'bc'. Because there may be up to 99 back references, all digits following the backslash are taken as part of a potential back reference number. If the pattern continues with a digit character, then some delimiter must be used to terminate the back reference. If the 'x' (beExteded) option is set, this can be whitespace. Otherwise an empty comment can be used. 

A back reference that occurs inside the parentheses to which it refers fails when the subpattern is first used, so, for example, (a\1) never matches. However, such references can be useful inside repeated subpatterns. For example, the pattern 

  (a|b\1)+

matches any number of 'a's and also 'aba', 'ababaa' etc. At each iteration of the subpattern, the back reference matches the character string corresponding to the previous iteration. In order for this to work, the pattern must be such that the first iteration does not need to match the back reference. This can be done using alternation, as in the example above, or by a quantifier with a minimum of zero. 


ASSERTIONS

An assertion is a test on the characters following or preceding the current matching point that does not actually consume any characters. The simple assertions coded as \b, \B, \A, \Z, \z, ^ and $ are described above. More complicated assertions are coded as subpatterns. There are two kinds: those that look ahead of the current position in the subject string, and those that look behind it. 

An assertion subpattern is matched in the normal way, except that it does not cause the current matching position to be changed. Lookahead assertions start with (?= for positive assertions and (?!! for negative assertions. For example, 

  \w+(?=;)

matches a word followed by a semicolon, but does not include the semicolon in the match, and 

  foo(?!!bar)

matches any occurrence of 'foo' that is not followed by 'bar'. Note that the apparently similar pattern 

  (?!!foo)bar

does not find an occurrence of 'bar' that is preceded by something other than 'foo'; it finds any occurrence of 'bar' whatsoever, because the assertion (?!!foo) is always true when the next three characters are 'bar'. A lookbehind assertion is needed to achieve this effect. 

Lookbehind assertions start with (?<= for positive assertions and (?&lt!! for negative assertions. For example, 

  (?<!!foo)bar

does find an occurrence of 'bar' that is not preceded by 'foo'. The contents of a lookbehind assertion are restricted such that all the strings it matches must have a fixed length. However, if there are several alternatives, they do not all have to have the same fixed length. Thus 

  (?<=bullock|donkey)
is permitted, but 

  (?<!!dogs?|cats?)

causes an error at compile time. Branches that match different length strings are permitted only at the top level of a lookbehind assertion. This is an extension compared with Perl 5.005, which requires all branches to match the same length of string. An assertion such as 

  (?=ab(c|de))

is not permitted, because its single top-level branch can match two different lengths, but it is acceptable if rewritten to use two top-level branches: 

  (?=abc|abde)

The implementation of lookbehind assertions is, for each alternative, to temporarily move the current position back by the fixed width and then try to match. If there are insufficient characters before the current position, the match is deemed to fail. Lookbehinds in conjunction with once-only subpatterns can be particularly useful for matching at the ends of strings; an example is given at the end of the section on once-only subpatterns. Several assertions (of any sort) may occur in succession. For example, 

  (?=\d{3})(?<!!999)foo

matches 'foo' preceded by three digits that are not '999'. Furthermore, assertions can be nested in any combination. For example, 

  (?=(?<!!foo)bar)baz

matches an occurrence of 'baz' that is preceded by 'bar' which in turn is not preceded by 'foo'. 

Assertion subpatterns are not capturing subpatterns, and may not be repeated, because it makes no sense to assert the same thing several times. If an assertion contains capturing subpatterns within it, these are always counted for the purposes of numbering the capturing subpatterns in the whole pattern. Substring capturing is carried out for positive assertions, but it does not make sense for negative assertions. 

Assertions count towards the maximum of 200 parenthesized subpatterns. 


ONCE-ONLY SUBPATTERNS

With both maximizing and minimizing repetition, failure of what follows normally causes the repeated item to be re-evaluated to see if a different number of repeats allows the rest of the pattern to match. Sometimes it is useful to prevent this, either to change the nature of the match, or to cause it fail earlier than it otherwise might, when the author of the pattern knows there is no point in carrying on. 

Consider, for example, the pattern \d+foo when applied to the subject line 

  123456bar

After matching all 6 digits and then failing to match 'foo', the normal action of the matcher is to try again with only 5 digits matching the \d+ item, and then with 4, and so on, before ultimately failing. Once-only subpatterns provide the means for specifying that once a portion of the pattern has matched, it is not to be re-evaluated in this way, so the matcher would give up immediately on failing to match 'foo' the first time. The notation is another kind of special parenthesis, starting with (?> as in this example: 

  (?>\d+)bar

This kind of parenthesis 'locks up' the part of the pattern it contains once it has matched, and a failure further into the pattern is prevented from backtracking into it. Backtracking past it to previous items, however, works as normal. 

An alternative description is that a subpattern of this type matches the string of characters that an identical standalone pattern would match, if anchored at the current point in the subject string. 

Once-only subpatterns are not capturing subpatterns. Simple cases such as the above example can be thought of as a maximizing repeat that must swallow everything it can. So, while both \d+ and \d+? are prepared to adjust the number of digits they match in order to make the rest of the pattern match, (?>\d+) can only match an entire sequence of digits. 

This construction can of course contain arbitrarily complicated subpatterns, and it can be nested. 

Once-only subpatterns can be used in conjunction with lookbehind assertions to specify efficient matching at the end of the subject string. Consider a simple pattern such as 

  abcd$

when applied to a long string which does not match it. Because matching proceeds from left to right, PCRE will look for each 'a' in the subject and then see if what follows matches the rest of the pattern. If the pattern is specified as 

  .*abcd$

then the initial .* matches the entire string at first, but when this fails, it backtracks to match all but the last character, then all but the last two characters, and so on. Once again the search for 'a' covers the entire string, from right to left, so we are no better off. However, if the pattern is written as 

  (?>.*)(?=abcd)

then there can be no backtracking for the .* item; it can match only the entire string. The subsequent lookbehind assertion does a single test on the last four characters. If it fails, the match fails immediately. For long strings, this approach makes a significant difference to the processing time. 


CONDITIONAL SUBPATTERNS

It is possible to cause the matching process to obey a subpattern conditionally or to choose between two alternative subpatterns, depending on the result of an assertion, or whether a previous capturing subpattern matched or not. The two possible forms of conditional subpattern are 

  (?(condition)yes-pattern)
  (?(condition)yes-pattern|no-pattern)

If the condition is satisfied, the yes-pattern is used; otherwise the no-pattern (if present) is used. If there are more than two alternatives in the subpattern, a compile-time error occurs. 

There are two kinds of condition. If the text between the parentheses consists of a sequence of digits, then the condition is satisfied if the capturing subpattern of that number has previously matched. Consider the following pattern, which contains non-significant white space to make it more readable (assume the 'x' (beExtended) option) and to divide it into three parts for ease of discussion: 

  ( \( )?    [^()]+    (?(1) \) )

The first part matches an optional opening parenthesis, and if that character is present, sets it as the first captured substring. The second part matches one or more characters that are not parentheses. The third part is a conditional subpattern that tests whether the first set of parentheses matched or not. If they did, that is, if subject started with an opening parenthesis, the condition is true, and so the yes-pattern is executed and a closing parenthesis is required. Otherwise, since no-pattern is not present, the subpattern matches nothing. In other words, this pattern matches a sequence of non-parentheses, optionally enclosed in parentheses. 

If the condition is not a sequence of digits, it must be an assertion. This may be a positive or negative lookahead or lookbehind assertion. Consider this pattern, again containing non-significant white space, and with the two alternatives on the second line: 

  (?(?=[^a-z]*[a-z])
  \d{2}[a-z]{3}-\d{2}  |  \d{2}-\d{2}-\d{2} )

The condition is a positive lookahead assertion that matches an optional sequence of non-letters followed by a letter. In other words, it tests for the presence of at least one letter in the subject. If a letter is found, the subject is matched against the first alternative; otherwise it is matched against the second. This pattern matches strings in one of the two forms dd-aaa-dd or dd-dd-dd, where aaa are letters and dd are digits. 


COMMENTS

The sequence (?# marks the start of a comment which continues up to the next closing parenthesis. Nested parentheses are not permitted. The characters that make up a comment play no part in the pattern matching at all. 

If the 'x' (beExtended) option is set, an unescaped # character outside a character class introduces a comment that continues up to the next newline character in the pattern."!
]style[(24 4 45 2 12 827 19 1761 9 6751 21 2205 11 450 15 3555 12 541 23 2425 11 2069 10 4396 15 2588 10 3372 21 3022 23 2413 8 398)f1b,f1,f3b,f1,f2b,f1,f2b,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1,f1b,f1! !

!Re methodsFor: 'documentation' stamp: 'acg 8/3/2002 23:04'!
aVersionsComment "

RePlugin Change History and Compatibility



Version 3.2 --

Make compatible with Squeak version 3.X
Further revisions to front end.
Many bug fixes.

Version 2.3b --
Substantially revise String convenience functions to serve as primary user interface to RePlugin. 
Implement pattern compile caching. 
Implement pattern match default caching. 
Order of magnitude performance improvement in global substitution routines. 
Implement modern Perl5 semantics for global matching and replacement. 
Minor bug fixes. 
Permit use of compiled patterns as reMatch: parameter. 
Plugin code changed so that semantics for '\n' and '\r' are hard-coded across all plugins to conform to Squeak standards regardless of variations in local hardware c libraries. 
Plugin code memory management scheme modified to permit compilation on most non-Macintosh systems. 
Plugin code changed to automatically incorporate RePlugin class comment in the generated plugin header. 
Release Wintel Plugin 
Release HP-UX Plugin 
Version 2.3a -- 
Release MacPPC Plugin. 

Limitations

There are some size limitations in PCRE but it is hoped that they will never in practice be relevant. 
maximum length of a compiled pattern is 65539 (sic) bytes. 
All values in repeating quantifiers must be less than 65536. 
The maximum number of capturing subpatterns is 99. 
The maximum number of all parenthesized subpatterns, including capturing subpatterns, assertions, and other types of subpattern, is 200. 

The maximum length of a subject string is the largest positive number that an integer variable can hold. However, PCRE uses recursion to handle subpatterns and indefinite repetition. This means that the available stack space may limit the size of a subject string that can be processed by certain patterns. 

Differences From Perl

The differences described here are with respect to Perl 5.005. 

1. By default, a whitespace character is any character that the C library function isspace() recognizes, though it is possible to compile PCRE with alternative character type tables. Normally \fBisspace()\fR matches space, formfeed, newline, carriage return, horizontal tab, and vertical tab. Perl 5 no longer includes vertical tab in its set of whitespace characters. The \v escape that was in the Perl documentation for a long time was never in fact recognized. However, the character itself was treated as whitespace at least up to 5.002. In 5.004 and 5.005 it does not match \s. 

2. PCRE does not allow repeat quantifiers on lookahead assertions. Perl permits them, but they do not mean what you might think. For example, (?!!a){3} does not assert that the next three characters are not 'a'. It just asserts that the next character is not 'a' three times. 

3. Capturing subpatterns that occur inside negative lookahead assertions are counted, but their entries in the offsets vector are never set. Perl sets its numerical variables from any such patterns that are matched before the assertion fails to match something (thereby succeeding), but only if the negative lookahead assertion contains just one branch. 

4. Though binary zero characters are supported in the subject string, they are not allowed in a pattern string because it is passed as a normal C string, terminated by zero. The escape sequence '\0' can be used in the pattern to represent a binary zero. 

5. The following Perl escape sequences are not supported: \l, \u, \L, \U, \E, \Q. In fact these are implemented by Perl's general string-handling and are not part of its pattern matching engine. 

6. The Perl \G assertion is not supported as it is not relevant to single pattern matches. 

7. Fairly obviously, PCRE does not support the (?{code}) construction. 

8. There are at the time of writing some oddities in Perl 5.005_02 concerned with the settings of captured strings when part of a pattern is repeated. For example, matching 'aba' against the pattern /^(a(b)?)+$/ sets $2 to the value 'b', but matching 'aabbaa' against /^(aa(bb)?)+$/ leaves $2 unset. However, if the pattern is changed to /^(aa(b(b))?)+$/ then $2 (and $3) get set. 

In Perl 5.004 $2 is set in both cases, and that is also true of PCRE. If in the future Perl changes to a consistent state that is different, PCRE may change to follow. 

9. Another as yet unresolved discrepancy is that in Perl 5.005_02 the pattern /^(a)?(?(1)a|b)+$/ matches the string 'a', whereas in PCRE it does not. However, in both Perl and PCRE /^(a)?a/ matched against 'a' leaves $1 unset. 

10. PCRE provides some extensions to the Perl regular expression facilities: 

(a) Although lookbehind assertions must match fixed length strings, each alternative branch of a lookbehind assertion can match a different length of string. Perl 5.005 requires them all to have the same length. 

(b) If 'E' is set and 's' is not set, the $ meta- character matches only at the very end of the string. 

(c) If 'X' is set, a backslash followed by a letter with no special meaning is faulted. 

(d) If 'U' is set, the greediness of the repetition quantifiers is inverted, that is, by default they are not greedy, but if followed by a question mark they are. 
"!
]style[(16 4 41 4 14 91 15 828 12 30 11 727 21 3340)f1b,f1,bf3,f1,f2b,f1,f2b,f1,f2b,f1,f2b,f1,bf3,f1! !

!Re methodsFor: 'documentation' stamp: 'acg 8/4/2002 12:56'!
anOptionsComment "

Compilation and Matching Options

Message Name			Code	Explanation

beCaseSensitive			-i		Case sensitive matching
beNotCaseSensitive		i		Ignore case during matching
beNotMultiline			-m		Anchor chars don't match line ending
beMultiline				m		Anchor chars match on line ending
beNotDotIncludesNewline	-s		'.' does not match line ending
beDotIncludesNewline	s		'.' matches line endings
beNotExtended			-x		extended mode off (see below)
beExtended				x		extended mode on (see below)
beNotDollarEndOnly		-E		$ matches \n before end of line
beDollarEndOnly			E		$ does not match \n before end of line
beGreedy				-U		quantifiers have ordinary meaning
beNotGreedy				U		reverses meaning of * and :*, also + and :+
beNotExtra				-X		PCRE Extra mode off (see below)		
beExtra					X		PCRE Extra mode on (see below)
beNotAnchored			-A		Matches may begin anywhere
beAnchored				A		Matches must start with first character
beBeginningOfLine		-B		subject starts at beginning of a line
beNotBeginningOfLine	B		subject start not at beginning of a line
beEndOfLine				-Z		subject end may be at end of line
beNotEndOfLine			Z		subject end may not be at end of a line

In extended mode (beExtended), whitespace are ignored unless escaped, and # precedes comment to next newline.  PCRE Extra mode is described in detail in the accompanying documention.  Options may be changed at any time, but a pattern recompile occurs after changing the value any option other than anchored (A), beginning of line (B) or end of line (Z).

Options may be specified using messages or by Perl-style option codes:

'a.*y' asRe
	beNotCaseSensitive;
	beDotIncludesNewline;
	search: 'CANDY IS ', Character cr asString, 'DANDY, BUT LIQUOR IS QUICKER'

'a.*y' asRe
	opt: 'is';
	search: 'CANDY IS ', Character cr asString, 'DANDY, BUT LIQUOR IS QUICKER'

"!
]style[(17 1 1 1 33 1 33 15 82 14 96 23 85 13 83 18 98 8 104 14 36 1 47 17 87 20 106 15 761)f1b,f1,f1b,f1,bf3,bf1,bf2u,bf1,f1,bf1,f1,bf1,f1,bf1,f1,bf1,f1,bf1,f1,bf1,f1,bf1,f1,bf1,f1,bf1,f1,bf1,f1! !

!Re methodsFor: 'documentation' stamp: 'acg 8/4/2002 13:35'!
anReComment "

Re -- The RePlugin Pattern Matching Engine

I.	Introduction

RePlugin is a Squeak Plugin providing modern regular expression matching operations similar to those found in Perl. It was written by Andrew C. Greenberg (werdna@gate.net), with contributions by Markus Kohler, Stephen Pair and others. RePlugin 3.2 (and 'the Gory Details' portion of this document) is directly taken from Version 2.04 of the excellent PCRE library by Philip Hazel with only minor modifications.  A table of Re public methods and String convenience methods is available in Re anReMethodsComment.

II.	Creating a Pattern Matching Engine Object

A pattern matcher for a pattern string may be created as follows:

	Re on: '\w+'

or by using the convenient String conversion method:

	'\w+' asRe

III.	Using the Pattern Matching Engine Object

	A.	Simple Searching.  A number of methods are provided for evaluating a target string with an engine.  To search a string:

	'\w+' asRe search: 'this will select the first word from me'.

which will return nil if not matched, or a ReMatch object corresponding to the match result information.  A substring can be searched with the following message:

	'\w+' asRe 
		search: 'this will select the second word' 
		from: 5 
		to: 10

	B.	Global Searching.  Methods are provided for collecting non-overlapping matches in a string.

	'\w+' asRe searchAndCollect: 'this will give a collection of my words'

or for doing a global search with more complex matching

	'\w+' asRe 
		search: 'makes a list of words in brackets' 
		andCollect: [:m | '<', m match, '>']

	C.	Global Replacement.  Methods are provided to globally search and substitute into a string.

	'\w+' asRe 
		search: 'makes a string with words in brackets' 
		andReplace: [:m | '<', m match, '>']

There is a special case in the instance where the empty string is matched, because the 'next' match would begin in the same place, thereby creating an infinite loop. This case is handled as in Perl 5.004, where an empty string is replaced with the result of calling the block, and the next search begins after 'bumping' the string to the next character. Accordingly, 

	'Thanks Markus and Steve for all your help' reMatch: 'x*' andReplace: [:m | '!!' ].

will answer: 

	'!!T!!h!!a!!n!!k!!s!! !!M!!a!!r!!k!!u!!s!! !!a!!n!!d!! !!S!!t!!e!!v!!e!! !!f!!o!!r!! !!a!!l!!l!! !!y!!o!!u!!r!! !!h!!e!!l!!p!!'

IV.	Search Engine Options

The pattern matching engine can be modified to match in a variety of different ways.  Re anOptionsComment describes those options in greater detail. Options may be set or reset using #be... and #beNot... messages.

	'a.*y' asRe
		beNotCaseSensitive;
		beDotIncludesNewline;
		search: 'CANDY IS ', Character cr asString, 
			'DANDY, BUT LIQUOR IS QUICKER'

or by using Perl-style option characters

	'a.*y' asRe
		opt: 'is';
		search: 'CANDY IS ', Character cr asString, 
			'DANDY, BUT LIQUOR IS QUICKER'

"!
]style[(11 4 42 2 15 490 22 2 47 149 48 19 413 22 304 25 732 26 1 86 19 402)f1b,f1,f3b,f1,bf2,f1,f1LRe anReMethodsComment;,f1,f2b,f1,bf2,bf1,f1,f1b,f1,f1b,f1,bf2,f2b,f1,f1LRe anOptionsComment;,f1! !

!Re methodsFor: 'documentation' stamp: 'acg 8/4/2002 11:35'!
anReOverviewComment "

RePlugin -- A Regular Expressions Plugin for Squeak

Introduction

RePlugin is a Squeak Plugin providing modern regular expression matching operations similar to those found in Perl. It was written by Andrew C. Greenberg (werdna@gate.net), with contributions by Markus Kohler, Stephen Pair and others. RePlugin 3.2 (and 'the Gory Details' portion of this document) is directly taken from Version 2.04 of the excellent PCRE library by Philip Hazel with only minor modifications.

RePlugin, an Overview

While the primary functionality (and documentation) for RePlugin is found in new classes RePattern and ReMatch and the operations set forth therein, a comprehensive set of convenience functions are provided in the String class for ease of use. 

A Simple Example to Get You Started

After installing RePlugin, you can execute the following in a workspace: 

	'Candy is dandy, but liquor is quicker.'  reMatch: 'a.*y'

This reMatch: message directs RePlugin to search the longer string for the leftmost occurrence of the letter 'a', followed by the longest string that can be collected thereafter comprising any characters, but ending in a 'y.' The message answers: 

	 a ReMatch('andy is dandy')

(*blush*) which is an object of type ReMatch. As you shall see later, ReMatch objects can be saved to obtain a wide range of information about the match result. When printed, as here, it conveniently identifies the substring that was actually matched, which can also be obtained from the ReMatch instance by sending it the message match. (Note that the longer string 'andy is dandy' was matched, and not the shorter 'andy'.) If there was no match of the string, for example if the subject string were 

	'You got 'y', but only after the 'a''

then the message would answer nil.  A common use of regular expression matching is simply to determine as a boolean result whether the pattern has been matched (similar to the #match method).  Accordingly, a convenience function is provided:

	('Candy is dandy, but liquor is quicker.' matchRe: 'a.*y') ifTrue: ['matched'] ifFalse: ['not matched']

Global Searching and Replacing

It is sometimes convenient to ask ReMatch to repeatedly search for non-overlapping matches of a regular expression, and to report a collection of information with respect to each of the matches found. For example, the message: 

	'Stupid is as stupid does.' reMatch: 'stupid' andCollect: [:m | m match ].

This message looks for occurrences of the regular expression 'stupid' in the subject string. Each time a match is found, the corresponding match object is passed to the block associated with the collect: keyword, and the results of those computations are returned in an OrderedCollection. Since the first occurrence begins with a capital, only one match is found. (You could collect all occurrences either by using a character class or the i modifier, for example, using the reMatch:opt:collect: message.) In this case, however, the answer will be: 

	OrderedCollection ('stupid' )

As a somewhat more useful example, 

	'Stupid is as stupid does.' reMatch: '\w+' andCollect: [:m | m match ].

can be used to collect an ordered collection of all non-whitespace phrases in the string, in this case: 

 	OrderedCollection ('Stupid' 'is' 'as' 'stupid' 'does' )

This particular form (collecting matches) is used with such frequency that a convenience function is provided:

	'Stupid is as stupid does.' collectRe: '\w+'
 
Sometimes you will want to substitute text for the matched text, which you can accomplish with the reMatch:collect: message and some fancy footwork, or which you can do quite easily, for example, as follows: 

	'Stupid is as stupid does.' reMatch: 'stupid' opt: 'i' sub: [:m | 'Andy' ].

which answers a string replacing all occurrences of stupid (because of the opt: 'i', the search is done without regard to case) with 'Andy', yielding: 

	'Andy is as Andy does.'

You can also 'capture' text by surrounding regular expression subexpressions with parentheses. For example, consider the following expression: 

	'    line has leading spaces' reMatch: '^\W+(.*)'

which answers 

	a ReMatch('     line has leading spaces')

This would have little utility, since it merely copies the line of text entirely. But since RePlugin keeps track of which text is 'captured' by which parenthetical group, which is numbered in the order the left parenthesis appears in the string. These group matches can be seperately obtained by sending the resulting match object the message 'matchAt:,' for example: 

	('    line has leading spaces' reMatch: '^\W+(.*)') matchAt: 1

which answers 

	'line has leading spaces'

That is, the line without the leading white space. Indeed, RePlugin remembers these parenthetical captures during the match, so that you can check for double words as follows: 

	'this line has has a double word' reMatch: '(\w+)\W+\1'

which matches 

	a ReMatch('has has')

These and other regular expression operations are discussed in substantially greater detail below. 


Matching With RePlugin

The Principal Messages

You may call RePlugin in any of the following ways: 

	subjectString reMatch: pattern [from: from] [to: to] [opt: optionString]
	subjectString reMatch: [opt: optionString] sub: aBlock [num: maxMatches]
	subjectString reMatch: [opt: optionString] collect: aBlock [num: maxMatches]

The keywords in square brackets are optional, in the sense that messages are available with every combination of keywords shown, with and without the optional keywords. 

The first message performs a single search on the substring of subjectString from from to to, using the modifiers set forth in optionString. If from: is not specified, then 1 is used, if to: is not specified, then subjectString size is used, and if opt: is not specified, then the empty string is used. 
It should be noted that everywhere a pattern is permitted in these operations, either a string or compiled pattern object (an Re) may be used. If a string is used, then RePlugin will first search to see if the object was recently compiled, and if so, use that object, or if not, compiles the expression and remembers it for later reuse. If a compiled pattern object (an Re) is used, then that compiled object will be used, thereby avoiding recompilations and table lookups. 

The second message performs repeated searches of subjectString for nonoverlapping matches of pattern, using compile and matching options optionString until no more matches are present or maxMatches have been found. (If maxMatches is less than zero, the number of matches will be limited only by the number of matches in the string.) Then, for each match found, replace the matched substring with the result of applying the corresponding match object to aBlock. If opt: is not specified, then the empty string is used, and if num: is not specified, then the equivalent of -1 is used. 

There is a special case in the instance where the empty string is matched, because the 'next' match would begin in the same place, thereby creating an infinite loop. This case is handled as in Perl 5.004, where an empty string is replaced with the result of calling the block, and the next search begins after 'bumping' the string to the next character. Accordingly, 

	'Thanks Markus and Steve for all your help' reMatch: 'x*' sub: [:m | '!!' ].  

will answer: 

	'!!T!!h!!a!!n!!k!!s!! !!M!!a!!r!!k!!u!!s!! !!a!!n!!d!! !!S!!t!!e!!v!!e!! !!f!!o!!r!! !!a!!l!!l!! !!y!!o!!u!!r!! !!h!!e!!l!!p!!'

Finally, the third message performs repeated searches of subjectString for nonoverlapping matches of pattern, using compile and matching options optionString until no more matches are present or maxMatches have been found. (If maxMatches is less than zero, the number of matches will be limited only by the number of matches in the string.) Then, for each match found, evalute aBlock with the corresponding matchObject, and maintain and then answer an ordered collection of the results in the order they were computed. If opt: is not specified, then the empty string is used, and if num: is not specified, then the equivalent of -1 is used. 

reMatch:collect: handles empty string in the same manner as reMatch:sub:, with the added proviso that an empty match will not be counted if it immediately follows a non-empty match. Accordingly 

	'123' reMatch: '\d*' collect: [:m | m match]

answers 

	OrderedCollection ('123' )

and not 'OrderedCollection ('123' ''),' although 

	'123' reMatch: '\d*' sub: [:m | '<', m match, '>']

will answer 

	 '<123><>'

These null match rules mirror the semantics of Perl 5's m/.../g and s/.../g operators. 

Using ReMatch to Obtain Principal Match Information

The substring of the substring matched by re is given by: 

	m match

The beginning and end of the substring in searchString is given by the messages from and to, respectively, so that the substring matched (the result of m match could be obtained with: 

 	m searchString
		copyFrom: (m from)
		to: (m to)


Using ReMatch to Obtain Captured Groups (and Collections of Captured Groups)

The number of substrings capturable by a parenthetical grouping in an re (regardless of the number actually matched to create m) is given by: 

	m numGroups
	
The string captured by parenthetical grouping i, where 1<=i<=(m numGroups) is given by 

	m matchAt: i

and this can be generated as follows: 

	m searchString
		copyFrom: (m fromAt: i)
		to: (m toAt: i)

And an array of size (m numGroups) can be generated from strings and indices accordingly: 

	m matches
	m froms
	m tos


Efficient Regular Expression Matching

RePattern tests for regular expression matching in three stages: 

1. Compiles the regular expression into a convenient internal form. 
2. Searches an object string or substring for matches. 
3. Produces results of queries on a match object. 

If you intend to repeatedly matching a single regular expression against many different strings, for example each line of a file or element of a collection, then repeating Step 1, the compilation, would be wasteful and inefficient. RePattern avoids recompilation by keeping track of the last dozen or so compiled regular expressions, avoiding the costly process of recompilation. Unfortunately, this adds the (less inefficient) cost of a table lookup with each regular expression match. 

Accordingly, RePattern permits you to generate and keep 'compiled pattern objects,' for repeated matching against subsequent strings without recompiling or searching the compilation cache. You can create an compiled pattern object with the asRePattern message: 

	'\w+' asRePattern

which answers 

	an Re('\w+ ')

and the resulting pattern can be used wherever a pattern string can be used, except that no recompilation or table lookup occurs. The following: 

	re := '\w+' asRePattern
	myCollection do: [:i|
		Transcript show: ((i reMatch: re) match); cr]

will be substantially faster than 

	myCollection do: [:i|
		Transcript show: ((i reMatch: '\w+') match); cr]

Regular Expression Syntax Summary

A regular expression (or regexp) specifies a set of strings that matches it. Regular expressions can be concatenated to form new regular expressions; if A and B are both regular expressions, then AB is also an regular expression. If a string p matches A and another string q matches B, the string pq will match AB. Thus, complex expressions are easily constructed from simpler primitive expressions. 

Regular expressions can contain both special and ordinary characters. Most ordinary characters, like 'A', 'a', or '0', are the simplest regular expressions; they simply match themselves. You can concatenate ordinary characters, so last matches the string 'last'. 

Some characters, like '|' or '(', are special. Special characters either stand for classes of ordinary characters, or affect how the regular expressions around them are interpreted. 

The special characters are: 

'.' 
(Dot.) In the default mode, this matches any character except a newline. If the 's' option has been specified, dot matches any character at all, including a newline. 

'^' 
(Caret.) Matches the start of the string, and if the 'm' option has been specified, then this also matches immediately after each newline. 

'$' 
Matches the end of the string, and if the 'm' option has been specified, then this also matches before a newline. foo matches both 'foo' and 'foobar', while the regular expression foo$ matches only 'foo'. 

'*' 
Causes the resulting regexp to match 0 or more repetitions of the preceding regexp, as many repetitions as are possible. ab* will match 'a', 'ab', or 'a' followed by any number of 'b's. 

'+' 
Causes the resulting regexp to match 1 or more repetitions of the preceding regexp. ab+ will match 'a' followed by any non-zero number of 'b's; it will not match just 'a'. 

'?' 
Causes the resulting regexp to match 0 or 1 repetitions of the preceding regexp. ab? will match either 'a' or 'ab'. 

*?, +?, ?? 
The '*', '+', and '?' qualifiers are all greedy; they match as much text as possible. Sometimes this behaviour isn't desired; if the regexp <.*> is matched against '<H1>title</H1>', it will match the entire string, and not just '<H1>'. Adding '?' after the qualifier makes it perform the match in non-greedy or minimal fashion; as few characters as possible will be matched. Using .*? in the previous expression will match only '<H1>'. 

{m,n} 
Causes the resulting regexp to match from m to n repetitions of the preceding regexp, attempting to match as many repetitions as possible. For example, a{3,5} will match from 3 to 5 'a' characters. Omitting n specifies an infinite upper bound; you can't omit m. 

{m,n}? 
Causes the resulting regexp to match from m to n repetitions of the preceding regexp, attempting to match as few repetitions as possible. This is the non-greedy version of the previous qualifier. For example, on the 6-character string 'aaaaaa', a{3,5} will match 5 'a' characters, while a{3,5}? will only match 3 characters. 

'\' 
Either escapes special characters (permitting you to match characters like '*', '?', and so forth), or signals a special sequence; special sequences are discussed below. 

[] 
Used to indicate a set of characters. Characters can be listed individually, or a range of characters can be indicated by giving two characters and separating them by a '-'. Special characters are not active inside sets. For example, [akm$] will match any of the characters 'a', 'k', 'm', or '$'; [a-z] will match any lowercase letter, and [a-zA-Z0-9] matches any letter or digit. Character classes such as \w or \S(defined below) are also acceptable inside a range. If you want to include a ']' or a '-' inside a set, precede it with a backslash, or place it as the first character. The pattern []] will match ']', for example. 

You can match the characters not within a range by complementing the set. This is indicated by including a '^' as the first character of the set; '^' elsewhere will simply match the '^' character. For example, [^5] will match any character except '5'. 

'|' 
A|B, where A and B can be arbitrary regexps, creates a regular expression that will match either A or B. This can be used inside groups (see below) as well. To match a literal '|', use \|, or enclose it inside a character class, as in [|]. 

(...) 
Matches whatever regular expression is inside the parentheses, and indicates the start and end of a group; the contents of a group can be retrieved after a match has been performed, and can be matched later in the string with the \number special sequence, described below. To match the literals '(' or '')', use \( or \), or enclose them inside a character class: [(] [)]. 

(?...) 
This is an extension notation (a '?' following a '(' is not meaningful otherwise). The first character after the '?' determines what the meaning and further syntax of the construct is. Following are the currently supported extensions: 

(?imsx[-imsx]) 
(One or more letters from the set 'i', 'm', 's', 'x'.) The group matches the empty string and set (or unset if the letters follow a '-') corresponding options for the regular expression or subexpression in which it is contained. 

(?:...) 
A non-grouping version of regular parentheses. Matches whatever regular expression is inside the parentheses, but the substring matched by the group cannot be retrieved after performing a match or referenced later in the pattern. 

(?#...) 
A comment; the contents of the parentheses are simply ignored. 

(?=...) 
Matches if ... matches next, but doesn't consume any of the string. This is called a lookahead assertion. For example, Isaac (?=Asimov) will match 'Isaac ' only if it's followed by 'Asimov'. 

(?!!...) 
Matches if ... doesn't match next. This is a negative lookahead assertion. For example, Isaac (?!!Asimov) will match 'Isaac ' only if it's not followed by 'Asimov'. 

(?<=...) 
Matches if ... matches, but doesn't consume any of the string. This is called a lookbehind assertion. For example, (?<=foo|fooey)bar will match 'bar' only if it's preceded by 'foo' or 'fooey'. All lookbehinds must have some fixed length, although alternatives need not be of the same length, as in the example. 

(?<!!...) 
Matches if ... doesn't match, and doesn't consume any of the string. This is called a negative lookbehind assertion. For example, (?<=foo|fooey)bar will match 'bar' only if it's not preceded by 'foo' or 'fooey'. 

(?(condition)yes-pattern) 
Matches if condition is false or if condition is true and yes-pattern matches. 

(?(condition)yes-pattern|no-pattern) 
Matches if condition is true and yes-pattern matches, or if condition is false and no-pattern matches. 

The special sequences consist of '\' and a character from the list below. If the ordinary character is not on the list, then the resulting regexp will match the second character. For example, \$ matches the character '$'. 

\number 
Matches the contents of the group of the same number. Groups are numbered starting from 1. For example, (.+) \1 matches 'the the' or '55 55', but not 'the end' (note the space after the group). This special sequence can only be used to match one of the first 99 groups. If the first digit of number is 0, or number is 3 octal digits long, it will not be interpreted as a group match, but as the character with octal value number. Inside the '[' and ']' of a character class, all numeric escapes are treated as characters. 
\A 
Matches only at the start of the string. 
\b 
Matches the empty string, but only at the beginning or end of a word. A word is defined as a sequence of alphanumeric characters, so the end of a word is indicated by whitespace or a non-alphanumeric character. 
\B 
Matches the empty string, but only when it is not at the beginning or end of a word. 
\d 
Matches any decimal digit; this is equivalent to the set [0-9]. 
\D 
Matches any non-digit character; this is equivalent to the set [^0-9]. 
\s 
Matches any whitespace character; this is equivalent to the set [ \t\n\r\f\v]. 
\S 
Matches any non-whitespace character; this is equivalent to the set [^ \t\n\r\f\v]. 
\w 
Matches any alphanumeric character; this is equivalent to the set [a-zA-Z0-9_]. 
\W 
Matches any non-alphanumeric character; this is equivalent to the set [^a-zA-Z0-9_]. 

\Z 
Matches only at the end of the string. 

\\ 
Matches a literal backslash. 


Compiler and Matching Option Modes Summary

  i  for Caseless Matching Mode
  m  for Multiline Mode
  s  for Dotall Mode (Dot matches newlines)
  x  for Extended Mode (whitespace not meaningful, comments permitted)
  A  for Anchored mode
  B  for NOTBOL mode (see below)
  E  for 'Dollar end only' mode (see below)
  U  for Ungreedy mode -- greediness of operators is reversed
  X  for PCRE 'Extra' mode (see below)
  Z  for NOTEOL mode (see below)

Options B and Z are available only when matching. Option A is available for both matching and compiling. The remaining options are available only for compiling patterns. "!
]style[(19 4 51 2 12 414 21 248 36 1307 30 17815)f1b,f1,f3b,f1,f2b,f1,f2b,f1,f2b,f1,f2b,f1! !

!Re methodsFor: 'accessing' stamp: 'acg 7/30/2002 21:05'!
action

	^action! !

!Re methodsFor: 'accessing' stamp: 'acg 7/30/2002 21:05'!
action: aBlock

	action _ aBlock! !

!Re methodsFor: 'accessing' stamp: 'acg 8/4/2002 10:37'!
asRe

	^self! !

!Re methodsFor: 'accessing' stamp: 'acg 7/29/2002 13:34'!
compiledPattern

	^compiledPattern! !

!Re methodsFor: 'accessing' stamp: 'acg 7/29/2002 13:35'!
compiledPattern: anRePattern

	^compiledPattern _ anRePattern! !

!Re methodsFor: 'accessing' stamp: 'acg 7/30/2002 12:49'!
on: aPattern

	self pattern: aPattern! !

!Re methodsFor: 'accessing' stamp: 'acg 7/29/2002 14:25'!
pattern

	compiledPattern _ nil.
	^pattern! !

!Re methodsFor: 'accessing' stamp: 'acg 7/29/2002 13:58'!
pattern: aString

	compiledPattern _ nil.
	pattern _ aString! !

!Re methodsFor: 'accessing' stamp: 'acg 7/30/2002 17:39'!
searchLimit

	^searchLimit! !

!Re methodsFor: 'accessing' stamp: 'acg 7/30/2002 17:40'!
searchLimit: anInteger

	searchLimit _ anInteger! !

!Re methodsFor: 'accessing' stamp: 'acg 7/30/2002 12:57'!
searchString

	^searchString! !

!Re methodsFor: 'accessing' stamp: 'acg 7/30/2002 21:38'!
searchString: aString

	searchString _ aString.! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 21:50'!
beAnchored 

	self isAnchored: true! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 21:50'!
beBeginningOfLine

	self isBeginningOfLine: true! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 21:51'!
beCaseSensitive

	self isCaseSensitive: true! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 21:51'!
beDollarEndOnly

	self isDollarEndOnly: true! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 21:51'!
beDotIncludesNewline

	self isDotIncludesNewline: true! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 21:51'!
beEndOfLine

	self isEndOfLine: true! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 21:51'!
beExtended
	
	self isExtended: true! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 21:53'!
beExtra 

	self isExtra: true! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 21:54'!
beGreedy

	self isGreedy: true! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 21:54'!
beMultiline

	self isMultiline: true! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 21:54'!
beNotAnchored 

	self isAnchored: false! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 21:55'!
beNotBeginningOfLine

	self isBeginningOfLine: false! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 21:55'!
beNotCaseSensitive

	self isCaseSensitive: false! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 21:56'!
beNotDollarEndOnly

	self isDollarEndOnly: false! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 21:56'!
beNotDotIncludesNewline

	self isDotIncludesNewline: false! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 21:57'!
beNotEndOfLine

	self isEndOfLine: false! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 21:57'!
beNotExtended
	
	self isExtended: false! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 21:57'!
beNotExtra

	self isExtra: false! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 21:57'!
beNotGreedy

	self isGreedy: false! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 21:57'!
beNotMultiline

	self isMultiline: false! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 11:40'!
isAnchored 

	^isAnchored! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 11:55'!
isAnchored: aBoolean

	isAnchored _ aBoolean! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 22:01'!
isBeginningOfLine

	^isBeginningOfLine! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 22:02'!
isBeginningOfLine: aBoolean

	isBeginningOfLine == aBoolean 
		ifFalse:[compiledPattern _ nil].
	isBeginningOfLine _ aBoolean! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 12:06'!
isCaseSensitive

	^isCaseSensitive! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 22:04'!
isCaseSensitive: aBoolean

	isCaseSensitive == aBoolean 
		ifFalse:[compiledPattern _ nil].
	isCaseSensitive _ aBoolean! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 11:41'!
isDollarEndOnly 

	^isDollarEndOnly! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 22:05'!
isDollarEndOnly: aBoolean

	isDollarEndOnly == aBoolean 
		ifFalse:[compiledPattern _ nil].
	isDollarEndOnly _ aBoolean! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 14:36'!
isDotIncludesNewline

	^isDotIncludesNewline! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 22:07'!
isDotIncludesNewline: aBoolean

	isDotIncludesNewline == aBoolean 
		ifFalse:[compiledPattern _ nil].
	isDotIncludesNewline _ aBoolean! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 11:42'!
isEndOfLine

	^isEndOfLine! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 11:54'!
isEndOfLine: aBoolean

	isEndOfLine _ aBoolean! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 11:42'!
isExtended 

	^isExtended! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 22:07'!
isExtended: aBoolean

	isExtended == aBoolean 
		ifFalse:[compiledPattern _ nil].
	isExtended _ aBoolean! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 11:42'!
isExtra 

	^isExtra! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 22:08'!
isExtra: aBoolean

	isExtra == aBoolean 
		ifFalse:[compiledPattern _ nil].
	isExtra _ aBoolean! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 11:42'!
isGreedy

	^isGreedy! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 22:08'!
isGreedy: aBoolean

	isGreedy == aBoolean 
		ifFalse:[compiledPattern _ nil].
	isGreedy _ aBoolean! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 11:42'!
isMultiline

	^isMultiline! !

!Re methodsFor: 'accessing options' stamp: 'acg 7/29/2002 22:08'!
isMultiline: aBoolean

	isMultiline == aBoolean 
		ifFalse:[compiledPattern _ nil].
	isMultiline _ aBoolean! !

!Re methodsFor: 'accessing options' stamp: 'acg 8/3/2002 10:47'!
opt: aString

	| setOrReset |
	setOrReset _ true.
	aString do: [:ch |
		ch = $-
			ifTrue: [setOrReset _ setOrReset not]
			ifFalse: [self setOptionForPCRECharacter: ch to: setOrReset]]! !

!Re methodsFor: 'accessing options' stamp: 'acg 8/3/2002 10:46'!
setOptionForPCRECharacter: aCharacter to: aBoolean

	aCharacter = $i ifTrue: [^self isCaseSensitive: aBoolean not].
	aCharacter = $m ifTrue:[^self isMultiline: aBoolean].
	aCharacter = $s ifTrue:[^self isDotIncludesNewline: aBoolean].
	aCharacter = $x ifTrue:[^self isExtended: aBoolean].
	aCharacter = $E ifTrue:[^self isDollarEndOnly: aBoolean].
	aCharacter = $U ifTrue:[^self isGreedy: aBoolean not].
	aCharacter = $X ifTrue:[^self isExtra: aBoolean].
	aCharacter = $A ifTrue:[^self isAnchored: aBoolean].
	aCharacter = $B ifTrue:[^self isBeginningOfLine: aBoolean not].
	aCharacter = $Z ifTrue:[^self isEndOfLine: aBoolean not].
	Error signal: '$', aCharacter asString, ' is not a PCRE option character.'
! !

!Re methodsFor: 'constants' stamp: 'acg 7/30/2002 10:43'!
PCREANCHORED

	^16! !

!Re methodsFor: 'constants' stamp: 'acg 7/30/2002 10:43'!
PCRECASELESS

	^1

! !

!Re methodsFor: 'constants' stamp: 'acg 7/30/2002 10:44'!
PCREDOLLARENDONLY

	^32
! !

!Re methodsFor: 'constants' stamp: 'acg 7/30/2002 10:44'!
PCREDOTALL

	^4! !

!Re methodsFor: 'constants' stamp: 'acg 7/30/2002 10:44'!
PCREEXTENDED

	^8! !

!Re methodsFor: 'constants' stamp: 'acg 7/30/2002 10:44'!
PCREEXTRA

	^64
! !

!Re methodsFor: 'constants' stamp: 'acg 7/30/2002 10:44'!
PCREMULTILINE

	^2! !

!Re methodsFor: 'constants' stamp: 'acg 7/30/2002 10:45'!
PCRENOTBOL

	^128! !

!Re methodsFor: 'constants' stamp: 'acg 7/30/2002 10:45'!
PCRENOTEOL

	^256! !

!Re methodsFor: 'constants' stamp: 'acg 7/30/2002 11:44'!
PCREUNGREEDY

	^512! !

!Re methodsFor: 'constants' stamp: 'acg 7/30/2002 11:45'!
codedOptionsForCompile

	| optionCode |

	optionCode _ 0.
	isAnchored ifTrue: [optionCode _ optionCode bitOr: self PCREANCHORED].

	isCaseSensitive ifFalse: [optionCode _ optionCode bitOr: self PCRECASELESS].
	isMultiline ifTrue: [optionCode _ optionCode bitOr: self PCREMULTILINE].
	isDotIncludesNewline ifTrue: [optionCode _ optionCode bitOr: self PCREDOTALL].
	isExtended ifTrue: [optionCode _ optionCode bitOr: self PCREEXTENDED].
	isDollarEndOnly ifTrue: [optionCode _ optionCode bitOr: self PCREDOLLARENDONLY].
	isExtra ifTrue: [optionCode _ optionCode bitOr: self PCREEXTRA].
	isGreedy ifFalse: [optionCode _ optionCode bitOr: self PCREUNGREEDY].

	^optionCode
! !

!Re methodsFor: 'constants' stamp: 'acg 7/30/2002 11:46'!
codedOptionsForMatch

	| optionCode |

	optionCode _ 0.

	isAnchored ifTrue: [optionCode _ optionCode bitOr: self PCREANCHORED].

	isBeginningOfLine ifFalse: [optionCode _ optionCode bitOr: self PCRENOTBOL].
	isEndOfLine ifFalse: [optionCode _ optionCode bitOr: self PCRENOTEOL].

	^optionCode! !

!Re methodsFor: 'compiling' stamp: 'acg 8/3/2002 11:36'!
assureCompiledPattern

	compiledPattern ifNil: [self compile].
	^compiledPattern! !

!Re methodsFor: 'compiling' stamp: 'acg 8/1/2002 02:48'!
compile

	compiledPattern _ RePattern new 
		compile: pattern 
		optCode: self codedOptionsForCompile 
		onErrorRun: [:x :y :errorString | Error signal: errorString].! !

!Re methodsFor: 'searching' stamp: 'acg 8/3/2002 11:56'!
collectFrom: aString

	^self search: aString andCollect: [:m | m match]! !

!Re methodsFor: 'searching' stamp: 'acg 8/6/2002 16:31'!
grepFrom: inStream

	^String streamContents: [:s | self grepFrom: inStream to: s]! !

!Re methodsFor: 'searching' stamp: 'acg 8/6/2002 16:19'!
grepFrom: inStream to: outStream

	"String streamContents: [:s |
		'a' asRe
			grepFrom: (ReadStream on: 'this is a test
of the emergency
broadcast system')
			to: s]"

	self grepFrom: inStream
		to: outStream
		onMatch: [:s :m | s nextPutAll: m searchString; cr]
		onNonMatch: [:s :l | ]! !

!Re methodsFor: 'searching' stamp: 'acg 8/6/2002 16:16'!
grepFrom: inStream to: outStream onMatch: matchBlock onNonMatch: nonMatchBlock

	| m line |
	[inStream atEnd] whileFalse:
		[(m _ self search: (line _ inStream nextLine))
			ifNil: [nonMatchBlock value: outStream value: line]
			ifNotNil: [matchBlock value: outStream value: m]]! !

!Re methodsFor: 'searching' stamp: 'acg 8/3/2002 11:38'!
search: aString

	^(self assureCompiledPattern)
		search: aString 
		from: 1 
		to: aString size 
		optCode: self codedOptionsForMatch! !

!Re methodsFor: 'searching' stamp: 'acg 8/3/2002 11:41'!
search: aString andCollect: aBlock

	^(self assureCompiledPattern)
		search: aString
		optCode: self codedOptionsForMatch
		collect: aBlock! !

!Re methodsFor: 'searching' stamp: 'acg 8/3/2002 11:45'!
search: aString andCollect: aBlock matchCount: anInteger

	^(self assureCompiledPattern)
		search: aString
		optCode: self codedOptionsForMatch
		collect: aBlock
		num: anInteger! !

!Re methodsFor: 'searching' stamp: 'acg 8/3/2002 23:36'!
search: aString andReplace: aBlock

	^(self assureCompiledPattern)
		search: aString
		optCode: self codedOptionsForMatch
		sub: aBlock
! !

!Re methodsFor: 'searching' stamp: 'acg 8/3/2002 11:45'!
search: aString andReplace: aBlock matchCount: anInteger

	^(self assureCompiledPattern)
		search: searchString
		optCode: self codedOptionsForMatch
		sub: action
		num: anInteger
	
! !

!Re methodsFor: 'searching' stamp: 'acg 8/3/2002 11:37'!
search: aString from: fromInteger to: toInteger

	^(self assureCompiledPattern)
		search: aString 
		from: fromInteger 
		to: toInteger 
		optCode: self codedOptionsForMatch! !

!Re methodsFor: 'searching' stamp: 'acg 8/3/2002 11:47'!
searchAndCollect: aString

	^self search: aString andCollect: [:m | m match]! !

!Re methodsFor: 'printing' stamp: 'acg 7/29/2002 13:27'!
printBoolean: aBoolean named: aString on: aStream

	aStream nextPut:$ .
	aBoolean ifFalse: [aStream nextPutAll: 'NOT '].
	aStream nextPutAll: aString.
	aStream nextPut:$.! !

!Re methodsFor: 'printing' stamp: 'acg 7/30/2002 12:09'!
printOn: aStream

	aStream nextPutAll: 'an Re'.
	pattern ifNotNil:
		[aStream nextPutAll: ' for '''.
		aStream nextPutAll: pattern.
		aStream nextPutAll: ''''].
	aStream nextPut: $(.
	self printBoolean: isAnchored named: 'anchored' on: aStream.
	aStream nextPut: $/.
	self printBoolean: isCaseSensitive named: 'case sensitive' on: aStream.
	self printBoolean: isDollarEndOnly named: 'dollar end only' on: aStream.
	self printBoolean: isDotIncludesNewline named: 'dot includes newline' on: aStream.
	self printBoolean: isExtended named: 'extended' on: aStream.
	self printBoolean: isExtra named: 'extra' on: aStream.
	self printBoolean: isGreedy named: 'greedy' on: aStream.
	self printBoolean: isMultiline named: 'multiline' on: aStream.
	aStream nextPut: $/.
	self printBoolean: isBeginningOfLine named: 'beginning of line' on: aStream.
	self printBoolean: isEndOfLine named: 'end of line' on: aStream.
	compiledPattern ifNil: [aStream nextPutAll: ' NOT'].
	aStream nextPutAll: ' compiled'.	
	aStream nextPut: $).! !

!Re methodsFor: 'private' stamp: 'acg 8/11/2002 11:19'!
beStrangeOption! !

!Re methodsFor: 'private' stamp: 'acg 7/30/2002 17:36'!
initialize

	pattern _ nil.
	self
		beNotAnchored;
		beCaseSensitive;
		beNotDollarEndOnly;
		beNotDotIncludesNewline;
		beNotExtended;
		beNotExtra;
		beNotMultiline;
		beBeginningOfLine;
		beEndOfLine;
		beGreedy.
	action _ [:m | m match].! !


!Re class methodsFor: 'as yet unclassified' stamp: 'acg 7/29/2002 13:31'!
new

	^super new initialize! !

!Re class methodsFor: 'as yet unclassified' stamp: 'acg 7/30/2002 12:50'!
on: aPattern

	^self new on: aPattern! !

!Re class methodsFor: 'as yet unclassified' stamp: 'acg 7/30/2002 12:51'!
on: aPattern search: aString

	^(self on: aPattern)
		search: aString
		from: 1
		to: aString size! !

!Re class methodsFor: 'as yet unclassified' stamp: 'acg 8/11/2002 23:34'!
status

	| code |
	^String streamContents: [:s |
		s nextPutAll: 'RePlugin Statuts:'; cr.
		s nextPutAll: '===================='; cr.
		s nextPutAll: 'Plugin version: '; cr.
		#( '\n' '\r' '\cj' ) do: [:eachCode |
			code _ 0.
			[code <=255 and: [(eachCode asRe search: code asCharacter asString) isNil]]
				whileTrue: [code _ code + 1].
			s nextPutAll: eachCode, ' = ', code hex; cr]]! !


!ReMatch methodsFor: 'accessing' stamp: 'acg 2/26/1999 23:57'!
endpos
	"Answer the final index of the substring of searchString searched to obtain me."

	^endpos! !

!ReMatch methodsFor: 'accessing' stamp: 'acg 2/26/1999 23:57'!
pos
	"Answer the initial index of the substring of searchString searched to obtain me."

	^pos! !

!ReMatch methodsFor: 'accessing' stamp: 'acg 2/26/1999 23:55'!
re
	"Answer the re matched to obtain me."

	^re! !

!ReMatch methodsFor: 'accessing' stamp: 'acg 2/26/1999 23:57'!
searchString
	"Answer the substring searched to obtain me."

	^searchString! !

!ReMatch methodsFor: 'principal matching' stamp: 'acg 3/15/1999 01:28'!
from
	"Answer the initial index of the substring matched by re."

	^self fromAt: 0! !

!ReMatch methodsFor: 'principal matching' stamp: 'acg 3/15/1999 01:28'!
match
	"Answer the substring matched by re."

	^self matchAt: 0! !

!ReMatch methodsFor: 'principal matching' stamp: 'acg 3/15/1999 01:29'!
to
	"Answer the final index of the substring matched by re."

	^self toAt: 0! !

!ReMatch methodsFor: 'subgroup matching' stamp: 'acg 3/15/1999 01:24'!
fromAt: anInteger
	"Answer the initial index of the substring matching grouping anInteger, or nil if group was not matched."

	| offset fromIndex |
	offset _ 2 * anInteger.
	((fromIndex _ matchArray at: (offset + 1)) < 0)
		ifTrue: [^nil].
	^ fromIndex + pos! !

!ReMatch methodsFor: 'subgroup matching' stamp: 'acg 3/15/1999 01:24'!
matchAt: anInteger
	"Answer the substring matching grouping anInteger, or nil if group was not matched."

	| offset fromIndex |
	offset _ 2 * anInteger.
	((fromIndex _ matchArray at: (offset + 1)) < 0)
		ifTrue: [^nil].
	^ searchString
		copyFrom: (fromIndex + pos)
		to: ((matchArray at: (offset + 2)) + pos - 1).! !

!ReMatch methodsFor: 'subgroup matching' stamp: 'acg 3/15/1999 01:24'!
numGroups
	"Answer the number SubGroups (not including the entire match) potentially matched by re, whether actually matched or not."

	^ ((matchArray size) // 3) - 1! !

!ReMatch methodsFor: 'subgroup matching' stamp: 'acg 3/15/1999 01:24'!
toAt: anInteger
	"Answer the final index of the substring matching grouping anInteger, or nil if group was not matched."

	| offset |
	offset _ 2 * anInteger.
	((matchArray at: (offset + 1)) < 0)
		ifTrue: [^nil].
	^ (matchArray at: (offset + 2)) + pos - 1! !

!ReMatch methodsFor: 'subgroup collections' stamp: 'acg 2/26/1999 23:52'!
froms
	"Answer an Array of initial indices of grouping substrings as matched, or nil, respectively."

	^(Array new: (self numGroups))
		collectWithIndex: [:n :i | self fromAt: i].! !

!ReMatch methodsFor: 'subgroup collections' stamp: 'acg 2/26/1999 23:53'!
matches
	"Answer an Array of grouping substrings as matched, or nil, respectively."

	^(Array new: (self numGroups))
		collectWithIndex: [:n :i | self matchAt: i].! !

!ReMatch methodsFor: 'subgroup collections' stamp: 'acg 2/26/1999 23:53'!
tos
	"Answer an Array of final indices of grouping substrings as matched, or nil, respectively."

	^(Array new: (self numGroups))
		collectWithIndex: [:n :i | self toAt: i].! !

!ReMatch methodsFor: 'private' stamp: 'acg 3/15/1999 01:29'!
matchArray: anIntegerArray forRe: aRePattern 
onString: aString from: startInteger to: stopInteger 
	"Initialize an instance of me in accordance with the parameters."

	matchArray _ anIntegerArray copy.
	re _ aRePattern.
	searchString _ aString.
	pos _ startInteger.
	endpos _ stopInteger.
! !

!ReMatch methodsFor: 'printing' stamp: 'acg 3/7/1999 08:04'!
printOn: aStream

	aStream nextPutAll: 'a '.
	(self species) printOn: aStream.
	aStream nextPut: $(.
	(self match) printOn: aStream.
	aStream nextPut: $).! !


!ReMatch class methodsFor: 'instance creation' stamp: 'acg 2/25/1999 20:41'!
matchArray: anIntegerArray forRe: aRePattern 
onString: aString from: startInteger to: stopInteger 

	^super new 
		matchArray: anIntegerArray 
		forRe: aRePattern 
		onString: aString 
		from: startInteger 
		to: stopInteger 
! !

!ReMatch class methodsFor: 'pattern matching' stamp: 'acg 3/7/1999 07:56'!
search: subjString match: patString opt: oStr 

	|re|
	re _ RePattern 
			on: patString 
			opt: (oStr select: [:ch| 'imsxAEUX' includes: ch]).
	^ re 
		search: subjString 
		opt: (oStr select: [:ch| 'ABZ' includes: ch])! !

!ReMatch class methodsFor: 'pattern matching' stamp: 'acg 3/7/1999 07:56'!
search: subjString match: patString opt: oStr from: startInteger to: stopInteger 

	|re|
	re _ RePattern 
			on: patString 
			opt: (oStr select: [:ch| 'imsxAEUX' includes: ch]).
	^ re 
		search: subjString 
		from: startInteger
		to: stopInteger
		opt: (oStr select: [:ch| 'ABZ' includes: ch])! !

!ReMatch class methodsFor: 'deprecated' stamp: 'acg 2/28/1999 02:23'!
on: srchString search: subjString

	^ self 
		on: srchString 
		search: subjString 
		opt: ''! !

!ReMatch class methodsFor: 'deprecated' stamp: 'acg 2/28/1999 02:23'!
on: srchString search: subjString from: startInteger

	^ self 
		on: srchString 
		search: subjString 
		from: startInteger
		to: (subjString size)
		opt: ''! !

!ReMatch class methodsFor: 'deprecated' stamp: 'acg 2/28/1999 02:23'!
on: srchString search: subjString from: startInteger opt: optString

	^ self 
		on: srchString 
		search: subjString 
		from: startInteger
		to: (subjString size)
		opt: optString! !

!ReMatch class methodsFor: 'deprecated' stamp: 'acg 2/28/1999 02:23'!
on: srchString search: subjString from: startInteger to: stopInteger

	^ self 
		on: srchString 
		search: subjString 
		from: startInteger
		to: stopInteger
		opt: ''! !

!ReMatch class methodsFor: 'deprecated' stamp: 'acg 2/28/1999 04:38'!
on: srchString search: subjString from: startInteger to: stopInteger opt: optString

	|re|
	re _ RePattern 
			on: srchString 
			opt: (optString select: [:ch| 'imsxAEUX' includes: ch]).
	^ re 
		search: subjString 
		from: startInteger
		to: stopInteger
		opt: (optString select: [:ch| 'ABZ' includes: ch])! !

!ReMatch class methodsFor: 'deprecated' stamp: 'acg 2/28/1999 04:38'!
on: srchString search: subjString opt: optString

	|re|
	re _ RePattern 
			on: srchString 
			opt: (optString select: [:ch| 'imsxAEUX' includes: ch]).
	^ re 
		search: subjString 
		opt: (optString select: [:ch| 'ABZ' includes: ch])! !


!RePattern methodsFor: 'documentation' stamp: 'acg 8/3/2002 23:21'!
aGeneralComment "

Perl-Style Regular Expressions in Smalltalk
by Andrew C. Greenberg

Use of RePattern directly is deprecated.  For versions 3.2 and upward, class Re serves as the primary interface.  RePattern will likely be deleted or supplanted in future versions."!
]style[(19 44 24 77 2 102)f1b,bf3,bf2,f1,f1LRe Comment;,f1! !

!RePattern methodsFor: 'accessing' stamp: 'acg 2/28/1999 00:42'!
pattern
	"Answer the pattern I am setup to match."

	^ pattern! !

!RePattern methodsFor: 'searching' stamp: 'acg 2/27/1999 02:53'!
search: aString
	"Answer nil if I don't match aString using standard options.  Otherwise return an appropriate ReMatch."

	^ self search: aString opt: ''! !

!RePattern methodsFor: 'searching' stamp: 'acg 2/26/1999 23:16'!
search: aString from: anInteger
	"Answer nil if I don't match the substring of aString beginning at anInteger using standard options.  Otherwise return an appropriate ReMatch."

	^ self
		search: aString
		from: anInteger
		to: (aString size)
		opt: ''! !

!RePattern methodsFor: 'searching' stamp: 'acg 2/26/1999 23:18'!
search: srchString from: anInteger opt: optString
	"Answer nil if I don't match the substring of srchString beginning at anInteger using options specified by optString.  Otherwise return an appropriate ReMatch."

	^ self
		search: srchString
		from: anInteger
		to: (srchString size)
		opt: optString! !

!RePattern methodsFor: 'searching' stamp: 'acg 2/26/1999 23:16'!
search: aString from: posInteger to: endposInteger
	"Answer nil if I don't match the substring of aString beginning at posInteger and ending at endposInteger using standard options.  Otherwise return an appropriate ReMatch."

	^self 
		search: aString
		from: posInteger
		to: endposInteger
		opt: ''! !

!RePattern methodsFor: 'searching' stamp: 'acg 2/28/1999 02:02'!
search: aString from: posInteger to: endposInteger opt: matchOptString
	"Answer nil if I don't match the substring of srchString beginning at posInteger and ending at endposInteger using options specified by optString.  Otherwise return an appropriate ReMatch."

	matchOptions _ self evalMatchString: matchOptString.
	lastMatchResult _ self 
		primPCREExec: aString from: posInteger to: endposInteger.
	(lastMatchResult < 0) ifTrue: [^nil].
	^ReMatch 
		matchArray: matchSpace
		forRe: self 
		onString: aString
		from: posInteger
		to: endposInteger
! !

!RePattern methodsFor: 'searching' stamp: 'acg 2/26/1999 23:17'!
search: srchString opt: optString
	"Answer nil if I don't match srchString using options specified by optString.  Otherwise return an appropriate ReMatch."

	matchOptions _ self evalMatchString: optString.
	lastMatchResult _ self 
		primPCREExec: srchString.
	(lastMatchResult < 0) ifTrue: [^nil].
	^ReMatch 
		matchArray: matchSpace
		forRe: self 
		onString: srchString
		from: 1
		to: (srchString size)! !

!RePattern methodsFor: 'global searching' stamp: 'acg 3/7/1999 16:26'!
search: aString collect: aBlock

	^self search: aString opt: '' collect: aBlock
! !

!RePattern methodsFor: 'global searching' stamp: 'acg 3/7/1999 16:27'!
search: aString collect: aBlock num: limitInteger

	^self search: aString opt: '' collect: aBlock num: limitInteger! !

!RePattern methodsFor: 'global searching' stamp: 'acg 3/7/1999 16:25'!
search: aString opt: optString collect: aBlock
"Sequentially search aString until no more matches are found.  Begin a subsequent search immediately where the preceding search ends, but handle empty matches specially as described below.  Answer an OrderedCollection of the result of applying aBlock to each match, or nil if no matches were found.

For example:

	(RePattern on: 'x+')
		gsearch: 'x xx xxx xxxxx xxxxxx'
		opt: '' 
		collect: [:m | m match] 
Answers: 	

	OrderedCollection ('x' 'xx' 'xxx' 'xxxxx' 'xxxxxx' )

After an empty string is matched, a subsequent search would attain the same result.  Accordingly, we simply bump the search one character in such cases.  Additionally, we do not count as a match empty matches which are adjacent to a preceding match." 

	| from to m results lastMatchFrom matchesSoFar |
	aString ifNil: [^nil].
	from _ 1. to _ aString size. lastMatchFrom _ -1. matchesSoFar _ 0.
	results  _ OrderedCollection new.

	[from <= (to+1)]
		whileTrue: [
			(m _ self search: aString from: from to: to opt: optString)
				ifNil: [
					(0 = results size) ifTrue: [^nil].
					^results]
				ifNotNil: [
					matchesSoFar _ matchesSoFar + 1.
					from _ (m to) + 1.
					(0 = m match size)
						ifTrue: [ "Handle an empty match"
							from _ from + 1.
							(lastMatchFrom = m from) 
								ifFalse: [results add:(aBlock value: m)]]
						ifFalse: [ "Handle a non-empty match"
							lastMatchFrom _ from. 
							results add:(aBlock value: m)]]].
	^results! !

!RePattern methodsFor: 'global searching' stamp: 'acg 3/7/1999 16:25'!
search: aString opt: optString collect: aBlock num: limitInteger
"Sequentially search aString until no more matches are found, or limitInteger matches are found, whichever occurs first.  Begin a subsequent searche immediately where the preceding search ended, but handle empty specially as described below.  Answer an OrderedCollection of the result of applying aBlock to each match, or nil if no matches were found.

For example:

	(RePattern on: 'x+')
		gsearch: 'x  xx xxx xxxxx xxxxxx'
		opt: '' 
		collect: [:m | m match] 
		num: 3 

Answers: 	

	OrderedCollection ('x' 'xx' 'xxx' )

After an empty string is matched, a subsequent search would attain the same result.  Accordingly, we simply bump the search one character in such cases.  Additionally, we do not count as a match empty matches which are adjacent to a preceding match." 

	| from to m results lastMatchFrom matchesSoFar |
	aString ifNil: [^nil].
	((limitInteger isNil) or: [limitInteger < 0]) ifTrue: [^self gsearch: aString opt: optString].
	from _ 1. to _ aString size. lastMatchFrom _ -1. matchesSoFar _ 0.
	results  _ OrderedCollection new.

	[(from <= (to+1)) and: [matchesSoFar < limitInteger]]
		whileTrue: [
			(m _ self search: aString from: from to: to opt: optString)
				ifNil: [
					(0 = results size) ifTrue: [^nil].
					^results]
				ifNotNil: [
					from _ (m to) + 1.
					(0 = m match size)
						ifTrue: [ "Handle an empty match"
							from _ from + 1.
							(lastMatchFrom = m from) 
								ifFalse: [ "empty match not adjacent preceding match"
									matchesSoFar _ matchesSoFar + 1.
									results add:(aBlock value: m)]]
						ifFalse: [ "Handle a non-empty match"
							matchesSoFar _ matchesSoFar + 1.
							lastMatchFrom _ from. 
							results add:(aBlock value: m)]]].

	(0 = results size) ifTrue: [^nil].
	^results! !

!RePattern methodsFor: 'global searching' stamp: 'acg 3/14/1999 21:18'!
search: aString opt: optString sub: aBlock

	|start m result |
	start _ 1.
	m _ self search: aString from: start opt: optString.
	result _ WriteStream on: (String new).
	[m isNil]
		whileFalse: [
			((m from) > start) 
				ifTrue: [ result nextPutAll: (aString copyFrom: start to: ((m from) - 1))].
			0 = m match size
				ifTrue: [
					start > aString size ifTrue: [
						result nextPutAll: (aBlock value: m). 
						^result contents].
					result nextPutAll: (aBlock value: m); nextPut: (aString at: start).
					start _ start + 1]
				ifFalse: [
					result nextPutAll: (aBlock value: m).
					start _ (m to) + 1].
			m _ self search: aString from: start opt: optString].
	(start <= (aString size))
		ifTrue: [result nextPutAll: (aString copyFrom: start to: (aString size))].
	^(result contents)
			! !

!RePattern methodsFor: 'global searching' stamp: 'acg 3/14/1999 21:51'!
search: aString opt: optString  sub: aBlock num: limitInteger

	|start m result numMatchesSoFar |
	start _ 1. numMatchesSoFar _ 0.
	((limitInteger isNil) or: [limitInteger < 0]) ifTrue: [^self gsearch: aString opt: optString].
	m _ self search: aString from: start opt: optString.
	result _ WriteStream on: (String new).
	[(m isNil) or: [numMatchesSoFar >= limitInteger]]
		whileFalse: [
			numMatchesSoFar _ numMatchesSoFar + 1.
			((m from) > start) 
				ifTrue: [ result _ result , (aString copyFrom: start to: ((m from) - 1))].
			0 = m match size
				ifTrue: [
					start > aString size ifTrue: [
						result nextPutAll: (aBlock value: m). 
						^result contents].
					result nextPutAll: (aBlock value: m); nextPut: (aString at: start).
					start _ start + 1]
				ifFalse: [
					result nextPutAll: (aBlock value: m).
					start _ (m to) + 1].
			m _ self search: aString from: start opt: optString].
	(start <= (aString size))
		ifTrue: [result nextPutAll: (aString copyFrom: start to: (aString size))].
	^(result contents)! !

!RePattern methodsFor: 'global searching' stamp: 'acg 3/7/1999 16:28'!
search: aString sub: aBlock

	^self search: aString opt: '' sub: aBlock! !

!RePattern methodsFor: 'global searching' stamp: 'acg 3/7/1999 16:28'!
search: aString sub: aBlock num: limitInteger

	^self search: aString opt: '' sub: aBlock num: limitInteger! !

!RePattern methodsFor: 'methods for RE' stamp: 'acg 7/30/2002 11:51'!
search: aString from: posInteger to: endposInteger optCode: matchOptionsInteger

	"Answer nil if I don't match the substring of srchString beginning at posInteger and ending at endposInteger using options specified by optString.  Otherwise return an appropriate ReMatch."

	matchOptions _ matchOptionsInteger.
	lastMatchResult _ self 
		primPCREExec: aString from: posInteger to: endposInteger.
	(lastMatchResult < 0) ifTrue: [^nil].
	^ReMatch 
		matchArray: matchSpace
		forRe: self 
		onString: aString
		from: posInteger
		to: endposInteger
! !

!RePattern methodsFor: 'methods for RE' stamp: 'acg 7/30/2002 17:31'!
search: aString optCode: anInteger collect: aBlock
"Sequentially search aString until no more matches are found.  Begin a subsequent search immediately where the preceding search ends, but handle empty matches specially as described below.  Answer an OrderedCollection of the result of applying aBlock to each match, or nil if no matches were found.

For example:

	(RePattern on: 'x+')
		gsearch: 'x xx xxx xxxxx xxxxxx'
		opt: '' 
		collect: [:m | m match] 
Answers: 	

	OrderedCollection ('x' 'xx' 'xxx' 'xxxxx' 'xxxxxx' )

After an empty string is matched, a subsequent search would attain the same result.  Accordingly, we simply bump the search one character in such cases.  Additionally, we do not count as a match empty matches which are adjacent to a preceding match." 

	| from to m results lastMatchFrom matchesSoFar |
	aString ifNil: [^nil].
	from _ 1. to _ aString size. lastMatchFrom _ -1. matchesSoFar _ 0.
	results  _ OrderedCollection new.

	[from <= (to+1)]
		whileTrue: [
			(m _ self search: aString from: from to: to optCode: anInteger)
				ifNil: [
					(0 = results size) ifTrue: [^nil].
					^results]
				ifNotNil: [
					matchesSoFar _ matchesSoFar + 1.
					from _ (m to) + 1.
					(0 = m match size)
						ifTrue: [ "Handle an empty match"
							from _ from + 1.
							(lastMatchFrom = m from) 
								ifFalse: [results add:(aBlock value: m)]]
						ifFalse: [ "Handle a non-empty match"
							lastMatchFrom _ from. 
							results add:(aBlock value: m)]]].
	^results! !

!RePattern methodsFor: 'methods for RE' stamp: 'acg 7/30/2002 17:33'!
search: aString optCode: optInteger collect: aBlock num: limitInteger
"Sequentially search aString until no more matches are found, or limitInteger matches are found, whichever occurs first.  Begin a subsequent searche immediately where the preceding search ended, but handle empty specially as described below.  Answer an OrderedCollection of the result of applying aBlock to each match, or nil if no matches were found.

For example:

	(RePattern on: 'x+')
		gsearch: 'x  xx xxx xxxxx xxxxxx'
		opt: '' 
		collect: [:m | m match] 
		num: 3 

Answers: 	

	OrderedCollection ('x' 'xx' 'xxx' )

After an empty string is matched, a subsequent search would attain the same result.  Accordingly, we simply bump the search one character in such cases.  Additionally, we do not count as a match empty matches which are adjacent to a preceding match." 

	| from to m results lastMatchFrom matchesSoFar |
	aString ifNil: [^nil].
	((limitInteger isNil) or: [limitInteger < 0]) 
		ifTrue: [^self search: aString optCode: optInteger collect: aBlock].
	from _ 1. to _ aString size. lastMatchFrom _ -1. matchesSoFar _ 0.
	results  _ OrderedCollection new.

	[(from <= (to+1)) and: [matchesSoFar < limitInteger]]
		whileTrue: [
			(m _ self search: aString from: from to: to optCode: optInteger)
				ifNil: [
					(0 = results size) ifTrue: [^nil].
					^results]
				ifNotNil: [
					from _ (m to) + 1.
					(0 = m match size)
						ifTrue: [ "Handle an empty match"
							from _ from + 1.
							(lastMatchFrom = m from) 
								ifFalse: [ "empty match not adjacent preceding match"
									matchesSoFar _ matchesSoFar + 1.
									results add:(aBlock value: m)]]
						ifFalse: [ "Handle a non-empty match"
							matchesSoFar _ matchesSoFar + 1.
							lastMatchFrom _ from. 
							results add:(aBlock value: m)]]].

	(0 = results size) ifTrue: [^nil].
	^results! !

!RePattern methodsFor: 'methods for RE' stamp: 'acg 7/30/2002 21:01'!
search: aString optCode: anInteger sub: aBlock

	|start m result |
	start _ 1.
	m _ self search: aString from: 1 to: aString size optCode: anInteger.
	result _ WriteStream on: (String new).
	[m isNil]
		whileFalse: [
			((m from) > start) 
				ifTrue: [ result nextPutAll: (aString copyFrom: start to: ((m from) - 1))].
			0 = m match size
				ifTrue: [
					start > aString size ifTrue: [
						result nextPutAll: (aBlock value: m). 
						^result contents].
					result nextPutAll: (aBlock value: m); nextPut: (aString at: start).
					start _ start + 1]
				ifFalse: [
					result nextPutAll: (aBlock value: m).
					start _ (m to) + 1].
			m _ self search: aString from: start to: aString size optCode: anInteger].
	(start <= (aString size))
		ifTrue: [result nextPutAll: (aString copyFrom: start to: (aString size))].
	^(result contents)
			! !

!RePattern methodsFor: 'methods for RE' stamp: 'acg 7/30/2002 21:19'!
search: aString optCode: anInteger  sub: aBlock num: limitInteger

	|start m result numMatchesSoFar |
	start _ 1. numMatchesSoFar _ 0.
	((limitInteger isNil) or: [limitInteger < 0]) ifTrue: 
		[^self search: aString optCode: anInteger  sub: aBlock].
	m _ self search: aString from: start to: aString size optCode: anInteger.
	result _ WriteStream on: (String new).
	[(m isNil) or: [numMatchesSoFar >= limitInteger]]
		whileFalse: [
			numMatchesSoFar _ numMatchesSoFar + 1.
			((m from) > start) 
				ifTrue: [ result nextPutAll: (aString copyFrom: start to: ((m from) - 1))].
			0 = m match size
				ifTrue: [
					start > aString size ifTrue: [
						result nextPutAll: (aBlock value: m). 
						^result contents].
					result nextPutAll: (aBlock value: m); nextPut: (aString at: start).
					start _ start + 1]
				ifFalse: [
					result nextPutAll: (aBlock value: m).
					start _ (m to) + 1].
			m _ self search: aString from: start to: aString size optCode: anInteger].
	(start <= (aString size))
		ifTrue: [result nextPutAll: (aString copyFrom: start to: (aString size))].
	^(result contents)! !

!RePattern methodsFor: 'constants' stamp: 'acg 2/24/1999 01:00'!
PCREANCHORED

	^16! !

!RePattern methodsFor: 'constants' stamp: 'acg 2/24/1999 00:59'!
PCRECASELESS

	^1

! !

!RePattern methodsFor: 'constants' stamp: 'acg 2/24/1999 01:00'!
PCREDOLLARENDONLY

	^32
! !

!RePattern methodsFor: 'constants' stamp: 'acg 2/24/1999 00:59'!
PCREDOTALL

	^4! !

!RePattern methodsFor: 'constants' stamp: 'acg 2/24/1999 00:59'!
PCREEXTENDED

	^8! !

!RePattern methodsFor: 'constants' stamp: 'acg 2/28/1999 04:35'!
PCREEXTRA

	^64
! !

!RePattern methodsFor: 'constants' stamp: 'acg 2/24/1999 00:59'!
PCREMULTILINE

	^2! !

!RePattern methodsFor: 'constants' stamp: 'acg 2/28/1999 04:36'!
PCRENOTBOL

	^128! !

!RePattern methodsFor: 'constants' stamp: 'acg 2/28/1999 04:36'!
PCRENOTEOL

	^256! !

!RePattern methodsFor: 'constants' stamp: 'acg 2/28/1999 04:36'!
PCREUNGREEDY

	^512! !

!RePattern methodsFor: 'primitives' stamp: 'acg 8/11/2002 23:42'!
primGetModuleName

	<primitive: 'getModuleName' module: 'rePlugin'>
	^nil! !

!RePattern methodsFor: 'primitives' stamp: 'acg 2/24/1999 23:48'!
primPCRECompile

	<primitive: 'primPCRECompile' module: 'rePlugin'>
	^RePlugin doPrimitive: 'primPCRECompile'! !

!RePattern methodsFor: 'primitives' stamp: 'acg 2/24/1999 23:10'!
primPCREExec: aCharBufferObject

	<primitive: 'primPCREExec' module: 'rePlugin'>
	^RePlugin doPrimitive: 'primPCREExec'
! !

!RePattern methodsFor: 'primitives' stamp: 'acg 2/26/1999 20:58'!
primPCREExec: aCharBufferObject from: fromInteger to: toInteger

	<primitive: 'primPCREExecfromto' module: 'rePlugin'>
	^RePlugin doPrimitive: 'primPCREExecfromto'
! !

!RePattern methodsFor: 'primitives' stamp: 'acg 2/25/1999 00:05'!
primPCRENumSubPatterns

	<primitive: 'primPCRENumSubPatterns' module: 'rePlugin'>
	^RePlugin doPrimitive: 'primPCRENumSubPatterns'
! !

!RePattern methodsFor: 'private' stamp: 'acg 2/28/1999 01:41'!
compile: aString
	"Initialize me so I can match aString using standard options.  If the compile fails, display an appropriate notifier and answer nil.  Otherwise answer self."

	^self compile: aString opt: ''.! !

!RePattern methodsFor: 'private' stamp: 'acg 2/28/1999 01:40'!
compile: aString opt: optString
	"Initialize me so I can match aString using options specified in optString.  If the compile fails, display an appropriate notifier and answer nil.  Otherwise answer self."

	^self 
		compile: aString 
		opt: optString
		onErrorRun: [:pat :off :err | self error: err. ^nil].! !

!RePattern methodsFor: 'private' stamp: 'acg 7/28/2002 23:27'!
compile: aString opt: optString onErrorRun: aBlock
	"Initialize me so I can match aString using options specified in optString.  If the compile of aString fails, send aBlock the following message:

	aBlock value: aString value: anInteger value: anErrorString

where, anInteger is the offset in aString where the error was identified, and anErrorString is a descriptive error message.  Answer nil if compiles fails, otherwise self."

	pattern _ (aString, (Character characterTable at: 1) asString). "Must be Zero-Terminated"
	compileOptions _ self evalCompileString: optString onErrorRun: aBlock.
	pcrePointer _ extraPointer _ errorString _ offset _ nil.
	matchOptions _ 0.
	(self primPCRECompile)
		ifNotNil: [aBlock value: pattern value: offset value: errorString].
	matchSpace _ IntegerArray new: 3 * ( 1 + (self primPCRENumSubPatterns)).
	^ self.! !

!RePattern methodsFor: 'private' stamp: 'acg 7/30/2002 11:52'!
compile: aString optCode: anInteger onErrorRun: aBlock
	"Initialize me so I can match aString using options specified in optString.  If the compile of aString fails, send aBlock the following message:

	aBlock value: aString value: anInteger value: anErrorString

where, anInteger is the offset in aString where the error was identified, and anErrorString is a descriptive error message.  Answer nil if compiles fails, otherwise self."

	pattern _ (aString, (Character characterTable at: 1) asString). "Must be Zero-Terminated"
	compileOptions _ anInteger.
	pcrePointer _ extraPointer _ errorString _ offset _ nil.
	matchOptions _ 0.
	(self primPCRECompile)
		ifNotNil: [aBlock value: pattern value: offset value: errorString].
	matchSpace _ IntegerArray new: 3 * ( 1 + (self primPCRENumSubPatterns)).
	^ self.! !

!RePattern methodsFor: 'private' stamp: 'acg 3/12/1999 22:18'!
evalCompileString: aString onErrorRun: aBlock
	"Answer an integer recognized by PCRE, and representing the compile time codes indicated in aString.  For details about the codes, see the comments for the corresponding constant function.  If aString contains unrecognized options, sent a message to aBlock"

	|result|
	result _ 0.
	aString doWithIndex: [:ch :index|
		"Traditional Perl Options"
		(ch == $i) ifTrue: [result _ result bitOr: (self PCRECASELESS)].
		(ch == $m) ifTrue: [result _ result bitOr: (self PCREMULTILINE)].
		(ch == $s) ifTrue: [result _ result bitOr: (self PCREDOTALL)].
		(ch == $x) ifTrue: [result _ result bitOr: (self PCREEXTENDED)].
		"PCRE Extensions"
		(ch == $A) ifTrue: [result _ result bitOr: (self PCREANCHORED)].
		(ch == $E) ifTrue: [result _ result bitOr: (self PCREDOLLARENDONLY)].
		(ch == $U) ifTrue: [result _ result bitOr: (self PCREUNGREEDY)].
		(ch == $X) ifTrue: [result _ result bitOr: (self PCREEXTRA)].
		('imsxABEUXZ' includes: ch) 
			ifFalse: [
				aBlock 
					value: aString 
					value: index 
					value: ('Invalid RE Compile Option: ', ch asString)]].
	^result! !

!RePattern methodsFor: 'private' stamp: 'acg 2/28/1999 00:56'!
evalMatchString: aString
	"Answer an integer recognized by PCRE, and representing the match time codes indicated in aString.  For details about the codes, see the comments for the corresponding constant function.  If aString contains unrecognized options, display a notifier"

	^self 
		evalMatchString: aString
		onErrorRun: [:source :index :message | self error: message]! !

!RePattern methodsFor: 'private' stamp: 'acg 3/12/1999 22:18'!
evalMatchString: aString onErrorRun: aBlock
	"Answer an integer recognized by PCRE, and representing the match time codes indicated in aString.  For details about the codes, see the comments for the corresponding constant function.  If aString contains unrecognized options, sent a message to aBlock"

	|result|
	result _ 0.
	aString doWithIndex: [:ch :index |
		(ch == $A) ifTrue: [result _ result bitOr: (self PCREANCHORED)].
		(ch == $B) ifTrue: [result _ result bitOr: (self PCRENOTBOL)].
		(ch == $Z) ifTrue: [result _ result bitOr: (self PCRENOTEOL)].
		('imsxABEUXZ' includes: ch) 
			ifFalse: [
				aBlock 
					value: aString 
					value: index 
					value: ('Invalid RE Match Option: ', ch asString)]].
	^result! !

!RePattern methodsFor: 'private' stamp: 'acg 2/27/1999 22:24'!
memoryState
	"Answer a report of the malloc:/free: engine (for tracking memory leaks)"

	^String cr, 
		((self primNumAllocs) asString), ' Allocations', String cr,
		((self primNumFrees) asString), ' Frees', String cr,
		((self primNetMemory) asString), ' Net Memory Taken', String cr! !

!RePattern methodsFor: 'private' stamp: 'acg 2/25/1999 08:37'!
primLastAlloc

	<primitive: 'primLastAlloc' module: 'rePlugin'>
	^RePlugin doPrimitive: 'primLastAlloc'
! !

!RePattern methodsFor: 'private' stamp: 'acg 2/21/1999 23:15'!
primNetMemory

	<primitive: 'primNetMemory' module: 'rePlugin'>
	^RePlugin doPrimitive: 'primNetMemory'
! !

!RePattern methodsFor: 'private' stamp: 'acg 2/21/1999 23:15'!
primNumAllocs

	<primitive: 'primNumAllocs' module: 'rePlugin'>
	^RePlugin doPrimitive: 'primNumAllocs'
! !

!RePattern methodsFor: 'private' stamp: 'acg 2/21/1999 23:15'!
primNumFrees

	<primitive: 'primNumFrees' module: 'rePlugin'>
	^RePlugin doPrimitive: 'primNumFrees'
! !

!RePattern methodsFor: 'printing' stamp: 'acg 3/7/1999 08:04'!
printOn: aStream

	aStream nextPutAll: 'a '.
	(self species) printOn: aStream.
	aStream nextPut: $(.
	(self pattern) printOn: aStream.
	aStream nextPut: $).! !

!RePattern methodsFor: 'deprecated' stamp: 'acg 3/2/1999 07:58'!
gsearch: aString

	^self gsearch: aString opt: ''.! !

!RePattern methodsFor: 'deprecated' stamp: 'acg 3/1/1999 08:33'!
gsearch: aString collect: aBlock

	| collection |
	collection _ self gsearch: aString.
	^collection
		ifNil: [nil]
		ifNotNil: [collection collect: aBlock]
			! !

!RePattern methodsFor: 'deprecated' stamp: 'acg 3/2/1999 09:47'!
gsearch: aString opt: optString
"Answer Collection of MatchObjects resulting sequential searches of aString for matches to me, or nil if there are no matches.  Do not include empty matches that are adjacent to a previous match."

	| from to m results lastMatchFrom |
	aString ifNil: [^nil].
	from _ 1. to _ aString size. lastMatchFrom _ -1.
	results  _ OrderedCollection new.

	[from <= (to+1)]
		whileTrue: [
			(m _ self search: aString from: from to: to opt: optString)
				ifNil: [
					(0 = results size) ifTrue: [^nil].
					^results]
				ifNotNil: [
					from _ (m to) + 1.
					(0 = m match size) 
						ifTrue: [
							from _ from + 1.
							(lastMatchFrom = m from) ifFalse: [results add:m]]
						ifFalse: [lastMatchFrom _ from. results add:m]]].
	^results! !

!RePattern methodsFor: 'deprecated' stamp: 'acg 3/7/1999 15:41'!
gsearch: aString opt: optString num: limitInteger
"Answer Collection of MatchObjects resulting sequential searches of aString for up to limitInteger matches to me, or nil if there are no matches at all.  Do not include empty matches that are adjacent to a previous match."

	| from to m results lastMatchFrom matchesSoFar |
	aString ifNil: [^nil].
	((limitInteger isNil) or: [limitInteger < 0]) ifTrue: [^self gsearch: aString opt: optString].
	from _ 1. to _ aString size. lastMatchFrom _ -1. matchesSoFar _ 0.
	results  _ OrderedCollection new.

	[(from <= (to+1)) and: [matchesSoFar < limitInteger]]
		whileTrue: [
			(m _ self search: aString from: from to: to opt: optString)
				ifNil: [
					(0 = results size) ifTrue: [^nil].
					^results]
				ifNotNil: [
					from _ (m to) + 1.
					(0 = m match size) 
						ifTrue: [
							from _ from + 1.
							(lastMatchFrom = m from) ifFalse: [results add:m]]
						ifFalse: [lastMatchFrom _ from. results add:m]]].
	^results! !

!RePattern methodsFor: 'deprecated' stamp: 'acg 2/28/1999 05:21'!
gsearch: aString sub: aBlock

	^self gsearch: aString sub: aBlock opt: ''! !

!RePattern methodsFor: 'deprecated' stamp: 'acg 3/2/1999 00:55'!
gsearch: aString sub: aBlock opt: optString

	|start m result |
	start _ 1.
	m _ self search: aString from: start opt: optString.
	result _ ''.
	[m isNil]
		whileFalse: [
			((m from) > start) 
				ifTrue: [ result _ result , (aString copyFrom: start to: ((m from) - 1))].
			0 = m match size
				ifTrue: [
					start > aString size ifTrue: [^ result].
					result _ result , (aString at: start) asString.
					start _ start + 1]
				ifFalse: [
					result _ result , (aBlock value: m).
					start _ (m to) + 1].
			m _ self search: aString from: start opt: optString].
	(start <= (aString size))
		ifTrue: [result _ result, (aString copyFrom: start)].
	^result
			! !

!RePattern methodsFor: 'deprecated' stamp: 'acg 3/7/1999 16:21'!
gsearch: aString sub: aBlock opt: optString num: limitInteger

	|start m result |
	start _ 1.
	m _ self search: aString from: start opt: optString.
	result _ ''.
	[m isNil]
		whileFalse: [
			((m from) > start) 
				ifTrue: [ result _ result , (aString copyFrom: start to: ((m from) - 1))].
			0 = m match size
				ifTrue: [
					start > aString size ifTrue: [^ result].
					result _ result , (aString at: start) asString.
					start _ start + 1]
				ifFalse: [
					result _ result , (aBlock value: m).
					start _ (m to) + 1].
			m _ self search: aString from: start opt: optString].
	(start <= (aString size))
		ifTrue: [result _ result, (aString copyFrom: start)].
	^result
			! !

!RePattern methodsFor: 'deprecated' stamp: 'acg 2/27/1999 22:22'!
initialize: aString
	"Initialize me so I can match aString using standard options.  If the compile fails, display an appropriate notifier and answer nil.  Otherwise answer self."

	^self initialize: aString opt: ''.! !

!RePattern methodsFor: 'deprecated' stamp: 'acg 2/27/1999 22:22'!
initialize: aString opt: optString
	"Initialize me so I can match aString using options specified in optString.  If the compile fails, display an appropriate notifier and answer nil.  Otherwise answer self."

	^self 
		initialize: aString 
		opt: optString
		onErrorRun: [:pat :off :err | self error: err. ^nil].! !

!RePattern methodsFor: 'deprecated' stamp: 'acg 2/27/1999 22:21'!
initialize: aString opt: optString onErrorRun: aBlock
	"Initialize me so I can match aString using options specified in optString.  If the compile of aString fails, send aBlock the following message:

	aBlock value: aString value: anInteger value: anErrorString

where, anInteger is the offset in aString where the error was identified, and anErrorString is a descriptive error message.  Answer nil if compiles fails, otherwise self."

	pattern _ (aString, (Character characterTable at: 1) asString). "Must be Zero-Terminated"
	compileOptions _ self evalCompileString: optString onErrorRun: aBlock.
	pcrePointer _ extraPointer _ errorString _ offset _ nil.
	matchOptions _ 0.
	(self primPCRECompile)
		ifNotNil: [aBlock value: pattern value: offset value: errorString].
	matchSpace _ IntegerArray new: 3 * ( 1 + (self primPCRENumSubPatterns)).
	^ self.! !

!RePattern methodsFor: 'deprecated' stamp: 'acg 2/27/1999 00:34'!
initializePattern: aPatString

	^self
		initializePattern: aPatString 
		onErrorRun:  [:pat :off :err | self error: err. ^nil].! !

!RePattern methodsFor: 'deprecated' stamp: 'acg 7/21/2002 17:26'!
initializePattern: aPatString onErrorRun: aBlock

	|pstr delimiter patStream optionString optionCode |
	pstr _ ReadStream on: aPatString.

	"Get and check the delimiter"
	delimiter _ pstr next.
	('!!@#$%^&*()_+-=[]{}|;:''"/?,.<>`~' includes: delimiter)
		ifFalse: [
			self error: (
				'rePattern: improper delimiter ',
				(delimiter asString),
				' in pattern ',
				aPatString)].

	"Get the pattern string, permit delimiter to be escaped with $\"
	patStream _ ReadWriteStream on: (String new: (aPatString size)).
	patStream nextPutAll: (pstr upTo: delimiter).
	[$\ = (pstr last)]
		whileTrue: [
			patStream nextPut: delimiter. 
			patStream nextPutAll:(pstr upTo: delimiter)].

	"Get and verify the option string"
	optionString _ pstr upToEnd.
	optionCode _ 0.
	optionString do: [:ch | 
		(ch = 'i') ifTrue: [optionCode _ optionCode bitOr: (self PCRECASELESS)].
		(ch = 'm') ifTrue: [optionCode _ optionCode bitOr: (self PCREMULTILINE)].
		(ch = 's') ifTrue: [optionCode _ optionCode bitOr: (self PCREDOTALL)].
		(ch = 'x') ifTrue: [optionCode _ optionCode bitOr: (self PCREEXTENDED)].
		('imsx' includes: ch)
			ifFalse: [
				self error: (
					'rePattern: improper option ', 
					(ch asString) ,
					' in pattern ',
					optionString)].].

	^self initialize: patStream contents opt: optionCode onErrorRun: aBlock.! !


!RePattern class methodsFor: 'instance creation' stamp: 'acg 3/7/1999 08:00'!
on: aString
	"Answer an instance of a compiled re for matching aString under Standard options.  If compile fails, display an appropriate notifier."

	^self 
		search: aString 
		opt: ''
		ifAbsentAdd:[self new compile: aString].! !

!RePattern class methodsFor: 'instance creation' stamp: 'acg 3/6/1999 11:43'!
on: aString opt: optString
	"Answer an instance of a compiled re for matching aString under the options specified by optString.  If compile fails, display an appropriate notifier."

	^ self 
		search: aString 
		opt: optString
		ifAbsentAdd: [self new compile: aString opt: optString]! !

!RePattern class methodsFor: 'instance creation' stamp: 'acg 3/7/1999 08:00'!
on: aString opt: optString onErrorRun: aBlock
"Answer an instance of a compiled re for matching aString under the options specified by optString.  If compile fails, send aBlock the following message:

	aBlock value: aString value: anInteger value: anErrorString

where, anInteger is the offset in aString where the error was identified, and anErrorString is a descriptive error message.
"

	^self 
		search: aString 
		opt: optString
		ifAbsentAdd:[self new 
			compile: aString
			opt: optString 
			onErrorRun: aBlock]! !

!RePattern class methodsFor: 'pattern caching' stamp: 'acg 3/6/1999 11:52'!
debugReport

	^ String cr,
		'Front: ', (Front asString), String cr,
		'Patterns: ', (Patterns asString), String cr,
		'Options: ', (Options asString), String cr,
		'CompileObjects: ', (CompileObjects asString), String cr.! !

!RePattern class methodsFor: 'pattern caching' stamp: 'acg 8/11/2002 14:36'!
initialize

	self initializeCache
	! !

!RePattern class methodsFor: 'pattern caching' stamp: 'acg 3/6/1999 21:18'!
initializeCache

	|size |
	size _ self queueSize.
	Patterns _ Array new: size.
	Options _ Array new: size.
	CompileObjects _ Array new: size.
	Front _ size.
	! !

!RePattern class methodsFor: 'pattern caching' stamp: 'acg 3/6/1999 16:52'!
primCacheSearch: pStr opt: oStr

	<primitive: 'primCacheSearch' module: 'rePlugin'>
	^RePlugin doPrimitive: 'primCacheSearch'
! !

!RePattern class methodsFor: 'pattern caching' stamp: 'acg 3/6/1999 12:11'!
queueSize

	^10! !

!RePattern class methodsFor: 'pattern caching' stamp: 'acg 3/7/1999 08:03'!
search: pStr opt: oStr ifAbsentAdd: aBlock

	|result |
	pStr ifNil: [^nil].
	Patterns withIndexDo: [:p :i | 
		((p = pStr) and: [(Options at: i) = oStr])
				ifTrue:[^CompileObjects at: i]].
	result _ aBlock value.
	(Front == (self queueSize))
		ifTrue: [Front _ 1]
		ifFalse: [Front _ Front + 1].
	Patterns at: Front put: pStr.
	Options at: Front put: oStr.
	CompileObjects at: Front put: result.
	^result! !


!RePlugin methodsFor: 're primitives' stamp: 'acg 3/12/1999 23:36'!
primPCRECompile

"<rcvr primPCRECompile>, where rcvr is an object with instance variables:

	'patternStr compileFlags pcrePtr extraPtr errorStr errorOffset matchFlags'	

Compile the regular expression in patternStr, and if the compilation is successful, attempt to optimize the compiled expression.  Store the results in <pcrePtr> and <extratr>, or fill errorStr with a meaningful errorString and errorOffset with an indicator where the error was found, applying compileFlags throughout.  Answer nil with a clean compile (regardless of whether an optimization is possible, and answer with the string otherwise."


	self export: true.
	self loadRcvrFromStackAt: 0.
	patternStrPtr _ self rcvrPatternStrPtr.
	compileFlags _ self rcvrCompileFlags.
	interpreterProxy failed ifTrue:[^ nil].

	pcrePtr _ self cCode: '(int) pcre_compile(patternStrPtr, compileFlags, 
					&errorStrBuffer, &errorOffset, NULL)'.
	pcrePtr
		ifTrue: [
			self allocateByteArrayAndSetRcvrPCREPtrFromPCRE: pcrePtr.
			extraPtr _ self cCode: '(int) pcre_study((pcre *)pcrePtr, compileFlags, &errorStrBuffer)'.
			self allocateByteArrayAndSetRcvrExtraPtrFrom: extraPtr.
			self rePluginFree: (self cCoerce: pcrePtr to: 'void *').
			extraPtr ifTrue: [self rePluginFree: (self cCoerce: extraPtr to: 'void *')].
			interpreterProxy failed ifTrue:[^ nil].
			interpreterProxy pop: 1 thenPush: interpreterProxy nilObject]
		ifFalse: [
			errorStr _ self allocateStringAndSetRcvrErrorStrFromCStr: errorStrBuffer.
			self rcvrErrorOffsetFrom: errorOffset.
			interpreterProxy failed ifTrue:[^ nil].
			interpreterProxy pop: 1 thenPush: errorStr].! !

!RePlugin methodsFor: 're primitives' stamp: 'acg 2/27/1999 01:08'!
primPCREExec

"<rcvr primPCREExec: searchObject>, where rcvr is an object with instance variables:

	'patternStr compileFlags pcrePtr extraPtr errorStr errorOffset matchFlags'	

Apply the regular expression (stored in <pcrePtr> and <extratr>, generated from calls to primPCRECompile), to smalltalk String searchObject using <matchOptions>.  If there is no match, answer nil.  Otherwise answer a ByteArray of offsets representing the results of the match."

	| searchObject searchBuffer length  result matchSpacePtr matchSpaceSize |
	self export: true.
	self var:#searchBuffer	declareC: 'char *searchBuffer'.
	self var:#matchSpacePtr	declareC: 'int *matchSpacePtr'.
	self var:#result			declareC: 'int result'.
	
	"Load Parameters"
	searchObject _ interpreterProxy stackObjectValue: 0.	
	searchBuffer _ interpreterProxy arrayValueOf: searchObject.
	length _ interpreterProxy byteSizeOf: searchObject.
	self loadRcvrFromStackAt: 1.
	"Load Instance Variables"
	pcrePtr _ self rcvrPCREBufferPtr.
	extraPtr _ self rcvrExtraPtr.
	matchFlags _ self rcvrMatchFlags.
	matchSpacePtr _ self rcvrMatchSpacePtr.
	matchSpaceSize _ self rcvrMatchSpaceSize.

	interpreterProxy failed ifTrue:[^ nil].
	
	result _ self 
		cCode: 'pcre_exec((pcre *)pcrePtr, (pcre_extra *)extraPtr, 
				searchBuffer, length, matchFlags, matchSpacePtr, matchSpaceSize)'.

	interpreterProxy pop: 2; pushInteger: result.

	"empty call so compiler doesn't bug me about variables not used"
	self touch: searchBuffer; touch: matchSpacePtr; touch: matchSpaceSize; touch: length
! !

!RePlugin methodsFor: 're primitives' stamp: 'acg 2/26/1999 21:01'!
primPCREExecfromto

"<rcvr primPCREExec: searchObject> from: fromInteger to: toInteger>, where rcvr is an object with instance variables:

	'patternStr compileFlags pcrePtr extraPtr errorStr errorOffset matchFlags'	

Apply the regular expression (stored in <pcrePtr> and <extratr>, generated from calls to primPCRECompile), to smalltalk String searchObject using <matchOptions>, beginning at offset <fromInteger> and continuing until offset <toInteger>.  If there is no match, answer nil.  Otherwise answer a ByteArray of offsets representing the results of the match."

	| searchObject searchBuffer length  result matchSpacePtr matchSpaceSize fromInteger toInteger |
	self export: true.
	self var:#searchBuffer	declareC: 'char *searchBuffer'.
	self var:#fromInteger declareC: 'int fromInteger'.
	self var:#toInteger declareC: 'int toInteger'.
	self var:#matchSpacePtr	declareC: 'int *matchSpacePtr'.
	self var:#result			declareC: 'int result'.
	
	"Load Parameters"
	toInteger _ interpreterProxy stackIntegerValue: 0.
	fromInteger _ interpreterProxy stackIntegerValue: 1.
	searchObject _ interpreterProxy stackObjectValue: 2.	
	searchBuffer _ interpreterProxy arrayValueOf: searchObject.
	length _ interpreterProxy byteSizeOf: searchObject.
	self loadRcvrFromStackAt: 3.

	"Validate parameters"
	interpreterProxy success: (1 <= fromInteger).
	interpreterProxy success: (toInteger<=length).
	fromInteger _ fromInteger - 1. "Smalltalk offsets are 1-based"
	interpreterProxy success: (fromInteger<=toInteger).

	"adjust length, searchBuffer"
	length _ toInteger - fromInteger.
	searchBuffer _ searchBuffer + fromInteger.

	"Load Instance Variables"
	pcrePtr _ self rcvrPCREBufferPtr.
	extraPtr _ self rcvrExtraPtr.
	matchFlags _ self rcvrMatchFlags.
	matchSpacePtr _ self rcvrMatchSpacePtr.
	matchSpaceSize _ self rcvrMatchSpaceSize.
	interpreterProxy failed ifTrue:[^ nil].
	
	result _ self 
		cCode: 'pcre_exec((pcre *)pcrePtr, (pcre_extra *)extraPtr, 
				searchBuffer, length, matchFlags, matchSpacePtr, matchSpaceSize)'.
	interpreterProxy pop: 2; pushInteger: result.

	"empty call so compiler doesn't bug me about variables not used"
	self touch: searchBuffer; touch: matchSpacePtr; touch: matchSpaceSize; touch: length
! !

!RePlugin methodsFor: 're primitives' stamp: 'acg 3/12/1999 23:32'!
primPCRENumSubPatterns

"<rcvr primPCRENumSubPatterns>, where rcvr is an object with instance variables:

	'patternStr compileFlags pcrePtr extraPtr errorStr errorOffset matchFlags'	

Return the number of subpatterns captured by the compiled pattern."

	self export: true.
	
	"Load Parameters"
	self loadRcvrFromStackAt: 0.
	"Load Instance Variables"
	pcrePtr _ self rcvrPCREBufferPtr.
	interpreterProxy pop: 1; pushInteger: (self cCode: 'pcre_info((pcre *)pcrePtr, NULL, NULL)').
! !

!RePlugin methodsFor: 'memory management' stamp: 'acg 2/25/1999 08:36'!
primLastAlloc
	
	self export: true.
	interpreterProxy pop:1; pushInteger: lastAlloc
! !

!RePlugin methodsFor: 'memory management' stamp: 'acg 2/21/1999 23:20'!
primNetMemory 
	
	self export: true.
	interpreterProxy pop:1; pushInteger: netMemory
! !

!RePlugin methodsFor: 'memory management' stamp: 'acg 2/21/1999 23:20'!
primNumAllocs

	self export: true.
	interpreterProxy pop:1; pushInteger: numAllocs
! !

!RePlugin methodsFor: 'memory management' stamp: 'acg 2/21/1999 23:20'!
primNumFrees 
	
	self export: true.
	interpreterProxy pop:1; pushInteger: numFrees
! !

!RePlugin methodsFor: 'memory management' stamp: 'acg 3/5/1999 09:18'!
rePluginFree: aPointer
	"Free a block of fixed memory allocated with rePluginMalloc.  Instrumented version of C free() to facilitate leak analysis from Smalltalk.   OS-specific variations on malloc/free, such as with MacOS, are handled by adding a C macro to the header file redefining malloc/free -- see the class comment"

	self inline: true.
	self var: #aPointer declareC: 'void * aPointer'.
	self returnTypeC: 'void'.

	numFrees _ numFrees + 1.
	(aPointer)
		ifTrue: [self cCode: 'free(aPointer)']	! !

!RePlugin methodsFor: 'memory management' stamp: 'acg 3/5/1999 09:19'!
rePluginMalloc: anInteger
	"Allocate a block of fixed memory using C calls to malloc().  Instrumented to facilitate leak analysis from Smalltalk.  Set global lastAlloc to anInteger.  OS-specific variations on malloc/free, such as with MacOS, are handled by adding a C macro to the header file redefining malloc/free -- see the class comment"

	| aPointer |
	self inline: true.
	self var: #anInteger declareC: 'size_t anInteger'.
	self var: #aPointer declareC: 'void *aPointer'.
	self returnTypeC: 'void *'.
	numAllocs _ numAllocs + 1.
	(aPointer _ self cCode: 'malloc(anInteger)')
		ifTrue: [lastAlloc _ anInteger].
	^aPointer
! !

!RePlugin methodsFor: 'rcvr linkage' stamp: 'acg 2/27/1999 23:22'!
allocateByteArrayAndSetRcvrExtraPtrFrom: anExtraPtr

	| extraObject extraByteArrayPtr |
	self var: #extraByteArrayPtr declareC: 'void *extraByteArrayPtr'.

	anExtraPtr
		ifFalse: [extraObject _ interpreterProxy nilObject]
		ifTrue: [
			"Allocate a Smalltalk ByteArray -- lastAlloc contains the length"
			extraObject _ interpreterProxy
						instantiateClass: (interpreterProxy classByteArray) 
						indexableSize: (self cCode: 'sizeof(real_pcre_extra)').
			self loadRcvrFromStackAt: 0. "Assume garbage collection after instantiation"

			"Copy from the C bytecode buffer to the Smalltalk ByteArray"
			extraByteArrayPtr _ interpreterProxy arrayValueOf: extraObject.	
			self cCode:'memcpy(extraByteArrayPtr, (void *) anExtraPtr, sizeof(real_pcre_extra))'].
 
	"Set rcvrErrorStr from errorStr and Return"
	self rcvrExtraPtrFrom: extraObject.
	self touch: extraByteArrayPtr.	
	^extraObject.
! !

!RePlugin methodsFor: 'rcvr linkage' stamp: 'acg 2/27/1999 22:57'!
allocateByteArrayAndSetRcvrPCREPtrFromPCRE: aPCREPtr

	| patObject patByteArrayPtr |
	self var: #patByteArrayPtr declareC: 'void *patByteArrayPtr'.

	"Allocate a Smalltalk ByteArray -- lastAlloc contains the length"
	patObject _ interpreterProxy
				instantiateClass: (interpreterProxy classByteArray) 
				indexableSize: lastAlloc.
	self loadRcvrFromStackAt: 0. "Assume garbage collection after instantiation"

	"Copy from the C bytecode buffer to the Smalltalk ByteArray"
	patByteArrayPtr _ interpreterProxy arrayValueOf: patObject.	
	self cCode:'memcpy(patByteArrayPtr, (void *) aPCREPtr, lastAlloc)'.
 
	"Set rcvrErrorStr from errorStr and Return"
	self rcvrPCREBufferFrom: patObject.
	self touch: patByteArrayPtr.	
	^patObject.
! !

!RePlugin methodsFor: 'rcvr linkage' stamp: 'acg 2/28/1999 16:39'!
allocateStringAndSetRcvrErrorStrFromCStr: aCStrBuffer

	|length errorStrObj errorStrObjPtr |
	self var: #aCStrBuffer declareC: 'char *aCStrBuffer'.
	self var: #errorStrObjPtr declareC: 'void *errorStrObjPtr'.
	"Allocate errorStrObj"
	length _ self cCode: 'strlen(aCStrBuffer)'.
	errorStrObj _ interpreterProxy
				instantiateClass: (interpreterProxy classString) 
				indexableSize: length.
	self loadRcvrFromStackAt: 0. "Assume garbage collection after instantiation"

	"Copy aCStrBuffer to errorStrObj's buffer"
	errorStrObjPtr _ interpreterProxy arrayValueOf: errorStrObj.	
	self cCode:'memcpy(errorStrObjPtr,aCStrBuffer,length)'.
	self touch: errorStrObjPtr; touch: errorStrObj.
	"Set rcvrErrorStr from errorStrObj and Return"
	self rcvrErrorStrFrom: errorStrObj.
	^errorStrObj.! !

!RePlugin methodsFor: 'rcvr linkage' stamp: 'acg 2/21/1999 22:58'!
loadRcvrFromStackAt: stackInteger

	self inline:true.
	rcvr _ interpreterProxy stackObjectValue: stackInteger.
! !

!RePlugin methodsFor: 'rcvr linkage' stamp: 'acg 2/21/1999 21:20'!
rcvrCompileFlags

	self inline:true.
	^interpreterProxy fetchInteger: 1 ofObject: rcvr.
! !

!RePlugin methodsFor: 'rcvr linkage' stamp: 'acg 2/21/1999 22:46'!
rcvrErrorOffsetFrom: anInteger

	self inline: true.
	interpreterProxy storeInteger: 5 ofObject: rcvr withValue: anInteger.
! !

!RePlugin methodsFor: 'rcvr linkage' stamp: 'acg 2/24/1999 20:53'!
rcvrErrorStrFrom: aString

	self inline: true.
	interpreterProxy 
		storePointer: 4
		ofObject: rcvr 
		withValue: aString.
! !

!RePlugin methodsFor: 'rcvr linkage' stamp: 'acg 2/27/1999 23:20'!
rcvrExtraPtr

	|extraObj|
	self inline: true.
	extraObj _ interpreterProxy fetchPointer: 3 ofObject: rcvr.
	(extraObj = (interpreterProxy nilObject))
		ifTrue: [^ self cCode: 'NULL'].
	^self 
		cCoerce:(interpreterProxy arrayValueOf: extraObj)
		to: 'int'.! !

!RePlugin methodsFor: 'rcvr linkage' stamp: 'acg 2/27/1999 23:42'!
rcvrExtraPtrFrom: aByteArrayOrNilObject

	self inline: true.
	interpreterProxy 
		storePointer: 3 
		ofObject: rcvr 
		withValue: aByteArrayOrNilObject! !

!RePlugin methodsFor: 'rcvr linkage' stamp: 'acg 2/21/1999 21:19'!
rcvrMatchFlags

	self inline: true.
	^interpreterProxy fetchInteger: 6 ofObject: rcvr.
! !

!RePlugin methodsFor: 'rcvr linkage' stamp: 'acg 2/25/1999 00:49'!
rcvrMatchSpacePtr

	self inline: true.
	self returnTypeC: 'int *'.
	^self
		cCoerce: (interpreterProxy fetchArray: 7 ofObject: rcvr)
		to: 'int *'.! !

!RePlugin methodsFor: 'rcvr linkage' stamp: 'acg 2/25/1999 00:52'!
rcvrMatchSpaceSize

	self inline: true.
	^(interpreterProxy byteSizeOf: (interpreterProxy fetchPointer: 7 ofObject: rcvr))//4.! !

!RePlugin methodsFor: 'rcvr linkage' stamp: 'acg 2/24/1999 21:33'!
rcvrPCREBufferFrom: aByteArray

	self inline: true.
	interpreterProxy 
		storePointer: 2 
		ofObject: rcvr 
		withValue: aByteArray! !

!RePlugin methodsFor: 'rcvr linkage' stamp: 'acg 2/24/1999 21:33'!
rcvrPCREBufferPtr

	self inline: true.
	^self
		cCoerce: (interpreterProxy fetchArray: 2 ofObject: rcvr)
		to: 'int'.! !

!RePlugin methodsFor: 'rcvr linkage' stamp: 'acg 2/24/1999 21:34'!
rcvrPatternStrPtr

	self inline: true.
	self returnTypeC: 'char *'.
	^self 
		cCoerce: (interpreterProxy fetchArray: 0 ofObject: rcvr) 
		to: 'char *'.! !

!RePlugin methodsFor: 'private' stamp: 'acg 3/12/1999 23:32'!
touch: anOop
	"Do nothing but fool the compiler into thinking my parameter was used.  Since I am inlined, I add no overhead whatsoever."

	self inline: true.! !


!RePlugin class methodsFor: 'plugin code generation' stamp: 'acg 7/27/2002 20:56'!
declareCVarsIn: cg

	cg addHeaderFile:'"rePlugin.h"'.

	"Memory Managament Error Checking"
	cg var: 'netMemory' 	declareC: 'int netMemory = 0'.
	cg var: 'numAllocs' 	declareC: 'int numAllocs = 0'.
	cg var: 'numFrees' 		declareC: 'int numFrees = 0'.
	cg var: 'lastAlloc'		declareC: 'int lastAlloc = 0'.

	"The receiver Object Pointer"
	cg var: 'rcvr'			declareC: 'int rcvr'.

	"Instance Variables of Receiver Object"
	cg var: 'patternStr'		declareC: 'int patternStr'.
	cg var: 'compileFlags'	declareC: 'int compileFlags'.
	cg var: 'pcrePtr'		declareC: 'int pcrePtr'.
	cg var: 'extraPtr'		declareC: 'int extraPtr'.
	cg var: 'errorStr'		declareC: 'int errorStr'.
	cg var: 'errorOffset'	declareC: 'int errorOffset'.
	cg var: 'matchFlags'	declareC: 'int matchFlags'.

	"Support Variables for Access to Receiver Instance Variables"
	cg var: 'patternStrPtr' declareC: 'char * patternStrPtr'.
	cg var: 'errorStrBuffer'	declareC: 'char * errorStrBuffer'.! !

!RePlugin class methodsFor: 'plugin code generation' stamp: 'acg 8/16/2002 22:51'!
hasHeaderFile
	"If there is a single intrinsic header file to be associated with the plugin, here is where you want to flag"
	^true! !

!RePlugin class methodsFor: 'plugin code generation' stamp: 'acg 7/27/2002 21:48'!
moduleName

	^'rePlugin'! !

!RePlugin class methodsFor: 'plugin code generation' stamp: 'acg 7/27/2002 20:09'!
requiresCrossPlatformFiles
	"default is ok for most, any plugin needing cross platform files must say so"
	^true! !


!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:36'!
testNewSuite1test1
	| re |
	re _ Re on: 'the quick brown fox'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'the quick brown fox') isNil not.
	self assert: (re search: 'The quick brown FOX') isNil.
	self assert: (re search: 'What do you know about the quick brown fox?') isNil not.
	self assert: (re search: 'What do you know about THE QUICK BROWN FOX?') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test10
	| re re2 |
	re _ Re on: '^(ba|b*){1,2}?bc'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'babc') isNil not.
	self assert: (re search: 'bbabc') isNil not.
	self assert: (re search: 'bababc') isNil not.
	self assert: (re search: 'bababbc') isNil.
	self assert: (re search: 'babababc') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test100
	| re re2 |
	re _ Re on: '[az-]+'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'za-') isNil not.
	self assert: (re search: 'b') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test101
	| re re2 |
	re _ Re on: '[a\-z]+'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a-z') isNil not.
	self assert: (re search: 'b') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test102
	| re re2 |
	re _ Re on: '[a-z]+'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcdxyz') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test103
	| re re2 |
	re _ Re on: '[\d-]+'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '12-34') isNil not.
	self assert: (re search: 'aaa') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test104
	| re re2 |
	re _ Re on: '[\d-z]+'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '12-34z') isNil not.
	self assert: (re search: 'aaa') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:39'!
testNewSuite1test105
	| re |
	re _ Re on: '\x5c'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '\') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test106
	| re re2 |
	re _ Re on: '\x20Z'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'the Zoo') isNil not.
	self assert: (re search: 'Zulu') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test107
	| re re2 |
	re _ Re on: '(abc)\1'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcabc') isNil not.
	self assert: (re search: 'ABCabc') isNil not.
	self assert: (re search: 'abcABC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test108
	| re re2 |
	re _ Re on: 'ab{3cd'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ab{3cd') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test109
	| re re2 |
	re _ Re on: 'ab{3,cd'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ab{3,cd') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test11
	| re re2 |
	re _ Re on: '^\ca\cA\c[\c{\c:'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPut: 16r01 asCharacter;
		nextPut: 16r01 asCharacter;
		nextPut: Character escape;
		nextPutAll: ';z'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test110
	| re re2 |
	re _ Re on: 'ab{3,4a}cd'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ab{3,4a}cd') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test111
	| re re2 |
	re _ Re on: '{4,5a}bc'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '{4,5a}bc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test112
	| re re2 |
	re _ Re on: '^a.b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'a';
		nextPut: Character linefeed;
		nextPutAll: 'b'])) isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'a';
		nextPut: Character cr;
		nextPutAll: 'b'])) isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test113
	| re re2 |
	re _ Re on: 'abc$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: Character cr])) isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: Character cr;
		nextPutAll: 'def'])) isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test114
	| re re2 |
	re _ Re on: '(abc)\123'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: 16r53 asCharacter])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test115
	| re re2 |
	re _ Re on: '(abc)\223'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: 16r93 asCharacter])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test116
	| re re2 |
	re _ Re on: '(abc)\323'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: 16rD3 asCharacter])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test117
	| re re2 |
	re _ Re on: '(abc)\500'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: 16r40 asCharacter])) isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: 8r100 asCharacter])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test118
	| re re2 |
	re _ Re on: '(abc)\5000'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: 16r40 asCharacter;
		nextPutAll: '0'])) isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: 16r40 asCharacter;
		nextPut: 16r30 asCharacter])) isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: 8r100 asCharacter;
		nextPutAll: '0'])) isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: 8r100 asCharacter;
		nextPut: 16r30 asCharacter])) isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: 8r100 asCharacter;
		nextPut: 8r060 asCharacter])) isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: 8r100 asCharacter;
		nextPut: 8r60 asCharacter])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test119
	| re re2 |
	re _ Re on: 'abc\81'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: 8r0 asCharacter;
		nextPutAll: '81'])) isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: 8r0 asCharacter;
		nextPut: 16r38 asCharacter;
		nextPut: 16r31 asCharacter])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test12
	| re re2 |
	re _ Re on: '^[ab\]cde]'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'athing') isNil not.
	self assert: (re search: 'bthing') isNil not.
	self assert: (re search: ']thing') isNil not.
	self assert: (re search: 'cthing') isNil not.
	self assert: (re search: 'dthing') isNil not.
	self assert: (re search: 'ething') isNil not.
	self assert: (re search: 'fthing') isNil.
	self assert: (re search: '[thing') isNil.
	self assert: (re search: '\\thing') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test120
	| re re2 |
	re _ Re on: 'abc\91'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: 8r0 asCharacter;
		nextPutAll: '91'])) isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: 8r0 asCharacter;
		nextPut: 16r39 asCharacter;
		nextPut: 16r31 asCharacter])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test121
	| re re2 |
	re _ Re on: '(a)(b)(c)(d)(e)(f)(g)(h)(i)(j)(k)(l)\12\123'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcdefghijkllS') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test122
	| re re2 |
	re _ Re on: '(a)(b)(c)(d)(e)(f)(g)(h)(i)(j)(k)\12\123'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abcdefghijk';
		nextPut: 8r12 asCharacter;
		nextPutAll: 'S'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test123
	| re re2 |
	re _ Re on: 'ab\gdef'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abgdef') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test124
	| re re2 |
	re _ Re on: 'a{0}bc'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'bc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test125
	| re re2 |
	re _ Re on: '(a|(bc)){0,0}?xyz'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'xyz') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test126
	| re re2 |
	re _ Re on: 'abc[\10]de'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: 8r010 asCharacter;
		nextPutAll: 'de'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test127
	| re re2 |
	re _ Re on: 'abc[\1]de'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: 8r1 asCharacter;
		nextPutAll: 'de'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test128
	| re re2 |
	re _ Re on: '(abc)[\1]de'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: 8r1 asCharacter;
		nextPutAll: 'de'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test129
	| re re2 |
	re _ Re on: 'a.b(?s)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'a';
		nextPut: Character cr;
		nextPutAll: 'b'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test13
	| re re2 |
	re _ Re on: '^[]cde]'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: ']thing') isNil not.
	self assert: (re search: 'cthing') isNil not.
	self assert: (re search: 'dthing') isNil not.
	self assert: (re search: 'ething') isNil not.
	self assert: (re search: 'athing') isNil.
	self assert: (re search: 'fthing') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test130
	| re re2 |
	re _ Re on: '^([^a])([^\b])([^c]*)([^d]{3,4})'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'baNOTccccd') isNil not.
	self assert: (re search: 'baNOTcccd') isNil not.
	self assert: (re search: 'baNOTccd') isNil not.
	self assert: (re search: 'bacccd') isNil not.
	self assert: (re search: 'anything') isNil.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'b';
		nextPut: Character backspace;
		nextPutAll: 'c'])) isNil.
	self assert: (re search: 'baccd') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test131
	| re re2 |
	re _ Re on: '[^a]'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'Abc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test132
	| re re2 |
	re _ Re on: '[^a]'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'Abc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test133
	| re re2 |
	re _ Re on: '[^a]+'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'AAAaAbc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test134
	| re re2 |
	re _ Re on: '[^a]+'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'AAAaAbc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test135
	| re re2 |
	re _ Re on: '[^a]+'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'bbb';
		nextPut: Character cr;
		nextPutAll: 'ccc'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test136
	| re re2 |
	re _ Re on: '[^k]$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
	self assert: (re search: 'abk') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test137
	| re re2 |
	re _ Re on: '[^k]{2,3}$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
	self assert: (re search: 'kbc') isNil not.
	self assert: (re search: 'kabc') isNil not.
	self assert: (re search: 'abk') isNil.
	self assert: (re search: 'akb') isNil.
	self assert: (re search: 'akk') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:40'!
testNewSuite1test138
	| re |
	re _ Re on: '^\d{8,}\@.+[^k]$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: '12345678@a.b.c.d'])) isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: '123456789@x.y.z'])) isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: '12345678@x.y.uk'])) isNil.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: '1234567@a.b.c.d'])) isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test139
	| re re2 |
	re _ Re on: '(a)\1{8,}'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aaaaaaaaa') isNil not.
	self assert: (re search: 'aaaaaaaaaa') isNil not.
	self assert: (re search: 'aaaaaaa') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test14
	| re re2 |
	re _ Re on: '^[^ab\]cde]'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'fthing') isNil not.
	self assert: (re search: '[thing') isNil not.
	self assert: (re search: '\\thing') isNil not.
	self assert: (re search: 'athing') isNil.
	self assert: (re search: 'bthing') isNil.
	self assert: (re search: ']thing') isNil.
	self assert: (re search: 'cthing') isNil.
	self assert: (re search: 'dthing') isNil.
	self assert: (re search: 'ething') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test140
	| re re2 |
	re _ Re on: '[^a]'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aaaabcd') isNil not.
	self assert: (re search: 'aaAabcd') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test141
	| re re2 |
	re _ Re on: '[^a]'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aaaabcd') isNil not.
	self assert: (re search: 'aaAabcd') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test142
	| re re2 |
	re _ Re on: '[^az]'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aaaabcd') isNil not.
	self assert: (re search: 'aaAabcd') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test143
	| re re2 |
	re _ Re on: '[^az]'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aaaabcd') isNil not.
	self assert: (re search: 'aaAabcd') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:41'!
testNewSuite1test144
	| re |
	re _ Re on: '\000\001\002\003\004\005\006\007\010\011\012\013\014\015\016\017\020\021\022\023\024\025\026\027\030\031\032\033\034\035\036\037\040\041\042\043\044\045\046\047\050\051\052\053\054\055\056\057\060\061\062\063\064\065\066\067\070\071\072\073\074\075\076\077\100\101\102\103\104\105\106\107\110\111\112\113\114\115\116\117\120\121\122\123\124\125\126\127\130\131\132\133\134\135\136\137\140\141\142\143\144\145\146\147\150\151\152\153\154\155\156\157\160\161\162\163\164\165\166\167\170\171\172\173\174\175\176\177\200\201\202\203\204\205\206\207\210\211\212\213\214\215\216\217\220\221\222\223\224\225\226\227\230\231\232\233\234\235\236\237\240\241\242\243\244\245\246\247\250\251\252\253\254\255\256\257\260\261\262\263\264\265\266\267\270\271\272\273\274\275\276\277\300\301\302\303\304\305\306\307\310\311\312\313\314\315\316\317\320\321\322\323\324\325\326\327\330\331\332\333\334\335\336\337\340\341\342\343\344\345\346\347\350\351\352\353\354\355\356\357\360\361\362\363\364\365\366\367\370\371\372\373\374\375\376\377'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 
		(String streamContents: [:s | 0 to: 255 do: [:each | s nextPut: each asCharacter]])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test145
	| re re2 |
	re _ Re on: 'P[^*]TAIRE[^*]{1,6}?LL'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'xxxxxxxxxxxPSTAIREISLLxxxxxxxxx') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test146
	| re re2 |
	re _ Re on: 'P[^*]TAIRE[^*]{1,}?LL'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'xxxxxxxxxxxPSTAIREISLLxxxxxxxxx') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test147
	| re re2 |
	re _ Re on: '(\.\d\d[1-9]?)\d+'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '1.230003938') isNil not.
	self assert: (re search: '1.875000282') isNil not.
	self assert: (re search: '1.235') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test148
	| re re2 |
	re _ Re on: '(\.\d\d((?=0)|\d(?=\d)))'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '1.230003938') isNil not.
	self assert: (re search: '1.875000282') isNil not.
	self assert: (re search: '1.235') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test149
	| re re2 |
	re _ Re on: 'a(?)b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test15
	| re re2 |
	re _ Re on: '^[^]cde]'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'athing') isNil not.
	self assert: (re search: 'fthing') isNil not.
	self assert: (re search: ']thing') isNil.
	self assert: (re search: 'cthing') isNil.
	self assert: (re search: 'dthing') isNil.
	self assert: (re search: 'ething') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test150
	| re re2 |
	re _ Re on: '\b(foo)\s+(\w+)'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'Food is on the foo table') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test151
	| re re2 |
	re _ Re on: 'foo(.*)bar'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'The food is under the bar in the barn.') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test152
	| re re2 |
	re _ Re on: 'foo(.*?)bar'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'The food is under the bar in the barn.') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test153
	| re re2 |
	re _ Re on: '(.*)(\d*)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'I have 2 numbers: 53147') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test154
	| re re2 |
	re _ Re on: '(.*)(\d+)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'I have 2 numbers: 53147') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test155
	| re re2 |
	re _ Re on: '(.*?)(\d*)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'I have 2 numbers: 53147') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test156
	| re re2 |
	re _ Re on: '(.*?)(\d+)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'I have 2 numbers: 53147') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test157
	| re re2 |
	re _ Re on: '(.*)(\d+)$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'I have 2 numbers: 53147') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test158
	| re re2 |
	re _ Re on: '(.*?)(\d+)$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'I have 2 numbers: 53147') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test159
	| re re2 |
	re _ Re on: '(.*)\b(\d+)$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'I have 2 numbers: 53147') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test16
	| re re2 |
	re _ Re on: '^\�'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '�') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test160
	| re re2 |
	re _ Re on: '(.*\D)(\d+)$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'I have 2 numbers: 53147') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test161
	| re re2 |
	re _ Re on: '^\D*(?!!123)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABC123') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test162
	| re re2 |
	re _ Re on: '^(\D*)(?=\d)(?!!123)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABC445') isNil not.
	self assert: (re search: 'ABC123') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test163
	| re re2 |
	re _ Re on: '^[W-]46]'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'W46]789') isNil not.
	self assert: (re search: '-46]789') isNil not.
	self assert: (re search: 'Wall') isNil.
	self assert: (re search: 'Zebra') isNil.
	self assert: (re search: '42') isNil.
	self assert: (re search: '[abcd]') isNil.
	self assert: (re search: ']abcd[') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test164
	| re re2 |
	re _ Re on: '^[W-\]46]'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'W46]789') isNil not.
	self assert: (re search: 'Wall') isNil not.
	self assert: (re search: 'Zebra') isNil not.
	self assert: (re search: 'Xylophone') isNil not.
	self assert: (re search: '42') isNil not.
	self assert: (re search: '[abcd]') isNil not.
	self assert: (re search: ']abcd[') isNil not.
	self assert: (re search: '\\backslash') isNil not.
	self assert: (re search: '-46]789') isNil.
	self assert: (re search: 'well') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test165
	| re re2 |
	re _ Re on: '\d\d/\d\d\/\d\d\d\d'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '01/01/2000') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:43'!
testNewSuite1test166
	| re |
	re _ Re on: 'word (?:[a-zA-Z0-9]+ ){0,10}otherword'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'word cat dog elephant mussel cow horse canary baboon snake shark otherword') isNil not.
	self assert: (re search: 'word cat dog elephant mussel cow horse canary baboon snake shark') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:34'!
testNewSuite1test167
	| re |
	re _ Re on: 'word (?:[a-zA-Z0-9]+ ){0,300}otherword'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'word cat dog elephant mussel cow horse canary baboon snake shark the quick brown fox and the lazy dog and several other words getting close to thirty by now I hope otherword') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test168
	| re re2 |
	re _ Re on: '^(a){0,0}'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'bcd') isNil not.
	self assert: (re search: 'abc') isNil not.
	self assert: (re search: 'aab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test169
	| re re2 |
	re _ Re on: '^(a){0,1}'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'bcd') isNil not.
	self assert: (re search: 'abc') isNil not.
	self assert: (re search: 'aab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test17
	| re re2 |
	re _ Re on: '^�'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '�') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test170
	| re re2 |
	re _ Re on: '^(a){0,2}'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'bcd') isNil not.
	self assert: (re search: 'abc') isNil not.
	self assert: (re search: 'aab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test171
	| re re2 |
	re _ Re on: '^(a){0,3}'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'bcd') isNil not.
	self assert: (re search: 'abc') isNil not.
	self assert: (re search: 'aab') isNil not.
	self assert: (re search: 'aaa') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test172
	| re re2 |
	re _ Re on: '^(a){0,}'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'bcd') isNil not.
	self assert: (re search: 'abc') isNil not.
	self assert: (re search: 'aab') isNil not.
	self assert: (re search: 'aaa') isNil not.
	self assert: (re search: 'aaaaaaaa') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:43'!
testNewSuite1test173
	| re |
	re _ Re on: '^(a){1,1}'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'bcd') isNil.
	self assert: (re search: 'abc') isNil not.
	self assert: (re search: 'aab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:35'!
testNewSuite1test174
	| re |
	re _ Re on: '^(a){1,2}'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'bcd') isNil.
	self assert: (re search: 'abc') isNil not.
	self assert: (re search: 'aab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:43'!
testNewSuite1test175
	| re |
	re _ Re on: '^(a){1,3}'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'bcd') isNil.
	self assert: (re search: 'abc') isNil not.
	self assert: (re search: 'aab') isNil not.
	self assert: (re search: 'aaa') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:44'!
testNewSuite1test176
	| re |
	re _ Re on: '^(a){1,}'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'bcd') isNil.
	self assert: (re search: 'abc') isNil not.
	self assert: (re search: 'aab') isNil not.
	self assert: (re search: 'aaa') isNil not.
	self assert: (re search: 'aaaaaaaa') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test177
	| re re2 |
	re _ Re on: '.*\.gif'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'borfle';
		nextPut: Character cr;
		nextPutAll: 'bib.gif';
		nextPut: Character cr;
		nextPutAll: 'no'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test178
	| re re2 |
	re _ Re on: '.{0,}\.gif'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'borfle';
		nextPut: Character cr;
		nextPutAll: 'bib.gif';
		nextPut: Character cr;
		nextPutAll: 'no'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test179
	| re re2 |
	re _ Re on: '.*\.gif'.
	re
		beMultiline.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'borfle';
		nextPut: Character cr;
		nextPutAll: 'bib.gif';
		nextPut: Character cr;
		nextPutAll: 'no'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test18
	| re re2 |
	re _ Re on: '^[0-9]+$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '0') isNil not.
	self assert: (re search: '1') isNil not.
	self assert: (re search: '2') isNil not.
	self assert: (re search: '3') isNil not.
	self assert: (re search: '4') isNil not.
	self assert: (re search: '5') isNil not.
	self assert: (re search: '6') isNil not.
	self assert: (re search: '7') isNil not.
	self assert: (re search: '8') isNil not.
	self assert: (re search: '9') isNil not.
	self assert: (re search: '10') isNil not.
	self assert: (re search: '100') isNil not.
	self assert: (re search: 'abc') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test180
	| re re2 |
	re _ Re on: '.*\.gif'.
	re
		beDotIncludesNewline.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'borfle';
		nextPut: Character cr;
		nextPutAll: 'bib.gif';
		nextPut: Character cr;
		nextPutAll: 'no'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test181
	| re re2 |
	re _ Re on: '.*\.gif'.
	re
		beMultiline;
		beDotIncludesNewline.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'borfle';
		nextPut: Character cr;
		nextPutAll: 'bib.gif';
		nextPut: Character cr;
		nextPutAll: 'no'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test182
	| re re2 |
	re _ Re on: '.*$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'borfle';
		nextPut: Character cr;
		nextPutAll: 'bib.gif';
		nextPut: Character cr;
		nextPutAll: 'no'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test183
	| re re2 |
	re _ Re on: '.*$'.
	re
		beMultiline.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'borfle';
		nextPut: Character cr;
		nextPutAll: 'bib.gif';
		nextPut: Character cr;
		nextPutAll: 'no'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test184
	| re re2 |
	re _ Re on: '.*$'.
	re
		beDotIncludesNewline.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'borfle';
		nextPut: Character cr;
		nextPutAll: 'bib.gif';
		nextPut: Character cr;
		nextPutAll: 'no'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test185
	| re re2 |
	re _ Re on: '.*$'.
	re
		beMultiline;
		beDotIncludesNewline.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'borfle';
		nextPut: Character cr;
		nextPutAll: 'bib.gif';
		nextPut: Character cr;
		nextPutAll: 'no'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test186
	| re re2 |
	re _ Re on: '.*$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'borfle';
		nextPut: Character cr;
		nextPutAll: 'bib.gif';
		nextPut: Character cr;
		nextPutAll: 'no';
		nextPut: Character cr])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test187
	| re re2 |
	re _ Re on: '.*$'.
	re
		beMultiline.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'borfle';
		nextPut: Character cr;
		nextPutAll: 'bib.gif';
		nextPut: Character cr;
		nextPutAll: 'no';
		nextPut: Character cr])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test188
	| re re2 |
	re _ Re on: '.*$'.
	re
		beDotIncludesNewline.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'borfle';
		nextPut: Character cr;
		nextPutAll: 'bib.gif';
		nextPut: Character cr;
		nextPutAll: 'no';
		nextPut: Character cr])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test189
	| re re2 |
	re _ Re on: '.*$'.
	re
		beMultiline;
		beDotIncludesNewline.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'borfle';
		nextPut: Character cr;
		nextPutAll: 'bib.gif';
		nextPut: Character cr;
		nextPutAll: 'no';
		nextPut: Character cr])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test19
	| re re2 |
	re _ Re on: '^.*nter'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'enter') isNil not.
	self assert: (re search: 'inter') isNil not.
	self assert: (re search: 'uponter') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test190
	| re re2 |
	re _ Re on: '(.*X|^B)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abcde';
		nextPut: Character cr;
		nextPutAll: '1234Xyz'])) isNil not.
	self assert: (re search: 'BarFoo') isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abcde';
		nextPut: Character cr;
		nextPutAll: 'Bar'])) isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test191
	| re re2 |
	re _ Re on: '(.*X|^B)'.
	re
		beMultiline.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abcde';
		nextPut: Character cr;
		nextPutAll: '1234Xyz'])) isNil not.
	self assert: (re search: 'BarFoo') isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abcde';
		nextPut: Character cr;
		nextPutAll: 'Bar'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test192
	| re re2 |
	re _ Re on: '(.*X|^B)'.
	re
		beDotIncludesNewline.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abcde';
		nextPut: Character cr;
		nextPutAll: '1234Xyz'])) isNil not.
	self assert: (re search: 'BarFoo') isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abcde';
		nextPut: Character cr;
		nextPutAll: 'Bar'])) isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test193
	| re re2 |
	re _ Re on: '(.*X|^B)'.
	re
		beMultiline;
		beDotIncludesNewline.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abcde';
		nextPut: Character cr;
		nextPutAll: '1234Xyz'])) isNil not.
	self assert: (re search: 'BarFoo') isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abcde';
		nextPut: Character cr;
		nextPutAll: 'Bar'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test194
	| re re2 |
	re _ Re on: '(?s)(.*X|^B)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abcde';
		nextPut: Character cr;
		nextPutAll: '1234Xyz'])) isNil not.
	self assert: (re search: 'BarFoo') isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abcde';
		nextPut: Character cr;
		nextPutAll: 'Bar'])) isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test195
	| re re2 |
	re _ Re on: '(?s:.*X|^B)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abcde';
		nextPut: Character cr;
		nextPutAll: '1234Xyz'])) isNil not.
	self assert: (re search: 'BarFoo') isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abcde';
		nextPut: Character cr;
		nextPutAll: 'Bar'])) isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test196
	| re re2 |
	re _ Re on: '^.*B'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: Character cr;
		nextPutAll: 'B'])) isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test197
	| re re2 |
	re _ Re on: '(?s)^.*B'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: Character cr;
		nextPutAll: 'B'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test198
	| re re2 |
	re _ Re on: '(?m)^.*B'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: Character cr;
		nextPutAll: 'B'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test199
	| re re2 |
	re _ Re on: '(?ms)^.*B'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: Character cr;
		nextPutAll: 'B'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test2
	| re re2 |
	re _ Re on: 'The quick brown fox'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'the quick brown fox') isNil not.
	self assert: (re search: 'The quick brown FOX') isNil not.
	self assert: (re search: 'What do you know about the quick brown fox?') isNil not.
	self assert: (re search: 'What do you know about THE QUICK BROWN FOX?') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test20
	| re re2 |
	re _ Re on: '^xxx[0-9]+$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'xxx0') isNil not.
	self assert: (re search: 'xxx1234') isNil not.
	self assert: (re search: 'xxx') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test200
	| re re2 |
	re _ Re on: '(?ms)^B'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: Character cr;
		nextPutAll: 'B'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test201
	| re re2 |
	re _ Re on: '(?s)B$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'B';
		nextPut: Character cr])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test202
	| re re2 |
	re _ Re on: '^[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '123456654321') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test203
	| re re2 |
	re _ Re on: '^\d\d\d\d\d\d\d\d\d\d\d\d'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '123456654321') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test204
	| re re2 |
	re _ Re on: '^[\d][\d][\d][\d][\d][\d][\d][\d][\d][\d][\d][\d]'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '123456654321') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test205
	| re re2 |
	re _ Re on: '^[abc]{12}'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcabcabcabc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test206
	| re re2 |
	re _ Re on: '^[a-c]{12}'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcabcabcabc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test207
	| re re2 |
	re _ Re on: '^(a|b|c){12}'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcabcabcabc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test208
	| re re2 |
	re _ Re on: '^[abcdefghijklmnopqrstuvwxy0123456789]'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'n') isNil not.
	self assert: (re search: 'z') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test209
	| re re2 |
	re _ Re on: 'abcde{0,0}'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcd') isNil not.
	self assert: (re search: 'abce') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 14:10'!
testNewSuite1test21
	| re |
	re _ Re on: '^.+[0-9][0-9][0-9]$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'x123') isNil not.
	self assert: (re search: 'xx123') isNil not.
	self assert: (re search: '123456') isNil not.
	self assert: (re search: '123') isNil.
	self assert: (re search: 'x1234') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test210
	| re re2 |
	re _ Re on: 'ab[cd]{0,0}e'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abe') isNil not.
	self assert: (re search: 'abcde') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test211
	| re re2 |
	re _ Re on: 'ab(c){0,0}d'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abd') isNil not.
	self assert: (re search: 'abcd') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test212
	| re re2 |
	re _ Re on: 'a(b*)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a') isNil not.
	self assert: (re search: 'ab') isNil not.
	self assert: (re search: 'abbbb') isNil not.
	self assert: (re search: 'bbbbb') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test213
	| re re2 |
	re _ Re on: 'ab\d{0}e'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abe') isNil not.
	self assert: (re search: 'ab1e') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:48'!
testNewSuite1test214
	| re |
	re _ Re on: '"([^\\"]+|\\.)*"'.
	self shouldnt: [re compile] raise: Error.
	self assert: ('the \"quick\" brown fox') isNil not.
	self assert: ('\"the \\\"quick\\\" brown fox\"') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test215
	| re re2 |
	re _ Re on: '.*?'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test216
	| re re2 |
	re _ Re on: '\b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test217
	| re re2 |
	re _ Re on: '\b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test218
	| re re2 |
	re _ Re on: ''.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test219
	| re re2 |
	re _ Re on: '<tr([\w\W\s\d][^<>]{0,})><TD([\w\W\s\d][^<>]{0,})>([\d]{0,}\.)(.*)((<BR>([\w\W\s\d][^<>]{0,})|[\s]{0,}))</a></TD><TD([\w\W\s\d][^<>]{0,})>([\w\W\s\d][^<>]{0,})</TD><TD([\w\W\s\d][^<>]{0,})>([\w\W\s\d][^<>]{0,})</TD></TR>'.
	re
		beNotCaseSensitive;
		beDotIncludesNewline.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '<TR BGCOLOR=''#DBE9E9''><TD align=left valign=top>43.<a href=''joblist.cfm?JobID=94 6735&Keyword=''>Word Processor<BR>(N-1286)</a></TD><TD align=left valign=top>Lega lstaff.com</TD><TD align=left valign=top>CA - Statewide</TD></TR>') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 14:11'!
testNewSuite1test22
	| re |
	re _ Re on: '^.+?[0-9][0-9][0-9]$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'x123') isNil not.
	self assert: (re search: 'xx123') isNil not.
	self assert: (re search: '123456') isNil not.
	self assert: (re search: '123') isNil.
	self assert: (re search: 'x1234') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test220
	| re re2 |
	re _ Re on: 'a[^a]b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'acb') isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'a';
		nextPut: Character cr;
		nextPutAll: 'b'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test221
	| re re2 |
	re _ Re on: 'a.b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'acb') isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'a';
		nextPut: Character cr;
		nextPutAll: 'b'])) isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test222
	| re re2 |
	re _ Re on: 'a[^a]b'.
	re
		beDotIncludesNewline.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'acb') isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'a';
		nextPut: Character cr;
		nextPutAll: 'b'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test223
	| re re2 |
	re _ Re on: 'a.b'.
	re
		beDotIncludesNewline.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'acb') isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'a';
		nextPut: Character cr;
		nextPutAll: 'b'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test224
	| re re2 |
	re _ Re on: '^(b+?|a){1,2}?c'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'bac') isNil not.
	self assert: (re search: 'bbac') isNil not.
	self assert: (re search: 'bbbac') isNil not.
	self assert: (re search: 'bbbbac') isNil not.
	self assert: (re search: 'bbbbbac') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test225
	| re re2 |
	re _ Re on: '^(b+|a){1,2}?c'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'bac') isNil not.
	self assert: (re search: 'bbac') isNil not.
	self assert: (re search: 'bbbac') isNil not.
	self assert: (re search: 'bbbbac') isNil not.
	self assert: (re search: 'bbbbbac') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 14:12'!
testNewSuite1test226
	| re |
	re _ Re on: '(?!!\A)x'.
	re
		beMultiline.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'x';
		nextPut: Character cr;
		nextPutAll: 'b';
		nextPut: Character cr])) isNil.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'a';
		nextPut: Character backspace;
		nextPutAll: 'x';
		nextPut: Character cr])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test227
	| re re2 |
	re _ Re on: '\x0{ab}'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPut: 8r0 asCharacter;
		nextPutAll: '{ab}'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test228
	| re re2 |
	re _ Re on: '(A|B)*?CD'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'CD') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test229
	| re re2 |
	re _ Re on: '(A|B)*CD'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'CD') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test23
	| re re2 |
	re _ Re on: '^([^!!]+)!!(.+)=apquxz\.ixr\.zzz\.ac\.uk$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc!!pqr=apquxz.ixr.zzz.ac.uk') isNil not.
	self assert: (re search: '!!pqr=apquxz.ixr.zzz.ac.uk') isNil.
	self assert: (re search: 'abc!!=apquxz.ixr.zzz.ac.uk') isNil.
	self assert: (re search: 'abc!!pqr=apquxz:ixr.zzz.ac.uk') isNil.
	self assert: (re search: 'abc!!pqr=apquxz.ixr.zzz.ac.ukk') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test230
	| re re2 |
	re _ Re on: '(AB)*?\1'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABABAB') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test231
	| re re2 |
	re _ Re on: '(AB)*\1'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABABAB') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test232
	| re re2 |
	re _ Re on: ' End of testinput1 '.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test24
	| re re2 |
	re _ Re on: ':'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'Well, we need a colon: somewhere') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test25
	| re re2 |
	re _ Re on: '([\da-f:]+)$'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '0abc') isNil not.
	self assert: (re search: 'abc') isNil not.
	self assert: (re search: 'fed') isNil not.
	self assert: (re search: 'E') isNil not.
	self assert: (re search: '::') isNil not.
	self assert: (re search: '5f03:12C0::932e') isNil not.
	self assert: (re search: 'fed def') isNil not.
	self assert: (re search: 'Any old stuff') isNil not.
	self assert: (re search: '0zzz') isNil.
	self assert: (re search: 'gzzz') isNil.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'fed';
		nextPut: 16r20 asCharacter])) isNil.
	self assert: (re search: 'Any old rubbish') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test26
	| re re2 |
	re _ Re on: '^.*\.(\d{1,3})\.(\d{1,3})\.(\d{1,3})$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '.1.2.3') isNil not.
	self assert: (re search: 'A.12.123.0') isNil not.
	self assert: (re search: '.1.2.3333') isNil.
	self assert: (re search: '1.2.3') isNil.
	self assert: (re search: '1234.2.3') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test27
	| re re2 |
	re _ Re on: '^(\d+)\s+IN\s+SOA\s+(\S+)\s+(\S+)\s*\(\s*$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '1 IN SOA non-sp1 non-sp2(') isNil not.
	self assert: (re search: '1    IN    SOA    non-sp1    non-sp2   (') isNil not.
	self assert: (re search: '1IN SOA non-sp1 non-sp2(') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test28
	| re re2 |
	re _ Re on: '^[a-zA-Z\d][a-zA-Z\d\-]*(\.[a-zA-Z\d][a-zA-z\d\-]*)*\.$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a.') isNil not.
	self assert: (re search: 'Z.') isNil not.
	self assert: (re search: '2.') isNil not.
	self assert: (re search: 'ab-c.pq-r.') isNil not.
	self assert: (re search: 'sxk.zzz.ac.uk.') isNil not.
	self assert: (re search: 'x-.y-.') isNil not.
	self assert: (re search: '-abc.peq.') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test29
	| re re2 |
	re _ Re on: '^\*\.[a-z]([a-z\-\d]*[a-z\d]+)?(\.[a-z]([a-z\-\d]*[a-z\d]+)?)*$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '*.a') isNil not.
	self assert: (re search: '*.b0-a') isNil not.
	self assert: (re search: '*.c3-b.c') isNil not.
	self assert: (re search: '*.c-a.b-c') isNil not.
	self assert: (re search: '*.0') isNil.
	self assert: (re search: '*.a-') isNil.
	self assert: (re search: '*.a-b.c-') isNil.
	self assert: (re search: '*.c-a.0-c') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:54'!
testNewSuite1test3
	| re |
	re _ Re on: 'abcd\t\n\r\f\a\e\071\x3b\$\\\?caxyz'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abcd';
		nextPut: Character tab;
		nextPut: Character cr;
		nextPut: Character cr;
		nextPut: Character newPage;
		nextPut: 7 asCharacter;
		nextPut: Character escape;
		nextPut: 8r071 asCharacter;
		nextPut: 16r3B asCharacter;
		nextPutAll: '$\?caxyz'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test30
	| re re2 |
	re _ Re on: '^(?=ab(de))(abd)(e)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abde') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test31
	| re re2 |
	re _ Re on: '^(?!!(ab)de|x)(abd)(f)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abdf') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test32
	| re re2 |
	re _ Re on: '^(?=(ab(cd)))(ab)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcd') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test33
	| re re2 |
	re _ Re on: '^[\da-f](\.[\da-f])*$'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a.b.c.d') isNil not.
	self assert: (re search: 'A.B.C.D') isNil not.
	self assert: (re search: 'a.b.c.1.2.3.C') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 14:16'!
testNewSuite1test34
	| re |
	re _ Re on: '^\".*\"\s*(;.*)?$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '"1234"') isNil not.
	self assert: (re search: '"abcd" ;') isNil not.
	self assert: (re search: '"" ; rhubarb') isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: '1234';
		nextPutAll: ' : things'])) isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test35
	| re re2 |
	re _ Re on: '^$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test36
	| re re2 |
	re _ Re on: '   ^    a   (?# begins with a)  b\sc (?# then b c) $ (?# then end)'.
	re
		beExtended.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ab c') isNil not.
	self assert: (re search: 'abc') isNil.
	self assert: (re search: 'ab cde') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test37
	| re re2 |
	re _ Re on: '(?x)   ^    a   (?# begins with a)  b\sc (?# then b c) $ (?# then end)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ab c') isNil not.
	self assert: (re search: 'abc') isNil.
	self assert: (re search: 'ab cde') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test38
	| re re2 |
	re _ Re on: '^   a\ b[c ]d       $'.
	re
		beExtended.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a bcd') isNil not.
	self assert: (re search: 'a b d') isNil not.
	self assert: (re search: 'abcd') isNil.
	self assert: (re search: 'ab d') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test39
	| re re2 |
	re _ Re on: '^(a(b(c)))(d(e(f)))(h(i(j)))(k(l(m)))$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcdefhijklm') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test4
	| re re2 |
	re _ Re on: 'a*abc?xyz+pqr{3}ab{2,}xy{4,5}pq{0,6}AB{0,}zz'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abxyzpqrrrabbxyyyypqAzz') isNil not.
	self assert: (re search: 'abxyzpqrrrabbxyyyypqAzz') isNil not.
	self assert: (re search: 'aabxyzpqrrrabbxyyyypqAzz') isNil not.
	self assert: (re search: 'aaabxyzpqrrrabbxyyyypqAzz') isNil not.
	self assert: (re search: 'aaaabxyzpqrrrabbxyyyypqAzz') isNil not.
	self assert: (re search: 'abcxyzpqrrrabbxyyyypqAzz') isNil not.
	self assert: (re search: 'aabcxyzpqrrrabbxyyyypqAzz') isNil not.
	self assert: (re search: 'aaabcxyzpqrrrabbxyyyypAzz') isNil not.
	self assert: (re search: 'aaabcxyzpqrrrabbxyyyypqAzz') isNil not.
	self assert: (re search: 'aaabcxyzpqrrrabbxyyyypqqAzz') isNil not.
	self assert: (re search: 'aaabcxyzpqrrrabbxyyyypqqqAzz') isNil not.
	self assert: (re search: 'aaabcxyzpqrrrabbxyyyypqqqqAzz') isNil not.
	self assert: (re search: 'aaabcxyzpqrrrabbxyyyypqqqqqAzz') isNil not.
	self assert: (re search: 'aaabcxyzpqrrrabbxyyyypqqqqqqAzz') isNil not.
	self assert: (re search: 'aaaabcxyzpqrrrabbxyyyypqAzz') isNil not.
	self assert: (re search: 'abxyzzpqrrrabbxyyyypqAzz') isNil not.
	self assert: (re search: 'aabxyzzzpqrrrabbxyyyypqAzz') isNil not.
	self assert: (re search: 'aaabxyzzzzpqrrrabbxyyyypqAzz') isNil not.
	self assert: (re search: 'aaaabxyzzzzpqrrrabbxyyyypqAzz') isNil not.
	self assert: (re search: 'abcxyzzpqrrrabbxyyyypqAzz') isNil not.
	self assert: (re search: 'aabcxyzzzpqrrrabbxyyyypqAzz') isNil not.
	self assert: (re search: 'aaabcxyzzzzpqrrrabbxyyyypqAzz') isNil not.
	self assert: (re search: 'aaaabcxyzzzzpqrrrabbxyyyypqAzz') isNil not.
	self assert: (re search: 'aaaabcxyzzzzpqrrrabbbxyyyypqAzz') isNil not.
	self assert: (re search: 'aaaabcxyzzzzpqrrrabbbxyyyyypqAzz') isNil not.
	self assert: (re search: 'aaabcxyzpqrrrabbxyyyypABzz') isNil not.
	self assert: (re search: 'aaabcxyzpqrrrabbxyyyypABBzz') isNil not.
	self assert: (re search: '>>>aaabxyzpqrrrabbxyyyypqAzz') isNil not.
	self assert: (re search: '>aaaabxyzpqrrrabbxyyyypqAzz') isNil not.
	self assert: (re search: '>>>>abcxyzpqrrrabbxyyyypqAzz') isNil not.
	self assert: (re search: 'abxyzpqrrabbxyyyypqAzz') isNil.
	self assert: (re search: 'abxyzpqrrrrabbxyyyypqAzz') isNil.
	self assert: (re search: 'abxyzpqrrrabxyyyypqAzz') isNil.
	self assert: (re search: 'aaaabcxyzzzzpqrrrabbbxyyyyyypqAzz') isNil.
	self assert: (re search: 'aaaabcxyzzzzpqrrrabbbxyyypqAzz') isNil.
	self assert: (re search: 'aaabcxyzpqrrrabbxyyyypqqqqqqqAzz') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test40
	| re re2 |
	re _ Re on: '^(?:a(b(c)))(?:d(e(f)))(?:h(i(j)))(?:k(l(m)))$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcdefhijklm') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test41
	| re re2 |
	re _ Re on: '^[\w][\W][\s][\S][\d][\D][\b][\n][\c]][\022]'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'a+ Z0+';
		nextPut: 16r08 asCharacter;
		nextPut: Character cr;
		nextPut: 16r1D asCharacter;
		nextPut: 16r12 asCharacter])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test42
	| re re2 |
	re _ Re on: '^[.^$|()*+?{,}]+'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: '.^';
		nextPutAll: '(*+)|{?,?}'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test43
	| re re2 |
	re _ Re on: '^a*\w'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'z') isNil not.
	self assert: (re search: 'az') isNil not.
	self assert: (re search: 'aaaz') isNil not.
	self assert: (re search: 'a') isNil not.
	self assert: (re search: 'aa') isNil not.
	self assert: (re search: 'aaaa') isNil not.
	self assert: (re search: 'a+') isNil not.
	self assert: (re search: 'aa+') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test44
	| re re2 |
	re _ Re on: '^a*?\w'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'z') isNil not.
	self assert: (re search: 'az') isNil not.
	self assert: (re search: 'aaaz') isNil not.
	self assert: (re search: 'a') isNil not.
	self assert: (re search: 'aa') isNil not.
	self assert: (re search: 'aaaa') isNil not.
	self assert: (re search: 'a+') isNil not.
	self assert: (re search: 'aa+') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test45
	| re re2 |
	re _ Re on: '^a+\w'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'az') isNil not.
	self assert: (re search: 'aaaz') isNil not.
	self assert: (re search: 'aa') isNil not.
	self assert: (re search: 'aaaa') isNil not.
	self assert: (re search: 'aa+') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test46
	| re re2 |
	re _ Re on: '^a+?\w'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'az') isNil not.
	self assert: (re search: 'aaaz') isNil not.
	self assert: (re search: 'aa') isNil not.
	self assert: (re search: 'aaaa') isNil not.
	self assert: (re search: 'aa+') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test47
	| re re2 |
	re _ Re on: '^\d{8}\w{2,}'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '1234567890') isNil not.
	self assert: (re search: '12345678ab') isNil not.
	self assert: (re search: '12345678__') isNil not.
	self assert: (re search: '1234567') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test48
	| re re2 |
	re _ Re on: '^[aeiou\d]{4,5}$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'uoie') isNil not.
	self assert: (re search: '1234') isNil not.
	self assert: (re search: '12345') isNil not.
	self assert: (re search: 'aaaaa') isNil not.
	self assert: (re search: '123456') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test49
	| re re2 |
	re _ Re on: '^[aeiou\d]{4,5}?'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'uoie') isNil not.
	self assert: (re search: '1234') isNil not.
	self assert: (re search: '12345') isNil not.
	self assert: (re search: 'aaaaa') isNil not.
	self assert: (re search: '123456') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test5
	| re re2 |
	re _ Re on: '^(abc){1,2}zz'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abczz') isNil not.
	self assert: (re search: 'abcabczz') isNil not.
	self assert: (re search: 'zz') isNil.
	self assert: (re search: 'abcabcabczz') isNil.
	self assert: (re search: '>>abczz') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test50
	| re re2 |
	re _ Re on: '\A(abc|def)=(\1){2,3}\Z'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc=abcabc') isNil not.
	self assert: (re search: 'def=defdefdef') isNil not.
	self assert: (re search: 'abc=defdef') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test51
	| re re2 |
	re _ Re on: '^(a)(b)(c)(d)(e)(f)(g)(h)(i)(j)(k)\11*(\3\4)\1(?#)2$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcdefghijkcda2') isNil not.
	self assert: (re search: 'abcdefghijkkkkcda2') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test52
	| re re2 |
	re _ Re on: '(cat(a(ract|tonic)|erpillar)) \1()2(3)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'cataract cataract23') isNil not.
	self assert: (re search: 'catatonic catatonic23') isNil not.
	self assert: (re search: 'caterpillar caterpillar23') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test54
	| re re2 |
	re _ Re on: '^From\s+\S+\s+([a-zA-Z]{3}\s+){2}\d{1,2}\s+\d\d:\d\d'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'From abcd  Mon Sep 01 12:33:02 1997') isNil not.
	self assert: (re search: 'From abcd  Mon Sep  1 12:33:02 1997') isNil not.
	self assert: (re search: 'From abcd  Sep 01 12:33:02 1997') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test55
	| re re2 |
	re _ Re on: '^12.34'.
	re
		beDotIncludesNewline.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: '12';
		nextPut: Character cr;
		nextPutAll: '34'])) isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: '12';
		nextPut: Character linefeed;
		nextPutAll: '34'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test56
	| re re2 |
	re _ Re on: '\w+(?=\t)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'the quick brown';
		nextPut: Character tab;
		nextPutAll: ' fox'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test57
	| re re2 |
	re _ Re on: 'foo(?!!bar)(.*)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'foobar is foolish see?') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test58
	| re re2 |
	re _ Re on: '(?:(?!!foo)...|^.{0,2})bar(.*)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'foobar crowbar etc') isNil not.
	self assert: (re search: 'barrel') isNil not.
	self assert: (re search: '2barrel') isNil not.
	self assert: (re search: 'A barrel') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test59
	| re re2 |
	re _ Re on: '^(\D*)(?=\d)(?!!123)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc456') isNil not.
	self assert: (re search: 'abc123') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test6
	| re re2 |
	re _ Re on: '^(b+?|a){1,2}?c'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'bc') isNil not.
	self assert: (re search: 'bbc') isNil not.
	self assert: (re search: 'bbbc') isNil not.
	self assert: (re search: 'bac') isNil not.
	self assert: (re search: 'bbac') isNil not.
	self assert: (re search: 'aac') isNil not.
	self assert: (re search: 'abbbbbbbbbbbc') isNil not.
	self assert: (re search: 'bbbbbbbbbbbac') isNil not.
	self assert: (re search: 'aaac') isNil.
	self assert: (re search: 'abbbbbbbbbbbac') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test60
	| re re2 |
	re _ Re on: '^1234(?# test newlines
  inside)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '1234') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test61
	| re re2 |
	re _ Re on: '^1234 #comment in extended re
  '.
	re
		beExtended.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '1234') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test62
	| re re2 |
	re _ Re on: '#rhubarb
  abcd'.
	re
		beExtended.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcd') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test63
	| re re2 |
	re _ Re on: '^abcd#rhubarb'.
	re
		beExtended.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcd') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test64
	| re re2 |
	re _ Re on: '^(a)\1{2,3}(.)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aaab') isNil not.
	self assert: (re search: 'aaaab') isNil not.
	self assert: (re search: 'aaaaab') isNil not.
	self assert: (re search: 'aaaaaab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test65
	| re re2 |
	re _ Re on: '(?!!^)abc'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'the abc') isNil not.
	self assert: (re search: 'abc') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test66
	| re re2 |
	re _ Re on: '(?=^)abc'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
	self assert: (re search: 'the abc') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test67
	| re re2 |
	re _ Re on: '^[ab]{1,3}(ab*|b)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aabbbbb') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test68
	| re re2 |
	re _ Re on: '^[ab]{1,3}?(ab*|b)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aabbbbb') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test69
	| re re2 |
	re _ Re on: '^[ab]{1,3}?(ab*?|b)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aabbbbb') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test7
	| re re2 |
	re _ Re on: '^(b+|a){1,2}c'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'bc') isNil not.
	self assert: (re search: 'bbc') isNil not.
	self assert: (re search: 'bbbc') isNil not.
	self assert: (re search: 'bac') isNil not.
	self assert: (re search: 'bbac') isNil not.
	self assert: (re search: 'aac') isNil not.
	self assert: (re search: 'abbbbbbbbbbbc') isNil not.
	self assert: (re search: 'bbbbbbbbbbbac') isNil not.
	self assert: (re search: 'aaac') isNil.
	self assert: (re search: 'abbbbbbbbbbbac') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test70
	| re re2 |
	re _ Re on: '^[ab]{1,3}(ab*?|b)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aabbbbb') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test73
	| re re2 |
	re _ Re on: 'abc\0def\00pqr\000xyz\0000AB'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: 8r0 asCharacter;
		nextPutAll: 'def';
		nextPut: 8r00 asCharacter;
		nextPutAll: 'pqr';
		nextPut: 8r000 asCharacter;
		nextPutAll: 'xyz';
		nextPut: 8r000 asCharacter;
		nextPutAll: '0AB'])) isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc456 abc';
		nextPut: 8r0 asCharacter;
		nextPutAll: 'def';
		nextPut: 8r00 asCharacter;
		nextPutAll: 'pqr';
		nextPut: 8r000 asCharacter;
		nextPutAll: 'xyz';
		nextPut: 8r000 asCharacter;
		nextPutAll: '0ABCDE'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test74
	| re re2 |
	re _ Re on: 'abc\x0def\x00pqr\x000xyz\x0000AB'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: 16r0D asCharacter;
		nextPutAll: 'ef';
		nextPut: 16r00 asCharacter;
		nextPutAll: 'pqr';
		nextPut: 16r00 asCharacter;
		nextPutAll: '0xyz';
		nextPut: 16r00 asCharacter;
		nextPutAll: '00AB'])) isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc456 abc';
		nextPut: 16r0D asCharacter;
		nextPutAll: 'ef';
		nextPut: 16r00 asCharacter;
		nextPutAll: 'pqr';
		nextPut: 16r00 asCharacter;
		nextPutAll: '0xyz';
		nextPut: 16r00 asCharacter;
		nextPutAll: '00ABCDE'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test75
	| re re2 |
	re _ Re on: '^[\000-\037]'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPut: 8r0 asCharacter;
		nextPutAll: 'A'])) isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPut: 8r01 asCharacter;
		nextPutAll: 'B'])) isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPut: 8r037 asCharacter;
		nextPutAll: 'C'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test76
	| re re2 |
	re _ Re on: '\0*'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPut: 8r0 asCharacter;
		nextPut: 8r0 asCharacter;
		nextPut: 8r0 asCharacter;
		nextPut: 8r0 asCharacter])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test77
	| re re2 |
	re _ Re on: 'A\x0{2,3}Z'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'The A';
		nextPut: 16r0 asCharacter;
		nextPut: 16r0 asCharacter;
		nextPutAll: 'Z'])) isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'An A';
		nextPut: 8r0 asCharacter;
		nextPut: 16r0 asCharacter;
		nextPut: 8r0 asCharacter;
		nextPutAll: 'Z'])) isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'A';
		nextPut: 8r0 asCharacter;
		nextPutAll: 'Z'])) isNil.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'A';
		nextPut: 8r0 asCharacter;
		nextPut: 16r0 asCharacter;
		nextPut: 8r0 asCharacter;
		nextPut: 16r0 asCharacter;
		nextPutAll: 'Z'])) isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test78
	| re re2 |
	re _ Re on: '^(cow|)\1(bell)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'cowcowbell') isNil not.
	self assert: (re search: 'bell') isNil not.
	self assert: (re search: 'cowbell') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test79
	| re re2 |
	re _ Re on: '^\s'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPut: 8r040 asCharacter;
		nextPutAll: 'abc'])) isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPut: 16r0C asCharacter;
		nextPutAll: 'abc'])) isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPut: Character cr;
		nextPutAll: 'abc'])) isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPut: Character linefeed;
		nextPutAll: 'abc'])) isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPut: Character tab;
		nextPutAll: 'abc'])) isNil not.
	self assert: (re search: 'abc') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test8
	| re re2 |
	re _ Re on: '^(b+|a){1,2}?bc'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'bbc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test80
	| re re2 |
	re _ Re on: '^a	b
  
    c'.
	re
		beExtended.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test81
	| re re2 |
	re _ Re on: '^(a|)\1*b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ab') isNil not.
	self assert: (re search: 'aaaab') isNil not.
	self assert: (re search: 'b') isNil not.
	self assert: (re search: 'acb') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test82
	| re re2 |
	re _ Re on: '^(a|)\1+b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aab') isNil not.
	self assert: (re search: 'aaaab') isNil not.
	self assert: (re search: 'b') isNil not.
	self assert: (re search: 'ab') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test83
	| re re2 |
	re _ Re on: '^(a|)\1?b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ab') isNil not.
	self assert: (re search: 'aab') isNil not.
	self assert: (re search: 'b') isNil not.
	self assert: (re search: 'acb') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test84
	| re re2 |
	re _ Re on: '^(a|)\1{2}b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aaab') isNil not.
	self assert: (re search: 'b') isNil not.
	self assert: (re search: 'ab') isNil.
	self assert: (re search: 'aab') isNil.
	self assert: (re search: 'aaaab') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test85
	| re re2 |
	re _ Re on: '^(a|)\1{2,3}b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aaab') isNil not.
	self assert: (re search: 'aaaab') isNil not.
	self assert: (re search: 'b') isNil not.
	self assert: (re search: 'ab') isNil.
	self assert: (re search: 'aab') isNil.
	self assert: (re search: 'aaaaab') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test86
	| re re2 |
	re _ Re on: 'ab{1,3}bc'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abbbbc') isNil not.
	self assert: (re search: 'abbbc') isNil not.
	self assert: (re search: 'abbc') isNil not.
	self assert: (re search: 'abc') isNil.
	self assert: (re search: 'abbbbbc') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test87
	| re re2 |
	re _ Re on: '([^.]*)\.([^:]*):[T ]+(.*)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'track1.title:TBlah blah blah') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test88
	| re re2 |
	re _ Re on: '([^.]*)\.([^:]*):[T ]+(.*)'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'track1.title:TBlah blah blah') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test89
	| re re2 |
	re _ Re on: '([^.]*)\.([^:]*):[t ]+(.*)'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'track1.title:TBlah blah blah') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test9
	| re re2 |
	re _ Re on: '^(b*|ba){1,2}?bc'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'babc') isNil not.
	self assert: (re search: 'bbabc') isNil not.
	self assert: (re search: 'bababc') isNil not.
	self assert: (re search: 'bababbc') isNil.
	self assert: (re search: 'babababc') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test90
	| re re2 |
	re _ Re on: '^[W-c]+$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'WXY_^abc') isNil not.
	self assert: (re search: 'wxy') isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test91
	| re re2 |
	re _ Re on: '^[W-c]+$'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'WXY_^abc') isNil not.
	self assert: (re search: 'wxy_^ABC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test92
	| re re2 |
	re _ Re on: '^[\x3f-\x5F]+$'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'WXY_^abc') isNil not.
	self assert: (re search: 'wxy_^ABC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test93
	| re re2 |
	re _ Re on: '^abc$'.
	re
		beMultiline.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'qqq';
		nextPut: Character cr;
		nextPutAll: 'abc'])) isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: Character cr;
		nextPutAll: 'zzz'])) isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'qqq';
		nextPut: Character cr;
		nextPutAll: 'abc';
		nextPut: Character cr;
		nextPutAll: 'zzz'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test94
	| re re2 |
	re _ Re on: '^abc$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'qqq';
		nextPut: Character cr;
		nextPutAll: 'abc'])) isNil.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: Character cr;
		nextPutAll: 'zzz'])) isNil.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'qqq';
		nextPut: Character cr;
		nextPutAll: 'abc';
		nextPut: Character cr;
		nextPutAll: 'zzz'])) isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test95
	| re re2 |
	re _ Re on: '\Aabc\Z'.
	re
		beMultiline.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: Character cr])) isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'qqq';
		nextPut: Character cr;
		nextPutAll: 'abc'])) isNil.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: Character cr;
		nextPutAll: 'zzz'])) isNil.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'qqq';
		nextPut: Character cr;
		nextPutAll: 'abc';
		nextPut: Character cr;
		nextPutAll: 'zzz'])) isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test96
	| re re2 |
	re _ Re on: '\A(.)*\Z'.
	re
		beDotIncludesNewline.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: Character cr;
		nextPutAll: 'def'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test97
	| re re2 |
	re _ Re on: '\A(.)*\Z'.
	re
		beMultiline.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: Character cr;
		nextPutAll: 'def'])) isNil.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test98
	| re re2 |
	re _ Re on: '(?:b)|(?::+)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'b::c') isNil not.
	self assert: (re search: 'c::b') isNil not.
! !

!ReTest methodsFor: 'newTestSuite1' stamp: 'acg 8/11/2002 12:30'!
testNewSuite1test99
	| re re2 |
	re _ Re on: '[-az]+'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'az-') isNil not.
	self assert: (re search: 'b') isNil.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test1
	| re re2 |
	re _ Re on: '(a)b|'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:57'!
testNewSuite2test10
	| re |
	re _ Re on: '(?X)ab\gdef'.
	re
		beExtra.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test100
	| re re2 |
	re _ Re on: '(?<!!(foo)a)bar'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'bar') isNil not.
	self assert: (re search: 'foobbar') isNil not.
	self assert: (re search: 'fooabar') isNil.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test101
	| re re2 |
	re _ Re on: '^(a)?(?(1)a|b)+$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a') isNil.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test102
	| re re2 |
	re _ Re on: '^(a\1?){4}$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aaaaaa') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:58'!
testNewSuite2test103
	| re |
	re _ Re on: 'a[b-a]'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:58'!
testNewSuite2test104
	| re |
	re _ Re on: 'a[]b'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:59'!
testNewSuite2test105
	| re |
	re _ Re on: 'a['.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:59'!
testNewSuite2test106
	| re |
	re _ Re on: '*a'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:00'!
testNewSuite2test107
	| re |
	re _ Re on: '(*)b'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:00'!
testNewSuite2test108
	| re |
	re _ Re on: 'abc)'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:00'!
testNewSuite2test109
	| re |
	re _ Re on: '(abc'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:00'!
testNewSuite2test11
	| re |
	re _ Re on: 'x{5,4}'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:01'!
testNewSuite2test110
	| re |
	re _ Re on: 'a**'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:01'!
testNewSuite2test111
	| re |
	re _ Re on: ')('.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:01'!
testNewSuite2test112
	| re |
	re _ Re on: '\1'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:01'!
testNewSuite2test113
	| re |
	re _ Re on: '\2'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:02'!
testNewSuite2test114
	| re |
	re _ Re on: '(a)|\2'.
	self should: [re compile] raise: Error.! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:03'!
testNewSuite2test115
	| re |
	re _ Re on: 'a[b-a]'.
	re
		beNotCaseSensitive.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:03'!
testNewSuite2test116
	| re |
	re _ Re on: 'a[]b'.
	re
		beNotCaseSensitive.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:03'!
testNewSuite2test117
	| re |
	re _ Re on: 'a['.
	re
		beNotCaseSensitive.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:03'!
testNewSuite2test118
	| re |
	re _ Re on: '*a'.
	re
		beNotCaseSensitive.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:03'!
testNewSuite2test119
	| re |
	re _ Re on: '(*)b'.
	re
		beNotCaseSensitive.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:04'!
testNewSuite2test12
	| re |
	re _ Re on: 'z{65536}'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:11'!
testNewSuite2test120
	| re |
	re _ Re on: 'abc)'.
	re
		beNotCaseSensitive.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:11'!
testNewSuite2test121
	| re |
	re _ Re on: '(abc'.
	re
		beNotCaseSensitive.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:12'!
testNewSuite2test122
	| re |
	re _ Re on: 'a**'.
	re
		beNotCaseSensitive.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:12'!
testNewSuite2test123
	| re |
	re _ Re on: ')('.
	re
		beNotCaseSensitive.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:12'!
testNewSuite2test124
	| re |
	re _ Re on: ':(?:'.
	self should: [re compile] raise: Error.! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:13'!
testNewSuite2test125
	| re |
	re _ Re on: '(?<%)b'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:14'!
testNewSuite2test126
	| re |
	re _ Re on: 'a(?{)b'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:14'!
testNewSuite2test127
	| re |
	re _ Re on: 'a(?{{})b'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:15'!
testNewSuite2test128
	| re |
	re _ Re on: 'a(?{}})b'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:15'!
testNewSuite2test129
	| re |
	re _ Re on: 'a(?{"{"})b'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:16'!
testNewSuite2test13
	| re |
	re _ Re on: '[abcd'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:19'!
testNewSuite2test130
	| re |
	re _ Re on: 'a(?{"{"}})b'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:19'!
testNewSuite2test131
	| re |
	re _ Re on: '(?(1?)a|b)'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:20'!
testNewSuite2test132
	| re |
	re _ Re on: '(?(1)a|b|c)'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:20'!
testNewSuite2test133
	| re |
	re _ Re on: '[a[:xyz:'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:20'!
testNewSuite2test134
	| re |
	re _ Re on: '(?<=x+)y'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:21'!
testNewSuite2test135
	| re |
	re _ Re on: 'a{37,17}'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test136
	| re re2 |
	re _ Re on: 'abc'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test137
	| re re2 |
	re _ Re on: 'abc'.
	re
		beStrangeOption;
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test138
	| re re2 |
	re _ Re on: 'abc'.
	re
		beStrangeOption;
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test139
	| re re2 |
	re _ Re on: '(a)bc(d)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcd') isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abcd';
		nextPutAll: '2'])) isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abcd';
		nextPutAll: '5'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:22'!
testNewSuite2test14
	| re |
	re _ Re on: '[\B]'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test140
	| re re2 |
	re _ Re on: '(.{20})'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcdefghijklmnopqrstuvwxyz') isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abcdefghijklmnopqrstuvwxyz';
		nextPutAll: '1'])) isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abcdefghijklmnopqrstuvwxyz';
		nextPutAll: '1'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test141
	| re re2 |
	re _ Re on: '(.{15})'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcdefghijklmnopqrstuvwxyz') isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abcdefghijklmnopqrstuvwxyz';
		nextPutAll: '1';
		nextPutAll: '1'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test142
	| re re2 |
	re _ Re on: '(.{16})'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcdefghijklmnopqrstuvwxyz') isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abcdefghijklmnopqrstuvwxyz';
		nextPutAll: '1';
		nextPutAll: '1'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test143
	| re re2 |
	re _ Re on: '^(a|(bc))de(f)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'adef';
		nextPutAll: '1';
		nextPutAll: '2';
		nextPutAll: '3';
		nextPutAll: '4'])) isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'bcdef';
		nextPutAll: '1';
		nextPutAll: '2';
		nextPutAll: '3';
		nextPutAll: '4'])) isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'adefghijk';
		nextPutAll: '0'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test144
	| re re2 |
	re _ Re on: '^abc\00def'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: 8r00 asCharacter;
		nextPutAll: 'def';
		nextPutAll: '0'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test145
	| re re2 |
	re _ Re on: 'word ((?:[a-zA-Z0-9]+ )((?:[a-zA-Z0-9]+ )((?:[a-zA-Z0-9]+ )((?:[a-zA-Z0-9]+ 
)((?:[a-zA-Z0-9]+ )((?:[a-zA-Z0-9]+ )((?:[a-zA-Z0-9]+ )((?:[a-zA-Z0-9]+ 
)?)?)?)?)?)?)?)?)?otherword'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test146
	| re re2 |
	re _ Re on: '.*X'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test147
	| re re2 |
	re _ Re on: '.*X'.
	re
		beStrangeOption;
		beDotIncludesNewline.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test148
	| re re2 |
	re _ Re on: '(.*X|^B)'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test149
	| re re2 |
	re _ Re on: '(.*X|^B)'.
	re
		beStrangeOption;
		beDotIncludesNewline.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:22'!
testNewSuite2test15
	| re |
	re _ Re on: '[z-a]'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test150
	| re re2 |
	re _ Re on: '(?s)(.*X|^B)'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test151
	| re re2 |
	re _ Re on: '(?s:.*X|^B)'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test152
	| re re2 |
	re _ Re on: '\Biss\B'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'Mississippi') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test153
	| re re2 |
	re _ Re on: '\Biss\B'.
	re
		beStrangeOption;
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'Mississippi') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test154
	| re re2 |
	re _ Re on: 'iss'.
	re
		beStrangeOption;
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'Mississippi') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test155
	| re re2 |
	re _ Re on: '\Biss\B'.
	re
		beStrangeOption;
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'Mississippi') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test156
	| re re2 |
	re _ Re on: '\Biss\B'.
	re
		beStrangeOption;
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'Mississippi') isNil not.
	re2 _ re copy beAnchored.
	self assert: (re2 search: 'Mississippi') isNil.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test157
	| re re2 |
	re _ Re on: '(?<=[Ms])iss'.
	re
		beStrangeOption;
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'Mississippi') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test158
	| re re2 |
	re _ Re on: '(?<=[Ms])iss'.
	re
		beStrangeOption;
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'Mississippi') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test159
	| re re2 |
	re _ Re on: '^iss'.
	re
		beStrangeOption;
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ississippi') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:22'!
testNewSuite2test16
	| re |
	re _ Re on: '^*'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test160
	| re re2 |
	re _ Re on: '.*iss'.
	re
		beStrangeOption;
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abciss';
		nextPut: Character cr;
		nextPutAll: 'xyzisspqr'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test161
	| re re2 |
	re _ Re on: '.i.'.
	re
		beStrangeOption;
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'Mississippi') isNil not.
	re2 _ re copy beAnchored.
	self assert: (re2 search: 'Mississippi') isNil not.
	self assert: (re search: 'Missouri river') isNil not.
	re2 _ re copy beAnchored.
	self assert: (re2 search: 'Missouri river') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test162
	| re re2 |
	re _ Re on: '^.is'.
	re
		beStrangeOption;
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'Mississippi') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test163
	| re re2 |
	re _ Re on: '^ab\n'.
	re
		beStrangeOption;
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'ab';
		nextPut: Character cr;
		nextPutAll: 'ab';
		nextPut: Character cr;
		nextPutAll: 'cd'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test164
	| re re2 |
	re _ Re on: '^ab\n'.
	re
		beMultiline;
		beStrangeOption;
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'ab';
		nextPut: Character cr;
		nextPutAll: 'ab';
		nextPut: Character cr;
		nextPutAll: 'cd'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test165
	| re re2 |
	re _ Re on: 'abc'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test166
	| re re2 |
	re _ Re on: 'abc|bac'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test167
	| re re2 |
	re _ Re on: '(abc|bac)'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test168
	| re re2 |
	re _ Re on: '(abc|(c|dc))'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test169
	| re re2 |
	re _ Re on: '(abc|(d|de)c)'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:22'!
testNewSuite2test17
	| re |
	re _ Re on: '(abc'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test170
	| re re2 |
	re _ Re on: 'a*'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test171
	| re re2 |
	re _ Re on: 'a+'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test172
	| re re2 |
	re _ Re on: '(baa|a+)'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test173
	| re re2 |
	re _ Re on: 'a{0,3}'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test174
	| re re2 |
	re _ Re on: 'baa{3,}'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test175
	| re re2 |
	re _ Re on: '"([^\\"]+|\\.)*"'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test176
	| re re2 |
	re _ Re on: '(abc|ab[cd])'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test177
	| re re2 |
	re _ Re on: '(a|.)'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test178
	| re re2 |
	re _ Re on: 'a|ba|\w'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test179
	| re re2 |
	re _ Re on: 'abc(?=pqr)'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:22'!
testNewSuite2test18
	| re |
	re _ Re on: '(?# abc'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test180
	| re re2 |
	re _ Re on: '...(?<=abc)'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test181
	| re re2 |
	re _ Re on: 'abc(?!!pqr)'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test182
	| re re2 |
	re _ Re on: 'ab.'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test183
	| re re2 |
	re _ Re on: 'ab[xyz]'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test184
	| re re2 |
	re _ Re on: 'abc*'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test185
	| re re2 |
	re _ Re on: 'ab.c*'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test186
	| re re2 |
	re _ Re on: 'a.c*'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test187
	| re re2 |
	re _ Re on: '.c*'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test188
	| re re2 |
	re _ Re on: 'ac*'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test189
	| re re2 |
	re _ Re on: '(a.c*|b.c*)'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:23'!
testNewSuite2test19
	| re |
	re _ Re on: '(?z)abc'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test190
	| re re2 |
	re _ Re on: 'a.c*|aba'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test191
	| re re2 |
	re _ Re on: '.+a'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test192
	| re re2 |
	re _ Re on: '(?=abcda)a.*'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test193
	| re re2 |
	re _ Re on: '(?=a)a.*'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test194
	| re re2 |
	re _ Re on: 'a(b)*'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test195
	| re re2 |
	re _ Re on: 'a\d*'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test196
	| re re2 |
	re _ Re on: 'ab\d*'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test197
	| re re2 |
	re _ Re on: 'a(\d)*'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test198
	| re re2 |
	re _ Re on: 'abcde{0,0}'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test199
	| re re2 |
	re _ Re on: 'ab\d+'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test2
	| re re2 |
	re _ Re on: 'abc'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
	self assert: (re search: 'defabc') isNil not.
	re2 _ re copy beAnchored.
	self assert: (re2 search: 'abc') isNil not.
	re2 _ re copy beAnchored.
	self assert: (re2 search: 'defabc') isNil.
	self assert: (re search: 'ABC') isNil.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test20
	| re re2 |
	re _ Re on: '.*b'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test200
	| re re2 |
	re _ Re on: 'a(?(1)b)'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test201
	| re re2 |
	re _ Re on: 'a(?(1)bag|big)'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test202
	| re re2 |
	re _ Re on: 'a(?(1)bag|big)*'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test203
	| re re2 |
	re _ Re on: 'a(?(1)bag|big)+'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test204
	| re re2 |
	re _ Re on: 'a(?(1)b..|b..)'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test205
	| re re2 |
	re _ Re on: 'ab\d{0}e'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 14:26'!
testNewSuite2test206
	| re |
	re _ Re on: 'a?b?'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a') isNil not.
	self assert: (re search: 'b') isNil not.
	self assert: (re search: 'ab') isNil not.
	self assert: (re search: '\') isNil not.
	self assert: (re search: 'xy') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 14:27'!
testNewSuite2test207
	| re |
	re _ Re on: '|-'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcd') isNil not.
	self assert: (re search: '-abc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:28'!
testNewSuite2test208
	| re |
	re _ Re on: 'a*(b+)(z)(z)'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aaaabbbbzzzz') isNil not.! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test209
	| re re2 |
	re _ Re on: '^.?abcd'.
	re
		beStrangeOption;
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test21
	| re re2 |
	re _ Re on: '.*?b'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test210
	| re re2 |
	re _ Re on: '\(             # ( at start
  (?:           # Non-capturing bracket
  (?>[^()]+)    # Either a sequence of non-brackets (no backtracking)
  |             # Or
  (?R)          # Recurse - i.e. nested bracketed string
  )*            # Zero or more contents
  \)            # Closing )
  '.
	re
		beExtended.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '(abcd)') isNil not.
	self assert: (re search: '(abcd)xyz') isNil not.
	self assert: (re search: 'xyz(abcd)') isNil not.
	self assert: (re search: '(ab(xy)cd)pqr') isNil not.
	self assert: (re search: '(ab(xycd)pqr') isNil not.
	self assert: (re search: '() abc ()') isNil not.
	self assert: (re search: '12(abcde(fsh)xyz(foo(bar))lmno)89') isNil not.
	self assert: (re search: 'abcd') isNil.
	self assert: (re search: 'abcd)') isNil.
	self assert: (re search: '(abcd') isNil.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test211
	| re re2 |
	re _ Re on: '\(  ( (?>[^()]+) | (?R) )* \) '.
	re
		beExtended;
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '(ab(xy)cd)pqr') isNil not.
	self assert: (re search: '1(abcd)(x(y)z)pqr') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test212
	| re re2 |
	re _ Re on: '\(  (?: (?>[^()]+) | (?R) ) \) '.
	re
		beExtended.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '(abcd)') isNil not.
	self assert: (re search: '(ab(xy)cd)') isNil not.
	self assert: (re search: '(a(b(c)d)e)') isNil not.
	self assert: (re search: '((ab))') isNil not.
	self assert: (re search: '()') isNil.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test213
	| re re2 |
	re _ Re on: '\(  (?: (?>[^()]+) | (?R) )? \) '.
	re
		beExtended.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '()') isNil not.
	self assert: (re search: '12(abcde(fsh)xyz(foo(bar))lmno)89') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test214
	| re re2 |
	re _ Re on: '\(  ( (?>[^()]+) | (?R) )* \) '.
	re
		beExtended.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '(ab(xy)cd)') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test215
	| re re2 |
	re _ Re on: '\( ( ( (?>[^()]+) | (?R) )* ) \) '.
	re
		beExtended.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '(ab(xy)cd)') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test216
	| re re2 |
	re _ Re on: '\( (123)? ( ( (?>[^()]+) | (?R) )* ) \) '.
	re
		beExtended.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '(ab(xy)cd)') isNil not.
	self assert: (re search: '(123ab(xy)cd)') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test217
	| re re2 |
	re _ Re on: '\( ( (123)? ( (?>[^()]+) | (?R) )* ) \) '.
	re
		beExtended.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '(ab(xy)cd)') isNil not.
	self assert: (re search: '(123ab(xy)cd)') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test218
	| re re2 |
	re _ Re on: '\( (((((((((( ( (?>[^()]+) | (?R) )* )))))))))) \) '.
	re
		beExtended.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '(ab(xy)cd)') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test219
	| re re2 |
	re _ Re on: '\( ( ( (?>[^()<>]+) | ((?>[^()]+)) | (?R) )* ) \) '.
	re
		beExtended.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '(abcd(xyz<p>qrs)123)') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test22
	| re re2 |
	re _ Re on: 'cat|dog|elephant'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'this sentence eventually mentions a cat') isNil not.
	self assert: (re search: 'this sentences rambles on and on for a while and then reaches elephant') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test220
	| re re2 |
	re _ Re on: '\( ( ( (?>[^()]+) | ((?R)) )* ) \) '.
	re
		beExtended.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '(ab(cd)ef)') isNil not.
	self assert: (re search: '(ab(cd(ef)gh)ij)') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test221
	| re re2 |
	re _ Re on: '^[[:alnum:]]'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test222
	| re re2 |
	re _ Re on: '^[[:alpha:]]'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test223
	| re re2 |
	re _ Re on: '^[[:ascii:]]'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test224
	| re re2 |
	re _ Re on: '^[[:cntrl:]]'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test225
	| re re2 |
	re _ Re on: '^[[:digit:]]'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test226
	| re re2 |
	re _ Re on: '^[[:graph:]]'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test227
	| re re2 |
	re _ Re on: '^[[:lower:]]'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test228
	| re re2 |
	re _ Re on: '^[[:print:]]'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test229
	| re re2 |
	re _ Re on: '^[[:punct:]]'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test23
	| re re2 |
	re _ Re on: 'cat|dog|elephant'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'this sentence eventually mentions a cat') isNil not.
	self assert: (re search: 'this sentences rambles on and on for a while and then reaches elephant') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test230
	| re re2 |
	re _ Re on: '^[[:space:]]'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test231
	| re re2 |
	re _ Re on: '^[[:upper:]]'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test232
	| re re2 |
	re _ Re on: '^[[:xdigit:]]'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test233
	| re re2 |
	re _ Re on: '^[[:word:]]'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test234
	| re re2 |
	re _ Re on: '^[[:^cntrl:]]'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test235
	| re re2 |
	re _ Re on: '^[12[:^digit:]]'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test236
	| re re2 |
	re _ Re on: '[01[:alpha:]%]'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:30'!
testNewSuite2test237
	| re |
	re _ Re on: '[[.ch.]]'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:30'!
testNewSuite2test238
	| re |
	re _ Re on: '[[=ch=]]'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:30'!
testNewSuite2test239
	| re |
	re _ Re on: '[[:rhubarb:]]'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test24
	| re re2 |
	re _ Re on: 'cat|dog|elephant'.
	re
		beNotCaseSensitive;
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'this sentence eventually mentions a CAT cat') isNil not.
	self assert: (re search: 'this sentences rambles on and on for a while to elephant ElePhant') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test240
	| re re2 |
	re _ Re on: '[[:upper:]]'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'A') isNil not.
	self assert: (re search: 'a') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test241
	| re re2 |
	re _ Re on: '[[:lower:]]'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'A') isNil not.
	self assert: (re search: 'a') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test242
	| re re2 |
	re _ Re on: '((?-i)[[:lower:]])[[:lower:]]'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ab') isNil not.
	self assert: (re search: 'aB') isNil not.
	self assert: (re search: 'Ab') isNil.
	self assert: (re search: 'AB') isNil.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:31'!
testNewSuite2test243
	| re |
	re _ Re on: '[\200-\410]'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:32'!
testNewSuite2test244
	| re |
	re _ Re on: '^(?(0)f|b)oo'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test245
	| re re2 |
	re _ Re on: '(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\w+)\s+(\270)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '\O900 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26 27 28 29 30 31 32 33 34 35 36 37 38 39 40 41 42 43 44 45 46 47 48 49 50 51 52 53 54 55 56 57 58 59 60 61 62 63 64 65 66 67 68 69 70 71 72 73 74 75 76 77 78 79 80 81 82 83 84 85 86 87 88 89 90 91 92 93 94 95 96 97 98 99 100 101 102 103 104 105 106 107 108 109 110 111 112 113 114 115 116 117 118 119 120 121 122 123 124 125 126 127 128 129 130 131 132 133 134 135 136 137 138 139 140 141 142 143 144 145 146 147 148 149 150 151 152 153 154 155 156 157 158 159 160 161 162 163 164 165 166 167 168 169 170 171 172 173 174 175 176 177 178 179 180 181 182 183 184 185 186 187 188 189 190 191 192 193 194 195 196 197 198 199 200 201 202 203 204 205 206 207 208 209 210 211 212 213 214 215 216 217 218 219 220 221 222 223 224 225 226 227 228 229 230 231 232 233 234 235 236 237 238 239 240 241 242 243 244 245 246 247 248 249 250 251 252 253 254 255 256 257 258 259 260 261 262 263 264 265 266 267 268 269 ABC ABC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test246
	| re re2 |
	re _ Re on: '(main(O)?)+'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'mainmain') isNil not.
	self assert: (re search: 'mainOmain') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:31'!
testNewSuite2test247
	| re re2 |
	re _ Re on: ' End of testinput2 '.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 00:28'!
testNewSuite2test248
	| re |
	re _ Re on: 'This one''s here because of the large output vector needed.'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 00:28'!
testNewSuite2test249
	| re |
	re _ Re on: '(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\d+(?:\s|$))(\w+)\s+(\270)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '\O900 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26 27 28 29 30 31 32 33 34 35 36 37 38 39 40 41 42 43 44 45 46 47 48 49 50 51 52 53 54 55 56 57 58 59 60 61 62 63 64 65 66 67 68 69 70 71 72 73 74 75 76 77 78 79 80 81 82 83 84 85 86 87 88 89 90 91 92 93 94 95 96 97 98 99 100 101 102 103 104 105 106 107 108 109 110 111 112 113 114 115 116 117 118 119 120 121 122 123 124 125 126 127 128 129 130 131 132 133 134 135 136 137 138 139 140 141 142 143 144 145 146 147 148 149 150 151 152 153 154 155 156 157 158 159 160 161 162 163 164 165 166 167 168 169 170 171 172 173 174 175 176 177 178 179 180 181 182 183 184 185 186 187 188 189 190 191 192 193 194 195 196 197 198 199 200 201 202 203 204 205 206 207 208 209 210 211 212 213 214 215 216 217 218 219 220 221 222 223 224 225 226 227 228 229 230 231 232 233 234 235 236 237 238 239 240 241 242 243 244 245 246 247 248 249 250 251 252 253 254 255 256 257 258 259 260 261 262 263 264 265 266 267 268 269 ABC ABC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test25
	| re re2 |
	re _ Re on: 'a|[bcd]'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 00:28'!
testNewSuite2test250
	| re |
	re _ Re on: 'This one''s here because Perl does this differently and PCRE can''t at present'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 00:28'!
testNewSuite2test251
	| re |
	re _ Re on: '(main(O)?)+'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'mainmain') isNil not.
	self assert: (re search: 'mainOmain') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 00:28'!
testNewSuite2test252
	| re |
	re _ Re on: ' End of testinput2 '.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test26
	| re re2 |
	re _ Re on: '(a|[^\dZ])'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test27
	| re re2 |
	re _ Re on: '(a|b)*[\s]'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:32'!
testNewSuite2test28
	| re |
	re _ Re on: '(ab\2)'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:34'!
testNewSuite2test29
	| re |
	re _ Re on: '{4,5}abc'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test3
	| re re2 |
	re _ Re on: '^abc'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
	re2 _ re copy beAnchored.
	self assert: (re2 search: 'abc') isNil not.
	self assert: (re search: 'defabc') isNil.
	re2 _ re copy beAnchored.
	self assert: (re2 search: 'defabc') isNil.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test30
	| re re2 |
	re _ Re on: '(a)(b)(c)\2'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcb') isNil not.
	self assert: (re search: '\O0abcb') isNil not.
	self assert: (re search: '\O3abcb') isNil not.
	self assert: (re search: '\O6abcb') isNil not.
	self assert: (re search: '\O9abcb') isNil not.
	self assert: (re search: '\O12abcb') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test31
	| re re2 |
	re _ Re on: '(a)bc|(a)(b)\2'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
	self assert: (re search: '\O0abc') isNil not.
	self assert: (re search: '\O3abc') isNil not.
	self assert: (re search: '\O6abc') isNil not.
	self assert: (re search: 'aba') isNil not.
	self assert: (re search: '\O0aba') isNil not.
	self assert: (re search: '\O3aba') isNil not.
	self assert: (re search: '\O6aba') isNil not.
	self assert: (re search: '\O9aba') isNil not.
	self assert: (re search: '\O12aba') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test32
	| re re2 |
	re _ Re on: 'abc$'.
	re
		beDollarEndOnly.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: Character cr])) isNil.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: Character cr;
		nextPutAll: 'def'])) isNil.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:34'!
testNewSuite2test33
	| re |
	re _ Re on: '(a)(b)(c)(d)(e)\6'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test34
	| re re2 |
	re _ Re on: 'the quick brown fox'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'the quick brown fox') isNil not.
	self assert: (re search: 'this is a line with the quick brown fox') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test35
	| re re2 |
	re _ Re on: 'the quick brown fox'.
	re
		beAnchored.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'the quick brown fox') isNil not.
	self assert: (re search: 'this is a line with the quick brown fox') isNil.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:36'!
testNewSuite2test36
	| re |
	re _ Re on: 'ab(?z)cd'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test37
	| re re2 |
	re _ Re on: '^abc|def'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcdef') isNil not.
	re2 _ re copy beNotBeginningOfLine.
	self assert: (re2 search: 'abcdef') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test38
	| re re2 |
	re _ Re on: '.*((abc)$|(def))'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'defabc') isNil not.
	re2 _ re copy beNotEndOfLine.
	self assert: (re2 search: 'defabc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test39
	| re re2 |
	re _ Re on: 'abc'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test4
	| re re2 |
	re _ Re on: 'a+bc'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test40
	| re re2 |
	re _ Re on: '^abc|def'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcdef') isNil not.
	re2 _ re copy beNotBeginningOfLine.
	self assert: (re2 search: 'abcdef') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test41
	| re re2 |
	re _ Re on: '.*((abc)$|(def))'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'defabc') isNil not.
	re2 _ re copy beNotEndOfLine.
	self assert: (re2 search: 'defabc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test42
	| re re2 |
	re _ Re on: 'the quick brown fox'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'the quick brown fox') isNil not.
	self assert: (re search: 'The Quick Brown Fox') isNil.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test43
	| re re2 |
	re _ Re on: 'the quick brown fox'.
	re
		beStrangeOption;
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'the quick brown fox') isNil not.
	self assert: (re search: 'The Quick Brown Fox') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test44
	| re re2 |
	re _ Re on: 'abc.def'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: Character cr;
		nextPutAll: 'def'])) isNil.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test45
	| re re2 |
	re _ Re on: 'abc$'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: Character cr])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:36'!
testNewSuite2test46
	| re |
	re _ Re on: '(abc)\2'.
	re
		beStrangeOption.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 14:28'!
testNewSuite2test47
	| re |
	re _ Re on: '(abc\1)'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:37'!
testNewSuite2test48
	| re |
	re _ Re on: ')'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:37'!
testNewSuite2test49
	| re |
	re _ Re on: 'a[]b'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test5
	| re re2 |
	re _ Re on: 'a*bc'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test50
	| re re2 |
	re _ Re on: '[^aeiou ]{3,}'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'co-processors, and for') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test51
	| re re2 |
	re _ Re on: '<.*>'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc<def>ghi<klm>nop') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test52
	| re re2 |
	re _ Re on: '<.*?>'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc<def>ghi<klm>nop') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test53
	| re re2 |
	re _ Re on: '<.*>'.
	re
		beNotGreedy.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc<def>ghi<klm>nop') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test54
	| re re2 |
	re _ Re on: '<.*>(?U)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc<def>ghi<klm>nop') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test55
	| re re2 |
	re _ Re on: '<.*?>'.
	re
		beNotGreedy.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc<def>ghi<klm>nop') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test56
	| re re2 |
	re _ Re on: '={3,}'.
	re
		beNotGreedy.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc========def') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test57
	| re re2 |
	re _ Re on: '(?U)={3,}?'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc========def') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test58
	| re re2 |
	re _ Re on: '(?<!!bar|cattle)foo'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'foo') isNil not.
	self assert: (re search: 'catfoo') isNil not.
	self assert: (re search: 'the barfoo') isNil.
	self assert: (re search: 'and cattlefoo') isNil.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:38'!
testNewSuite2test59
	| re |
	re _ Re on: '(?<=a+)b'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test6
	| re re2 |
	re _ Re on: 'a{3}bc'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:38'!
testNewSuite2test60
	| re |
	re _ Re on: '(?<=aaa|b{0,3})b'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:39'!
testNewSuite2test61
	| re |
	re _ Re on: '(?<!!(foo)a\1)bar'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test62
	| re re2 |
	re _ Re on: '(?i)abc'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test63
	| re re2 |
	re _ Re on: '(a|(?m)a)'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test64
	| re re2 |
	re _ Re on: '(?i)^1234'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test65
	| re re2 |
	re _ Re on: '(^b|(?i)^d)'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test66
	| re re2 |
	re _ Re on: '(?s).*'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test67
	| re re2 |
	re _ Re on: '[abcd]'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test68
	| re re2 |
	re _ Re on: '(?i)[abcd]'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test69
	| re re2 |
	re _ Re on: '(?m)[xy]|(b|c)'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test7
	| re re2 |
	re _ Re on: '(abc|a+z)'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test70
	| re re2 |
	re _ Re on: '(^a|^b)'.
	re
		beMultiline.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test71
	| re re2 |
	re _ Re on: '(?i)(^a|^b)'.
	re
		beMultiline.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:39'!
testNewSuite2test72
	| re |
	re _ Re on: '(a)(?(1)a|b|c)'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:39'!
testNewSuite2test73
	| re |
	re _ Re on: '(?(?=a)a|b|c)'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:40'!
testNewSuite2test74
	| re |
	re _ Re on: '(?(1a)'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:40'!
testNewSuite2test75
	| re |
	re _ Re on: '(?(?i))'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:40'!
testNewSuite2test76
	| re |
	re _ Re on: '(?(abc))'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:40'!
testNewSuite2test77
	| re |
	re _ Re on: '(?(?<ab))'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test78
	| re re2 |
	re _ Re on: '((?s)blah)\s+\1'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test79
	| re re2 |
	re _ Re on: '((?i)blah)\s+\1'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test8
	| re re2 |
	re _ Re on: '^abc$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'def';
		nextPut: Character cr;
		nextPutAll: 'abc'])) isNil.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test80
	| re re2 |
	re _ Re on: '((?i)b)'.
	re
		beStrangeOption;
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test81
	| re re2 |
	re _ Re on: '(a*b|(?i:c*(?-i)d))'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test82
	| re re2 |
	re _ Re on: 'a$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a') isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'a';
		nextPut: Character cr])) isNil not.
	re2 _ re copy beNotEndOfLine.
	self assert: (re2 search: 'a') isNil.
	re2 _ re copy beNotEndOfLine.
	self assert: (re2 search: (String streamContents: [:s | s
		nextPutAll: 'a';
		nextPut: Character cr])) isNil.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test83
	| re re2 |
	re _ Re on: 'a$'.
	re
		beMultiline.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a') isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'a';
		nextPut: Character cr])) isNil not.
	re2 _ re copy beNotEndOfLine.
	self assert: (re2 search: (String streamContents: [:s | s
		nextPutAll: 'a';
		nextPut: Character cr])) isNil not.
	re2 _ re copy beNotEndOfLine.
	self assert: (re2 search: 'a') isNil.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test84
	| re re2 |
	re _ Re on: '\Aabc'.
	re
		beMultiline.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test85
	| re re2 |
	re _ Re on: '^abc'.
	re
		beMultiline;
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test86
	| re re2 |
	re _ Re on: '^((a+)(?U)([ab]+)(?-U)([bc]+)(\w*))'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aaaaabbbbbcccccdef') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test87
	| re re2 |
	re _ Re on: '(?<=foo)[ab]'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test88
	| re re2 |
	re _ Re on: '(?<!!foo)(alpha|omega)'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test89
	| re re2 |
	re _ Re on: '(?!!alphabet)[ab]'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:40'!
testNewSuite2test9
	| re |
	re _ Re on: 'ab\gdef'.
	re
		beExtra.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test90
	| re re2 |
	re _ Re on: '(?<=foo\n)^bar'.
	re
		beMultiline.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test91
	| re re2 |
	re _ Re on: '(?>^abc)'.
	re
		beMultiline.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'def';
		nextPut: Character cr;
		nextPutAll: 'abc'])) isNil not.
	self assert: (re search: 'defabc') isNil.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:41'!
testNewSuite2test92
	| re |
	re _ Re on: '(?<=ab(c+)d)ef'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:41'!
testNewSuite2test93
	| re |
	re _ Re on: '(?<=ab(?<=c+)d)ef'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 13:41'!
testNewSuite2test94
	| re |
	re _ Re on: '(?<=ab(c|de)f)g'.
	self should: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test95
	| re re2 |
	re _ Re on: 'The next three are in testinput2 because they have variable length branches'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test96
	| re re2 |
	re _ Re on: '(?<=bullock|donkey)-cart'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'the bullock-cart') isNil not.
	self assert: (re search: 'a donkey-cart race') isNil not.
	self assert: (re search: 'cart') isNil.
	self assert: (re search: 'horse-and-cart') isNil.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test97
	| re re2 |
	re _ Re on: '(?<=ab(?i)x|y|z)'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test98
	| re re2 |
	re _ Re on: '(?>.*)(?<=(abcd)|(xyz))'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'alphabetabcd') isNil not.
	self assert: (re search: 'endingxyz') isNil not.
! !

!ReTest methodsFor: 'newTestSuite2' stamp: 'acg 8/11/2002 12:30'!
testNewSuite2test99
	| re re2 |
	re _ Re on: '(?<=ab(?i)x(?-i)y|(?i)z|b)ZZ'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abxyZZ') isNil not.
	self assert: (re search: 'abXyZZ') isNil not.
	self assert: (re search: 'ZZZ') isNil not.
	self assert: (re search: 'zZZ') isNil not.
	self assert: (re search: 'bZZ') isNil not.
	self assert: (re search: 'BZZ') isNil not.
	self assert: (re search: 'ZZ') isNil.
	self assert: (re search: 'abXYZZ') isNil.
	self assert: (re search: 'zzz') isNil.
	self assert: (re search: 'bzz') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test1
	| re re2 |
	re _ Re on: '(?<!!bar)foo'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'foo') isNil not.
	self assert: (re search: 'catfood') isNil not.
	self assert: (re search: 'arfootle') isNil not.
	self assert: (re search: 'rfoosh') isNil not.
	self assert: (re search: 'barfoo') isNil.
	self assert: (re search: 'towbarfoo') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test10
	| re re2 |
	re _ Re on: '((?>\d+))(\w)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '12345a') isNil not.
	self assert: (re search: '12345+') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test100
	| re re2 |
	re _ Re on: 'a[^-b]c'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'adc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 13:43'!
testNewSuite3test101
	| re |
	re _ Re on: 'a[^]b]c'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'adc') isNil not.
	self assert: (re search: 'a-c') isNil not.
	self assert: (re search: 'a]c') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test102
	| re re2 |
	re _ Re on: '\ba\b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a-') isNil not.
	self assert: (re search: '-a') isNil not.
	self assert: (re search: '-a-') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test103
	| re re2 |
	re _ Re on: '\by\b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'xy') isNil.
	self assert: (re search: 'yz') isNil.
	self assert: (re search: 'xyz') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test104
	| re re2 |
	re _ Re on: '\Ba\B'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a-') isNil.
	self assert: (re search: '-a') isNil.
	self assert: (re search: '-a-') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test105
	| re re2 |
	re _ Re on: '\By\b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'xy') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test106
	| re re2 |
	re _ Re on: '\by\B'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'yz') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test107
	| re re2 |
	re _ Re on: '\By\B'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'xyz') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test108
	| re re2 |
	re _ Re on: '\w'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 13:45'!
testNewSuite3test109
	| re |
	re _ Re on: '\W'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '-') isNil not.
	self assert: (re search: 'a') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test11
	| re re2 |
	re _ Re on: '(?>a+)b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aaab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test110
	| re re2 |
	re _ Re on: 'a\sb'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a b') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 14:29'!
testNewSuite3test111
	| re |
	re _ Re on: 'a\Sb'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a-b') isNil not.
	self assert: (re search: 'a b') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test112
	| re re2 |
	re _ Re on: '\d'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '1') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 13:46'!
testNewSuite3test113
	| re |
	re _ Re on: '\D'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '-') isNil not.
	self assert: (re search: '1') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test114
	| re re2 |
	re _ Re on: '[\w]'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 13:46'!
testNewSuite3test115
	| re |
	re _ Re on: '[\W]'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '-') isNil not.
	self assert: (re search: 'a') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test116
	| re re2 |
	re _ Re on: 'a[\s]b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a b') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 13:46'!
testNewSuite3test117
	| re |
	re _ Re on: 'a[\S]b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a-b') isNil not.
	self assert: (re search: 'a b') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test118
	| re re2 |
	re _ Re on: '[\d]'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '1') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 13:46'!
testNewSuite3test119
	| re |
	re _ Re on: '[\D]'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '-') isNil not.
	self assert: (re search: '1') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test12
	| re re2 |
	re _ Re on: '((?>a+)b)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aaab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test120
	| re re2 |
	re _ Re on: 'ab|cd'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
	self assert: (re search: 'abcd') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test121
	| re re2 |
	re _ Re on: '()ef'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'def') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test122
	| re re2 |
	re _ Re on: '$b'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test123
	| re re2 |
	re _ Re on: 'a\(b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a(b') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test124
	| re re2 |
	re _ Re on: 'a\(*b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ab') isNil not.
	self assert: (re search: 'a((b') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 13:47'!
testNewSuite3test125
	| re |
	re _ Re on: 'a\\b'.
	self shouldnt: [re compile] raise: Error.
	self assert: ('a\b') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test126
	| re re2 |
	re _ Re on: '((a))'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test127
	| re re2 |
	re _ Re on: '(a)b(c)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test128
	| re re2 |
	re _ Re on: 'a+b+c'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aabbabc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test129
	| re re2 |
	re _ Re on: 'a{1,}b{1,}c'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aabbabc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test13
	| re re2 |
	re _ Re on: '(?>(a+))b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aaab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test130
	| re re2 |
	re _ Re on: 'a.+?c'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcabc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test131
	| re re2 |
	re _ Re on: '(a+|b)*'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test132
	| re re2 |
	re _ Re on: '(a+|b){0,}'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test133
	| re re2 |
	re _ Re on: '(a+|b)+'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test134
	| re re2 |
	re _ Re on: '(a+|b){1,}'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test135
	| re re2 |
	re _ Re on: '(a+|b)?'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test136
	| re re2 |
	re _ Re on: '(a+|b){0,1}'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test137
	| re re2 |
	re _ Re on: '[^ab]*'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'cde') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test138
	| re re2 |
	re _ Re on: 'abc'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'b') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test139
	| re re2 |
	re _ Re on: '/a*/'.
	re
		beStrangeOption;
		beStrangeOption;
		beStrangeOption;
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test14
	| re re2 |
	re _ Re on: '(?>b)+'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aaabbbccc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test140
	| re re2 |
	re _ Re on: '([abc])*d'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abbbcd') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test141
	| re re2 |
	re _ Re on: '([abc])*bcd'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcd') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test142
	| re re2 |
	re _ Re on: 'a|b|c|d|e'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'e') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test143
	| re re2 |
	re _ Re on: '(a|b|c|d|e)f'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ef') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test144
	| re re2 |
	re _ Re on: 'abcd*efg'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcdefg') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test145
	| re re2 |
	re _ Re on: 'ab*'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'xabyabbbz') isNil not.
	self assert: (re search: 'xayabbbz') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test146
	| re re2 |
	re _ Re on: '(ab|cd)e'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcde') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test147
	| re re2 |
	re _ Re on: '[abhgefdc]ij'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'hij') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test148
	| re re2 |
	re _ Re on: '^(ab|cd)e'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test149
	| re re2 |
	re _ Re on: '(abc|)ef'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcdef') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test15
	| re re2 |
	re _ Re on: '(?>a+|b+|c+)*c'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aaabbbbccccd') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test150
	| re re2 |
	re _ Re on: '(a|b)c*d'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcd') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test151
	| re re2 |
	re _ Re on: '(ab|ab*)bc'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test152
	| re re2 |
	re _ Re on: 'a([bc]*)c*'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test153
	| re re2 |
	re _ Re on: 'a([bc]*)(c*d)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcd') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test154
	| re re2 |
	re _ Re on: 'a([bc]+)(c*d)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcd') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test155
	| re re2 |
	re _ Re on: 'a([bc]*)(c+d)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcd') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test156
	| re re2 |
	re _ Re on: 'a[bcd]*dcdcde'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'adcdcde') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test157
	| re re2 |
	re _ Re on: 'a[bcd]+dcdcde'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcde') isNil.
	self assert: (re search: 'adcdcde') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test158
	| re re2 |
	re _ Re on: '(ab|a)b*c'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test159
	| re re2 |
	re _ Re on: '((a)(b)c)(d)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcd') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test16
	| re re2 |
	re _ Re on: '((?>[^()]+)|\([^()]*\))+'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '((abc(ade)ufh()()x') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test160
	| re re2 |
	re _ Re on: '[a-zA-Z_][a-zA-Z0-9_]*'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'alpha') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test161
	| re re2 |
	re _ Re on: '^a(bc+|b[eh])g|.h$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abh') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test162
	| re re2 |
	re _ Re on: '(bc+d$|ef*g.|h?i(j|k))'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'effgz') isNil not.
	self assert: (re search: 'ij') isNil not.
	self assert: (re search: 'reffgz') isNil not.
	self assert: (re search: 'effg') isNil.
	self assert: (re search: 'bcdd') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test163
	| re re2 |
	re _ Re on: '((((((((((a))))))))))'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test164
	| re re2 |
	re _ Re on: '((((((((((a))))))))))\10'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aa') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test165
	| re re2 |
	re _ Re on: '(((((((((a)))))))))'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test166
	| re re2 |
	re _ Re on: 'multiple words of text'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aa') isNil.
	self assert: (re search: 'uh-uh') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test167
	| re re2 |
	re _ Re on: 'multiple words'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'multiple words, yeah') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test168
	| re re2 |
	re _ Re on: '(.*)c(.*)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcde') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test169
	| re re2 |
	re _ Re on: '\((.*), (.*)\)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '(a, b)') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test17
	| re re2 |
	re _ Re on: '\(((?>[^()]+)|\([^()]+\))+\)'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '(abc)') isNil not.
	self assert: (re search: '(abc(def)xyz)') isNil not.
	self assert: (re search: '((()aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test170
	| re re2 |
	re _ Re on: '[k]'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test171
	| re re2 |
	re _ Re on: 'abcd'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcd') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test172
	| re re2 |
	re _ Re on: 'a(bc)d'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcd') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test173
	| re re2 |
	re _ Re on: 'a[-]?c'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ac') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test174
	| re re2 |
	re _ Re on: '(abc)\1'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcabc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test175
	| re re2 |
	re _ Re on: '([a-c]*)\1'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcabc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 13:49'!
testNewSuite3test176
	| re |
	re _ Re on: '(a)|\1'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a') isNil not.
	self assert: (re search: 'ab') isNil not.
	self assert: (re search: 'x') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test177
	| re re2 |
	re _ Re on: '(([a-c])b*?\2)*'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ababbbcbc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test178
	| re re2 |
	re _ Re on: '(([a-c])b*?\2){3}'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ababbbcbc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test179
	| re re2 |
	re _ Re on: '((\3|b)\2(a)x)+'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aaaxabaxbaaxbbax') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test18
	| re re2 |
	re _ Re on: 'a(?-i)b'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ab') isNil not.
	self assert: (re search: 'Ab') isNil.
	self assert: (re search: 'aB') isNil.
	self assert: (re search: 'AB') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test180
	| re re2 |
	re _ Re on: '((\3|b)\2(a)){2,}'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'bbaababbabaaaaabbaaaabba') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test181
	| re re2 |
	re _ Re on: 'abc'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABC') isNil not.
	self assert: (re search: 'XABCY') isNil not.
	self assert: (re search: 'ABABC') isNil not.
	self assert: (re search: 'aaxabxbaxbbx') isNil.
	self assert: (re search: 'XBC') isNil.
	self assert: (re search: 'AXC') isNil.
	self assert: (re search: 'ABX') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test182
	| re re2 |
	re _ Re on: 'ab*c'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test183
	| re re2 |
	re _ Re on: 'ab*bc'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABC') isNil not.
	self assert: (re search: 'ABBC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test184
	| re re2 |
	re _ Re on: 'ab*?bc'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABBBBC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test185
	| re re2 |
	re _ Re on: 'ab{0,}?bc'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABBBBC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test186
	| re re2 |
	re _ Re on: 'ab+?bc'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABBC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test187
	| re re2 |
	re _ Re on: 'ab+bc'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABC') isNil.
	self assert: (re search: 'ABQ') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test188
	| re re2 |
	re _ Re on: 'ab{1,}bc'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test189
	| re re2 |
	re _ Re on: 'ab+bc'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABBBBC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test19
	| re re2 |
	re _ Re on: '(a (?x)b c)d e'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a bcd e') isNil not.
	self assert: (re search: 'a b cd e') isNil.
	self assert: (re search: 'abcd e') isNil.
	self assert: (re search: 'a bcde') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test190
	| re re2 |
	re _ Re on: 'ab{1,}?bc'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABBBBC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test191
	| re re2 |
	re _ Re on: 'ab{1,3}?bc'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABBBBC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test192
	| re re2 |
	re _ Re on: 'ab{3,4}?bc'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABBBBC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test193
	| re re2 |
	re _ Re on: 'ab{4,5}?bc'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABQ') isNil.
	self assert: (re search: 'ABBBBC') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test194
	| re re2 |
	re _ Re on: 'ab??bc'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABBC') isNil not.
	self assert: (re search: 'ABC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test195
	| re re2 |
	re _ Re on: 'ab{0,1}?bc'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test196
	| re re2 |
	re _ Re on: 'ab??bc'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test197
	| re re2 |
	re _ Re on: 'ab??c'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test198
	| re re2 |
	re _ Re on: 'ab{0,1}?c'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test199
	| re re2 |
	re _ Re on: '^abc$'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABC') isNil not.
	self assert: (re search: 'ABBBBC') isNil.
	self assert: (re search: 'ABCC') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test2
	| re re2 |
	re _ Re on: '\w{3}(?<!!bar)foo'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'catfood') isNil not.
	self assert: (re search: 'foo') isNil.
	self assert: (re search: 'barfoo') isNil.
	self assert: (re search: 'towbarfoo') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test20
	| re re2 |
	re _ Re on: '(a b(?x)c d (?-x)e f)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a bcde f') isNil not.
	self assert: (re search: 'abcdef') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test200
	| re re2 |
	re _ Re on: '^abc'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABCC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test201
	| re re2 |
	re _ Re on: '^abc$'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test202
	| re re2 |
	re _ Re on: 'abc$'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'AABC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test203
	| re re2 |
	re _ Re on: '^'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test204
	| re re2 |
	re _ Re on: '$'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test205
	| re re2 |
	re _ Re on: 'a.c'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABC') isNil not.
	self assert: (re search: 'AXC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test206
	| re re2 |
	re _ Re on: 'a.*?c'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'AXYZC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 13:50'!
testNewSuite3test207
	| re |
	re _ Re on: 'a.*c'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'AABC') isNil not.
	self assert: (re search: 'AXYZD') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test208
	| re re2 |
	re _ Re on: 'a[bc]d'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABD') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test209
	| re re2 |
	re _ Re on: 'a[b-d]e'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ACE') isNil not.
	self assert: (re search: 'ABC') isNil.
	self assert: (re search: 'ABD') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test21
	| re re2 |
	re _ Re on: '(a(?i)b)c'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
	self assert: (re search: 'aBc') isNil not.
	self assert: (re search: 'abC') isNil.
	self assert: (re search: 'aBC') isNil.
	self assert: (re search: 'Abc') isNil.
	self assert: (re search: 'ABc') isNil.
	self assert: (re search: 'ABC') isNil.
	self assert: (re search: 'AbC') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test210
	| re re2 |
	re _ Re on: 'a[b-d]'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'AAC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test211
	| re re2 |
	re _ Re on: 'a[-b]'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'A-') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test212
	| re re2 |
	re _ Re on: 'a[b-]'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'A-') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test213
	| re re2 |
	re _ Re on: 'a]'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'A]') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test214
	| re re2 |
	re _ Re on: 'a[]]b'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'A]B') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test215
	| re re2 |
	re _ Re on: 'a[^bc]d'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'AED') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test216
	| re re2 |
	re _ Re on: 'a[^-b]c'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ADC') isNil not.
	self assert: (re search: 'ABD') isNil.
	self assert: (re search: 'A-C') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test217
	| re re2 |
	re _ Re on: 'a[^]b]c'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ADC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test218
	| re re2 |
	re _ Re on: 'ab|cd'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABC') isNil not.
	self assert: (re search: 'ABCD') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test219
	| re re2 |
	re _ Re on: '()ef'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'DEF') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test22
	| re re2 |
	re _ Re on: 'a(?i:b)c'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
	self assert: (re search: 'aBc') isNil not.
	self assert: (re search: 'ABC') isNil.
	self assert: (re search: 'abC') isNil.
	self assert: (re search: 'aBC') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test220
	| re re2 |
	re _ Re on: '$b'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'A]C') isNil.
	self assert: (re search: 'B') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test221
	| re re2 |
	re _ Re on: 'a\(b'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'A(B') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test222
	| re re2 |
	re _ Re on: 'a\(*b'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'AB') isNil not.
	self assert: (re search: 'A((B') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 13:52'!
testNewSuite3test223
	| re re2 |
	re _ Re on: 'a\\b'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	re2 _ re copy beNotBeginningOfLine.
	self assert: (re2 search: 'A') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test224
	| re re2 |
	re _ Re on: '((a))'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test225
	| re re2 |
	re _ Re on: '(a)b(c)'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test226
	| re re2 |
	re _ Re on: 'a+b+c'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'AABBABC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test227
	| re re2 |
	re _ Re on: 'a{1,}b{1,}c'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'AABBABC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test228
	| re re2 |
	re _ Re on: 'a.+?c'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABCABC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test229
	| re re2 |
	re _ Re on: 'a.*?c'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABCABC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test23
	| re re2 |
	re _ Re on: 'a(?i:b)*c'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aBc') isNil not.
	self assert: (re search: 'aBBc') isNil not.
	self assert: (re search: 'aBC') isNil.
	self assert: (re search: 'aBBC') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test230
	| re re2 |
	re _ Re on: 'a.{0,5}?c'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABCABC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test231
	| re re2 |
	re _ Re on: '(a+|b)*'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'AB') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test232
	| re re2 |
	re _ Re on: '(a+|b){0,}'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'AB') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test233
	| re re2 |
	re _ Re on: '(a+|b)+'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'AB') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test234
	| re re2 |
	re _ Re on: '(a+|b){1,}'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'AB') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test235
	| re re2 |
	re _ Re on: '(a+|b)?'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'AB') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test236
	| re re2 |
	re _ Re on: '(a+|b){0,1}'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'AB') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test237
	| re re2 |
	re _ Re on: '(a+|b){0,1}?'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'AB') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test238
	| re re2 |
	re _ Re on: '[^ab]*'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'CDE') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test239
	| re re2 |
	re _ Re on: 'abc'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test24
	| re re2 |
	re _ Re on: 'a(?=b(?i)c)\w\wd'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcd') isNil not.
	self assert: (re search: 'abCd') isNil not.
	self assert: (re search: 'aBCd') isNil.
	self assert: (re search: 'abcD') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test240
	| re re2 |
	re _ Re on: 'a*'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test241
	| re re2 |
	re _ Re on: '([abc])*d'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABBBCD') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test242
	| re re2 |
	re _ Re on: '([abc])*bcd'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABCD') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test243
	| re re2 |
	re _ Re on: 'a|b|c|d|e'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'E') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test244
	| re re2 |
	re _ Re on: '(a|b|c|d|e)f'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'EF') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test245
	| re re2 |
	re _ Re on: 'abcd*efg'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABCDEFG') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test246
	| re re2 |
	re _ Re on: 'ab*'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'XABYABBBZ') isNil not.
	self assert: (re search: 'XAYABBBZ') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test247
	| re re2 |
	re _ Re on: '(ab|cd)e'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABCDE') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test248
	| re re2 |
	re _ Re on: '[abhgefdc]ij'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'HIJ') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 13:53'!
testNewSuite3test249
	| re |
	re _ Re on: '^(ab|cd)e'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABCDE') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test25
	| re re2 |
	re _ Re on: '(?s-i:more.*than).*million'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'more than million') isNil not.
	self assert: (re search: 'more than MILLION') isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'more ';
		nextPut: Character cr;
		nextPutAll: ' than Million'])) isNil not.
	self assert: (re search: 'MORE THAN MILLION') isNil.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'more ';
		nextPut: Character cr;
		nextPutAll: ' than ';
		nextPut: Character cr;
		nextPutAll: ' million'])) isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test250
	| re re2 |
	re _ Re on: '(abc|)ef'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABCDEF') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test251
	| re re2 |
	re _ Re on: '(a|b)c*d'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABCD') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test252
	| re re2 |
	re _ Re on: '(ab|ab*)bc'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test253
	| re re2 |
	re _ Re on: 'a([bc]*)c*'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test254
	| re re2 |
	re _ Re on: 'a([bc]*)(c*d)'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABCD') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test255
	| re re2 |
	re _ Re on: 'a([bc]+)(c*d)'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABCD') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test256
	| re re2 |
	re _ Re on: 'a([bc]*)(c+d)'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABCD') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test257
	| re re2 |
	re _ Re on: 'a[bcd]*dcdcde'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ADCDCDE') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test258
	| re re2 |
	re _ Re on: 'a[bcd]+dcdcde'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test259
	| re re2 |
	re _ Re on: '(ab|a)b*c'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test26
	| re re2 |
	re _ Re on: '(?:(?s-i)more.*than).*million'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'more than million') isNil not.
	self assert: (re search: 'more than MILLION') isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'more ';
		nextPut: Character cr;
		nextPutAll: ' than Million'])) isNil not.
	self assert: (re search: 'MORE THAN MILLION') isNil.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'more ';
		nextPut: Character cr;
		nextPutAll: ' than ';
		nextPut: Character cr;
		nextPutAll: ' million'])) isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test260
	| re re2 |
	re _ Re on: '((a)(b)c)(d)'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABCD') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test261
	| re re2 |
	re _ Re on: '[a-zA-Z_][a-zA-Z0-9_]*'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ALPHA') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test262
	| re re2 |
	re _ Re on: '^a(bc+|b[eh])g|.h$'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABH') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test263
	| re re2 |
	re _ Re on: '(bc+d$|ef*g.|h?i(j|k))'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'EFFGZ') isNil not.
	self assert: (re search: 'IJ') isNil not.
	self assert: (re search: 'REFFGZ') isNil not.
	self assert: (re search: 'ADCDCDE') isNil.
	self assert: (re search: 'EFFG') isNil.
	self assert: (re search: 'BCDD') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test264
	| re re2 |
	re _ Re on: '((((((((((a))))))))))'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'A') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test265
	| re re2 |
	re _ Re on: '((((((((((a))))))))))\10'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'AA') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test266
	| re re2 |
	re _ Re on: '(((((((((a)))))))))'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'A') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test267
	| re re2 |
	re _ Re on: '(?:(?:(?:(?:(?:(?:(?:(?:(?:(a))))))))))'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'A') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test268
	| re re2 |
	re _ Re on: '(?:(?:(?:(?:(?:(?:(?:(?:(?:(a|b|c))))))))))'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'C') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test269
	| re re2 |
	re _ Re on: 'multiple words of text'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'AA') isNil.
	self assert: (re search: 'UH-UH') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test27
	| re re2 |
	re _ Re on: '(?>a(?i)b+)+c'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
	self assert: (re search: 'aBbc') isNil not.
	self assert: (re search: 'aBBc') isNil not.
	self assert: (re search: 'Abc') isNil.
	self assert: (re search: 'abAb') isNil.
	self assert: (re search: 'abbC') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test270
	| re re2 |
	re _ Re on: 'multiple words'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'MULTIPLE WORDS, YEAH') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test271
	| re re2 |
	re _ Re on: '(.*)c(.*)'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABCDE') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test272
	| re re2 |
	re _ Re on: '\((.*), (.*)\)'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '(A, B)') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test273
	| re re2 |
	re _ Re on: '[k]'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test274
	| re re2 |
	re _ Re on: 'abcd'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABCD') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test275
	| re re2 |
	re _ Re on: 'a(bc)d'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABCD') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test276
	| re re2 |
	re _ Re on: 'a[-]?c'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'AC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test277
	| re re2 |
	re _ Re on: '(abc)\1'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABCABC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test278
	| re re2 |
	re _ Re on: '([a-c]*)\1'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ABCABC') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test279
	| re re2 |
	re _ Re on: 'a(?!!b).'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abad') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test28
	| re re2 |
	re _ Re on: '(?=a(?i)b)\w\wc'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
	self assert: (re search: 'aBc') isNil not.
	self assert: (re search: 'Ab') isNil.
	self assert: (re search: 'abC') isNil.
	self assert: (re search: 'aBC') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test280
	| re re2 |
	re _ Re on: 'a(?=d).'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abad') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test281
	| re re2 |
	re _ Re on: 'a(?=c|d).'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abad') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test282
	| re re2 |
	re _ Re on: 'a(?:b|c|d)(.)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ace') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test283
	| re re2 |
	re _ Re on: 'a(?:b|c|d)*(.)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ace') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test284
	| re re2 |
	re _ Re on: 'a(?:b|c|d)+?(.)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ace') isNil not.
	self assert: (re search: 'acdbcdbe') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test285
	| re re2 |
	re _ Re on: 'a(?:b|c|d)+(.)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'acdbcdbe') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test286
	| re re2 |
	re _ Re on: 'a(?:b|c|d){2}(.)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'acdbcdbe') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test287
	| re re2 |
	re _ Re on: 'a(?:b|c|d){4,5}(.)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'acdbcdbe') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test288
	| re re2 |
	re _ Re on: 'a(?:b|c|d){4,5}?(.)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'acdbcdbe') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test289
	| re re2 |
	re _ Re on: '((foo)|(bar))*'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'foobar') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test29
	| re re2 |
	re _ Re on: '(?<=a(?i)b)(\w\w)c'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abxxc') isNil not.
	self assert: (re search: 'aBxxc') isNil not.
	self assert: (re search: 'Abxxc') isNil.
	self assert: (re search: 'ABxxc') isNil.
	self assert: (re search: 'abxxC') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test290
	| re re2 |
	re _ Re on: 'a(?:b|c|d){6,7}(.)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'acdbcdbe') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test291
	| re re2 |
	re _ Re on: 'a(?:b|c|d){6,7}?(.)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'acdbcdbe') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test292
	| re re2 |
	re _ Re on: 'a(?:b|c|d){5,6}(.)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'acdbcdbe') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test293
	| re re2 |
	re _ Re on: 'a(?:b|c|d){5,6}?(.)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'acdbcdbe') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test294
	| re re2 |
	re _ Re on: 'a(?:b|c|d){5,7}(.)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'acdbcdbe') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test295
	| re re2 |
	re _ Re on: 'a(?:b|c|d){5,7}?(.)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'acdbcdbe') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test296
	| re re2 |
	re _ Re on: 'a(?:b|(c|e){1,2}?|d)+?(.)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ace') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test297
	| re re2 |
	re _ Re on: '^(.+)?B'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'AB') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test298
	| re re2 |
	re _ Re on: '^([^a-z])|(\^)$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '.') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test299
	| re re2 |
	re _ Re on: '^[<>]&'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '<&OUT') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test3
	| re re2 |
	re _ Re on: '(?<=(foo)a)bar'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'fooabar') isNil not.
	self assert: (re search: 'bar') isNil.
	self assert: (re search: 'foobbar') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test30
	| re re2 |
	re _ Re on: '(?:(a)|b)(?(1)A|B)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aA') isNil not.
	self assert: (re search: 'bB') isNil not.
	self assert: (re search: 'aB') isNil.
	self assert: (re search: 'bA') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test300
	| re re2 |
	re _ Re on: '^(a\1?){4}$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aaaaaaaaaa') isNil not.
	self assert: (re search: 'AB') isNil.
	self assert: (re search: 'aaaaaaaaa') isNil.
	self assert: (re search: 'aaaaaaaaaaa') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test301
	| re re2 |
	re _ Re on: '^(a(?(1)\1)){4}$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aaaaaaaaaa') isNil not.
	self assert: (re search: 'aaaaaaaaa') isNil.
	self assert: (re search: 'aaaaaaaaaaa') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test302
	| re re2 |
	re _ Re on: '(?:(f)(o)(o)|(b)(a)(r))*'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'foobar') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test303
	| re re2 |
	re _ Re on: '(?<=a)b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ab') isNil not.
	self assert: (re search: 'cb') isNil.
	self assert: (re search: 'b') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test304
	| re re2 |
	re _ Re on: '(?<!!c)b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ab') isNil not.
	self assert: (re search: 'b') isNil not.
	self assert: (re search: 'b') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test305
	| re re2 |
	re _ Re on: '(?:..)*a'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aba') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test306
	| re re2 |
	re _ Re on: '(?:..)*?a'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aba') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test307
	| re re2 |
	re _ Re on: '^(?:b|a(?=(.)))*\1'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test308
	| re re2 |
	re _ Re on: '^(){3,5}'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test309
	| re re2 |
	re _ Re on: '^(a+)*ax'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aax') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test31
	| re re2 |
	re _ Re on: '^(a)?(?(1)a|b)+$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aa') isNil not.
	self assert: (re search: 'b') isNil not.
	self assert: (re search: 'bb') isNil not.
	self assert: (re search: 'ab') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test310
	| re re2 |
	re _ Re on: '^((a|b)+)*ax'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aax') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test311
	| re re2 |
	re _ Re on: '^((a|bc)+)*ax'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aax') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test312
	| re re2 |
	re _ Re on: '(a|x)*ab'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'cab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test313
	| re re2 |
	re _ Re on: '(a)*ab'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'cab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test314
	| re re2 |
	re _ Re on: '(?:(?i)a)b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test315
	| re re2 |
	re _ Re on: '((?i)a)b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test316
	| re re2 |
	re _ Re on: '(?:(?i)a)b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'Ab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test317
	| re re2 |
	re _ Re on: '((?i)a)b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'Ab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test318
	| re re2 |
	re _ Re on: '(?:(?i)a)b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'cb') isNil.
	self assert: (re search: 'aB') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test319
	| re re2 |
	re _ Re on: '((?i)a)b'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test32
	| re re2 |
	re _ Re on: '^(?(?=abc)\w{3}:|\d\d)$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc:') isNil not.
	self assert: (re search: '12') isNil not.
	self assert: (re search: '123') isNil.
	self assert: (re search: 'xyz') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test320
	| re re2 |
	re _ Re on: '(?i:a)b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test321
	| re re2 |
	re _ Re on: '((?i:a))b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test322
	| re re2 |
	re _ Re on: '(?i:a)b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'Ab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test323
	| re re2 |
	re _ Re on: '((?i:a))b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'Ab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test324
	| re re2 |
	re _ Re on: '(?i:a)b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aB') isNil.
	self assert: (re search: 'aB') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test325
	| re re2 |
	re _ Re on: '((?i:a))b'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test326
	| re re2 |
	re _ Re on: '(?:(?-i)a)b'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test327
	| re re2 |
	re _ Re on: '((?-i)a)b'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test328
	| re re2 |
	re _ Re on: '(?:(?-i)a)b'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aB') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test329
	| re re2 |
	re _ Re on: '((?-i)a)b'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aB') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test33
	| re re2 |
	re _ Re on: '^(?(?!!abc)\d\d|\w{3}:)$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc:') isNil not.
	self assert: (re search: '12') isNil not.
	self assert: (re search: '123') isNil.
	self assert: (re search: 'xyz') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 14:30'!
testNewSuite3test330
	| re |
	re _ Re on: '(?:(?-i)a)b'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aB') isNil not.
	self assert: (re search: 'Ab') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test331
	| re re2 |
	re _ Re on: '((?-i)a)b'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test332
	| re re2 |
	re _ Re on: '(?:(?-i)a)b'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aB') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test333
	| re re2 |
	re _ Re on: '((?-i)a)b'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aB') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test334
	| re re2 |
	re _ Re on: '(?:(?-i)a)b'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'Ab') isNil.
	self assert: (re search: 'AB') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test335
	| re re2 |
	re _ Re on: '((?-i)a)b'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test336
	| re re2 |
	re _ Re on: '(?-i:a)b'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test337
	| re re2 |
	re _ Re on: '((?-i:a))b'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test338
	| re re2 |
	re _ Re on: '(?-i:a)b'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aB') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test339
	| re re2 |
	re _ Re on: '((?-i:a))b'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aB') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test34
	| re re2 |
	re _ Re on: '(?(?<=foo)bar|cat)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'foobar') isNil not.
	self assert: (re search: 'cat') isNil not.
	self assert: (re search: 'fcat') isNil not.
	self assert: (re search: 'focat') isNil not.
	self assert: (re search: 'foocat') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test340
	| re re2 |
	re _ Re on: '(?-i:a)b'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'AB') isNil.
	self assert: (re search: 'Ab') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test341
	| re re2 |
	re _ Re on: '((?-i:a))b'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test342
	| re re2 |
	re _ Re on: '(?-i:a)b'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aB') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test343
	| re re2 |
	re _ Re on: '((?-i:a))b'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aB') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test344
	| re re2 |
	re _ Re on: '(?-i:a)b'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'Ab') isNil.
	self assert: (re search: 'AB') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test345
	| re re2 |
	re _ Re on: '((?-i:a))b'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test346
	| re re2 |
	re _ Re on: '((?-i:a.))b'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'AB') isNil.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'a';
		nextPut: Character cr;
		nextPutAll: 'B'])) isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test347
	| re re2 |
	re _ Re on: '((?s-i:a.))b'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'a';
		nextPut: Character cr;
		nextPutAll: 'B'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test348
	| re re2 |
	re _ Re on: '(?:c|d)(?:)(?:a(?:)(?:b)(?:b(?:))(?:b(?:)(?:b)))'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'cabbbb') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test349
	| re re2 |
	re _ Re on: '(?:c|d)(?:)(?:aaaaaaaa(?:)(?:bbbbbbbb)(?:bbbbbbbb(?:))(?:bbbbbbbb(?:)(?:bbbbbbbb)))'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'caaaaaaaabbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test35
	| re re2 |
	re _ Re on: '(?(?<!!foo)cat|bar)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'foobar') isNil not.
	self assert: (re search: 'cat') isNil not.
	self assert: (re search: 'fcat') isNil not.
	self assert: (re search: 'focat') isNil not.
	self assert: (re search: 'foocat') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test350
	| re re2 |
	re _ Re on: '(ab)\d\1'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'Ab4ab') isNil not.
	self assert: (re search: 'ab4Ab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test351
	| re re2 |
	re _ Re on: 'foo\w*\d{4}baz'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'foobar1234baz') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test352
	| re re2 |
	re _ Re on: 'x(~~)*(?:(?:F)?)?'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'x~~') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test353
	| re re2 |
	re _ Re on: '^a(?#xxx){3}c'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aaac') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test354
	| re re2 |
	re _ Re on: '^a (?#xxx) (?#yyy) {3}c'.
	re
		beExtended.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aaac') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test355
	| re re2 |
	re _ Re on: '(?<!![cd])b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'B';
		nextPut: Character cr;
		nextPutAll: 'B'])) isNil.
	self assert: (re search: 'dbcb') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test356
	| re re2 |
	re _ Re on: '(?<!![cd])[ab]'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'dbaacb') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test357
	| re re2 |
	re _ Re on: '(?<!!(c|d))b'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test358
	| re re2 |
	re _ Re on: '(?<!!(c|d))[ab]'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'dbaacb') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test359
	| re re2 |
	re _ Re on: '(?<!!cd)[ab]'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'cdaccb') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test36
	| re re2 |
	re _ Re on: '( \( )? [^()]+ (?(1) \) |) '.
	re
		beExtended.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcd') isNil not.
	self assert: (re search: '(abcd)') isNil not.
	self assert: (re search: 'the quick (abcd) fox') isNil not.
	self assert: (re search: '(abcd') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test360
	| re re2 |
	re _ Re on: '^(?:a?b?)*$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'dbcb') isNil.
	self assert: (re search: 'a--') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test361
	| re re2 |
	re _ Re on: '((?s)^a(.))((?m)^b$)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'a';
		nextPut: Character cr;
		nextPutAll: 'b';
		nextPut: Character cr;
		nextPutAll: 'c';
		nextPut: Character cr])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test362
	| re re2 |
	re _ Re on: '((?m)^b$)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'a';
		nextPut: Character cr;
		nextPutAll: 'b';
		nextPut: Character cr;
		nextPutAll: 'c';
		nextPut: Character cr])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test363
	| re re2 |
	re _ Re on: '(?m)^b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'a';
		nextPut: Character cr;
		nextPutAll: 'b';
		nextPut: Character cr])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test364
	| re re2 |
	re _ Re on: '(?m)^(b)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'a';
		nextPut: Character cr;
		nextPutAll: 'b';
		nextPut: Character cr])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test365
	| re re2 |
	re _ Re on: '((?m)^b)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'a';
		nextPut: Character cr;
		nextPutAll: 'b';
		nextPut: Character cr])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test366
	| re re2 |
	re _ Re on: '\n((?m)^b)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'a';
		nextPut: Character cr;
		nextPutAll: 'b';
		nextPut: Character cr])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test367
	| re re2 |
	re _ Re on: '((?s).)c(?!!.)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'a';
		nextPut: Character cr;
		nextPutAll: 'b';
		nextPut: Character cr;
		nextPutAll: 'c';
		nextPut: Character cr])) isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'a';
		nextPut: Character cr;
		nextPutAll: 'b';
		nextPut: Character cr;
		nextPutAll: 'c';
		nextPut: Character cr])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test368
	| re re2 |
	re _ Re on: '((?s)b.)c(?!!.)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'a';
		nextPut: Character cr;
		nextPutAll: 'b';
		nextPut: Character cr;
		nextPutAll: 'c';
		nextPut: Character cr])) isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'a';
		nextPut: Character cr;
		nextPutAll: 'b';
		nextPut: Character cr;
		nextPutAll: 'c';
		nextPut: Character cr])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test369
	| re re2 |
	re _ Re on: '^b'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test37
	| re re2 |
	re _ Re on: '( \( )? [^()]+ (?(1) \) ) '.
	re
		beExtended.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcd') isNil not.
	self assert: (re search: '(abcd)') isNil not.
	self assert: (re search: 'the quick (abcd) fox') isNil not.
	self assert: (re search: '(abcd') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test370
	| re re2 |
	re _ Re on: '()^b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'a';
		nextPut: Character cr;
		nextPutAll: 'b';
		nextPut: Character cr;
		nextPutAll: 'c';
		nextPut: Character cr])) isNil.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'a';
		nextPut: Character cr;
		nextPutAll: 'b';
		nextPut: Character cr;
		nextPutAll: 'c';
		nextPut: Character cr])) isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test371
	| re re2 |
	re _ Re on: '((?m)^b)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'a';
		nextPut: Character cr;
		nextPutAll: 'b';
		nextPut: Character cr;
		nextPutAll: 'c';
		nextPut: Character cr])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test372
	| re re2 |
	re _ Re on: '(?(1)a|b)'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test373
	| re re2 |
	re _ Re on: '(?(1)b|a)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test374
	| re re2 |
	re _ Re on: '(x)?(?(1)a|b)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a') isNil.
	self assert: (re search: 'a') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test375
	| re re2 |
	re _ Re on: '(x)?(?(1)b|a)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test376
	| re re2 |
	re _ Re on: '()?(?(1)b|a)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test377
	| re re2 |
	re _ Re on: '()(?(1)b|a)'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test378
	| re re2 |
	re _ Re on: '()?(?(1)a|b)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test379
	| re re2 |
	re _ Re on: '^(\()?blah(?(1)(\)))$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '(blah)') isNil not.
	self assert: (re search: 'blah') isNil not.
	self assert: (re search: 'a') isNil.
	self assert: (re search: 'blah)') isNil.
	self assert: (re search: '(blah') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test38
	| re re2 |
	re _ Re on: '^(?(2)a|(1)(2))+$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '12') isNil not.
	self assert: (re search: '12a') isNil not.
	self assert: (re search: '12aa') isNil not.
	self assert: (re search: '1234') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test380
	| re re2 |
	re _ Re on: '^(\(+)?blah(?(1)(\)))$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '(blah)') isNil not.
	self assert: (re search: 'blah') isNil not.
	self assert: (re search: 'blah)') isNil.
	self assert: (re search: '(blah') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test381
	| re re2 |
	re _ Re on: '(?(?!!a)a|b)'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test382
	| re re2 |
	re _ Re on: '(?(?!!a)b|a)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test383
	| re re2 |
	re _ Re on: '(?(?=a)b|a)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a') isNil.
	self assert: (re search: 'a') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test384
	| re re2 |
	re _ Re on: '(?(?=a)a|b)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test385
	| re re2 |
	re _ Re on: '(?=(a+?))(\1ab)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aaab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test386
	| re re2 |
	re _ Re on: '^(?=(a+?))\1ab'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test387
	| re re2 |
	re _ Re on: '(\w+:)+'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'one:') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test388
	| re re2 |
	re _ Re on: '$(?<=^(a))'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test389
	| re re2 |
	re _ Re on: '(?=(a+?))(\1ab)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aaab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test39
	| re re2 |
	re _ Re on: '((?i)blah)\s+\1'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'blah blah') isNil not.
	self assert: (re search: 'BLAH BLAH') isNil not.
	self assert: (re search: 'Blah Blah') isNil not.
	self assert: (re search: 'blaH blaH') isNil not.
	self assert: (re search: 'blah BLAH') isNil.
	self assert: (re search: 'Blah blah') isNil.
	self assert: (re search: 'blaH blah') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test390
	| re re2 |
	re _ Re on: '^(?=(a+?))\1ab'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aaab') isNil.
	self assert: (re search: 'aaab') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test391
	| re re2 |
	re _ Re on: '([\w:]+::)?(\w+)$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcd') isNil not.
	self assert: (re search: 'xy:z:::abcd') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test392
	| re re2 |
	re _ Re on: '^[^bcd]*(c+)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aexycd') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test393
	| re re2 |
	re _ Re on: '(a*)b+'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'caab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test394
	| re re2 |
	re _ Re on: '([\w:]+::)?(\w+)$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcd') isNil not.
	self assert: (re search: 'xy:z:::abcd') isNil not.
	self assert: (re search: 'abcd:') isNil.
	self assert: (re search: 'abcd:') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test395
	| re re2 |
	re _ Re on: '^[^bcd]*(c+)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aexycd') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test396
	| re re2 |
	re _ Re on: '(>a+)ab'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test397
	| re re2 |
	re _ Re on: '(?>a+)b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aaab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test398
	| re re2 |
	re _ Re on: '([[:]+)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a:[b]:') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test399
	| re re2 |
	re _ Re on: '([[=]+)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a=[b]=') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test4
	| re re2 |
	re _ Re on: '\Aabc\z'.
	re
		beMultiline.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: Character cr])) isNil.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'qqq';
		nextPut: Character cr;
		nextPutAll: 'abc'])) isNil.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'abc';
		nextPut: Character cr;
		nextPutAll: 'zzz'])) isNil.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'qqq';
		nextPut: Character cr;
		nextPutAll: 'abc';
		nextPut: Character cr;
		nextPutAll: 'zzz'])) isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test40
	| re re2 |
	re _ Re on: '((?i)blah)\s+(?i:\1)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'blah blah') isNil not.
	self assert: (re search: 'BLAH BLAH') isNil not.
	self assert: (re search: 'Blah Blah') isNil not.
	self assert: (re search: 'blaH blaH') isNil not.
	self assert: (re search: 'blah BLAH') isNil not.
	self assert: (re search: 'Blah blah') isNil not.
	self assert: (re search: 'blaH blah') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test400
	| re re2 |
	re _ Re on: '([[.]+)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a.[b].') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test401
	| re re2 |
	re _ Re on: '((?>a+)b)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aaab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test402
	| re re2 |
	re _ Re on: '(?>(a+))b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aaab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test403
	| re re2 |
	re _ Re on: '((?>[^()]+)|\([^()]*\))+'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '((abc(ade)ufh()()x') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test404
	| re re2 |
	re _ Re on: 'a\Z'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aaab') isNil.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'a';
		nextPut: Character cr;
		nextPutAll: 'b';
		nextPut: Character cr])) isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test405
	| re re2 |
	re _ Re on: 'b\Z'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'a';
		nextPut: Character cr;
		nextPutAll: 'b';
		nextPut: Character cr])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test406
	| re re2 |
	re _ Re on: 'b\z'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test407
	| re re2 |
	re _ Re on: 'b\Z'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'a';
		nextPut: Character cr;
		nextPutAll: 'b'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test408
	| re re2 |
	re _ Re on: 'b\z'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'a';
		nextPut: Character cr;
		nextPutAll: 'b'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test409
	| re re2 |
	re _ Re on: '^(?>(?(1)\.|())[^\W_](?>[a-z0-9-]*[^\W_])?)+$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a') isNil not.
	self assert: (re search: 'abc') isNil not.
	self assert: (re search: 'a-b') isNil not.
	self assert: (re search: '0-9') isNil not.
	self assert: (re search: 'a.b') isNil not.
	self assert: (re search: '5.6.7') isNil not.
	self assert: (re search: 'the.quick.brown.fox') isNil not.
	self assert: (re search: 'a100.b200.300c') isNil not.
	self assert: (re search: '12-ab.1245') isNil not.
	self assert: (re search: (String streamContents: [:s | s])) isNil.
	self assert: (re search: '.a') isNil.
	self assert: (re search: '-a') isNil.
	self assert: (re search: 'a-') isNil.
	self assert: (re search: 'a.') isNil.
	self assert: (re search: 'a_b') isNil.
	self assert: (re search: 'a.-') isNil.
	self assert: (re search: 'a..') isNil.
	self assert: (re search: 'ab..bc') isNil.
	self assert: (re search: 'the.quick.brown.fox-') isNil.
	self assert: (re search: 'the.quick.brown.fox.') isNil.
	self assert: (re search: 'the.quick.brown.fox_') isNil.
	self assert: (re search: 'the.quick.brown.fox+') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test41
	| re re2 |
	re _ Re on: '(?>a*)*'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a') isNil not.
	self assert: (re search: 'aa') isNil not.
	self assert: (re search: 'aaaa') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test410
	| re re2 |
	re _ Re on: '(?>.*)(?<=(abcd|wxyz))'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'alphabetabcd') isNil not.
	self assert: (re search: 'endingwxyz') isNil not.
	self assert: (re search: 'a rather long string that doesn''t end with one of them') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 13:53'!
testNewSuite3test411
	| re |
	re _ Re on: 'word (?>(?:(?!!otherword)[a-zA-Z0-9]+ ){0,30})otherword'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'word cat dog elephant mussel cow horse canary baboon snake shark otherword') isNil not.
	self assert: (re search: 'word cat dog elephant mussel cow horse canary baboon snake shark') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 13:54'!
testNewSuite3test412
	| re |
	re _ Re on: 'word (?>[a-zA-Z0-9]+ ){0,30}otherword'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'word cat dog elephant mussel cow horse canary baboon snake shark the quick brown fox and the lazy dog and several other words getting close to thirty by now I hope otherword') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test413
	| re re2 |
	re _ Re on: '(?<=\d{3}(?!!999))foo'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '999foo') isNil not.
	self assert: (re search: '123999foo') isNil not.
	self assert: (re search: '123abcfoo') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test414
	| re re2 |
	re _ Re on: '(?<=(?!!...999)\d{3})foo'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '999foo') isNil not.
	self assert: (re search: '123999foo') isNil not.
	self assert: (re search: '123abcfoo') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test415
	| re re2 |
	re _ Re on: '(?<=\d{3}(?!!999)...)foo'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '123abcfoo') isNil not.
	self assert: (re search: '123456foo') isNil not.
	self assert: (re search: '123999foo') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test416
	| re re2 |
	re _ Re on: '(?<=\d{3}...)(?<!!999)foo'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '123abcfoo') isNil not.
	self assert: (re search: '123456foo') isNil not.
	self assert: (re search: '123999foo') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test417
	| re re2 |
	re _ Re on: '<a[\s]+href[\s]*=[\s]*          # find <a href=
 ([\"\''])?                       # find single or double quote
 (?(1) (.*?)\1 | ([^\s]+))       # if quote found, match up to next matching
                                 # quote, otherwise match up to next space
'.
	re
		beNotCaseSensitive;
		beDotIncludesNewline;
		beExtended.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '<a href=abcd xyz') isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: '<a href=';
		nextPutAll: 'abcd xyz pqr';
		nextPutAll: ' cats'])) isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: '<a href=';
		nextPutAll: 'abcd xyz pqr';
		nextPutAll: ' cats'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test418
	| re re2 |
	re _ Re on: '<a\s+href\s*=\s*                # find <a href=
 (["''])?                         # find single or double quote
 (?(1) (.*?)\1 | (\S+))          # if quote found, match up to next matching
                                 # quote, otherwise match up to next space
'.
	re
		beNotCaseSensitive;
		beDotIncludesNewline;
		beExtended.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '<a href=abcd xyz') isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: '<a href=';
		nextPutAll: 'abcd xyz pqr';
		nextPutAll: ' cats'])) isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: '<a href       =       ';
		nextPutAll: 'abcd xyz pqr';
		nextPutAll: ' cats'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test419
	| re re2 |
	re _ Re on: '<a\s+href(?>\s*)=(?>\s*)        # find <a href=
 (["''])?                         # find single or double quote
 (?(1) (.*?)\1 | (\S+))          # if quote found, match up to next matching
                                 # quote, otherwise match up to next space
'.
	re
		beNotCaseSensitive;
		beDotIncludesNewline;
		beExtended.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '<a href=abcd xyz') isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: '<a href=';
		nextPutAll: 'abcd xyz pqr';
		nextPutAll: ' cats'])) isNil not.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: '<a href       =       ';
		nextPutAll: 'abcd xyz pqr';
		nextPutAll: ' cats'])) isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test42
	| re re2 |
	re _ Re on: '(abc|)+'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
	self assert: (re search: 'abcabc') isNil not.
	self assert: (re search: 'abcabcabc') isNil not.
	self assert: (re search: 'xyz') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test420
	| re re2 |
	re _ Re on: '((Z)+|A)*'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ZABCDEFG') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test421
	| re re2 |
	re _ Re on: '(Z()|A)*'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ZABCDEFG') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test422
	| re re2 |
	re _ Re on: '(Z(())|A)*'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ZABCDEFG') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test423
	| re re2 |
	re _ Re on: '((?>Z)+|A)*'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ZABCDEFG') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test424
	| re re2 |
	re _ Re on: '((?>)+|A)*'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ZABCDEFG') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test425
	| re re2 |
	re _ Re on: 'a*'.
	re
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abbab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test426
	| re re2 |
	re _ Re on: '^[a-\d]'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcde') isNil not.
	self assert: (re search: '-things') isNil not.
	self assert: (re search: '0digit') isNil not.
	self assert: (re search: 'bcdef') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test427
	| re re2 |
	re _ Re on: '^[\d-a]'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcde') isNil not.
	self assert: (re search: '-things') isNil not.
	self assert: (re search: '0digit') isNil not.
	self assert: (re search: 'bcdef') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:32'!
testNewSuite3test428
	| re re2 |
	re _ Re on: ' End of testinput3 '.
	re
		beStrangeOption;
		beStrangeOption;
		beStrangeOption;
		beStrangeOption;
		beStrangeOption;
		beStrangeOption;
		beStrangeOption.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test43
	| re re2 |
	re _ Re on: '([a]*)*'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a') isNil not.
	self assert: (re search: 'aaaaa') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test44
	| re re2 |
	re _ Re on: '([ab]*)*'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a') isNil not.
	self assert: (re search: 'b') isNil not.
	self assert: (re search: 'ababab') isNil not.
	self assert: (re search: 'aaaabcde') isNil not.
	self assert: (re search: 'bbbb') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test45
	| re re2 |
	re _ Re on: '([^a]*)*'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'b') isNil not.
	self assert: (re search: 'bbbb') isNil not.
	self assert: (re search: 'aaa') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test46
	| re re2 |
	re _ Re on: '([^ab]*)*'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'cccc') isNil not.
	self assert: (re search: 'abab') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test47
	| re re2 |
	re _ Re on: '([a]*?)*'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a') isNil not.
	self assert: (re search: 'aaaa') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test48
	| re re2 |
	re _ Re on: '([ab]*?)*'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a') isNil not.
	self assert: (re search: 'b') isNil not.
	self assert: (re search: 'abab') isNil not.
	self assert: (re search: 'baba') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test49
	| re re2 |
	re _ Re on: '([^a]*?)*'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'b') isNil not.
	self assert: (re search: 'bbbb') isNil not.
	self assert: (re search: 'aaa') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 13:54'!
testNewSuite3test5
	| re |
	re _ Re on: '(?>.*/)foo'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '/this/is/a/very/long/line/in/deed/with/very/many/slashes/in/it/you/see/foo') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test50
	| re re2 |
	re _ Re on: '([^ab]*?)*'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'c') isNil not.
	self assert: (re search: 'cccc') isNil not.
	self assert: (re search: 'baba') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test51
	| re re2 |
	re _ Re on: '(?>a*)*'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a') isNil not.
	self assert: (re search: 'aaabcde') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test52
	| re re2 |
	re _ Re on: '((?>a*))*'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aaaaa') isNil not.
	self assert: (re search: 'aabbaa') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test53
	| re re2 |
	re _ Re on: '((?>a*?))*'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aaaaa') isNil not.
	self assert: (re search: 'aabbaa') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test54
	| re re2 |
	re _ Re on: '(?(?=[^a-z]+[a-z])  \d{2}-[a-z]{3}-\d{2}  |  \d{2}-\d{2}-\d{2} ) '.
	re
		beExtended.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '12-sep-98') isNil not.
	self assert: (re search: '12-09-98') isNil not.
	self assert: (re search: 'sep-12-98') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test55
	| re re2 |
	re _ Re on: '(?<=(foo))bar\1'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'foobarfoo') isNil not.
	self assert: (re search: 'foobarfootling') isNil not.
	self assert: (re search: 'foobar') isNil.
	self assert: (re search: 'barfoo') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test56
	| re re2 |
	re _ Re on: '(?i:saturday|sunday)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'saturday') isNil not.
	self assert: (re search: 'sunday') isNil not.
	self assert: (re search: 'Saturday') isNil not.
	self assert: (re search: 'Sunday') isNil not.
	self assert: (re search: 'SATURDAY') isNil not.
	self assert: (re search: 'SUNDAY') isNil not.
	self assert: (re search: 'SunDay') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test57
	| re re2 |
	re _ Re on: '(a(?i)bc|BB)x'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcx') isNil not.
	self assert: (re search: 'aBCx') isNil not.
	self assert: (re search: 'bbx') isNil not.
	self assert: (re search: 'BBx') isNil not.
	self assert: (re search: 'abcX') isNil.
	self assert: (re search: 'aBCX') isNil.
	self assert: (re search: 'bbX') isNil.
	self assert: (re search: 'BBX') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test58
	| re re2 |
	re _ Re on: '^([ab](?i)[cd]|[ef])'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ac') isNil not.
	self assert: (re search: 'aC') isNil not.
	self assert: (re search: 'bD') isNil not.
	self assert: (re search: 'elephant') isNil not.
	self assert: (re search: 'Europe') isNil not.
	self assert: (re search: 'frog') isNil not.
	self assert: (re search: 'France') isNil not.
	self assert: (re search: 'Africa') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test59
	| re re2 |
	re _ Re on: '^(ab|a(?i)[b-c](?m-i)d|x(?i)y|z)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ab') isNil not.
	self assert: (re search: 'aBd') isNil not.
	self assert: (re search: 'xy') isNil not.
	self assert: (re search: 'xY') isNil not.
	self assert: (re search: 'zebra') isNil not.
	self assert: (re search: 'Zambesi') isNil not.
	self assert: (re search: 'aCD') isNil.
	self assert: (re search: 'XY') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test6
	| re re2 |
	re _ Re on: '(?>.*/)foo'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '/this/is/a/very/long/line/in/deed/with/very/many/slashes/in/and/foo') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test60
	| re re2 |
	re _ Re on: '(?<=foo\n)^bar'.
	re
		beMultiline.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'foo';
		nextPut: Character cr;
		nextPutAll: 'bar'])) isNil not.
	self assert: (re search: 'bar') isNil.
	self assert: (re search: (String streamContents: [:s | s
		nextPutAll: 'baz';
		nextPut: Character cr;
		nextPutAll: 'bar'])) isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test61
	| re re2 |
	re _ Re on: '(?<=(?<!!foo)bar)baz'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'barbaz') isNil not.
	self assert: (re search: 'barbarbaz') isNil not.
	self assert: (re search: 'koobarbaz') isNil not.
	self assert: (re search: 'baz') isNil.
	self assert: (re search: 'foobarbaz') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 13:55'!
testNewSuite3test62
	| re |
	re _ Re on: 'The case of aaaaaa is missed out below because I think Perl 5.005_02 gets'.
	self shouldnt: [re compile] raise: Error.! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 13:58'!
testNewSuite3test63
	| re |
	re _ Re on: '^(a\1?){4}$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a') isNil.
	self assert: (re search: 'aa') isNil.
	self assert: (re search: 'aaa') isNil.
	self assert: (re search: 'aaaa') isNil not.
	self assert: (re search: 'aaaaa') isNil not.
	self assert: (re search: 'aaaaaaa') isNil not.
	self assert: (re search: 'aaaaaaaa') isNil.
	self assert: (re search: 'aaaaaaaaa') isNil.
	self assert: (re search: 'aaaaaaaaaa') isNil not.
	self assert: (re search: 'aaaaaaaaaaa') isNil.
	self assert: (re search: 'aaaaaaaaaaaa') isNil.
	self assert: (re search: 'aaaaaaaaaaaaa') isNil.
	self assert: (re search: 'aaaaaaaaaaaaaa') isNil.
	self assert: (re search: 'aaaaaaaaaaaaaaa') isNil.
	self assert: (re search: 'aaaaaaaaaaaaaaaa') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 14:00'!
testNewSuite3test64
	| re |
	re _ Re on: '^(a\1?)(a\1?)(a\2?)(a\3?)$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a') isNil.
	self assert: (re search: 'aa') isNil.
	self assert: (re search: 'aaa') isNil.
	self assert: (re search: 'aaaa') isNil not.
	self assert: (re search: 'aaaaa') isNil not.
	self assert: (re search: 'aaaaaa') isNil not.
	self assert: (re search: 'aaaaaaa') isNil not.
	self assert: (re search: 'aaaaaaaa') isNil.
	self assert: (re search: 'aaaaaaaaa') isNil.
	self assert: (re search: 'aaaaaaaaaa') isNil not.
	self assert: (re search: 'aaaaaaaaaaa') isNil.
	self assert: (re search: 'aaaaaaaaaaaa') isNil.
	self assert: (re search: 'aaaaaaaaaaaaa') isNil.
	self assert: (re search: 'aaaaaaaaaaaaaa') isNil.
	self assert: (re search: 'aaaaaaaaaaaaaaa') isNil.
	self assert: (re search: 'aaaaaaaaaaaaaaaa') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 14:03'!
testNewSuite3test65
	| re |
	re _ Re on: 'The following tests are taken from the Perl 5.005 test suite; some of them'.
	self shouldnt: [re compile] raise: Error.! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test66
	| re re2 |
	re _ Re on: 'abc'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
	self assert: (re search: 'xabcy') isNil not.
	self assert: (re search: 'ababc') isNil not.
	self assert: (re search: 'xbc') isNil.
	self assert: (re search: 'axc') isNil.
	self assert: (re search: 'abx') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test67
	| re re2 |
	re _ Re on: 'ab*c'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test68
	| re re2 |
	re _ Re on: 'ab*bc'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
	self assert: (re search: 'abbc') isNil not.
	self assert: (re search: 'abbbbc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test69
	| re re2 |
	re _ Re on: '.{1}'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abbbbc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test7
	| re re2 |
	re _ Re on: '(?>(\.\d\d[1-9]?))\d+'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '1.230003938') isNil not.
	self assert: (re search: '1.875000282') isNil not.
	self assert: (re search: '1.235') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test70
	| re re2 |
	re _ Re on: '.{3,4}'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abbbbc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test71
	| re re2 |
	re _ Re on: 'ab{0,}bc'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abbbbc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test72
	| re re2 |
	re _ Re on: 'ab+bc'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abbc') isNil not.
	self assert: (re search: 'abc') isNil.
	self assert: (re search: 'abq') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test73
	| re re2 |
	re _ Re on: 'ab{1,}bc'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test74
	| re re2 |
	re _ Re on: 'ab+bc'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abbbbc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test75
	| re re2 |
	re _ Re on: 'ab{1,}bc'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abbbbc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test76
	| re re2 |
	re _ Re on: 'ab{1,3}bc'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abbbbc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test77
	| re re2 |
	re _ Re on: 'ab{3,4}bc'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abbbbc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test78
	| re re2 |
	re _ Re on: 'ab{4,5}bc'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abq') isNil.
	self assert: (re search: 'abbbbc') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test79
	| re re2 |
	re _ Re on: 'ab?bc'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abbc') isNil not.
	self assert: (re search: 'abc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test8
	| re re2 |
	re _ Re on: '^((?>\w+)|(?>\s+))*$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'now is the time for all good men to come to the aid of the party') isNil not.
	self assert: (re search: 'this is not a line with only words and spaces!!') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test80
	| re re2 |
	re _ Re on: 'ab{0,1}bc'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test81
	| re re2 |
	re _ Re on: 'ab?bc'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test82
	| re re2 |
	re _ Re on: 'ab?c'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test83
	| re re2 |
	re _ Re on: 'ab{0,1}c'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test84
	| re re2 |
	re _ Re on: '^abc$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
	self assert: (re search: 'abbbbc') isNil.
	self assert: (re search: 'abcc') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test85
	| re re2 |
	re _ Re on: '^abc'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abcc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test86
	| re re2 |
	re _ Re on: '^abc$'.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 14:02'!
testNewSuite3test87
	| re |
	re _ Re on: 'abc$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aabc') isNil not.
	self assert: (re search: 'aabc') isNil not.
	self assert: (re search: 'aabcd') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test88
	| re re2 |
	re _ Re on: '^'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test89
	| re re2 |
	re _ Re on: '$'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test9
	| re re2 |
	re _ Re on: '(\d+)(\w)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '12345a') isNil not.
	self assert: (re search: '12345+') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test90
	| re re2 |
	re _ Re on: 'a.c'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abc') isNil not.
	self assert: (re search: 'axc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test91
	| re re2 |
	re _ Re on: 'a.*c'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'axyzc') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test92
	| re re2 |
	re _ Re on: 'a[bc]d'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'abd') isNil not.
	self assert: (re search: 'axyzd') isNil.
	self assert: (re search: 'abc') isNil.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test93
	| re re2 |
	re _ Re on: 'a[b-d]e'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'ace') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test94
	| re re2 |
	re _ Re on: 'a[b-d]'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aac') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test95
	| re re2 |
	re _ Re on: 'a[-b]'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a-') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test96
	| re re2 |
	re _ Re on: 'a[b-]'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a-') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test97
	| re re2 |
	re _ Re on: 'a]'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a]') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test98
	| re re2 |
	re _ Re on: 'a[]]b'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'a]b') isNil not.
! !

!ReTest methodsFor: 'newTestSuite3' stamp: 'acg 8/11/2002 12:31'!
testNewSuite3test99
	| re re2 |
	re _ Re on: 'a[^bc]d'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: 'aed') isNil not.
	self assert: (re search: 'abd') isNil.
	self assert: (re search: 'abd') isNil.
! !

!ReTest methodsFor: 'newTestSuite4' stamp: 'acg 8/11/2002 12:37'!
testNewSuit4test1
	| re re2 |
	re _ Re on: '^[\w]+'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '�cole') isNil.
! !

!ReTest methodsFor: 'newTestSuite4' stamp: 'acg 8/11/2002 12:37'!
testNewSuit4test10
	| re re2 |
	re _ Re on: '(.+)\b(.+)'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '�cole') isNil not.
! !

!ReTest methodsFor: 'newTestSuite4' stamp: 'acg 8/11/2002 12:37'!
testNewSuit4test12
	| re re2 |
	re _ Re on: '�cole'.
	re
		beNotCaseSensitive.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '�cole') isNil not.
	self assert: (re search: '�cole') isNil.
! !

!ReTest methodsFor: 'newTestSuite4' stamp: 'acg 8/11/2002 12:37'!
testNewSuit4test18
	| re re2 |
	re _ Re on: ' End of testinput4 '.
	self shouldnt: [re compile] raise: Error.
! !

!ReTest methodsFor: 'newTestSuite4' stamp: 'acg 8/11/2002 12:37'!
testNewSuit4test3
	| re re2 |
	re _ Re on: '^[\w]+'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '�cole') isNil.
! !

!ReTest methodsFor: 'newTestSuite4' stamp: 'acg 8/11/2002 12:37'!
testNewSuit4test4
	| re re2 |
	re _ Re on: '^[\W]+'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '�cole') isNil not.
! !

!ReTest methodsFor: 'newTestSuite4' stamp: 'acg 8/11/2002 14:08'!
testNewSuit4test6
	| re |
	re _ Re on: '[\b]'.
	self shouldnt: [re compile] raise: Error.
	self assert: (16r08 asCharacter asString) isNil not.
	self assert: (re search: 'a') isNil.
! !

!ReTest methodsFor: 'newTestSuite4' stamp: 'acg 8/11/2002 12:37'!
testNewSuit4test8
	| re re2 |
	re _ Re on: '^\w+'.
	self shouldnt: [re compile] raise: Error.
	self assert: (re search: '�cole') isNil.
! !


!String methodsFor: 'converting' stamp: 'acg 8/16/2002 22:48'!
asRe

	^Re on: self! !

!String methodsFor: 'regular expressions' stamp: 'acg 8/3/2002 17:23'!
asReDo: aBlock

	^aBlock value: self asRe! !

!String methodsFor: 'regular expressions' stamp: 'acg 8/4/2002 10:34'!
collectRe: aString

	"Answer the collection of my substrings matching aString"

	^aString asRe collectFrom: self! !

!String methodsFor: 'regular expressions' stamp: 'acg 8/4/2002 10:34'!
matchRe: aString

	"Answer whether the string is matched by the regular expression"

	^(aString asRe search: self) isNil not! !

!String methodsFor: 'regular expressions' stamp: 'acg 8/4/2002 10:32'!
reMatch: aString

	^aString asRe search: self! !

!String methodsFor: 'regular expressions' stamp: 'acg 8/4/2002 10:32'!
reMatch: aString andCollect: aBlock

	"Answer the collection of my substrings matching aString"

	^aString asRe search: self andCollect: aBlock! !

!String methodsFor: 'regular expressions' stamp: 'acg 8/4/2002 10:33'!
reMatch: aString andReplace: aBlock

	"Answer the collection of my substrings matching aString"

	^aString asRe search: self andReplace: aBlock! !

!String methodsFor: 'regular expressions' stamp: 'acg 8/4/2002 10:33'!
reMatch: aString from: fromInteger to: toInteger

	^aString asRe search: self from: fromInteger to: toInteger! !

!String methodsFor: 'regular expressions' stamp: 'acg 8/4/2002 10:33'!
reMatch: aString replace: aBlock

	"Answer the collection of my substrings matching aString"

	^aString asRe search: aString andReplace: aBlock! !

ReTest class removeSelector: #buildTestNamed:from:!
ReTest class removeSelector: #expandDataString:!
ReTest class removeSelector: #expandParmList:!
ReTest class removeSelector: #getDataFrom:!
ReTest class removeSelector: #hasFailers:!
ReTest class removeSelector: #install!
ReTest class removeSelector: #install1!
ReTest class removeSelector: #install2!
ReTest class removeSelector: #install3!
ReTest class removeSelector: #installNew1!
ReTest class removeSelector: #installNew2!
ReTest class removeSelector: #installNew3!
ReTest class removeSelector: #installNew4!
ReTest class removeSelector: #installNew5!
ReTest class removeSelector: #installNew6!
ReTest class removeSelector: #installTestNamed:in:from:!
ReTest class removeSelector: #optionFor:!
ReTest class removeSelector: #printDataArray:to:!
ReTest class removeSelector: #testData2From:!
ReTest class removeSelector: #testDataChunkFromFileStream:!
ReTest class removeSelector: #testDataFrom:!
ReTest class removeSelector: #testDataFromFileNamed:!
RePattern initialize!
