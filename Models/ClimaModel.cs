// Models/ClimaModel.cs
namespace Clima.Models
{
    public class ClimaModel
    {
        public string Name { get; set; }
        public MainInfo Main { get; set; } = new MainInfo();
        public WeatherInfo[] Weather { get; set; } = new WeatherInfo[1];

        public class MainInfo
        {
            public double Temp { get; set; }
        }

        public class WeatherInfo
        {
            public string Description { get; set; }
        }
    }
}
