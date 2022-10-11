// Decompiled with JetBrains decompiler
// Type: System.Data.Linq.DataLoadOptions
// Assembly: System.Data.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 43A00CAE-A2D4-43EB-9464-93379DA02EC9
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Data.Linq.dll

using System.Collections;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Data.Linq
{
  public sealed class DataLoadOptions
  {
    private bool frozen;
    private Dictionary<MetaPosition, MemberInfo> includes = new Dictionary<MetaPosition, MemberInfo>();
    private Dictionary<MetaPosition, LambdaExpression> subqueries = new Dictionary<MetaPosition, LambdaExpression>();

    public void LoadWith<T>(Expression<Func<T, object>> expression)
    {
      if (expression == null)
        throw Error.ArgumentNull(nameof (expression));
      this.Preload(DataLoadOptions.GetLoadWithMemberInfo((LambdaExpression) expression));
    }

    public void LoadWith(LambdaExpression expression)
    {
      if (expression == null)
        throw Error.ArgumentNull(nameof (expression));
      this.Preload(DataLoadOptions.GetLoadWithMemberInfo(expression));
    }

    public void AssociateWith<T>(Expression<Func<T, object>> expression)
    {
      if (expression == null)
        throw Error.ArgumentNull(nameof (expression));
      this.AssociateWithInternal((LambdaExpression) expression);
    }

    public void AssociateWith(LambdaExpression expression)
    {
      if (expression == null)
        throw Error.ArgumentNull(nameof (expression));
      this.AssociateWithInternal(expression);
    }

    private void AssociateWithInternal(LambdaExpression expression)
    {
      Expression body = expression.Body;
      while (body.NodeType == ExpressionType.Convert || body.NodeType == ExpressionType.ConvertChecked)
        body = ((UnaryExpression) body).Operand;
      LambdaExpression lambdaExpression = Expression.Lambda(body, expression.Parameters.ToArray<ParameterExpression>());
      this.Subquery(DataLoadOptions.Searcher.MemberInfoOf(lambdaExpression), lambdaExpression);
    }

    internal bool IsPreloaded(MemberInfo member) => !(member == (MemberInfo) null) ? this.includes.ContainsKey(new MetaPosition(member)) : throw Error.ArgumentNull(nameof (member));

    internal static bool ShapesAreEquivalent(DataLoadOptions ds1, DataLoadOptions ds2)
    {
      if (ds1 != ds2 && (ds1 != null && !ds1.IsEmpty || ds2 != null && !ds2.IsEmpty))
      {
        if (ds1 == null || ds2 == null || ds1.includes.Count != ds2.includes.Count)
          return false;
        foreach (MetaPosition key in ds2.includes.Keys)
        {
          if (!ds1.includes.ContainsKey(key))
            return false;
        }
      }
      return true;
    }

    internal LambdaExpression GetAssociationSubquery(MemberInfo member)
    {
      if (member == (MemberInfo) null)
        throw Error.ArgumentNull(nameof (member));
      LambdaExpression associationSubquery = (LambdaExpression) null;
      this.subqueries.TryGetValue(new MetaPosition(member), out associationSubquery);
      return associationSubquery;
    }

    internal void Freeze() => this.frozen = true;

    internal void Preload(MemberInfo association)
    {
      if (association == (MemberInfo) null)
        throw Error.ArgumentNull(nameof (association));
      if (this.frozen)
        throw Error.IncludeNotAllowedAfterFreeze();
      this.includes.Add(new MetaPosition(association), association);
      this.ValidateTypeGraphAcyclic();
    }

    private void Subquery(MemberInfo association, LambdaExpression subquery)
    {
      if (this.frozen)
        throw Error.SubqueryNotAllowedAfterFreeze();
      subquery = (LambdaExpression) Funcletizer.Funcletize((Expression) subquery);
      DataLoadOptions.ValidateSubqueryMember(association);
      DataLoadOptions.ValidateSubqueryExpression(subquery);
      this.subqueries[new MetaPosition(association)] = subquery;
    }

    private static MemberInfo GetLoadWithMemberInfo(LambdaExpression lambda)
    {
      Expression expression = lambda.Body;
      if (expression != null && (expression.NodeType == ExpressionType.Convert || expression.NodeType == ExpressionType.ConvertChecked))
        expression = ((UnaryExpression) expression).Operand;
      if (expression is MemberExpression memberExpression && memberExpression.Expression.NodeType == ExpressionType.Parameter)
        return memberExpression.Member;
      throw Error.InvalidLoadOptionsLoadMemberSpecification();
    }

    private void ValidateTypeGraphAcyclic()
    {
      IEnumerable<MemberInfo> memberInfos = (IEnumerable<MemberInfo>) this.includes.Values;
      int num = 0;
      for (int index = 0; index < this.includes.Count; ++index)
      {
        HashSet<Type> source = new HashSet<Type>();
        foreach (MemberInfo mi in memberInfos)
          source.Add(DataLoadOptions.GetIncludeTarget(mi));
        List<MemberInfo> memberInfoList = new List<MemberInfo>();
        bool flag = false;
        foreach (MemberInfo memberInfo in memberInfos)
        {
          MemberInfo edge = memberInfo;
          if (source.Where<Type>((Func<Type, bool>) (et => et.IsAssignableFrom(edge.DeclaringType) || edge.DeclaringType.IsAssignableFrom(et))).Any<Type>())
          {
            memberInfoList.Add(edge);
          }
          else
          {
            ++num;
            flag = true;
            if (num == this.includes.Count)
              return;
          }
        }
        if (!flag)
          throw Error.IncludeCycleNotAllowed();
        memberInfos = (IEnumerable<MemberInfo>) memberInfoList;
      }
      throw new InvalidOperationException("Bug in ValidateTypeGraphAcyclic");
    }

    private static Type GetIncludeTarget(MemberInfo mi)
    {
      Type memberType = TypeSystem.GetMemberType(mi);
      return memberType.IsGenericType ? memberType.GetGenericArguments()[0] : memberType;
    }

    private static void ValidateSubqueryMember(MemberInfo mi)
    {
      Type memberType = TypeSystem.GetMemberType(mi);
      if (memberType == (Type) null)
        throw Error.SubqueryNotSupportedOn((object) mi);
      if (!typeof (IEnumerable).IsAssignableFrom(memberType))
        throw Error.SubqueryNotSupportedOnType((object) mi.Name, (object) mi.DeclaringType);
    }

    private static void ValidateSubqueryExpression(LambdaExpression subquery)
    {
      if (!typeof (IEnumerable).IsAssignableFrom(subquery.Body.Type))
        throw Error.SubqueryMustBeSequence();
      new DataLoadOptions.SubqueryValidator().VisitLambda(subquery);
    }

    internal bool IsEmpty => this.includes.Count == 0 && this.subqueries.Count == 0;

    private static class Searcher
    {
      internal static MemberInfo MemberInfoOf(LambdaExpression lambda)
      {
        DataLoadOptions.Searcher.Visitor visitor = new DataLoadOptions.Searcher.Visitor();
        visitor.VisitLambda(lambda);
        return visitor.MemberInfo;
      }

      private class Visitor : System.Data.Linq.SqlClient.ExpressionVisitor
      {
        internal MemberInfo MemberInfo;

        internal override Expression VisitMemberAccess(MemberExpression m)
        {
          this.MemberInfo = m.Member;
          return base.VisitMemberAccess(m);
        }

        internal override Expression VisitMethodCall(MethodCallExpression m)
        {
          this.Visit(m.Object);
          using (IEnumerator<Expression> enumerator = m.Arguments.GetEnumerator())
          {
            if (enumerator.MoveNext())
              this.Visit(enumerator.Current);
          }
          return (Expression) m;
        }
      }
    }

    private class SubqueryValidator : System.Data.Linq.SqlClient.ExpressionVisitor
    {
      private bool isTopLevel = true;

      internal override Expression VisitMethodCall(MethodCallExpression m)
      {
        bool isTopLevel = this.isTopLevel;
        try
        {
          if (this.isTopLevel && !SubqueryRules.IsSupportedTopLevelMethod(m.Method))
            throw Error.SubqueryDoesNotSupportOperator((object) m.Method.Name);
          this.isTopLevel = false;
          return base.VisitMethodCall(m);
        }
        finally
        {
          this.isTopLevel = isTopLevel;
        }
      }
    }
  }
}
