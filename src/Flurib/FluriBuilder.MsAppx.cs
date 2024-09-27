using System;

namespace Flurib;

public partial class FluriBuilder
{
    public static readonly string UriSchemeMsAppx = "ms-appx";

    /// <summary>
    /// Create a Avares URI
    /// </summary>
    /// <param name="msAppxUriPath"></param>
    /// <returns></returns>
    public FluriBuilder CreateMsAppxUri(string msAppxUriPath)
    {
        IrregularUri = new Uri($"{UriSchemeMsAppx}://{msAppxUriPath}", UriKind.RelativeOrAbsolute);
        return this;
    }
}
