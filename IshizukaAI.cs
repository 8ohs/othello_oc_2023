using System;

class IshizukaAI : OthelloAI, OthelloAIInterface
{
    public IshizukaAI(String name) : base(name) {}
    public IshizukaAI() : base() {}
    
    public int[] action(int[,] board, int color)
    {
        System.Random r1 = new System.Random();
	Board b = new Board();
	b.setBoard(board);

	int[,] gouhoute = b.getGouhoute(color);
	
        int[,] ans = new int[2, 64];
        int ansNum = 0;
        int maxReversals = (color == 1) ? int.MinValue : int.MaxValue; // ターン数に応じた最大ひっくり返し数
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (canPut(i, j, color, board))
                {
                    int reversals = countReversals(i, j, color, board);
                    if ((color == 1 && reversals > maxReversals) || (color == -1 && reversals < maxReversals))
                    {
                        //ansNum = 0;
                        ans[0, ansNum] = i;
                        ans[1, ansNum] = j;
                        maxReversals = reversals;
                    }
                    else if (reversals == maxReversals)
                    {
                        ans[0, ansNum] = i;
                        ans[1, ansNum] = j;
                    }
                    ansNum++;
                }
            }
        }

	//printAns(gouhoute, ansNum);
	printAns2(ans, ansNum);
	    
        if (ansNum == 0) { return new int[] { -1, -1 }; }
        int ax = r1.Next(0, ansNum);
        return new int[] { ans[0, ax], ans[1, ax] };
    }

    void printAns(int[,] ans, int ansNum) {
	Console.WriteLine("ansNum={0}",ans.GetLength(0));
	for (int i = 0; i < ans.GetLength(0); i++) {
	    Console.WriteLine("[x,y]=[{0},{1}]",ans[i,0], ans[i,1]);
	}
    }

    void printAns2(int[,] ans, int ansNum) {
	Console.WriteLine("ansNum={0}", ansNum);
	for (int i = 0; i < ans.GetLength(1); i++) {
	    Console.WriteLine("[x,y]=[{0},{1}]",ans[0,i], ans[1,i]);
	}
    }

    
    bool canPut(int x, int y, int color, int[,] board)
    {
        if (x > 7 || x < 0 || y > 7 || y < 0)
        {
            return false;
        }
        if (board[y, x] != 0) { return false; }
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i == 0 && j == 0) { continue; }
                for (int k = 1; ; k++)
                {
                    if (y + i * k > 7 || y + i * k < 0 || x + j * k > 7 || x + j * k < 0) { break; }
                    if (board[y + i * k, x + j * k] != color * -1 && k == 1) { break; }
                    if (k != 1 && board[y + i * k, x + j * k] == color) { return true; }
                    if (k != 1 && board[y + i * k, x + j * k] == 0) { break; }
                }
            }
        }
        return false;
    }
    //指定された位置の駒がひっくり返される数を数える
    int countReversals(int x, int y, int color, int[,] board)
    {
        int reversals = 0;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i == 0 && j == 0) { continue; }
                for (int k = 1; ; k++)
                {
                    if (y + i * k > 7 || y + i * k < 0 || x + j * k > 7 || x + j * k < 0) { break; }
                    if (board[y + i * k, x + j * k] != color * -1 && k == 1) { break; }
                    if (k != 1 && board[y + i * k, x + j * k] == color)
                    {
                        reversals += k - 1;
                        break;
                    }
                    if (k != 1 && board[y + i * k, x + j * k] == 0) { break; }
                }
            }
        }
        return reversals;
    }
}
