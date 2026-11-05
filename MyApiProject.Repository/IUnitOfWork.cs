using MyApiProject.Core.Entities;
using MyApiProject.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApiProject.Repository;
public interface IUnitOfWork : IAsyncDisposable
{
    IGenaricRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
    Task<int> CompleteAsync(); 
}

