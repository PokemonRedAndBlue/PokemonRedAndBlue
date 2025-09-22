using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonoGameLibrary.Storage
{
    public record PokemonData
    {
        public int Id { get; init; }
        public int Level { get; init; }
        public int Hp { get; init; }
        public int MaxHp { get; init; }
        public List<int> Moves { get; init; } = new();
    }

    public record PlayerData
    {
        public int Version { get; init; } = 1;
        public string Name { get; init; } = "Player";
        public int Level { get; init; } = 1;
        public int Gold { get; init; }
        public List<PokemonData> Team { get; init; } = new();
    }

    public static class SaveManager
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true
        };

        public static string GetDefaultSavePath(string fileName = "save_v1.json")
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var dir = Path.Combine(appData, "PokemonRedAndBlue");
            Directory.CreateDirectory(dir);
            return Path.Combine(dir, fileName);
        }

        public static async Task SaveAsync(PlayerData data, string path)
        {
            var temp = path + ".tmp";
            var json = JsonSerializer.Serialize(data, JsonOptions);

            // atomic-ish write
            await File.WriteAllTextAsync(temp, json);
            File.Replace(temp, path, null); // overwrite atomically when possible
        }

        public static PlayerData? Load(string path)
        {
            if (!File.Exists(path)) return null;
            var json = File.ReadAllText(path);
            try
            {
                var data = JsonSerializer.Deserialize<PlayerData>(json, JsonOptions);
                return data;
            }
            catch (JsonException)
            {
                // corrupt save — return null or implement recovery
                return null;
            }
        }
    }
}