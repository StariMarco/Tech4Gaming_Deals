using System;
using System.IO;
using Newtonsoft.Json;
using Tech4Gaming_Deals.Models;

namespace Tech4Gaming_Deals.Managers
{
    public static class LocalDataManager
    {
        public static void Save(LocalData data)
        {
            // Serialize object to Json
            var jsonData = JsonConvert.SerializeObject(data);
            // Get the file path
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "temp.txt");
            // Write to file
            File.WriteAllText(fileName, jsonData);
        }

        public static LocalData Load()
        {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "temp.txt");

            if (!File.Exists(fileName))
                return null;

            string jsonData = File.ReadAllText(fileName);

            return (JsonConvert.DeserializeObject<LocalData>(jsonData));
        }
    }
}
