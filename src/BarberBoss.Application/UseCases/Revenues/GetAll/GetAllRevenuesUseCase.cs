using AutoMapper;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories.Revenues;

namespace BarberBoss.Application.UseCases.Revenues.GetAll;

public class GetAllRevenuesUseCase: IGetAllRevenuesUseCase
{
    private readonly IRevenuesRepository _repository;
    private readonly IMapper _mapper;

    public GetAllRevenuesUseCase(IRevenuesRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResponseRevenuesJson> Execute()
    {
        var result = await _repository.GetAll();

        return new ResponseRevenuesJson
        {
            Revenues = _mapper.Map<List<ResponseRevenueJson>>(result)
        };
    }
}

