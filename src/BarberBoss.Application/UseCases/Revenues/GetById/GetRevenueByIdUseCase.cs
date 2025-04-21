using AutoMapper;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories.Revenues;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Revenues.GetById;

public class GetRevenueByIdUseCase : IGetRevenueByIdUseCase
{
    private readonly IRevenuesRepository _repository;
    private readonly IMapper _mapper;

    public GetRevenueByIdUseCase(IRevenuesRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<ResponseRevenueJson> Execute(long id)
    {
        var result = await _repository.GetById(id);

        if (result is null)
        {
            throw new NotFoundException(ResourceErrorMessages.REVENUE_NOT_FOUND);
        }

        return _mapper.Map<ResponseRevenueJson>(result);
    }
}

