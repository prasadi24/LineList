namespace LineList.Cenovus.Com.UI.Models
{
    public class HomeModel
    {
        public bool ShouldPromptForUserSettings { get; set; }

        public HomeModel()
        {
            ShouldPromptForUserSettings = false;
        }
    }
}