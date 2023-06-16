using System;
//盤のクラス

public class Board {
    private int[,] board = new int[8,8];

    public Board() {
	this.initBoard();
    }

    public int[,] getGouhoute(int player) {
	//合法手の配列を返す
	int[,] gouhoute = new int[60,2];
	int count = 0;
	for (int x = 0; x < 8; x++) {
	    for (int y = 0; y < 8; y++) {
		if (isStonePuttable(x, y, player)) {
		    gouhoute[count,0] = x;
		    gouhoute[count,1] = y;
		    count++;
		}
	    }
	}
	int[,] ans = new int[count,2];
	for (int i = 0; i < count; i++) {
	    ans[i,0] = gouhoute[i,0];
	    ans[i,1] = gouhoute[i,1];
	}

	return ans;
    }

    public void setBoard(int[,] board) {
	//盤面をセットする
	for (int x = 0; x < 8; x++) {
	    for (int y = 0; y < 8; y++) {
		this.board[y,x] = board[y,x];
	    }
	}
    }

    public int[,] getBoard() {
	//盤面のコピーを返す
	int[,] copyBoard = new int[8,8];
	for (int x = 0; x < 8; x++) {
	    for (int y = 0; y < 8; y++) {
		copyBoard[y,x] = this.board[y,x];
	    }
	}
	return copyBoard;
    }
    
    public void showResult() {
	Console.WriteLine("Game Set!!!");
	Console.WriteLine("Result");
	
	this.showBoard();
	Console.WriteLine("○:{0}   ●:{1}", this.numOfStone(1), this.numOfStone(-1));
    }

    public int numOfStone(int player) {
	//現在の盤面のplayerの石の数を返す
	int count = 0;
	for (int x = 0; x < 8; x++) {
	    for (int y = 0; y < 8; y++) {
		if (this.board[y,x] == player) count++;
	    }
	}
	return count;
    }

    public int numOfPuttable(int player) {
	//playerがおける石の数を返す
	int count = 0;
	for (int x = 0; x < 8; x++) {
	    for (int y = 0; y < 8; y++) {
		if (isStonePuttable(x, y, player)) {
		    count++;
		}
	    }
	}
	return count;
    }

    public void showBoardPuttable(int player) {
	//boardでplayerがおける場所をプリント
	int[,] board_2 = new int[8,8];
	for (int x = 0; x < 8; x++) {
	    for (int y = 0; y < 8; y++) {
		board_2[y,x] += this.board[y,x];
		if (this.isStonePuttable(x, y, player)) {
		    board_2[y,x] = 2;
		}
	    }
	}
	this.showBoard(board_2);
    }

    public int putStone(int x, int y, int player) {
	//boardの(x,y)にplayerの石をおく(返り値はひっくり返した数)
	if (!this.isStonePuttable(x, y, player)) return -1;
	this.board[y,x] = player;
	
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
		if (this.board[cp_y,cp_x] != -1*player) continue;

		bool settable = false;
		
		while (true) {
		    cp_x += v_x;
		    cp_y += v_y;
		    if (cp_x < 0 || cp_x > 7 || cp_y < 0 || cp_y > 7) break;
		    if (this.board[cp_y,cp_x] == 0) break;
		    if (this.board[cp_y,cp_x] == player) {
			settable = true;
			break;
		    }
		}
		
		if (settable) { //ひっくり返せる方向のとき
		    cp_x = x + v_x;
		    cp_y = y + v_y;
		    do {
			count++;
			this.board[cp_y,cp_x] *= -1;
			cp_x += v_x;
			cp_y += v_y;
		    } while (this.board[cp_y,cp_x] != player);
		}
	    }
	}
	return count;
    }

    public bool isStonePuttable(int x, int y, int player) {
	//boardの(x,y)にplayerの石が置けるときtrueを返す
	//範囲外
	if (x < 0 || x > 7 || y < 0 || y > 7) return false;
	//すでに置かれている 
	if (this.board[y,x] != 0) return false;

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
		if (this.board[cp_y,cp_x] != -1*player) continue;

		while (true) {
		    cp_x += v_x;
		    cp_y += v_y;
		    if (cp_x < 0 || cp_x > 7 || cp_y < 0 || cp_y > 7) break;
		    if (this.board[cp_y,cp_x] == 0) break;
		    if (this.board[cp_y,cp_x] == player) return true;
		}
	    }
	}
	return false;
    }
    
    public void initBoard() {
	//ボードを初期化
	for (int x = 0; x < 8; x++) {
	    for (int y = 0; y < 8; y++) {
		this.board[y,x] = 0;
	    }
	}
	this.board[3,4] = 1;
	this.board[4,3] = 1;
	this.board[4,4] = -1;
	this.board[3,3] = -1;
    }

    public void showBoard() {
	//ボードを表示
	Console.WriteLine(" 01234567");
	for (int y = 0; y < 8; y++) {
	    Console.Write(y);
	    for (int x = 0; x < 8; x++) {
		switch (this.board[y,x]) {
		    case 0:
			Console.Write("*");
			break;
		    case 1:
		        Console.Write("○");
		        break;
		    case -1:
			Console.Write("●");
			break;
		    case 2:
			Console.Write("@");
			break;
		    default:
			break;
		}
	    }
	    Console.WriteLine();
	}
    }

    public void showBoard(int[,] board) {
	//ボードを表示
	Console.WriteLine(" 01234567");
	for (int y = 0; y < 8; y++) {
	    Console.Write(y);
	    for (int x = 0; x < 8; x++) {
		switch (board[y,x]) {
		    case 0:
			Console.Write("*");
			break;
		    case 1:
		        Console.Write("○");
		        break;
		    case -1:
			Console.Write("●");
			break;
		    case 2:
			Console.Write("@");
			break;
		    default:
			break;
		}
	    }
	    Console.WriteLine();
	}
    }
}
