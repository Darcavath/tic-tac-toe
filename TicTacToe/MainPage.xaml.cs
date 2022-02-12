using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using Xamarin.Forms;

namespace TicTacToe {
	public class Score {
		public DateTime date { get; set; }
		public int data { get; set; }
	}

	public enum Difficulty {
		normal,
		hard,
		expert
	};

	public partial class MainPage : ContentPage {
		Game game = new Game();
		//TitleScreen titleScreen = new TitleScreen();
		
		public bool playerTurn = false;
		public string headerMessage = "";
		public string footerMessage = "";
		public bool gameActive = false;
		public bool loop = true;
		public Difficulty difficulty = Difficulty.normal;
		public string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HighScores.json");
		public Random rnd = new Random();
		public List<List<int>> winPatterns = new List<List<int>>() {
			new List<int> { 0, 1, 2 },
			new List<int> { 3, 4, 5 },
			new List<int> { 6, 7, 8 },
			new List<int> { 0, 3, 6 },
			new List<int> { 1, 4, 7 },
			new List<int> { 2, 5, 8 },
			new List<int> { 0, 4, 8 },
			new List<int> { 2, 4, 6 }
		};
		//public StackLayout stackLayout = new StackLayout();

		List<Score> highScore = new List<Score>() {
			//new Score { date = DateTime.Today, data = 125 },
			//new Score { date = DateTime.Today, data = 78 }
		};
		

		public MainPage() {
			// Initialize system.
			InitializeComponent();

			// Initialize.
			Initialize();

			// Initialize the title screen.
			//stackLayout = game.titleScreen.InitializeTitle(ButtonClick, (int)difficulty);
			//InitializeTitle();
			Content = game.titleScreen.InitializeTitle(ButtonClick, (int)difficulty);
		}

		//--------------------------------------------------------------------------------
		// General initialization.
		//--------------------------------------------------------------------------------
		public void Initialize() {
			string jsonString = File.ReadAllText(fileName);
			highScore = JsonSerializer.Deserialize<List<Score>>(jsonString);
			//highScore = JsonSerializer.Deserialize<Score>(jsonString)!;
		}

		//--------------------------------------------------------------------------------
		// Initialize the title screen.
		//--------------------------------------------------------------------------------
		//public void InitializeTitle() {
			
		//}

		////--------------------------------------------------------------------------------
		//// Initialize and setup the gameboard.
		////--------------------------------------------------------------------------------
		//public void InitializeBoard() {
			
		//}

		////--------------------------------------------------------------------------------
		//// Initialize high scores.
		////--------------------------------------------------------------------------------
		//public void InitializeHighScores() {
		//	// Test data.
			
		//	//Console.WriteLine($"Path = {Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HighScores.json")}");
		//	//string fileName = "HighScores.json";
		//	//highScore = JsonSerializer.Deserialize<Score>(jsonString)!;
			
		//	//Console.WriteLine(File.ReadAllText(fileName));

		//}

		//--------------------------------------------------------------------------------
		// Handle title screen clicks here and player control.
		//--------------------------------------------------------------------------------
		//public void TitleButtonClick(object sender, EventArgs e) {
		//	Button clickedBut = sender as Button;

		//			}

