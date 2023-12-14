public class LeftOfBoardClass
{
    private char[] _leftBoard;
    public LeftOfBoardClass(int height)
    {
        _leftBoard = MethodToMakeLeftSide(height);
    }

    private char[] MethodToMakeLeftSide(int height)
    {
        char[] leftChars = new char[height];
        for (int i = 1; i < height; i++)
        {            
                leftChars[i] = '|';
        }
        //string returnLeftAsString = new string(leftChars);
        return leftChars;
    }
    public char[] ReturningLeftOfBoard() => _leftBoard;
}