using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.IO;
class Client // skapar en json-fil som innehåller en secretKey
{
    public byte[] secretKey {get; private set;} = new byte[10]; // varför tar json-filen enbart med egenskaper och inte fält? Vill inte att 'name' ska vara ett fält då jag vill att det ska vara private set 
    public Client(string name)
    {
        GenerateSecretKey();
        GenerateJsonFile(name);
    }
    private void GenerateSecretKey() //skapar en secret key
    {
        RandomNumberGenerator rng = RandomNumberGenerator.Create();
        rng.GetBytes(secretKey);
    }

    public void GenerateJsonFile(string name)
    {
        string json = JsonSerializer.Serialize(this);
        File.WriteAllText(name + ".json", json);
    }
}
