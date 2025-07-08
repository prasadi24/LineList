using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.DataTransferObjects
{
	public class HomeViewModel
	{
		public WelcomeMessage WelcomeMessage { get; set; }
		public string TotalLineList { get; set; }
		public string AverageLines { get; set; }
		public string TotalUsers { get; set; }
		public string LastUpdated { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsCenovusAdmin { get; set; }
    }
}