using System;

public class RandomPlayer : OthelloAI, OthelloAIInterface {
    private static Random rand = new Random();

    public RandomPlayer() : base() {}
    public RandomPlayer(String name) : base(name) {}
    
    public int[] action(int[,] board, int player) {

	//合法手を列挙してから選ぶ
	Board b = new Board();
	b.setBoard(board);
	int[,] gouhoute = b.getGouhoute(player);
	int len = gouhoute.GetLength(0);
	int index = rand.Next(0,len);	

	//printGouhoute(gouhoute);
	//Console.WriteLine("len={0},index={1}",len, index);
	
	int[] ans = new int[2];
	ans[0] = gouhoute[index, 0];
	ans[1] = gouhoute[index, 1];

	return ans;
    }

    void printGouhoute(int[,] te) {
	for (int i = 0; i < te.GetLength(0); i++) {
	    Console.WriteLine("{0}:[{1},{2}]", i, te[i,0], te[i,1]);
	}
    }
}

public class RPForMonteCarlo : OthelloAI, OthelloAIInterface {
    private static Random rand = new Random();

    public RPForMonteCarlo() : base() {}
    public RPForMonteCarlo(String name) : base(name) {}
    
    public int[] action(int[,] board, int player) {

	//合法手を列挙してから選ぶ
	Board b = new Board();
	b.setBoard(board);
	int[,] gouhoute = b.getGouhoute(player);
	int len = gouhoute.GetLength(0);
	int index = rand.Next(0,len);	

	int[] ans = new int[2];
	ans[0] = gouhoute[index, 0];
	ans[1] = gouhoute[index, 1];

	return ans;
    }
}
