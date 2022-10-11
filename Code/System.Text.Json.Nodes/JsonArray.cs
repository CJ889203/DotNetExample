// Decompiled with JetBrains decompiler
// Type: System.Text.Json.Nodes.JsonArray
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
  /// <summary>Represents a mutable JSON array.</summary>
  [DebuggerDisplay("JsonArray[{List.Count}]")]
  [DebuggerTypeProxy(typeof (JsonArray.DebugView))]
  public sealed class JsonArray : 
    JsonNode,
    IList<JsonNode?>,
    ICollection<JsonNode?>,
    IEnumerable<JsonNode?>,
    IEnumerable
  {

    #nullable disable
    private JsonElement? _jsonElement;
    private System.Collections.Generic.List<JsonNode> _list;


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonArray" /> class that is empty.</summary>
    /// <param name="options">Options to control the behavior.</param>
    public JsonArray(JsonNodeOptions? options = null)
      : base(options)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonArray" /> class that contains items from the specified params array.</summary>
    /// <param name="options">Options to control the behavior.</param>
    /// <param name="items">The items to add to the new <see cref="T:System.Text.Json.Nodes.JsonArray" />.</param>
    public JsonArray(JsonNodeOptions options, params JsonNode?[] items)
      : base(new JsonNodeOptions?(options))
    {
      this.InitializeFromArray(items);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonArray" /> class that contains items from the specified array.</summary>
    /// <param name="items">The items to add to the new <see cref="T:System.Text.Json.Nodes.JsonArray" />.</param>
    public JsonArray(params JsonNode?[] items)
      : base()
    {
      this.InitializeFromArray(items);
    }


    #nullable disable
    private void InitializeFromArray(JsonNode[] items)
    {
      System.Collections.Generic.List<JsonNode> jsonNodeList = new System.Collections.Generic.List<JsonNode>((IEnumerable<JsonNode>) items);
      for (int index = 0; index < items.Length; ++index)
        items[index]?.AssignParent((JsonNode) this);
      this._list = jsonNodeList;
    }


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonArray" /> class that contains items from the specified <see cref="T:System.Text.Json.JsonElement" />.</summary>
    /// <param name="element">The <see cref="T:System.Text.Json.JsonElement" />.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="element" /> is not a <see cref="F:System.Text.Json.JsonValueKind.Array" />.</exception>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonArray" /> class that contains items from the specified <see cref="T:System.Text.Json.JsonElement" />.</returns>
    public static JsonArray? Create(JsonElement element, JsonNodeOptions? options = null)
    {
      if (element.ValueKind == JsonValueKind.Null)
        return (JsonArray) null;
      return element.ValueKind == JsonValueKind.Array ? new JsonArray(element, options) : throw new InvalidOperationException(SR.Format(SR.NodeElementWrongType, (object) "Array"));
    }


    #nullable disable
    internal JsonArray(JsonElement element, JsonNodeOptions? options = null)
      : base(options)
    {
      this._jsonElement = new JsonElement?(element);
    }


    #nullable enable
    /// <summary>Adds an object to the end of the <see cref="T:System.Text.Json.Nodes.JsonArray" />.</summary>
    /// <param name="value">The object to be added to the end of the <see cref="T:System.Text.Json.Nodes.JsonArray" />.</param>
    /// <typeparam name="T">The type of object to be added.</typeparam>
    [RequiresUnreferencedCode("Creating JsonValue instances with non-primitive types is not compatible with trimming. It can result in non-primitive types being serialized, which may have their members trimmed.")]
    public void Add<T>(T? value)
    {
      if ((object) value == null)
      {
        this.Add((JsonNode) null);
      }
      else
      {
        if (!(value is JsonNode jsonNode))
          jsonNode = (JsonNode) new JsonValueNotTrimmable<T>(value);
        this.Add(jsonNode);
      }
    }

    internal System.Collections.Generic.List<JsonNode?> List
    {
      get
      {
        this.CreateNodes();
        return this._list;
      }
    }


    #nullable disable
    internal JsonNode GetItem(int index) => this.List[index];

    internal void SetItem(int index, JsonNode value)
    {
      value?.AssignParent((JsonNode) this);
      this.DetachParent(this.List[index]);
      this.List[index] = value;
    }

    internal override void GetPath(System.Collections.Generic.List<string> path, JsonNode child)
    {
      if (child != null)
      {
        int num = this.List.IndexOf(child);
        System.Collections.Generic.List<string> stringList = path;
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 1);
        interpolatedStringHandler.AppendLiteral("[");
        interpolatedStringHandler.AppendFormatted<int>(num);
        interpolatedStringHandler.AppendLiteral("]");
        string stringAndClear = interpolatedStringHandler.ToStringAndClear();
        stringList.Add(stringAndClear);
      }
      this.Parent?.GetPath(path, (JsonNode) this);
    }


    #nullable enable
    /// <summary>Writes the <see cref="T:System.Text.Json.Nodes.JsonNode" /> into the provided <see cref="T:System.Text.Json.Utf8JsonWriter" /> as JSON.</summary>
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
        this.CreateNodes();
        if (options == null)
          options = JsonSerializerOptions.s_defaultOptions;
        writer.WriteStartArray();
        for (int index = 0; index < this._list.Count; ++index)
          JsonNodeConverter.Instance.Write(writer, this._list[index], options);
        writer.WriteEndArray();
      }
    }

    private void CreateNodes()
    {
      if (this._list != null)
        return;
      System.Collections.Generic.List<JsonNode> jsonNodeList;
      if (!this._jsonElement.HasValue)
      {
        jsonNodeList = new System.Collections.Generic.List<JsonNode>();
      }
      else
      {
        JsonElement jsonElement = this._jsonElement.Value;
        jsonNodeList = new System.Collections.Generic.List<JsonNode>(jsonElement.GetArrayLength());
        foreach (JsonElement enumerate in jsonElement.EnumerateArray())
        {
          JsonNode jsonNode = JsonNodeConverter.Create(enumerate, this.Options);
          jsonNode?.AssignParent((JsonNode) this);
          jsonNodeList.Add(jsonNode);
        }
        this._jsonElement = new JsonElement?();
      }
      this._list = jsonNodeList;
    }

    /// <summary>Gets the number of elements contained in the <see cref="T:System.Text.Json.Nodes.JsonArray" />.</summary>
    public int Count => this.List.Count;

    /// <summary>Adds a <see cref="T:System.Text.Json.Nodes.JsonNode" /> to the end of the <see cref="T:System.Text.Json.Nodes.JsonArray" />.</summary>
    /// <param name="item">The <see cref="T:System.Text.Json.Nodes.JsonNode" /> to be added to the end of the <see cref="T:System.Text.Json.Nodes.JsonArray" />.</param>
    public void Add(JsonNode? item)
    {
      item?.AssignParent((JsonNode) this);
      this.List.Add(item);
    }

    /// <summary>Removes all elements from the <see cref="T:System.Text.Json.Nodes.JsonArray" />.</summary>
    public void Clear()
    {
      for (int index = 0; index < this.List.Count; ++index)
        this.DetachParent(this.List[index]);
      this.List.Clear();
    }

    /// <summary>Determines whether an element is in the <see cref="T:System.Text.Json.Nodes.JsonArray" />.</summary>
    /// <param name="item">The object to locate in the <see cref="T:System.Text.Json.Nodes.JsonArray" />.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="item" /> is found in the <see cref="T:System.Text.Json.Nodes.JsonArray" />; otherwise, <see langword="false" />.</returns>
    public bool Contains(JsonNode? item) => this.List.Contains(item);

    /// <summary>The object to locate in the <see cref="T:System.Text.Json.Nodes.JsonArray" />.</summary>
    /// <param name="item">The <see cref="T:System.Text.Json.Nodes.JsonNode" /> to locate in the <see cref="T:System.Text.Json.Nodes.JsonArray" />.</param>
    /// <returns>The index of item if found in the list; otherwise, -1.</returns>
    public int IndexOf(JsonNode? item) => this.List.IndexOf(item);

    /// <summary>Inserts an element into the <see cref="T:System.Text.Json.Nodes.JsonArray" /> at the specified index.</summary>
    /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
    /// <param name="item">The <see cref="T:System.Text.Json.Nodes.JsonNode" /> to insert.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is less than 0 or <paramref name="index" /> is greater than <see cref="P:System.Text.Json.Nodes.JsonArray.Count" />.</exception>
    public void Insert(int index, JsonNode? item)
    {
      item?.AssignParent((JsonNode) this);
      this.List.Insert(index, item);
    }

    /// <summary>Removes the first occurrence of a specific <see cref="T:System.Text.Json.Nodes.JsonNode" /> from the <see cref="T:System.Text.Json.Nodes.JsonArray" />.</summary>
    /// <param name="item">The <see cref="T:System.Text.Json.Nodes.JsonNode" /> to remove from the <see cref="T:System.Text.Json.Nodes.JsonArray" />.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="item" /> is successfully removed; otherwise, <see langword="false" />.</returns>
    public bool Remove(JsonNode? item)
    {
      if (!this.List.Remove(item))
        return false;
      this.DetachParent(item);
      return true;
    }

    /// <summary>Removes the element at the specified index of the <see cref="T:System.Text.Json.Nodes.JsonArray" />.</summary>
    /// <param name="index">The zero-based index of the element to remove.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is less than 0 or <paramref name="index" /> is greater than <see cref="P:System.Text.Json.Nodes.JsonArray.Count" />.</exception>
    public void RemoveAt(int index)
    {
      JsonNode jsonNode = this.List[index];
      this.List.RemoveAt(index);
      this.DetachParent(jsonNode);
    }


    #nullable disable
    /// <summary>Copies the entire <see cref="T:System.Array" /> to a compatible one-dimensional array, starting at the specified index of the target array.</summary>
    /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Text.Json.Nodes.JsonArray" />. The Array must have zero-based indexing.</param>
    /// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is less than 0.</exception>
    /// <exception cref="T:System.ArgumentException">The number of elements in the source ICollection is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
    void ICollection<JsonNode>.CopyTo(JsonNode[] array, int index) => this.List.CopyTo(array, index);


    #nullable enable
    /// <summary>Returns an enumerator that iterates through the <see cref="T:System.Text.Json.Nodes.JsonArray" />.</summary>
    /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> for the <see cref="T:System.Text.Json.Nodes.JsonNode" />.</returns>
    public IEnumerator<JsonNode?> GetEnumerator() => (IEnumerator<JsonNode>) this.List.GetEnumerator();


    #nullable disable
    /// <summary>Returns an enumerator that iterates through the <see cref="T:System.Text.Json.Nodes.JsonArray" />.</summary>
    /// <returns>A <see cref="T:System.Collections.IEnumerator" /> for the <see cref="T:System.Text.Json.Nodes.JsonArray" />.</returns>
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) this.List).GetEnumerator();

    /// <summary>Returns <see langword="false" />.</summary>
    bool ICollection<JsonNode>.IsReadOnly => false;

    private void DetachParent(JsonNode item)
    {
      if (item == null)
        return;
      item.Parent = (JsonNode) null;
    }

    [ExcludeFromCodeCoverage]
    private class DebugView
    {
      [DebuggerBrowsable(DebuggerBrowsableState.Never)]
      private JsonArray _node;

      public DebugView(JsonArray node) => this._node = node;

      public string Json => this._node.ToJsonString();

      public string Path => this._node.GetPath();

      [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
      private JsonArray.DebugView.DebugViewItem[] Items
      {
        get
        {
          JsonArray.DebugView.DebugViewItem[] items = new JsonArray.DebugView.DebugViewItem[this._node.List.Count];
          for (int index = 0; index < this._node.List.Count; ++index)
            items[index].Value = this._node.List[index];
          return items;
        }
      }

      [DebuggerDisplay("{Display,nq}")]
      private struct DebugViewItem
      {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public JsonNode Value;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string Display
        {
          get
          {
            if (this.Value == null)
              return "null";
            if (this.Value is JsonValue)
              return this.Value.ToJsonString();
            if (this.Value is JsonObject jsonObject)
            {
              DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(12, 1);
              interpolatedStringHandler.AppendLiteral("JsonObject[");
              interpolatedStringHandler.AppendFormatted<int>(jsonObject.Count);
              interpolatedStringHandler.AppendLiteral("]");
              return interpolatedStringHandler.ToStringAndClear();
            }
            JsonArray jsonArray = (JsonArray) this.Value;
            DefaultInterpolatedStringHandler interpolatedStringHandler1 = new DefaultInterpolatedStringHandler(11, 1);
            interpolatedStringHandler1.AppendLiteral("JsonArray[");
            interpolatedStringHandler1.AppendFormatted<int>(jsonArray.List.Count);
            interpolatedStringHandler1.AppendLiteral("]");
            return interpolatedStringHandler1.ToStringAndClear();
          }
        }
      }
    }
  }
}
