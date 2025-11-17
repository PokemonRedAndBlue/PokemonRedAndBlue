using PokemonGame;
using System;
using System.Collections.Generic;

namespace TrainerMethods
{
    public class TrainerTeam : Team
    {
        // This is the perfect way to store teams.
        // We make it 'private' so it can only be accessed by methods inside this class.
        private Dictionary<string, Team> _teamDatabase;

        // A constructor is a great place to load all the teams into the dictionary.
        public TrainerTeam()
        {
            // Create a team for the "Youngster"
            Team youngsterTeam = new Team(new Pokemon[]
            {
            new Pokemon("Rattata", 5)
            });

            // Create a team for the "hiker"
            Team hikerTeam = new Team(new Pokemon[]
            {
            new Pokemon("Pikachu", 8)
            });

            // Create a team for the "blackbelt"
            Team blackbeltTeam = new Team(new Pokemon[]
            {
            new Pokemon("abra", 8)
            });

            // Create a team for the "GymLeader"
            Team gymLeaderTeam = new Team(new Pokemon[]
            {
            new Pokemon("Onix", 14)
            });

            //initialize the dictionary with our new teams
            _teamDatabase = new Dictionary<string, Team>
        {
            // The "key" is the string ID, the "value" is the Team object.
            { "youngster", youngsterTeam },
            { "gymLeader", gymLeaderTeam },
            { "hiker", hikerTeam },
            { "blackbelt", blackbeltTeam },
            // You can add more teams here
        };
        }

        // This method now looks up the trainerID in the dictionary.
        public Team GetTrainerTeam(String trainerID)
        {
            // Use TryGetValue for a safe lookup.
            // This prevents an error if the trainerID doesn't exist.
            if (_teamDatabase.TryGetValue(trainerID, out Team foundTeam))
            {
                // Found the team, return it.
                return foundTeam;
            }

            // If no team is found for that ID, return null.
            // (You could also log an error here).
            Console.WriteLine($"Error: No team found for ID: {trainerID}");
            return null;
        }
    }
}