// Decompiled with JetBrains decompiler
// Type: System.Text.Json.Nodes.JsonObject
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization.Converters;


#nullable enable
namespace System.Text.Json.Nodes
{
  /// <summary>Represents a mutable JSON object.</summary>
  [DebuggerDisplay("JsonObject[{Count}]")]
  [DebuggerTypeProxy(typeof (JsonObject.DebugView))]
  public sealed class JsonObject : 
    JsonNode,
    IDictionary<string, JsonNode?>,
    ICollection<KeyValuePair<string, JsonNode?>>,
    IEnumerable<KeyValuePair<string, JsonNode?>>,
    IEnumerable
  {

    #nullable disable
    private JsonElement? _jsonElement;
    private JsonPropertyDictionary<JsonNode> _dictionary;


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonObject" /> class that is empty.</summary>
    /// <param name="options">Options to control the behavior.</param>
    public JsonObject(JsonNodeOptions? options = null)
      : base(options)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonObject" /> class that contains the specified <paramref name="properties" />.</summary>
    /// <param name="properties">The properties to be added.</param>
    /// <param name="options">Options to control the behavior.</param>
    public JsonObject(
      IEnumerable<KeyValuePair<string, JsonNode?>> properties,
      JsonNodeOptions? options = null)
      : base()
    {
      foreach (KeyValuePair<string, JsonNode> property in properties)
        this.Add(property.Key, property.Value);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonObject" /> class that contains properties from the specified <see cref="T:System.Text.Json.JsonElement" />.</summary>
    /// <param name="element">The <see cref="T:System.Text.Json.JsonElement" />.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonObject" /> class that contains properties from the specified <see cref="T:System.Text.Json.JsonElement" />.</returns>
    public static JsonObject? Create(JsonElement element, JsonNodeOptions? options = null)
    {
      if (element.ValueKind == JsonValueKind.Null)
        return (JsonObject) null;
      return element.ValueKind == JsonValueKind.Object ? new JsonObject(element, options) : throw new InvalidOperationException(SR.Format(SR.NodeElementWrongType, (object) "Object"));
    }


    #nullable disable
    internal JsonObject(JsonElement element, JsonNodeOptions? options = null)
      : base(options)
    {
      this._jsonElement = new JsonElement?(element);
    }


    #nullable enable
    /// <summary>Returns the value of a property with the specified name.</summary>
    /// <param name="propertyName">The name of the property to return.</param>
    /// <param name="jsonNode">The JSON value of the property with the specified name.</param>
    /// <returns>
    /// <see langword="true" /> if a property with the specified name was found; otherwise, <see langword="false" />.</returns>
    public bool TryGetPropertyValue(string propertyName, out JsonNode? jsonNode) => ((IDictionary<string, JsonNode>) this).TryGetValue(propertyName, out jsonNode);

    /// <summary>Write the <see cref="T:System.Text.Json.Nodes.JsonNode" /> into the provided <see cref="T:System.Text.Json.Utf8JsonWriter" /> as JSON.</summary>
    /// <param name="writer">The <see cref="T:System.Text.Json.Utf8JsonWriter" />.</param>
    /// <param name="options">Options to control the serialization behavior.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="writer" /> parameter is <see langword="null" />.</exception>
    public override void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions? options = null)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      if (this._jsonElement.HasValue)
      {
        this._jsonElement.Value.WriteTo(writer);
      }
      else
      {
        if (options == null)
          options = JsonSerializerOptions.s_defaultOptions;
        writer.WriteStartObject();
        foreach (KeyValuePair<string, JsonNode> keyValuePair in this)
        {
          writer.WritePropertyName(keyValuePair.Key);
          JsonNodeConverter.Instance.Write(writer, keyValuePair.Value, options);
        }
        writer.WriteEndObject();
      }
    }


    #nullable disable
    internal JsonNode GetItem(string propertyName)
    {
      JsonNode jsonNode;
      return this.TryGetPropertyValue(propertyName, out jsonNode) ? jsonNode : (JsonNode) null;
    }

