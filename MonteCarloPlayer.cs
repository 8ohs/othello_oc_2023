using System;
//RandomPlayerを修正してから作る予定

public class MonteCarloPlayer : OthelloAI, OthelloAIInterface {
    public MonteCarloPlayer(String name) : base(name) {} //コンストラクタ
    public MonteCarloPlayer() : base() {}

    public int[] action(int[,] board, int player) {
	RandomPlayer p1 = new RandomPlayer();
	RandomPlayer p2 = new RandomPlayer();
	Board board = new Board();
	board.setBoard(board);
	int[,] gouhoute = board.getGouhoute(player);
	gouhoute = extendArray(gouhoute);
    }

    private int[,] extendArray(int[,] array) {
	//合法手の配列にプレイアウト数と勝利数を格納するために配列を拡張
	int ans = new int[array.GetLength(0),4];
	for (int i = 0; i < array.GetLength(0); i++) {
	    ans[i,0] = array[i,0];
	    ans[i,1] = array[i,1];
	}
	return ans;
    }
}
