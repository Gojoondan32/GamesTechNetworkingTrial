public struct MatchData
{
    public PlayerSelection Player1, Player2;
    public CardType Player1Card, Player2Card;

    public MatchData(PlayerSelection player1, PlayerSelection player2, CardType player1Card, CardType player2Card)
    {
        Player1 = player1;
        Player2 = player2;
        Player1Card = player1Card;
        Player2Card = player2Card;
    }
}
