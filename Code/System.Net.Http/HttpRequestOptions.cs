// Decompiled with JetBrains decompiler
// Type: System.Net.Http.HttpRequestOptions
// Assembly: System.Net.Http, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: D3111F3C-D07D-4FFA-B4EC-BDA891CAE931
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Net.Http.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Net.Http.xml

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;


#nullable enable
namespace System.Net.Http
{
  /// <summary>Represents a collection of options for an HTTP request.</summary>
  public sealed class HttpRequestOptions : 
    IDictionary<string, object?>,
    ICollection<KeyValuePair<string, object?>>,
    IEnumerable<KeyValuePair<string, object?>>,
    IEnumerable
  {
    private Dictionary<string, object?> Options { get; } = new Dictionary<string, object>();

    /// <summary>Gets or sets the element with the specified key.</summary>
    /// <param name="key">The key of the element to get or set.</param>
    /// <returns>The element with the specified key.</returns>
    object? IDictionary<
    #nullable disable
    string, object>.this[string key]
    {
      get => this.Options[key];
      set => this.Options[key] = value;
    }


    #nullable enable
    /// <summary>Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2" />.</summary>
    /// <returns>An <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" />.</returns>
    ICollection<string> IDictionary<
    #nullable disable
    string, object>.Keys => (ICollection<string>) this.Options.Keys;


    #nullable enable
    /// <summary>Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2" />.</summary>
    /// <returns>An <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" />.</returns>
    ICollection<object?> IDictionary<
    #nullable disable
    string, object>.Values => (ICollection<object>) this.Options.Values;

    /// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.</summary>
    /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
    int ICollection<KeyValuePair<string, object>>.Count => this.Options.Count;

    /// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.</summary>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, <see langword="false" />.</returns>
    bool ICollection<KeyValuePair<string, object>>.IsReadOnly => ((ICollection<KeyValuePair<string, object>>) this.Options).IsReadOnly;

    /// <summary>Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2" />.</summary>
    /// <param name="key">The object to use as the key of the element to add.</param>
    /// <param name="value">The object to use as the value of the element to add.</param>
    void IDictionary<string, object>.Add(string key, object value) => this.Options.Add(key, value);

    /// <summary>Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.</summary>
    /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
    void ICollection<KeyValuePair<string, object>>.Add(
      KeyValuePair<string, object> item)
    {
      ((ICollection<KeyValuePair<string, object>>) this.Options).Add(item);
    }

    /// <summary>Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.</summary>
    void ICollection<KeyValuePair<string, object>>.Clear() => this.Options.Clear();

    /// <summary>Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.</summary>
    /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />.</returns>
    bool ICollection<KeyValuePair<string, object>>.Contains(
      KeyValuePair<string, object> item)
    {
      return ((ICollection<KeyValuePair<string, object>>) this.Options).Contains(item);
    }

    /// <summary>Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key.</summary>
    /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2" />.</param>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the key; otherwise, <see langword="false" />.</returns>
    bool IDictionary<string, object>.ContainsKey(string key) => this.Options.ContainsKey(key);

    /// <summary>Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.</summary>
    /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
    /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
    void ICollection<KeyValuePair<string, object>>.CopyTo(
      KeyValuePair<string, object>[] array,
      int arrayIndex)
    {
      ((ICollection<KeyValuePair<string, object>>) this.Options).CopyTo(array, arrayIndex);
    }

    /// <summary>Returns an enumerator that iterates through the collection.</summary>
    /// <returns>An enumerator that can be used to iterate through the collection.</returns>
    IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator() => (IEnumerator<KeyValuePair<string, object>>) this.Options.GetEnumerator();

    /// <summary>Returns an enumerator that iterates through a collection.</summary>
    /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) this.Options).GetEnumerator();

    /// <summary>Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2" />.</summary>
    /// <param name="key">The key of the element to remove.</param>
    /// <returns>
    /// <see langword="true" /> if the element is successfully removed; otherwise, <see langword="false" />. This method also returns <see langword="false" /> if <paramref name="key" /> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2" />.</returns>
    bool IDictionary<string, object>.Remove(string key) => this.Options.Remove(key);

    /// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.</summary>
    /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />. This method also returns <see langword="false" /> if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
    bool ICollection<KeyValuePair<string, object>>.Remove(
      KeyValuePair<string, object> item)
    {
      return ((ICollection<KeyValuePair<string, object>>) this.Options).Remove(item);
    }

    /// <summary>Gets the value associated with the specified key.</summary>
    /// <param name="key">The key of the value to get.</param>
    /// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value" /> parameter. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Collections.Generic.Dictionary`2" /> contains an element with the specified key; otherwise, <see langword="false" />.</returns>
    bool IDictionary<string, object>.TryGetValue(string key, out object value) => this.Options.TryGetValue(key, out value);


    #nullable enable
    /// <summary>Gets the value of a specified HTTP request option.</summary>
    /// <param name="key">The strongly typed key to get the value of an HTTP request option.</param>
    /// <param name="value">When this method returns, contains the value of the specified HTTP request option.</param>
    /// <typeparam name="TValue">The type of the HTTP value as defined by <paramref name="key" />.</typeparam>
    /// <returns>
    /// <see langword="true" /> if the collection contains an element with the specified key; otherwise, <see langword="false" />.</returns>
    public bool TryGetValue<TValue>(HttpRequestOptionsKey<TValue> key, [MaybeNullWhen(false)] out TValue value)
    {
      object obj1;
      if (this.Options.TryGetValue(key.Key, out obj1) && obj1 is TValue obj2)
      {
        value = obj2;
        return true;
      }
      value = default (TValue);
      return false;
    }

    /// <summary>Sets the value of a specified HTTP request option.</summary>
    /// <param name="key">The strongly typed key for the HTTP request option.</param>
    /// <param name="value">The value of the HTTP request option.</param>
    /// <typeparam name="TValue">The type of the HTTP value as defined by <paramref name="key" />.</typeparam>
    public void Set<TValue>(HttpRequestOptionsKey<TValue> key, TValue value) => this.Options[key.Key] = (object) value;
  }
}
