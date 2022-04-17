/*
using QuickyFUR.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using System;


namespace Bakery.Tests.Mock
{
    public static class DatabaseMock
    {
        public static ApplicationDbContext Instance
        {
            get
            {
                var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

                return new ApplicationDbContext(dbContextOptions);
            }
        }
    }
}*/