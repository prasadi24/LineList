namespace LineList.Cenovus.Com.Services
{
    public class FlatLineList
    {
        public Guid Id { get; set; }
        public string Revision { get; set; }
        public string DocumentNumber { get; set; }
        public string Status { get; set; }
        public string EpProject { get; set; }
        public string Specification { get; set; }
        public string DateIssued { get; set; }
        public string Description { get; set; }
    }

    public class FlatLine
    {
        public Guid Id { get; set; }
        public string AltOpMode { get; set; }
        public string ParentChild { get; set; }
        public string AreaID { get; set; }
        public string Spec { get; set; }
        public string LocationID { get; set; }
        public string CommCode { get; set; }
        public string ClassServMat { get; set; }
        public string SizeNPS { get; set; }
        public string LineNo { get; set; }
        public string InsulThk { get; set; }
        public string InsulType { get; set; }
        public string TraceDesignType { get; set; }
        public string TraceDesignHoldTemp { get; set; }
        public string TracingDesignNumTracers { get; set; }
        public string InsMat { get; set; }
        public string LineFrom { get; set; }
        public string LineTo { get; set; }
        public string OrigPIDNo { get; set; }
        public string Sched { get; set; }
        public string Thk { get; set; }
        public string FluidPhase { get; set; }
        public string OpPress { get; set; }
        public string OpTemp { get; set; }
        public string DesignPress { get; set; }
        public string DesignMaxTemp { get; set; }
        public string DesignMinTemp { get; set; }
        public string TestPress { get; set; }
        public string TestMed { get; set; }
        public string ExpTemp { get; set; }
        public string UpsetPress { get; set; }
        public string UpsetTemp { get; set; }
        public string MDMTTemp { get; set; }
        public string CorrAllow { get; set; }
        public string Xray { get; set; }
        public string NDECat { get; set; }
        public string PWHT { get; set; }
        public string StressRel { get; set; }
        public string PaintSys { get; set; }
        public string IntCoatLiner { get; set; }
        public string Code { get; set; }
        public string ABSARegistration { get; set; }
        public string PressureProtection { get; set; }
        public string AsBuilt { get; set; }
        public string Fluid { get; set; }
        public string CsaClassLocation { get; set; }
        public string CSALVPHVP { get; set; }
        public string PipeMaterialSpecifications { get; set; }
        public string HoopStressLevel { get; set; }
        public string SourService { get; set; }
        public string Notes { get; set; }
        public string LineRev { get; set; }
        public string DocNo { get; set; }
        public string Status { get; set; }
    }
}