using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;

namespace BarberBoss.Application.UseCases.Revenues.Register;
public interface IRegisterRevenueUseCase
{
    Task<ResponseRegisteredRevenueJson> Execute(RequestRevenueJson request);
}
