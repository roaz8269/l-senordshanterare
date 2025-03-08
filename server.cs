using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.IO;
using System.Dynamic;
class Server
{
    public Dictionary<string, string> vault = new Dictionary<string, string>(); //?
    public string EncryptedVault { get; private set; }
    public byte[] IV {get; private set;} = new byte[16]; 

    public byte[] vaultKey {get; private set;}

    public Server(string name, byte[] vaultKey)
    {
        //GenerateIV();
        this.vaultKey = vaultKey;
        vault["gmail"] = "123";
        EncryptedVault = EncryptVault();
        GenerateJsonFile(name);
        
    }
    public void GenerateJsonFile(string name)
    {
        string json = JsonSerializer.Serialize(this);
        File.WriteAllText(name + ".json", json);
    }

    private void GenerateIV() //genererar en ny IV
    {
        RandomNumberGenerator rng = RandomNumberGenerator.Create();
        rng.GetBytes(IV);
    }

    public string EncryptVault()
    {
        GenerateIV(); // Skapa en ny IV för denna kryptering
        string json = JsonSerializer.Serialize(vault); // Konvertera dictionary till JSON
        byte[] encryptedData = EncryptString(json); // Kombinera IV och krypterad data i en Base64-sträng
        byte[] combinedData = new byte[IV.Length + encryptedData.Length];
        Buffer.BlockCopy(IV, 0, combinedData, 0, IV.Length);
        Buffer.BlockCopy(encryptedData, 0, combinedData, IV.Length, encryptedData.Length);
        return Convert.ToBase64String(combinedData);
    }

    private byte[] EncryptString(string plainText)
    {
        using Aes aes = Aes.Create();
        aes.Key = vaultKey;
        aes.IV = IV;

        using ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
        return encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
    }
}