using MyApiProject.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyApiProject.Core.Specifications;
public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
{
    public Expression<Func<T, bool>> Criteira { get; set; } = null;
    public List<Expression<Func<T, object>>> Include { get; set; } = new List<Expression<Func<T, object>>>();
    public Expression<Func<T, object>> OrderBy { get; set; } = null;
    public Expression<Func<T, object>> OrderByDesc { get; set; } = null;

    public BaseSpecification()
    {
        //Include = new List<Expression<Func<T, object>>>();

        //Criteira = null;
    }
    public BaseSpecification(Expression<Func<T,bool>> criteiraExpression)
    {
        Criteira = criteiraExpression;
    }
    private protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    private protected void AddOrderByDesc(Expression<Func<T, object>> orderByDescExpression)
    {
        OrderByDesc = orderByDescExpression;
    }


}
