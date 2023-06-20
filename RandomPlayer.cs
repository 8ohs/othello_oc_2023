using System;
using System.Threading;

public class RandomPlayer : OthelloAI, OthelloAIInterface {
    private static Random rand = new Random();

    public RandomPlayer() : base() {}
    public RandomPlayer(String name) : base(name) {}
    
    public int[] action(int[,] board, int player) {
	
	Board b = new Board();
	b.setBoard(board);
	int[,] gouhoute = b.getGouhoute(player);
	int len = gouhoute.GetLength(0);
	int index = rand.Next(0,len);	

	int[] ans = new int[2];
	ans[0] = gouhoute[index, 0];
	ans[1] = gouhoute[index, 1];

	//Thread.Sleep(2000); //ゆっくりにする
	
	return ans;
    }
}
