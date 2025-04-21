
using System.Reflection;
using BarberBoss.Application.UseCases.Revenues.Reports.Pdf.Colors;
using BarberBoss.Application.UseCases.Revenues.Reports.Pdf.Fonts;
using BarberBoss.Domain.Reports;
using BarberBoss.Domain.Repositories.Revenues;
using CashFlow.Domain.Extensions;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Drawing;
using PdfSharp.Fonts;

namespace BarberBoss.Application.UseCases.Revenues.Reports.Pdf;

public class GenerateRevenuesReportPdfUseCase : IGenerateRevenuesReportPdfUseCase
{
    private const string CURRENCY_SIMBOL = "$";
    private const int HEIGHT_ROW_REVENUE_TABLE = 25;

    private readonly IRevenuesRepository _repository;
    public GenerateRevenuesReportPdfUseCase(IRevenuesRepository repository)
    {
        _repository = repository;

        GlobalFontSettings.FontResolver = new ExpensesReportFontResolver();
    }

    public async Task<byte[]> Execute(DateOnly month)
    {
        var revenues = await _repository.FilterByMonth(month);

        if (revenues.Count == 0)
        {
            return [];
        }

        var document = CreateDocument(month);
        var page = CreatePage(document);

        CreateHeaderWithProfilePhotoAndName(page);

        var totalRevenues = revenues.Sum(expense => expense.Amount);

        CreateWeeklyRevenueSection(page, month, totalRevenues);

        foreach (var revenue in revenues)
        {
            var table = CreateRevenueTable(page);

            var row = table.AddRow();
            row.Height = HEIGHT_ROW_REVENUE_TABLE;

            AddRevenueTitle(row.Cells[0], revenue.Title);
            AddHeaderForAmount(row.Cells[3]);


            row = table.AddRow();
            row.Height = HEIGHT_ROW_REVENUE_TABLE;

            row.Cells[0].AddParagraph(revenue.OccurredAt.ToString("D"));
            SetStyleBaseForRevenueInformation(row.Cells[0]);
            row.Cells[0].Format.LeftIndent = 9;

            row.Cells[1].AddParagraph(revenue.OccurredAt.ToString("t"));
            SetStyleBaseForRevenueInformation(row.Cells[1]);

            row.Cells[2].AddParagraph(revenue.PaymentType.PaymentTypeToString());
            SetStyleBaseForRevenueInformation(row.Cells[2]);

            AddAmountForRevenue(row.Cells[3], revenue.Amount);

            if (!string.IsNullOrWhiteSpace(revenue.Description))
            {
                var descriptionRow = table.AddRow();
                descriptionRow.Height = HEIGHT_ROW_REVENUE_TABLE;

                descriptionRow.Cells[0].AddParagraph(revenue.Description);

                descriptionRow.Cells[0].Format.Font = new Font
                {
                    Name = FontHelper.ROBOTO_REGULAR,
                    Size = 10,
                    Color = Colors.ColorsHelper.DESCRIPTION
                };

                descriptionRow.Cells[0].Shading.Color = ColorsHelper.GRAY_LIGHT;
                descriptionRow.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                descriptionRow.Cells[0].MergeRight = 2;
                descriptionRow.Cells[0].Format.LeftIndent = 9;
            }

            AddWhiteSpace(table);

        }

        return RenderDocument(document);
    }

    private Document CreateDocument(DateOnly month)
    {
        var document = new Document();

        document.Info.Title = $"{ResourceReportGenerationMessages.REVENUES_FOR} {month:Y}";
        document.Info.Author = "BarberBoss";

        var style = document.Styles["Normal"];
        style!.Font.Name = FontHelper.ROBOTO_REGULAR;

        return document;
    }

