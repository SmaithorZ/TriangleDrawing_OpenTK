//This class will be the entry point for our game.
using Triangle_Drawing_Window;

namespace Triangle_Drawing_Window
{
    class Program
    {
        public static void Main(string[] args)
        {
            //Instantiating the Game class
            using (Game game = new Game(500, 500))
            {
                game.Run();
            }
        }
    }
}