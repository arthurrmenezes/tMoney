using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tMoney.Application.Services.CategoryContext.Inputs;
using tMoney.Application.Services.CategoryContext.Interface;
using tMoney.Domain.BoundedContexts.CategoryContext.ENUMs;
using tMoney.Domain.ValueObjects;
using tMoney.WebApi.Controllers.CategoryContext.Payloads;

namespace tMoney.WebApi.Controllers.CategoryContext;

[ApiController]
[Route("api/v1/categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateCategoryAsync(
        [FromBody] CreateCategoryPayload input,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(input.Title))
            throw new ArgumentException("O nome é obrigatório.");

        if (!Enum.IsDefined(typeof(CategoryType), input.Type))
            throw new ArgumentException("Tipo de categoria inválido.");

        var accountIdString = User.FindFirst("accountId")?.Value;
        if (string.IsNullOrEmpty(accountIdString))
            throw new ArgumentException("Conta não encontrada.");

        if (!Guid.TryParse(accountIdString, out var accountIdGuid))
            throw new ArgumentException("ID da conta inválido no token");

        var response = await _categoryService.CreateCategoryServiceAsync(
            accountId: IdValueObject.Factory(accountIdGuid),
            input: CreateCategoryServiceInput.Factory(
                title: input.Title,
                type: input.Type),
            cancellationToken);

        return Ok(response);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllCategoriesByAccountIdAsync(
        CancellationToken cancellationToken,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 50) pageSize = 50;

        var accountIdString = User.FindFirst("accountId")?.Value;
        if (string.IsNullOrEmpty(accountIdString))
            throw new ArgumentException("Conta não encontrada.");

        if (!Guid.TryParse(accountIdString, out var accountIdGuid))
            throw new ArgumentException("ID da conta inválido no token");

        var response = await _categoryService.GetAllCategoriesByAccountIdServiceAsync(
            accountId: IdValueObject.Factory(accountIdGuid),
            pageNumber: pageNumber,
            pageSize: pageSize,
            cancellationToken: cancellationToken);

        return Ok(response);
    }
}
