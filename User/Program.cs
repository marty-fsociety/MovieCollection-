using System;
using System.IO;
using System.Text.Json;
using System.Text.Unicode;
using System.Text.Encodings.Web;

namespace User {
	class SettingsMovie {
		public string Name { get; set; }
		public string Type { get; set; }
		public int Year { get; set; }
	}

	class MetodsMovie {
		public static List<SettingsMovie> Movies = new();  
		public static void CreateList() {
			try {
				using(FileStream stream = new("vc.json", FileMode.Open)) {
					List<SettingsMovie> ListMovie = JsonSerializer.Deserialize<List<SettingsMovie>>(stream);
					foreach(SettingsMovie movie in ListMovie) {
						Movies.Add(movie);
					}
				}
			}
			catch (System.IO.FileNotFoundException) {
				FileStream stream = new("vc.json", FileMode.Create);
			}
			catch (System.Text.Json.JsonException) {
			}
		}
		public static void AddMovie() {
			var newMovie = new SettingsMovie();
			while (true) {
				Console.Write("Что хочешь добавить? Фильм или сериал?(f/s) ");
				string userMovie = Console.ReadLine();
				if (userMovie == "s" || userMovie == "f") {
					if (userMovie == "s") {
						newMovie.Type = "Сериал";
					}
					else if(userMovie == "f") {
						newMovie.Type = "Фильм";
					}
					break;
				}
				else {
					Console.WriteLine("Программа не поняла вас");
					continue;
				}
			}

			Console.Write("Какое у него название? ");
			newMovie.Name = Console.ReadLine();

			
			while (true) {
				if(newMovie.Type == "s") {
					Console.Write("Какого года ваш сериал? ");
				}
				else if(newMovie.Type == "f") {
					Console.WriteLine("Какого года ваш фильм? ");
				}
				try {
					newMovie.Year = Convert.ToInt32(Console.ReadLine());
					break;
				}
				catch (System.FormatException) {
					Console.WriteLine("В году выпуска вашего фильма или сериала должны присутсвовать только числа");
					continue;
				}
			}

			Movies.Add(newMovie);

			var options = new JsonSerializerOptions {
				Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
				WriteIndented = true
			};

			using(var stream = new FileStream("vc.json", FileMode.OpenOrCreate)) {
				JsonSerializer.Serialize(stream, Movies, options);
			}
		}
		public static void FindMovie() {
			while (true) {
				Console.WriteLine("По какому критерию будешь искать фильм или сериал?: ");
				Console.WriteLine("'n' - по названию, ");
				Console.WriteLine("'y' - по году выпуска. ");
				string UserMovie = Console.ReadLine();
				if(UserMovie == "n") {
					Console.Write("Введи название фильма или сериала: ");
					string NameMovie = Console.ReadLine();
					try {
						using(FileStream stream = new("vc.json", FileMode.Open)) {
							var ListMovie = JsonSerializer.Deserialize<List<SettingsMovie>>(stream);
							Console.WriteLine($"Все фильмы и сериалы, которые имеют  название '{NameMovie}'");
							foreach(SettingsMovie movie in ListMovie) {
								if (movie.Name != NameMovie) {
									continue;
								}
								else {
									Console.WriteLine($"Название: '{movie.Name}'. Год: '{movie.Year}'");
								}
							}
						}
						break;
					}
					catch (System.IO.FileNotFoundException) {
						Console.WriteLine("Не удалось найти базу данных. Перезапустите программу");
						break;
					}
				}
				else if(UserMovie == "y") {
					while (true) {
						try {
							Console.Write("Введи год выпуска фильма или сериала: ");
							int YearMovie = Convert.ToInt32(Console.ReadLine());
							try {
								using(FileStream stream = new("vc.json", FileMode.Open)) {
									var ListMovie = JsonSerializer.Deserialize<List<SettingsMovie>>(stream);
									Console.WriteLine($"Все сериалы и фильмы, которые имеют {YearMovie} год выпуска: ");
									foreach(SettingsMovie movie in ListMovie) {
										if(movie.Year != YearMovie) {
											continue;
										}
										else {
											Console.WriteLine($"Название: '{movie.Name}'. Год: '{movie.Year}'");
										}	
									}
								}
							}
							catch (System.IO.FileNotFoundException) {
								Console.WriteLine("Не удалось найти базу данных. Перезапустите программу");
								break;
							}
							catch (System.Text.Json.JsonException) {
								Console.WriteLine("В базе данных нет фильмов и сериалов, но ты можешь их добавить по команде  'add'");	
								break;
							}
							break;
						}
						catch (System.FormatException) {
							Console.WriteLine("Программа не поняла тебя");
							continue;
						}
					}
					break;
				}
				else {
					Console.WriteLine("Программа не поняла тебя, попробуй еще раз");
					continue;
				}
			}
		}
		public static void ShowAllMovie() {
			try {
				using(FileStream stream = new("vc.json", FileMode.Open)) {
					var ListMovie = JsonSerializer.Deserialize<List<SettingsMovie>>(stream);
					Console.WriteLine("Все сериалы и фильмы, которые хранятся в базе данных: ");
					foreach(SettingsMovie movie in ListMovie) {
						Console.WriteLine($"Название {movie.Name}. Год: {movie.Year}");
					}
				}
			}
			catch (System.IO.FileNotFoundException) {
				Console.WriteLine("Не удалось найти базу данных. Перезапустите программу");
			}
		}
	}


	class Program {
		static void Main() {
			Console.WriteLine("----------ПОИСК-ФИЛЬМОВ-ИЛИ-СЕРИАЛОВ---------------");
			Console.WriteLine("'find' - найти сериал");
			Console.WriteLine("'add' - добавить сериал в базу данных");
			Console.WriteLine("'sa' -  показать все фильмы и сериалы, лежащие в базе данных");
			Console.WriteLine("'exit' - выйти");
			MetodsMovie.CreateList();
			while (true) {
				Console.Write("Что будешь делать? ");
				string ActionUser = Console.ReadLine();
				if(ActionUser == "find") {
					MetodsMovie.FindMovie();
					continue;
				}
				else if(ActionUser == "add") {
					MetodsMovie.AddMovie();	
					continue;
				}
				else if(ActionUser == "sa") {
					MetodsMovie.ShowAllMovie();
					continue;
				}
				else if(ActionUser == "exit") {
					Console.WriteLine("Ты вышел из программы");
					break;
				}
				else {
					Console.WriteLine("Программа не поняла тебя");
					continue;
				}
			}
		}
	}
}
	
