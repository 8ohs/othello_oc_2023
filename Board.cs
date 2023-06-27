using System;
//盤のクラス

public class Board {
    private int[,] board = new int[8,8];

    public Board() {
	this.initBoard();
    }

    public Board(int[,] board) {
	this.setBoard(board);
    }

    public void battle(OthelloAIInterface p1, OthelloAIInterface p2, int n, int mode) {
	/*
	  n
	  試合回数
	  
	  mode
	  0 毎回盤面出力
	  1 最終結果のみ出力
	  2 盤面出力なし
	 */
	int win_1p = 0;
	int win_2p = 0;
	int draw = 0;
	
	for (int i = 0; i < n; i++) {
	    this.initBoard();

	    if (mode == 0) { //初期盤面
		this.showBoard();
		//board.showBoardPuttable(1);//1が置ける場所を@で表示
		Console.Write("\r\n\r\n ================= \r\n\r\n");
	    }
	    
	    do {
		if (mode == 0) {
		    Console.Write("先行({0})",p1.getName());
		    Console.WriteLine(" : ○");
		}
	    
		if (this.numOfPuttable(1) != 0) {
		    int[] te = p1.action(this.getBoard(), 1);
		    if (mode == 0) Console.WriteLine("(x,y)=({0},{1})",te[0],te[1]);
		    if ((this.putStone(te[0], te[1], 1)) == -1) this.lose(p1);
		} else if (mode == 0) this.skip(p1);

		if (mode == 0) {
		    this.showBoard();
		    //board.showBoardPuttable(-1);//-1が置ける場所を@で表示
		    Console.Write("\r\n\r\n ================= \r\n\r\n");
		    Console.Write("後攻({0})",p2.getName());
		    Console.WriteLine(" : ●");
		}
	    
		if (this.numOfPuttable(-1) != 0) {
		    int[] te = p2.action(this.getBoard(), -1);
		    if (mode == 0) Console.WriteLine("(x,y)=({0},{1})",te[0],te[1]);
		    if (this.putStone(te[0], te[1], -1) == -1) this.lose(p2);
		} else if (mode == 0) this.skip(p2);

		if (mode == 0) {
		    this.showBoard();
		    //board.showBoardPuttable(1);//1が置ける場所を@で表示
		    Console.Write("\r\n\r\n ================= \r\n\r\n");
		}
	    
	    } while (this.numOfPuttable(1) + this.numOfPuttable(-1) != 0);
	
	    if (mode <= 1) this.showResult();
	
	    if (this.numOfStone(1) > this.numOfStone(-1)) {
		win_1p++;
	    } else if (this.numOfStone(1) < this.numOfStone(-1)){
		win_2p++;
	    } else {
		draw++;
	    }
	}
	Console.WriteLine("先行○({3}):{0} 後攻●({4}):{1} draw:{2}", win_1p, win_2p, draw, p1.getName(), p2.getName());
    }

    private void lose(OthelloAIInterface p) {
	Console.WriteLine("{0}が、おけないマスに置こうとしました。", p.getName());
	Console.WriteLine("よって{0}の反則負けです", p.getName());
	Environment.Exit(0);
    }

    private void skip(OthelloAIInterface p) {
	Console.WriteLine("{0}はおけるマスがありません。", p.getName());
    }

    public int battleRandom(int[,] board, int x, int y, int player) {
	/*
	  (x,y)にplayerの石を置いてランダムに戦った結果
	  playerが負けたら1、勝ったら0を返す
	*/
	Board b = new Board(board);
	
	b.putStone(x,y,player);
	RPForMonteCarlo p1 = new RPForMonteCarlo();
	RPForMonteCarlo p2 = new RPForMonteCarlo();
	
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

    public bool isLose(int player) {
	return (this.numOfStone(player) < this.numOfStone(player*-1));
    }


    public int battleRandom(int x, int y, int player, int w) {
	/*
	  (x,y)にplayerの石を置いてランダムに戦った結果
	  playerが負けたらw、勝ったら0を返す
	*/
	Board b = new Board(this.board);
	
	b.putStone(x,y,player);
	RPForMonteCarlo p1 = new RPForMonteCarlo();
	RPForMonteCarlo p2 = new RPForMonteCarlo();
	
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

	if (b.numOfStone(player) == 0) return w;
	if (b.numOfStone(player) < b.numOfStone(player*-1)) return 1;
	else return 0;
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

    public int numOfPuttable(int player) {
	int count = 0;
	for (int x = 0; x < 8; x++) {
	    for (int y = 0; y < 8; y++) {
		if (this.isStonePuttable(x, y, player)) count++;
	    }
	}
	return count;
    }
    
    public int numOfPuttable() {
	int count = 0;
	for (int x = 0; x < 8; x++) {
	    for (int y = 0; y < 8; y++) {
		if (this.isStonePuttable(x, y, -1)) count++;
		if (this.isStonePuttable(x, y, 1)) count++;
	    }
	}
	return count;
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

    public int numOfStone() {
	//現在の盤面のplayerの石の数を返す
	int count = 0;
	for (int x = 0; x < 8; x++) {
	    for (int y = 0; y < 8; y++) {
		if (this.board[y,x] != 0) count++;
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
