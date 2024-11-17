using System.Collections.Generic;
using System.Threading;

namespace Polis_TjuvTestByMike
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int cityRow = 25;
            int cityCol = 100;
            int prisonAndPoorHouseRow = 10;
            int prisonAndPoorHouseCol = 25;
            int cityMapNameXPos = 46;
            int prisonMapNameXPos = 114;
            int poorHouseMapNameXPos = 142;
            int numOfThieves = 20;
            int numOfCitizens = 20;
            int numOfPolice =10;
            
            //Map Positionerna
            int prisonStartX = cityCol + 5;
            int poorHouseStartX = cityCol + prisonAndPoorHouseCol + 10;

            Helper helper = new Helper();
            City city = new City();

            string[,] cityMap = new string[cityRow, cityCol];
            string[,] prisonMap = new string[prisonAndPoorHouseRow, prisonAndPoorHouseCol];
            string[,] poorHouseMap = new string[prisonAndPoorHouseRow, prisonAndPoorHouseCol];

            List<Person> folket = Helper.CreatePopulation(cityRow, cityCol, numOfThieves, numOfPolice, numOfCitizens);
            List<Person> prisoners = new List<Person>();
            List<Person> poorPeople = new List<Person>();

            Console.SetCursorPosition(0, 30);
            Console.WriteLine("==============================| Kollisioner |====================================");

            while (true)
            {
                //Move för alla tre städer
                foreach (var person in folket)
                {
                    Helper.Move(person, cityRow, cityCol);
                }
                foreach(var prisoner in prisoners)
                {
                    Helper.Move(prisoner, prisonAndPoorHouseRow, prisonAndPoorHouseCol);
                }
                foreach(var poorPerson in poorPeople)
                {
                    Helper.Move(poorPerson, prisonAndPoorHouseRow, prisonAndPoorHouseCol);
                }

                Console.SetCursorPosition(0, 48);
                Console.WriteLine("====================| Statistik |===========================");
                Console.WriteLine("Antal personer i staden: " + folket.Count);
                Console.WriteLine("Antal tjuvar i fängelset: " + prisoners.Count);
                Console.WriteLine("Antal medborgare i fattighuset: " + poorPeople.Count);
                Console.SetCursorPosition(0, 53);
                Console.WriteLine("====================| Options |===========================");
                Console.WriteLine("Tryck [C] för att lägga till citizens:");
                Console.WriteLine("Tryck [P] för att lägga till poliser:");
                Console.WriteLine("Tryck [T] för att lägga till tjuvar:");
                //Rita alla tre städer
                city.DrawMap(cityMap, cityRow, cityCol, folket, 0, 1, "City", cityMapNameXPos);
                city.DrawMap(prisonMap, prisonAndPoorHouseRow, prisonAndPoorHouseCol, prisoners, prisonStartX, 1, "Prison", prisonMapNameXPos);
                city.DrawMap(poorHouseMap, prisonAndPoorHouseRow, prisonAndPoorHouseCol, poorPeople, poorHouseStartX, 1, "PoorHouse", poorHouseMapNameXPos);

                helper.CheckCollisions(folket, prisoners, poorPeople);

                if (Console.KeyAvailable)
                {
                    char key = Console.ReadKey().KeyChar;
                    switch (char.ToLower(key))
                    {
                        case 'c':
                             folket.AddRange(Helper.CreatePopulation(cityRow, cityCol, 0, 0, 5));
                            break;
                        case 't':
                            folket.AddRange(Helper.CreatePopulation(cityRow, cityCol, 5, 0, 0));
                            break;
                        case 'p':
                            folket.AddRange(Helper.CreatePopulation(cityRow, cityCol, 0, 5, 0));
                            break;
                    }
                }
            

            }
            

        }

    }

}
