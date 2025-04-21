using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Entities;

namespace BarberBoss.Application.AutoMapper;

public class AutoMapping: Profile
{
    public AutoMapping()
    {
        RequestToEntity();
        EntityToResponse();
    }

    private void RequestToEntity()
    {
        CreateMap<RequestRevenueJson, Revenue>();
    }

    private void EntityToResponse()
    {
        CreateMap<Revenue, ResponseRegisteredRevenueJson>();
        CreateMap<Revenue, ResponseRevenueJson>();
    }
}

