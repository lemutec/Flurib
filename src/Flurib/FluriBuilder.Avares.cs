using System;

namespace Flurib;

public partial class FluriBuilder
{
    /// <summary>
    /// How to include avares?
    /// <AvaloniaResource Include="Assets\**" />
    /// How to get avares?
    /// AssetLoader.Open(new Uri(uriString));
    /// </summary>
    public static readonly string UriSchemeAvares = "avares";

    /// <summary>
    /// Create a Avares URI
    /// </summary>
    /// <param name="avaresUriPath"></param>
    /// <returns></returns>
    public FluriBuilder CreateAvaresUri(string avaresUriPath)
    {
        IrregularUri = new Uri($"{UriSchemeAvares}://{avaresUriPath}", UriKind.Absolute);
        return this;
    }

    /// <summary>
    /// Create a cross-assembly resource URI
    /// </summary>
    /// <param name="assemblyName"></param>
    /// <param name="resourcePath"></param>
    /// <returns></returns>
    public FluriBuilder CreateAssemblyAvaresUri(string assemblyName, string resourcePath)
    {
        IrregularUri = new Uri($"{UriSchemeAvares}://{assemblyName}/{resourcePath}", UriKind.Absolute);
        return this;
    }
}
