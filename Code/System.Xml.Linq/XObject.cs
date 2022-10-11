// Decompiled with JetBrains decompiler
// Type: System.Xml.Linq.XObject
// Assembly: System.Xml.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 0BA49A34-043C-4DB0-B03B-2DE95259DDD8
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Xml.Linq.dll

using System.Collections.Generic;

namespace System.Xml.Linq
{
  [__DynamicallyInvokable]
  public abstract class XObject : IXmlLineInfo
  {
    internal XContainer parent;
    internal object annotations;

    internal XObject()
    {
    }

    [__DynamicallyInvokable]
    public string BaseUri
    {
      [__DynamicallyInvokable] get
      {
        XObject xobject = this;
        while (true)
        {
          for (; xobject == null || xobject.annotations != null; xobject = (XObject) xobject.parent)
          {
            if (xobject == null)
              return string.Empty;
            BaseUriAnnotation baseUriAnnotation = xobject.Annotation<BaseUriAnnotation>();
            if (baseUriAnnotation != null)
              return baseUriAnnotation.baseUri;
          }
          xobject = (XObject) xobject.parent;
        }
      }
    }

    [__DynamicallyInvokable]
    public XDocument Document
    {
      [__DynamicallyInvokable] get
      {
        XObject document = this;
        while (document.parent != null)
          document = (XObject) document.parent;
        return document as XDocument;
      }
    }

    [__DynamicallyInvokable]
    public abstract XmlNodeType NodeType { [__DynamicallyInvokable] get; }

    [__DynamicallyInvokable]
    public XElement Parent
    {
      [__DynamicallyInvokable] get => this.parent as XElement;
    }

    [__DynamicallyInvokable]
    public void AddAnnotation(object annotation)
    {
      if (annotation == null)
        throw new ArgumentNullException(nameof (annotation));
      if (this.annotations == null)
      {
        object obj;
        if (!(annotation is object[]))
        {
          obj = annotation;
        }
        else
        {
          obj = (object) new object[1];
          obj[0] = annotation;
        }
        this.annotations = obj;
      }
      else if (!(this.annotations is object[] annotations))
      {
        this.annotations = (object) new object[2]
        {
          this.annotations,
          annotation
        };
      }
      else
      {
        int index = 0;
        while (index < annotations.Length && annotations[index] != null)
          ++index;
        if (index == annotations.Length)
        {
          Array.Resize<object>(ref annotations, index * 2);
          this.annotations = (object) annotations;
        }
        annotations[index] = annotation;
      }
    }

    [__DynamicallyInvokable]
    public object Annotation(Type type)
    {
      if (type == (Type) null)
        throw new ArgumentNullException(nameof (type));
      if (this.annotations != null)
      {
        if (!(this.annotations is object[] annotations))
        {
          if (type.IsInstanceOfType(this.annotations))
            return this.annotations;
        }
        else
        {
          for (int index = 0; index < annotations.Length; ++index)
          {
            object o = annotations[index];
            if (o != null)
            {
              if (type.IsInstanceOfType(o))
                return o;
            }
            else
              break;
          }
        }
      }
      return (object) null;
    }

    [__DynamicallyInvokable]
    public T Annotation<T>() where T : class
    {
      if (this.annotations != null)
      {
        if (!(this.annotations is object[] annotations))
          return this.annotations as T;
        for (int index = 0; index < annotations.Length; ++index)
        {
          object obj1 = annotations[index];
          if (obj1 != null)
          {
            if (obj1 is T obj2)
              return obj2;
          }
          else
            break;
        }
      }
      return default (T);
    }

    [__DynamicallyInvokable]
    public IEnumerable<object> Annotations(Type type) => !(type == (Type) null) ? this.AnnotationsIterator(type) : throw new ArgumentNullException(nameof (type));