    private Section CreatePage(Document document)
    {
        var section = document.AddSection();

        section.PageSetup = document.DefaultPageSetup.Clone();

        section.PageSetup.PageFormat = PageFormat.A4;

        section.PageSetup.LeftMargin = 40;
        section.PageSetup.RightMargin = 40;
        section.PageSetup.TopMargin = 50;
        section.PageSetup.BottomMargin = 50;

        return section;
    }

    private void CreateHeaderWithProfilePhotoAndName(Section page)
    {
        var table = page.AddTable();

        table.AddColumn();
        table.AddColumn("300");

        var row = table.AddRow();

        var assembly = Assembly.GetExecutingAssembly();
        var directoryName = Path.GetDirectoryName(assembly.Location);
        var pathFile = Path.Combine(directoryName!, "Logo", "logo.png");

        row.Cells[0].AddImage(pathFile);

        row.Cells[1].AddParagraph("BarberBoss");
        row.Cells[1].Format.Font = new Font { Name = FontHelper.BEBAS_NEUE_REGULAR, Size = 25 };
        row.Cells[1].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;
        row.Cells[1].Format.LeftIndent = "13.5pt";
    }

    private void CreateWeeklyRevenueSection(Section page, DateOnly month, decimal totalRevenues)
    {
        var paragraph = page.AddParagraph();

        paragraph.Format.SpaceBefore = "40";
        paragraph.Format.SpaceAfter = "40";

        var title = string.Format(ResourceReportGenerationMessages.MONTHLY_REVENUE);

        paragraph.AddFormattedText(title, new Font { Name = FontHelper.ROBOTO_MEDIUM, Size = 15 });

        paragraph.AddLineBreak();

        paragraph.AddFormattedText($"{totalRevenues} {CURRENCY_SIMBOL}", new Font { Name = FontHelper.BEBAS_NEUE_REGULAR, Size = 50 });
    }

    private Table CreateRevenueTable(Section page)
    {
        var table = page.AddTable();

        table.AddColumn("143").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("140").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("147").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("70").Format.Alignment = ParagraphAlignment.Right;
        return table;
    }

    private void AddRevenueTitle(Cell cell, string expenseTitle)
    {
        cell.AddParagraph(expenseTitle);

        cell.Format.Font = new Font
        {
            Name = FontHelper.BEBAS_NEUE_REGULAR,
            Size = 15,
            Color = Colors.ColorsHelper.WHITE
        };

        cell.Shading.Color = ColorsHelper.GREEN_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.MergeRight = 2;
        cell.Format.LeftIndent = 5;
    }

    private void AddHeaderForAmount(Cell cell)
    {
        cell.AddParagraph(ResourceReportGenerationMessages.AMOUNT);

        cell.Format.Font = new Font
        {
            Name = FontHelper.BEBAS_NEUE_REGULAR,
            Size = 15,
            Color = Colors.ColorsHelper.WHITE
        };

        cell.Shading.Color = ColorsHelper.GREEN;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void SetStyleBaseForRevenueInformation(Cell cell)
    {
        cell.Format.Font = new Font
        {
            Name = FontHelper.ROBOTO_REGULAR,
            Size = 10,
            Color = Colors.ColorsHelper.BLACK
        };

        cell.Shading.Color = ColorsHelper.GRAY;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void AddAmountForRevenue(Cell cell, decimal amount)
    {
        cell.AddParagraph($"{amount} {CURRENCY_SIMBOL}");

        cell.Format.Font = new Font
        {
            Name = FontHelper.ROBOTO_REGULAR,
            Size = 10,
            Color = Colors.ColorsHelper.BLACK
        };

        cell.Shading.Color = ColorsHelper.WHITE;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void AddWhiteSpace(Table table)
    {
        var row = table.AddRow();
        row.Height = 15;
        row.Borders.Visible = false;
    }

    private byte[] RenderDocument(Document document)
    {
        var renderer = new PdfDocumentRenderer
        {
            Document = document,
        };

        renderer.RenderDocument();

        using var file = new MemoryStream();
        renderer.PdfDocument.Save(file);

        return file.ToArray();
    }

}

