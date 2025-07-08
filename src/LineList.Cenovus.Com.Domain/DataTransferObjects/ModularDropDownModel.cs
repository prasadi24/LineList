namespace LineList.Cenovus.Com.Domain.DataTransferObjects
{
    public class ModularDropDownModel
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public ModularDropDownModel(string name, string value)
        {
            Name = name;
            Value = value;
        }

    }
}