    internal override void GetPath(List<string> path, JsonNode child)
    {
      if (child != null)
      {
        this.InitializeIfRequired();
        string key = this._dictionary.FindValue(child).Value.Key;
        if (key.IndexOfAny(ReadStack.SpecialCharacters) != -1)
          path.Add("['" + key + "']");
        else
          path.Add("." + key);
      }
      if (this.Parent == null)
        return;
      this.Parent.GetPath(path, (JsonNode) this);
    }

    internal void SetItem(string propertyName, JsonNode value)
    {
      this.InitializeIfRequired();
      this.DetachParent(this._dictionary.SetValue(propertyName, value, (Action) (() => value?.AssignParent((JsonNode) this))));
    }

    private void DetachParent(JsonNode item)
    {
      this.InitializeIfRequired();
      if (item == null)
        return;
      item.Parent = (JsonNode) null;
    }


    #nullable enable
    /// <summary>Adds an element with the provided property name and value to the <see cref="T:System.Text.Json.Nodes.JsonObject" />.</summary>
    /// <param name="propertyName">The property name of the element to add.</param>
    /// <param name="value">The value of the element to add.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="propertyName" />is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">An element with the same property name already exists in the <see cref="T:System.Text.Json.Nodes.JsonObject" />.</exception>
    public void Add(string propertyName, JsonNode? value)
    {
      this.InitializeIfRequired();
      this._dictionary.Add(propertyName, value);
      value?.AssignParent((JsonNode) this);
    }

    /// <summary>Adds the specified property to the <see cref="T:System.Text.Json.Nodes.JsonObject" />.</summary>
    /// <param name="property">The KeyValuePair structure representing the property name and value to add to the <see cref="T:System.Text.Json.Nodes.JsonObject" />.</param>
    /// <exception cref="T:System.ArgumentException">An element with the same property name already exists in the <see cref="T:System.Text.Json.Nodes.JsonObject" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">The property name of <paramref name="property" /> is <see langword="null" />.</exception>
    public void Add(KeyValuePair<string, JsonNode?> property) => this.Add(property.Key, property.Value);

    /// <summary>Removes all elements from the <see cref="T:System.Text.Json.Nodes.JsonObject" />.</summary>
    public void Clear()
    {
      if (this._jsonElement.HasValue)
      {
        this._jsonElement = new JsonElement?();
      }
      else
      {
        if (this._dictionary == null)
          return;
        foreach (JsonNode jsonNode in (IEnumerable<JsonNode>) this._dictionary.GetValueCollection())
          this.DetachParent(jsonNode);
        this._dictionary.Clear();
      }
    }

    /// <summary>Determines whether the <see cref="T:System.Text.Json.Nodes.JsonObject" /> contains an element with the specified property name.</summary>
    /// <param name="propertyName">The property name to locate in the <see cref="T:System.Text.Json.Nodes.JsonObject" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="propertyName" /> is <see langword="null" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Text.Json.Nodes.JsonObject" /> contains an element with the specified property name; otherwise, <see langword="false" />.</returns>
    public bool ContainsKey(string propertyName)
    {
      this.InitializeIfRequired();
      return this._dictionary.ContainsKey(propertyName);
    }

    /// <summary>Gets the number of elements contained in <see cref="T:System.Text.Json.Nodes.JsonObject" />.</summary>
    public int Count
    {
      get
      {
        this.InitializeIfRequired();
        return this._dictionary.Count;
      }
    }

    /// <summary>Removes the element with the specified property name from the <see cref="T:System.Text.Json.Nodes.JsonObject" />.</summary>
    /// <param name="propertyName">The property name of the element to remove.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="propertyName" /> is <see langword="null" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the element is successfully removed; otherwise, <see langword="false" />.</returns>
    public bool Remove(string propertyName)
    {
      if (propertyName == null)
        throw new ArgumentNullException(nameof (propertyName));
      this.InitializeIfRequired();
      JsonNode existing;
      bool flag = this._dictionary.TryRemoveProperty(propertyName, out existing);
      if (flag)
        this.DetachParent(existing);
      return flag;
    }


