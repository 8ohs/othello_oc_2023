using System;
using System.Threading;

public class RandomPlayer : OthelloAI, OthelloAIInterface {
    private static Random rand = new Random();

    public RandomPlayer() : base() {}
    public RandomPlayer(String name) : base(name) {}
    
    public int[] action(int[,] board, int player) {
	int x = 0;
	int y = 0;
	do {
	    x = rand.Next(0,8);
	    y = rand.Next(0,8);
	} while (!isStonePuttable(board, x, y, player));

	int[] ans = new int[2];
	ans[0] = x;
	ans[1] = y;

	//Thread.Sleep(2000); //ゆっくりにする
	
	return ans;
    }

    private bool isStonePuttable(int[,] board, int x, int y, int player) {
	if (x < 0 || x > 7 || y < 0 || y > 7) return false;
	//すでに置かれている 
	if (board[y,x] != 0) return false;

	int cp_x = x;
	int cp_y = y;
	
	for (int v_x = -1; v_x <= 1; v_x++) {
	    for (int v_y = -1 ; v_y <= 1; v_y++) {

		if (v_x == 0 && v_y == 0) continue;
		
		cp_x = x;
		cp_y = y;
		cp_x += v_x;
		cp_y += v_y;
		if (cp_x < 0 || cp_x > 7 || cp_y < 0 || cp_y > 7) continue;
		if (board[cp_y,cp_x] != -1*player) continue;

		while (true) {
		    cp_x += v_x;
		    cp_y += v_y;
		    if (cp_x < 0 || cp_x > 7 || cp_y < 0 || cp_y > 7) break;
		    if (board[cp_y,cp_x] == 0) break;
		    if (board[cp_y,cp_x] == player) return true;
		}
	    }
	}
	return false;
    }
}
