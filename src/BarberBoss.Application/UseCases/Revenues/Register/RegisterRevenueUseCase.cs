using System.IO.MemoryMappedFiles;
using System.Net.Http.Headers;
using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Revenues;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Revenues.Register;

public class RegisterRevenueUseCase: IRegisterRevenueUseCase
{

    private readonly IRevenuesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RegisterRevenueUseCase(IRevenuesRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseRegisteredRevenueJson> Execute(RequestRevenueJson request)
    {
        Validate(request);

        var entity = _mapper.Map<Revenue>(request);

        await _repository.Add(entity);

        await _unitOfWork.Commit();

        return _mapper.Map<ResponseRegisteredRevenueJson>(entity);
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

