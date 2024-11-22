using FinancialServices.Domain.Entities;
using System;


namespace FinancialServices.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
    }
}