public class TopOfBoardClass
{
    private string _boardTop;
    public TopOfBoardClass(int width)
    {
        _boardTop = MethodToMakeBoardTop(width);
    }
    private string MethodToMakeBoardTop(int width)
    {
        char[] topChars = new char[width];
        for (int i = 1; i < width; i++)
        {
            if (i % 2 == 1)
            {
                topChars[i] = '-';
                //Console.WriteLine("-");
            }
            else
            {
                topChars[i] = ' ';
                //Console.WriteLine(" ");
            }
        }
        string returnTopAsString = new string(topChars);
        return returnTopAsString;
    }

    public string ReturningTopOfBoard() => _boardTop;
}
