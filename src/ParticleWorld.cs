using System.Numerics;
using Raylib_cs;


namespace Particles
{

    public class ParticlePos
    {
        public int x;  
        public int y;

        public ParticlePos(int x=0, int y=0)
        {
            this.x = x;
            this.y = y;
        }

        //converts a float vector to a ParticlePos object
        public static ParticlePos FromVector(Vector2 vector)
        {
            ParticlePos pos = new ParticlePos
            {
                x = (int)vector.X,
                y = (int)vector.Y
            };
            return pos;
        }

        public ParticlePos Copy()
        {
            return new ParticlePos(x, y);
        }

        //calculates the (rounded) mouse position in the world grid.
        public static ParticlePos MouseToWorldPoint(Vector2 mousePos,ParticlePos screenSize,ParticlePos worldSize)
        {
            
            int xPos = (int)Math.Round(mousePos.X/(screenSize.x/worldSize.x));
            int yPos = (int)Math.Round(mousePos.Y / (screenSize.y/worldSize.y));
            return new ParticlePos(xPos,yPos);
        }
    }

    public class ParticleWorld
    {
        //the current world state, true -> cell is alive, false -> cell is dead 
        private bool[,] worldArray;
        private bool[,] bufferArray;

        public ParticlePos worldSize {  get; private set; }


        //color of each particle
        Color cellColor = Color.White;


        //the rendered size of each cell, gets calculated based on the world and screen size
        private ParticlePos particleSize;
        

        //Constructor
        public ParticleWorld(ParticlePos worldSize,ParticlePos screenSize)
        {
            worldArray = new bool[worldSize.y, worldSize.x];
            bufferArray = new bool[worldSize.y, worldSize.x];

            this.worldSize = worldSize;
            
            particleSize = new ParticlePos(screenSize.x/worldSize.x,screenSize.y/worldSize.y); 
            
        }


        //fills the world randomly with alive cells
        public void RandomizeWorld()
        {
            Random random = new Random();
            for (int y = 0; y < worldArray.GetLength(0); y++)
            {
                for (int x = 0; x < worldArray.GetLength(1); x++)
                {
                    if(random.Next(1,3) == 2)
                    {
                        SetParticle(new ParticlePos(x,y));
                    }
                }
            }
        }
        
        public void RenderWorld()
        {
            
            
            for (int y = 0; y < worldArray.GetLength(0); y++)
            {
                for (int x = 0; x < worldArray.GetLength(1); x++)
                {
                    if(worldArray[y, x]==true)
                    {
                        Raylib.DrawRectangle(x*particleSize.x,y*particleSize.y,particleSize.x,particleSize.y,cellColor);
                        //Raylib.DrawRectangleGradientEx(new Rectangle(x * particleSize.X, y * particleSize.Y, particleSize.X, particleSize.Y), Color.Orange, Color.Lime, Color.Blue, Color.Yellow);
                        //^--- This is a graphics test. It just changes the color of the rect that is drawn. Remove old drawing code and uncomment to see for yourself.
                    }

                   
                }
            }
            
            
        }

        


        //Simulates one iterration of the World
        public void Iteration()
        {
            bool[,] nextState = bufferArray;
           

            ParticlePos currentParticle = new ParticlePos();

            

            for(int y = 0; y < worldArray.GetLength(0); y++)
            {
                currentParticle.y = y;
                for(int x = 0;x < worldArray.GetLength(1); x++)
                {
                    currentParticle.x = x;

                    //checks if the position of the current part. is not on the "border"
                    if(ValidatePos(currentParticle))
                    {
                        int neighbourCount = GetNeighbours(currentParticle);

                        bool currentValue = worldArray[currentParticle.y, currentParticle.x];

                        bool newValue = DefaultRuleSet(neighbourCount, currentValue);
                        //applies the rule set to the current value.

                        nextState[currentParticle.y, currentParticle.x] = newValue;
                        



                    }
                }
            }
            
            bufferArray = worldArray;
            worldArray = nextState;

        }

        //the default conways game of life rule set.
        private bool DefaultRuleSet(int neighbours, bool currentValue)
        {
            /*
             * Conway's GOL Rules
             * A live cell dies if it has fewer than two live neighbors.
             * A live cell with two or three live neighbors lives on to the next generation.
             * A live cell with more than three live neighbors dies.
             * A dead cell will be brought back to live if it has exactly three live neighbors.
             */
            if (neighbours < 2 && currentValue == true)
            {
                return false;
            }
            else if (neighbours > 3 && currentValue == true)
            {
                return false;
            }
            else if ((neighbours == 2 || neighbours == 3) && currentValue == true)
            {
                return true;
            }
            else if (neighbours == 3 && currentValue == false)
            {
                return true;
            }
            return false;
        }
        

        //Gets all live neigbours at the given position. 
        private int GetNeighbours(ParticlePos pos)
        {
            int neighbourCount = 0; 
            /*
             * Starting at top-left, going to bottom-right
             * tl X  X   | Y = -1 X = -1 ... 1 
             * X  C  X   | Y = 0  X = -1 ... 1
             * X  X  br  | Y = 1  X = -1 ... 1
             * 
             * tl -> top left
             * br -> bottom right
             * C -> the given position
             * 
             */
            for(int y = pos.y - 1; y <= pos.y + 1; y++)
            {
                for(int x = pos.x - 1; x <= pos.x + 1; x++)
                {
                    if(x == pos.x && y == pos.y)
                    {
                        //loop is at the current particle.
                        continue;
                    }
                    if (worldArray[y,x] == true)
                    {
                        //neighbour is alive
                        neighbourCount++;
                    }
                }
            }

           return neighbourCount;

        }

        private bool ValidatePos(ParticlePos worldPos)
        {
            //checks if position is in bounds of the world. The last and first positions are counted as the border and are not valid.
            if (worldPos.x >= 1 && worldPos.y >= 1 && worldPos.y < worldSize.y-1 && worldPos.x < worldSize.x-1)
            {
                //position is valid
                return true;
            }

            //position is not valid
            return false;
        }

        //"places" an alive cell at the given coordinates if valid.
        public void SetParticle(ParticlePos worldPos)
        {
            if (ValidatePos(worldPos))
            {
                worldArray[worldPos.y, worldPos.x] = true;
            }

        }

       


    }
}
