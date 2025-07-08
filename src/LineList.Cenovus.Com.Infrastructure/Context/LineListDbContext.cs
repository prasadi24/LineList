using LineList.Cenovus.Com.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LineList.Cenovus.Com.Infrastructure.Context
{
    public class LineListDbContext : DbContext
    {
        public LineListDbContext()
        {
        }

        public LineListDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<NotesConfiguration> NotesConfigurations { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<CenovusProject> CenovusProjects { get; set; }
        public DbSet<Code> Codes { get; set; }
        public DbSet<Commodity> Commodities { get; set; }
        public DbSet<CorrosionAllowance> CorrosionAllowances { get; set; }
        public DbSet<CsaClassLocation> CsaClassLocations { get; set; }
        public DbSet<CsaHvpLvp> CsaHvpLvps { get; set; }
        public DbSet<EpCompany> EpCompanies { get; set; }
        public DbSet<EpCompanyAlpha> EpCompanyAlphas { get; set; }
        public DbSet<EpProject> EpProjects { get; set; }
        public DbSet<EpProjectInsulationDefault> EpProjectInsulationDefaults { get; set; }
        public DbSet<EpProjectInsulationDefaultRow> EpProjectInsulationDefaultRows { get; set; }
        public DbSet<EpProjectInsulationDefaultColumn> EpProjectInsulationDefaultColumns { get; set; }
        public DbSet<EpProjectInsulationDefaultDetail> EpProjectInsulationDefaultDetails { get; set; }
        public DbSet<EpProjectRole> EpProjectRoles { get; set; }
        public DbSet<EpProjectUserRole> EpProjectUserRoles { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Fluid> Fluids { get; set; }
        public DbSet<FluidPhase> FluidPhases { get; set; }
        public DbSet<InsulationDefault> InsulationDefaults { get; set; }
        public DbSet<InsulationDefaultDetail> InsulationDefaultDetails { get; set; }
        public DbSet<InsulationDefaultRow> InsulationDefaultRows { get; set; }
        public DbSet<InsulationDefaultColumn> InsulationDefaultColumns { get; set; }
        public DbSet<InsulationThickness> InsulationThicknesses { get; set; }
        public DbSet<InsulationMaterial> InsulationMaterials { get; set; }
        public DbSet<InsulationType> InsulationTypes { get; set; }
        public DbSet<InternalCoatingLiner> InternalCoatingLiners { get; set; }
        public DbSet<Line> Lines { get; set; }
        public DbSet<LineListModel> LineLists { get; set; }
        public DbSet<LineListRevision> LineListRevisions { get; set; }
        public DbSet<LineListStatus> LineListStatuses { get; set; }
        public DbSet<LineListStatusState> LineListStatusStates { get; set; }
        public DbSet<LineRevision> LineRevisions { get; set; }
        public DbSet<LineRevisionOperatingMode> LineRevisionOperatingModes { get; set; }
        public DbSet<LineRevisionSegment> LineRevisionSegments { get; set; }
        public DbSet<LineStatus> LineStatuses { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationType> LocationTypes { get; set; }
        public DbSet<NdeCategory> NdeCategories { get; set; }
        public DbSet<OperatingMode> OperatingModes { get; set; }
        public DbSet<PaintSystem> PaintSystems { get; set; }
        public DbSet<PipeSpecification> PipeSpecifications { get; set; }
        public DbSet<PressureProtection> PressureProtections { get; set; }
        public DbSet<ProjectType> ProjectTypes { get; set; }
        public DbSet<ReferenceLine> ReferenceLines { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleUser> RoleUsers { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<ScheduleDefault> ScheduleDefaults { get; set; }
        public DbSet<SegmentType> SegmentTypes { get; set; }
        public DbSet<SizeNps> SizeNps_s { get; set; }
        public DbSet<Specification> Specifications { get; set; }
        public DbSet<StressAnalysis> StressAnalysises { get; set; }
        public DbSet<PostWeldHeatTreatment> PostWeldHeatTreatments { get; set; }
        public DbSet<TestMedium> TestMedia { get; set; }
        public DbSet<TestPressure> TestPressure { get; set; }
        public DbSet<TracingDesignNumberOfTracers> TracingDesignNumberOfTracers { get; set; }
        public DbSet<TracingType> TracingTypes { get; set; }
        public DbSet<UserPreference> UserPreferences { get; set; }
        public DbSet<Xray> Xrays { get; set; }

        public DbSet<Import> Imports { get; set; }
        public DbSet<ImportSheet> ImportSheets { get; set; }
        public DbSet<ImportSheetColumn> ImportSheetColumns { get; set; }

        public DbSet<ImportRow> ImportRows { get; set; }
        public DbSet<ImportFacility> ImportFacilities { get; set; }

        public DbSet<ImportLocation> ImportLocations { get; set; }
        public DbSet<ImportCommodity> ImportCommodities { get; set; }
        public DbSet<ImportRowException> ImportRowExceptions { get; set; }

        public DbSet<Validation> Validations { get; set; }
        public DbSet<ValidationRule> ValidationRules { get; set; }
        public DbSet<ValidationField> ValidationFields { get; set; }

        public DbSet<WelcomeMessage> WelcomeMessages { get; set; }
        public DbSet<ConcurrentEngineeringLine> ConcurrentEngineeringLines { get; set; }

        public DbSet<LineDesignationTableViewHeader> LineDesignationTableViewHeaders { get; set; }

        public DbSet<LineDesignationTableViewRevision> LineDesignationTableViewRevisions { get; set; }

        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Server=tcp:CGYWSQLDV0037.cenovus.com,60593;Database=LineList;User ID=LINELIST;Password=^4Nl>F7l2X!d6^£C;TrustServerCertificate=True;",
                    sqlOptions =>
                    {
                        sqlOptions.CommandTimeout(240);
                        sqlOptions.EnableRetryOnFailure();
                    });
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
            .Entity<ConcurrentEngineeringLine>()
            .ToView("ConcurrentEngineeringLine")
            .HasKey(l => l.Id);

            modelBuilder
           .Entity<LineDesignationTableViewHeader>()
           .ToView("LineDesignationTableViewHeader")
           .HasKey(l => l.Id);

            modelBuilder
          .Entity<LineDesignationTableViewRevision>()
          .ToView("LineDesignationTableViewRevisions")
          .HasKey(l => l.LineListRevisionId);

            

            modelBuilder.Entity<UserPreference>()
              .Ignore(up => up.Id)  // Ignore the inherited Id property
              .HasKey(up => up.UserName);  // Set UserName as the primary key

            modelBuilder.Entity<ImportSheet>()
                .HasMany(s => s.ImportRows)
                .WithOne(r => r.ImportSheet)
                .HasForeignKey(r => r.ImportSheetId);

            modelBuilder.Entity<ImportRow>()
                .HasMany(r => r.Exceptions)
                .WithOne(e => e.ImportRow)
                .HasForeignKey(e => e.ImportRowId);


            modelBuilder.Entity<LineRevision>()
                .ToTable(tb => tb.HasTrigger("LineRevision_UpdateInsert"));

            modelBuilder.Entity<LineListRevision>()
               .ToTable(tb => tb.HasTrigger("LineListRevision_UpdateInsert"));

            base.OnModelCreating(modelBuilder);
            
        }
    }
}