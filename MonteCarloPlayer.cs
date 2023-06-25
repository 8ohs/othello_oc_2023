using System;
using System.Threading.Tasks;

public class MonteCarloPlayer : OthelloAI, OthelloAIInterface {
    public MonteCarloPlayer(String name, int n) : base(name) {
	this.maxTryNum = n;
    }
    public MonteCarloPlayer(String name) : base(name) {}
    public MonteCarloPlayer() : base() {}

    private int maxTryNum = 1000; //プレイアウト数    

    public int[] action(int[,] board, int player) {
	Board b = new Board();
	Random rand = new Random();

	b.setBoard(board);
	int[,] gouhoute = b.getGouhoute(player);
	int len = gouhoute.GetLength(0);
	
	int[] tryNum = new int[len];
	int[] loseNum = new int[len];
	int ansIndex = 0;
	double maxLoseRate = 0;

	// for (int i = 0; i < this.maxTryNum; i++) {
	//     //Console.Write("({0}/{1})\r", i, maxTryNum); //プレイアウトの状況を表示	    
	//     Task.Run(() => playout(b, tryNum, loseNum, gouhoute, len, player));
	// }

	//Parallel.For(0, this.maxTryNum, new ParallelOptions(){MaxDegreeOfParallelism = 100}, i => playout(board, tryNum, loseNum, gouhoute, len, player));

	//debug(gouhoute, tryNum, loseNum, len);

	Parallel.For(0, this.maxTryNum, i => {
//Console.Write("({0}/{1})\r", i, maxTryNum); //プレイアウトの状況を表示	    
	    int index = rand.Next(0,len);
	    tryNum[index]++;
	    loseNum[index] += battleResult(board, gouhoute[index,0], gouhoute[index,1], player);
	});

	//debug(gouhoute, tryNum, loseNum, len);
	
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

    // static void playout(int[,] board, int[] tryNum, int[] loseNum, int[,] gouhoute, int len, int player) {	
    // 	int index = new Random().Next(0,len);
    // 	tryNum[index]++;
    // 	loseNum[index] += battleResult(board, gouhoute[index,0], gouhoute[index,1], player);
    // }

    static int battleResult(int[,] board, int x, int y, int player) {
	/*
	  (x,y)にplayerの石を置いてランダムに戦った結果
	  playerが負けたら1、勝ったら0を返す
	*/
	Board b = new Board();
	b.setBoard(board);
	
	b.putStone(x,y,player);
	//RPForMonteCarlo p1 = new RPForMonteCarlo();
	//RPForMonteCarlo p2 = new RPForMonteCarlo();

	RandomPlayer p1 = new RandomPlayer();
	RandomPlayer p2 = new RandomPlayer();
	
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

    static void debug(int[,] gouhoute, int[] tryNum, int[] loseNum, int len) {
	Console.WriteLine("tryNum");
	for (int i = 0; i < len; i++) {
	    Console.WriteLine("[{2},{3}]{0}:{1}",i, tryNum[i], gouhoute[i,0], gouhoute[i,1]);
	}

	Console.WriteLine("loseNum");
	for (int i = 0; i < len; i++) {
	    Console.WriteLine("[{2},{3}]{0}:{1}",i, loseNum[i], gouhoute[i,0], gouhoute[i,1]);
	}
    }
}
