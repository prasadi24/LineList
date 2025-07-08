using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("LineRevisionOperatingMode")]
    public class LineRevisionOperatingMode : LLTable
    {
        //public Guid? pressureProtectionId;

        [ForeignKey("LineRevision")]
        public Guid? LineRevisionId { get; set; }

        public virtual LineRevision LineRevision { get; set; }

        public string? IsAbsaRegistration { get; set; } //SER 435.  Changes by Armando Chaves.
        public Guid? CodeId { get; set; }
        public Guid? CsaClassLocationId { get; set; }
        public Guid? CsaHvpLvpId { get; set; }
        public Guid? FluidId { get; set; }
        public Guid? FluidPhaseId { get; set; }
        public Guid? OperatingModeId { get; set; }

        public decimal? HoopStressLevel { get; set; }
        public string? LineRoutingFrom { get; set; }
        public string? LineRoutingTo { get; set; }
        public string? Notes { get; set; }
        public string OperatingModeNumber { get; set; }

        public string? ModularId { get; set; }

        public string? OperatingPressurePipe { get; set; }
        public string? OperatingPressureAnnulus { get; set; }
        public string? OperatingTemperatureAnnulus { get; set; }
        public string? OperatingTemperaturePipe { get; set; }
        public string? PipeMaterialSpecification { get; set; }
        public bool? IsSourService { get; set; }

        public virtual Code Code { get; set; }
        public virtual CsaClassLocation CsaClassLocation { get; set; }
        public virtual CsaHvpLvp CsaHvpLvp { get; set; }
        public virtual Fluid Fluid { get; set; }
        public virtual FluidPhase FluidPhase { get; set; }
        public virtual OperatingMode OperatingMode { get; set; }

        public Guid? PressureProtectionId { get; set; } //SER 435.  Changes by Armando Chaves.
        public virtual PressureProtection PressureProtection { get; set; } //SER 435.  Changes by Armando Chaves.
        public bool? RequiresCsaInformation { get; set; }
    }
}