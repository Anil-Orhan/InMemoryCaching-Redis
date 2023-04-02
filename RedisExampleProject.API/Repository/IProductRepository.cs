﻿using RedisExampleProject.API.Models;

namespace RedisExampleProject.API.Repository;

public interface IProductRepository
{
    Task<List<Product?>> GetAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<Product?> CreateAsync(Product? product);
}