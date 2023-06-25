using System;

public class NegaMaxPlayer : OthelloAI, OthelloAIInterface {
    public NegaMaxPlayer(String name) : base(name) {}
    public NegaMaxPlayer() : base() {}

    private const int DEPTH = 10;
    private int player;

    public int[] action(int[,] board, int player) {
	this.player = player;
	int ansIndex = 0;
	int maxScore = 0;
	Board b = new Board(board);
	int[,] gouhoute = b.getGouhoute(player);

	for (int i = 0; i < gouhoute.GetLength(0); i++) {
	    b.setBoard(board);
	    b.putStone(gouhoute[i,0], gouhoute[i,1], player);
	    int score = eval(b.getBoard(), DEPTH, player*-1);
	    if (maxScore < score) {
		ansIndex = i;
		maxScore = score;
	    }
	}
	return new int[] {gouhoute[ansIndex, 0], gouhoute[ansIndex, 1]};
    }

    private int eval(int[,] board, int depth, int player) {
	if (depth == 0) return calcEval(board, player);

	int maxScore = 0;
	Board b = new Board();
	b.setBoard(board);
	int[,] gouhoute = b.getGouhoute(player);
	int gouhouteLen = gouhoute.GetLength(0);
	
	if (b.numOfPuttable(1) + b.numOfPuttable(-1) == 0) return calcEval(board, player);//終局時
	if (b.numOfPuttable(player) == 0) eval(board, depth-1, player*-1);//パス
		
	for (int i = 0; i < gouhouteLen; i++) {
	    b.setBoard(board);
	    b.putStone(gouhoute[i,0], gouhoute[i,1], player);
	    int score = -1*eval(b.getBoard(), depth-1, player*-1);
	    if (maxScore < score) {
		maxScore = score;
	    }
	}
	return maxScore;
    }

    private int calcEval(int[,] board, int player) {
	//少ないほど大きい値
	Board b = new Board();
	b.setBoard(board);
	return (player * this.player) * (-1*b.numOfStone(player));
    }

    private void debug(Board b, int eval) {
	b.showBoard();
	Console.WriteLine("eval = {0}", eval);
    }
}
