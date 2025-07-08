using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.Area;
using LineList.Cenovus.Com.API.DataTransferObjects.CenovusProject;
using LineList.Cenovus.Com.API.DataTransferObjects.Code;
using LineList.Cenovus.Com.API.DataTransferObjects.Commodity;
using LineList.Cenovus.Com.API.DataTransferObjects.ConcurrentEngineeringLine;
using LineList.Cenovus.Com.API.DataTransferObjects.CorrosionAllowance;
using LineList.Cenovus.Com.API.DataTransferObjects.CsaClassLocation;
using LineList.Cenovus.Com.API.DataTransferObjects.CsaHvpLvp;
using LineList.Cenovus.Com.API.DataTransferObjects.EpCompany;
using LineList.Cenovus.Com.API.DataTransferObjects.EpCompanyAlpha;
using LineList.Cenovus.Com.API.DataTransferObjects.EpProject;
using LineList.Cenovus.Com.API.DataTransferObjects.EpProjectInsulationDefault;
using LineList.Cenovus.Com.API.DataTransferObjects.EpProjectInsulationDefaultColumn;
using LineList.Cenovus.Com.API.DataTransferObjects.EpProjectInsulationDefaultDetail;
using LineList.Cenovus.Com.API.DataTransferObjects.EpProjectInsulationDefaultRow;
using LineList.Cenovus.Com.API.DataTransferObjects.EpProjectRole;
using LineList.Cenovus.Com.API.DataTransferObjects.EpProjectUserRole;
using LineList.Cenovus.Com.API.DataTransferObjects.Facility;
using LineList.Cenovus.Com.API.DataTransferObjects.Fluid;
using LineList.Cenovus.Com.API.DataTransferObjects.FluidPhase;
using LineList.Cenovus.Com.API.DataTransferObjects.Import;
using LineList.Cenovus.Com.API.DataTransferObjects.ImportCommodity;
using LineList.Cenovus.Com.API.DataTransferObjects.ImportLocation;
using LineList.Cenovus.Com.API.DataTransferObjects.ImportRow;
using LineList.Cenovus.Com.API.DataTransferObjects.ImportRowException;
using LineList.Cenovus.Com.API.DataTransferObjects.ImportSheet;
using LineList.Cenovus.Com.API.DataTransferObjects.ImportSheetColumn;
using LineList.Cenovus.Com.API.DataTransferObjects.InsulationDefault;
using LineList.Cenovus.Com.API.DataTransferObjects.InsulationDefaultColumn;
using LineList.Cenovus.Com.API.DataTransferObjects.InsulationDefaultDetail;
using LineList.Cenovus.Com.API.DataTransferObjects.InsulationDefaultRow;
using LineList.Cenovus.Com.API.DataTransferObjects.InsulationMaterial;
using LineList.Cenovus.Com.API.DataTransferObjects.InsulationThickness;
using LineList.Cenovus.Com.API.DataTransferObjects.InsulationType;
using LineList.Cenovus.Com.API.DataTransferObjects.InternalCoatingLiner;
using LineList.Cenovus.Com.API.DataTransferObjects.Line;
using LineList.Cenovus.Com.API.DataTransferObjects.LineListRevision;
using LineList.Cenovus.Com.API.DataTransferObjects.LineListStatus;
using LineList.Cenovus.Com.API.DataTransferObjects.LineListStatusState;
using LineList.Cenovus.Com.API.DataTransferObjects.LineRevision;
using LineList.Cenovus.Com.API.DataTransferObjects.LineStatus;
using LineList.Cenovus.Com.API.DataTransferObjects.Location;
using LineList.Cenovus.Com.API.DataTransferObjects.LocationType;
using LineList.Cenovus.Com.API.DataTransferObjects.NdeCategory;
using LineList.Cenovus.Com.API.DataTransferObjects.NotesConfiguration;
using LineList.Cenovus.Com.API.DataTransferObjects.OperatingMode;
using LineList.Cenovus.Com.API.DataTransferObjects.PaintSystem;
using LineList.Cenovus.Com.API.DataTransferObjects.PipeSpecification;
using LineList.Cenovus.Com.API.DataTransferObjects.PostWeldHeatTreatment;
using LineList.Cenovus.Com.API.DataTransferObjects.PressureProtection;
using LineList.Cenovus.Com.API.DataTransferObjects.ProjectType;
using LineList.Cenovus.Com.API.DataTransferObjects.ReferenceLine;
using LineList.Cenovus.Com.API.DataTransferObjects.Role;
using LineList.Cenovus.Com.API.DataTransferObjects.RoleUser;
using LineList.Cenovus.Com.API.DataTransferObjects.Schedule;
using LineList.Cenovus.Com.API.DataTransferObjects.ScheduleDefault;
using LineList.Cenovus.Com.API.DataTransferObjects.SegmentType;
using LineList.Cenovus.Com.API.DataTransferObjects.SizeNps;
using LineList.Cenovus.Com.API.DataTransferObjects.Specification;
using LineList.Cenovus.Com.API.DataTransferObjects.StressAnalysis;
using LineList.Cenovus.Com.API.DataTransferObjects.TestMedium;
using LineList.Cenovus.Com.API.DataTransferObjects.TestPressure;
using LineList.Cenovus.Com.API.DataTransferObjects.TracingDesignNumberOfTracers;
using LineList.Cenovus.Com.API.DataTransferObjects.TracingType;
using LineList.Cenovus.Com.API.DataTransferObjects.UserPreference;
using LineList.Cenovus.Com.API.DataTransferObjects.Validation;
using LineList.Cenovus.Com.API.DataTransferObjects.ValidationField;
using LineList.Cenovus.Com.API.DataTransferObjects.ValidationRule;
using LineList.Cenovus.Com.API.DataTransferObjects.WelcomeMessage;
using LineList.Cenovus.Com.API.DataTransferObjects.Xray;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.UI.Configuration
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<Area, AreaResultDto>().ReverseMap();
            CreateMap<Area, AreaAddDto>().ReverseMap();
            CreateMap<Area, AreaEditDto>().ReverseMap();

            CreateMap<CenovusProject, CenovusProjectResultDto>().ReverseMap();
            CreateMap<CenovusProject, CenovusProjectAddDto>().ReverseMap();
            CreateMap<CenovusProject, CenovusProjectEditDto>().ReverseMap();

            CreateMap<Code, CodeResultDto>().ReverseMap();
            CreateMap<Code, CodeAddDto>().ReverseMap();
            CreateMap<Code, CodeEditDto>().ReverseMap();

            CreateMap<Commodity, CommodityResultDto>().ReverseMap();
            CreateMap<Commodity, CommodityAddDto>().ReverseMap();
            CreateMap<Commodity, CommodityEditDto>().ReverseMap();

            CreateMap<ConcurrentEngineeringLine, ConcurrentEngineeringLineResultDto>().ReverseMap();
            //CreateMap<ConcurrentEngineeringLine, ConcurrentEngineeringLineAddDto>().ReverseMap();
            //CreateMap<ConcurrentEngineeringLine, ConcurrentEngineeringLineEditDto>().ReverseMap();

            CreateMap<CorrosionAllowance, CorrosionAllowanceResultDto>().ReverseMap();
            CreateMap<CorrosionAllowance, CorrosionAllowanceAddDto>().ReverseMap();
            CreateMap<CorrosionAllowance, CorrosionAllowanceEditDto>().ReverseMap();

            CreateMap<CsaClassLocation, CsaClassLocationResultDto>().ReverseMap();
            CreateMap<CsaClassLocation, CsaClassLocationAddDto>().ReverseMap();
            CreateMap<CsaClassLocation, CsaClassLocationEditDto>().ReverseMap();

            CreateMap<CsaHvpLvp, CsaHvpLvpResultDto>().ReverseMap();
            CreateMap<CsaHvpLvp, CsaHvpLvpAddDto>().ReverseMap();
            CreateMap<CsaHvpLvp, CsaHvpLvpEditDto>().ReverseMap();

            //CreateMap<Entity, EntityResultDto>().ReverseMap();
            //CreateMap<Entity, EntityAddDto>().ReverseMap();
            //CreateMap<Entity, EntityEditDto>().ReverseMap();

            CreateMap<EpCompany, EpCompanyResultDto>()
    .ForMember(dest => dest.Logo, opt => opt.MapFrom(src => src.Logo != null ? Convert.ToBase64String(src.Logo) : null))
    .ReverseMap()
    .ForMember(dest => dest.Logo, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Logo) ? Convert.FromBase64String(src.Logo) : null));

            CreateMap<EpCompany, EpCompanyAddDto>().ReverseMap();
            CreateMap<EpCompany, EpCompanyEditDto>().ReverseMap();

            CreateMap<EpCompanyAlpha, EpCompanyAlphaResultDto>().ReverseMap();
            CreateMap<EpCompanyAlpha, EpCompanyAlphaAddDto>().ReverseMap();
            CreateMap<EpCompanyAlpha, EpCompanyAlphaEditDto>().ReverseMap();

            CreateMap<EpProject, EpProjectResultDto>().ReverseMap();
            CreateMap<EpProject, EpProjectAddDto>().ReverseMap();
            CreateMap<EpProject, EpProjectEditDto>().ReverseMap();

            CreateMap<EpProjectInsulationDefault, EpProjectInsulationDefaultResultDto>().ReverseMap();
            CreateMap<EpProjectInsulationDefault, EpProjectInsulationDefaultAddDto>().ReverseMap();
            CreateMap<EpProjectInsulationDefault, EpProjectInsulationDefaultEditDto>().ReverseMap();

            CreateMap<EpProjectInsulationDefaultColumn, EpProjectInsulationDefaultColumnResultDto>().ReverseMap();
            CreateMap<EpProjectInsulationDefaultColumn, EpProjectInsulationDefaultColumnAddDto>().ReverseMap();
            CreateMap<EpProjectInsulationDefaultColumn, EpProjectInsulationDefaultColumnEditDto>().ReverseMap();

            CreateMap<EpProjectInsulationDefaultDetail, EpProjectInsulationDefaultDetailResultDto>().ReverseMap();
            CreateMap<EpProjectInsulationDefaultDetail, EpProjectInsulationDefaultDetailAddDto>().ReverseMap();
            CreateMap<EpProjectInsulationDefaultDetail, EpProjectInsulationDefaultDetailEditDto>().ReverseMap();

            CreateMap<EpProjectInsulationDefaultRow, EpProjectInsulationDefaultRowResultDto>().ReverseMap();
            CreateMap<EpProjectInsulationDefaultRow, EpProjectInsulationDefaultRowAddDto>().ReverseMap();
            CreateMap<EpProjectInsulationDefaultRow, EpProjectInsulationDefaultRowEditDto>().ReverseMap();

            CreateMap<EpProjectRole, EpProjectRoleResultDto>().ReverseMap();
            CreateMap<EpProjectRole, EpProjectRoleAddDto>().ReverseMap();
            CreateMap<EpProjectRole, EpProjectRoleEditDto>().ReverseMap();

            CreateMap<EpProjectUserRole, EpProjectUserRoleResultDto>().ReverseMap();
            CreateMap<EpProjectUserRole, EpProjectUserRoleAddDto>().ReverseMap();
            CreateMap<EpProjectUserRole, EpProjectUserRoleEditDto>()
                .ForMember(dest => dest.EpCompanyName, opt => opt.MapFrom(src => src.EpProject.EpCompany.Name)).ReverseMap();
           

            CreateMap<Facility, FacilityResultDto>().ReverseMap();
            CreateMap<Facility, FacilityAddDto>().ReverseMap();
            CreateMap<Facility, FacilityEditDto>().ReverseMap();

            CreateMap<Fluid, FluidResultDto>().ReverseMap();
            CreateMap<Fluid, FluidAddDto>().ReverseMap();
            CreateMap<Fluid, FluidEditDto>().ReverseMap();

            CreateMap<FluidPhase, FluidPhaseResultDto>().ReverseMap();
            CreateMap<FluidPhase, FluidPhaseAddDto>().ReverseMap();
            CreateMap<FluidPhase, FluidPhaseEditDto>().ReverseMap();

            CreateMap<Import, ImportResultDto>().ReverseMap();
            CreateMap<Import, ImportAddDto>().ReverseMap();
            CreateMap<Import, ImportEditDto>().ReverseMap();

            CreateMap<ImportCommodity, ImportCommodityResultDto>().ReverseMap();
            CreateMap<ImportCommodity, ImportCommodityAddDto>().ReverseMap();
            CreateMap<ImportCommodity, ImportCommodityEditDto>().ReverseMap();

            CreateMap<ImportLocation, ImportLocationResultDto>().ReverseMap();
            CreateMap<ImportLocation, ImportLocationAddDto>().ReverseMap();
            CreateMap<ImportLocation, ImportLocationEditDto>().ReverseMap();

            CreateMap<ImportRow, ImportRowResultDto>().ReverseMap();
            CreateMap<ImportRow, ImportRowAddDto>().ReverseMap();
            CreateMap<ImportRow, ImportRowEditDto>().ReverseMap();

            CreateMap<ImportRowException, ImportRowExceptionResultDto>().ReverseMap();
            CreateMap<ImportRowException, ImportRowExceptionAddDto>().ReverseMap();
            CreateMap<ImportRowException, ImportRowExceptionEditDto>().ReverseMap();

            //CreateMap<ImportRowView, ImportRowViewResultDto>().ReverseMap();
            //CreateMap<ImportRowView, ImportRowViewAddDto>().ReverseMap();
            //CreateMap<ImportRowView, ImportRowViewEditDto>().ReverseMap();

            CreateMap<ImportSheet, ImportSheetResultDto>().ReverseMap();
            CreateMap<ImportSheet, ImportSheetAddDto>().ReverseMap();
            CreateMap<ImportSheet, ImportSheetEditDto>().ReverseMap();

            CreateMap<ImportSheetColumn, ImportSheetColumnResultDto>().ReverseMap();
            CreateMap<ImportSheetColumn, ImportSheetColumnAddDto>().ReverseMap();
            CreateMap<ImportSheetColumn, ImportSheetColumnEditDto>().ReverseMap();

            CreateMap<InsulationDefault, InsulationDefaultResultDto>().ReverseMap();
            CreateMap<InsulationDefault, InsulationDefaultAddDto>().ReverseMap();
            CreateMap<InsulationDefault, InsulationDefaultEditDto>().ReverseMap();

            CreateMap<InsulationDefaultColumn, InsulationDefaultColumnResultDto>().ReverseMap();
            CreateMap<InsulationDefaultColumn, InsulationDefaultColumnAddDto>().ReverseMap();
            CreateMap<InsulationDefaultColumn, InsulationDefaultColumnEditDto>().ReverseMap();

            CreateMap<InsulationDefaultDetail, InsulationDefaultDetailResultDto>().ReverseMap();
            CreateMap<InsulationDefaultDetail, InsulationDefaultDetailAddDto>().ReverseMap();
            CreateMap<InsulationDefaultDetail, InsulationDefaultDetailEditDto>().ReverseMap();

            CreateMap<InsulationDefaultRow, InsulationDefaultRowResultDto>().ReverseMap();
            CreateMap<InsulationDefaultRow, InsulationDefaultRowAddDto>().ReverseMap();
            CreateMap<InsulationDefaultRow, InsulationDefaultRowEditDto>().ReverseMap();

            CreateMap<InsulationMaterial, InsulationMaterialResultDto>().ReverseMap();
            CreateMap<InsulationMaterial, InsulationMaterialAddDto>().ReverseMap();
            CreateMap<InsulationMaterial, InsulationMaterialEditDto>().ReverseMap();

            CreateMap<InsulationThickness, InsulationThicknessResultDto>().ReverseMap();
            CreateMap<InsulationThickness, InsulationThicknessAddDto>().ReverseMap();
            CreateMap<InsulationThickness, InsulationThicknessEditDto>().ReverseMap();

            CreateMap<InsulationType, InsulationTypeResultDto>().ReverseMap();
            CreateMap<InsulationType, InsulationTypeAddDto>().ReverseMap();
            CreateMap<InsulationType, InsulationTypeEditDto>().ReverseMap();

            CreateMap<InternalCoatingLiner, InternalCoatingLinerResultDto>().ReverseMap();
            CreateMap<InternalCoatingLiner, InternalCoatingLinerAddDto>().ReverseMap();
            CreateMap<InternalCoatingLiner, InternalCoatingLinerEditDto>().ReverseMap();

            CreateMap<Line, LineResultDto>().ReverseMap();
            CreateMap<Line, LineAddDto>().ReverseMap();
            CreateMap<Line, LineEditDto>().ReverseMap();

            //CreateMap<LineDesignationTableViewHeader, LineDesignationTableViewHeaderResultDto>().ReverseMap();
            //CreateMap<LineDesignationTableViewHeader, LineDesignationTableViewHeaderAddDto>().ReverseMap();
            //CreateMap<LineDesignationTableViewHeader, LineDesignationTableViewHeaderEditDto>().ReverseMap();

            //CreateMap<LineDesignationTableViewRevision, LineDesignationTableViewRevisionResultDto>().ReverseMap();
            //CreateMap<LineDesignationTableViewRevision, LineDesignationTableViewRevisionAddDto>().ReverseMap();
            //CreateMap<LineDesignationTableViewRevision, LineDesignationTableViewRevisionEditDto>().ReverseMap();

            //CreateMap<LineListModel, LineListModelResultDto>().ReverseMap();
            //CreateMap<LineListModel, LineListModelAddDto>().ReverseMap();
            //CreateMap<LineListModel, LineListModelEditDto>().ReverseMap();

            CreateMap<LineListRevision, LineListRevisionResultDto>().ReverseMap();
            CreateMap<LineListRevision, LineListRevisionAddDto>().ReverseMap();
            CreateMap<LineListRevision, LineListRevisionEditDto>().ReverseMap();

            CreateMap<LineListStatus, LineListStatusResultDto>().ReverseMap();
            CreateMap<LineListStatus, LineListStatusAddDto>().ReverseMap();
            CreateMap<LineListStatus, LineListStatusEditDto>().ReverseMap();

            CreateMap<LineListStatusState, LineListStatusStateResultDto>().ReverseMap();
            CreateMap<LineListStatusState, LineListStatusStateAddDto>().ReverseMap();
            CreateMap<LineListStatusState, LineListStatusStateEditDto>().ReverseMap();

            CreateMap<LineRevision, LineRevisionResultDto>().ReverseMap();
            CreateMap<LineRevision, LineRevisionAddDto>().ReverseMap();
            CreateMap<LineRevision, LineRevisionEditDto>().ReverseMap();

            //CreateMap<LineRevisionOperatingMode, LineRevisionOperatingModeResultDto>().ReverseMap();
            //CreateMap<LineRevisionOperatingMode, LineRevisionOperatingModeAddDto>().ReverseMap();
            //CreateMap<LineRevisionOperatingMode, LineRevisionOperatingModeEditDto>().ReverseMap();

            //CreateMap<LineRevisionSegment, LineRevisionSegmentResultDto>().ReverseMap();
            //CreateMap<LineRevisionSegment, LineRevisionSegmentAddDto>().ReverseMap();
            //CreateMap<LineRevisionSegment, LineRevisionSegmentEditDto>().ReverseMap();

            CreateMap<LineStatus, LineStatusResultDto>().ReverseMap();
            CreateMap<LineStatus, LineStatusAddDto>().ReverseMap();
            CreateMap<LineStatus, LineStatusEditDto>().ReverseMap();

            //CreateMap<LLLookupTable, LLLookupTableResultDto>().ReverseMap();
            //CreateMap<LLLookupTable, LLLookupTableAddDto>().ReverseMap();
            //CreateMap<LLLookupTable, LLLookupTableEditDto>().ReverseMap();

            //CreateMap<LLLookupTableNoDescription, LLLookupTableNoDescriptionResultDto>().ReverseMap();
            //CreateMap<LLLookupTableNoDescription, LLLookupTableNoDescriptionAddDto>().ReverseMap();
            //CreateMap<LLLookupTableNoDescription, LLLookupTableNoDescriptionEditDto>().ReverseMap();

            //CreateMap<LLTable, LLTableResultDto>().ReverseMap();
            //CreateMap<LLTable, LLTableAddDto>().ReverseMap();
            //CreateMap<LLTable, LLTableEditDto>().ReverseMap();

            CreateMap<Location, LocationResultDto>().ReverseMap();
            CreateMap<Location, LocationAddDto>().ReverseMap();
            CreateMap<Location, LocationEditDto>().ReverseMap();

            CreateMap<LocationType, LocationTypeResultDto>().ReverseMap();
            CreateMap<LocationType, LocationTypeAddDto>().ReverseMap();
            CreateMap<LocationType, LocationTypeEditDto>().ReverseMap();

            //CreateMap<Modular, ModularResultDto>().ReverseMap();
            //CreateMap<Modular, ModularAddDto>().ReverseMap();
            //CreateMap<Modular, ModularEditDto>().ReverseMap();

            CreateMap<NdeCategory, NdeCategoryResultDto>().ReverseMap();
            CreateMap<NdeCategory, NdeCategoryAddDto>().ReverseMap();
            CreateMap<NdeCategory, NdeCategoryEditDto>().ReverseMap();

            CreateMap<OperatingMode, OperatingModeResultDto>().ReverseMap();
            CreateMap<OperatingMode, OperatingModeAddDto>().ReverseMap();
            CreateMap<OperatingMode, OperatingModeEditDto>().ReverseMap();

            CreateMap<PaintSystem, PaintSystemResultDto>().ReverseMap();
            CreateMap<PaintSystem, PaintSystemAddDto>().ReverseMap();
            CreateMap<PaintSystem, PaintSystemEditDto>().ReverseMap();

            CreateMap<PipeSpecification, PipeSpecificationResultDto>().ReverseMap();
            CreateMap<PipeSpecification, PipeSpecificationAddDto>().ReverseMap();
            CreateMap<PipeSpecification, PipeSpecificationEditDto>().ReverseMap();

            CreateMap<PostWeldHeatTreatment, PostWeldHeatTreatmentResultDto>().ReverseMap();
            CreateMap<PostWeldHeatTreatment, PostWeldHeatTreatmentAddDto>().ReverseMap();
            CreateMap<PostWeldHeatTreatment, PostWeldHeatTreatmentEditDto>().ReverseMap();

            CreateMap<PressureProtection, PressureProtectionResultDto>().ReverseMap();
            CreateMap<PressureProtection, PressureProtectionAddDto>().ReverseMap();
            CreateMap<PressureProtection, PressureProtectionEditDto>().ReverseMap();

            CreateMap<ProjectType, ProjectTypeResultDto>().ReverseMap();
            CreateMap<ProjectType, ProjectTypeAddDto>().ReverseMap();
            CreateMap<ProjectType, ProjectTypeEditDto>().ReverseMap();

            CreateMap<ReferenceLine, ReferenceLineResultDto>().ReverseMap();
            CreateMap<ReferenceLine, ReferenceLineAddDto>().ReverseMap();
            CreateMap<ReferenceLine, ReferenceLineEditDto>().ReverseMap();

            CreateMap<Role, RoleResultDto>().ReverseMap();
            CreateMap<Role, RoleAddDto>().ReverseMap();
            CreateMap<Role, RoleEditDto>().ReverseMap();

            CreateMap<RoleUser, RoleUserResultDto>().ReverseMap();
            CreateMap<RoleUser, RoleUserAddDto>().ReverseMap();
            CreateMap<RoleUser, RoleUserEditDto>().ReverseMap();

            //CreateMap<RuleOperatorEnum, RuleOperatorEnumResultDto>().ReverseMap();
            //CreateMap<RuleOperatorEnum, RuleOperatorEnumAddDto>().ReverseMap();
            //CreateMap<RuleOperatorEnum, RuleOperatorEnumEditDto>().ReverseMap();

            //CreateMap<RuleTypeEnum, RuleTypeEnumResultDto>().ReverseMap();
            //CreateMap<RuleTypeEnum, RuleTypeEnumAddDto>().ReverseMap();
            //CreateMap<RuleTypeEnum, RuleTypeEnumEditDto>().ReverseMap();

            CreateMap<Schedule, ScheduleResultDto>().ReverseMap();
            CreateMap<Schedule, ScheduleAddDto>().ReverseMap();
            CreateMap<Schedule, ScheduleEditDto>().ReverseMap();

            CreateMap<ScheduleDefault, ScheduleDefaultResultDto>()
                .ForMember(dest => dest.SpecificationName, opt => opt.MapFrom(src => src.PipeSpecification.Specification.Name));
            CreateMap<ScheduleDefault, ScheduleDefaultAddDto>().ReverseMap();
            CreateMap<ScheduleDefault, ScheduleDefaultEditDto>().ReverseMap();

            CreateMap<SegmentType, SegmentTypeResultDto>().ReverseMap();
            CreateMap<SegmentType, SegmentTypeAddDto>().ReverseMap();
            CreateMap<SegmentType, SegmentTypeEditDto>().ReverseMap();

            CreateMap<SizeNps, SizeNpsResultDto>().ReverseMap();
            CreateMap<SizeNps, SizeNpsAddDto>().ReverseMap();
            CreateMap<SizeNps, SizeNpsEditDto>().ReverseMap();

            CreateMap<Specification, SpecificationResultDto>().ReverseMap();
            CreateMap<Specification, SpecificationAddDto>().ReverseMap();
            CreateMap<Specification, SpecificationEditDto>().ReverseMap();

            CreateMap<StressAnalysis, StressAnalysisResultDto>().ReverseMap();
            CreateMap<StressAnalysis, StressAnalysisAddDto>().ReverseMap();
            CreateMap<StressAnalysis, StressAnalysisEditDto>().ReverseMap();

            CreateMap<TestMedium, TestMediumResultDto>().ReverseMap();
            CreateMap<TestMedium, TestMediumAddDto>().ReverseMap();
            CreateMap<TestMedium, TestMediumEditDto>().ReverseMap();

            CreateMap<TestPressure, TestPressureResultDto>().ReverseMap();
            CreateMap<TestPressure, TestPressureAddDto>().ReverseMap();
            CreateMap<TestPressure, TestPressureEditDto>().ReverseMap();

            CreateMap<TracingDesignNumberOfTracers, TracingDesignNumberOfTracersResultDto>().ReverseMap();
            CreateMap<TracingDesignNumberOfTracers, TracingDesignNumberOfTracersAddDto>().ReverseMap();
            CreateMap<TracingDesignNumberOfTracers, TracingDesignNumberOfTracersEditDto>().ReverseMap();

            CreateMap<TracingType, TracingTypeResultDto>().ReverseMap();
            CreateMap<TracingType, TracingTypeAddDto>().ReverseMap();
            CreateMap<TracingType, TracingTypeEditDto>().ReverseMap();

            CreateMap<UserPreference, UserPreferenceResultDto>().ReverseMap();
            CreateMap<UserPreference, UserPreferenceAddDto>().ReverseMap();
            CreateMap<UserPreference, UserPreferenceEditDto>().ReverseMap();

            CreateMap<Validation, ValidationResultDto>().ReverseMap();
            CreateMap<Validation, ValidationAddDto>().ReverseMap();
            CreateMap<Validation, ValidationEditDto>().ReverseMap();

            CreateMap<ValidationField, ValidationFieldResultDto>().ReverseMap();
            CreateMap<ValidationField, ValidationFieldAddDto>().ReverseMap();
            CreateMap<ValidationField, ValidationFieldEditDto>().ReverseMap();

            //CreateMap<ValidationFieldTypeEnum, ValidationFieldTypeEnumResultDto>().ReverseMap();
            //CreateMap<ValidationFieldTypeEnum, ValidationFieldTypeEnumAddDto>().ReverseMap();
            //CreateMap<ValidationFieldTypeEnum, ValidationFieldTypeEnumEditDto>().ReverseMap();

            CreateMap<ValidationRule, ValidationRuleResultDto>().ReverseMap();
            CreateMap<ValidationRule, ValidationRuleAddDto>().ReverseMap();
            CreateMap<ValidationRule, ValidationRuleEditDto>().ReverseMap();

            //CreateMap<ValidationTypeEnum, ValidationTypeEnumResultDto>().ReverseMap();
            //CreateMap<ValidationTypeEnum, ValidationTypeEnumAddDto>().ReverseMap();
            //CreateMap<ValidationTypeEnum, ValidationTypeEnumEditDto>().ReverseMap();

            CreateMap<Xray, XrayResultDto>().ReverseMap();
            CreateMap<Xray, XrayAddDto>().ReverseMap();
            CreateMap<Xray, XrayEditDto>().ReverseMap();

            CreateMap<WelcomeMessage, WelcomeMessageResultDto>().ReverseMap();
            CreateMap<WelcomeMessage, WelcomeMessageAddDto>().ReverseMap();
            CreateMap<WelcomeMessage, WelcomeMessageEditDto>().ReverseMap();

            CreateMap<NotesConfiguration, NotesConfigurationResultDto>().ReverseMap();
            CreateMap<NotesConfiguration, NotesConfigurationAddDto>().ReverseMap();
            CreateMap<NotesConfiguration, NotesConfigurationEditDto>().ReverseMap();

            CreateMap<WelcomeMessage, SearchLineListViewModel>().ReverseMap();

            CreateMap<SearchLineListViewModel, LineListModel>().ReverseMap();
            //.ForMember(dest => dest.DocumentNumber, opt => opt.MapFrom(src => src.DocumentNumber))
            //.ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))


            CreateMap<SearchLineListViewModel, LineListRevision>().ReverseMap();
                //.ForMember(dest => dest.DocumentRevision, opt => opt.MapFrom(src => src.DocumentRevision ?? "0"))
                //.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                //.ForMember(dest => dest.LineListStatusId, opt => opt.MapFrom(src => src.SelectedLineListStatusId))
                //.ForMember(dest => dest.EpCompanyId, opt => opt.MapFrom(src => src.SelectedEPId ))
                //.ForMember(dest => dest.SpecificationId, opt => opt.MapFrom(src => src.SelectedSpecificationId))
                //.ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.SelectedLocationId))
                //.ForMember(dest => dest.AreaId, opt => opt.MapFrom(src => src.SelectedAreaId))
                //.ForMember(dest => dest.EpProjectId, opt => opt.MapFrom(src => src.SelectedEPProjectId))
                //.ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                //.ForMember(dest => dest.PreparedBy, opt => opt.MapFrom(src => src.PreparedBy))
                //.ForMember(dest => dest.ReviewedBy, opt => opt.MapFrom(src => src.ReviewedBy))
                //.ForMember(dest => dest.ApprovedByLead, opt => opt.MapFrom(src => src.ApprovedByLead))
                //.ForMember(dest => dest.ApprovedByProject, opt =>  opt.MapFrom(src => src.PreparedByProcess))
                //.ForMember(dest => dest.PreparedByMechanical, opt => opt.MapFrom(src => src.PreparedByMechanical))
                //.ForMember(dest => dest.ReviewByProcess, opt => opt.MapFrom(src => src.ReviewByProcess))
                //.ForMember(dest => dest.ReviewedByMechanical, opt => opt.MapFrom(src => src.ReviewedByMechanical))
                ////.ForMember(dest => dest.ApprovedByLeadDiscEngineerComplex, opt => opt.MapFrom(src => src.ApprovedByLeadDiscEngineerComplex))
                ////.ForMember(dest => dest.ApprovedByProjectEngineerComplex, opt => opt.MapFrom(src => src.ApprovedByProjectEngineerComplex))
                //.ReverseMap();
        }

        public static IMapper Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutomapperConfig>();
            });

            return config.CreateMapper();
        }

        private byte[] ReadStream(Stream stream)
        {
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}