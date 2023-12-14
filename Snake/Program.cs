
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using System.Text.Json;

Console.CursorVisible = false;
const string highScoreFileName = "HighScoreFile.json";
List<HighScore> highScoreList = new List<HighScore>();

if (!File.Exists(highScoreFileName))
{
    highScoreList.Add(new HighScore
    { Score = 0, PlayerName = "n/a" }
    );
    highScoreList.Add(new HighScore
    { Score = 0, PlayerName = "n/a" }
        );
    highScoreList.Add(new HighScore
    { Score = 0, PlayerName = "n/a" }
        );
    highScoreList.Add(new HighScore
    { Score = 0, PlayerName = "n/a" }
        );
    highScoreList.Add(new HighScore
    { Score = 0, PlayerName = "n/a" }
        );
}else
{
    string jsonHighscore = File.ReadAllText(highScoreFileName);
    highScoreList = JsonSerializer.Deserialize<List<HighScore>>(jsonHighscore);
}
highScoreList.Sort((h, h2) => {
    if (h.Score == h2.Score)
    {
        return 0;
    }
    if (h.Score > h2.Score)
    {
        return -1;
    }
    if (h.Score < h2.Score)
    {
        return 1;
    }
    else
    {
        return 0;
    }
});

await MainMenu(highScoreList).ConfigureAwait(false);
Console.ReadKey();


//---------------------------------------------------------------------------------------------------


