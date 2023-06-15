using System;
/*
  プレイヤー1:  1 : ○
  プレイヤー2: -1 : ●
*/
public class OthelloMain {
    static public void Main() {
	Board board = new Board();
	
	//Console.WriteLine("ボード(初期盤面)");
	//board.showBoard();

	//Human h_p1 = new Human("最弱オセロ");
	//Human h_p2 = new Human();
	RandomPlayer r_p1 = new RandomPlayer("ランダムくん");
	//RandomPlayer r_p2 = new RandomPlayer("ランダム2");
	MonteCarloPlayer m_p1 = new MonteCarloPlayer("もんちゃん");
	//MonteCarloPlayer m_p2 = new MonteCarloPlayer("もんちゃん2");
	//MinPlayer min_p1 = new MinPlayer("Min君1");
	//MinPlayer min_p2 = new MinPlayer("Min君2");
	//IshizukaAI i_p = new IshizukaAI("まなるん");

	battle(board, m_p1, r_p1, 10, 0);
	//battle(board, min_p2, min_p1, 1000, 2);
    }

    static void battle(Board board, OthelloAIInterface p1, OthelloAIInterface p2, int n, int mode) {
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
	    board.initBoard();

	    if (mode == 0) { //初期盤面
		board.showBoard();
		//board.showBoardPuttable(1);//1が置ける場所を@で表示
		Console.Write("\r\n\r\n ================= \r\n\r\n");
	    }
	    
	    do {
		if (mode == 0) {
		    Console.Write("先行({0})",p1.getName());
		    Console.WriteLine(" : ○");
		}
	    
		if (board.numOfPuttable(1) != 0) {
		    int[] te = p1.action(board.getBoard(), 1);
		    if (mode == 0) Console.WriteLine("(x,y)=({0},{1})",te[0],te[1]);
		    if ((board.putStone(te[0], te[1], 1)) == -1) lose(p1);
		} else if (mode == 0) skip(p1);

		if (mode == 0) {
		    board.showBoard();
		    Console.Write("\r\n\r\n ================= \r\n\r\n");
		    Console.Write("後攻({0})",p2.getName());
		    Console.WriteLine(" : ●");
		}
	    
		if (board.numOfPuttable(-1) != 0) {
		    int[] te = p2.action(board.getBoard(), -1);
		    if (mode == 0) Console.WriteLine("(x,y)=({0},{1})",te[0],te[1]);
		    if (board.putStone(te[0], te[1], -1) == -1) lose(p2);
		} else if (mode == 0) skip(p2);

		if (mode == 0) {
		    board.showBoard();
		    //board.showBoardPuttable(1);//1が置ける場所を@で表示
		    Console.Write("\r\n\r\n ================= \r\n\r\n");
		}
	    
	    } while (board.numOfPuttable(1) + board.numOfPuttable(-1) != 0);
	
	    if (mode <= 1) board.showResult();
	
	    if (board.numOfStone(1) > board.numOfStone(-1)) {
		win_1p++;
	    } else if (board.numOfStone(1) < board.numOfStone(-1)){
		win_2p++;
	    } else {
		draw++;
	    }
	}
	Console.WriteLine("先行({3}):{0} 後攻({4}):{1} draw:{2}", win_1p, win_2p, draw, p1.getName(), p2.getName());
    }

    static void lose(OthelloAIInterface p) {
	Console.WriteLine("{0}が、おけないマスに置こうとしました。", p.getName());
	Console.WriteLine("よって{0}の反則負けです", p.getName());
	Environment.Exit(0);
    }

    static void skip(OthelloAIInterface p) {
	Console.WriteLine("{0}はおけるマスがありません。", p.getName());
    }
}
