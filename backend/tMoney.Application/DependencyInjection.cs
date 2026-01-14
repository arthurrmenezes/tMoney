using Microsoft.Extensions.DependencyInjection;
using tMoney.Application.Services.AccountContext;
using tMoney.Application.Services.AccountContext.Interfaces;
using tMoney.Application.Services.AuthContext;
using tMoney.Application.Services.AuthContext.Interfaces;
using tMoney.Application.Services.CategoryContext;
using tMoney.Application.Services.CategoryContext.Interface;
using tMoney.Application.Services.InstallmentContext;
using tMoney.Application.Services.InstallmentContext.Interfaces;
using tMoney.Application.Services.TransactionContext;
using tMoney.Application.Services.TransactionContext.Interfaces;
using tMoney.Application.UseCases.AuthContext.GoogleAuthUseCase;
using tMoney.Application.UseCases.AuthContext.GoogleAuthUseCase.Inputs;
using tMoney.Application.UseCases.AuthContext.GoogleAuthUseCase.Outputs;
using tMoney.Application.UseCases.Interfaces;
using tMoney.Application.UseCases.TransactionContext.CreateTransactionUseCase;
using tMoney.Application.UseCases.TransactionContext.CreateTransactionUseCase.Inputs;
using tMoney.Application.UseCases.TransactionContext.CreateTransactionUseCase.Outputs;
using tMoney.Application.UseCases.TransactionContext.DeleteTransactionUseCase;
using tMoney.Application.UseCases.TransactionContext.DeleteTransactionUseCase.Inputs;
using tMoney.Application.UseCases.TransactionContext.GetAllTransactionsUseCase;
using tMoney.Application.UseCases.TransactionContext.GetAllTransactionsUseCase.Inputs;
using tMoney.Application.UseCases.TransactionContext.GetAllTransactionsUseCase.Outputs;
using tMoney.Application.UseCases.TransactionContext.GetTransactionUseCase;
using tMoney.Application.UseCases.TransactionContext.GetTransactionUseCase.Inputs;
using tMoney.Application.UseCases.TransactionContext.GetTransactionUseCase.Outputs;
using tMoney.Application.UseCases.TransactionContext.UpdateOverdueTransactionsUseCase;
using tMoney.Application.UseCases.TransactionContext.UpdateTransactionUseCase;
using tMoney.Application.UseCases.TransactionContext.UpdateTransactionUseCase.Input;
using tMoney.Application.UseCases.TransactionContext.UpdateTransactionUseCase.Outputs;

namespace tMoney.Application;

public static class DependencyInjection
{
    public static IServiceCollection ApplyApplicationDependencyInjection(this IServiceCollection serviceCollection)
    {
        #region Services Dependencies Configuration

        serviceCollection.AddScoped<IAuthService, AuthService>();
        serviceCollection.AddScoped<IAccountService, AccountService>();
        serviceCollection.AddScoped<ICategoryService, CategoryService>();
        serviceCollection.AddScoped<ITransactionService, TransactionService>();
        serviceCollection.AddScoped<IInstallmentService, InstallmentService>();

        #endregion

        #region Use Cases Dependencies Configuration

        serviceCollection.AddScoped<IUseCase<CreateTransactionUseCaseInput, CreateTransactionUseCaseOutput>, CreateTransactionUseCase>();
        serviceCollection.AddScoped<IUseCase<GetTransactionUseCaseInput, GetTransactionUseCaseOutput>, GetTransactionUseCase>();
        serviceCollection.AddScoped<IUseCase<GetAllTransactionsUseCaseInput, GetAllTransactionsUseCaseOutput>, GetAllTransactionsUseCase>();
        serviceCollection.AddScoped<IUseCase<UpdateTransactionUseCaseInput, UpdateTransactionUseCaseOutput>, UpdateTransactionUseCase>();
        serviceCollection.AddScoped<IUseCase<DeleteTransactionUseCaseInput>, DeleteTransactionUseCase>();
        serviceCollection.AddScoped<UpdateOverdueTransactionsUseCase>();

        serviceCollection.AddScoped<IUseCase<GoogleAuthUseCaseInput, GoogleAuthUseCaseOutput>, GoogleAuthUseCase>();

        #endregion

        return serviceCollection;
    }
}
