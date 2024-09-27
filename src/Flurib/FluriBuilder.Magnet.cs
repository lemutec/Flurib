using System;

namespace Flurib;

public partial class FluriBuilder
{
    public static readonly string UriSchemeMagnet = "magnet";

    /// <summary>
    /// Create a magnet link with multiple trackers
    /// </summary>
    /// <param name="infoHash"></param>
    /// <param name="trackers"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public FluriBuilder CreateMagnetLink(string infoHash, string? name = null, string[] trackers = null!)
    {
        string magnetLink = $"{UriSchemeMagnet}:?xt=urn:btih:{infoHash}";

        if (!string.IsNullOrEmpty(name))
        {
            magnetLink += $"&dn={Uri.EscapeDataString(name)}";
        }

        if (trackers != null && trackers.Length > 0)
        {
            foreach (string tracker in trackers)
            {
                magnetLink += $"&tr={Uri.EscapeDataString(tracker)}";
            }
        }

        IrregularUri = new Uri(magnetLink);
        return this;
    }
}
