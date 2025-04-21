using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Revenues;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Revenues.Update;

public class UpdateRevenueUseCase : IUpdateRevenueUseCase
{
    private readonly IMapper _mapper;
    private readonly IRevenuesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRevenueUseCase(IMapper mapper, IUnitOfWork unitOfWork, IRevenuesRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
        _unitOfWork = unitOfWork;   
    }

    public async Task Execute(long id, RequestRevenueJson request)
    {
        Validate(request);

        var revenue = await _repository.GetById(id);

        if (revenue == null)
        {
            throw new NotFoundException(ResourceErrorMessages.REVENUE_NOT_FOUND);
        }

        _mapper.Map(request, revenue);
        _repository.Update(revenue);

        await _unitOfWork.Commit();
    }

    private void Validate(RequestRevenueJson request)
    {
        var validator = new RevenueValidator();
        var result = validator.Validate(request);

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}

