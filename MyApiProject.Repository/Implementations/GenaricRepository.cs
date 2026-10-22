using Microsoft.EntityFrameworkCore;
using MyApiProject.Core.Entities;
using MyApiProject.Core.Specifications;
using MyApiProject.Repository.Interfaces;
using MyApiProject.Repository.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApiProject.Repository.Implementations;
public class GenaricRepository<T> : IGenaricRepository<T> where T : BaseEntity
{
    private readonly AppDbContext _context;

    public GenaricRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecification<T> spec)
    {
        return await SpecificationsEvaluator<T>.GetQuery(_context.Set<T>(),spec).ToListAsync(); 
    }



    public async Task<T?> GetByIdWithSpecAsync(ISpecification<T> spec)
    {
        return await SpecificationsEvaluator<T>.GetQuery(_context.Set<T>(),spec).FirstOrDefaultAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(x=>x.Id == id);
    }


}
