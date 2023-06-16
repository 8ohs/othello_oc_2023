using System;

class MaekuboAI : OthelloAI, OthelloAIInterface
{
    //コンストラクタ
    public MaekuboAI(String name) : base(name) {}
    public MaekuboAI() : base() {}
	
    int[,] scoreTable = new int[8, 8]{          //ボードの評価値
    { 100, -20,  10,   5,   5,  10, -20, 100 },
    { -20, -50,  -2,  -2,  -2,  -2, -50, -20 },
    {  10,  -2,   1,   1,   1,   1,  -2,  10 },
    {   5,  -2,   1,   0,   0,   1,  -2,   5 },
    {   5,  -2,   1,   0,   0,   1,  -2,   5 },
    {  10,  -2,   1,   1,   1,   1,  -2,  10 },
    { -20, -50,  -2,  -2,  -2,  -2, -50, -20 },
    { 100, -20,  10,   5,   5,  10, -20, 100 }
    };
    
    public int[] action(int[,] board, int color)
    {
        int total = 10000;//スコアを保存
        int corX = 0;//値を返すときのx座標
        int corY = 0;//値を返すときのy座標
        int[,] borad = board;           //ボードの複製
        int[,] board2 = new int[8, 8];
        int[,] board3 = new int[8, 8];
        Array.Copy(borad, board2, board.Length);
        Array.Copy(borad, board3, board.Length);
        int[,] ans = new int[2, 64];
        int ansNum = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (canPut2(i, j, color, board))//置けるならば,配列のansNum番目にそのx,y座標格納
                {
                    ans[0, ansNum] = i;
                    ans[1, ansNum] = j;
                    ansNum++;
                }
            }
        }
        if (ansNum == 0) { return new int[] { -1, -1 }; }
        for (int a = 0; a < ansNum; a++)
        {
            int X = ans[0, a];      //置ける場所の座標を取り出し
            int Y = ans[1, a];
            Put2(X, Y, color, board2);    //疑似ボードに指定の場所に駒を置き,ひっくり返す動作もする
            int score = CalculateScore(board2, color);//更新された疑似ボードから得点を計算する
            if (total > score)         //スコアの更新,負けたいのでスコアが小さいものを探す
            {
                corX = X;
                corY = Y;
                total = score;
            }
            Array.Copy(board3, board2, board3.Length);//盤面をもう一つの疑似ボードBを用いて盤面を疑似ボードAを元に戻す
        }
        return new int[] { corX, corY };　//スコアが小さい手が返り値となる
    }
    void Put2(int x, int y, int color, int[,] board2)
    //xが盤面のx座標、yが盤面のyの座標、colorは自分の色,
    {
        if (!canPut2(x, y, color, board2)) { return; }
        board2[y, x] = color;
        //nowColor *= -1;
        //置こうとしている場所が空きじゃなければfalseを返す
        //if (board[y, x] != 0) { return; }
        //iとjで座標xとyからの方向ベクトルを表す　iとjが両方0のときは考慮しない
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i == 0 && j == 0) { continue; }
                for (int k = 1; ; k++)      //kが方向ベクトルを整数倍していく
                {
                    //参照する場所が存在しないとbreakする
                    if (y + i * k > 7 || y + i * k < 0 || x + j * k > 7 || x + j * k < 0) { break; }
                    //最初の駒が敵コマじゃなければbreakする
                    if (board2[y + i * k, x + j * k] != color * -1 && k == 1) { break; }
                    //最初の駒以降に自分と同じ駒を見つけるとtrue(置ける)
                    if (k != 1 && board2[y + i * k, x + j * k] == color) { reverse2(x, y, j, i, color, board2); }
                    //自分の駒が来る前に空白のマスに止まるとbreakする
                    if (k != 1 && board2[y + i * k, x + j * k] == 0) { break; }
                }
            }
        }
    }
    void reverse2(int x, int y, int j, int i, int color, int[,] board2)
    {
        x += j;
        y += i;
        while (board2[y, x] != color)
        {
            board2[y, x] *= -1;
            x += j;
            y += i;
        }
    }
    int CalculateScore(int[,] board2, int color)
    {
        int fscore = 0;
        // 得点テーブルを使用して盤面の評価値を計算する
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (board2[i, j] == color)
                {
                    fscore += scoreTable[i, j];
                }
                else if (board2[i, j] == color * -1)
                {
                    fscore -= scoreTable[i, j];
                }
            }
        }
        return fscore;
    }
    bool canPut2(int x, int y, int color, int[,] board)
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
}
