namespace BarberBoss.Infrastructure.DataAccess.Repositories;

using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories.Revenues;
using Microsoft.EntityFrameworkCore;

internal class RevenuesRepository: IRevenuesRepository
{
    private readonly BarberBossDbContext _dbContext;
    public RevenuesRepository(BarberBossDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Revenue revenue)
    {
        await _dbContext.Revenues.AddAsync(revenue);
    }

    public async Task<List<Revenue>> GetAll()
    {
        return await _dbContext.Revenues.AsNoTracking().ToListAsync();
    }

    public async Task<Revenue?> GetById(long id)
    {
        return await _dbContext.Revenues.AsNoTracking().FirstOrDefaultAsync(revenue => revenue.Id == id);
    }

    public void Update(Revenue revenue)
    {
        _dbContext.Update(revenue);
    }

    public async Task<bool> Delete(long id)
    {
        var result = await _dbContext.Revenues.FirstOrDefaultAsync(revenue => revenue.Id == id);

        if (result is null)
        {
            return false;
        }

        _dbContext.Revenues.Remove(result);

        return true;
    }

    public async Task<List<Revenue>> FilterByMonth(DateOnly date)
    {
        var startDate = new DateTime(year: date.Year, month: date.Month, day: 1).Date;

        var daysInMonth = DateTime.DaysInMonth(year: date.Year, month: date.Month);
        var endDate = new DateTime(year: date.Year, month: date.Month, day: daysInMonth, hour: 23, minute: 59, second: 59);

        return await _dbContext
            .Revenues
            .AsNoTracking()
            .Where(expense => expense.OccurredAt >= startDate && expense.OccurredAt <= endDate)
            .OrderBy(expense => expense.OccurredAt)
            .ThenBy(expense => expense.Title)
            .ToListAsync();
    }
}

