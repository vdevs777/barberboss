
using BarberBoss.Domain.Repositories.Revenues;
using BarberBoss.Domain.Repositories;
using BarberBoss.Exception.ExceptionsBase;
using BarberBoss.Exception;

namespace BarberBoss.Application.UseCases.Revenues.Delete;

public class DeleteRevenueUseCase : IDeleteRevenueUseCase
{
    private readonly IRevenuesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRevenueUseCase(IUnitOfWork unitOfWork, IRevenuesRepository repository)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(long id)
    {
        var result = await _repository.Delete(id);

        if (result == false)
        {
            throw new NotFoundException(ResourceErrorMessages.REVENUE_NOT_FOUND);
        }

        await _unitOfWork.Commit();
    }
}

