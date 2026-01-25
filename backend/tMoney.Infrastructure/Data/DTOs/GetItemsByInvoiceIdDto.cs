using tMoney.Domain.BoundedContexts.InstallmentContext.Entities;
using tMoney.Domain.ValueObjects;

namespace tMoney.Infrastructure.Data.DTOs;

public sealed class GetItemsByInvoiceIdDto
{
    public InstallmentItem InstallmentItem { get; }
    public string Title { get; }
    public int TotalInstallments { get; }
    public IdValueObject CategoryId { get; }

    private GetItemsByInvoiceIdDto(InstallmentItem installmentItem, string title, int totalInstallments, IdValueObject categoryId)
    {
        InstallmentItem = installmentItem;
        Title = title;
        TotalInstallments = totalInstallments;
        CategoryId = categoryId;
    }

    public static GetItemsByInvoiceIdDto Factory(InstallmentItem installmentItem, string title, int totalInstallments, IdValueObject categoryId)
        => new(installmentItem, title, totalInstallments, categoryId);
}
