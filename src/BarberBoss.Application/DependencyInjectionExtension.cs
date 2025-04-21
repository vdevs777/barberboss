using Microsoft.Extensions.DependencyInjection;

using BarberBoss.Application.AutoMapper;

using BarberBoss.Application.UseCases.Revenues.GetAll;
using BarberBoss.Application.UseCases.Revenues.GetById;
using BarberBoss.Application.UseCases.Revenues.Register;
using BarberBoss.Application.UseCases.Revenues.Update;
using BarberBoss.Application.UseCases.Revenues.Delete;
using BarberBoss.Application.UseCases.Revenues.Reports.Excel;
using BarberBoss.Application.UseCases.Revenues.Reports.Pdf;

namespace BarberBoss.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddAutoMapper(services);
        AddUseCases(services);
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutoMapping));
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterRevenueUseCase , RegisterRevenueUseCase>();
        services.AddScoped<IGetAllRevenuesUseCase, GetAllRevenuesUseCase>();
        services.AddScoped<IGetRevenueByIdUseCase, GetRevenueByIdUseCase>();
        services.AddScoped<IUpdateRevenueUseCase, UpdateRevenueUseCase>();
        services.AddScoped<IDeleteRevenueUseCase, DeleteRevenueUseCase>();
        services.AddScoped<IGenerateRevenuesReportExcelUseCase, GenerateRevenuesReportExcelUseCase>();
        services.AddScoped<IGenerateRevenuesReportPdfUseCase, GenerateRevenuesReportPdfUseCase>();
    }
}

