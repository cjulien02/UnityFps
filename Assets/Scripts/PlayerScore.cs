
public class PlayerScore{
    private int frags = 0;

    public int GetFrags()
    {
        return frags;
    }

    public void IncrementFrags()
    {
        frags++;
    }
    
    private int deaths = 0;

    public int GetDeaths()
    {
        return deaths;
    }

    public void IncrementDeaths()
    {
        deaths++;
    }
    
}
