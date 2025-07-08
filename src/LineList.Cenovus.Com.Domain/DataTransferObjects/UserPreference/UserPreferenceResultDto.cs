namespace LineList.Cenovus.Com.API.DataTransferObjects.UserPreference
{
    public class UserPreferenceResultDto
    {
        public string UserName { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public DateTime ModifiedOn { get; set; }
    }
}