    private IEnumerable<object> AnnotationsIterator(Type type)
    {
      if (this.annotations != null)
      {
        if (!(this.annotations is object[] a))
        {
          if (type.IsInstanceOfType(this.annotations))
            yield return this.annotations;
        }
        else
        {
          for (int i = 0; i < a.Length; ++i)
          {
            object o = a[i];
            if (o != null)
            {
              if (type.IsInstanceOfType(o))
                yield return o;
            }
            else
              break;
          }
        }
        a = (object[]) null;
      }
    }

    [__DynamicallyInvokable]
    public IEnumerable<T> Annotations<T>() where T : class
    {
      if (this.annotations != null)
      {
        if (!(this.annotations is object[] a))
        {
          if (this.annotations is T annotations)
            yield return annotations;
        }
        else
        {
          for (int i = 0; i < a.Length; ++i)
          {
            object obj1 = a[i];
            if (obj1 != null)
            {
              if (obj1 is T obj2)
                yield return obj2;
            }
            else
              break;
          }
        }
        a = (object[]) null;
      }
    }

    [__DynamicallyInvokable]
    public void RemoveAnnotations(Type type)
    {
      if (type == (Type) null)
        throw new ArgumentNullException(nameof (type));
      if (this.annotations == null)
        return;
      if (!(this.annotations is object[] annotations))
      {
        if (!type.IsInstanceOfType(this.annotations))
          return;
        this.annotations = (object) null;
      }
      else
      {
        int index = 0;
        int num = 0;
        for (; index < annotations.Length; ++index)
        {
          object o = annotations[index];
          if (o != null)
          {
            if (!type.IsInstanceOfType(o))
              annotations[num++] = o;
          }
          else
            break;
        }
        if (num == 0)
        {
          this.annotations = (object) null;
        }
        else
        {
          while (num < index)
            annotations[num++] = (object) null;
        }
      }
    }

    [__DynamicallyInvokable]
    public void RemoveAnnotations<T>() where T : class
    {
      if (this.annotations == null)
        return;
      if (!(this.annotations is object[] annotations))
      {
        if (!(this.annotations is T))
          return;
        this.annotations = (object) null;
      }
      else
      {
        int index = 0;
        int num = 0;
        for (; index < annotations.Length; ++index)
        {
          object obj = annotations[index];
          if (obj != null)
          {
            if (!(obj is T))
              annotations[num++] = obj;
          }
          else
            break;
        }
        if (num == 0)
        {
          this.annotations = (object) null;
        }
        else
        {
          while (num < index)
            annotations[num++] = (object) null;
        }
      }
    }

    [__DynamicallyInvokable]
    public event EventHandler<XObjectChangeEventArgs> Changed
    {
      [__DynamicallyInvokable] add
      {
        if (value == null)
          return;
        XObjectChangeAnnotation annotation = this.Annotation<XObjectChangeAnnotation>();
        if (annotation == null)
        {
          annotation = new XObjectChangeAnnotation();
          this.AddAnnotation((object) annotation);
        }
        annotation.changed += value;
      }
      [__DynamicallyInvokable] remove
      {
        if (value == null)
          return;
        XObjectChangeAnnotation changeAnnotation = this.Annotation<XObjectChangeAnnotation>();
        if (changeAnnotation == null)
          return;
        changeAnnotation.changed -= value;
        if (changeAnnotation.changing != null || changeAnnotation.changed != null)
          return;
        this.RemoveAnnotations<XObjectChangeAnnotation>();
      }
    }

    [__DynamicallyInvokable]
    public event EventHandler<XObjectChangeEventArgs> Changing
    {
      [__DynamicallyInvokable] add
      {
        if (value == null)
          return;
        XObjectChangeAnnotation annotation = this.Annotation<XObjectChangeAnnotation>();
        if (annotation == null)
        {
          annotation = new XObjectChangeAnnotation();
          this.AddAnnotation((object) annotation);
        }
        annotation.changing += value;
      }
      [__DynamicallyInvokable] remove
      {
        if (value == null)
          return;
        XObjectChangeAnnotation changeAnnotation = this.Annotation<XObjectChangeAnnotation>();
        if (changeAnnotation == null)
          return;
        changeAnnotation.changing -= value;
        if (changeAnnotation.changing != null || changeAnnotation.changed != null)
          return;
        this.RemoveAnnotations<XObjectChangeAnnotation>();
      }
    }

