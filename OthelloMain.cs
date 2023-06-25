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
	//RandomPlayer r_p1 = new RandomPlayer("ランダムくん");
	//RandomPlayer r_p2 = new RandomPlayer("ランダム2");
	//MonteCarloPlayer m_p1 = new MonteCarloPlayer("もんちゃん1000", 1000);
	//MonteCarloPlayer2 m2_p1 = new MonteCarloPlayer2("もんちゃん2_100", 100);
	//MonteCarloPlayer2 m2_p2 = new MonteCarloPlayer2("もんちゃん2_1000_20", 1000, 20);
	MonteCarloPlayer3 m3_p1 = new MonteCarloPlayer3("モン君3_1000", 1000);
	MonteCarloPlayer3 m3_p2 = new MonteCarloPlayer3("モン様3_1000", 1000);
	//MonteCarloPlayer m_p2 = new MonteCarloPlayer("もんちゃん50", 50);
	//MinPlayer min_p1 = new MinPlayer("Min君1");
	//MinPlayer min_p2 = new MinPlayer("Min君2");
	//CustomMon cm_p1 = new CustomMon("石塚AI");
	//MaekuboAI mae_p1 = new MaekuboAI("前久保AI");
	//BannoAI b_p1 = new BannoAI("バンノAI");

	board.battle(m3_p1, m3_p2, 1, 0);
	/*
	  battle(p1, p2, n, mode);
	  n
	  試合回数
	  
	  mode
	  0 毎回盤面出力
	  1 最終結果のみ出力
	  2 盤面出力なし
	 */
    }
}
