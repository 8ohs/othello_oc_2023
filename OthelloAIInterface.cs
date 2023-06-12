using System;

public interface OthelloAIInterface {
    int[] action(int[,] board, int player);
    String getName();
}
