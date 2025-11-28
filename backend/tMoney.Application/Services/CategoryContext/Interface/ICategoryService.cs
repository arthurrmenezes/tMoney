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
}
