using tMoney.Application.Services.CategoryContext.Inputs;
using tMoney.Application.Services.CategoryContext.Interface;
using tMoney.Application.Services.CategoryContext.Outputs;
using tMoney.Domain.BoundedContexts.CategoryContext.Entities;
using tMoney.Domain.ValueObjects;
using tMoney.Infrastructure.Data.Repositories.Interfaces;
using tMoney.Infrastructure.Data.UnitOfWork.Interfaces;

namespace tMoney.Application.Services.CategoryContext;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateCategoryServiceOutput> CreateCategoryServiceAsync(IdValueObject accountId, CreateCategoryServiceInput input, 
        CancellationToken cancellationToken)
    {
        var categoryExists = await _categoryRepository.GetByTitleAsync(input.Title, accountId.Id, cancellationToken);
        if (categoryExists is not null)
            throw new ArgumentException("Já existe uma categoria registrada com este nome.");

        var category = new Category(
            title: input.Title,
            type: input.Type,
            accountId: accountId);

        await _categoryRepository.AddAsync(category, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var output = CreateCategoryServiceOutput.Factory(
            id: category.Id.ToString(),
            title: category.Title,
            type: category.Type.ToString(),
            accountId: category.AccountId.ToString(),
            updatedAt: null,
            createdAt: category.CreatedAt);

        return output;
    }
}
