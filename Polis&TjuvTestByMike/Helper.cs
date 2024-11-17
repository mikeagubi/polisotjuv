using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Threading;

namespace Polis_TjuvTestByMike
{
    internal class Helper
    {
        //Metod som skapar alla invånare i staden
        public static List<Person> CreatePopulation(int cityRow, int cityCol, int numOfThieves, int numOfPolice, int numOfCitizen)
        {
            List<Person> people = new List<Person>();
            Random rnd = new Random();
            for (int i = 0; i < numOfPolice; i++)
            {
                int xPos = rnd.Next(1, cityRow);
                int yPos = rnd.Next(1, cityCol);
                int xDir = rnd.Next(-1, 2);
                int yDir = rnd.Next(-1, 2);
                people.Add(new Police("P", xPos, yPos, xDir, yDir));
            }
            for (int j = 0; j < numOfThieves; j++)
            {
                int xPos = rnd.Next(1, cityRow);
                int yPos = rnd.Next(1, cityCol);
                int xDir = rnd.Next(-1, 2);
                int yDir = rnd.Next(-1, 2);
                people.Add(new Thief("T", xPos, yPos, xDir, yDir));
            }
            for (int k = 0; k < numOfCitizen; k++)
            {
                int xPos = rnd.Next(1, cityRow);
                int yPos = rnd.Next(1, cityCol);
                int xDir = rnd.Next(-1, 2);
                int yDir = rnd.Next(-1, 2);
                people.Add(new Citizen("C", xPos, yPos, xDir, yDir));
            }
            return people;
        }


        
        //Metoden som bestämmer rörelsen samt teleporteringen av alla invånare över alla mappar
        public static void Move(Person person, int cityRow, int cityCol)
        {
            person.XPosition += person.XDirection;
            person.YPosition += person.YDirection;
            if (person.XPosition < 1)
            {
                person.XPosition = cityCol - 2;  
            }
            else if (person.XPosition > cityCol - 2)
            {
                person.XPosition = 1; 
            }
            if (person.YPosition < 1)
            {
                person.YPosition = cityRow - 2; 
            }
            else if (person.YPosition > cityRow - 2)
            {
                person.YPosition = 1; 
            }
        }

        
        
        //Metod som kontrollerar kollisioner
        public void CheckCollisions(List<Person> people, List<Person> prisoners, List<Person> poorPeople)
        {
            for (int i = 0; i < people.Count; i++)
            {
                for (int j = i+1; j < people.Count; j++) 
                {
                    if (people[i].YPosition == people[j].YPosition && people[i].XPosition == people[j].XPosition)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.SetCursorPosition(people[i].XPosition, people[i].YPosition);
                        Console.Write(people[i].Marker + people[j].Marker);
                        Console.ResetColor();
                        HandleCollisions(people[i], people[j], i, j, people, prisoners, poorPeople);
                    }
                }
            }
        }

        
        //Metod som hanterar kollisioner
        public void HandleCollisions(Person firstPerson, Person secondPerson, int firstIndex, int secondIndex, 
            List<Person> people, List<Person> prisoners, List<Person> poorPeople)
        {
            int[] rowsToClear = new int[] { 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44 };
            foreach (var row in rowsToClear)
            {
                Console.SetCursorPosition(0, row);
                Console.Write(new string(' ', Console.WindowWidth)); 
            }
            if (firstPerson is Police police && secondPerson is Citizen citizen && citizen.BackPack.Count == 0)
            {
                Console.SetCursorPosition(0, 31);
                Console.WriteLine(PoliceInspection(police, citizen, firstIndex, secondIndex, people, poorPeople, prisoners));
                Thread.Sleep(1000);
            }
            if (firstPerson is Police && secondPerson is Thief)
            {
                Console.SetCursorPosition(0, 31);
                Console.WriteLine(ArrestOrRelease(firstPerson, secondPerson, firstIndex, secondIndex, people, prisoners, poorPeople));
                Thread.Sleep(1000);
            }
            else if (firstPerson is Thief && secondPerson is Citizen)
            {
                Console.SetCursorPosition(0, 31);
                Console.WriteLine(StealOrFail(firstPerson, secondPerson, firstIndex, secondIndex));
                Thread.Sleep(1000);
            }
        }



        //Metod som hanterar kollision mellan Police och Citizen
        public string PoliceInspection(Person police, Person citizen, int firstIndex, int secondIndex, List<Person> people, List<Person> poorPeople, List<Person> prisoners)
        {
            string story = "Kollision mellan Police #" + firstIndex + " & Citizen #" + secondIndex + " vid position: (" + police.XPosition + ", " + police.YPosition + ")";
            story += "\nPolisen utför en rutinmässig kontroll på medborgaren.";
            story += "\nPolisen upptäcker att medborgaren saknar tillgångar och eskorterar hen till fattighuset för stöd.\"";
            MoveToJail(citizen, people, prisoners, poorPeople);
            return story;
        }



        //Metod som hanterar kollision mellan Police och Thief
        public string ArrestOrRelease(Person firstPerson, Person secondPerson, int firstIndex, int secondIndex, List<Person> people, List<Person> prisoners, List<Person> poorPeople)
        {
            string story = "";
            if (firstPerson is Police police && secondPerson is Thief thief)
            {
                story += "Kollision mellan Police #" + firstIndex + " & Thief #" + secondIndex + " vid position: (" + firstPerson.XPosition + ", " + firstPerson.YPosition + ")";
                story += "\nPolisen visiterar tjuven.";
                if (thief.ThiefInventory.Count > 0)
                {
                    foreach (var item in thief.ThiefInventory)
                    {
                        police.PoliceInventory.Add(item);
                        story += "\nPolisen beslagtog: " + item.Name;
                    }
                    story += "\nTjuven skickas till fängelset.";
                    MoveToJail(thief, people, prisoners, poorPeople);
                }
                else
                {
                    story += "\nInget hittades vid visiteringen; tjuven får gå vidare.";
                }
                return story;
            }
            return story;
        }




        //Metod som hanterar Kollision mellan Thief och Citizen
        public string StealOrFail(Person firstPerson, Person secondPerson, int firstIndex, int secondIndex)
        {
            string story = "";
            if (firstPerson is Thief thief && secondPerson is Citizen citizen)
            {
                story += "Kollision mellan Thief #" + firstIndex + " & Citizen #" + secondIndex + " vid position: (" + firstPerson.XPosition + ", " + firstPerson.YPosition + ")";
                if (citizen.BackPack.Count > 0)
                {
                    var stolenItem = citizen.BackPack[0];
                    citizen.BackPack.RemoveAt(0);
                    thief.ThiefInventory.Add(stolenItem);
                    story += "\nTjuven lyckas sno: " + stolenItem.Name;
                    story += "\nCitizern ligger på marken och gråter.";
                }
                else
                {
                    story += "\nMedborgaren är fattig; tjuven är totalt besviken.";
                }
                return story;
            }
            return story;
        }

        
        
        //Metod som flyttar tjuv till Fängelset eller citizen till poorhouse
        public void MoveToJail(Person person, List<Person> people, List<Person> prisoners, List<Person> poorPeople)
        {
            if(person.GetType().Name is "Thief")
            {
                person.XPosition = 2;
                person.YPosition = 3; 
                prisoners.Add(person);
                people.Remove(person);
            }
            else if(person.GetType().Name == "Citizen")
            {
                person.XPosition = 3;
                person.YPosition = 2;
                poorPeople.Add(person);
                people.Remove(person);
            }
           
        }





    }

}

        