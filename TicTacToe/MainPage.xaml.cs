using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using Xamarin.Forms;
using System.Linq;

namespace TicTacToe {
	public partial class MainPage : ContentPage {
		public Game game;
		
		public string headerMessage = "";
		public string footerMessage = "";
		
		public string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HighScores.json");
		public string errorLogFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Errors.log");

		public MainPage() {
			// Initialize system.
			InitializeComponent();

			// Initialize.
			Initialize();
		}

		//--------------------------------------------------------------------------------
		// General initialization.
		//--------------------------------------------------------------------------------
		public void Initialize() {
			game = new Game(this);
			
			// Create or open error log.
			if (!File.Exists(errorLogFile)) {
				string message = $"{DateTime.Now.ToShortDateString()}: error log file created.\n";
				File.WriteAllText(errorLogFile, message);
			}

			// Create or load high scores file.
			if (File.Exists(fileName)) {
				string jsonString = File.ReadAllText(fileName);
				game.highScoreList = JsonSerializer.Deserialize<List<Score>>(jsonString);
			} else {
				// Create highscores file.
				string message = $"{DateTime.Now.ToShortDateString()}: highscores file {fileName} created.\n";
				File.WriteAllText(errorLogFile, message);
			}

			diffButton.Text = "Normal";

			// Set number of players.
			if (game.numPlayers == 1) {
				vsButton.Text = "vs CPU";
			} else if (game.numPlayers == 2) {
				vsButton.Text = "vs Player2";
			}
		}

		//--------------------------------------------------------------------------------
		// Handle Play button.
		//--------------------------------------------------------------------------------
		public void PlayButtonClick(object sender, EventArgs e) {
			if ((sender as Button).Equals(playButton)) {
				game.gameActive = true;
				Application.Current.MainPage = game.gameBoard;

				// Reset gameboard.
				foreach (Button button in game.gameBoard.buttons) {
					button.Text = "";
					button.InputTransparent = false;
				}

				// Set initial header values.
				game.gameBoard.headerLeft.Text = $"Player1: {Game.wins}";
				game.gameBoard.headerMiddle.Text = $"Draws: {Game.draws}";
				if (game.numPlayers == 1) {
					game.gameBoard.headerRight.Text = $"CPU: {Game.losses}";
				} else {
					game.gameBoard.headerRight.Text = $"Player2: {Game.losses}";
				}

				// Select random first player.
				int result = game.rnd.Next(0, 2);
				if (result == 0) {
					game.gameBoard.footerButton.Text = "Player1, make your move...";
					game.playerTurn = true;
					return;
				} else {
					game.playerTurn = false;
					if (game.numPlayers == 2) {
						game.gameBoard.footerButton.Text = "Player2, make your move...";
					} else {
						game.gameBoard.CPUTurnAsync("O");
					}
					return;
				}


			}
		}

		//--------------------------------------------------------------------------------
		// Handle Difficulty button.
		//--------------------------------------------------------------------------------
		public void DifficultyButtonClick(object sender, EventArgs e) {
			if (game.numPlayers == 1) {
				// Change game difficulty.
				if ((sender as Button).Equals(diffButton)) {
					switch (game.difficulty) {
						case 0:
							game.difficulty = Difficulty.hard;
							diffButton.Text = "Hard";
							break;
						case (Difficulty)1:
							game.difficulty = Difficulty.expert;
							diffButton.Text = "Expert";
							break;
						case (Difficulty)2:
							game.difficulty = Difficulty.normal;
							diffButton.Text = "Normal";
							break;
						default:
							break;
					}
				}
			}
		}

		//--------------------------------------------------------------------------------
		// Handle vs button.
		//--------------------------------------------------------------------------------
		public void VSButtonClick(object sender, EventArgs e) {
			if ((sender as Button).Equals(vsButton)) {
				if (game.numPlayers == 1) {
					game.numPlayers = 2;
					vsButton.FontSize = 38;
					vsButton.Text = "vs Player2";
					diffButton.Text = "";
				} else if (game.numPlayers == 2) {
					game.numPlayers = 1;
					vsButton.FontSize = 48;
					vsButton.Text = "vs CPU";
					switch (game.difficulty) {
						case 0:
							diffButton.Text = "Normal";
							break;
						case (Difficulty)1:
							diffButton.Text = "Hard";
							break;
						case (Difficulty)2:
							diffButton.Text = "Expert";
							break;
						default:
							break;
					}
				}
			}
		}

		//--------------------------------------------------------------------------------
		// Handle High Score button.
		//--------------------------------------------------------------------------------
		public void HSButtonClick(object sender, EventArgs e) {
			if ((sender as Button).Equals(hsButton)) {
				Application.Current.MainPage = game.highScore;
				game.highScore.LoadScores();
			}
		}
		//--------------------------------------------------------------------------------
		// Handle Quit button.
		//--------------------------------------------------------------------------------
		public void QuitButtonClick(object sender, EventArgs e) {
			if ((sender as Button).Equals(quitButton)) {
				CloseGame();
			}
		}

		//--------------------------------------------------------------------------------
		// Close the game.
		//--------------------------------------------------------------------------------
		public void CloseGame() {
			// Write high scores file on quit.
			string currentDateTime = DateTime.Now.ToShortDateString();
			game.highScoreList.Add(new Score { date = currentDateTime, data = Game.wins });
			// Sort highScoreList.
			List<Score> objSortedList = game.highScoreList.OrderByDescending(o => o.data).ToList();

			string jsonString = JsonSerializer.Serialize(objSortedList);
			File.WriteAllText(fileName, jsonString);
			string errorMessage = "Highscores written to HighScore.json\n";
			File.WriteAllText(errorLogFile, errorMessage);

			// Handle quitting on different platforms.
			if (Device.RuntimePlatform == Device.iOS) {
				DisplayAlert("Notice", "Highscores written, please press home to quit.", "Okay");
			} else if (Device.RuntimePlatform == Device.macOS) {
				System.Diagnostics.Process.GetCurrentProcess().Kill();
			}
		}
	}
}
