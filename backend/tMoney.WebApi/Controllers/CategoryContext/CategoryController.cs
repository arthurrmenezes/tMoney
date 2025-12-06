using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tMoney.Application.Services.CategoryContext.Inputs;
using tMoney.Application.Services.CategoryContext.Interface;
using tMoney.Domain.BoundedContexts.CategoryContext.ENUMs;
using tMoney.Domain.ValueObjects;
using tMoney.WebApi.Controllers.CategoryContext.Payloads;
using tMoney.WebApi.Extensions;

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

        var accountId = User.GetAccountId();

        var response = await _categoryService.CreateCategoryServiceAsync(
            accountId: IdValueObject.Factory(accountId),
            input: CreateCategoryServiceInput.Factory(
                title: input.Title,
                type: input.Type),
            cancellationToken);

        return Ok(response);
    }

    [HttpGet]
    [Route("{categoryId}")]
    [Authorize]
    public async Task<IActionResult> GetCategoryByIdAsync(
        [FromRoute] Guid categoryId,
        CancellationToken cancellationToken)
    {
        var accountId = User.GetAccountId();

        var response = await _categoryService.GetCategoryByIdServiceAsync(
            categoryId: IdValueObject.Factory(categoryId),
            accountId: IdValueObject.Factory(accountId),
            cancellationToken: cancellationToken);

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

        var accountId = User.GetAccountId();

        var response = await _categoryService.GetAllCategoriesByAccountIdServiceAsync(
            accountId: IdValueObject.Factory(accountId),
            pageNumber: pageNumber,
            pageSize: pageSize,
            cancellationToken: cancellationToken);

        return Ok(response);
    }

    [HttpPatch]
    [Route("{categoryId}")]
    [Authorize]
    public async Task<IActionResult> UpdateCategoryDetailsByTitleAsync(
        [FromRoute] Guid categoryId,
        [FromBody] UpdateCategoryDetailsByTitlePayload input,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(input.NewTitle) && input.NewType.ToString() is null)
            throw new ArgumentException("Informe pelo menos um campo para atualizar.");

        if (!string.IsNullOrEmpty(input.NewType.ToString()))
        {
            if (!Enum.IsDefined(typeof(CategoryType), input.NewType!))
                throw new ArgumentException("Tipo de categoria inválido.");
        }

        var accountId = User.GetAccountId();

        var response = await _categoryService.UpdateCategoryDetailsByIdServiceAsync(
            categoryId: IdValueObject.Factory(categoryId),
            accountId: IdValueObject.Factory(accountId),
            input: UpdateCategoryDetailsByTitleServiceInput.Factory(
                newTitle: input.NewTitle,
                newType: input.NewType),
            cancellationToken: cancellationToken);

        return Ok(response);
    }

    [HttpDelete]
    [Route("{categoryId}")]
    [Authorize]
    public async Task<IActionResult> DeleteCategoryByIdAsync(
        [FromRoute] Guid categoryId,
        CancellationToken cancellationToken)
    {
        var accountId = User.GetAccountId();

        await _categoryService.DeleteCategoryByIdServiceAsync(
            categoryId: IdValueObject.Factory(categoryId),
            accountId: IdValueObject.Factory(accountId),
            cancellationToken: cancellationToken);

        return NoContent();
    }
}
