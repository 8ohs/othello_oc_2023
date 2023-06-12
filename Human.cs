using System;
//手入力

public class Human : OthelloAI, OthelloAIInterface {

    public Human() : base() {}
    
    public Human(String name) : base(name) {
    }
    
    public int[] action(int[,] board, int player) {
	Console.Write("x y :  ");
	String[] input = Console.ReadLine().Split(' ');
	int[] ans = new int[2];
	ans[0] = int.Parse(input[0]);
	ans[1] = int.Parse(input[1]);

	return ans;
    }
}
