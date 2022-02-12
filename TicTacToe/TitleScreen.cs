using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TicTacToe {
	public class TitleScreen {
		public Label playLabel;
		public Button playButton;
		public Label quitLabel;
		public Button quitButton;
		public Label versesLabel;
		public Button versesButton;
		public Label scoreLabel;
		public Button scoreButton;
		public Label diffLabel;
		public Button diffButton;

		public TitleScreen() {
			playLabel = new Label();
			playButton = new Button();
			quitLabel = new Label();
			quitButton = new Button();
			versesLabel = new Label();
			versesButton = new Button();
			scoreLabel = new Label();
			scoreButton = new Button();
			diffLabel = new Label();
			diffButton = new Button();
		}

		public StackLayout InitializeTitle(EventHandler eventHandler, int diff) {
			// Create a new stack layout.
			StackLayout stackLayout = new StackLayout { Padding = 5 };

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
			playButton.Clicked += new EventHandler(eventHandler);

			BoxView diffBoxView = new BoxView {
				BackgroundColor = Xamarin.Forms.Color.FromHex("#2196F3"),
				CornerRadius = 5
			};
			topGrid.Children.Add(diffBoxView, 0, 1);
			topGrid.Children.Add(diffLabel, 0, 1);
			if (diff == 0) {
				diffLabel.Text = "Normal";
			} else if (diff == 1) {
				diffLabel.Text = "Hard";
			} else {
				diffLabel.Text = "Expert";
			}
			diffLabel.FontSize = 32;
			diffLabel.HorizontalOptions = LayoutOptions.Center;
			diffLabel.VerticalOptions = LayoutOptions.Center;
			topGrid.Children.Add(diffButton, 0, 1);
			diffButton.IsEnabled = true;
			diffButton.Clicked += new EventHandler(eventHandler);

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
			versesButton.Clicked += new EventHandler(eventHandler);

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
			scoreButton.Clicked += new EventHandler(eventHandler);

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
			quitButton.Clicked += new EventHandler(eventHandler);

			stackLayout.Children.Add(topGrid);
			return stackLayout;
			//content = stackLayout;
		}
	}
}
