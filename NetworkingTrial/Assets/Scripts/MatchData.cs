public class MatchData
{
    public Player Player1, Player2; //This should only ever be used as a callback
    public CardType Player1Card, Player2Card;

    private bool _isReady;
    public bool IsReady{get{return _isReady;}}

    public int Counter;

    public MatchData()
    {
        Reset();
    }

    public void AddMatchData(Player player, CardType cardType){
        if(Player1 == null){
            Player1 = player;
            Player1Card = cardType;
        }
        else{
            Player2 = player;
            Player2Card = cardType;
            _isReady = true;
        }
    }

    public void Reset(){
        Player1 = null;
        Player2 = null;
        _isReady = false;
        Counter = 0;
    }
}
