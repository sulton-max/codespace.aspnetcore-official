﻿using Domain.Models.Entity;

namespace Application.DAL.Repositories
{
    public interface IBookStoreRepository : IRepositoryBase<Book>
    {
        IRepositoryBase<Book> BookRepository { get; }
    }
}