		//--------------------------------------------------------------------------------
		// Handle clicks here. Player control.
		//--------------------------------------------------------------------------------
		public void ButtonClick(object sender, EventArgs e) {
			Button clickedBut = sender as Button;
			int elementNumber = -1;

			// Handle titlescreen clicks.
			if (clickedBut.Equals(game.titleScreen.playButton)) {
				gameActive = true;
				Content = game.gameBoard.InitializeBoard(ButtonClick);

				// Select random first player.
				int result = rnd.Next(0, 2);
				Console.WriteLine($"*** result = {result} ***");
				if (result == 0) {
					// Enable all blank buttons so the player can move.
					for (int i = 0; i < 9; i++) {
						if (game.gameBoard.positionButton[i].IsEnabled == false) {
							game.gameBoard.positionButton[i].IsEnabled = true;
						}
					}
					playerTurn = true;
				} else {
					playerTurn = false;
					CPUTurn("O");
				}
			}

			if (clickedBut.Equals(game.titleScreen.quitButton)) {
				CloseGame();
			}

			// 
			if (clickedBut.Equals(game.titleScreen.diffButton)) {
				switch (difficulty) {
					case 0:
						difficulty = Difficulty.hard;
						game.titleScreen.diffLabel.Text = "Hard";
						break;
					case (Difficulty)1:
						difficulty = Difficulty.expert;
						game.titleScreen.diffLabel.Text = "Expert";
						break;
					case (Difficulty)2:
						difficulty = Difficulty.normal;
						game.titleScreen.diffLabel.Text = "Normal";
						break;
					default:
						break;
				}
			}

			if (clickedBut.Equals(game.gameBoard.returnButton)) {
				//game.gameBoard.ResetGameBoard();
				Content = game.titleScreen.InitializeTitle(ButtonClick, (int)difficulty);
			}
			
			// Handle gameboard clicks.
			// Iterate through button array for matching element.
			for (int i = 0; i < 9; i++) {
				if (clickedBut.Equals(game.gameBoard.positionButton[i])) {
					elementNumber = i;

					// Set button's matching label.
					game.gameBoard.positionLabel[elementNumber].Text = "X";
					clickedBut.IsEnabled = false;
					playerTurn = false;
				}
			}

			

			// Disable all blank buttons until player can move again.
			for (int i = 0; i < 9; i++) {
				if (game.gameBoard.positionButton[i].IsEnabled == true) {
					game.gameBoard.positionButton[i].IsEnabled = false;
				}
			}

			// Check for player win.
			if (CheckForWinner("X")) {
				gameActive = false;
				Game.wins += 1;
				game.gameBoard.headerLabel[0].Text = $"Player: {Game.wins}";
				game.gameBoard.footerLabel.Text = "You win! Click for menu.";
				game.gameBoard.returnButton.IsEnabled = true;
				//await Task.Delay(2000);
			}

			// Check for draw game.
			if (gameActive == true && CheckForDraw() == true) {
				gameActive = false;
				// Code here
			}

			// Control gameflow if active.
			if (gameActive == true) {
				// Set control for CPU.
				playerTurn = false;

				// Initialize CPU move.
				CPUTurn("O");
			} else {
				// Code
			}

			// Handle highscores return.
			if (clickedBut.Equals(game.titleScreen.scoreButton)) {
				game.highScore.InitializeHighScoreScreen(highScore, ButtonClick);
			}
		}

		//--------------------------------------------------------------------------------
		// Handle gameboard clicks here and player control.
		//--------------------------------------------------------------------------------
		public void HSButtonClick(object sender, EventArgs e) {
			Button clickedBut = sender as Button;

			
		}

