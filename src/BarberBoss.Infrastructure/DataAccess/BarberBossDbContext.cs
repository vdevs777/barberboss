﻿using BarberBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BarberBoss.Infrastructure.DataAccess;
internal class BarberBossDbContext: DbContext
{
    public BarberBossDbContext(DbContextOptions options): base(options) { }

    public DbSet<Revenue> Revenues { get; set; }
}

