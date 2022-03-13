using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Xamarin.Forms;

namespace TicTacToe {
	public partial class HighScore : ContentPage {
		public Game game;
		
		public HighScore(Game game) {
			InitializeComponent();

			this.game = game;

			// ** Todo: Handle different platforms. **
			string tempString = "";
			List<Score> objSortedList = game.highScoreList.OrderByDescending(o => o.data).ToList();
			string jsonString = JsonSerializer.Serialize(objSortedList);
			JsonDocument doc = JsonDocument.Parse(jsonString);
			JsonElement root = doc.RootElement;
			var entries = root.EnumerateArray();
			int count = 0;
			while (entries.MoveNext()) {
				var entry = entries.Current;
				var props = entry.EnumerateObject();
				while (props.MoveNext()) {
					var prop = props.Current;
					if (prop.Name == "date") {
						tempString += $"{prop.Value} \t- ";
					} else {
						tempString += $"\t{prop.Value}\n";
						count++;
					}
				}
			}
			tempString += $"{count} scores recorded.";
			contentLabel.Text = tempString;
			//			Label contentLabel = new Label {
			//				//Text = jsonString,
			//				Text = tempString,
			//				FontSize = 32
			//			};

			Console.WriteLine("*** Should have read scores! ***");
		}

		public void ReturnButtonClick(object sender, EventArgs e) {
			// Handle highscores. --------------------------------------------------------
			if ((sender as Button).Equals(returnButton)) {
				//Content = game.mainPage.Content;

				Application.Current.MainPage = game.mainPage;// new GameBoard(game);
			}
			//if ((sender as Button).Equals(returnButton)) {
			//	Content = ;
			//}
		}
	}
}
