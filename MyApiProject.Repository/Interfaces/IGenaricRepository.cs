using MyApiProject.Core.Entities;
using MyApiProject.Core.Specifications;

namespace MyApiProject.Repository.Interfaces;

public interface IGenaricRepository<T> where T : BaseEntity
{
    
     Task<T?> GetByIdAsync(int id);
     Task<T?> GetByIdWithSpecAsync(ISpecification<T> spec);

    Task<IReadOnlyList<T>> GetAllAsync();
    Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec);

    Task<int> GetCountAsync(ISpecification<T> spec);
}
