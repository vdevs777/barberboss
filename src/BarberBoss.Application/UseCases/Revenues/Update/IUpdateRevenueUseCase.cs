using BarberBoss.Communication.Requests;

namespace BarberBoss.Application.UseCases.Revenues.Update;
public interface IUpdateRevenueUseCase
{
    Task Execute(long id, RequestRevenueJson request);
}
