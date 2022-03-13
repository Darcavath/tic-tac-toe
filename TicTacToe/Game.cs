using System;
using System.Collections.Generic;

namespace TicTacToe {
	public class Score {
		public string date { get; set; }
		public int data { get; set; }
	}

	public enum Difficulty {
		normal,
		hard,
		expert
	};

	public class Game {
		public MainPage mainPage;
		public HighScore highScore;
		public GameBoard gameBoard;
		public Difficulty difficulty = Difficulty.normal;
		public bool gameActive = false;
		public bool loop = true;
		public bool playerTurn = false;
		public static int wins;
		public string playerScore;
		public static int losses;
		public static int draws;
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

		public List<Score> highScoreList = new List<Score>() {
			//new Score { date = DateTime.Today, data = 125 },
			//new Score { date = DateTime.Today, data = 78 }
		};


		public Game(MainPage page) {
			mainPage = page;
			highScore = new HighScore(this);
			gameBoard = new GameBoard(this);
			wins = 0;
			playerScore = $"Player: {wins}";
			losses = 0;
			draws = 0;
		}
	}
}
