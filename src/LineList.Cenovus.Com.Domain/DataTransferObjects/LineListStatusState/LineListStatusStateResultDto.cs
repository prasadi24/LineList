namespace LineList.Cenovus.Com.API.DataTransferObjects.LineListStatusState
{
    public class LineListStatusStateResultDto
    {
        public Guid Id { get; set; }

        public bool IsForUpRev { get; set; }

        public Guid CurrentStatusId { get; set; }

        public Guid FutureStatusId { get; set; }

        public Guid? RequiredIssuedStatus1Id { get; set; }

        public Guid? RequiredIssuedStatus2Id { get; set; }

        public Guid? RequiredIssuedStatus3Id { get; set; }

        public Guid? ExcludeIssuedStatus1Id { get; set; }

        public Guid? ExcludeIssuedStatus2Id { get; set; }

        public Guid? ExcludeIssuedStatus3Id { get; set; }

        public Guid? ExcludeIssuedStatus4Id { get; set; }
    }
}