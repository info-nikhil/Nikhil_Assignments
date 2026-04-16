using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        // AUTH
        var credential = new ClientSecretCredential(
            "26982901-0512-43d5-b490-d67ca78de0da",
            "6da7a87d-81c8-4e39-8009-3f9df0ecfcf8",
            "g.V8Q~1ZjvfAxheb_OOKtKDzi5aAcjkChvlOtbDx"
        );

        // KEY VAULT
        var keyClient = new KeyClient(
            new Uri("https://key-vault-capgemini-cu.vault.azure.net/"),
            credential
        );

        var key = await keyClient.GetKeyAsync("secret-key-imageBlib");
        var cryptoClient = new CryptographyClient(key.Value.Id, credential);

        // BLOB DOWNLOAD
        var blobClient = new BlobContainerClient(
            new Uri("https://10aprcapgeminicu8cba.blob.core.windows.net/data"),
            credential
        );

        var blob = blobClient.GetBlobClient("secure-image.bin");

        Console.WriteLine("Downloading encrypted file...");

        var download = await blob.DownloadAsync();

        using var ms = new MemoryStream();
        await download.Value.Content.CopyToAsync(ms);

        ms.Position = 0;
        using var reader = new BinaryReader(ms);

        // 🔹 STEP 1: READ IV
        int ivLength = reader.ReadInt32();
        byte[] iv = reader.ReadBytes(ivLength);

        // 🔹 STEP 2: READ ENCRYPTED AES KEY
        int keyLength = reader.ReadInt32();
        byte[] encryptedKey = reader.ReadBytes(keyLength);

        // 🔹 STEP 3: READ ENCRYPTED IMAGE
        int imageLength = reader.ReadInt32();
        byte[] encryptedImage = reader.ReadBytes(imageLength);

        Console.WriteLine("Decrypting AES key...");

        // 🔹 STEP 4: DECRYPT AES KEY USING KEY VAULT
        var decryptedKey = await cryptoClient.DecryptAsync(
            EncryptionAlgorithm.RsaOaep,
            encryptedKey
        );

        Console.WriteLine("Decrypting image...");

        // 🔹 STEP 5: DECRYPT IMAGE USING AES
        using Aes aes = Aes.Create();
        aes.Key = decryptedKey.Plaintext;
        aes.IV = iv;

        byte[] decryptedImage;

        using (var decryptor = aes.CreateDecryptor())
        using (var msDecrypt = new MemoryStream(encryptedImage))
        using (var cs = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
        using (var resultStream = new MemoryStream())
        {
            await cs.CopyToAsync(resultStream);
            decryptedImage = resultStream.ToArray();
        }

        // 🔹 STEP 6: SAVE IMAGE
        await File.WriteAllBytesAsync("output.jpg", decryptedImage);

        Console.WriteLine("Image decrypted and saved as output.jpg");
    }
}