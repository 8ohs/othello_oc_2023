using System;
//一番早く見つけた一番返せる石が小さい所を選び続ける

public class MinPlayer : OthelloAI, OthelloAIInterface {
    public MinPlayer() : base() {}
    public MinPlayer(String name) : base(name) {}
    
    public int[] action(int[,] board, int player) {
	int[,] gouhoute = new int[60,3];//合法手列挙[見つけた順番,x,y,ひっくり返せる個数]
	int count = 0;//おける個数
	int te_ans_num = 100;
	int te_ans = 0;

	for (int x = 0; x < 8; x++) {
	    for (int y = 0; y < 8; y++) {
		if (isStonePuttable(board, x, y, player)) {
		    gouhoute[count,0] = x;
		    gouhoute[count,1] = y;
		    gouhoute[count,2] = numOfReverseStone(board, x, y, player);
		    count++;
		}
	    }
	}

	for (int i = 0; i < count; i++) {
	    if (gouhoute[i,2] < te_ans_num) {
		te_ans_num = gouhoute[i,2];
		te_ans = i;
	    }
	}

	int[] ans = new int[2];
	ans[0] = gouhoute[te_ans,0];
	ans[1] = gouhoute[te_ans,1];

	return ans;
    }

    public bool isStonePuttable(int[,] board, int x, int y, int player) {
	//boardの(x,y)にplayerの石が置けるときtrueを返す
	//範囲外
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

    public int numOfReverseStone(int[,] board, int x, int y, int player) {
	//boardの(x,y)にplayerの石を置いたときにひっくり返せる数を返す
	if (isStonePuttable(board, x, y, player)) return -1;
	board[y,x] = player;
	
	int count = 0;
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

		bool settable = false;
		
		while (true) {
		    cp_x += v_x;
		    cp_y += v_y;
		    if (cp_x < 0 || cp_x > 7 || cp_y < 0 || cp_y > 7) break;
		    if (board[cp_y,cp_x] == 0) break;
		    if (board[cp_y,cp_x] == player) {
			settable = true;
			break;
		    }
		}
		
		if (settable) { //ひっくり返せる方向のとき
		    cp_x = x + v_x;
		    cp_y = y + v_y;
		    do {
			count++;
			//board[cp_y,cp_x] *= -1; ひっくり返す
			cp_x += v_x;
			cp_y += v_y;
		    } while (board[cp_y,cp_x] != player);
		}
	    }
	}
	return count;
    }
}
