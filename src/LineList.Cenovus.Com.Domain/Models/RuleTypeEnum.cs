namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    public enum RuleTypeEnum
    {
        Ignore = 0,
        MustBe = 1,
        Lookup = 2,
        TypeCheck = 3,
        MinLength = 4,
        MaxLength = 5,
        Match = 6,
        NotExist = 7,
        FormatCheck = 8,
        Status = 9,
        SegmentCount = 10,
        MatchParent = 11,
        MatchLineList = 12
    }
}