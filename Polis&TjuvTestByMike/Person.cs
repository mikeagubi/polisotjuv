using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polis_TjuvTestByMike
{

    internal class Person
    {
        public string Marker { get; set; }
        public int YPosition { get; set; }
        public int XPosition { get; set; }
        public int XDirection { get; set; }
        public int YDirection { get; set; }
        public List<Item> Inventory { get; set; }

        public Person(string marker, int xPosition, int yPosition, int xDirection, int yDirection)
        {
            Marker = marker;
            YPosition = xPosition;
            XPosition = yPosition;
            XDirection = xDirection;
            YDirection = yDirection;
            Inventory = new List<Item>();
        }
    }

    internal class Police : Person
    {
        public List<Item> PoliceInventory { get; set; }
        public Police(string marker, int xPosition, int yPosition, int xDirection, int yDirekction)

            : base(marker, xPosition, yPosition, xDirection, yDirekction)
        {
           
            PoliceInventory = new List<Item>();
            Marker = "P";
           
        }
    }

    internal class Thief : Person
    {
        public List<Item> ThiefInventory { get; set; }

        public Thief(string marker, int xPosition, int yPosition, int xDirection, int yDirection)
            : base(marker, xPosition, yPosition, xDirection, yDirection)
        {
            ThiefInventory = new List<Item>();
            Marker = "T";
        }

    }

    internal class Citizen : Person
    {
        public List<Item> BackPack { get; set; }
        public Citizen(string marker, int xPosition, int yPosition, int xDirection, int yDirection)
            : base(marker, xPosition, yPosition, xDirection, yDirection)
        {
            BackPack = new List<Item>();
            BackPack.Add(new("Smycken"));
            BackPack.Add(new("Mobiltelefon"));
            BackPack.Add(new("Pengar"));
            BackPack.Add(new("Klocka"));
            Marker = "C";
        }

    }

}
