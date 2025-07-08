using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("ImportRow")]
    public class ImportRow : Entity
    {
        public int RowNumber { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsImported { get; set; }

        //Foreign Keys
        public virtual ImportSheet ImportSheet { get; set; }

        public virtual ICollection<ImportRowException> Exceptions { get; set; }
        public Guid ImportSheetId { get; set; }

        //Excel Fields
        public string excelABSARegistration { get; set; }

        public string excelAltOpMode { get; set; }
        public string excelAnalysis { get; set; }
        public string excelAreaID { get; set; }
        public string excelAsBuilt { get; set; }
        public string excelChecked { get; set; }
        public string excelClassServMat { get; set; }
        public string excelCode { get; set; }
        public string excelCommCode { get; set; }
        public string excelCommodity { get; set; }
        public string excelCorrAllow { get; set; }
        public string excelCsaClassLocation { get; set; }
        public string excelCSALVPHVP { get; set; }
        public string excelDateYMD { get; set; }
        public string excelDateIssued { get; set; }
        public string excelDeleted { get; set; }
        public string excelDescription { get; set; }
        public string excelDesignMaxTemp { get; set; }
        public string excelDesignMinTemp { get; set; }
        public string excelDesignPress { get; set; }
        public string excelDocNo { get; set; }
        public string excelDocumentNumber { get; set; }
        public string excelDocRev { get; set; }
        public string excelDuplicate { get; set; }
        public string excelEP { get; set; }
        public string excelEPProj { get; set; }
        public string excelExpTemp { get; set; }
        public string excelFileName { get; set; }
        public string excelFluid { get; set; }
        public string excelFluidPhase { get; set; }
        public string excelHoopStressLevel { get; set; }
        public string excelInsMat { get; set; }
        public string excelInsulThk { get; set; }
        public string excelInsulType { get; set; }
        public string excelIntCoatLiner { get; set; }
        public string excelLineFrom { get; set; }
        public string excelLineNo { get; set; }
        public string excelLineRev { get; set; }
        public string excelLineTo { get; set; }
        public string excelLocationID { get; set; }
        public string excelMDMTTemp { get; set; }
        public string excelNDECat { get; set; }
        public string excelNotes { get; set; }
        public string excelOpPress { get; set; }
        public string excelOpTemp { get; set; }
        public string excelOrigPIDNo { get; set; }
        public string excelPaintSys { get; set; }
        public string excelParentChild { get; set; }
        public string excelPipeMaterial { get; set; }
        public string excelPipeMaterialSpecifications { get; set; }
        public string excelPressureProtection { get; set; }
        public string excelProjNo { get; set; }
        public string excelPWHT { get; set; }
        public string excelReservedBy { get; set; }
        public string excelRev { get; set; }
        public string excelSched { get; set; }
        public string excelSizeNPS { get; set; }
        public string excelSourService { get; set; }
        public string excelSpec { get; set; }
        public string excelSpecification { get; set; }
        public string excelStatus { get; set; }
        public string excelStressRel { get; set; }
        public string excelTestMed { get; set; }
        public string excelTestPress { get; set; }
        public string excelThk { get; set; }
        public string excelTraceDesignType { get; set; }
        public string excelTraceDesignHoldTemp { get; set; }
        public string excelTracingDesignNumTracers { get; set; }
        public string excelUpsetPress { get; set; }
        public string excelUpsetTemp { get; set; }
        public string excelWallThk { get; set; }
        public string excelXray { get; set; }

        public string excelModularId { get; set; } //SER 296

        public string this[string propertyName]
        {
            get
            {
                var prop = typeof(ImportRow).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                return prop?.GetValue(this)?.ToString();
            }
            set
            {
                var prop = typeof(ImportRow).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                if (prop != null && prop.CanWrite)
                {
                    // Convert the string value to the property type if necessary
                    var convertedValue = Convert.ChangeType(value, prop.PropertyType);
                    prop.SetValue(this, convertedValue);
                }
            }
        }
    }
}