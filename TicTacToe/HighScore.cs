using System;
using System.Collections.Generic;
using System.Text.Json;
using Xamarin.Forms;

namespace TicTacToe {
	public class HighScore {
		public Label returnLabel;
		public Button returnButton;

		public HighScore() {
			returnLabel = new Label();
			returnButton = new Button();
		}

		public StackLayout InitializeHighScoreScreen(List<Score> highScore, EventHandler eventHandler) {
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

			// ** Todo: Handle different platforms. **
			string jsonString = JsonSerializer.Serialize(highScore);
			Label contentLabel = new Label {
				Text = jsonString,
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
			bottomGrid.Children.Add(bottomBoxView, 0, 0);
			bottomGrid.Children.Add(returnLabel, 0, 0);
			returnLabel.Text = "Return to menu";
			returnLabel.FontSize = 32;
			returnLabel.HorizontalOptions = LayoutOptions.Center;
			returnLabel.VerticalOptions = LayoutOptions.Center;
			bottomGrid.Children.Add(returnButton, 0, 0);
			returnButton.IsEnabled = true;
			returnButton.Clicked += new EventHandler(eventHandler);

			stackLayout.Children.Add(bottomGrid);
			//content = stackLayout;
			return stackLayout;
		}
	}
}
