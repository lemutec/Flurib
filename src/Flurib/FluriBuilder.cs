using System;
using System.Text;

namespace Flurib;

/// <summary>
/// Provides a custom constructor for uniform resource identifiers (URIs) and modifies URIs for the System.Uri class.
/// </summary>
public partial class FluriBuilder
{
    private string scheme = default!;
    private string host = default!;
    private int port = default!;
    private string path = default!;
    private string query = default!;
    private string fragment = default!;
    private string username = default!;
    private string password = default!;

    private Uri uri = default!;
    private bool modified = default!;

    /// <summary>
    /// Initializes a new instance of the System.UriBuilder class.
    /// </summary>
    public FluriBuilder()
    {
        Initialize(Uri.UriSchemeHttp, "localhost", -1, string.Empty, string.Empty);
    }

    /// <summary>
    /// Initializes a new instance of the System.UriBuilder class with the specified URI.
    /// </summary>
    /// <param name="uri">A URI string.</param>
    /// <exception cref="ArgumentNullException">uri is null.</exception>
    /// <exception cref="UriFormatException">uri is a zero-length string or contains only spaces. -or- The parsing routine detected a scheme in an invalid form. -or- The parser detected more than two consecutive slashes in a URI that does not use the "file" scheme. -or- uri is not a valid URI. Note: In .NET for Windows Store apps or the Portable Class Library, catch the base class exception, System.FormatException, instead.</exception>
    public FluriBuilder(string uri)
    {
        _ = uri ?? throw new ArgumentNullException("uriString");

        if (Uri.TryCreate(uri, UriKind.Absolute, out Uri? u))
        {
            Initialize(u);
        }
        else if (!uri.Contains(Uri.SchemeDelimiter))
        {
            // second chance, UriBuilder parsing is more forgiving than Uri
            Initialize(new Uri(Uri.UriSchemeHttp + Uri.SchemeDelimiter + uri));
        }
        else
        {
            throw new UriFormatException();
        }
    }

    /// <summary>
    /// Initializes a new instance of the System.UriBuilder class with the specified System.Uri instance.
    /// </summary>
    /// <param name="uri">An instance of the System.Uri class.</param>
    /// <exception cref="ArgumentNullException">uri is null.</exception>
    public FluriBuilder(Uri uri)
    {
#if NET_4_0
		if (uri == null)
			throw new ArgumentNullException ("uri");
#endif
        Initialize(uri);
    }

    /// <summary>
    /// Initializes a new instance of the System.UriBuilder class with the specified scheme and host.
    /// </summary>
    /// <param name="schemeName">An Internet access protocol.</param>
    /// <param name="hostName">A DNS-style domain name or IP address.</param>
    public FluriBuilder(string? schemeName, string? hostName)
    {
        Initialize(schemeName, hostName, -1, string.Empty, string.Empty);
    }

    /// <summary>
    /// Initializes a new instance of the System.UriBuilder class with the specified scheme, host, and port.
    /// </summary>
    /// <param name="scheme">An Internet access protocol.</param>
    /// <param name="host">A DNS-style domain name or IP address.</param>
    /// <param name="portNumber">An IP port number for the service.</param>
    /// <exception cref="ArgumentOutOfRangeException">portNumber is less than -1 or greater than 65,535.</exception>
    public FluriBuilder(string? scheme, string? host, int portNumber)
    {
        Initialize(scheme, host, portNumber, string.Empty, string.Empty);
    }

    /// <summary>
    /// Initializes a new instance of the System.UriBuilder class with the specified scheme, host, port number, and path.
    /// </summary>
    /// <param name="scheme">An Internet access protocol.</param>
    /// <param name="host">A DNS-style domain name or IP address.</param>
    /// <param name="port">An IP port number for the service.</param>
    /// <param name="pathValue">The path to the Internet resource.</param>
    /// <exception cref="ArgumentOutOfRangeException">port is less than -1 or greater than 65,535.</exception>
    public FluriBuilder(string? scheme, string? host, int port, string pathValue)
    {
        Initialize(scheme, host, port, pathValue, string.Empty);
    }

    /// <summary>
    /// Initializes a new instance of the System.UriBuilder class with the specified scheme, host, port number, path, and query string or fragment identifier.
    /// </summary>
    /// <param name="scheme">An Internet access protocol.</param>
    /// <param name="host">A DNS-style domain name or IP address.</param>
    /// <param name="port">An IP port number for the service.</param>
    /// <param name="path">The path to the Internet resource.</param>
    /// <param name="extraValue">A query string or fragment identifier.</param>
    /// <exception cref="ArgumentException">extraValue is neither null nor System.String.Empty, nor does a valid fragment identifier begin with a number sign (#), nor a valid query string begin with a question mark (?).</exception>
    /// <exception cref="ArgumentOutOfRangeException">port is less than -1 or greater than 65,535.</exception>
    public FluriBuilder(string? scheme, string? host, int port, string path, string extraValue)
    {
        Initialize(scheme, host, port, path, extraValue);
    }

    private void Initialize(Uri uri)
    {
        Initialize(uri.Scheme, uri.Host, uri.Port, uri.AbsolutePath, string.Empty);
        fragment = uri.Fragment;
        query = uri.Query;
        username = uri.UserInfo;
        int pos = username.IndexOf(':');
        if (pos != -1)
        {
            password = username.Substring(pos + 1);
            username = username.Substring(0, pos);
        }
        else
        {
            password = string.Empty;
        }
    }

