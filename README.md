Game of Life in C# (using Raylib)

This is a simple implementation of Conway's Game of Life in C#, using the Raylib library for graphics. 
The Game of Life is a cellular automaton where cells evolve based on the number of neighboring cells, creating complex patterns from simple rules.

Features:
  Placing cells
  Pause/Resume simulation
  Generate Random cells



How it works:
  Each cell follows Conway's rules:
  Any live cell with fewer than 2 or more than 3 neighbors dies.
  Any live cell with 2 or 3 neighbors lives on to the next generation.
  Any dead cell with exactly 3 neighbors becomes alive.

  How to use:
  Build from the source code in the "src" directory yourself, or download the 

