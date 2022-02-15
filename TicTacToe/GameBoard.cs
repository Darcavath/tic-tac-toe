using System;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace TicTacToe {
	public class GameBoard {
		public Label[] positionLabel = new Label[9];
		public Button[] positionButton = new Button[9];
		public Label[] headerLabel = new Label[3];
		public Label footerLabel;
		public Button returnButton;
		public Grid gridBoard;
		public StackLayout stackLayout;

		public GameBoard() {
			// Setup buttons and labels.
			for (int i = 0; i < 9; i++) {
				positionLabel[i] = new Label();
				positionButton[i] = new Button();
			}
			for (int i = 0; i < 3; i++) {
				headerLabel[i] = new Label();
			}

			footerLabel = new Label();
			returnButton = new Button();
		}

		//--------------------------------------------------------------------------------
		// Initialize and setup the gameboard.
		//--------------------------------------------------------------------------------
		public void InitializeBoard(EventHandler eventHandler) {
			// Create a new stack layout.
			stackLayout = new StackLayout { Padding = 5 };

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
			headerLabel[0].Text = $"Player: {Game.wins}";
			headerLabel[0].FontSize = 24;
			headerLabel[0].HorizontalOptions = LayoutOptions.Start;
			headerLabel[0].VerticalOptions = LayoutOptions.Center;
			topGrid.Children.Add(headerLabel[1], 1, 0);
			headerLabel[1].Text = $"Draw: {Game.draws}";
			headerLabel[1].FontSize = 24;
			headerLabel[1].HorizontalOptions = LayoutOptions.Center;
			headerLabel[1].VerticalOptions = LayoutOptions.Center;
			topGrid.Children.Add(headerLabel[2], 2, 0);
			headerLabel[2].Text = $"CPU: {Game.losses}";
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
			positionButton[0].IsEnabled = false;
			positionButton[0].Clicked += new EventHandler(eventHandler);

			gridBoard.Children.Add(positionLabel[1], 1, 0);
			positionLabel[1].Text = "";
			positionLabel[1].FontSize = 96;
			positionLabel[1].HorizontalOptions = LayoutOptions.Center;
			positionLabel[1].VerticalOptions = LayoutOptions.Center;
			gridBoard.Children.Add(positionButton[1], 1, 0);
			positionButton[1].IsEnabled = false;
			positionButton[1].Clicked += new EventHandler(eventHandler);

			gridBoard.Children.Add(positionLabel[2], 2, 0);
			positionLabel[2].Text = "";
			positionLabel[2].FontSize = 96;
			positionLabel[2].HorizontalOptions = LayoutOptions.Center;
			positionLabel[2].VerticalOptions = LayoutOptions.Center;
			gridBoard.Children.Add(positionButton[2], 2, 0);
			positionButton[2].IsEnabled = false;
			positionButton[2].Clicked += new EventHandler(eventHandler);

			// Row 1
			gridBoard.Children.Add(positionLabel[3], 0, 1);
			positionLabel[3].Text = "";
			positionLabel[3].FontSize = 96;
			positionLabel[3].HorizontalOptions = LayoutOptions.Center;
			positionLabel[3].VerticalOptions = LayoutOptions.Center;
			gridBoard.Children.Add(positionButton[3], 0, 1);
			positionButton[3].IsEnabled = false;
			positionButton[3].Clicked += new EventHandler(eventHandler);

			gridBoard.Children.Add(positionLabel[4], 1, 1);
			positionLabel[4].Text = "";
			positionLabel[4].FontSize = 96;
			positionLabel[4].HorizontalOptions = LayoutOptions.Center;
			positionLabel[4].VerticalOptions = LayoutOptions.Center;
			gridBoard.Children.Add(positionButton[4], 1, 1);
			positionButton[4].IsEnabled = false;
			positionButton[4].Clicked += new EventHandler(eventHandler);

			gridBoard.Children.Add(positionLabel[5], 2, 1);
			positionLabel[5].Text = "";
			positionLabel[5].FontSize = 96;
			positionLabel[5].HorizontalOptions = LayoutOptions.Center;
			positionLabel[5].VerticalOptions = LayoutOptions.Center;
			gridBoard.Children.Add(positionButton[5], 2, 1);
			positionButton[5].IsEnabled = false;
			positionButton[5].Clicked += new EventHandler(eventHandler);

			// Row 2
			gridBoard.Children.Add(positionLabel[6], 0, 2);
			positionLabel[6].Text = "";
			positionLabel[6].FontSize = 96;
			positionLabel[6].HorizontalOptions = LayoutOptions.Center;
			positionLabel[6].VerticalOptions = LayoutOptions.Center;
			gridBoard.Children.Add(positionButton[6], 0, 2);
			positionButton[6].IsEnabled = false;
			positionButton[6].Clicked += new EventHandler(eventHandler);

			gridBoard.Children.Add(positionLabel[7], 1, 2);
			positionLabel[7].Text = "";
			positionLabel[7].FontSize = 96;
			positionLabel[7].HorizontalOptions = LayoutOptions.Center;
			positionLabel[7].VerticalOptions = LayoutOptions.Center;
			gridBoard.Children.Add(positionButton[7], 1, 2);
			positionButton[7].IsEnabled = false;
			positionButton[7].Clicked += new EventHandler(eventHandler);

			gridBoard.Children.Add(positionLabel[8], 2, 2);
			positionLabel[8].Text = "";
			positionLabel[8].FontSize = 96;
			positionLabel[8].HorizontalOptions = LayoutOptions.Center;
			positionLabel[8].VerticalOptions = LayoutOptions.Center;
			gridBoard.Children.Add(positionButton[8], 2, 2);
			positionButton[8].IsEnabled = false;
			positionButton[8].Clicked += new EventHandler(eventHandler);

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
			returnButton.Clicked += new EventHandler(eventHandler);

			stackLayout.Children.Add(bottomGrid);

			//Title = "Tic-Tac-Toe";
			//Content = grid;
			//content = stackLayout;
			//return stackLayout;
		}

		//--------------------------------------------------------------------------------
		// Reset the gameboard for a new game.
		//--------------------------------------------------------------------------------
		public void ResetGameBoard() {

			for (int i = 0; i < 9; i++) {
				positionLabel[i].Text = "";
				positionButton[i].IsEnabled = false;
			}

			returnButton.IsEnabled = false;
		}
	}
}
