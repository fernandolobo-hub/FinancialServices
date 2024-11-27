using PublicBonds.Domain.Entities;
using System;


namespace PublicBonds.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
    }
}