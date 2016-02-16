public class Player
{

    public string companyName;

    int cash = 0;
    public int Cash { get { return cash; } }

    int reputation = 0;
    public int Reputation { get { return reputation; } }

    public Player(string companyName)
    {
        this.companyName = companyName;
    }

    public void ChangeCash(int deltaValue)
    {
        cash += deltaValue;
    }

}
