using System.Net.Mime;
using BarberBoss.Application.UseCases.Revenues.Delete;
using BarberBoss.Application.UseCases.Revenues.GetAll;
using BarberBoss.Application.UseCases.Revenues.GetById;
using BarberBoss.Application.UseCases.Revenues.Register;
using BarberBoss.Application.UseCases.Revenues.Reports.Excel;
using BarberBoss.Application.UseCases.Revenues.Reports.Pdf;
using BarberBoss.Application.UseCases.Revenues.Update;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace BarberBoss.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RevenuesController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredRevenueJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRevenue([FromServices] IRegisterRevenueUseCase useCase, [FromBody] RequestRevenueJson request)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseRevenuesJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetRevenues([FromServices] IGetAllRevenuesUseCase useCase)
    {
        var response = await useCase.Execute();

        if (response.Revenues.Count > 0)
        {
            return Ok(response);
        }

        return NoContent();
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseRevenueJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRevenueById([FromServices] IGetRevenueByIdUseCase useCase, [FromRoute] long id)
    {
        var response = await useCase.Execute(id);

        if (response is null)
        {
            return NotFound();
        }

        return Ok(response);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EditRevenue([FromServices] IUpdateRevenueUseCase useCase, [FromRoute] long id, [FromBody] RequestRevenueJson request)
    {
        await useCase.Execute(id, request);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRevenue([FromServices] IDeleteRevenueUseCase useCase, [FromRoute] long id)
    {
        await useCase.Execute(id);
        
        return NoContent();
    }

    [HttpGet("report/excel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetExcel([FromServices] IGenerateRevenuesReportExcelUseCase useCase, [FromHeader] DateOnly month)
    {
        byte[] file = await useCase.Execute(month);

        if (file.Length > 0)
            return File(file, MediaTypeNames.Application.Octet, "report.xlsx");

        return NoContent();
    }

    [HttpGet("report/pdf")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetPdf([FromServices] IGenerateRevenuesReportPdfUseCase useCase, [FromHeader] DateOnly month)
    {
        byte[] file = await useCase.Execute(month);

        if (file.Length > 0)
            return File(file, MediaTypeNames.Application.Pdf, "report.pdf");

        return NoContent();
    }
}
