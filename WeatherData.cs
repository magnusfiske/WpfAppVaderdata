using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppVäderdata
{
	public class WeatherData
	{
		public WeatherData(DateTime? date, string sky, string wind, int degrees, int humidity)
		{
			Date = date;
			Sky = sky;
			Wind = wind;
			Degrees = degrees;
			Humidity = humidity;
		}



		public DateTime? Date { get; set; }

		public string Sky { get; set; }



		public string Wind { get; set; }



		public int Degrees { get; set; }



		public int Humidity { get; set; }

		public override string ToString()
		{
			return $"Datum:{Date} Väderlek:{Sky} Vindstyrka:{Wind} Antal grader:{Degrees} Luftfuktighet:{Humidity}";
		}
	}
}
