[![dotnet package](https://github.com/8ohs/othello_oc_2023/actions/workflows/dotnet-package.yml/badge.svg)](https://github.com/8ohs/othello_oc_2023/actions/workflows/dotnet-package.yml)
- `OthelloMain.cs`
  - Mainメソッド  
- `OthelloAIInterface.cs`
  - OthelloAIのインターフェイス  
- `OthelloAI.cs`
  - OthelloAIの基底クラス  
- `Board.cs`
  - 盤面のクラス  
- `Human.cs`
  - すべて手入力のOthelloAI  
- `RandomPlayer.cs`
  - すべてランダムに打つOthelloAI  
- `MinPlayer.cs`
  - 合法手の中で打てる数が1番少ない且つ１番早く見つけた手を打つOthelloAI  
- `MonteCarloPlayer.cs`
  - 原始的モンテカルロ
- `MonteCarloPlayer2.cs`
  - ボロ負けし易い手を選びやすい原始的モンテカルロ
- `MonteCarloPlayer3.cs`
  - ボロ負けし易い手を選びやすくて,角を選びにくくて,最後は読み切る原始的モンテカルロ
- `NegaMaxPlayer.cs`
  - 簡易NegaMaxのつもり