    #nullable disable
    /// <summary>Determines whether the <see cref="T:System.Text.Json.Nodes.JsonObject" /> contains a specific property name and <see cref="T:System.Text.Json.Nodes.JsonNode" /> reference.</summary>
    /// <param name="item">The element to locate in the <see cref="T:System.Text.Json.Nodes.JsonObject" />.</param>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Text.Json.Nodes.JsonObject" /> contains an element with the property name; otherwise, <see langword="false" />.</returns>
    bool ICollection<KeyValuePair<string, JsonNode>>.Contains(
      KeyValuePair<string, JsonNode> item)
    {
      this.InitializeIfRequired();
      return this._dictionary.Contains(item);
    }

    /// <summary>Copies the elements of the <see cref="T:System.Text.Json.Nodes.JsonObject" /> to an array of type KeyValuePair starting at the specified array index.</summary>
    /// <param name="array">The one-dimensional Array that is the destination of the elements copied from <see cref="T:System.Text.Json.Nodes.JsonObject" />.</param>
    /// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is less than 0.</exception>
    /// <exception cref="T:System.ArgumentException">The number of elements in the source ICollection is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
    void ICollection<KeyValuePair<string, JsonNode>>.CopyTo(
      KeyValuePair<string, JsonNode>[] array,
      int index)
    {
      this.InitializeIfRequired();
      this._dictionary.CopyTo(array, index);
    }


    #nullable enable
    /// <summary>Returns an enumerator that iterates through the <see cref="T:System.Text.Json.Nodes.JsonObject" />.</summary>
    /// <returns>An enumerator that iterates through the <see cref="T:System.Text.Json.Nodes.JsonObject" />.</returns>
    public IEnumerator<KeyValuePair<string, JsonNode?>> GetEnumerator()
    {
      this.InitializeIfRequired();
      return this._dictionary.GetEnumerator();
    }


    #nullable disable
    /// <summary>Removes a key and value from the <see cref="T:System.Text.Json.Nodes.JsonObject" />.</summary>
    /// <param name="item">The KeyValuePair structure representing the property name and value to remove from the <see cref="T:System.Text.Json.Nodes.JsonObject" />.</param>
    /// <returns>
    /// <see langword="true" /> if the element is successfully removed; otherwise, <see langword="false" />.</returns>
    bool ICollection<KeyValuePair<string, JsonNode>>.Remove(
      KeyValuePair<string, JsonNode> item)
    {
      return this.Remove(item.Key);
    }


    #nullable enable
    /// <summary>Gets a collection containing the property names in the <see cref="T:System.Text.Json.Nodes.JsonObject" />.</summary>
    ICollection<string> IDictionary<
    #nullable disable
    string, JsonNode>.Keys
    {
      get
      {
        this.InitializeIfRequired();
        return this._dictionary.Keys;
      }
    }


    #nullable enable
    /// <summary>Gets a collection containing the property values in the <see cref="T:System.Text.Json.Nodes.JsonObject" />.</summary>
    ICollection<JsonNode?> IDictionary<
    #nullable disable
    string, JsonNode>.Values
    {
      get
      {
        this.InitializeIfRequired();
        return this._dictionary.Values;
      }
    }

    /// <summary>Gets the value associated with the specified property name.</summary>
    /// <param name="propertyName">The property name of the value to get.</param>
    /// <param name="jsonNode">When this method returns, contains the value associated with the specified property name, if the property name is found; otherwise, <see langword="null" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="propertyName" /> is <see langword="null" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Text.Json.Nodes.JsonObject" /> contains an element with the specified property name; otherwise, <see langword="false" />.</returns>
    bool IDictionary<string, JsonNode>.TryGetValue(
      string propertyName,
      out JsonNode jsonNode)
    {
      this.InitializeIfRequired();
      return this._dictionary.TryGetValue(propertyName, out jsonNode);
    }

