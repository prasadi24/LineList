using AutoMapper;
using LineList.Cenovus.Com.Common;
using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Repositories;
using LineList.Cenovus.Com.Domain.Services;
using LineList.Cenovus.Com.Infrastructure.Context;
using LineList.Cenovus.Com.Infrastructure.Repositories;
using LineList.Cenovus.Com.RulesEngine;
using LineList.Cenovus.Com.Security;
using LineList.Cenovus.Com.UI.Security;
using LineListModelList.Cenovus.Com.Domain.Services;

namespace LineList.Cenovus.Com.UI.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IMapper>(AutomapperConfig.Configure());
            services.AddScoped<LineListDbContext>();

            services.AddScoped<EmailUtility>();

            services.AddScoped<ILineListModelRepository, LineListModelRepository>();
            services.AddScoped<ILineListModelService, LineListModelService>();

            services.AddScoped<IFacilityRepository, FacilityRepository>();
            services.AddScoped<IFacilityService, FacilityService>();

            services.AddScoped<IAreaRepository, AreaRepository>();
            services.AddScoped<IAreaService, AreaService>();
            services.AddScoped<ICodeRepository, CodeRepository>();
            services.AddScoped<ICodeService, CodeService>();

            services.AddScoped<ICenovusProjectRepository, CenovusProjectRepository>();
            services.AddScoped<ICenovusProjectService, CenovusProjectService>();

            services.AddScoped<ICommodityRepository, CommodityRepository>();
            services.AddScoped<ICommodityService, CommodityService>();

            services.AddScoped<IConcurrentEngineeringLineRepository, ConcurrentEngineeringLineRepository>();
            services.AddScoped<IConcurrentEngineeringLineService, ConcurrentEngineeringLineService>();

            services.AddScoped<ICorrosionAllowanceRepository, CorrosionAllowanceRepository>();
            services.AddScoped<ICorrosionAllowanceService, CorrosionAllowanceService>();

            services.AddScoped<ICsaClassLocationRepository, CsaClassLocationRepository>();
            services.AddScoped<ICsaClassLocationService, CsaClassLocationService>();

            services.AddScoped<ICsaHvpLvpRepository, CsaHvpLvpRepository>();
            services.AddScoped<ICsaHvpLvpService, CsaHvpLvpService>();

            services.AddScoped<IEpCompanyAlphaRepository, EpCompanyAlphaRepository>();
            services.AddScoped<IEpCompanyAlphaService, EpCompanyAlphaService>();

            services.AddScoped<IEpCompanyRepository, EpCompanyRepository>();
            services.AddScoped<IEpCompanyService, EpCompanyService>();

            services.AddScoped<IEpProjectInsulationDefaultColumnRepository, EpProjectInsulationDefaultColumnRepository>();
            services.AddScoped<IEpProjectInsulationDefaultColumnService, EpProjectInsulationDefaultColumnService>();

            services.AddScoped<IEpProjectInsulationDefaultDetailRepository, EpProjectInsulationDefaultDetailRepository>();
            services.AddScoped<IEpProjectInsulationDefaultDetailService, EpProjectInsulationDefaultDetailService>();

            services.AddScoped<IEpProjectInsulationDefaultRowRepository, EpProjectInsulationDefaultRowRepository>();
            services.AddScoped<IEpProjectInsulationDefaultRowService, EpProjectInsulationDefaultRowService>();

            services.AddScoped<IEpProjectInsulationDefaultRepository, EpProjectInsulationDefaultRepository>();
            services.AddScoped<IEpProjectInsulationDefaultService, EpProjectInsulationDefaultService>();

            services.AddScoped<IEpProjectRoleRepository, EpProjectRoleRepository>();
            services.AddScoped<IEpProjectRoleService, EpProjectRoleService>();

            services.AddScoped<IEpProjectRepository, EpProjectRepository>();
            services.AddScoped<IEpProjectService, EpProjectService>();

            services.AddScoped<IEpProjectUserRoleRepository, EpProjectUserRoleRepository>();
            services.AddScoped<IEpProjectUserRoleService, EpProjectUserRoleService>();

            services.AddScoped<IFluidPhaseRepository, FluidPhaseRepository>();
            services.AddScoped<IFluidPhaseService, FluidPhaseService>();

            services.AddScoped<IFluidRepository, FluidRepository>();
            services.AddScoped<IFluidService, FluidService>();

            services.AddScoped<IImportCommodityRepository, ImportCommodityRepository>();
            services.AddScoped<IImportCommodityService, ImportCommodityService>();

            services.AddScoped<IImportFacilityRepository, ImportFacilityRepository>();
            services.AddScoped<IImportFacilityService, ImportFacilityService>();

            services.AddScoped<IImportLocationRepository, ImportLocationRepository>();
            services.AddScoped<IImportLocationService, ImportLocationService>();

            services.AddScoped<IImportRowExceptionRepository, ImportRowExceptionRepository>();
            services.AddScoped<IImportRowExceptionService, ImportRowExceptionService>();

            services.AddScoped<IImportRowRepository, ImportRowRepository>();
            services.AddScoped<IImportRowService, ImportRowService>();

            services.AddScoped<IImportRowViewRepository, ImportRowViewRepository>();
            services.AddScoped<IImportRowViewService, ImportRowViewService>();

            services.AddScoped<IImportRepository, ImportRepository>();
            services.AddScoped<IImportService, ImportService>();

            services.AddScoped<IImportSheetColumnRepository, ImportSheetColumnRepository>();
            services.AddScoped<IImportSheetColumnService, ImportSheetColumnService>();

            services.AddScoped<IImportSheetRepository, ImportSheetRepository>();
            services.AddScoped<IImportSheetService, ImportSheetService>();

            services.AddScoped<IInsulationDefaultColumnRepository, InsulationDefaultColumnRepository>();
            services.AddScoped<IInsulationDefaultColumnService, InsulationDefaultColumnService>();

            services.AddScoped<IInsulationDefaultDetailRepository, InsulationDefaultDetailRepository>();
            services.AddScoped<IInsulationDefaultDetailService, InsulationDefaultDetailService>();

            services.AddScoped<IInsulationDefaultRowRepository, InsulationDefaultRowRepository>();
            services.AddScoped<IInsulationDefaultRowService, InsulationDefaultRowService>();

            services.AddScoped<IInsulationDefaultRepository, InsulationDefaultRepository>();
            services.AddScoped<IInsulationDefaultService, InsulationDefaultService>();

            services.AddScoped<IInsulationMaterialRepository, InsulationMaterialRepository>();
            services.AddScoped<IInsulationMaterialService, InsulationMaterialService>();

            services.AddScoped<IInsulationThicknessRepository, InsulationThicknessRepository>();
            services.AddScoped<IInsulationThicknessService, InsulationThicknessService>();

            services.AddScoped<IInsulationTypeRepository, InsulationTypeRepository>();
            services.AddScoped<IInsulationTypeService, InsulationTypeService>();

            services.AddScoped<IInternalCoatingLinerRepository, InternalCoatingLinerRepository>();
            services.AddScoped<IInternalCoatingLinerService, InternalCoatingLinerService>();

            services.AddScoped<ILineDesignationTableViewHeaderRepository, LineDesignationTableViewHeaderRepository>();
            services.AddScoped<ILineDesignationTableViewHeaderService, LineDesignationTableViewHeaderService>();

            //services.AddScoped<ILineDesignationTableViewRevisionRepository, LineDesignationTableViewRevisionRepository>();
            //services.AddScoped<ILineDesignationTableViewRevisionService, LineDesignationTableViewRevisionService>();

            services.AddScoped<ILineListRevisionRepository, LineListRevisionRepository>();
            services.AddScoped<ILineListRevisionService, LineListRevisionService>();

            services.AddScoped<ILineListStatusRepository, LineListStatusRepository>();
            services.AddScoped<ILineListStatusService, LineListStatusService>();

            services.AddScoped<ILineListStatusStateRepository, LineListStatusStateRepository>();
            services.AddScoped<ILineListStatusStateService, LineListStatusStateService>();

            services.AddScoped<ILineRevisionOperatingModeRepository, LineRevisionOperatingModeRepository>();
            services.AddScoped<ILineRevisionOperatingModeService, LineRevisionOperatingModeService>();

            services.AddScoped<ILineRevisionSegmentRepository, LineRevisionSegmentRepository>();
            services.AddScoped<ILineRevisionSegmentService, LineRevisionSegmentService>();

            services.AddScoped<ILineRevisionRepository, LineRevisionRepository>();
            services.AddScoped<ILineRevisionService, LineRevisionService>();

            services.AddScoped<ILineRepository, LineRepository>();
            services.AddScoped<ILineService, LineService>();

            services.AddScoped<ILineStatusRepository, LineStatusRepository>();
            services.AddScoped<ILineStatusService, LineStatusService>();

            services.AddScoped<ILLLookupTableNoDescriptionRepository, LLLookupTableNoDescriptionRepository>();
            services.AddScoped<ILLLookupTableNoDescriptionService, LLLookupTableNoDescriptionService>();

            services.AddScoped<ILLLookupTableRepository, LLLookupTableRepository>();
            services.AddScoped<ILLLookupTableService, LLLookupTableService>();

            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<ILocationService, LocationService>();

            services.AddScoped<ILocationTypeRepository, LocationTypeRepository>();
            services.AddScoped<ILocationTypeService, LocationTypeService>();

            services.AddScoped<IModularRepository, ModularRepository>();
            services.AddScoped<IModularService, ModularService>();

            services.AddScoped<INdeCategoryRepository, NdeCategoryRepository>();
            services.AddScoped<INdeCategoryService, NdeCategoryService>();

            services.AddScoped<IOperatingModeRepository, OperatingModeRepository>();
            services.AddScoped<IOperatingModeService, OperatingModeService>();

            services.AddScoped<IPaintSystemRepository, PaintSystemRepository>();
            services.AddScoped<IPaintSystemService, PaintSystemService>();

            services.AddScoped<IPipeSpecificationRepository, PipeSpecificationRepository>();
            services.AddScoped<IPipeSpecificationService, PipeSpecificationService>();

            services.AddScoped<IPostWeldHeatTreatmentRepository, PostWeldHeatTreatmentRepository>();
            services.AddScoped<IPostWeldHeatTreatmentService, PostWeldHeatTreatmentService>();

            services.AddScoped<IPressureProtectionRepository, PressureProtectionRepository>();
            services.AddScoped<IPressureProtectionService, PressureProtectionService>();

            services.AddScoped<IProjectTypeRepository, ProjectTypeRepository>();
            services.AddScoped<IProjectTypeService, ProjectTypeService>();

            services.AddScoped<IReferenceLineRepository, ReferenceLineRepository>();
            services.AddScoped<IReferenceLineService, ReferenceLineService>();

            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<IRoleUserRepository, RoleUserRepository>();
            services.AddScoped<IRoleUserService, RoleUserService>();

            services.AddScoped<IScheduleDefaultRepository, ScheduleDefaultRepository>();
            services.AddScoped<IScheduleDefaultService, ScheduleDefaultService>();

            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<IScheduleService, ScheduleService>();

            services.AddScoped<ISegmentTypeRepository, SegmentTypeRepository>();
            services.AddScoped<ISegmentTypeService, SegmentTypeService>();

            services.AddScoped<ISizeNpsRepository, SizeNpsRepository>();
            services.AddScoped<ISizeNpsService, SizeNpsService>();

            services.AddScoped<ISpecificationRepository, SpecificationRepository>();
            services.AddScoped<ISpecificationService, SpecificationService>();

            services.AddScoped<IStressAnalysisRepository, StressAnalysisRepository>();
            services.AddScoped<IStressAnalysisService, StressAnalysisService>();

            services.AddScoped<ITestMediumRepository, TestMediumRepository>();
            services.AddScoped<ITestMediumService, TestMediumService>();

            services.AddScoped<ITestPressureRepository, TestPressureRepository>();
            services.AddScoped<ITestPressureService, TestPressureService>();

            services.AddScoped<ITracingDesignNumberOfTracersRepository, TracingDesignNumberOfTracersRepository>();
            services.AddScoped<ITracingDesignNumberOfTracersService, TracingDesignNumberOfTracersService>();

            services.AddScoped<ITracingTypeRepository, TracingTypeRepository>();
            services.AddScoped<ITracingTypeService, TracingTypeService>();

            services.AddScoped<IUserPreferenceRepository, UserPreferenceRepository>();
            services.AddScoped<IUserPreferenceService, UserPreferenceService>();

            services.AddScoped<IValidationFieldRepository, ValidationFieldRepository>();
            services.AddScoped<IValidationFieldService, ValidationFieldService>();

            services.AddScoped<IValidationRuleRepository, ValidationRuleRepository>();
            services.AddScoped<IValidationRuleService, ValidationRuleService>();

            services.AddScoped<IValidationRepository, ValidationRepository>();
            services.AddScoped<IValidationService, ValidationService>();

            services.AddScoped<IXrayRepository, XrayRepository>();
            services.AddScoped<IXrayService, XrayService>();

            services.AddScoped<IWelcomeMessageRepository, WelcomeMessageRepository>();
            services.AddScoped<IWelcomeMessageService, WelcomeMessageService>();


            services.AddScoped<INotesConfigurationService, NotesConfigurationService>();
            services.AddScoped<INotesConfigurationRepository, NotesConfigurationRepository>();

            services.AddScoped<LineListRules>();
            services.AddScoped<FlatFactory>();
            services.AddScoped<UserManager>();
            services.AddHttpContextAccessor();
            services.AddScoped<CurrentUser>();

            services.AddScoped<AzureAdService>();

            return services;
        }
    }
}