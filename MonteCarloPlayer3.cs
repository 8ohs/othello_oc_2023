using System;
using System.Threading.Tasks;

public class MonteCarloPlayer3 : OthelloAI, OthelloAIInterface {
    public MonteCarloPlayer3(String name, int n) : base(name) {
	this.maxTryNum = n;
    }
    public MonteCarloPlayer3(int n) : base() {
	this.maxTryNum = n;
    }
    public MonteCarloPlayer3(String name, int n, int w) : base(name) {
	this.maxTryNum = n;
	this.weight = w;
    }    
    public MonteCarloPlayer3(String name) : base(name) {}
    public MonteCarloPlayer3() : base() {}

    private int maxTryNum = 1000; //プレイアウト数
    private int weight = 10;
    private int myPlayer = 0;
    private const int DEPTH = 14;

    public int[] action(int[,] board, int player) {
	Board b = new Board(board);
	this.myPlayer = player;
	int[,] gouhoute = b.getGouhoute(player);
	int len = gouhoute.GetLength(0);	
	
	if (b.numOfStone() >= 64 - DEPTH) { //最後は読み切る
	    for (int i = 0; i < len; i++) {
		Board b2 = new Board(board);
		b2.putStone(gouhoute[i,0], gouhoute[i,1], player);
		if (isLose(b2, -1*player)) {
		    Console.WriteLine("=============負け確定===========");
		    return new int[] {gouhoute[i,0], gouhoute[i,1]};
		} else Console.WriteLine("=============まだわんちゃん...===========");
	    }
	}
	
	Random rand = new Random();
	int[] tryNum = new int[len];
	int[] loseNum = new int[len];
	int ansIndex = 0;
	double maxLoseRate = 0;

	for (int i = 0; i < this.maxTryNum; i++) {
	    //Console.Write("({0}/{1})\r", i, maxTryNum); //プレイアウトの状況を表示
	    int index = rand.Next(0,len);
	    tryNum[index]++;
	    loseNum[index] += b.battleRandom(gouhoute[index,0], gouhoute[index,1], player, this.weight);
	    if (gouhoute[index,0] % 7 == 0 & gouhoute[index,1] % 7 == 0) loseNum[index] -= 5;//角はちょっと選びにくく
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
	return new int[] {gouhoute[ansIndex, 0], gouhoute[ansIndex, 1]};
    }

    private bool isLose(Board b, int player) {
	if (b.numOfPuttable() == 0) return b.isLose(this.myPlayer);
	if (b.numOfPuttable(player) == 0) return isLose(b, player*-1);
	int[,] gouhoute = b.getGouhoute(player);
	int len = gouhoute.GetLength(0);
	
	if (player != myPlayer) { //AND
	    for (int i = 0;  i < len; i++) {
		Board b2 = new Board(b.getBoard());
		b2.putStone(gouhoute[i,0], gouhoute[i,1], player);
		if (!isLose(b2, player*-1)) return false;
	    }
	    return true;
	} else { //OR
	    for (int i = 0; i < len; i++) {
		Board b2 = new Board(b.getBoard());
		b2.putStone(gouhoute[i,0], gouhoute[i,1], player);
		if (isLose(b2, player*-1)) return true;
	    }
	}
	return false;
    }
}
