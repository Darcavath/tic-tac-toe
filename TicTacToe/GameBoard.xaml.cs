using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TicTacToe {
	public partial class GameBoard : ContentPage {
		//public MainPage mainPage;
		public Game game;
		public List<Button> buttons = new List<Button>();

		public GameBoard(Game game) {
			InitializeComponent();

			this.game = game;

			// Add XAML position button to list for iteration.
			buttons.Add(position0);
			buttons.Add(position1);
			buttons.Add(position2);
			buttons.Add(position3);
			buttons.Add(position4);
			buttons.Add(position5);
			buttons.Add(position6);
			buttons.Add(position7);
			buttons.Add(position8);
		}

		//--------------------------------------------------------------------------------
		// Handle clicks here. Player control.
		//--------------------------------------------------------------------------------
		public void ButtonClick(object sender, EventArgs e) {
			Button clickedButton = sender as Button;

			// returnButton only control, gameBoard only.
			if (clickedButton.Equals(game.gameBoard.footerButton) && game.gameActive == true) {
				return;
			}

			// Iterate through button array for matching element.
			if (game.playerTurn == true) {
				for (int i = 0; i < 9; i++) {
					if (clickedButton.Equals(buttons[i])) {
						// Handle clickable?
						if (clickedButton.InputTransparent == false) {
							buttons[i].Text = "X";
							buttons[i].InputTransparent = true;
							game.playerTurn = false;

							// Disable all blank buttons so player cannot move again.
							foreach (Button button in buttons) {
								if (button.InputTransparent.Equals(false)) {
									button.InputTransparent.Equals(true);
								}
							}
						} else {
							// Here? Just leave.
							return;
						}
					}
				}

				// Check for player win.
				if (CheckForWinner("X") == true) {
					game.gameActive = false;
					game.gameBoard.headerLeft.Text = $"{game.playerScore}";
					return;
				}

				// Check for draw game.
				if (game.gameActive == true && CheckForDraw() == true) {
					game.gameActive = false;
					game.playerTurn = false;
					return;
				}

				// Control gameflow if active.
				if (game.gameActive == true && game.numPlayers == 1) {
					if (game.playerTurn == false) {
						// Initialize CPU move.
						CPUTurnAsync("O");
					}
				}

				if (game.numPlayers == 2) {
					game.gameBoard.footerButton.Text = "Player2, make your move...";
				}
			}

			// Handle Player2 when active.
			if (game.playerTurn == false && game.numPlayers == 2) {
				for (int i = 0; i < 9; i++) {
					if (clickedButton.Equals(buttons[i])) {
						// Handle clickable?
						if (clickedButton.InputTransparent == false) {
							buttons[i].Text = "O";
							buttons[i].InputTransparent = true;
							game.playerTurn = true;

							// Disable all blank buttons so player cannot move again.
							foreach (Button button in buttons) {
								if (button.InputTransparent.Equals(false)) {
									button.InputTransparent.Equals(true);
								}
							}
						} else {
							// Here? Just leave.
							return;
						}
					}
				}

				// Check for player win.
				if (CheckForWinner("O") == true) {
					game.gameActive = false;
					game.gameBoard.headerRight.Text = $"Player2: {Game.losses}";
					return;
				}

				// Check for draw game.
				if (game.gameActive == true && CheckForDraw() == true) {
					game.gameActive = false;
					game.playerTurn = false;
					return;
				}

				if (game.numPlayers == 2) {
					game.gameBoard.footerButton.Text = "Player1, make your move...";
				}
			}

			// returnButton only control, gameBoard only.
			if (clickedButton.Equals(game.gameBoard.footerButton) && game.gameActive == false) {
				if (clickedButton.InputTransparent == false) {
					Application.Current.MainPage = game.mainPage;
				}
				return;
			}

			return;
		}

		//--------------------------------------------------------------------------------
		// Perform CPU turn.
		//--------------------------------------------------------------------------------
		public async void CPUTurnAsync(string symbol) {
			bool moved = false;
			string opponentSymbol;

			// Set opponent symbol.
			if (symbol == "X") {
				opponentSymbol = "O";
			} else {
				opponentSymbol = "X";
			}

			// Display delay for CPU turn.
			game.gameBoard.footerButton.Text = "Please wait...";
			await Task.Delay(1000);

			// Move determined by difficulty.
			int diffNumber = 0;
			if (game.difficulty == 0) {
				diffNumber = 5;
			} else if (game.difficulty == (Difficulty)1) {
				diffNumber = 8;
			} else if (game.difficulty == (Difficulty)2) {
				diffNumber = 10;
			}

			// Get random number for CPU move.
			int chance = game.rnd.Next(1, 11);

			if (chance <= diffNumber) {
				// Win if possible.
				if (game.playerTurn == false) {
					foreach (List<int> element in game.winPatterns) {
						int winningPosition = -1;
						int ownElement = 0;

						foreach (int elementInt in element) {
							// Count own elements.
							if (buttons[elementInt].Text == symbol) {
								ownElement++;
							}
							// Select potential move.
							if (buttons[elementInt].Text == "") {
								winningPosition = elementInt;
							}
						}
						// Make move and return.
						if (ownElement == 2 && winningPosition != -1) {
							buttons[winningPosition].Text = symbol;
							buttons[winningPosition].InputTransparent = true;
							moved = true;
							break;
						}
					}

					// Block if no winning move.
					if (moved == false) {
						foreach (List<int> element in game.winPatterns) {
							int blockPosition = -1;
							int opponentElement = 0;

							foreach (int elementInt in element) {
								// Count opponent elements.
								if (buttons[elementInt].Text == opponentSymbol) {
									opponentElement++;
								}
								// Select potential move.
								if (buttons[elementInt].Text == "") {
									blockPosition = elementInt;
								}
							}
							// Make move and return.
							if (opponentElement == 2 && blockPosition != -1) {
								buttons[blockPosition].Text = symbol;
								buttons[blockPosition].InputTransparent = true;
								moved = true;
								break;
							}
						}
					}

					// Take middle position if available.
					if (moved == false) {
						if (buttons[4].Text == "") {
							buttons[4].Text = symbol;
							buttons[4].InputTransparent = true;
							moved = true;
						}
					}
				}

			}

			// Make a random move.
			if (moved == false && game.playerTurn == false) {
				int rndPosition;
				do {
					rndPosition = game.rnd.Next(0, 9);
				} while (buttons[rndPosition].Text != "");

				buttons[rndPosition].Text = symbol;
				buttons[rndPosition].InputTransparent = true;
			}

			// Check for CPU win.
			if (CheckForWinner("O") == true) {
				game.gameActive = false;
				headerRight.Text = $"CPU: {Game.losses}";
			}

			// Check for draw.
			if (game.gameActive == true && CheckForDraw() == true) {
				game.gameActive = false;
			}

			// Enable all blank buttons so the player can move again.
			for (int i = 0; i < 9; i++) {
				if (buttons[i].Text == "") {
					buttons[i].InputTransparent = false;
				}
			}

			if (game.gameActive == true) {
				game.gameBoard.footerButton.Text = "Player1, make your move...";
				game.playerTurn = true;
			}
		}

		//--------------------------------------------------------------------------------
		// Check for winner.
		//--------------------------------------------------------------------------------
		public bool CheckForWinner(string symbol) {
			// Iterate through patterns and identify the winning one.
			foreach (List<int> element in game.winPatterns) {
				int ownElement = 0;

				foreach (int elementInt in element) {
					// Count own elements.
					if (buttons[elementInt].Text == symbol) {
						ownElement++;
					}
				}
				// Make move and return.
				if (ownElement == 3) {
					if (symbol == "X") {
						Game.wins++;
						game.playerScore = $"Player1: {Game.wins}";
						if (game.numPlayers == 1) {
							game.gameBoard.footerButton.Text = "You win! Click for menu.";
						} else {
							game.gameBoard.footerButton.Text = "Player1 wins! Click for menu.";
						}
					} else {
						Game.losses++;
						if (game.numPlayers == 1) {
							game.gameBoard.footerButton.Text = "Sorry, you lose! Click for menu.";
						} else {
							game.gameBoard.footerButton.Text = "Player2 wins! Click for menu.";
						}
					}
					// Disable all blank buttons.
					for (int i = 0; i < 9; i++) {
						if (buttons[i].Text == "") {
							buttons[i].InputTransparent = true;
						}
					}
					footerButton.InputTransparent = false;
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
				if (buttons[i].Text == "") {
					count++;
				}
			}

			if (count == 0) {
				Game.draws++;
				headerMiddle.Text = $"Draws: {Game.draws}";
				game.gameBoard.footerButton.Text = "It's a draw! Click for menu.";
				game.gameBoard.footerButton.InputTransparent = false;
				return true;
			} else {
				return false;
			}
		}
	}
}
