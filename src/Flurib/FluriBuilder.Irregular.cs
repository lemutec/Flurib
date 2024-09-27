using System;

namespace Flurib;

public partial class FluriBuilder
{
    private static readonly string _schemeDelimiterMailtoAndNews = ":";

    public static readonly string SchemeDelimiter = "://";
    public static readonly string UriSchemeFile = "file";
    public static readonly string UriSchemeFtp = "ftp";
    public static readonly string UriSchemeGopher = "gopher";
    public static readonly string UriSchemeHttp = "http";
    public static readonly string UriSchemeHttps = "https";
    public static readonly string UriSchemeMailto = "mailto";
    public static readonly string UriSchemeNews = "news";
    public static readonly string UriSchemeNntp = "nntp";
    public static readonly string UriSchemeNetPipe = "net.pipe";
    public static readonly string UriSchemeNetTcp = "net.tcp";

    public static string GetSchemeDelimiter(string scheme)
    {
        if (scheme == UriSchemeMailto || scheme == UriSchemeNews)
        {
            return _schemeDelimiterMailtoAndNews;
        }

        return SchemeDelimiter;
    }

    /// <summary>
    /// Create a local file URI
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public FluriBuilder CreateFileUri(string filePath)
    {
        IrregularUri = new Uri(filePath, UriKind.RelativeOrAbsolute);
        return this;
    }

    /// <summary>
    /// Create a relative URI
    /// </summary>
    /// <param name="relativePath"></param>
    /// <returns></returns>
    public FluriBuilder CreateRelativeUri(string relativePath)
    {
        IrregularUri = new Uri(relativePath, UriKind.Relative);
        return this;
    }

    public Uri IrregularUri { get; set; } = null!;
}
