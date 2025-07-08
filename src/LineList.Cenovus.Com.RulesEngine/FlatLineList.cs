namespace LineList.Cenovus.Com.RulesEngine
{
    [Serializable]
    public class FlatLineList
    {
        public FlatLineList() { }

        public Guid Id;
        public string DocumentNumber;
        public string DocumentRevision;
        public string Commodity;
        public string DateIssued;
        public string Description;
        public string EpCompany;
        public string EpProject;
        public string FileName;
        public string Specification;
        public string Status;
        public FlatSourceEnum Source;
    }
}
