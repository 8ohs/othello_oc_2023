using System;

public class CustomMon : OthelloAI, OthelloAIInterface
{
    public CustomMon(String name) : base(name) { } //コンストラクタ
    public CustomMon() : base() { }

    private const int MAX_TRY_NUM = 500;
    //	private const int kaisu  = MAX_TRY_NUM / len;

    public int[] action(int[,] board, int player)
    {
	Board b = new Board();
	//Random rand = new Random();
	RandomPlayer p1 = new RandomPlayer();
	RandomPlayer p2 = new RandomPlayer();

	b.setBoard(board);
	int[,] gouhoute = b.getGouhoute(player);
	int len = gouhoute.GetLength(0);

	int[] loseNum = new int[len];
	int kaisu = MAX_TRY_NUM / len;
	int ansIndex = 0;
	double maxLoseRate = 0;
	for(int i = 0; i < len; i++) {
	    //int index = rand.Next(0, len);
	    for (int j = 0; j < kaisu; j++)
	    {		
		loseNum[i] += battleResult(b, gouhoute[i, 0], gouhoute[i, 1], p1, p2, player);



		//Console.Write("\r({0}/{1})\r", i, MAX_TRY_NUM); //プレイアウトの状況を表示
		/*
		  int index = rand.Next(0, len);
		  tryNum[index]++;
		  loseNum[index] += battleResult(b, gouhoute[index, 0], gouhoute[index, 1], p1, p2, player);
		*/
	    }
	}


	for (int i = 0; i < len; i++)
	{
	    //if (tryNum[i] == 0) continue;	    
	    if ((double)(loseNum[i]) / (double)(kaisu) > maxLoseRate)
	    {
		maxLoseRate = (double)(loseNum[i]) / (double)(kaisu);
		ansIndex = i;
	    }
	}

	int[] ans = new int[2];
	ans[0] = gouhoute[ansIndex, 0];
	ans[1] = gouhoute[ansIndex, 1];
	return ans;
    }

    static int battleResult(Board board, int x, int y, OthelloAIInterface p1, OthelloAIInterface p2, int player)
    {
	//負けたら1かったら0を返す
	Board b = new Board();
	b.setBoard(board.getBoard());
	b.putStone(x, y, player);

	do
	{
	    if (b.numOfPuttable(player * -1) != 0)
	    {
		int[] te = p1.action(b.getBoard(), player * -1);
		b.putStone(te[0], te[1], player * -1);
	    }

	    if (b.numOfPuttable(player) != 0)
	    {
		int[] te = p2.action(b.getBoard(), player);
		b.putStone(te[0], te[1], player);
	    }

	} while (b.numOfPuttable(1) + b.numOfPuttable(-1) != 0);


	if (b.numOfStone(player) < b.numOfStone(player * -1)) return 1;
	else return 0;
    }
}
