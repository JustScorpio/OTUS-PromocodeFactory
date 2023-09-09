using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Otus.Teaching.PromoCodeFactory.Core.Domain;

namespace Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IRepository<T>
        where T: BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync(); // получение всех объектов
        Task<T> GetByIdAsync(Guid id); // получение одного объекта по id
        Task CreateAsync(T item); // создание объекта
        Task UpdateAsync(T item); // обновление объекта
        Task DeleteAsync(Guid id); // удаление объекта по id
    }
}