    /// <summary>Returns <see langword="false" />.</summary>
    bool ICollection<KeyValuePair<string, JsonNode>>.IsReadOnly => false;

    /// <summary>Returns an enumerator that iterates through the <see cref="T:System.Text.Json.Nodes.JsonObject" />.</summary>
    /// <returns>An enumerator that iterates through the <see cref="T:System.Text.Json.Nodes.JsonObject" />.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
      this.InitializeIfRequired();
      return (IEnumerator) this._dictionary.GetEnumerator();
    }

    private void InitializeIfRequired()
    {
      if (this._dictionary != null)
        return;
      JsonNodeOptions? options = this.Options;
      int num;
      if (!options.HasValue)
      {
        num = 0;
      }
      else
      {
        options = this.Options;
        num = options.Value.PropertyNameCaseInsensitive ? 1 : 0;
      }
      JsonPropertyDictionary<JsonNode> propertyDictionary = new JsonPropertyDictionary<JsonNode>(num != 0);
      if (this._jsonElement.HasValue)
      {
        foreach (JsonProperty jsonProperty in this._jsonElement.Value.EnumerateObject())
        {
          JsonNode jsonNode = JsonNodeConverter.Create(jsonProperty.Value, this.Options);
          if (jsonNode != null)
            jsonNode.Parent = (JsonNode) this;
          propertyDictionary.Add(new KeyValuePair<string, JsonNode>(jsonProperty.Name, jsonNode));
        }
        this._jsonElement = new JsonElement?();
      }
      this._dictionary = propertyDictionary;
    }

    [ExcludeFromCodeCoverage]
    private class DebugView
    {
      [DebuggerBrowsable(DebuggerBrowsableState.Never)]
      private JsonObject _node;

      public DebugView(JsonObject node) => this._node = node;

      public string Json => this._node.ToJsonString();

      public string Path => this._node.GetPath();

      [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
      private JsonObject.DebugView.DebugViewProperty[] Items
      {
        get
        {
          JsonObject.DebugView.DebugViewProperty[] items = new JsonObject.DebugView.DebugViewProperty[this._node.Count];
          int index = 0;
          foreach (KeyValuePair<string, JsonNode> keyValuePair in this._node)
          {
            items[index].PropertyName = keyValuePair.Key;
            items[index].Value = keyValuePair.Value;
            ++index;
          }
          return items;
        }
      }

      [DebuggerDisplay("{Display,nq}")]
      private struct DebugViewProperty
      {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public JsonNode Value;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string PropertyName;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string Display
        {
          get
          {
            if (this.Value == null)
              return this.PropertyName + " = null";
            if (this.Value is JsonValue)
              return this.PropertyName + " = " + this.Value.ToJsonString();
            if (this.Value is JsonObject jsonObject)
            {
              DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 2);
              interpolatedStringHandler.AppendFormatted(this.PropertyName);
              interpolatedStringHandler.AppendLiteral(" = JsonObject[");
              interpolatedStringHandler.AppendFormatted<int>(jsonObject.Count);
              interpolatedStringHandler.AppendLiteral("]");
              return interpolatedStringHandler.ToStringAndClear();
            }
            JsonArray jsonArray = (JsonArray) this.Value;
            DefaultInterpolatedStringHandler interpolatedStringHandler1 = new DefaultInterpolatedStringHandler(14, 2);
            interpolatedStringHandler1.AppendFormatted(this.PropertyName);
            interpolatedStringHandler1.AppendLiteral(" = JsonArray[");
            interpolatedStringHandler1.AppendFormatted<int>(jsonArray.Count);
            interpolatedStringHandler1.AppendLiteral("]");
            return interpolatedStringHandler1.ToStringAndClear();
          }
        }
      }
    }
  }
}
