
using System.Drawing;

class SpawnFruit
{
    bool isValidFruitPosition = false;
    int fruitPositionX, fruitPositionY;
    public SpawnFruit(List<Point> SnakeLocation, int width, int height)
    {     
        Random rnd = new Random();
        while (!isValidFruitPosition)
        {
            fruitPositionX = rnd.Next(2, width - 1);
            fruitPositionY = rnd.Next(2, height);
            foreach (Point point in SnakeLocation)
            {
                if (point.X == fruitPositionX + 2 || point.X == fruitPositionX - 2)
                {
                    isValidFruitPosition = false;
                    break;
                }
                else if (point.Y == fruitPositionY + 2 || point.Y == fruitPositionY - 2)
                {
                    isValidFruitPosition = false;
                    break;
                }
                else
                {
                    isValidFruitPosition = true;
                }
            }
            if (isValidFruitPosition)
            {
                Console.SetCursorPosition(fruitPositionX, fruitPositionY);
                Console.WriteLine("@");
            }
        }
    }
    public int GetXAxis()
    {
        return fruitPositionX;
    }
    public int GetYAxis()
    {
        return fruitPositionY;
    }

}

