using System;

namespace FootballManagerCONSOLE
{
    public class Player
    {
        #region PROPERTIES
        public string Name { get; set; }
        public int Age { get; set; }
        public int RetireAge { get; set; }
        public string Position { get; set; }
        public int Rating { get; set; } // Rating between 5 and 99     
        public float Value { get; set; }
        #endregion

        #region FIELDS
        private static readonly string[] Positions = { "F", "M", "D", "G" }; // F: Forward, M: Midfielder, D: Defender, G: Goalkeeper
        private static readonly string[] Names = { "John Doe", "Jane Smith", "Alex Brown", "Emily Davis", "Chris Wilson" };
        private static Random rand = new Random();
        #endregion

        #region CONSTRUCTORS
        // Parameterized constructor
        public Player(string name, int age, int retireAge, string position, int rating)
        {
            Name = name;
            Age = age;
            RetireAge = retireAge;
            Position = position;
            Rating = rating;
        }

        public Player(int league)
        {
            // Assign a random name
            Name = Names[rand.Next(Names.Length)];

            // Determine the rating based on the league level
            int minRating, maxRating;
            switch (league)
            {
                case 1: // Top league
                    minRating = 70;
                    maxRating = 99;
                    break;
                case 2: // Second league
                    minRating = 60;
                    maxRating = 79;
                    break;
                case 3: // Third league
                    minRating = 50;
                    maxRating = 69;
                    break;
                case 4: // Fourth league
                    minRating = 40;
                    maxRating = 59;
                    break;
                case 5: // Fifth league
                    minRating = 30;
                    maxRating = 49;
                    break;
                case 6: // Sixth league (lowest)
                default:
                    minRating = 10;
                    maxRating = 29;
                    break;
            }

            // Generate a random rating within the range
            Rating = rand.Next(minRating, maxRating + 1);

            // Determine age and retirement age based on the rating
            Age = rand.Next(18, 36); // Realistic player age range

            // Calculate realistic retirement age
            if (Age <= 25)
            {
                RetireAge = Age + rand.Next(10, 15); // Younger players retire later
            }
            else if (Age <= 30)
            {
                RetireAge = Age + rand.Next(5, 10); // Mid-age players have moderate time left
            }
            else
            {
                RetireAge = Age + rand.Next(3, 7); // Older players retire sooner
            }

            // Ensure retirement age is logically after the current age
            if (RetireAge <= Age)
            {
                RetireAge = Age + 1;
            }

            // Assign a random position
            Position = Positions[rand.Next(Positions.Length)];
        }
        #endregion


        #region METHODS
        // Example method to display player information
        public void DisplayPlayerInfo()
        {
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Age: {Age}");
            Console.WriteLine($"Retirement Age: {RetireAge}");
            Console.WriteLine($"Position: {Position}");
            Console.WriteLine($"Rating: {Rating}");
        }
        #endregion
    }
}
