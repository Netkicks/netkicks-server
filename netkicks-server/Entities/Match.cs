public class Match
{
    public int id;
    public string password;
    public Player[] players = new Player[Constants.MAX_PLAYERS_PER_MATCH];
    
    public void AddPlayerToMatch(Player incomingPlayer)
    {
        int index = Utils.GetNextEmptySlot(players);
        players[index] = incomingPlayer;
    }

    public bool isFull()
    {
        return false;
    }
}