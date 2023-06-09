using System;
using System.Threading.Tasks;

public class MonteCarloPlayer2 : OthelloAI, OthelloAIInterface {
    public MonteCarloPlayer2(String name, int n) : base(name) {
	this.maxTryNum = n;
    }
    public MonteCarloPlayer2(int n) : base() {
	this.maxTryNum = n;
    }
    public MonteCarloPlayer2(String name, int n, int w) : base(name) {
	this.maxTryNum = n;
	this.weight = w;
    }    
    public MonteCarloPlayer2(String name) : base(name) {}
    public MonteCarloPlayer2() : base() {}

    private int maxTryNum = 1000; //プレイアウト数
    private int weight = 10;

    public int[] action(int[,] board, int player) {
	Board b = new Board();
	Random rand = new Random();
	//RandomPlayer p1 = new RandomPlayer();
	//RandomPlayer p2 = new RandomPlayer();

	RPForMonteCarlo p1 = new RPForMonteCarlo();
	RPForMonteCarlo p2 = new RPForMonteCarlo();

	b.setBoard(board);
	int[,] gouhoute = b.getGouhoute(player);
	int len = gouhoute.GetLength(0);
	
	int[] tryNum = new int[len];
	int[] loseNum = new int[len];
	int ansIndex = 0;
	double maxLoseRate = 0;

	for (int i = 0; i < this.maxTryNum; i++) {
	    //Console.Write("({0}/{1})\r", i, maxTryNum); //プレイアウトの状況を表示
	    int index = rand.Next(0,len);
	    tryNum[index]++;
	    loseNum[index] += battleResult(b, gouhoute[index,0], gouhoute[index,1], p1, p2, player);
	}

	// Parallel.For(0, this.maxTryNum, i => {
	//     int index = rand.Next(0,len);
	//     tryNum[index]++;
	//     loseNum[index] += battleResult(b, gouhoute[index,0], gouhoute[index,1], p1, p2, player);
	// });

	
	for (int i = 0; i < len; i++) {
	    if (tryNum[i] == 0) continue;
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

    private int battleResult(Board board, int x, int y, OthelloAIInterface p1, OthelloAIInterface p2, int player) {
	//負けたら1かったら0を返す
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

	if (b.numOfStone(player) == 0) return this.weight;
	
	if (b.numOfStone(player) < b.numOfStone(player*-1)) {
	    return 1;
	} else return 0;
	
    }
}
