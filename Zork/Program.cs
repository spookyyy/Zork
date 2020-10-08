﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

//Zork Version 2.1
namespace Zork
{
    class Program
    {
        static Program()
        {
            RoomMap = new Dictionary<string, Room>();
            foreach (Room room in Rooms)
            {
                RoomMap[room.Name] = room;
            }
        }

        private static Room CurrentRoom
        {
            get
            {
                return Rooms[Location.Row, Location.Column];
            }
        }

        static void Main(string[] args)
        {
            Commands command = Commands.UNKNOWN;

            Room previousRoom = null;
            string defaultRoomsFileName = "Rooms.txt";
            string roomsFileName = (args.Length > 0 ? args[(int)CommandLineArguments.RoomsFileName] : defaultRoomsFileName);
            InitializeRoomDescription(roomsFileName);

            Console.WriteLine("Welcome to Zork!");

            while (command != Commands.QUIT)
            {
                Console.WriteLine($"{CurrentRoom}");
                if(previousRoom != CurrentRoom)
                {
                    Console.WriteLine(CurrentRoom.Description);
                    previousRoom = CurrentRoom;
                }

                Console.Write("> ");
                command = ToCommand(Console.ReadLine().Trim());
                
                switch(command)
                {
                    case Commands.QUIT:
                        Console.WriteLine("Thank you for playing!");
                        break;

                    case Commands.LOOK:
                        Console.WriteLine(CurrentRoom.Description);
                        break;

                    case Commands.NORTH:
                    case Commands.SOUTH:
                    case Commands.EAST:
                    case Commands.WEST:
                        if (!Move(command)) Console.WriteLine("The way is shut!");
                        break;

                    default:
                        Console.WriteLine("Unknown command.");
                        break;
                }
            }
        }

        private static bool Move(Commands command)
        {
            Assert.IsTrue(IsDirection(command), "Invalid direction.");

            bool isValidMove = true;

            switch (command)
            {
                case Commands.NORTH when Location.Row < Rooms.GetLength(0) - 1:
                    Location.Row++;
                    break;
                case Commands.SOUTH when Location.Row > 0:
                    Location.Row--;
                    break;
                case Commands.EAST when Location.Column < Rooms.GetLength(1) - 1:
                    Location.Column++;
                    break;
                case Commands.WEST when Location.Column > 0:
                    Location.Column--;
                    break;
                default:
                    isValidMove = false;
                    break;
            }

            return isValidMove;
        }

        private static Commands ToCommand(string commandString) =>
            Enum.TryParse(commandString, ignoreCase: true, out Commands result) ? result : Commands.UNKNOWN;

        private static bool IsDirection(Commands command) => Directions.Contains(command);

        private static void InitializeRoomDescription(string roomsFileName)
        {
            const string fieldDelimiter = "##";
            const int expectedFieldCount = 2;

            string[] lines = File.ReadAllLines(roomsFileName);

            foreach (string line in lines)
            {
                string[] fields = line.Split(fieldDelimiter);
                if(fields.Length != expectedFieldCount)
                {
                    throw new InvalidDataException("Invalid record.");
                }

                string name = fields[(int)Fields.Name];
                string description = fields[(int)Fields.Description];

                RoomMap[name].Description = description;
            }
        }

        private static readonly Dictionary<string, Room> RoomMap;

        private static readonly Room[,] Rooms = 
        {
            { new Room("Rocky Trail"), new Room("South of House"), new Room("Canyon View") },
            { new Room("Forest"), new Room("West of House"), new Room("Behind House") },
            { new Room("Dense Woods"), new Room("North of House"), new Room("Clearing") }
        };

        //stuck with the array here because it's a simpler structure, and i believe it's faster
        private static readonly Commands[] Directions =
        {
            Commands.NORTH,
            Commands.SOUTH,
            Commands.EAST,
            Commands.WEST
        };

        private enum Fields
        {
            Name = 0,
            Description
        }

        private enum CommandLineArguments
        {
            RoomsFileName = 0
        }

        private static (int Row, int Column) Location = (1, 1);
    }
}
