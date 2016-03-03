public class Player
{


    public string companyName;

    private int cash;
    public int Cash { get { return cash; } }

    private int reputation = 0;
    public int Reputation { get { return reputation; } }

    private int lastDayWithCash = 0;

    public Player(string companyName)
    {
        this.companyName = companyName;
        this.cash = Settings.Player_StartCash;
    }

    public void ChangeCash(int deltaValue)
    {
        cash += deltaValue;
    }

    public void ChangeReputation(int deltaValue)
    {
        reputation += deltaValue;
    }

    public bool CheckIfLost(int currentDay)
    {
        if (cash > 0)
        {
            lastDayWithCash = currentDay;
            return false;
        }
        else
            return currentDay - lastDayWithCash > Settings.Player_DaysWithoutCashLoseCondition;
    }

}