    private void Initialize(string? scheme, string? host, int port, string pathValue, string extraValue)
    {
        modified = true;

        Scheme = scheme!;
        Host = host!;
        Port = port;
        Path = pathValue;
        query = string.Empty;
        fragment = string.Empty;
        Path = pathValue;
        username = string.Empty;
        password = string.Empty;

        if (string.IsNullOrEmpty(extraValue))
        {
            return;
        }

        if (extraValue[0] == '#')
        {
            Fragment = extraValue.Remove(0, 1);
        }
        else if (extraValue[0] == '?')
        {
            Query = extraValue.Remove(0, 1);
        }
        else
        {
            throw new ArgumentException("extraValue");
        }
    }

    /// <summary>
    ///  Gets or sets the fragment portion of the URI, including the leading '#' character if not empty.
    /// </summary>
    public string Fragment
    {
        get => fragment;
        set
        {
            fragment = value;
            if (fragment == null)
            {
                fragment = string.Empty;
            }
            else if (fragment.Length > 0)
            {
                fragment = "#" + value.Replace("%23", "#");
            }
            modified = true;
        }
    }

    /// <summary>
    /// Gets or sets the Domain Name System (DNS) host name or IP address of a server.
    /// </summary>
    public string Host
    {
        get => host;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                host = string.Empty;
            }
            else if ((value.IndexOf(':') != -1) && (value[0] != '['))
            {
                host = "[" + value + "]";
            }
            else
            {
                host = value;
            }
            modified = true;
        }
    }

    /// <summary>
    /// Gets or sets the password associated with the user that accesses the URI.
    /// </summary>
    public string Password
    {
        get => password;
        set => password = value ?? string.Empty;
    }

    /// <summary>
    /// Gets or sets the path to the resource referenced by the URI.
    /// </summary>
    public string Path
    {
        get => path;
        set
        {
            if (value == null || value.Length == 0)
            {
                path = "/";
            }
            else
            {
                //TODO: path = Uri.EscapeString(value.Replace('\\', '/'), Uri.EscapeCommonHexBracketsQuery);
                path = value.Replace('\\', '/');
            }
            modified = true;
        }
    }

    /// <summary>
    /// Gets or sets the port number of the URI.
    /// </summary>
    public int Port
    {
        get => port;
        set
        {
            if (value < -1)
            {
                throw new ArgumentOutOfRangeException("value");
            }
            // apparently it is
            port = value;
            modified = true;
        }
    }

    /// <summary>
    /// Gets or sets any query information included in the URI, including the leading '?' character if not empty.
    /// </summary>
    public string Query
    {
        get => query;
        set
        {
            // LAMESPEC: it doesn't say to always prepend a
            // question mark to the value.. it does say this
            // for fragment.
            if (value == null || value.Length == 0)
            {
                query = string.Empty;
            }
            else
            {
                query = "?" + value;
            }

            modified = true;
        }
    }

    /// <summary>
    /// Gets or sets the scheme name of the URI.
    /// </summary>
    public string Scheme
    {
        get => scheme;
        set
        {
            value ??= string.Empty;
            int colonPos = value.IndexOf(':');
            if (colonPos != -1)
            {
                value = value.Substring(0, colonPos);
            }

            scheme = value.ToLower();
            modified = true;
        }
    }

    /// <summary>
    /// Gets the System.Uri instance constructed by the specified System.UriBuilder instance.
    /// </summary>
    public Uri Uri
    {
        get
        {
            if (!modified)
            {
                return uri;
            }

            uri = new Uri(ToString());
            // some properties are updated once the Uri is created - see unit tests
            host = uri.Host;
            path = uri.AbsolutePath;
            modified = false;
            return uri;
        }
    }

    /// <summary>
    /// Gets or sets the user name associated with the user that accesses the URI.
    /// </summary>
    public string UserName
    {
        get => username;
        set
        {
            username = (value == null) ? string.Empty : value;
            modified = true;
        }
    }

    /// <summary>
    /// Compares an existing System.Uri instance with the contents of the System.UriBuilder for equality.
    /// </summary>
    /// <param name="rparam">The object to compare with the current instance.</param>
    /// <returns>true if rparam represents the same System.Uri as the System.Uri constructed by this System.UriBuilder instance; otherwise, false.</returns>
    public override bool Equals(object? rparam)
    {
        return rparam != null && Uri.Equals(rparam.ToString());
    }

    /// <summary>
    /// Returns the hash code for the URI.
    /// </summary>
    /// <returns>The hash code generated for the URI.</returns>
    public override int GetHashCode()
    {
        return Uri.GetHashCode();
    }

    /// <summary>
    /// Returns the display string for the specified System.UriBuilder instance.
    /// </summary>
    /// <returns>The string that contains the unescaped display string of the System.UriBuilder.</returns>
    /// <exception cref="UriFormatException">The System.UriBuilder instance has a bad password. Note: In .NET for Windows Store apps or the Portable Class Library, catch the base class exception, System.FormatException, instead.</exception>
    public override string ToString()
    {
        StringBuilder builder = new();

        builder.Append(scheme);
        // note: mailto and news use ':', not "://", as their delimiter
        builder.Append(GetSchemeDelimiter(scheme));

        if (username != string.Empty)
        {
            builder.Append(username);
            if (password != string.Empty)
            {
                builder.Append(":" + password);
            }

            builder.Append('@');
        }

        if (host.Length > 0)
        {
            builder.Append(host);
            if (port > 0)
            {
                builder.Append(":" + port);
            }
        }

        if (path != string.Empty
         && builder[builder.Length - 1] != '/'
         && path.Length > 0
         && path[0] != '/')
        {
            builder.Append('/');
        }
        builder.Append(path);
        builder.Append(query);
        builder.Append(fragment);

        return builder.ToString();
    }
}
