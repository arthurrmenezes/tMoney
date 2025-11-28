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

    public async Task<GetAllCategoriesByAccountIdServiceOutput> GetAllCategoriesByAccountIdServiceAsync(IdValueObject accountId, int pageNumber, int pageSize,
        CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync(accountId.Id, pageNumber, pageSize, cancellationToken);

        var categoriesOutput = categories.Select(c => new GetAllCategoriesByAccountIdServiceOutputCategory(
            id: c.Id.ToString(),
            title: c.Title,
            type: c.Type.ToString(),
            accountId: c.AccountId.ToString(),
            updatedAt: c.UpdatedAt,
            createdAt: c.CreatedAt)).ToArray();

        var totalCategories = await _categoryRepository.GetTotalCategoriesNumberAsync(accountId.Id, cancellationToken);

        var totalPages = (int) Math.Ceiling((double)totalCategories / pageSize);

        var output = GetAllCategoriesByAccountIdServiceOutput.Factory(
            totalCategories: totalCategories,
            pageNumber: pageNumber,
            pageSize: pageSize,
            totalPages: totalPages,
            categories: categoriesOutput);

        return output;
    }

    public async Task<UpdateCategoryDetailsByIdServiceOutput> UpdateCategoryDetailsByIdServiceAsync(IdValueObject categoryId, IdValueObject accountId, 
        UpdateCategoryDetailsByTitleServiceInput input, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId.Id, accountId.Id, cancellationToken);
        if (category is null)
            throw new KeyNotFoundException("Categoria não encontrada.");

        if (input.NewTitle is not null && input.NewTitle != category.Title)
        {
            var titleExists = await _categoryRepository.GetByTitleAsync(input.NewTitle, accountId.Id, cancellationToken);
            if (titleExists is not null)
                throw new ArgumentException($"Você já possui uma categoria chamada '{input.NewTitle}'.");
        }

        category.UpdateCategoryDetails(input.NewTitle, input.NewType);

        _categoryRepository.Update(category);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var output = UpdateCategoryDetailsByIdServiceOutput.Factory(
            id: category.Id.ToString(),
            title: category.Title,
            type: category.Type.ToString(),
            accountId: category.AccountId.ToString(),
            updatedAt: category.UpdatedAt!.Value,
            createdAt: category.CreatedAt);

        return output;
    }
}
