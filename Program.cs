using Particles;
using Raylib_cs;


namespace Main
{
    class Timer
    {
        float currentTime;
        float maxTime;
        public bool doRestart;
        public bool pause = false;
        public Timer(float timeS,bool restart=false)
        {
            maxTime = timeS;
            currentTime = maxTime;
            doRestart = restart;
        }

        
        public void Set(float timeS)
        {
            maxTime = timeS;
        }

        public void Restart()
        {
            currentTime=maxTime;
        }

        private bool Over()
        {
            return currentTime <= 0;
        }

        //Returns current timer state every call
        public bool Update(float deltaTime)
        {
            if (pause == true)
            {
                return false;
            }

            if (Over())
            {
                if (doRestart)
                {
                    Restart();
                }
                return true;
            }
            
            currentTime -=deltaTime;
            return false;
        }

    }

    public static class MainProgram
    {
        private static ParticleWorld world;

        private static ParticlePos screenSize;

        private static Timer gameTick;

        public static void Main()
        {
            
            screenSize = new ParticlePos(800,800);

            world = new ParticleWorld(new ParticlePos(80,80),screenSize);

            gameTick = new Timer(0.2f, restart: true);

            StartGame();
        }

        private static void StartGame()
        {
            Raylib.InitWindow(screenSize.X, screenSize.Y, "Game of Life");

            Color clearColor = Color.Black;

            Raylib.SetTargetFPS(60);

            
            

            while (!Raylib.WindowShouldClose())
            {
                float deltaTime = Raylib.GetFrameTime();

                GetInput();
                
                Raylib.BeginDrawing();
                Raylib.ClearBackground(clearColor);
                
                
                if (gameTick.Update(deltaTime))
                {
                    
                    UpdateGame();
                    
                }

                world.RenderWorld();

                Raylib.DrawText("Press 'P' to pause, 'R' to resume.\nPress 'C' to Randomize.", 10, 10, 15, Color.Orange);

                Raylib.EndDrawing();
            }

            
            
            Raylib.CloseWindow();
        }

        private static void GetInput()
        {
            if (Raylib.IsMouseButtonDown(0))
            {
                //converting cordinates
                ParticlePos spawnPos = ParticlePos.MouseToWorldPoint(Raylib.GetMousePosition(),screenSize,world.worldSize);
                
                world.SetParticle(spawnPos);
            }

            if (Raylib.IsKeyDown(KeyboardKey.P))
            {
                gameTick.pause = true;
            }

            if (Raylib.IsKeyDown(KeyboardKey.R))
            {
                gameTick.pause= false;  
            }

            if (Raylib.IsKeyPressed(KeyboardKey.C))
            {
                world.RandomizeWorld();
            }
        }

        private static void UpdateGame()
        {
            world.Iteration();
        }


    }

}