		//--------------------------------------------------------------------------------
		// Perform CPU turn.
		//--------------------------------------------------------------------------------
		public async void CPUTurn(string symbol) {
			bool moved = false;
			string opponentSymbol;

			// Set opponent symbol.
			if (symbol == "X") {
				opponentSymbol = "O";
			} else {
				opponentSymbol = "X";
			}

			// Display delay for CPU turn.
			game.gameBoard.footerLabel.Text = "Please wait...";
			await Task.Delay(2000);

			// Move determined by difficulty.
			int diffNumber = 0;
			if (difficulty == 0) {
				diffNumber = 5;
			} else if (difficulty == (Difficulty)1) {
				diffNumber = 8;
			} else if (difficulty == (Difficulty)2) {
				diffNumber = 10;
			}

			// Get random number for CPU move.
			int chance = rnd.Next(1, 11);

			if (chance <= diffNumber) {
				// Win if possible.
				if (playerTurn == false) {
					foreach (List<int> element in winPatterns) {
						int winningPosition = -1;
						int ownElement = 0;

						foreach (int elementInt in element) {
							// Count own elements.
							if (game.gameBoard.positionLabel[elementInt].Text == symbol) {
								ownElement++;
							}
							// Select potential move.
							if (game.gameBoard.positionLabel[elementInt].Text == "") {
								winningPosition = elementInt;
							}
						}
						// Make move and return.
						if (ownElement == 2 && winningPosition != -1) {
							game.gameBoard.positionLabel[winningPosition].Text = symbol;
							game.gameBoard.positionButton[winningPosition].IsEnabled = false;
							moved = true;
							break;
						}
					}

					// Block if no winning move.
					if (moved == false) {
						foreach (List<int> element in winPatterns) {
							int blockPosition = -1;
							int opponentElement = 0;

							foreach (int elementInt in element) {
								// Count opponent elements.
								if (game.gameBoard.positionLabel[elementInt].Text == opponentSymbol) {
									opponentElement++;
								}
								// Select potential move.
								if (game.gameBoard.positionLabel[elementInt].Text == "") {
									blockPosition = elementInt;
								}
							}
							// Make move and return.
							if (opponentElement == 2 && blockPosition != -1) {
								game.gameBoard.positionLabel[blockPosition].Text = symbol;
								game.gameBoard.positionButton[blockPosition].IsEnabled = false;
								moved = true;
								break;
							}
						}
					}

					// Take middle position if available.
					if (moved == false) {
						if (game.gameBoard.positionLabel[4].Text == "") {
							game.gameBoard.positionLabel[4].Text = symbol;
							game.gameBoard.positionButton[4].IsEnabled = false;
							moved = true;
						}
					}
				}

				// Make a random move.
				if (moved == false && playerTurn == false) {
					int rndPosition;
					do {
						rndPosition = rnd.Next(0, 9);
					} while (game.gameBoard.positionLabel[rndPosition].Text != "");

					game.gameBoard.positionLabel[rndPosition].Text = symbol;
					game.gameBoard.positionButton[rndPosition].IsEnabled = false;
					moved = true;
					playerTurn = true;
				}
			} else {
				// Make a random move.
				if (moved == false && playerTurn == false) {
					int rndPosition;
					do {
						rndPosition = rnd.Next(0, 9);
					} while (game.gameBoard.positionLabel[rndPosition].Text != "");

					game.gameBoard.positionLabel[rndPosition].Text = symbol;
					game.gameBoard.positionButton[rndPosition].IsEnabled = false;
					moved = true;
					playerTurn = true;
				}
			}

			// Check for CPU win.
			if (CheckForWinner("O") == true) {
				gameActive = false;
				Game.losses++;
				game.gameBoard.headerLabel[2].Text = $"CPU: {Game.losses}";
				game.gameBoard.returnButton.IsEnabled = true;
			}

			// Check for draw.
			if (gameActive == true && CheckForDraw() == true) {
				gameActive = false;
			}

			// Enable all blank buttons so the player can move again.
			for (int i = 0; i < 9; i++) {
				if (game.gameBoard.positionLabel[i].Text == "") {
					game.gameBoard.positionButton[i].IsEnabled = true;
				}
			}

			if (gameActive == true) {
				game.gameBoard.footerLabel.Text = "Please make your move...";
				playerTurn = true;
			}
		}

		//--------------------------------------------------------------------------------
		// Check for winner.
		//--------------------------------------------------------------------------------
		public bool CheckForWinner(string symbol) {
			// Iterate through patterns and identify the winning one.
			foreach (List<int> element in winPatterns) {
				int ownElement = 0;

				foreach (int elementInt in element) {
					// Count own elements.
					if (game.gameBoard.positionLabel[elementInt].Text == symbol) {
						ownElement++;
					}
				}
				// Make move and return.
				if (ownElement == 3) {
					if (symbol == "X") {
						game.gameBoard.footerLabel.Text = "You Win! Click for menu.";
					} else {
						game.gameBoard.footerLabel.Text = "Sorry, you lose! Click for menu.";
					}
					// Disable all blank buttons.
					for (int i = 0; i < 9; i++) {
						if (game.gameBoard.positionLabel[i].Text == "") {
							game.gameBoard.positionButton[i].IsEnabled = false;
						}
					}
					return true;
				}
			}
			// No winner.
			return false;
		}

		//--------------------------------------------------------------------------------
		// Check for draw game.
		//--------------------------------------------------------------------------------
		public bool CheckForDraw() {
			int count = 0;

			// Iterate through all labels and count open moves.
			for (int i = 0; i < 9; i++) {
				if (game.gameBoard.positionLabel[i].Text == "") {
					count++;
				}
			}

			if (count == 0) {
				Game.draws++;
				game.gameBoard.headerLabel[1].Text = $"Draws: {Game.draws}";
				game.gameBoard.footerLabel.Text = "It's a draw! Click for menu.";
				game.gameBoard.returnButton.IsEnabled = true;
				return true;
			} else {
				return false;
			}
		}

		//--------------------------------------------------------------------------------
		// Close the game. ???
		//--------------------------------------------------------------------------------
		public void CloseGame() {
			DateTime currentDateTime = DateTime.Now;
			highScore.Add(new Score { date = DateTime.Today, data = Game.wins });
			string jsonString = JsonSerializer.Serialize(highScore);
			File.WriteAllText(fileName, jsonString);

			// ** Fix this! **
			Console.WriteLine("** Exit complete! **");
			System.Environment.Exit(0);
		}
	}
}
