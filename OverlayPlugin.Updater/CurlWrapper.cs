using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Reflection;
using System.IO;

namespace RainbowMage.OverlayPlugin.Updater
{
    public static class CurlWrapper
    {
        private static string USER_AGENT;
        private static bool initialized = false;

        private static Dictionary<int, Download> downloadInfos = new Dictionary<int, Download>();
        private static int nextDownloadIdx = 1;

        private static object _global_lock = new object();

        #region CURL Header
        [DllImport("libcurl.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern CURLcode curl_global_init(long flags);

        [DllImport("libcurl.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr curl_easy_init();

        [DllImport("libcurl.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern CURLcode curl_easy_perform(IntPtr easy_handle);

        [DllImport("libcurl.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void curl_easy_cleanup(IntPtr handle);

        [DllImport("libcurl.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern CURLcode curl_easy_setopt(IntPtr handle, CURLoption option, long parameter);

        [DllImport("libcurl.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern CURLcode curl_easy_setopt(IntPtr handle, CURLoption option, IntPtr parameter);

        [DllImport("libcurl.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern CURLcode curl_easy_setopt(IntPtr handle, CURLoption option, string parameter);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        delegate UIntPtr write_callback(IntPtr ptr, UIntPtr size, UIntPtr nmemb, IntPtr userdata);

        [DllImport("libcurl.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern CURLcode curl_easy_setopt(IntPtr handle, CURLoption option, write_callback callback);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate int progress_callback(IntPtr clientp, long dltotal, long dlnow, long ultotal, long ulnow);

        [DllImport("libcurl.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern CURLcode curl_easy_setopt(IntPtr handle, CURLoption option, progress_callback callback);

        [DllImport("libcurl.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern CURLcode curl_easy_getinfo(IntPtr curl, CURLINFO info, out long value);

        [DllImport("libcurl.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr curl_slist_append(IntPtr list, string item);

        [DllImport("libcurl.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void curl_slist_free_all(IntPtr list);

        private const long CURL_GLOBAL_SSL = 1 << 0; /* no purpose since since 7.57.0 */
        private const long CURL_GLOBAL_WIN32 = 1 << 1;
        private const long CURL_GLOBAL_ALL = CURL_GLOBAL_SSL | CURL_GLOBAL_WIN32;
        private const long CURL_GLOBAL_NOTHING = 0;
        private const long CURL_GLOBAL_DEFAULT = CURL_GLOBAL_ALL;
        private const long CURL_GLOBAL_ACK_EINTR = 1 << 2;

        private enum CURLcode {
            CURLE_OK = 0,
            CURLE_UNSUPPORTED_PROTOCOL,    /* 1 */
            CURLE_FAILED_INIT,             /* 2 */
            CURLE_URL_MALFORMAT,           /* 3 */
            CURLE_NOT_BUILT_IN,            /* 4 - [was obsoleted in August 2007 for
                                    7.17.0, reused in April 2011 for 7.21.5] */
            CURLE_COULDNT_RESOLVE_PROXY,   /* 5 */
            CURLE_COULDNT_RESOLVE_HOST,    /* 6 */
            CURLE_COULDNT_CONNECT,         /* 7 */
            CURLE_WEIRD_SERVER_REPLY,      /* 8 */
            CURLE_REMOTE_ACCESS_DENIED,    /* 9 a service was denied by the server
                                    due to lack of access - when login fails
                                    this is not returned. */
            CURLE_FTP_ACCEPT_FAILED,       /* 10 - [was obsoleted in April 2006 for
                                    7.15.4, reused in Dec 2011 for 7.24.0]*/
            CURLE_FTP_WEIRD_PASS_REPLY,    /* 11 */
            CURLE_FTP_ACCEPT_TIMEOUT,      /* 12 - timeout occurred accepting server
                                    [was obsoleted in August 2007 for 7.17.0,
                                    reused in Dec 2011 for 7.24.0]*/
            CURLE_FTP_WEIRD_PASV_REPLY,    /* 13 */
            CURLE_FTP_WEIRD_227_FORMAT,    /* 14 */
            CURLE_FTP_CANT_GET_HOST,       /* 15 */
            CURLE_HTTP2,                   /* 16 - A problem in the http2 framing layer.
                                    [was obsoleted in August 2007 for 7.17.0,
                                    reused in July 2014 for 7.38.0] */
            CURLE_FTP_COULDNT_SET_TYPE,    /* 17 */
            CURLE_PARTIAL_FILE,            /* 18 */
            CURLE_FTP_COULDNT_RETR_FILE,   /* 19 */
            CURLE_OBSOLETE20,              /* 20 - NOT USED */
            CURLE_QUOTE_ERROR,             /* 21 - quote command failure */
            CURLE_HTTP_RETURNED_ERROR,     /* 22 */
            CURLE_WRITE_ERROR,             /* 23 */
            CURLE_OBSOLETE24,              /* 24 - NOT USED */
            CURLE_UPLOAD_FAILED,           /* 25 - failed upload "command" */
            CURLE_READ_ERROR,              /* 26 - couldn't open/read from file */
            CURLE_OUT_OF_MEMORY,           /* 27 */
                                           /* Note: CURLE_OUT_OF_MEMORY may sometimes indicate a conversion error
                                                    instead of a memory allocation error if CURL_DOES_CONVERSIONS
                                                    is defined
                                           */
            CURLE_OPERATION_TIMEDOUT,      /* 28 - the timeout time was reached */
            CURLE_OBSOLETE29,              /* 29 - NOT USED */
            CURLE_FTP_PORT_FAILED,         /* 30 - FTP PORT operation failed */
            CURLE_FTP_COULDNT_USE_REST,    /* 31 - the REST command failed */
            CURLE_OBSOLETE32,              /* 32 - NOT USED */
            CURLE_RANGE_ERROR,             /* 33 - RANGE "command" didn't work */
            CURLE_HTTP_POST_ERROR,         /* 34 */
            CURLE_SSL_CONNECT_ERROR,       /* 35 - wrong when connecting with SSL */
            CURLE_BAD_DOWNLOAD_RESUME,     /* 36 - couldn't resume download */
            CURLE_FILE_COULDNT_READ_FILE,  /* 37 */
            CURLE_LDAP_CANNOT_BIND,        /* 38 */
            CURLE_LDAP_SEARCH_FAILED,      /* 39 */
            CURLE_OBSOLETE40,              /* 40 - NOT USED */
            CURLE_FUNCTION_NOT_FOUND,      /* 41 - NOT USED starting with 7.53.0 */
            CURLE_ABORTED_BY_CALLBACK,     /* 42 */
            CURLE_BAD_FUNCTION_ARGUMENT,   /* 43 */
            CURLE_OBSOLETE44,              /* 44 - NOT USED */
            CURLE_INTERFACE_FAILED,        /* 45 - INTERFACE failed */
            CURLE_OBSOLETE46,              /* 46 - NOT USED */
            CURLE_TOO_MANY_REDIRECTS,      /* 47 - catch endless re-direct loops */
            CURLE_UNKNOWN_OPTION,          /* 48 - User specified an unknown option */
            CURLE_TELNET_OPTION_SYNTAX,    /* 49 - Malformed telnet option */
            CURLE_OBSOLETE50,              /* 50 - NOT USED */
            CURLE_OBSOLETE51,              /* 51 - NOT USED */
            CURLE_GOT_NOTHING,             /* 52 - when this is a specific error */
            CURLE_SSL_ENGINE_NOTFOUND,     /* 53 - SSL crypto engine not found */
            CURLE_SSL_ENGINE_SETFAILED,    /* 54 - can not set SSL crypto engine as
                                    default */
            CURLE_SEND_ERROR,              /* 55 - failed sending network data */
            CURLE_RECV_ERROR,              /* 56 - failure in receiving network data */
            CURLE_OBSOLETE57,              /* 57 - NOT IN USE */
            CURLE_SSL_CERTPROBLEM,         /* 58 - problem with the local certificate */
            CURLE_SSL_CIPHER,              /* 59 - couldn't use specified cipher */
            CURLE_PEER_FAILED_VERIFICATION, /* 60 - peer's certificate or fingerprint
                                     wasn't verified fine */
            CURLE_BAD_CONTENT_ENCODING,    /* 61 - Unrecognized/bad encoding */
            CURLE_LDAP_INVALID_URL,        /* 62 - Invalid LDAP URL */
            CURLE_FILESIZE_EXCEEDED,       /* 63 - Maximum file size exceeded */
            CURLE_USE_SSL_FAILED,          /* 64 - Requested FTP SSL level failed */
            CURLE_SEND_FAIL_REWIND,        /* 65 - Sending the data requires a rewind
                                    that failed */
            CURLE_SSL_ENGINE_INITFAILED,   /* 66 - failed to initialise ENGINE */
            CURLE_LOGIN_DENIED,            /* 67 - user, password or similar was not
                                    accepted and we failed to login */
            CURLE_TFTP_NOTFOUND,           /* 68 - file not found on server */
            CURLE_TFTP_PERM,               /* 69 - permission problem on server */
            CURLE_REMOTE_DISK_FULL,        /* 70 - out of disk space on server */
            CURLE_TFTP_ILLEGAL,            /* 71 - Illegal TFTP operation */
            CURLE_TFTP_UNKNOWNID,          /* 72 - Unknown transfer ID */
            CURLE_REMOTE_FILE_EXISTS,      /* 73 - File already exists */
            CURLE_TFTP_NOSUCHUSER,         /* 74 - No such user */
            CURLE_CONV_FAILED,             /* 75 - conversion failed */
            CURLE_CONV_REQD,               /* 76 - caller must register conversion
                                    callbacks using curl_easy_setopt options
                                    CONV_FROM_NETWORK_FUNCTION,
                                    CONV_TO_NETWORK_FUNCTION, and
                                    CONV_FROM_UTF8_FUNCTION */
            CURLE_SSL_CACERT_BADFILE,      /* 77 - could not load CACERT file, missing
                                    or wrong format */
            CURLE_REMOTE_FILE_NOT_FOUND,   /* 78 - remote file not found */
            CURLE_SSH,                     /* 79 - error from the SSH layer, somewhat
                                    generic so the error message will be of
                                    interest when this has happened */

            CURLE_SSL_SHUTDOWN_FAILED,     /* 80 - Failed to shut down the SSL
                                    connection */
            CURLE_AGAIN,                   /* 81 - socket is not ready for send/recv,
                                    wait till it's ready and try again (Added
                                    in 7.18.2) */
            CURLE_SSL_CRL_BADFILE,         /* 82 - could not load CRL file, missing or
                                    wrong format (Added in 7.19.0) */
            CURLE_SSL_ISSUER_ERROR,        /* 83 - Issuer check failed.  (Added in
                                    7.19.0) */
            CURLE_FTP_PRET_FAILED,         /* 84 - a PRET command failed */
            CURLE_RTSP_CSEQ_ERROR,         /* 85 - mismatch of RTSP CSeq numbers */
            CURLE_RTSP_SESSION_ERROR,      /* 86 - mismatch of RTSP Session Ids */
            CURLE_FTP_BAD_FILE_LIST,       /* 87 - unable to parse FTP file list */
            CURLE_CHUNK_FAILED,            /* 88 - chunk callback reported error */
            CURLE_NO_CONNECTION_AVAILABLE, /* 89 - No connection available, the
                                    session will be queued */
            CURLE_SSL_PINNEDPUBKEYNOTMATCH, /* 90 - specified pinned public key did not
                                     match */
            CURLE_SSL_INVALIDCERTSTATUS,   /* 91 - invalid certificate status */
            CURLE_HTTP2_STREAM,            /* 92 - stream error in HTTP/2 framing layer
                                    */
            CURLE_RECURSIVE_API_CALL,      /* 93 - an api function was called from
                                    inside a callback */
            CURLE_AUTH_ERROR,              /* 94 - an authentication function returned an
                                    error */
            CURL_LAST /* never use! */
        };

        private const int CURL_ERROR_SIZE = 256;

        /* CURLPROTO_ defines are for the *PROTOCOLS options */
        private const int CURLPROTO_HTTP = (1 << 0);
        private const int CURLPROTO_HTTPS = (1 << 1);
        private const int CURLPROTO_FTP = (1 << 2);
        private const int CURLPROTO_FTPS = (1 << 3);
        private const int CURLPROTO_SCP = (1 << 4);
        private const int CURLPROTO_SFTP = (1 << 5);
        private const int CURLPROTO_TELNET = (1 << 6);
        private const int CURLPROTO_LDAP = (1 << 7);
        private const int CURLPROTO_LDAPS = (1 << 8);
        private const int CURLPROTO_DICT = (1 << 9);
        private const int CURLPROTO_FILE = (1 << 10);
        private const int CURLPROTO_TFTP = (1 << 11);
        private const int CURLPROTO_IMAP = (1 << 12);
        private const int CURLPROTO_IMAPS = (1 << 13);
        private const int CURLPROTO_POP3 = (1 << 14);
        private const int CURLPROTO_POP3S = (1 << 15);
        private const int CURLPROTO_SMTP = (1 << 16);
        private const int CURLPROTO_SMTPS = (1 << 17);
        private const int CURLPROTO_RTSP = (1 << 18);
        private const int CURLPROTO_RTMP = (1 << 19);
        private const int CURLPROTO_RTMPT = (1 << 20);
        private const int CURLPROTO_RTMPE = (1 << 21);
        private const int CURLPROTO_RTMPTE = (1 << 22);
        private const int CURLPROTO_RTMPS = (1 << 23);
        private const int CURLPROTO_RTMPTS = (1 << 24);
        private const int CURLPROTO_GOPHER = (1 << 25);
        private const int CURLPROTO_SMB = (1 << 26);
        private const int CURLPROTO_SMBS = (1 << 27);
        private const int CURLPROTO_ALL = (~0) /* enable everything */;

        private const int CURLOPTTYPE_LONG          = 0;
        private const int CURLOPTTYPE_OBJECTPOINT   = 10000;
        private const int CURLOPTTYPE_FUNCTIONPOINT = 20000;
        private const int CURLOPTTYPE_OFF_T         = 30000;

        private const int CURLOPTTYPE_STRINGPOINT = CURLOPTTYPE_OBJECTPOINT;
        private const int CURLOPTTYPE_SLISTPOINT = CURLOPTTYPE_OBJECTPOINT;

        private enum CURLoption
        {
            /* This is the FILE * or void * the regular output should be written to. */
            WRITEDATA = CURLOPTTYPE_OBJECTPOINT + 1,

            /* The full URL to get/put */
            URL = CURLOPTTYPE_STRINGPOINT + 2,

            /* Port number to connect to, if other than default. */
            PORT = CURLOPTTYPE_LONG + 3,

            /* Name of proxy to use. */
            PROXY = CURLOPTTYPE_STRINGPOINT + 4,

            /* "user:password;options" to use when fetching. */
            USERPWD = CURLOPTTYPE_STRINGPOINT + 5,

            /* "user:password" to use with proxy. */
            PROXYUSERPWD = CURLOPTTYPE_STRINGPOINT + 6,

            /* Range to get, specified as an ASCII string. */
            RANGE = CURLOPTTYPE_STRINGPOINT + 7,

            /* not used */

            /* Specified file stream to upload from (use as input): */
            READDATA = CURLOPTTYPE_OBJECTPOINT + 9,

            /* Buffer to receive error messages in, must be at least CURL_ERROR_SIZE
            * bytes big. */
            ERRORBUFFER = CURLOPTTYPE_OBJECTPOINT + 10,

            /* Function that will be called to store the output (instead of fwrite). The
            * parameters will use fwrite() syntax, make sure to follow them. */
            WRITEFUNCTION = CURLOPTTYPE_FUNCTIONPOINT + 11,

            /* Function that will be called to read the input (instead of fread). The
            * parameters will use fread() syntax, make sure to follow them. */
            READFUNCTION = CURLOPTTYPE_FUNCTIONPOINT + 12,

            /* Time-out the read operation after this amount of seconds */
            TIMEOUT = CURLOPTTYPE_LONG + 13,

            /* If the INFILE is used, this can be used to inform libcurl about
            * how large the file being sent really is. That allows better error
            * checking and better verifies that the upload was successful. -1 means
            * unknown size.
            *
            * For large file support, there is also a _LARGE version of the key
            * which takes an off_t type, allowing platforms with larger off_t
            * sizes to handle larger files.  See below for INFILESIZE_LARGE.
            */
            INFILESIZE = CURLOPTTYPE_LONG + 14,

            /* POST static input fields. */
            POSTFIELDS = CURLOPTTYPE_OBJECTPOINT + 15,

            /* Set the referrer page (needed by some CGIs) */
            REFERER = CURLOPTTYPE_STRINGPOINT + 16,

            /* Set the FTP PORT string (interface name, named or numerical IP address)
                Use i.e '-' to use default address. */
            FTPPORT = CURLOPTTYPE_STRINGPOINT + 17,

            /* Set the User-Agent string (examined by some CGIs) */
            USERAGENT = CURLOPTTYPE_STRINGPOINT + 18,

            /* If the download receives less than "low speed limit" bytes/second
            * during "low speed time" seconds, the operations is aborted.
            * You could i.e if you have a pretty high speed connection, abort if
            * it is less than 2000 bytes/sec during 20 seconds.
            */

            /* Set the "low speed limit" */
            LOW_SPEED_LIMIT = CURLOPTTYPE_LONG + 19,

            /* Set the "low speed time" */
            LOW_SPEED_TIME = CURLOPTTYPE_LONG + 20,

            /* Set the continuation offset.
            *
            * Note there is also a _LARGE version of this key which uses
            * off_t types, allowing for large file offsets on platforms which
            * use larger-than-32-bit off_t's.  Look below for RESUME_FROM_LARGE.
            */
            RESUME_FROM = CURLOPTTYPE_LONG + 21,

            /* Set cookie in request: */
            COOKIE = CURLOPTTYPE_STRINGPOINT + 22,

            /* This points to a linked list of headers, struct curl_slist kind. This
                list is also used for RTSP (in spite of its name) */
            HTTPHEADER = CURLOPTTYPE_SLISTPOINT + 23,

            /* This points to a linked list of post entries, struct curl_httppost */
            HTTPPOST = CURLOPTTYPE_OBJECTPOINT + 24,

            /* name of the file keeping your private SSL-certificate */
            SSLCERT = CURLOPTTYPE_STRINGPOINT + 25,

            /* password for the SSL or SSH private key */
            KEYPASSWD = CURLOPTTYPE_STRINGPOINT + 26,

            /* send TYPE parameter? */
            CRLF = CURLOPTTYPE_LONG + 27,

            /* send linked-list of QUOTE commands */
            QUOTE = CURLOPTTYPE_SLISTPOINT + 28,

            /* send FILE * or void * to store headers to, if you use a callback it
                is simply passed to the callback unmodified */
            HEADERDATA = CURLOPTTYPE_OBJECTPOINT + 29,

            /* point to a file to read the initial cookies from, also enables
                "cookie awareness" */
            COOKIEFILE = CURLOPTTYPE_STRINGPOINT + 31,

            /* What version to specifically try to use.
                See CURL_SSLVERSION defines below. */
            SSLVERSION = CURLOPTTYPE_LONG + 32,

            /* What kind of HTTP time condition to use, see defines */
            TIMECONDITION = CURLOPTTYPE_LONG + 33,

            /* Time to use with the above condition. Specified in number of seconds
                since 1 Jan 1970 */
            TIMEVALUE = CURLOPTTYPE_LONG + 34,

            /* 35 = OBSOLETE */

            /* Custom request, for customizing the get command like
                HTTP: DELETE, TRACE and others
                FTP: to use a different list command
                */
            CUSTOMREQUEST = CURLOPTTYPE_STRINGPOINT + 36,

            /* FILE handle to use instead of stderr */
            STDERR = CURLOPTTYPE_OBJECTPOINT + 37,

            /* 38 is not used */

            /* send linked-list of post-transfer QUOTE commands */
            POSTQUOTE = CURLOPTTYPE_SLISTPOINT + 39,

            OBSOLETE40 = CURLOPTTYPE_OBJECTPOINT + 40, /* OBSOLETE, do not use! */

            VERBOSE = CURLOPTTYPE_LONG + 41,      /* talk a lot */
            HEADER = CURLOPTTYPE_LONG + 42,       /* throw the header out too */
            NOPROGRESS = CURLOPTTYPE_LONG + 43,   /* shut off the progress meter */
            NOBODY = CURLOPTTYPE_LONG + 44,       /* use HEAD to get http document */
            FAILONERROR = CURLOPTTYPE_LONG + 45,  /* no output on http error codes >= 400 */
            UPLOAD = CURLOPTTYPE_LONG + 46,       /* this is an upload */
            POST = CURLOPTTYPE_LONG + 47,         /* HTTP POST method */
            DIRLISTONLY = CURLOPTTYPE_LONG + 48,  /* bare names when listing directories */

            APPEND = CURLOPTTYPE_LONG + 50,       /* Append instead of overwrite on upload! */

            /* Specify whether to read the user+password from the .netrc or the URL.
            * This must be one of the CURL_NETRC_* enums below. */
            NETRC = CURLOPTTYPE_LONG + 51,

            FOLLOWLOCATION = CURLOPTTYPE_LONG + 52,  /* use Location: Luke! */

            TRANSFERTEXT = CURLOPTTYPE_LONG + 53, /* transfer data in text/ASCII format */
            PUT = CURLOPTTYPE_LONG + 54,          /* HTTP PUT */

            /* 55 = OBSOLETE */

            /* DEPRECATED
            * Function that will be called instead of the internal progress display
            * function. This function should be defined as the curl_progress_callback
            * prototype defines. */
            PROGRESSFUNCTION = CURLOPTTYPE_FUNCTIONPOINT + 56,

            /* Data passed to the PROGRESSFUNCTION and XFERINFOFUNCTION
                callbacks */
            PROGRESSDATA = CURLOPTTYPE_OBJECTPOINT + 57,
            XFERINFODATA = PROGRESSDATA,

            /* We want the referrer field set automatically when following locations */
            AUTOREFERER = CURLOPTTYPE_LONG + 58,

            /* Port of the proxy, can be set in the proxy string as well with:
                "[host]:[port]" */
            PROXYPORT = CURLOPTTYPE_LONG + 59,

            /* size of the POST input data, if strlen() is not good to use */
            POSTFIELDSIZE = CURLOPTTYPE_LONG + 60,

            /* tunnel non-http operations through a HTTP proxy */
            HTTPPROXYTUNNEL = CURLOPTTYPE_LONG + 61,

            /* Set the interface string to use as outgoing network interface */
            INTERFACE = CURLOPTTYPE_STRINGPOINT + 62,

            /* Set the krb4/5 security level, this also enables krb4/5 awareness.  This
            * is a string, 'clear', 'safe', 'confidential' or 'private'.  If the string
            * is set but doesn't match one of these, 'private' will be used.  */
            KRBLEVEL = CURLOPTTYPE_STRINGPOINT + 63,

            /* Set if we should verify the peer in ssl handshake, set 1 to verify. */
            SSL_VERIFYPEER = CURLOPTTYPE_LONG + 64,

            /* The CApath or CAfile used to validate the peer certificate
                this option is used only if SSL_VERIFYPEER is true */
            CAINFO = CURLOPTTYPE_STRINGPOINT + 65,

            /* 66 = OBSOLETE */
            /* 67 = OBSOLETE */

            /* Maximum number of http redirects to follow */
            MAXREDIRS = CURLOPTTYPE_LONG + 68,

            /* Pass a long set to 1 to get the date of the requested document (if
                possible)! Pass a zero to shut it off. */
            FILETIME = CURLOPTTYPE_LONG + 69,

            /* This points to a linked list of telnet options */
            TELNETOPTIONS = CURLOPTTYPE_SLISTPOINT + 70,

            /* Max amount of cached alive connections */
            MAXCONNECTS = CURLOPTTYPE_LONG + 71,

            OBSOLETE72 = CURLOPTTYPE_LONG + 72, /* OBSOLETE, do not use! */

            /* 73 = OBSOLETE */

            /* Set to explicitly use a new connection for the upcoming transfer.
                Do not use this unless you're absolutely sure of this, as it makes the
                operation slower and is less friendly for the network. */
            FRESH_CONNECT = CURLOPTTYPE_LONG + 74,

            /* Set to explicitly forbid the upcoming transfer's connection to be re-used
                when done. Do not use this unless you're absolutely sure of this, as it
                makes the operation slower and is less friendly for the network. */
            FORBID_REUSE = CURLOPTTYPE_LONG + 75,

            /* Set to a file name that contains random data for libcurl to use to
                seed the random engine when doing SSL connects. */
            RANDOM_FILE = CURLOPTTYPE_STRINGPOINT + 76,

            /* Set to the Entropy Gathering Daemon socket pathname */
            EGDSOCKET = CURLOPTTYPE_STRINGPOINT + 77,

            /* Time-out connect operations after this amount of seconds, if connects are
                OK within this time, then fine... This only aborts the connect phase. */
            CONNECTTIMEOUT = CURLOPTTYPE_LONG + 78,

            /* Function that will be called to store headers (instead of fwrite). The
            * parameters will use fwrite() syntax, make sure to follow them. */
            HEADERFUNCTION = CURLOPTTYPE_FUNCTIONPOINT + 79,

            /* Set this to force the HTTP request to get back to GET. Only really usable
                if POST, PUT or a custom request have been used first.
            */
            HTTPGET = CURLOPTTYPE_LONG + 80,

            /* Set if we should verify the Common name from the peer certificate in ssl
            * handshake, set 1 to check existence, 2 to ensure that it matches the
            * provided hostname. */
            SSL_VERIFYHOST = CURLOPTTYPE_LONG + 81,

            /* Specify which file name to write all known cookies in after completed
                operation. Set file name to "-" (dash) to make it go to stdout. */
            COOKIEJAR = CURLOPTTYPE_STRINGPOINT + 82,

            /* Specify which SSL ciphers to use */
            SSL_CIPHER_LIST = CURLOPTTYPE_STRINGPOINT + 83,

            /* Specify which HTTP version to use! This must be set to one of the
                CURL_HTTP_VERSION* enums set below. */
            HTTP_VERSION = CURLOPTTYPE_LONG + 84,

            /* Specifically switch on or off the FTP engine's use of the EPSV command. By
                default, that one will always be attempted before the more traditional
                PASV command. */
            FTP_USE_EPSV = CURLOPTTYPE_LONG + 85,

            /* type of the file keeping your SSL-certificate ("DER", "PEM", "ENG") */
            SSLCERTTYPE = CURLOPTTYPE_STRINGPOINT + 86,

            /* name of the file keeping your private SSL-key */
            SSLKEY = CURLOPTTYPE_STRINGPOINT + 87,

            /* type of the file keeping your private SSL-key ("DER", "PEM", "ENG") */
            SSLKEYTYPE = CURLOPTTYPE_STRINGPOINT + 88,

            /* crypto engine for the SSL-sub system */
            SSLENGINE = CURLOPTTYPE_STRINGPOINT + 89,

            /* set the crypto engine for the SSL-sub system as default
                the param has no meaning...
            */
            SSLENGINE_DEFAULT = CURLOPTTYPE_LONG + 90,

            /* Non-zero value means to use the global dns cache */
            DNS_USE_GLOBAL_CACHE = CURLOPTTYPE_LONG + 91, /* DEPRECATED, do not use! */

            /* DNS cache timeout */
            DNS_CACHE_TIMEOUT = CURLOPTTYPE_LONG + 92,

            /* send linked-list of pre-transfer QUOTE commands */
            PREQUOTE = CURLOPTTYPE_SLISTPOINT + 93,

            /* set the debug function */
            DEBUGFUNCTION = CURLOPTTYPE_FUNCTIONPOINT + 94,

            /* set the data for the debug function */
            DEBUGDATA = CURLOPTTYPE_OBJECTPOINT + 95,

            /* mark this as start of a cookie session */
            COOKIESESSION = CURLOPTTYPE_LONG + 96,

            /* The CApath directory used to validate the peer certificate
                this option is used only if SSL_VERIFYPEER is true */
            CAPATH = CURLOPTTYPE_STRINGPOINT + 97,

            /* Instruct libcurl to use a smaller receive buffer */
            BUFFERSIZE = CURLOPTTYPE_LONG + 98,

            /* Instruct libcurl to not use any signal/alarm handlers, even when using
                timeouts. This option is useful for multi-threaded applications.
                See libcurl-the-guide for more background information. */
            NOSIGNAL = CURLOPTTYPE_LONG + 99,

            /* Provide a CURLShare for mutexing non-ts data */
            SHARE = CURLOPTTYPE_OBJECTPOINT + 100,

            /* indicates type of proxy. accepted values are CURLPROXY_HTTP (default),
                CURLPROXY_HTTPS, CURLPROXY_SOCKS4, CURLPROXY_SOCKS4A and
                CURLPROXY_SOCKS5. */
            PROXYTYPE = CURLOPTTYPE_LONG + 101,

            /* Set the Accept-Encoding string. Use this to tell a server you would like
                the response to be compressed. Before 7.21.6, this was known as
                ENCODING */
            ACCEPT_ENCODING = CURLOPTTYPE_STRINGPOINT + 102,

            /* Set pointer to private data */
            PRIVATE = CURLOPTTYPE_OBJECTPOINT + 103,

            /* Set aliases for HTTP 200 in the HTTP Response header */
            HTTP200ALIASES = CURLOPTTYPE_SLISTPOINT + 104,

            /* Continue to send authentication (user+password) when following locations,
                even when hostname changed. This can potentially send off the name
                and password to whatever host the server decides. */
            UNRESTRICTED_AUTH = CURLOPTTYPE_LONG + 105,

            /* Specifically switch on or off the FTP engine's use of the EPRT command (
                it also disables the LPRT attempt). By default, those ones will always be
                attempted before the good old traditional PORT command. */
            FTP_USE_EPRT = CURLOPTTYPE_LONG + 106,

            /* Set this to a bitmask value to enable the particular authentications
                methods you like. Use this in combination with USERPWD.
                Note that setting multiple bits may cause extra network round-trips. */
            HTTPAUTH = CURLOPTTYPE_LONG + 107,

            /* Set the ssl context callback function, currently only for OpenSSL or
                WolfSSL ssl_ctx, or mbedTLS mbedtls_ssl_config in the second argument.
                The function must match the curl_ssl_ctx_callback prototype. */
            SSL_CTX_FUNCTION = CURLOPTTYPE_FUNCTIONPOINT + 108,

            /* Set the userdata for the ssl context callback function's third
                argument */
            SSL_CTX_DATA = CURLOPTTYPE_OBJECTPOINT + 109,

            /* FTP Option that causes missing dirs to be created on the remote server.
                In 7.19.4 we introduced the convenience enums for this option using the
                CURLFTP_CREATE_DIR prefix.
            */
            FTP_CREATE_MISSING_DIRS = CURLOPTTYPE_LONG + 110,

            /* Set this to a bitmask value to enable the particular authentications
                methods you like. Use this in combination with PROXYUSERPWD.
                Note that setting multiple bits may cause extra network round-trips. */
            PROXYAUTH = CURLOPTTYPE_LONG + 111,

            /* FTP option that changes the timeout, in seconds, associated with
                getting a response.  This is different from transfer timeout time and
                essentially places a demand on the FTP server to acknowledge commands
                in a timely manner. */
            FTP_RESPONSE_TIMEOUT = CURLOPTTYPE_LONG + 112,
            SERVER_RESPONSE_TIMEOUT = FTP_RESPONSE_TIMEOUT,

            /* Set this option to one of the CURL_IPRESOLVE_* defines (see below) to
                tell libcurl to resolve names to those IP versions only. This only has
                affect on systems with support for more than one, i.e IPv4 _and_ IPv6. */
            IPRESOLVE = CURLOPTTYPE_LONG + 113,

            /* Set this option to limit the size of a file that will be downloaded from
                an HTTP or FTP server.

                Note there is also _LARGE version which adds large file support for
                platforms which have larger off_t sizes.  See MAXFILESIZE_LARGE below. */
            MAXFILESIZE = CURLOPTTYPE_LONG + 114,

            /* See the comment for INFILESIZE above, but in short, specifies
            * the size of the file being uploaded.  -1 means unknown.
            */
            INFILESIZE_LARGE = CURLOPTTYPE_OFF_T + 115,

            /* Sets the continuation offset.  There is also a LONG version of this;
            * look above for RESUME_FROM.
            */
            RESUME_FROM_LARGE = CURLOPTTYPE_OFF_T + 116,

            /* Sets the maximum size of data that will be downloaded from
            * an HTTP or FTP server.  See MAXFILESIZE above for the LONG version.
            */
            MAXFILESIZE_LARGE = CURLOPTTYPE_OFF_T + 117,

            /* Set this option to the file name of your .netrc file you want libcurl
                to parse (using the NETRC option). If not set, libcurl will do
                a poor attempt to find the user's home directory and check for a .netrc
                file in there. */
            NETRC_FILE = CURLOPTTYPE_STRINGPOINT + 118,

            /* Enable SSL/TLS for FTP, pick one of:
                CURLUSESSL_TRY     - try using SSL, proceed anyway otherwise
                CURLUSESSL_CONTROL - SSL for the control connection or fail
                CURLUSESSL_ALL     - SSL for all communication or fail
            */
            USE_SSL = CURLOPTTYPE_LONG + 119,

            /* The _LARGE version of the standard POSTFIELDSIZE option */
            POSTFIELDSIZE_LARGE = CURLOPTTYPE_OFF_T + 120,

            /* Enable/disable the TCP Nagle algorithm */
            TCP_NODELAY = CURLOPTTYPE_LONG + 121,

            /* 122 OBSOLETE, used in 7.12.3. Gone in 7.13.0 */
            /* 123 OBSOLETE. Gone in 7.16.0 */
            /* 124 OBSOLETE, used in 7.12.3. Gone in 7.13.0 */
            /* 125 OBSOLETE, used in 7.12.3. Gone in 7.13.0 */
            /* 126 OBSOLETE, used in 7.12.3. Gone in 7.13.0 */
            /* 127 OBSOLETE. Gone in 7.16.0 */
            /* 128 OBSOLETE. Gone in 7.16.0 */

            /* When FTP over SSL/TLS is selected (with USE_SSL), this option
                can be used to change libcurl's default action which is to first try
                "AUTH SSL" and then "AUTH TLS" in this order, and proceed when a OK
                response has been received.

                Available parameters are:
                CURLFTPAUTH_DEFAULT - let libcurl decide
                CURLFTPAUTH_SSL     - try "AUTH SSL" first, then TLS
                CURLFTPAUTH_TLS     - try "AUTH TLS" first, then SSL
            */
            FTPSSLAUTH = CURLOPTTYPE_LONG + 129,

            IOCTLFUNCTION = CURLOPTTYPE_FUNCTIONPOINT + 130,
            IOCTLDATA = CURLOPTTYPE_OBJECTPOINT + 131,

            /* 132 OBSOLETE. Gone in 7.16.0 */
            /* 133 OBSOLETE. Gone in 7.16.0 */

            /* zero terminated string for pass on to the FTP server when asked for
                "account" info */
            FTP_ACCOUNT = CURLOPTTYPE_STRINGPOINT + 134,

            /* feed cookie into cookie engine */
            COOKIELIST = CURLOPTTYPE_STRINGPOINT + 135,

            /* ignore Content-Length */
            IGNORE_CONTENT_LENGTH = CURLOPTTYPE_LONG + 136,

            /* Set to non-zero to skip the IP address received in a 227 PASV FTP server
                response. Typically used for FTP-SSL purposes but is not restricted to
                that. libcurl will then instead use the same IP address it used for the
                control connection. */
            FTP_SKIP_PASV_IP = CURLOPTTYPE_LONG + 137,

            /* Select "file method" to use when doing FTP, see the curl_ftpmethod
                above. */
            FTP_FILEMETHOD = CURLOPTTYPE_LONG + 138,

            /* Local port number to bind the socket to */
            LOCALPORT = CURLOPTTYPE_LONG + 139,

            /* Number of ports to try, including the first one set with LOCALPORT.
                Thus, setting it to 1 will make no additional attempts but the first.
            */
            LOCALPORTRANGE = CURLOPTTYPE_LONG + 140,

            /* no transfer, set up connection and let application use the socket by
                extracting it with CURLINFO_LASTSOCKET */
            CONNECT_ONLY = CURLOPTTYPE_LONG + 141,

            /* Function that will be called to convert from the
                network encoding (instead of using the iconv calls in libcurl) */
            CONV_FROM_NETWORK_FUNCTION = CURLOPTTYPE_FUNCTIONPOINT + 142,

            /* Function that will be called to convert to the
                network encoding (instead of using the iconv calls in libcurl) */
            CONV_TO_NETWORK_FUNCTION = CURLOPTTYPE_FUNCTIONPOINT + 143,

            /* Function that will be called to convert from UTF8
                (instead of using the iconv calls in libcurl)
                Note that this is used only for SSL certificate processing */
            CONV_FROM_UTF8_FUNCTION = CURLOPTTYPE_FUNCTIONPOINT + 144,

            /* if the connection proceeds too quickly then need to slow it down */
            /* limit-rate: maximum number of bytes per second to send or receive */
            MAX_SEND_SPEED_LARGE = CURLOPTTYPE_OFF_T + 145,
            MAX_RECV_SPEED_LARGE = CURLOPTTYPE_OFF_T + 146,

            /* Pointer to command string to send if USER/PASS fails. */
            FTP_ALTERNATIVE_TO_USER = CURLOPTTYPE_STRINGPOINT + 147,

            /* callback function for setting socket options */
            SOCKOPTFUNCTION = CURLOPTTYPE_FUNCTIONPOINT + 148,
            SOCKOPTDATA = CURLOPTTYPE_OBJECTPOINT + 149,

            /* set to 0 to disable session ID re-use for this transfer, default is
                enabled (== 1) */
            SSL_SESSIONID_CACHE = CURLOPTTYPE_LONG + 150,

            /* allowed SSH authentication methods */
            SSH_AUTH_TYPES = CURLOPTTYPE_LONG + 151,

            /* Used by scp/sftp to do public/private key authentication */
            SSH_PUBLIC_KEYFILE = CURLOPTTYPE_STRINGPOINT + 152,
            SSH_PRIVATE_KEYFILE = CURLOPTTYPE_STRINGPOINT + 153,

            /* Send CCC (Clear Command Channel) after authentication */
            FTP_SSL_CCC = CURLOPTTYPE_LONG + 154,

            /* Same as TIMEOUT and CONNECTTIMEOUT, but with ms resolution */
            TIMEOUT_MS = CURLOPTTYPE_LONG + 155,
            CONNECTTIMEOUT_MS = CURLOPTTYPE_LONG + 156,

            /* set to zero to disable the libcurl's decoding and thus pass the raw body
                data to the application even when it is encoded/compressed */
            HTTP_TRANSFER_DECODING = CURLOPTTYPE_LONG + 157,
            HTTP_CONTENT_DECODING = CURLOPTTYPE_LONG + 158,

            /* Permission used when creating new files and directories on the remote
                server for protocols that support it, SFTP/SCP/FILE */
            NEW_FILE_PERMS = CURLOPTTYPE_LONG + 159,
            NEW_DIRECTORY_PERMS = CURLOPTTYPE_LONG + 160,

            /* Set the behaviour of POST when redirecting. Values must be set to one
                of CURL_REDIR* defines below. This used to be called POST301 */
            POSTREDIR = CURLOPTTYPE_LONG + 161,

            /* used by scp/sftp to verify the host's public key */
            SSH_HOST_PUBLIC_KEY_MD5 = CURLOPTTYPE_STRINGPOINT + 162,

            /* Callback function for opening socket (instead of socket(2)). Optionally,
                callback is able change the address or refuse to connect returning
                CURL_SOCKET_BAD.  The callback should have type
                curl_opensocket_callback */
            OPENSOCKETFUNCTION = CURLOPTTYPE_FUNCTIONPOINT + 163,
            OPENSOCKETDATA = CURLOPTTYPE_OBJECTPOINT + 164,

            /* POST volatile input fields. */
            COPYPOSTFIELDS = CURLOPTTYPE_OBJECTPOINT + 165,

            /* set transfer mode (;type=<a|i>) when doing FTP via an HTTP proxy */
            PROXY_TRANSFER_MODE = CURLOPTTYPE_LONG + 166,

            /* Callback function for seeking in the input stream */
            SEEKFUNCTION = CURLOPTTYPE_FUNCTIONPOINT + 167,
            SEEKDATA = CURLOPTTYPE_OBJECTPOINT + 168,

            /* CRL file */
            CRLFILE = CURLOPTTYPE_STRINGPOINT + 169,

            /* Issuer certificate */
            ISSUERCERT = CURLOPTTYPE_STRINGPOINT + 170,

            /* (IPv6) Address scope */
            ADDRESS_SCOPE = CURLOPTTYPE_LONG + 171,

            /* Collect certificate chain info and allow it to get retrievable with
                CURLINFO_CERTINFO after the transfer is complete. */
            CERTINFO = CURLOPTTYPE_LONG + 172,

            /* "name" and "pwd" to use when fetching. */
            USERNAME = CURLOPTTYPE_STRINGPOINT + 173,
            PASSWORD = CURLOPTTYPE_STRINGPOINT + 174,

            /* "name" and "pwd" to use with Proxy when fetching. */
            PROXYUSERNAME = CURLOPTTYPE_STRINGPOINT + 175,
            PROXYPASSWORD = CURLOPTTYPE_STRINGPOINT + 176,

            /* Comma separated list of hostnames defining no-proxy zones. These should
                match both hostnames directly, and hostnames within a domain. For
                example, local.com will match local.com and www.local.com, but NOT
                notlocal.com or www.notlocal.com. For compatibility with other
                implementations of this, .local.com will be considered to be the same as
                local.com. A single * is the only valid wildcard, and effectively
                disables the use of proxy. */
            NOPROXY = CURLOPTTYPE_STRINGPOINT + 177,

            /* block size for TFTP transfers */
            TFTP_BLKSIZE = CURLOPTTYPE_LONG + 178,

            /* Socks Service */
            SOCKS5_GSSAPI_SERVICE = CURLOPTTYPE_STRINGPOINT + 179, /* DEPRECATED, do not use! */

            /* Socks Service */
            SOCKS5_GSSAPI_NEC = CURLOPTTYPE_LONG + 180,

            /* set the bitmask for the protocols that are allowed to be used for the
                transfer, which thus helps the app which takes URLs from users or other
                external inputs and want to restrict what protocol(s) to deal
                with. Defaults to CURLPROTO_ALL. */
            PROTOCOLS = CURLOPTTYPE_LONG + 181,

            /* set the bitmask for the protocols that libcurl is allowed to follow to,
                as a subset of the PROTOCOLS ones. That means the protocol needs
                to be set in both bitmasks to be allowed to get redirected to. */
            REDIR_PROTOCOLS = CURLOPTTYPE_LONG + 182,

            /* set the SSH knownhost file name to use */
            SSH_KNOWNHOSTS = CURLOPTTYPE_STRINGPOINT + 183,

            /* set the SSH host key callback, must point to a curl_sshkeycallback
                function */
            SSH_KEYFUNCTION = CURLOPTTYPE_FUNCTIONPOINT + 184,

            /* set the SSH host key callback custom pointer */
            SSH_KEYDATA = CURLOPTTYPE_OBJECTPOINT + 185,

            /* set the SMTP mail originator */
            MAIL_FROM = CURLOPTTYPE_STRINGPOINT + 186,

            /* set the list of SMTP mail receiver(s) */
            MAIL_RCPT = CURLOPTTYPE_SLISTPOINT + 187,

            /* FTP: send PRET before PASV */
            FTP_USE_PRET = CURLOPTTYPE_LONG + 188,

            /* RTSP request method (OPTIONS, SETUP, PLAY, etc...) */
            RTSP_REQUEST = CURLOPTTYPE_LONG + 189,

            /* The RTSP session identifier */
            RTSP_SESSION_ID = CURLOPTTYPE_STRINGPOINT + 190,

            /* The RTSP stream URI */
            RTSP_STREAM_URI = CURLOPTTYPE_STRINGPOINT + 191,

            /* The Transport: header to use in RTSP requests */
            RTSP_TRANSPORT = CURLOPTTYPE_STRINGPOINT + 192,

            /* Manually initialize the client RTSP CSeq for this handle */
            RTSP_CLIENT_CSEQ = CURLOPTTYPE_LONG + 193,

            /* Manually initialize the server RTSP CSeq for this handle */
            RTSP_SERVER_CSEQ = CURLOPTTYPE_LONG + 194,

            /* The stream to pass to INTERLEAVEFUNCTION. */
            INTERLEAVEDATA = CURLOPTTYPE_OBJECTPOINT + 195,

            /* Let the application define a custom write method for RTP data */
            INTERLEAVEFUNCTION = CURLOPTTYPE_FUNCTIONPOINT + 196,

            /* Turn on wildcard matching */
            WILDCARDMATCH = CURLOPTTYPE_LONG + 197,

            /* Directory matching callback called before downloading of an
                individual file (chunk) started */
            CHUNK_BGN_FUNCTION = CURLOPTTYPE_FUNCTIONPOINT + 198,

            /* Directory matching callback called after the file (chunk)
                was downloaded, or skipped */
            CHUNK_END_FUNCTION = CURLOPTTYPE_FUNCTIONPOINT + 199,

            /* Change match (fnmatch-like) callback for wildcard matching */
            FNMATCH_FUNCTION = CURLOPTTYPE_FUNCTIONPOINT + 200,

            /* Let the application define custom chunk data pointer */
            CHUNK_DATA = CURLOPTTYPE_OBJECTPOINT + 201,

            /* FNMATCH_FUNCTION user pointer */
            FNMATCH_DATA = CURLOPTTYPE_OBJECTPOINT + 202,

            /* send linked-list of name:port:address sets */
            RESOLVE = CURLOPTTYPE_SLISTPOINT + 203,

            /* Set a username for authenticated TLS */
            TLSAUTH_USERNAME = CURLOPTTYPE_STRINGPOINT + 204,

            /* Set a password for authenticated TLS */
            TLSAUTH_PASSWORD = CURLOPTTYPE_STRINGPOINT + 205,

            /* Set authentication type for authenticated TLS */
            TLSAUTH_TYPE = CURLOPTTYPE_STRINGPOINT + 206,

            /* Set to 1 to enable the "TE:" header in HTTP requests to ask for
                compressed transfer-encoded responses. Set to 0 to disable the use of TE:
                in outgoing requests. The current default is 0, but it might change in a
                future libcurl release.

                libcurl will ask for the compressed methods it knows of, and if that
                isn't any, it will not ask for transfer-encoding at all even if this
                option is set to 1.

            */
            TRANSFER_ENCODING = CURLOPTTYPE_LONG + 207,

            /* Callback function for closing socket (instead of close(2)). The callback
                should have type curl_closesocket_callback */
            CLOSESOCKETFUNCTION = CURLOPTTYPE_FUNCTIONPOINT + 208,
            CLOSESOCKETDATA = CURLOPTTYPE_OBJECTPOINT + 209,

            /* allow GSSAPI credential delegation */
            GSSAPI_DELEGATION = CURLOPTTYPE_LONG + 210,

            /* Set the name servers to use for DNS resolution */
            DNS_SERVERS = CURLOPTTYPE_STRINGPOINT + 211,

            /* Time-out accept operations (currently for FTP only) after this amount
                of milliseconds. */
            ACCEPTTIMEOUT_MS = CURLOPTTYPE_LONG + 212,

            /* Set TCP keepalive */
            TCP_KEEPALIVE = CURLOPTTYPE_LONG + 213,

            /* non-universal keepalive knobs (Linux, AIX, HP-UX, more) */
            TCP_KEEPIDLE = CURLOPTTYPE_LONG + 214,
            TCP_KEEPINTVL = CURLOPTTYPE_LONG + 215,

            /* Enable/disable specific SSL features with a bitmask, see CURLSSLOPT_* */
            SSL_OPTIONS = CURLOPTTYPE_LONG + 216,

            /* Set the SMTP auth originator */
            MAIL_AUTH = CURLOPTTYPE_STRINGPOINT + 217,

            /* Enable/disable SASL initial response */
            SASL_IR = CURLOPTTYPE_LONG + 218,

            /* Function that will be called instead of the internal progress display
            * function. This function should be defined as the curl_xferinfo_callback
            * prototype defines. (Deprecates PROGRESSFUNCTION) */
            XFERINFOFUNCTION = CURLOPTTYPE_FUNCTIONPOINT + 219,

            /* The XOAUTH2 bearer token */
            XOAUTH2_BEARER = CURLOPTTYPE_STRINGPOINT + 220,

            /* Set the interface string to use as outgoing network
            * interface for DNS requests.
            * Only supported by the c-ares DNS backend */
            DNS_INTERFACE = CURLOPTTYPE_STRINGPOINT + 221,

            /* Set the local IPv4 address to use for outgoing DNS requests.
            * Only supported by the c-ares DNS backend */
            DNS_LOCAL_IP4 = CURLOPTTYPE_STRINGPOINT + 222,

            /* Set the local IPv6 address to use for outgoing DNS requests.
            * Only supported by the c-ares DNS backend */
            DNS_LOCAL_IP6 = CURLOPTTYPE_STRINGPOINT + 223,

            /* Set authentication options directly */
            LOGIN_OPTIONS = CURLOPTTYPE_STRINGPOINT + 224,

            /* Enable/disable TLS NPN extension (http2 over ssl might fail without) */
            SSL_ENABLE_NPN = CURLOPTTYPE_LONG + 225,

            /* Enable/disable TLS ALPN extension (http2 over ssl might fail without) */
            SSL_ENABLE_ALPN = CURLOPTTYPE_LONG + 226,

            /* Time to wait for a response to a HTTP request containing an
            * Expect: 100-continue header before sending the data anyway. */
            EXPECT_100_TIMEOUT_MS = CURLOPTTYPE_LONG + 227,

            /* This points to a linked list of headers used for proxy requests only,
                struct curl_slist kind */
            PROXYHEADER = CURLOPTTYPE_SLISTPOINT + 228,

            /* Pass in a bitmask of "header options" */
            HEADEROPT = CURLOPTTYPE_LONG + 229,

            /* The public key in DER form used to validate the peer public key
                this option is used only if SSL_VERIFYPEER is true */
            PINNEDPUBLICKEY = CURLOPTTYPE_STRINGPOINT + 230,

            /* Path to Unix domain socket */
            UNIX_SOCKET_PATH = CURLOPTTYPE_STRINGPOINT + 231,

            /* Set if we should verify the certificate status. */
            SSL_VERIFYSTATUS = CURLOPTTYPE_LONG + 232,

            /* Set if we should enable TLS false start. */
            SSL_FALSESTART = CURLOPTTYPE_LONG + 233,

            /* Do not squash dot-dot sequences */
            PATH_AS_IS = CURLOPTTYPE_LONG + 234,

            /* Proxy Service Name */
            PROXY_SERVICE_NAME = CURLOPTTYPE_STRINGPOINT + 235,

            /* Service Name */
            SERVICE_NAME = CURLOPTTYPE_STRINGPOINT + 236,

            /* Wait/don't wait for pipe/mutex to clarify */
            PIPEWAIT = CURLOPTTYPE_LONG + 237,

            /* Set the protocol used when curl is given a URL without a protocol */
            DEFAULT_PROTOCOL = CURLOPTTYPE_STRINGPOINT + 238,

            /* Set stream weight, 1 - 256 (default is 16) */
            STREAM_WEIGHT = CURLOPTTYPE_LONG + 239,

            /* Set stream dependency on another CURL handle */
            STREAM_DEPENDS = CURLOPTTYPE_OBJECTPOINT + 240,

            /* Set E-xclusive stream dependency on another CURL handle */
            STREAM_DEPENDS_E = CURLOPTTYPE_OBJECTPOINT + 241,

            /* Do not send any tftp option requests to the server */
            TFTP_NO_OPTIONS = CURLOPTTYPE_LONG + 242,

            /* Linked-list of host:port:connect-to-host:connect-to-port,
                overrides the URL's host:port (only for the network layer) */
            CONNECT_TO = CURLOPTTYPE_SLISTPOINT + 243,

            /* Set TCP Fast Open */
            TCP_FASTOPEN = CURLOPTTYPE_LONG + 244,

            /* Continue to send data if the server responds early with an
            * HTTP status code >= 300 */
            KEEP_SENDING_ON_ERROR = CURLOPTTYPE_LONG + 245,

            /* The CApath or CAfile used to validate the proxy certificate
                this option is used only if PROXY_SSL_VERIFYPEER is true */
            PROXY_CAINFO = CURLOPTTYPE_STRINGPOINT + 246,

            /* The CApath directory used to validate the proxy certificate
                this option is used only if PROXY_SSL_VERIFYPEER is true */
            PROXY_CAPATH = CURLOPTTYPE_STRINGPOINT + 247,

            /* Set if we should verify the proxy in ssl handshake,
                set 1 to verify. */
            PROXY_SSL_VERIFYPEER = CURLOPTTYPE_LONG + 248,

            /* Set if we should verify the Common name from the proxy certificate in ssl
            * handshake, set 1 to check existence, 2 to ensure that it matches
            * the provided hostname. */
            PROXY_SSL_VERIFYHOST = CURLOPTTYPE_LONG + 249,

            /* What version to specifically try to use for proxy.
                See CURL_SSLVERSION defines below. */
            PROXY_SSLVERSION = CURLOPTTYPE_LONG + 250,

            /* Set a username for authenticated TLS for proxy */
            PROXY_TLSAUTH_USERNAME = CURLOPTTYPE_STRINGPOINT + 251,

            /* Set a password for authenticated TLS for proxy */
            PROXY_TLSAUTH_PASSWORD = CURLOPTTYPE_STRINGPOINT + 252,

            /* Set authentication type for authenticated TLS for proxy */
            PROXY_TLSAUTH_TYPE = CURLOPTTYPE_STRINGPOINT + 253,

            /* name of the file keeping your private SSL-certificate for proxy */
            PROXY_SSLCERT = CURLOPTTYPE_STRINGPOINT + 254,

            /* type of the file keeping your SSL-certificate ("DER", "PEM", "ENG") for
                proxy */
            PROXY_SSLCERTTYPE = CURLOPTTYPE_STRINGPOINT + 255,

            /* name of the file keeping your private SSL-key for proxy */
            PROXY_SSLKEY = CURLOPTTYPE_STRINGPOINT + 256,

            /* type of the file keeping your private SSL-key ("DER", "PEM", "ENG") for
                proxy */
            PROXY_SSLKEYTYPE = CURLOPTTYPE_STRINGPOINT + 257,

            /* password for the SSL private key for proxy */
            PROXY_KEYPASSWD = CURLOPTTYPE_STRINGPOINT + 258,

            /* Specify which SSL ciphers to use for proxy */
            PROXY_SSL_CIPHER_LIST = CURLOPTTYPE_STRINGPOINT + 259,

            /* CRL file for proxy */
            PROXY_CRLFILE = CURLOPTTYPE_STRINGPOINT + 260,

            /* Enable/disable specific SSL features with a bitmask for proxy, see
                CURLSSLOPT_* */
            PROXY_SSL_OPTIONS = CURLOPTTYPE_LONG + 261,

            /* Name of pre proxy to use. */
            PRE_PROXY = CURLOPTTYPE_STRINGPOINT + 262,

            /* The public key in DER form used to validate the proxy public key
                this option is used only if PROXY_SSL_VERIFYPEER is true */
            PROXY_PINNEDPUBLICKEY = CURLOPTTYPE_STRINGPOINT + 263,

            /* Path to an abstract Unix domain socket */
            ABSTRACT_UNIX_SOCKET = CURLOPTTYPE_STRINGPOINT + 264,

            /* Suppress proxy CONNECT response headers from user callbacks */
            SUPPRESS_CONNECT_HEADERS = CURLOPTTYPE_LONG + 265,

            /* The request target, instead of extracted from the URL */
            REQUEST_TARGET = CURLOPTTYPE_STRINGPOINT + 266,

            /* bitmask of allowed auth methods for connections to SOCKS5 proxies */
            SOCKS5_AUTH = CURLOPTTYPE_LONG + 267,

            /* Enable/disable SSH compression */
            SSH_COMPRESSION = CURLOPTTYPE_LONG + 268,

            /* Post MIME data. */
            MIMEPOST = CURLOPTTYPE_OBJECTPOINT + 269,

            /* Time to use with the TIMECONDITION. Specified in number of
                seconds since 1 Jan 1970. */
            TIMEVALUE_LARGE = CURLOPTTYPE_OFF_T + 270,

            /* Head start in milliseconds to give happy eyeballs. */
            HAPPY_EYEBALLS_TIMEOUT_MS = CURLOPTTYPE_LONG + 271,

            /* Function that will be called before a resolver request is made */
            RESOLVER_START_FUNCTION = CURLOPTTYPE_FUNCTIONPOINT + 272,

            /* User data to pass to the resolver start callback. */
            RESOLVER_START_DATA = CURLOPTTYPE_OBJECTPOINT + 273,

            /* send HAProxy PROXY protocol header? */
            HAPROXYPROTOCOL = CURLOPTTYPE_LONG + 274,

            /* shuffle addresses before use when DNS returns multiple */
            DNS_SHUFFLE_ADDRESSES = CURLOPTTYPE_LONG + 275,

            /* Specify which TLS 1.3 ciphers suites to use */
            TLS13_CIPHERS = CURLOPTTYPE_STRINGPOINT + 276,
            PROXY_TLS13_CIPHERS = CURLOPTTYPE_STRINGPOINT + 277,

            /* Disallow specifying username/login in URL. */
            DISALLOW_USERNAME_IN_URL = CURLOPTTYPE_LONG + 278,

            /* DNS-over-HTTPS URL */
            DOH_URL = CURLOPTTYPE_STRINGPOINT + 279,

            /* Preferred buffer size to use for uploads */
            UPLOAD_BUFFERSIZE = CURLOPTTYPE_LONG + 280,

            /* Time in ms between connection upkeep calls for long-lived connections. */
            UPKEEP_INTERVAL_MS = CURLOPTTYPE_LONG + 281,

            /* Specify URL using CURL URL API. */
            CURLU = CURLOPTTYPE_OBJECTPOINT + 282,

            /* add trailing data just after no more data is available */
            TRAILERFUNCTION = CURLOPTTYPE_FUNCTIONPOINT + 283,

            /* pointer to be passed to HTTP_TRAILER_FUNCTION */
            TRAILERDATA = CURLOPTTYPE_OBJECTPOINT + 284,

            /* set this to 1L to allow HTTP/0.9 responses or 0L to disallow */
            HTTP09_ALLOWED = CURLOPTTYPE_LONG + 285,

            /* alt-svc control bitmask */
            ALTSVC_CTRL = CURLOPTTYPE_LONG + 286,

            /* alt-svc cache file name to possibly read from/write to */
            ALTSVC = CURLOPTTYPE_STRINGPOINT + 287,

            /* maximum age of a connection to consider it for reuse (in seconds) */
            MAXAGE_CONN = CURLOPTTYPE_LONG + 288,

            /* SASL authorisation identity */
            SASL_AUTHZID = CURLOPTTYPE_STRINGPOINT + 289,

            LASTENTRY /* the last unused */
        };

        private const int CURLINFO_STRING = 0x100000;
        private const int CURLINFO_LONG = 0x200000;
        private const int CURLINFO_DOUBLE = 0x300000;
        private const int CURLINFO_SLIST = 0x400000;
        private const int CURLINFO_PTR = 0x400000 /* same as SLIST */;
        private const int CURLINFO_SOCKET = 0x500000;
        private const int CURLINFO_OFF_T = 0x600000;
        private const int CURLINFO_MASK = 0x0fffff;
        private const int CURLINFO_TYPEMASK = 0xf00000;

        private enum CURLINFO {
            NONE, /* first, never use this */
            EFFECTIVE_URL = CURLINFO_STRING + 1,
            RESPONSE_CODE = CURLINFO_LONG + 2,
            TOTAL_TIME = CURLINFO_DOUBLE + 3,
            NAMELOOKUP_TIME = CURLINFO_DOUBLE + 4,
            CONNECT_TIME = CURLINFO_DOUBLE + 5,
            PRETRANSFER_TIME = CURLINFO_DOUBLE + 6,
            SIZE_UPLOAD = CURLINFO_DOUBLE + 7,
            SIZE_UPLOAD_T = CURLINFO_OFF_T + 7,
            SIZE_DOWNLOAD = CURLINFO_DOUBLE + 8,
            SIZE_DOWNLOAD_T = CURLINFO_OFF_T + 8,
            SPEED_DOWNLOAD = CURLINFO_DOUBLE + 9,
            SPEED_DOWNLOAD_T = CURLINFO_OFF_T + 9,
            SPEED_UPLOAD = CURLINFO_DOUBLE + 10,
            SPEED_UPLOAD_T = CURLINFO_OFF_T + 10,
            HEADER_SIZE = CURLINFO_LONG + 11,
            REQUEST_SIZE = CURLINFO_LONG + 12,
            SSL_VERIFYRESULT = CURLINFO_LONG + 13,
            FILETIME = CURLINFO_LONG + 14,
            FILETIME_T = CURLINFO_OFF_T + 14,
            CONTENT_LENGTH_DOWNLOAD = CURLINFO_DOUBLE + 15,
            CONTENT_LENGTH_DOWNLOAD_T = CURLINFO_OFF_T + 15,
            CONTENT_LENGTH_UPLOAD = CURLINFO_DOUBLE + 16,
            CONTENT_LENGTH_UPLOAD_T = CURLINFO_OFF_T + 16,
            STARTTRANSFER_TIME = CURLINFO_DOUBLE + 17,
            CONTENT_TYPE = CURLINFO_STRING + 18,
            REDIRECT_TIME = CURLINFO_DOUBLE + 19,
            REDIRECT_COUNT = CURLINFO_LONG + 20,
            PRIVATE = CURLINFO_STRING + 21,
            HTTP_CONNECTCODE = CURLINFO_LONG + 22,
            HTTPAUTH_AVAIL = CURLINFO_LONG + 23,
            PROXYAUTH_AVAIL = CURLINFO_LONG + 24,
            OS_ERRNO = CURLINFO_LONG + 25,
            NUM_CONNECTS = CURLINFO_LONG + 26,
            SSL_ENGINES = CURLINFO_SLIST + 27,
            COOKIELIST = CURLINFO_SLIST + 28,
            LASTSOCKET = CURLINFO_LONG + 29,
            FTP_ENTRY_PATH = CURLINFO_STRING + 30,
            REDIRECT_URL = CURLINFO_STRING + 31,
            PRIMARY_IP = CURLINFO_STRING + 32,
            APPCONNECT_TIME = CURLINFO_DOUBLE + 33,
            CERTINFO = CURLINFO_PTR + 34,
            CONDITION_UNMET = CURLINFO_LONG + 35,
            RTSP_SESSION_ID = CURLINFO_STRING + 36,
            RTSP_CLIENT_CSEQ = CURLINFO_LONG + 37,
            RTSP_SERVER_CSEQ = CURLINFO_LONG + 38,
            RTSP_CSEQ_RECV = CURLINFO_LONG + 39,
            PRIMARY_PORT = CURLINFO_LONG + 40,
            LOCAL_IP = CURLINFO_STRING + 41,
            LOCAL_PORT = CURLINFO_LONG + 42,
            TLS_SESSION = CURLINFO_PTR + 43,
            ACTIVESOCKET = CURLINFO_SOCKET + 44,
            TLS_SSL_PTR = CURLINFO_PTR + 45,
            HTTP_VERSION = CURLINFO_LONG + 46,
            PROXY_SSL_VERIFYRESULT = CURLINFO_LONG + 47,
            PROTOCOL = CURLINFO_LONG + 48,
            SCHEME = CURLINFO_STRING + 49,
            /* Fill in new entries below here! */

            /* Preferably these would be defined conditionally based on the
               sizeof curl_off_t being 64-bits */
            TOTAL_TIME_T = CURLINFO_OFF_T + 50,
            NAMELOOKUP_TIME_T = CURLINFO_OFF_T + 51,
            CONNECT_TIME_T = CURLINFO_OFF_T + 52,
            PRETRANSFER_TIME_T = CURLINFO_OFF_T + 53,
            STARTTRANSFER_TIME_T = CURLINFO_OFF_T + 54,
            REDIRECT_TIME_T = CURLINFO_OFF_T + 55,
            APPCONNECT_TIME_T = CURLINFO_OFF_T + 56,
            RETRY_AFTER = CURLINFO_OFF_T + 57,

            LASTONE = 57
        };
        #endregion

        public delegate bool ProgressInfoCallback(long resumed, long dltotal, long dlnow, long ultotal, long ulnow);

        public static void Init(string pluginDirectory)
        {
            USER_AGENT = "ngld/OverlayPlugin v" + Assembly.GetExecutingAssembly().GetName().Version.ToString();

            var libPath = Path.Combine(pluginDirectory, "libs", Environment.Is64BitProcess ? "x64" : "x86", "libcurl.dll");
            if (!File.Exists(libPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(libPath));

                using (var stream = File.OpenWrite(libPath))
                {
                    var data = Environment.Is64BitProcess ? Resources.libcurl_x64 : Resources.libcurl;
                    stream.Write(data, 0, data.Length);
                }
            }

            var result = NativeMethods.LoadLibrary(libPath);

            if (result == IntPtr.Zero)
                throw new CurlException($"libcurl.dll load failed: {Marshal.GetLastWin32Error()}");

            lock (_global_lock)
            {
                if (curl_global_init(CURL_GLOBAL_NOTHING) != 0)
                {
                    throw new CurlException($"Init failed!");
                }

                initialized = true;
            }
        }

        public static string Get(string url)
        {
            return Get(url, new Dictionary<string, string>(), null, null, false);
        }

        public static unsafe string Get(string url, Dictionary<string, string> headers, string downloadDest,
            ProgressInfoCallback info_cb, bool resume)
        {
            if (!initialized)
                throw new CurlException("Not initialized!");

            var error = new byte[CURL_ERROR_SIZE];
            error[0] = 0;

            var header_list = IntPtr.Zero;
            var dlInfo = new Download();

            if (downloadDest == null)
            {
                // We have to return the response later as a string so we use a StringBuilder as the download destination.
                dlInfo.builder = new StringBuilder();
            } else
            {
                if (resume)
                {
                    try
                    {
                        dlInfo.handle = File.Open(downloadDest, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                    } catch (FileNotFoundException)
                    {
                        // If the file doesn't exist, we have to create it.
                        dlInfo.handle = File.Open(downloadDest, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                    }

                    if (dlInfo.handle.Position > 0)
                    {
                        headers["Range"] = "bytes=" + dlInfo.handle.Position + "-";
                    }
                } else
                {
                    dlInfo.handle = File.Open(downloadDest, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                }
            }

            // We store our DownloadInfo here and only pass an index to curl since we can't pass complicated objects
            // to unmanaged code and we need this info in DataCallback() later.
            var dlIdx = nextDownloadIdx++;
            downloadInfos[dlIdx] = dlInfo;

            long resumePos = dlInfo.handle == null ? 0 : dlInfo.handle.Position;

            var handle = curl_easy_init();
            if (handle == IntPtr.Zero || handle == null)
                throw new CurlException("curl_easy_init() failed!");

            // Pin the delegates we pass to cURL to make sure the GC doesn't remove them.
            var writeDele = new write_callback(dlInfo.DataCallback);
            var writePin = GCHandle.Alloc(writeDele);

            progress_callback progressDele = null;
            GCHandle? progressPin = null;

            if (info_cb != null)
            {
                dlInfo.infoCallback = info_cb;
                progressDele = new progress_callback(dlInfo.ProgressWrapperCallback);
                progressPin = GCHandle.Alloc(progressDele);
            }

            try
            {
                fixed (byte* errorPtr = error)
                {
                    curl_easy_setopt(handle, CURLoption.URL, url);
                    curl_easy_setopt(handle, CURLoption.ERRORBUFFER, (IntPtr) errorPtr);
                    curl_easy_setopt(handle, CURLoption.ACCEPT_ENCODING, "");
                    curl_easy_setopt(handle, CURLoption.FOLLOWLOCATION, 1L);
                    curl_easy_setopt(handle, CURLoption.REDIR_PROTOCOLS, CURLPROTO_HTTP | CURLPROTO_HTTPS);
                    curl_easy_setopt(handle, CURLoption.MAXREDIRS, 10L);
                    curl_easy_setopt(handle, CURLoption.USERAGENT, USER_AGENT);

                    if (downloadDest == null)
                    {
                        curl_easy_setopt(handle, CURLoption.TIMEOUT, 60L);
                    }

                    curl_easy_setopt(handle, CURLoption.WRITEFUNCTION, writeDele);

                    if (info_cb != null)
                    {
                        curl_easy_setopt(handle, CURLoption.XFERINFOFUNCTION, progressDele);
                        curl_easy_setopt(handle, CURLoption.NOPROGRESS, 0L);
                    }

                    foreach (var pair in headers)
                    {
                        header_list = curl_slist_append(header_list, pair.Key + ": " + pair.Value);
                    }
                    curl_easy_setopt(handle, CURLoption.HTTPHEADER, header_list);

                    var result = curl_easy_perform(handle);
                    if (dlInfo.exception != null) throw dlInfo.exception;

                    if (result != CURLcode.CURLE_OK)
                    {
                        throw new CurlException($"Request to \"{url}\" failed: {result}; {Encoding.UTF8.GetString(error).Trim('\0')}");
                    }

                    curl_easy_getinfo(handle, CURLINFO.RESPONSE_CODE, out long code);
                    if (code != 200 && code != 206)
                    {
                        throw new CurlException($"Request to \"{url}\" failed with code: {code}!");
                    }
                }

                if (dlInfo.handle != null)
                {
                    dlInfo.handle.Close();
                }

                return dlInfo.builder?.ToString();
            } finally
            {
                writePin.Free();
                progressPin?.Free();

                if (dlIdx > -1)
                {
                    if (dlInfo.handle != null)
                        dlInfo.handle.Close();

                    downloadInfos.Remove(dlIdx);
                }

                if (header_list != IntPtr.Zero)
                    curl_slist_free_all(header_list);

                curl_easy_cleanup(handle);
            }
        }

        private class Download
        {
            public byte[] buffer;
            public FileStream handle;
            public StringBuilder builder;
            public Exception exception;
            public long resumed = 0;
            public ProgressInfoCallback infoCallback;

            public UIntPtr DataCallback(IntPtr ptr, UIntPtr size, UIntPtr nmemb, IntPtr userdata)
            {
                var total_size = (int)((uint)size * (uint)nmemb);
                try
                {
                    if (buffer == null || buffer.Length < total_size)
                    {
                        buffer = new byte[total_size + 1024];
                    }

                    // This is ugly but the easiest way to do this for now (unless we want to call FileStream.Write()
                    // for each byte).
                    Marshal.Copy(ptr, buffer, 0, total_size);

                    if (handle != null)
                    {
                        handle.Write(buffer, 0, total_size);
                    }
                    else if (builder != null)
                    {
                        builder.Append(Encoding.UTF8.GetString(buffer, 0, total_size));
                    }
                    else
                    {
                        throw new CurlException("Missing handle or builder!");
                    }
                }
                catch (Exception ex)
                {
                    exception = ex;
                    return (UIntPtr)0;
                }

                return (UIntPtr)total_size;
            }

            public int ProgressWrapperCallback(IntPtr clientp, long dltotal, long dlnow, long ultotal, long ulnow)
            {
                try
                {
                    return infoCallback(resumed, dltotal, dlnow, ultotal, ulnow) ? 1 : 0;
                } catch (Exception ex)
                {
                    exception = ex;
                    return 1;
                }
            }
        }
    }

    [Serializable]
    public class CurlException : Exception
    {
        public CurlException(string message) : base(message)
        {
        }

        public CurlException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
