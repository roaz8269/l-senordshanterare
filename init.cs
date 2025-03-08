using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.IO;    


class Init
{
    string masterPsw;
    Client client;
    Server server; // frågan är om dessa ska vara i en egen klass eller om man bara kan ha de direkt i projekt-filen...
    public byte[] vaultKey {get; private set;} //vet inte om denna blir rätt, vet inte hur man testar det

    public Init (string clientName, string serverName, string psw)
    {
        masterPsw = psw;
        client = new Client(clientName);
        vaultKey = GenerateVaultKey(masterPsw, client.secretKey);
        server = new Server(serverName, vaultKey);
    }

    private byte[] GenerateVaultKey(string masterPsw, byte[] secretKey) //slår ihop masterPsw och secretKey till en vaultKey m.h.a Password-Based Key Derivation Function
    {
        byte[] bytesMasterPsw = Encoding.UTF8.GetBytes(masterPsw); // konverterar masterPsw till bytes
        byte[] vaultKey = new byte[bytesMasterPsw.Length + secretKey.Length]; 
        Buffer.BlockCopy(bytesMasterPsw, 0, secretKey, 0, bytesMasterPsw.Length); //fattar inte vad dessa gör...
        Buffer.BlockCopy(secretKey, 0, secretKey, bytesMasterPsw.Length, secretKey.Length);
        
        byte[] salt = new byte[16]; // vet inte riktigt vad denna ska vara till för ...
        using (var pbkdf2 = new Rfc2898DeriveBytes(vaultKey, salt, 10000, HashAlgorithmName.SHA256)) //Password-Based Key Derivation Function
        {
            return pbkdf2.GetBytes(32);
        }
    }

}
