using System;

namespace Flurib;

public partial class FluriBuilder
{
    public static readonly string UriSchemePack = "pack";

    /// <summary>
    /// Create a Pack URI
    /// </summary>
    /// <param name="packUriPath"></param>
    /// <returns></returns>
    public FluriBuilder CreatePackUri(string packUriPath)
    {
        IrregularUri = new Uri($"{UriSchemePack}://{packUriPath}", UriKind.Absolute);
        return this;
    }

    /// <summary>
    /// Create a resource URI
    /// pack://application:,,,/Assets/Images/logo.ico
    /// </summary>
    /// <param name="resourcePath"></param>
    /// <returns></returns>
    public FluriBuilder CreateResourceUri(string resourcePath)
    {
        IrregularUri = new Uri($"{UriSchemePack}://application:,,,/{resourcePath}", UriKind.Absolute);
        return this;
    }

    /// <summary>
    /// Create a cross-assembly resource URI
    /// pack://application:,,,/Flurib;component/Assets/Images/logo.ico
    /// </summary>
    /// <param name="assemblyName"></param>
    /// <param name="resourcePath"></param>
    /// <returns></returns>
    public FluriBuilder CreateAssemblyResourceUri(string assemblyName, string resourcePath)
    {
        IrregularUri = new Uri($"{UriSchemePack}://application:,,,/{assemblyName};component/{resourcePath}", UriKind.Absolute);
        return this;
    }
}
