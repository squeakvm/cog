'From Squeak3.8 of ''5 May 2005'' [latest update: #6665] on 20 January 2006 at 11:22:22 pm'!!MPEGDisplayMorph methodsFor: 'accessing' stamp: 'JMM 1/20/2006 23:15'!isThereAFile	mpegFile isBufferBased ifTrue: [^true].	^(FileStream isAFileNamed: mpegFile fileName)! !!MPEGDisplayMorph methodsFor: 'accessing' stamp: 'JMM 1/20/2006 23:15'!moviePosition	"Answer a number between 0.0 and 1.0 indicating the current position within the movie."	mpegFile ifNil: [^ 0.0].	mpegFile fileHandle ifNil: [^ 0.0].	self isThereAFile ifFalse: [^0.0].	mpegFile hasVideo		ifTrue: [^ ((mpegFile videoGetFrame: 0) asFloat / (mpegFile videoFrames: 0)) min: 1.0].	soundTrack ifNotNil: [^ soundTrack soundPosition].	^ 0.0! !!MPEGDisplayMorph methodsFor: 'accessing' stamp: 'JMM 1/20/2006 23:16'!totalFrames	"Answer the total number of frames in this movie."	mpegFile ifNil: [^ 0].	mpegFile fileHandle ifNil: [^ 0].	self isThereAFile ifFalse: [^ 0].	mpegFile hasVideo ifFalse: [^ 0].	^ mpegFile videoFrames: 0! !!MPEGDisplayMorph methodsFor: 'accessing' stamp: 'JMM 1/20/2006 23:16'!totalSeconds	"Answer the total number of seconds in this movie."	mpegFile ifNil: [^ 0].	mpegFile fileHandle ifNil: [^ 0].	self isThereAFile ifFalse: [^ 0].	mpegFile hasVideo ifFalse: [^ 0].	^ self totalFrames asFloat / (mpegFile videoFrameRate: 0)! !!MPEGDisplayMorph methodsFor: 'commands' stamp: 'JMM 1/20/2006 23:09'!startPlaying	"Start playing the movie at the current position."	| frameIndex |	self stopPlaying.	stopFrame := nil.	self mpegFileIsOpen ifFalse: [^ self].	 (mpegFile fileName notNil) ifTrue:		[(FileStream isAFileNamed: mpegFile fileName) ifFalse: [ | newFileResult newFileName |		self inform: 'Path changed. Enter new one for: ', (FileDirectory localNameFor: mpegFile fileName).		newFileResult := StandardFileMenu oldFile.		newFileName := newFileResult directory fullNameFor: newFileResult name.			mpegFile openFile: newFileName]].		mpegFile hasAudio		ifTrue:			[mpegFile hasVideo ifTrue:				["set movie frame position from soundTrack position"				soundTrack reset.  "ensure file is open before positioning"				soundTrack soundPosition: (mpegFile videoGetFrame: 0) asFloat / (mpegFile videoFrames: 0).				"now set frame index from the soundtrack position for best sync"				frameIndex := ((soundTrack millisecondsSinceStart * desiredFrameRate) // 1000).				frameIndex := (frameIndex max: 0) min: ((mpegFile videoFrames: 0) - 3).				mpegFile videoSetFrame: frameIndex stream: 0].			SoundPlayer stopReverb.			soundTrack volume: volume.			soundTrack repeat: repeat.			soundTrack resumePlaying.			startFrame := startMSecs := 0]		ifFalse:			[soundTrack := nil.			startFrame := mpegFile videoGetFrame: 0.			startMSecs := Time millisecondClockValue].	running := true! !!MPEGDisplayMorph methodsFor: 'file open/close' stamp: 'JMM 1/20/2006 23:02'!openFileNamed: mpegFileName	"Try to open the MPEG file with the given name. Answer true if successful."	| e |	self closeFile.	(FileDirectory default fileExists: mpegFileName)		ifFalse: [self inform: ('File not found: {1}' translated format: {mpegFileName}). ^ false].	(MPEGFile isFileValidMPEG: mpegFileName)		ifTrue: [mpegFile := MPEGFile openFileUseBuffer: mpegFileName]		ifFalse: [			(JPEGMovieFile isJPEGMovieFile: mpegFileName)				ifTrue: [mpegFile := JPEGMovieFile new openFileNamed: mpegFileName]				ifFalse: [self inform: ('Not an MPEG or JPEG movie file: {1}' translated format: {mpegFileName}). ^ false]].	mpegFile fileHandle ifNil: [^ false].	"initialize soundTrack"	mpegFile hasAudio		ifTrue: [soundTrack := mpegFile audioPlayerForChannel: 1]		ifFalse: [soundTrack := nil].	mpegFile hasVideo		ifTrue: [  "set screen size and display first frame"			desiredFrameRate := mpegFile videoFrameRate: 0.			soundTrack ifNotNil: [  "compute frame rate from length of audio track"				desiredFrameRate := (mpegFile videoFrames: 0) / soundTrack duration].			e := (mpegFile videoFrameWidth: 0)@(mpegFile videoFrameHeight: 0).			frameBuffer := Form extent: e depth: (Display depth max: 16).			super extent: e.			self nextFrame]		ifFalse: [  "hide screen for audio-only files"			super extent: 250@0].! !