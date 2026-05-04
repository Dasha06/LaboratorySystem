using Data.Models;
using Data.Repositories.Implementations;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Data.DependencyInjection;

public static class DataServiceCollectionExtensions
{
    public static IServiceCollection AddDataLayer(this IServiceCollection services,
        Action<DbContextOptionsBuilder> configureDbContext)
    {
        services.AddDbContext<SystemdatabaseContext>(configureDbContext);

        services.AddScoped<IAnalysisDepartmentRepository, AnalysisDepartmentRepositoryImpl>();
        services.AddScoped<IAnalysisWorkRepository, AnalysisWorkRepositoryImpl>();
        services.AddScoped<IAnalysiseRepository, AnalysiseRepositoryImpl>();
        services.AddScoped<IAnalysisesTemplateRepository, AnalysisesTemplateRepositoryImpl>();
        services.AddScoped<IBarcodeAnalysiseRepository, BarcodeAnalysiseRepositoryImpl>();
        services.AddScoped<IBarcodeMaterialRepository, BarcodeMaterialRepositoryImpl>();
        services.AddScoped<IContractRepository, ContractRepositoryImpl>();
        services.AddScoped<IContractAnalysiseRepository, ContractAnalysiseRepositoryImpl>();
        services.AddScoped<IDoctorRepository, DoctorRepositoryImpl>();
        services.AddScoped<ILpuRepository, LpuRepositoryImpl>();
        services.AddScoped<ILpuContractRepository, LpuContractRepositoryImpl>();
        services.AddScoped<IMaterialRepository, MaterialRepositoryImpl>();
        services.AddScoped<IMeasurementRepository, MeasurementRepositoryImpl>();
        services.AddScoped<IOrderRepository, OrderRepositoryImpl>();
        services.AddScoped<IOrderChangeRepository, OrderChangeRepositoryImpl>();
        services.AddScoped<IPatientRepository, PatientRepositoryImpl>();
        services.AddScoped<IPatientChangeRepository, PatientChangeRepositoryImpl>();
        services.AddScoped<IQualitativeStandartRepository, QualitativeStandartRepositoryImpl>();
        services.AddScoped<IQuantitativeStandartRepository, QuantitativeStandartRepositoryImpl>();
        services.AddScoped<IReferentialGroupRepository, ReferentialGroupRepositoryImpl>();
        services.AddScoped<IRoleRepository, RoleRepositoryImpl>();
        services.AddScoped<ITripodRepository, TripodRepositoryImpl>();
        services.AddScoped<ITripodBarcodeMaterialRepository, TripodBarcodeMaterialRepositoryImpl>();
        services.AddScoped<ITypeChangeRepository, TypeChangeRepositoryImpl>();
        services.AddScoped<IWorkerRepository, WorkerRepositoryImpl>();

        return services;
    }
}
