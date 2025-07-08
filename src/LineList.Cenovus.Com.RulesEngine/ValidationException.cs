using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.RulesEngine
{
    [Serializable]
    public class ValidationException
    {
        public Validation Validation { get; set; }
        public FlatLine Line { get; set; }
        public FlatLineList LineList { get; set; }

        public ValidationException()
        { }

        public ValidationException(Validation validation, FlatLine line, FlatLineList lineList)
        {
            this.Validation = validation;
            this.Line = line;
            this.LineList = lineList;
        }
    }
}