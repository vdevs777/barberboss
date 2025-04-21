using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Entities;

namespace BarberBoss.Application.UseCases.Revenues.GetAll;
public interface IGetAllRevenuesUseCase
{
    Task<ResponseRevenuesJson> Execute();
}
