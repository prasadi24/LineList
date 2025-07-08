namespace LineList.Cenovus.Com.RulesEngine
{
    [Serializable]
    public class FlatValidation
    {
        public Guid LineId;
        public string AltOpMode;
        public string FieldName;
        public string Message;
        public bool IsRequiredToIssueHardRevision;

        // SER798
        public string RownumberNew;

    }
}
