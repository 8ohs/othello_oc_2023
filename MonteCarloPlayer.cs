using System;

public class MonteCarloPlayer : OthelloAI, OthelloAIInterface {
    public MonteCarloPlayer(String name) : base(name) {} //コンストラクタ
    public MonteCarloPlayer() : base() {}

    private const int MAX_TRY_NUM = 1000;

    public int[] action(int[,] board, int player) {
	Board b = new Board();
	Random rand = new Random();
	RandomPlayer p1 = new RandomPlayer();
	RandomPlayer p2 = new RandomPlayer();

	b.setBoard(board);
	int[,] gouhoute = b.getGouhoute(player);
	
	/*
	gouhoute[0,0] 0個目のx座標
	gouhoute[0,1] 0個目のy座標

	gouhoute[1,0] 1個目のX座標
	gouhoute[1,1] 1個目のy座標
	*/

	int len = gouhoute.GetLength(0);
	int[] tryNum = new int[len];
	int[] loseNum = new int[len];
	int ansIndex = 0;
	double maxLoseRate = 0;

	for (int i = 0; i < MAX_TRY_NUM; i++) {
	    int index = rand.Next(0,len);
	    tryNum[index]++;
	    loseNum[index] += battleResult(b, gouhoute[index,0], gouhoute[index,1], p1, p2, player);
	}

	
	for (int i = 0; i < len; i++) {
	    if ((double)(loseNum[i]) / (double)(tryNum[i]) > maxLoseRate) {
		maxLoseRate = (double)(loseNum[i]) / (double)(tryNum[i]);
		ansIndex = i;
	    }
	}

	int[] ans = new int[2];
	ans[0] = gouhoute[ansIndex,0];
	ans[1] = gouhoute[ansIndex,1];
	return ans;
    }

    static int battleResult(Board board, int x, int y, OthelloAIInterface p1, OthelloAIInterface p2, int player) {
	Board b = new Board();
	b.setBoard(board.getBoard());
	b.putStone(x,y,player);
	
	do {
	    if (b.numOfPuttable(player*-1) != 0) {
		int[] te = p1.action(b.getBoard(), player*-1);
		b.putStone(te[0], te[1], player*-1);
	    }
	    
	    if (b.numOfPuttable(player) != 0) {
		int[] te = p2.action(b.getBoard(), player);
		b.putStone(te[0], te[1], player);
	    }
	    
	} while (b.numOfPuttable(1) + b.numOfPuttable(-1) != 0);
	
	
	if (b.numOfStone(player) < b.numOfStone(player*-1)) return 1;
	else return 0;
    }
}
