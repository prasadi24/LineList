using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
	[Serializable]
	public class LLLookupTable : LLLookupTableNoDescription
	{
		public string? Description { get; set; }

		[NotMapped]
		public string Name_dash_Description
		{
			get
			{
				return FormatNameDescription(Name, Description);
			}
		}

		public static string FormatNameDescription(string name, string description)
		{
			if (name == description || string.IsNullOrWhiteSpace(description))
			{
				return name; // makes things look pretty, otherwise it looks like a duplicate
			}
			else if (string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(description))
			{
				return description;
			}
			else
			{
				return name + " - " + description;
			}
		}
	}
}