    [__DynamicallyInvokable]
    bool IXmlLineInfo.HasLineInfo() => this.Annotation<LineInfoAnnotation>() != null;

    [__DynamicallyInvokable]
    int IXmlLineInfo.LineNumber
    {
      [__DynamicallyInvokable] get
      {
        LineInfoAnnotation lineInfoAnnotation = this.Annotation<LineInfoAnnotation>();
        return lineInfoAnnotation != null ? lineInfoAnnotation.lineNumber : 0;
      }
    }

    [__DynamicallyInvokable]
    int IXmlLineInfo.LinePosition
    {
      [__DynamicallyInvokable] get
      {
        LineInfoAnnotation lineInfoAnnotation = this.Annotation<LineInfoAnnotation>();
        return lineInfoAnnotation != null ? lineInfoAnnotation.linePosition : 0;
      }
    }

    internal bool HasBaseUri => this.Annotation<BaseUriAnnotation>() != null;

    internal bool NotifyChanged(object sender, XObjectChangeEventArgs e)
    {
      bool flag = false;
      XObject xobject = this;
      while (true)
      {
        for (; xobject == null || xobject.annotations != null; xobject = (XObject) xobject.parent)
        {
          if (xobject == null)
            return flag;
          XObjectChangeAnnotation changeAnnotation = xobject.Annotation<XObjectChangeAnnotation>();
          if (changeAnnotation != null)
          {
            flag = true;
            if (changeAnnotation.changed != null)
              changeAnnotation.changed(sender, e);
          }
        }
        xobject = (XObject) xobject.parent;
      }
    }

    internal bool NotifyChanging(object sender, XObjectChangeEventArgs e)
    {
      bool flag = false;
      XObject xobject = this;
      while (true)
      {
        for (; xobject == null || xobject.annotations != null; xobject = (XObject) xobject.parent)
        {
          if (xobject == null)
            return flag;
          XObjectChangeAnnotation changeAnnotation = xobject.Annotation<XObjectChangeAnnotation>();
          if (changeAnnotation != null)
          {
            flag = true;
            if (changeAnnotation.changing != null)
              changeAnnotation.changing(sender, e);
          }
        }
        xobject = (XObject) xobject.parent;
      }
    }

    internal void SetBaseUri(string baseUri) => this.AddAnnotation((object) new BaseUriAnnotation(baseUri));

    internal void SetLineInfo(int lineNumber, int linePosition) => this.AddAnnotation((object) new LineInfoAnnotation(lineNumber, linePosition));

    internal bool SkipNotify()
    {
      XObject xobject = this;
      while (true)
      {
        for (; xobject == null || xobject.annotations != null; xobject = (XObject) xobject.parent)
        {
          if (xobject == null)
            return true;
          if (xobject.Annotations<XObjectChangeAnnotation>() != null)
            return false;
        }
        xobject = (XObject) xobject.parent;
      }
    }

    internal SaveOptions GetSaveOptionsFromAnnotations()
    {
      XObject xobject = this;
      while (true)
      {
        for (; xobject == null || xobject.annotations != null; xobject = (XObject) xobject.parent)
        {
          if (xobject == null)
            return SaveOptions.None;
          object optionsFromAnnotations = xobject.Annotation(typeof (SaveOptions));
          if (optionsFromAnnotations != null)
            return (SaveOptions) optionsFromAnnotations;
        }
        xobject = (XObject) xobject.parent;
      }
    }
  }
}
