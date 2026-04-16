using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using System.Text;

namespace capgemini_testing_app
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            string tenantId = "26982901-0512-43d5-b490-d67ca78de0da";
            string clientId = "505d447c-9165-41e5-aa42-3527c53e2c0c";
            string clientSecret = "VQ58Q~vOoAxL8TzYBCdcdd3JPvEh14NiqPQAPaay";


            var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

            string vaultUrl = "https://key-vault-capgemini-cu.vault.azure.net/";
            string keyName = "secret-key-9170";


            var keyClient = new KeyClient(new Uri(vaultUrl), credential);
            KeyVaultKey key;

            key = await keyClient.GetKeyAsync(keyName);

            string originalText = "Sensitive order data for CloudXeus Technology Services";
            byte[] plaintextBytes = Encoding.UTF8.GetBytes(originalText);

            var cryptoClient = new CryptographyClient(key.Id, credential);

            EncryptResult encryptResult = await cryptoClient.EncryptAsync(
                EncryptionAlgorithm.RsaOaep,
                plaintextBytes);

            Console.WriteLine("Encrypted text (Base64):");
            Console.WriteLine(Convert.ToBase64String(encryptResult.Ciphertext));

            DecryptResult decryptResult = await cryptoClient.DecryptAsync(
                EncryptionAlgorithm.RsaOaep,
                encryptResult.Ciphertext);

            string decryptedText = Encoding.UTF8.GetString(decryptResult.Plaintext);

            Console.WriteLine("\nDecrypted text:");
            Console.WriteLine(decryptedText);

            Console.ReadLine();
        }
    }
}
