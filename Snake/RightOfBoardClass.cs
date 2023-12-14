internal class RightOfBoardClass
{
    private char[] _rightBoard;
    public RightOfBoardClass(int height)
    {
        _rightBoard = MethodToMakeRightSide(height);
    }

    private char[] MethodToMakeRightSide(int height)
    {
        char[] rightChars = new char[height];
        for (int i = 1; i < height; i++)
        {
            rightChars[i] = '|';
        }
        //string returnLeftAsString = new string(leftChars);
        return rightChars;
    }
    public char[] ReturningRightOfBoard() => _rightBoard;
}