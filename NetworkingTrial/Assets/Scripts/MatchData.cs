public struct MatchData
{
    public PlayerNetwork Player1, Player2;
    public CardType Player1Card, Player2Card;

    public MatchData(PlayerNetwork player1, PlayerNetwork player2, CardType player1Card, CardType player2Card)
    {
        Player1 = player1;
        Player2 = player2;
        Player1Card = player1Card;
        Player2Card = player2Card;
    }
}
