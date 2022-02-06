using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using Xamarin.Forms;
using System.IO;

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
		public Label[] positionLabel;
		public Label[] headerLabel;
		public Label footerLabel;
		public Grid gridBoard;
		public Button[] positionButton;
		public Label playLabel = new Label();
		public Button playButton = new Button();
		public Label quitLabel = new Label();
		public Button quitButton = new Button();
		public Label versesLabel = new Label();
		public Button versesButton = new Button();
		public Label scoreLabel = new Label();
		public Button scoreButton = new Button();
		public Label diffLabel = new Label();
		public Button diffButton = new Button();
		public Label returnLabel = new Label();
		public Button returnButton = new Button();
		public bool playerTurn = true;
		public string headerMessage = "";
		public string footerMessage = "";
		public bool gameActive = false;
		public int wins = 0;
		public int losses = 0;
		public int draws = 0;
		public bool titleScreen = true; // Necessary?
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
			InitializeTitle();
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
		public void InitializeTitle() {
			// Create a new stack layout.
			var stackLayout = new StackLayout { Padding = 5 };

			// Load title image.
			Image image = new Image { Source = "tic-tac-toe.png" };
			//Image image = new Image();
			//image.Source = ImageSource.FromFile("tic-tac-toe.png");
			image.Aspect = Aspect.AspectFill;
			stackLayout.Children.Add(image);

			// Create title grid.
			Grid topGrid = new Grid {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				RowSpacing = 5,
				ColumnSpacing = 5,
				RowDefinitions = {
					new RowDefinition(),
					new RowDefinition(),
					new RowDefinition()
				},
				ColumnDefinitions = {
					new ColumnDefinition(),
					new ColumnDefinition()
				}
			};

			// Add menu buttons.
			BoxView playBoxView = new BoxView {
				BackgroundColor = Xamarin.Forms.Color.FromHex("#2196F3"),
				CornerRadius = 5,
			};
			topGrid.Children.Add(playBoxView, 0, 0);
			Grid.SetColumnSpan(playBoxView, 2);
			topGrid.Children.Add(playLabel, 0, 0);
			playLabel.Text = "Play";
			playLabel.FontSize = 32;
			Grid.SetColumnSpan(playLabel, 2);
			playLabel.HorizontalOptions = LayoutOptions.Center;
			playLabel.VerticalOptions = LayoutOptions.Center;
			topGrid.Children.Add(playButton, 0, 0);
			Grid.SetColumnSpan(playButton, 2);
			playButton.IsEnabled = true;
			playButton.Clicked += new EventHandler(TitleButtonClick);

			BoxView diffBoxView = new BoxView {
				BackgroundColor = Xamarin.Forms.Color.FromHex("#2196F3"),
				CornerRadius = 5
			};
			topGrid.Children.Add(diffBoxView, 0, 1);
			topGrid.Children.Add(diffLabel, 0, 1);
			if (difficulty == 0) {
				diffLabel.Text = "Normal";
			} else if (difficulty == (Difficulty)1) {
				diffLabel.Text = "Hard";
			} else {
				diffLabel.Text = "Expert";
			}
			diffLabel.FontSize = 32;
			diffLabel.HorizontalOptions = LayoutOptions.Center;
			diffLabel.VerticalOptions = LayoutOptions.Center;
			topGrid.Children.Add(diffButton, 0, 1);
			diffButton.IsEnabled = true;
			diffButton.Clicked += new EventHandler(TitleButtonClick);

			BoxView vsBoxView = new BoxView {
				BackgroundColor = Xamarin.Forms.Color.FromHex("#2196F3"),
				CornerRadius = 5
			};
			topGrid.Children.Add(vsBoxView, 1, 1);
			topGrid.Children.Add(versesLabel, 1, 1);
			versesLabel.Text = "vs CPU";
			versesLabel.FontSize = 32;
			versesLabel.HorizontalOptions = LayoutOptions.Center;
			versesLabel.VerticalOptions = LayoutOptions.Center;
			topGrid.Children.Add(versesButton, 1, 1);
			versesButton.IsEnabled = true;
			versesButton.Clicked += new EventHandler(TitleButtonClick);

			BoxView hsBoxView = new BoxView {
				BackgroundColor = Xamarin.Forms.Color.FromHex("#2196F3"),
				CornerRadius = 5
			};
			topGrid.Children.Add(hsBoxView, 0, 2);
			topGrid.Children.Add(scoreLabel, 0, 2);
			scoreLabel.Text = "High Scores";
			scoreLabel.FontSize = 32;
			scoreLabel.HorizontalTextAlignment = TextAlignment.Center;
			scoreLabel.HorizontalOptions = LayoutOptions.Center;
			scoreLabel.VerticalOptions = LayoutOptions.Center;
			topGrid.Children.Add(scoreButton, 0, 2);
			scoreButton.IsEnabled = true;
			scoreButton.Clicked += new EventHandler(HSButtonClick);

			BoxView quitBoxView = new BoxView {
				BackgroundColor = Xamarin.Forms.Color.FromHex("#2196F3"),
				CornerRadius = 5
			};
			topGrid.Children.Add(quitBoxView, 1, 2);
			topGrid.Children.Add(quitLabel, 1, 2);
			quitLabel.Text = "Quit";
			quitLabel.FontSize = 32;
			quitLabel.HorizontalOptions = LayoutOptions.Center;
			quitLabel.VerticalOptions = LayoutOptions.Center;
			topGrid.Children.Add(quitButton, 1, 2);
			quitButton.IsEnabled = true;
			quitButton.Clicked += new EventHandler(TitleButtonClick);

			stackLayout.Children.Add(topGrid);
			Content = stackLayout;
		}

		//--------------------------------------------------------------------------------
		// Initialize and setup the gameboard.
		//--------------------------------------------------------------------------------
		public void InitializeBoard() {
			positionLabel = new Label[9];
			positionButton = new Button[9];
			headerLabel = new Label[3];
			for (int i = 0; i < 9; i++) {
				positionLabel[i] = new Label();
			}
			for (int i = 0; i < 9; i++) {
				positionButton[i] = new Button();
			}
			for (int i = 0; i < 3; i++) {
				headerLabel[i] = new Label();
			}

			// Create a new stack layout.
			var stackLayout = new StackLayout { Padding = 5 };

			// Create top grid.
			Grid topGrid = new Grid {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				RowSpacing = 5,
				ColumnSpacing = 5,
				RowDefinitions = {
					new RowDefinition(),
				},
				ColumnDefinitions = {
					new ColumnDefinition(),
					new ColumnDefinition(),
					new ColumnDefinition()
				}
			};

			BoxView boxView = new BoxView {
				BackgroundColor = Xamarin.Forms.Color.FromHex("#2196F3"),
				CornerRadius = 5
			};
			Grid.SetColumnSpan(boxView, 3);
			topGrid.Children.Add(boxView);

			// Setup header labels.
			topGrid.Children.Add(headerLabel[0], 0, 0);
			headerLabel[0].Text = $"Player: {wins}";
			headerLabel[0].FontSize = 24;
			headerLabel[0].HorizontalOptions = LayoutOptions.Start;
			headerLabel[0].VerticalOptions = LayoutOptions.Center;
			topGrid.Children.Add(headerLabel[1], 1, 0);
			headerLabel[1].Text = $"Draw: {draws}";
			headerLabel[1].FontSize = 24;
			headerLabel[1].HorizontalOptions = LayoutOptions.Center;
			headerLabel[1].VerticalOptions = LayoutOptions.Center;
			topGrid.Children.Add(headerLabel[2], 2, 0);
			headerLabel[2].Text = $"CPU: {losses}";
			headerLabel[2].FontSize = 24;
			headerLabel[2].HorizontalOptions = LayoutOptions.End;
			headerLabel[2].VerticalOptions = LayoutOptions.Center;

			stackLayout.Children.Add(topGrid);

			// Create a grid for the gameboard.
			gridBoard = new Grid();
			gridBoard.HorizontalOptions = LayoutOptions.FillAndExpand;
			gridBoard.VerticalOptions = LayoutOptions.FillAndExpand;
			gridBoard.RowSpacing = 5;
			gridBoard.ColumnSpacing = 5;
			gridBoard.IsEnabled = false;
			gridBoard.RowDefinitions.Add(new RowDefinition());
			gridBoard.RowDefinitions.Add(new RowDefinition());
			gridBoard.RowDefinitions.Add(new RowDefinition());
			gridBoard.ColumnDefinitions.Add(new ColumnDefinition());
			gridBoard.ColumnDefinitions.Add(new ColumnDefinition());
			gridBoard.ColumnDefinitions.Add(new ColumnDefinition());

			// Row 0
			gridBoard.Children.Add(positionLabel[0], 0, 0);
			positionLabel[0].Text = "";
			positionLabel[0].FontSize = 96;
			positionLabel[0].HorizontalOptions = LayoutOptions.Center;
			positionLabel[0].VerticalOptions = LayoutOptions.Center;
			gridBoard.Children.Add(positionButton[0], 0, 0);
			positionButton[0].IsEnabled = true;
			positionButton[0].Clicked += new EventHandler(ButtonClick);

			gridBoard.Children.Add(positionLabel[1], 1, 0);
			positionLabel[1].Text = "";
			positionLabel[1].FontSize = 96;
			positionLabel[1].HorizontalOptions = LayoutOptions.Center;
			positionLabel[1].VerticalOptions = LayoutOptions.Center;
			gridBoard.Children.Add(positionButton[1], 1, 0);
			positionButton[1].IsEnabled = true;
			positionButton[1].Clicked += new EventHandler(ButtonClick);

			gridBoard.Children.Add(positionLabel[2], 2, 0);
			positionLabel[2].Text = "";
			positionLabel[2].FontSize = 96;
			positionLabel[2].HorizontalOptions = LayoutOptions.Center;
			positionLabel[2].VerticalOptions = LayoutOptions.Center;
			gridBoard.Children.Add(positionButton[2], 2, 0);
			positionButton[2].IsEnabled = true;
			positionButton[2].Clicked += new EventHandler(ButtonClick);

			// Row 1
			gridBoard.Children.Add(positionLabel[3], 0, 1);
			positionLabel[3].Text = "";
			positionLabel[3].FontSize = 96;
			positionLabel[3].HorizontalOptions = LayoutOptions.Center;
			positionLabel[3].VerticalOptions = LayoutOptions.Center;
			gridBoard.Children.Add(positionButton[3], 0, 1);
			positionButton[3].IsEnabled = true;
			positionButton[3].Clicked += new EventHandler(ButtonClick);

			gridBoard.Children.Add(positionLabel[4], 1, 1);
			positionLabel[4].Text = "";
			positionLabel[4].FontSize = 96;
			positionLabel[4].HorizontalOptions = LayoutOptions.Center;
			positionLabel[4].VerticalOptions = LayoutOptions.Center;
			gridBoard.Children.Add(positionButton[4], 1, 1);
			positionButton[4].IsEnabled = true;
			positionButton[4].Clicked += new EventHandler(ButtonClick);

			gridBoard.Children.Add(positionLabel[5], 2, 1);
			positionLabel[5].Text = "";
			positionLabel[5].FontSize = 96;
			positionLabel[5].HorizontalOptions = LayoutOptions.Center;
			positionLabel[5].VerticalOptions = LayoutOptions.Center;
			gridBoard.Children.Add(positionButton[5], 2, 1);
			positionButton[5].IsEnabled = true;
			positionButton[5].Clicked += new EventHandler(ButtonClick);

			// Row 2
			gridBoard.Children.Add(positionLabel[6], 0, 2);
			positionLabel[6].Text = "";
			positionLabel[6].FontSize = 96;
			positionLabel[6].HorizontalOptions = LayoutOptions.Center;
			positionLabel[6].VerticalOptions = LayoutOptions.Center;
			gridBoard.Children.Add(positionButton[6], 0, 2);
			positionButton[6].IsEnabled = true;
			positionButton[6].Clicked += new EventHandler(ButtonClick);

			gridBoard.Children.Add(positionLabel[7], 1, 2);
			positionLabel[7].Text = "";
			positionLabel[7].FontSize = 96;
			positionLabel[7].HorizontalOptions = LayoutOptions.Center;
			positionLabel[7].VerticalOptions = LayoutOptions.Center;
			gridBoard.Children.Add(positionButton[7], 1, 2);
			positionButton[7].IsEnabled = true;
			positionButton[7].Clicked += new EventHandler(ButtonClick);

			gridBoard.Children.Add(positionLabel[8], 2, 2);
			positionLabel[8].Text = "";
			positionLabel[8].FontSize = 96;
			positionLabel[8].HorizontalOptions = LayoutOptions.Center;
			positionLabel[8].VerticalOptions = LayoutOptions.Center;
			gridBoard.Children.Add(positionButton[8], 2, 2);
			positionButton[8].IsEnabled = true;
			positionButton[8].Clicked += new EventHandler(ButtonClick);

			// Add the grid to the stack.
			stackLayout.Children.Add(gridBoard);

			// Create the bottom frame display.
			Frame bottomFrame = new Frame {
				BackgroundColor = Xamarin.Forms.Color.FromHex("#2196F3"),
				CornerRadius = 5,
				Content = new Label {
					HorizontalTextAlignment = TextAlignment.Center,
					FontSize = 24,
					Text = "Information here...",
				}
			};

			// Create top grid.
			Grid bottomGrid = new Grid {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				RowSpacing = 5,
				ColumnSpacing = 5,
				RowDefinitions = {
					new RowDefinition()
				},
				ColumnDefinitions = {
					new ColumnDefinition()
				}
			};

			BoxView bottomBoxView = new BoxView {
				BackgroundColor = Xamarin.Forms.Color.FromHex("#2196F3"),
				CornerRadius = 5
			};
			bottomGrid.Children.Add(bottomBoxView);

			footerLabel = new Label();
			bottomGrid.Children.Add(footerLabel, 0, 0);
			footerLabel.Text = "Please make your move...";
			footerLabel.FontSize = 24;
			footerLabel.HorizontalOptions = LayoutOptions.Center;
			footerLabel.VerticalOptions = LayoutOptions.Center;
			bottomGrid.Children.Add(returnButton, 0, 0);
			returnButton.IsEnabled = false;
			returnButton.Clicked += new EventHandler(TitleButtonClick);

			stackLayout.Children.Add(bottomGrid);

			//Title = "Tic-Tac-Toe";
			//Content = grid;
			Content = stackLayout;
		}

		//--------------------------------------------------------------------------------
		// Initialize high scores.
		//--------------------------------------------------------------------------------
		public void InitializeHighScores() {
			// Test data.
			
			
			//Console.WriteLine($"Path = {Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HighScores.json")}");
			//string fileName = "HighScores.json";
			//highScore = JsonSerializer.Deserialize<Score>(jsonString)!;
			
			//Console.WriteLine(File.ReadAllText(fileName));

			

			// Create a new stack layout.
			var stackLayout = new StackLayout { Padding = 5 };

			// Create grid for header.
			Grid headerGrid = new Grid {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				RowSpacing = 5,
				ColumnSpacing = 5,
				RowDefinitions = {
					new RowDefinition()
				},
				ColumnDefinitions = {
					new ColumnDefinition()
				}
			};

			BoxView headerBoxView = new BoxView {
				BackgroundColor = Xamarin.Forms.Color.FromHex("#2196F3"),
				CornerRadius = 5
			};
			headerGrid.Children.Add(headerBoxView, 0, 0);

			Label titleLabel = new Label {
				Text = "High Scores",
				FontSize = 32,
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center
			};
			headerGrid.Children.Add(titleLabel, 0, 0);
			stackLayout.Children.Add(headerGrid);

			// Grid for scrollview.
			Grid scrollGrid = new Grid {
				//HorizontalOptions = LayoutOptions.FillAndExpand,
				//VerticalOptions = LayoutOptions.FillAndExpand,
				RowSpacing = 5,
				ColumnSpacing = 5,
				RowDefinitions = {
					new RowDefinition()
				},
				ColumnDefinitions = {
					new ColumnDefinition()
				}
			};

			string jsonString = JsonSerializer.Serialize(highScore);
			Label contentLabel = new Label {
				Text = jsonString,// "This is the time for all good men. Apple is the greatest computer company in the word! I love Apple!This is the time for all good men. Apple is the greatest computer company in the word! I love Apple!This is the time for all good men. Apple is the greatest computer company in the word! I love Apple!This is the time for all good men. Apple is the greatest computer company in the word! I love Apple!This is the time for all good men. Apple is the greatest computer company in the word! I love Apple!This is the time for all good men. Apple is the greatest computer company in the word! I love Apple!",
				FontSize = 32
			};

			// ** This height should be dynamic. **
			ScrollView scrollView = new ScrollView {
				Content = contentLabel,
				HeightRequest = 400,
				VerticalOptions = LayoutOptions.FillAndExpand
			};
			scrollGrid.Children.Add(scrollView, 0, 0);
			stackLayout.Children.Add(scrollGrid);

			// Create grid for return button.
			Grid bottomGrid = new Grid {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				RowSpacing = 5,
				ColumnSpacing = 5,
				RowDefinitions = {
					new RowDefinition()
				},
				ColumnDefinitions = {
					new ColumnDefinition()
				}
			};

			BoxView bottomBoxView = new BoxView {
				BackgroundColor = Xamarin.Forms.Color.FromHex("#2196F3"),
				CornerRadius = 5
			};
			bottomGrid.Children.Add(bottomBoxView, 0 , 0);
			bottomGrid.Children.Add(returnLabel, 0, 0);
			returnLabel.Text = "Return to menu";
			returnLabel.FontSize = 32;
			returnLabel.HorizontalOptions = LayoutOptions.Center;
			returnLabel.VerticalOptions = LayoutOptions.Center;
			bottomGrid.Children.Add(returnButton, 0 , 0);
			returnButton.IsEnabled = true;
			returnButton.Clicked += new EventHandler(TitleButtonClick);

			stackLayout.Children.Add(bottomGrid);	
			Content = stackLayout;
		}

		//--------------------------------------------------------------------------------
		// Handle title screen clicks here and player control.
		//--------------------------------------------------------------------------------
		public void TitleButtonClick(object sender, EventArgs e) {
			Button clickedBut = sender as Button;

			if (clickedBut.Equals(playButton)) {
				gameActive = true;
				InitializeBoard();
			}


			if (clickedBut.Equals(quitButton)) {
				CloseGame();
			}

			// 
			if (clickedBut.Equals(diffButton)) {
				switch (difficulty) {
					case 0:
						difficulty = Difficulty.hard;
						diffLabel.Text = "Hard";
						break;
					case (Difficulty)1:
						difficulty = Difficulty.expert;
						diffLabel.Text = "Expert";
						break;
					case (Difficulty)2:
						difficulty = Difficulty.normal;
						diffLabel.Text = "Normal";
						break;
					default:
						break;
				}
			}

			if (clickedBut.Equals(returnButton)) {
				InitializeTitle();
			}
		}

		//--------------------------------------------------------------------------------
		// Handle gameboard clicks here and player control.
		//--------------------------------------------------------------------------------
		public void ButtonClick(object sender, EventArgs e) {
			Button clickedBut = sender as Button;
			int elementNumber = -1;

			// Iterate through button array for matching element.
			for (int i = 0; i < 9; i++) {
				if (clickedBut.Equals(positionButton[i])) {
					elementNumber = i;
				}
			}

			// Set button's matching label.
			positionLabel[elementNumber].Text = "X";
			clickedBut.IsEnabled = false;

			// Disable all blank buttons until player can move again.
			for (int i = 0; i < 9; i++) {
				if (positionButton[i].IsEnabled == true) {
					positionButton[i].IsEnabled = false;
				}
			}

			// Check for player win.
			if (CheckForWinner("X")) {
				gameActive = false;
				wins += 1;
				headerLabel[0].Text = $"Player: {wins}";
				footerLabel.Text = "You win! Click for menu.";
				returnButton.IsEnabled = true;
				//await Task.Delay(2000);
			}

			// Check for draw game.
			if (CheckForDraw() == true) {
				gameActive = false;
				draws++;
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
		}

		//--------------------------------------------------------------------------------
		// Handle gameboard clicks here and player control.
		//--------------------------------------------------------------------------------
		public void HSButtonClick(object sender, EventArgs e) {
			Button clickedBut = sender as Button;

			if (clickedBut.Equals(scoreButton)) {
				InitializeHighScores();
			}
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
			footerLabel.Text = "Please wait...";
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

			// Get randome number for CPU move.
			int chance = rnd.Next(1, 11);

			if (chance <= diffNumber) {
				// Win if possible.
				foreach (List<int> element in winPatterns) {
					int winningPosition = -1;
					int ownElement = 0;

					foreach (int elementInt in element) {
						// Count own elements.
						if (positionLabel[elementInt].Text == symbol) {
							ownElement++;
						}
						// Select potential move.
						if (positionLabel[elementInt].Text == "") {
							winningPosition = elementInt;
						}
					}
					// Make move and return.
					if (ownElement == 2 && winningPosition != -1) {
						positionLabel[winningPosition].Text = symbol;
						positionButton[winningPosition].IsEnabled = false;
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
							if (positionLabel[elementInt].Text == opponentSymbol) {
								opponentElement++;
							}
							// Select potential move.
							if (positionLabel[elementInt].Text == "") {
								blockPosition = elementInt;
							}
						}
						// Make move and return.
						if (opponentElement == 2 && blockPosition != -1) {
							positionLabel[blockPosition].Text = symbol;
							positionButton[blockPosition].IsEnabled = false;
							moved = true;
							break;
						}
					}
				}

				// Take middle position if available.
				if (moved == false) {
					if (positionLabel[4].Text == "") {
						positionLabel[4].Text = symbol;
						positionButton[4].IsEnabled = false;
						moved = true;
					}
				}
			}

			// Make a random move.
			if (moved == false) {
				int rndPosition;
				do {
					rndPosition = rnd.Next(0, 9); // returns random integers >= 10 and < 19
				} while (positionLabel[rndPosition].Text != "");

				positionLabel[rndPosition].Text = symbol;
				positionButton[rndPosition].IsEnabled = false;
			}

			// Check for CPU win.
			if (CheckForWinner("O") == true) {
				losses++;
				headerLabel[2].Text = $"CPU: {losses}";
				returnButton.IsEnabled = true;
				await Task.Delay(2000);
				// Code
			}

			// Check for draw.
			if (CheckForDraw() == true) {
				await Task.Delay(2000);
			}

			// Enable all blank buttons so the player can move again.
			for (int i = 0; i < 9; i++) {
				if (positionLabel[i].Text == "") {
					positionButton[i].IsEnabled = true;
				}
			}

			footerLabel.Text = "Please make your move...";
			playerTurn = true;
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
					if (positionLabel[elementInt].Text == symbol) {
						ownElement++;
					}
				}
				// Make move and return.
				if (ownElement == 3) {
					if (symbol == "X") {
						footerLabel.Text = "You Win! Click for menu.";
					} else {
						footerLabel.Text = "Sorry, you lose! Click for menu.";
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
				if (positionLabel[i].Text == "") {
					count++;
				}
			}

			if (count == 0) {
				draws++;
				headerLabel[1].Text = $"Draws: {draws}";
				footerLabel.Text = "It's a draw! Click for menu.";
				returnButton.IsEnabled = true;
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
			highScore.Add(new Score { date = DateTime.Today, data = wins });
			string jsonString = JsonSerializer.Serialize(highScore);
			File.WriteAllText(fileName, jsonString);

			// ** Fix this! **
			Console.WriteLine("** Exit complete! **");
			System.Environment.Exit(0);
		}
	}
}
