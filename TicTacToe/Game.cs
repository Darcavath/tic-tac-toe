namespace TicTacToe {
	public class Game {
		public TitleScreen titleScreen;
		public HighScore highScore;
		public GameBoard gameBoard;
		public static int wins;
		public static int losses;
		public static int draws;

		public Game() {
			titleScreen = new TitleScreen();
			highScore = new HighScore();
			gameBoard = new GameBoard();
			wins = 0;
			losses = 0;
			draws = 0;
		}
	}
}
