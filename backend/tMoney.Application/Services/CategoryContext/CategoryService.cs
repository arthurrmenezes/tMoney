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
    private readonly ITransactionRepository _transactionRepository;

    public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, ITransactionRepository transactionRepository)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _transactionRepository = transactionRepository;
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
            accountId: accountId,
            isDefault: false);

        await _categoryRepository.AddAsync(category, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var output = CreateCategoryServiceOutput.Factory(
            id: category.Id.ToString(),
            title: category.Title,
            type: category.Type.ToString(),
            accountId: category.AccountId.ToString(),
            isDefault: category.IsDefault,
            updatedAt: null,
            createdAt: category.CreatedAt);

        return output;
    }

    public async Task<GetCategoryByIdServiceOutput> GetCategoryByIdServiceAsync(IdValueObject categoryId, IdValueObject accountId, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId.Id, accountId.Id, cancellationToken);
        if (category is null)
            throw new KeyNotFoundException("Categoria não encontrada");

        var output = GetCategoryByIdServiceOutput.Factory(
            id: category.Id.ToString(),
            title: category.Title,
            type: category.Type.ToString(),
            accountId: category.AccountId.ToString(),
            isDefault: category.IsDefault,
            updatedAt: category.UpdatedAt,
            createdAt: category.CreatedAt);

        return output;
    }

    public async Task<GetAllCategoriesByAccountIdServiceOutput> GetAllCategoriesByAccountIdServiceAsync(
        IdValueObject accountId, 
        GetAllCategoriesByAccountIdServiceInput input,
        CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync(accountId.Id, input.PageNumber, input.PageSize, input.CategoryType, input.TextSearch,
            cancellationToken);

        var categoriesOutput = categories.Select(c => new GetAllCategoriesByAccountIdServiceOutputCategory(
            id: c.Id.ToString(),
            title: c.Title,
            type: c.Type.ToString(),
            accountId: c.AccountId.ToString(),
            isDefault: c.IsDefault,
            updatedAt: c.UpdatedAt,
            createdAt: c.CreatedAt)).ToArray();

        var totalCategories = await _categoryRepository.GetTotalCategoriesNumberAsync(accountId.Id, input.CategoryType, input.TextSearch, cancellationToken);

        var totalPages = (int)Math.Ceiling((double)totalCategories / input.PageSize);

        var output = GetAllCategoriesByAccountIdServiceOutput.Factory(
            totalCategories: totalCategories,
            pageNumber: input.PageNumber,
            pageSize: input.PageSize,
            totalPages: totalPages,
            categoryType: input.CategoryType.HasValue ? input.CategoryType.ToString() : "All",
            categories: categoriesOutput);

        return output;
    }

    public async Task<UpdateCategoryDetailsByIdServiceOutput> UpdateCategoryDetailsByIdServiceAsync(IdValueObject categoryId, IdValueObject accountId,
        UpdateCategoryDetailsByTitleServiceInput input, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId.Id, accountId.Id, cancellationToken);
        if (category is null)
            throw new KeyNotFoundException("Categoria não encontrada.");

        if (category.IsDefault && input.NewType.HasValue && input.NewType != category.Type)
            throw new InvalidOperationException("Não é permitido alterar o tipo de uma categoria padrão.");

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
            isDefault: category.IsDefault,
            updatedAt: category.UpdatedAt!.Value,
            createdAt: category.CreatedAt);

        return output;
    }

    public async Task DeleteCategoryByIdServiceAsync(IdValueObject categoryId, IdValueObject accountId, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId.Id, accountId.Id, cancellationToken);
        if (category is null)
            throw new KeyNotFoundException("Categoria não encontrada");

        if (category.IsDefault)
            throw new InvalidOperationException("A categoria padrão não pode ser removida.");

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var defaultCategory = await _categoryRepository.GetDefaultCategoryByIdAsync(accountId.Id, cancellationToken);
            if (defaultCategory is null)
                throw new InvalidOperationException("Categoria padrão não encontrada para esta conta. Entre em contato com o suporte.");

            await _transactionRepository.UpdateCategoryForDefaultAsync(
                currentCategoryId: categoryId.Id,
                defaultCategoryId: defaultCategory.Id.Id,
                accountId: accountId.Id,
                cancellationToken: cancellationToken);

            _categoryRepository.Delete(category);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
