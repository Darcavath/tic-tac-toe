using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace TicTacToe {
	public partial class MainPage : ContentPage {
        public Label[] positionLabel;
        public Label[] headerLabel;
        public Label footerLabel;
        public Grid gridBoard;
        public Button[] positionButton;
        public bool playerTurn = true;
        public string headerMessage = "";
        public string footerMessage = "";
        public int wins = 0;
        public int loses = 0;
        public int draws = 0;
        public bool loop = true;
        Random rnd = new Random();

        public MainPage() {
            // Initialize system.
			InitializeComponent();

            // Initialize the gameboard.
            InitializeBoard();
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
                BackgroundColor = Color.FromHex("#2196F3"),
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
            headerLabel[2].Text = $"CPU: {loses}";
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
                BackgroundColor = Color.FromHex("#2196F3"),
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
                BackgroundColor = Color.FromHex("#2196F3"),
                CornerRadius = 5
            };
            bottomGrid.Children.Add(bottomBoxView);

            footerLabel = new Label();
            bottomGrid.Children.Add(footerLabel, 0, 0);
            footerLabel.Text = "Please make your move...";
            footerLabel.FontSize = 24;
            footerLabel.HorizontalOptions = LayoutOptions.Center;
            footerLabel.VerticalOptions = LayoutOptions.Center;

            stackLayout.Children.Add(bottomGrid);

            Title = "Grid alignment demo";
            //Content = grid;
            Content = stackLayout;
        }

        //--------------------------------------------------------------------------------
        // Handle gameboard clicks here.
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
                Console.WriteLine("Yes!");
            }

            // Set control for CPU.
            playerTurn = false;

            //footerLabel.Text = "Please wait...";

            // ** Initialize CPU move here? **
            CPUTurn("O");
        }

        // Perform CPU turn.
        public void CPUTurn() {
            // Check for winning move.
            CPUTurn("O");

            if (CheckForWinner("O")) {
                //footerLabel.Text = "Sorry, you lose!";
            }

            // Check for block.

            // Take middle if available.

            // Random move.
		}

        //--------------------------------------------------------------------------------
        // Perform CPU turn.
        //--------------------------------------------------------------------------------
        public void CPUTurn(string symbol) {
            List<List<int>> winPatterns = new List<List<int>>() {
                new List<int> { 0, 1, 2 },
                new List<int> { 3, 4, 5 },
                new List<int> { 6, 7, 8 },
                new List<int> { 0, 3, 6 },
                new List<int> { 1, 4, 7 },
                new List<int> { 2, 5, 8 },
                new List<int> { 0, 4, 8 },
                new List<int> { 2, 4, 6 }
            };

            int ownElement = 0;
            int winningPosition = -1;

            // Iterate through patterns and identify the winning one.
            foreach (List<int> element in winPatterns) {
                winningPosition = -1;
                ownElement = 0;

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
                    Console.WriteLine("Winning move is: " + winningPosition);
                    break;
                }
            }

            // Make a random move.
            do {
                winningPosition = rnd.Next(0, 9); // returns random integers >= 10 and < 19
            } while (positionLabel[winningPosition].Text != "");
            
            positionLabel[winningPosition].Text = symbol;
            positionButton[winningPosition].IsEnabled = false;

            // Disable all blank buttons until player can move again.
            for (int i = 0; i < 9; i++) {
                if (positionLabel[i].Text == "") {
                    positionButton[i].IsEnabled = true;
                }
            }

            playerTurn = true;
        }

        //--------------------------------------------------------------------------------
        // Check for winner.
        //--------------------------------------------------------------------------------
        public bool CheckForWinner(string symbol) {
            List<List<int>> winPatterns = new List<List<int>>() {
            new List<int> { 0, 1, 2 },
                new List<int> { 3, 4, 5 },
                new List<int> { 6, 7, 8 },
                new List<int> { 0, 3, 6 },
                new List<int> { 1, 4, 7 },
                new List<int> { 2, 5, 8 },
                new List<int> { 0, 4, 8 },
                new List<int> { 2, 4, 6 }
            };

            int ownElement = 0;
            int winningPosition = -1;

            // Iterate through patterns and identify the winning one.
            foreach (List<int> element in winPatterns) {
                winningPosition = -1;
                ownElement = 0;

                foreach (int elementInt in element) {
                    // Count own elements.
                    if (positionLabel[elementInt].Text == symbol) {
                        ownElement++;
                    }
                }
                // Make move and return.
                if (ownElement == 3) {
                    if (symbol == "X") {
                        footerLabel.Text = "You Win!";
                    } else {
                        footerLabel.Text = "Sorry, you lose!";
                    }
                    return true;
                }
            }
            return false;
        }
	}
}
