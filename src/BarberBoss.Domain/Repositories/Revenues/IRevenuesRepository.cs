using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories.Revenues;
public interface IRevenuesRepository
{
    Task Add(Revenue revenue);
    Task<List<Revenue>> GetAll();
    Task<Revenue?> GetById(long id);
    void Update(Revenue revenue);
    Task<bool> Delete(long id);
    Task<List<Revenue>> FilterByMonth(DateOnly date);
}
