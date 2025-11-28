using tMoney.Application.Services.CategoryContext.Inputs;
using tMoney.Application.Services.CategoryContext.Outputs;
using tMoney.Domain.ValueObjects;

namespace tMoney.Application.Services.CategoryContext.Interface;

public interface ICategoryService
{
    public Task<CreateCategoryServiceOutput> CreateCategoryServiceAsync(
        IdValueObject accountId,
        CreateCategoryServiceInput input,
        CancellationToken cancellationToken);

    public Task<GetAllCategoriesByAccountIdServiceOutput> GetAllCategoriesByAccountIdServiceAsync(
        IdValueObject accountId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);

    public Task<UpdateCategoryDetailsByIdServiceOutput> UpdateCategoryDetailsByIdServiceAsync(
        IdValueObject categoryId,
        IdValueObject accountId,
        UpdateCategoryDetailsByTitleServiceInput input,
        CancellationToken cancellationToken);
}
