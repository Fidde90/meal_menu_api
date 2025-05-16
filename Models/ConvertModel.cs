namespace meal_menu_api.Models
{
    public class ConvertModel
    {
        public string Unit { get; set; }

        public double Volume { get; set; }

        public ConvertModel(string unit, double volume)
        {
            Unit = unit;
            Volume = volume;
        }
    }
}
