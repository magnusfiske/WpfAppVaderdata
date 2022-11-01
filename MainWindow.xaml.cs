using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAppVäderdata
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		
		public MainWindow()
		{
			InitializeComponent();
			WeatherList = new List<WeatherData>();
			DisplayContent();

		}
		public List<WeatherData> WeatherList { get; set; }
		public string FileName { get; set; }
		private async void btnCreateWeatherData_Click(object sender, RoutedEventArgs e)
		{
			DateTime? date = calWeather.SelectedDate;
			string wind = comboWind.Text;
			string cmbSky = comboSky.Text;
			string degreesCelsius = txtBoxDegreeC.Text;
			string humidity = txtBoxHumid.Text;
			
			try
			{
				WeatherData newWeather = new WeatherData(date, cmbSky, wind, int.Parse(degreesCelsius), int.Parse(humidity));
				WeatherList.Add(newWeather);
				await WritingWeatherDataToFile();
			}
			catch (FormatException ex)
			{
				MessageBox.Show("Endast siffror, försök igen", "Inmatningsfel!", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Felaktig inmatning, försök igen\n{ex}", "Inmatningsfel!", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			DisplayContent();
		}

		private void DisplayContent()
		{
			//listBoxWeather.Items.Clear();
			
			//weatherList.ForEach(x => x.ToString());
			listBoxWeather.ItemsSource = null;
			listBoxWeather.ItemsSource = WeatherList;
		}

		public async Task WritingWeatherDataToFile()
		{
			if (!File.Exists(FileName))
			{
				Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
				dlg.FileName = "WeatherForecast";
				dlg.DefaultExt = ".json";
				dlg.Filter = "weather file (.json)|*.json";
				Nullable<bool> result = dlg.ShowDialog();
				if (result == true)
				{
					string filename = dlg.FileName;
					FileName = filename;
					using FileStream createStream = File.Create(filename);
					await JsonSerializer.SerializeAsync(createStream, WeatherList);
					await createStream.DisposeAsync();
				}
			}
			else
			{
				string filename = FileName;
				using FileStream createStream = File.Create(filename);
				await JsonSerializer.SerializeAsync(createStream, WeatherList);
				await createStream.DisposeAsync();
			}
		}		

		public async Task WriteWeatherDataToListBox()
		{
			if (!File.Exists(FileName))
			{
				Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
				dlg.FileName = "WeatherForecast";
				dlg.DefaultExt = ".json";
				dlg.Filter = "weather file (.json)|*.json";
				Nullable<bool> result = dlg.ShowDialog();
				if (result == true)
				{
					string filename = dlg.FileName;
					FileName = filename;
					using FileStream openStream = File.OpenRead(filename);
					List<WeatherData>? getWeatherList =
					await JsonSerializer.DeserializeAsync<List<WeatherData>>(openStream);
					WeatherList.Clear();
					WeatherList.AddRange(getWeatherList);
				}
				else
				{
					string filename = FileName;
					using FileStream openStream = File.OpenRead(filename);
					List<WeatherData>? getWeatherList =
					await JsonSerializer.DeserializeAsync<List<WeatherData>>(openStream);
					WeatherList.Clear();
					WeatherList.AddRange(getWeatherList);
				}
			}
			DisplayContent();
			
		}

		private void EnableButton(object sender, RoutedEventArgs e)
		{
			if (calWeather.SelectedDate == null || txtBoxDegreeC == null || comboWind.SelectedItem == null || comboSky.SelectedItem == null || txtBoxHumid == null)
				return;
			else
			{
				btnCreateWeatherData.IsEnabled = true;
			}
		}

		private void txtBoxHumid_GotFocus(object sender, RoutedEventArgs e)
		{
			txtBoxHumid.Clear();
		}

		private void txtBoxDegreeC_GotFocus(object sender, RoutedEventArgs e)
		{
			txtBoxDegreeC.Clear();
		}

		private async void btnLoadWeatherData_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				await WriteWeatherDataToListBox();

            }
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
	}
}
