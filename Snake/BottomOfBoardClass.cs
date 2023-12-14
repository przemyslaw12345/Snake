public class BottomOfBoardClass
{
    private string _bottomBoard;
    public BottomOfBoardClass(int width)
    {
        _bottomBoard = MethodToMakeBoardBottom(width);
    }

    private string MethodToMakeBoardBottom(int width)
    {
        char[] bottomChars = new char[width];
        for (int i = 1; i < width; i++)
        {
            if (i % 2 == 1)
            {
                bottomChars[i] = '-';
                //Console.WriteLine("-");
            }
            else
            {
                bottomChars[i] = ' ';
                //Console.WriteLine(" ");
            }
        }
        string returnBottomAsString = new string(bottomChars);
        return returnBottomAsString;
    }
    public string ReturningBottomOfBoard() => _bottomBoard;

}