async Task MainMenu(List<HighScore> highScoreList)
{
    
    bool isTrue = true;
    while(isTrue)
    {
        Console.Clear();
        string menuSelecter = "0";
        Console.WriteLine(
            $"Snake by Przemek{Environment.NewLine}" +
            $"Please select from the follwing options: {Environment.NewLine}" +
            $"1. Play{Environment.NewLine}" +
            $"2. High Scores{Environment.NewLine}" +
            $"3. Quit");
        menuSelecter = Console.ReadLine();
        isTrue = await MethodSelectingOptionsFromMenu(isTrue, menuSelecter, highScoreList).ConfigureAwait(false);
    }
}
async Task<bool> MethodSelectingOptionsFromMenu(bool isTrue, string menuSelecter, List<HighScore> highScoreList)
{
    switch (menuSelecter)
    {
        case "1":
            await PlayingTheGame(highScoreList).ConfigureAwait(false);
            isTrue = false;
            break;
        case "2":
            await CheckHighScoreMethodAsync(highScoreList);
            break;
        case "3":
            Console.WriteLine(
                $"Thank you for playing. {Environment.NewLine}" +
                $"Press any key to exit application.");
            isTrue = false;
            break;
        default:
            Console.WriteLine("Incorrect input, please try again.");
            Console.ReadKey();
            break;
    }

    return isTrue;
}
async Task PlayingTheGame(List<HighScore> highScoreList)
{
    
    int width = SelectBoardSize();
    int height = width/3;
    CreateBoard(width, height);
    await Play(width, height, highScoreList).ConfigureAwait(false);
}
int SelectBoardSize()
{
    bool isBoardSelected = false;
    int boardWidth = 0;
    while (!isBoardSelected)
    {
        Console.Clear();
        
        string selecterForBoard = "0";
        Console.WriteLine(
                 $"Snake by Przemek{Environment.NewLine} " +
                 $"Please select desired board size: {Environment.NewLine}" +
                 $"1. Small{Environment.NewLine}" +
                 $"2. Medium{Environment.NewLine}" +
                 $"3. Large");
        selecterForBoard = Console.ReadLine();
        switch (selecterForBoard)
        {
            case "1":
                boardWidth = 45;
                isBoardSelected = true;
                break;
            case "2":
                boardWidth = 60;
                isBoardSelected = true;
                break;
            case "3":
                boardWidth = 75;
                isBoardSelected = true;
                break;
            default:
                Console.WriteLine("Incorrect input, please try again.");
                break;
        }
    }
    return boardWidth;
}
void CreateBoard(int width, int height)
{
    
    Console.Clear();

    TopOfBoardClass boardTop = new TopOfBoardClass(width);
    Console.SetCursorPosition(1, 1);
    Console.WriteLine(boardTop.ReturningTopOfBoard());

    BottomOfBoardClass boardBottom = new BottomOfBoardClass(width);
    Console.SetCursorPosition(1, height+1);
    Console.WriteLine(boardBottom.ReturningBottomOfBoard());

    LeftOfBoardClass boardLeft = new LeftOfBoardClass(height);
    for (int i = 0; i < boardLeft.ReturningLeftOfBoard().Length; i++)
    {
        Console.SetCursorPosition(1, i+1 );
        Console.WriteLine(boardLeft.ReturningLeftOfBoard()[i]);
    }

    RightOfBoardClass boardRight = new RightOfBoardClass(height);
    for (int i = 0; i < boardRight.ReturningRightOfBoard().Length; i++)
    {
        Console.SetCursorPosition(width, i+1);
        Console.WriteLine(boardRight.ReturningRightOfBoard()[i]);
    }
}
async Task Play(int width, int height, List<HighScore> highScoreList)
{

    int initialX = width / 2;
    int initialY = height / 3;
    int fruitXPosition = 10,
        fruitYPosition = 10;
    int score = 0;
    bool isRunning = true;
    bool addNewBodySegment = false;
    bool isFruitSpawned = false;


    ScoreMethod newScore = new ScoreMethod(score);

    Console.SetCursorPosition(initialX, initialY);
    int currentX = initialX;
    int currentY = initialY;


    char noInputForKey = 'd';
    List<Point> SnakeLocation = SnakeLocationMethod(currentX, currentY);

    while (isRunning)
    {

        addNewBodySegment = false;
        SpawnFruit newFruit;
        newFruit = NewFruitSpawner(width, height, ref fruitXPosition, ref fruitYPosition, ref isFruitSpawned, SnakeLocation);

        char key = KeyInput(noInputForKey);
        noInputForKey = key;
        SnakeDirectionMethod(ref currentX, ref currentY, key);

        SnakeLocation.Add(new Point(currentX, currentY));
        SnakePositioningMethod(SnakeLocation);

        if (SnakeLocation[SnakeLocation.Count - 1].X == fruitXPosition && SnakeLocation[SnakeLocation.Count - 1].Y == fruitYPosition)
        {
            addNewBodySegment = true;
            isFruitSpawned = false;
            score = newScore.UpdateScore(score);

        }

        RemovingExcessBodySegmentsMethod(addNewBodySegment, SnakeLocation);

        isRunning = CheckIfSnakePositionIsValid(SnakeLocation, width, height);

        await WaitingTask().ConfigureAwait(false);
        await GameOverChecker(highScoreList, isRunning, newScore);
    }


}
async Task GameOverChecker(List<HighScore> highScoreList, bool isRunning, ScoreMethod newScore)
{
    if (!isRunning)
    {
        await HighScoreListAsync(newScore.ReturnScore(), highScoreList);
    }
}
bool CheckIfSnakePositionIsValid(List<Point> SnakeLocation, int width, int height)
{
    bool isSnakeTouchingItself = false;
    for (int i = 0; i < SnakeLocation.Count-1; i++)
    {
        if (SnakeLocation[SnakeLocation.Count - 1].X == SnakeLocation[i].X && SnakeLocation[SnakeLocation.Count - 1].Y == SnakeLocation[i].Y)
        {
            isSnakeTouchingItself = true;
            break;
        }
        else
        {
            isSnakeTouchingItself = false;
        }
    }
    
    if (SnakeLocation[SnakeLocation.Count - 1].X == 1 || SnakeLocation[SnakeLocation.Count - 1].Y == 1 || 
            SnakeLocation[SnakeLocation.Count - 1].X == width || SnakeLocation[SnakeLocation.Count - 1].Y == height+1 || isSnakeTouchingItself)
    {

        return false;
    }
    else
    {
        return true;
    }
    
}
char KeyInput(char noInputForKey)
{
    char keyReturned = noInputForKey;
    if (Console.KeyAvailable) {
        var keyInput = Console.ReadKey(true);

        keyReturned = keyInput.KeyChar;
        return keyReturned ;
    }
    return keyReturned;
}
static async Task WaitingTask()
{    
    await Task.Delay(250); 
}
 async Task CheckHighScoreMethodAsync(List<HighScore> highScoreList)
{
    Console.Clear();
    for (int i = 0; i<highScoreList.Count; i++)
    {
        Console.WriteLine($"{i}  Score: {highScoreList[i].Score}  Player Name: {highScoreList[i].PlayerName}");
    }

    
    //Console.WriteLine("Will make high score in future");
    Console.ReadKey();
    await MainMenu(highScoreList);
}
async Task HighScoreListAsync(int returnedScore, List<HighScore> highScoreList)
{
    Console.Clear();

    // sorting compilator for the list of highscores
    highScoreList.Sort((h, h2) => {
        if (h.Score == h2.Score)
        {
            return 0;
        }
        if (h.Score > h2.Score)
        {
            return -1;
        }
        if (h.Score < h2.Score)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    });

    for (int i = 0; i < highScoreList.Count; i++)
    {
        if (highScoreList[i].Score < returnedScore)
        {
            highScoreList[i].Score = returnedScore;
            Console.WriteLine($"Congratulations you have reached a high score of {returnedScore}! Please enter your name so we may remember you for the ages: ");
            highScoreList[i].PlayerName = Console.ReadLine();
            break;
        }
    }
    
    var jsonHighscore = JsonSerializer.Serialize(highScoreList);
    await File.WriteAllTextAsync(highScoreFileName, jsonHighscore);


    Console.WriteLine("Press any key to return to Main Menu");
    await MainMenu(highScoreList).ConfigureAwait(false);
    Console.ReadKey();    
    

}
static List<Point> SnakeLocationMethod(int currentX, int currentY)
{
    List<Point> SnakeLocation = new List<Point>();
    SnakeLocation.Add(new Point(currentX - 2, currentY));
    SnakeLocation.Add(new Point(currentX - 1, currentY));
    SnakeLocation.Add(new Point(currentX, currentY));
    return SnakeLocation;
}
static SpawnFruit NewFruitSpawner(int width, int height, ref int fruitXPosition, ref int fruitYPosition, ref bool isFruitSpawned, List<Point> SnakeLocation)
{
    SpawnFruit newFruit;
    if (!isFruitSpawned)
    {
        newFruit = new SpawnFruit(SnakeLocation, width, height);
        fruitXPosition = newFruit.GetXAxis();
        fruitYPosition = newFruit.GetYAxis();
        isFruitSpawned = true;
        return newFruit;
    }
    else
    {
        return null;
    }

    
}
static void SnakeDirectionMethod(ref int currentX, ref int currentY, char key)
{
    switch (key)
    {
        case 'w':
            currentY--;
            break;
        case 'a':
            currentX--;
            break;
        case 's':
            currentY++;
            break;
        case 'd':
            currentX++;
            break;
        default:
            break;
    }
}
static void SnakePositioningMethod(List<Point> SnakeLocation)
{
    foreach (Point point in SnakeLocation)
    {
        Console.SetCursorPosition((int)point.X, (int)point.Y);
        Console.Write("o");
    }
}
static void RemovingExcessBodySegmentsMethod(bool addNewBodySegment, List<Point> SnakeLocation)
{
    if (!addNewBodySegment)
    {
        Point position = SnakeLocation[0];
        Console.SetCursorPosition(position.X, position.Y);
        Console.Write(" ");
        SnakeLocation.RemoveAt(0);

    }
}