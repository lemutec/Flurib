using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flurib;

public partial class FluriBuilder
{
    /// <summary>
    /// Appends a query string parameter with a key, and many values. Multiple values will be comma seperated. If only 1 value is passed and its null or value, the key will be added to the QS.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="values"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns></returns>
    public FluriBuilder WithParameter(string key, params string[] values)
        => WithParameter(key, valuesEnum: values);

    /// <summary>
    /// Appends query strings from a list of key-value pairs (usually a dictionary).
    /// </summary>
    /// <param name="parameterDictionary"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns></returns>
    public FluriBuilder WithParameter(IEnumerable<KeyValuePair<string, string>> parameterDictionary)
    {
        if (parameterDictionary == null)
        {
            throw new ArgumentNullException(nameof(parameterDictionary));
        }
        foreach (var item in parameterDictionary)
        {
            WithParameter(item.Key, item.Value);
        }
        return this;
    }

    /// <summary>
    /// Appends a query string parameter with a key, and many values. Multiple values will be comma seperated. If only 1 value is passed and its null or value, the key will be added to the QS.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="valuesEnum"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns></returns>
    public FluriBuilder WithParameter(string key, IEnumerable<object> valuesEnum)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }
        if (valuesEnum == null)
        {
            valuesEnum = [];
        }
        var intitialValue = string.IsNullOrWhiteSpace(Query) ? "" : $"{Query.TrimStart('?')}&";
        Query = intitialValue.AppendKeyValue(key, valuesEnum);
        return this;
    }

    /// <summary>
    /// Appends query strings from dictionary
    /// </summary>
    /// <param name="bld"></param>
    /// <param name="parameterDictionary"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns></returns>
    public FluriBuilder WithParameter(IEnumerable<(string, string)> parameterDictionary)
    {
        if (parameterDictionary == null) throw new ArgumentNullException(nameof(parameterDictionary));
        foreach (var item in parameterDictionary)
        {
            WithParameter(item.Item1, item.Item2);
        }
        return this;
    }

    /// <summary>
    /// Appends a fragments parameter with a key, and many values. Multiple values will be comma seperated. If only 1 value is passed and its null or value, the key will be added to the fragment.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="values"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns></returns>
    public FluriBuilder WithFragment(string key, params string[] values)
        => WithFragment(key, valuesEnum: values);

    /// <summary>
    /// Appends fragments from dictionary
    /// </summary>
    /// <param name="fragmentDictionary"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns></returns>
    public FluriBuilder WithFragment(IDictionary<string, string> fragmentDictionary)
    {
        if (fragmentDictionary == null)
        {
            throw new ArgumentNullException(nameof(fragmentDictionary));
        }
        foreach (var item in fragmentDictionary)
        {
            WithFragment(item.Key, item.Value);
        }
        return this;
    }

    /// <summary>
    /// Appends a fragments with a key, and many values. Multiple values will be comma seperated. If only 1 value is passed and its null or value, the key will be added to the fragment.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="valuesEnum"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns></returns>
    public FluriBuilder WithFragment(string key, IEnumerable<object> valuesEnum)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }
        if (valuesEnum == null)
        {
            valuesEnum = [];
        }
        var intitialValue = string.IsNullOrWhiteSpace(Fragment) ? "" : $"{Fragment.TrimStart('#')}&";
        Fragment = intitialValue.AppendKeyValue(key, valuesEnum);
        return this;
    }

    /// <summary>
    /// Sets the port to be the port number
    /// </summary>
    /// <param name="port"></param>
    /// <exception cref="ArgumentOutOfRangeException">Throws if port is less than one</exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns></returns>
    public FluriBuilder WithPort(int port)
    {
        if (port < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(port));
        }
        Port = port;
        return this;
    }

    /// <summary>
    /// Removes the port number for default ports
    /// </summary>
    /// <returns></returns>
    public FluriBuilder WithoutDefaultPort()
    {
        if (Uri.IsDefaultPort)
        {
            Port = -1;
        }
        return this;
    }

    /// <summary>
    /// appends a path segment to the path. Can be called multiple times to append multiple segments
    /// </summary>
    /// <param name="pathSegment"></param>
    /// <exception cref="ArgumentNullException">You pass a string as a path segment</exception>
    /// <returns></returns>
    public FluriBuilder WithPathSegment(string pathSegment)
    {
        if (string.IsNullOrWhiteSpace(pathSegment))
        {
            throw new ArgumentNullException(nameof(pathSegment));
        }
        var path = pathSegment.TrimStart('/');
        Path = $"{Path.TrimEnd('/')}/{path}";
        return this;
    }

    /// <summary>
    /// Sets your Uri Scheme
    /// </summary>
    /// <param name="scheme"></param>
    /// <exception cref="ArgumentNullException">You must pass a scheme</exception>
    /// <returns></returns>
    public FluriBuilder WithScheme(string scheme)
    {
        if (string.IsNullOrWhiteSpace(scheme))
        {
            throw new ArgumentNullException(nameof(scheme));
        }
        Scheme = scheme;
        return this;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <exception cref="ArgumentNullException">You must pass a ho0st</exception>
    /// <returns></returns>
    public FluriBuilder WithHost(string host)
    {
        if (string.IsNullOrWhiteSpace(host))
        {
            throw new ArgumentNullException(nameof(host));
        }
        Host = host;
        return this;
    }

    public string PathAndQuery() => (Path + Query);

    /// <summary>
    /// Use Https?
    /// </summary>
    /// <param name="predicate">default true, if false sets scheme to http</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns></returns>
    public FluriBuilder UseHttps(bool predicate = true)
    {
        Scheme = predicate ? "https" : "http";
        return this;
    }

    /// <summary>
    /// Escape Url query string
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns></returns>
    [Obsolete("SYSLIB0013: 'Uri.EscapeUriString(string)' is obsolete: 'Uri.EscapeUriString can corrupt the Uri string in some cases. Consider using\r\nUri.EscapeDataString for query string components instead.'")]
    public string ToEscapeString() => Uri.EscapeUriString(Uri.ToString());

    /// <summary>
    /// Escape the whole Url string
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns></returns>
    public string ToEscapeDataString() => Uri.EscapeDataString(Uri.ToString());

    /// <summary>
    /// Returns the Uri string
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns></returns>
    public string ToUriString() => (IrregularUri ?? Uri).ToString();
}

file static class StringExtension
{
    /// <summary>
    /// Appends x-www-form-urlencoded key and valuesEnum into initialValue.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public static string AppendKeyValue(this string intitialValue, string key, IEnumerable<object> valuesEnum)
    {
        var encodedKey = Uri.EscapeDataString(key);

        var sb = new StringBuilder($"{intitialValue}{encodedKey}");

        var Values = (
            from x in valuesEnum
            let v = x?.ToString()
            where !string.IsNullOrWhiteSpace(v)
            select Uri.EscapeDataString(v)
            ).ToArray();

        if (Values.Length > 0)
        {
            sb.Append("=");
            sb.Append(string.Join(",", Values));
        }

        return sb.ToString();
    }
}
