using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using Xamarin.Forms;
using System.Linq;
using System.Threading;

namespace TicTacToe {
	public partial class MainPage : ContentPage {
		public Game game;// = new Game();
		//public TitleScreen titleScreen;
		
		//public bool playerTurn = false;
		public string headerMessage = "";
		public string footerMessage = "";
		
		public string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HighScores.json");
		public string errorLogFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Errors.log");

		public MainPage() {
			// Initialize system.
			InitializeComponent();

			// Initialize.
			Initialize();

			//Content = game.titleScreen.Content;
		}

		//--------------------------------------------------------------------------------
		// General initialization.
		//--------------------------------------------------------------------------------
		public void Initialize() {
			game = new Game(this);
			//???
			// ***** MOVE THIS!!! *****
			// Create or open error log.
			if (!File.Exists(errorLogFile)) {
				//File.Create(errorLogFile);
				string message = $"{DateTime.Now.ToShortDateString()}: error log file el created.\n";
				File.WriteAllText(errorLogFile, message);
			}

			// Create or load high scores file.
			if (File.Exists(fileName)) {
				string jsonString = File.ReadAllText(fileName);
				game.highScoreList = JsonSerializer.Deserialize<List<Score>>(jsonString);
			} else {
				// Create highscores file.
				//File.Create(fileName);

				string message = $"{DateTime.Now.ToShortDateString()}: highscores file {fileName} created.\n";
				File.WriteAllText(errorLogFile, message);
			}
			// ************************


			diffButton.Text = "Normal";

			game.gameBoard.headerLeft.Text = $"Player: {Game.wins}";
			game.gameBoard.headerMiddle.Text = $"Draws: {Game.draws}";
			game.gameBoard.headerRight.Text = $"CPU: {Game.losses}";
			// Setup stacklayouts.
			//game.titleScreen = new TitleScreen(this);
			//game.highScore.InitializeHighScoreScreen(highScoreList, ButtonClick);
		}

		//--------------------------------------------------------------------------------
		// Handle Play button.
		//--------------------------------------------------------------------------------
		public void PlayButtonClick(object sender, EventArgs e) {
			if ((sender as Button).Equals(playButton)) {
				game.gameActive = true;
				Application.Current.MainPage = game.gameBoard;// new GameBoard(game);
				//DisplayAlert("Something", "Message", "OK");

				// Reset gameboard.
				foreach (Button button in game.gameBoard.buttons) {
					button.Text = "";
					button.InputTransparent = false;
				}

				// Select random first player.
				int result = game.rnd.Next(0, 2);
				// ** DEBUGGING **
				if (result == 0) {
					game.gameBoard.footerButton.Text = "Please make your move...";
					game.playerTurn = true;
					return;
				} else {
					game.playerTurn = false;
					game.gameBoard.CPUTurnAsync("O");
					return;
				}


			}

			// More here...
		}

		//--------------------------------------------------------------------------------
		// Handle Difficulty button.
		//--------------------------------------------------------------------------------
		public void DifficultyButtonClick(object sender, EventArgs e) {
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

		//--------------------------------------------------------------------------------
		// Handle CPU button.
		//--------------------------------------------------------------------------------
		public void CPUButtonClick(object sender, EventArgs e) {
			if ((sender as Button).Equals(cpuButton)) {
				// ?
			}
		}

		//--------------------------------------------------------------------------------
		// Handle High Score button.
		//--------------------------------------------------------------------------------
		public void HSButtonClick(object sender, EventArgs e) {
			if ((sender as Button).Equals(hsButton)) {
				Application.Current.MainPage = new HighScore(game);
				//Content = game.highScore.Content;
			}
		}
		//--------------------------------------------------------------------------------
		// Handle Difficulty button.
		//--------------------------------------------------------------------------------
		public void QuitButtonClick(object sender, EventArgs e) {
			if ((sender as Button).Equals(quitButton)) {
				CloseGame();
			}
		}

		//--------------------------------------------------------------------------------
		// Close the game. ???
		//--------------------------------------------------------------------------------
		public void CloseGame() {
			// Write high scores file on quit.
			string currentDateTime = DateTime.Now.ToShortDateString();
			game.highScoreList.Add(new Score { date = currentDateTime, data = Game.wins });
			// Sort ?
			List<Score> objSortedList = game.highScoreList.OrderByDescending(o => o.data).ToList();


			string jsonString = JsonSerializer.Serialize(objSortedList);
			File.WriteAllText(fileName, jsonString);
			string errorMessage = "Highscores written to HighScore.json\n";
			File.WriteAllText(errorLogFile, errorMessage);

			// ** Fix this! ** Only for Mac, no exit on iOS!
			Console.WriteLine("** Exit complete! **");
			System.Diagnostics.Process.GetCurrentProcess().Kill();
		}

		void cpuButton_Clicked(System.Object sender, System.EventArgs e) {
		}

		public static implicit operator View(MainPage v) {
			throw new NotImplementedException();
		}
	}
}
