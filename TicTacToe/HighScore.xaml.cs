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
		}

		//--------------------------------------------------------------------------------
		// Load scores and sort to display.
		//--------------------------------------------------------------------------------
		public void LoadScores() {
			// Load scores from score list to display in scrollview.
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
		}

		//--------------------------------------------------------------------------------
		// Handle Return button.
		//--------------------------------------------------------------------------------
		public void ReturnButtonClick(object sender, EventArgs e) {
			if ((sender as Button).Equals(returnButton)) {
				Application.Current.MainPage = game.mainPage;
			}
		}
	}
}
