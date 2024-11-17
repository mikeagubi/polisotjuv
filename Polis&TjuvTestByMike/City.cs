using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polis_TjuvTestByMike
{
    internal class City
    {
        public void DrawMap(string[,] map, int row, int col, List<Person> persons, int startX, int startY,string MapName, int cityNamePosX)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (i == 0 || i == row - 1 || j == 0 || j == col - 1)
                    {
                        map[i, j] = "X";
                    }
                    else
                    {
                        map[i, j] = " ";
                    }
                }
            }
            foreach (var person in persons)
            {
                //Markering för alla
                if(person.YPosition >= 0 && person.YPosition < row 
                    && person.XPosition >= 0 && person.XPosition < col)
                {
                    map[person.YPosition, person.XPosition] = person.Marker;
                }
            }
            Console.SetCursorPosition(cityNamePosX, 0);
            Console.WriteLine(MapName);
            for (int i = 0; i < map.GetLength(0); i++)
            {
                Console.SetCursorPosition(startX, startY + i); 
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i, j]);
                }
                Console.WriteLine();
            }
        }
    }

}
