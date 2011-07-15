/*
 * A set of definitions for C source code control systems, to provide accurate
 * and definitive version information to the VM.
 *
 * Currently instantiated only for Subversion.  Please add definitions for
 * other repositories as appropriate.
 *
 * I guess a good way to manage this is to edit the below define list to select
 * appropriate the repository type, and then that's the extent of the fork.
 *
 * Eliot Miranda
 * eliot.miranda@gmail.com
 * 15 July 2011
 */

#define SCCS 0
#define RCS 0
#define CVS 0
#define SUBVERSION 1
#define BAZAAR 0
#define MERCURIAL 0
#define GIT 0


#if SUBVERSION
static char SvnRawRevisionString[] = "$Rev$";
# define REV_START (SvnRawRevisionString + 6)

static char SvnRawRepositoryURL[] = "$URL$";
# define URL_START (SvnRawRepositoryURL + 6)

static long
revisionAsLong() { return atol(REV_START); }

static char *
revisionAsString()
{
	char *maybe_space = strchr(REV_START,' ');
	if (maybe_space)
		*maybe_space = 0;
	return REV_START;
}

static char *
repositoryURL()
{
	char *maybe_platforms = strstr(URL_START, "/platforms");
	if (maybe_platforms)
		*maybe_platforms = 0;
	return URL_START;
}
# undef REV_START
# undef URL_START
#else /* SUBVERSION */
static long
revisionAsLong() { return -1; }

static char *
revisionAsString() { return "?"; }

static char *
repositoryURL() { return "unknown"; }
#endif /* SUBVERSION */

static char *sourceVersion = 0;

static char *sourceVersionString()
{
	if (!sourceVersion) {
		int len = strlen(revisionAsString()) + strlen(repositoryURL()) + 3;
		sourceVersion = malloc(len);
		sprintf(sourceVersion,"r%s %s",revisionAsString(),repositoryURL());
	}
	return sourceVersion;
}
