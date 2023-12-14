using static System.Formats.Asn1.AsnWriter;

class ScoreMethod
{
    private int localScore;
    public ScoreMethod(int score)
    {
        PrintScore(score);
        
    }   
    public int UpdateScore(int score)
    {
        localScore = score + 10;
        PrintScore(localScore);
        return localScore;
    }
    private void PrintScore(int score)
    {
        Console.SetCursorPosition(0, 0);
        Console.WriteLine($"Score: {score}");
    }

    public int ReturnScore()
    {
        return localScore;
    }
}

