using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using Azure.Storage.Blobs;

class Program
{
    static async Task Main()
    {
        var credential = new ClientSecretCredential(
            "26982901-0512-43d5-b490-d67ca78de0da",
            "6da7a87d-81c8-4e39-8009-3f9df0ecfcf8",
            "g.V8Q~1ZjvfAxheb_OOKtKDzi5aAcjkChvlOtbDx"
        );

        var keyClient = new KeyClient(
            new Uri("https://key-vault-capgemini-cu.vault.azure.net/"),
            credential
        );

        var key = await keyClient.GetKeyAsync("secret-key-imageBlib");
        var cryptoClient = new CryptographyClient(key.Value.Id, credential);

        // READ IMAGE
        byte[] imageBytes = await File.ReadAllBytesAsync(
            @"D:\NET\repos\71\BlobEncryptAndDecrypt\sadie.jpeg"
        );

        // AES KEY
        using Aes aes = Aes.Create();
        aes.GenerateKey();
        aes.GenerateIV();

        byte[] encryptedImage;

        using (var encryptor = aes.CreateEncryptor())
        using (var ms = new MemoryStream())
        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        {
            await cs.WriteAsync(imageBytes, 0, imageBytes.Length);
            cs.Close();
            encryptedImage = ms.ToArray();
        }

        // ENCRYPT AES KEY USING KEY VAULT
        var encryptedKey = await cryptoClient.EncryptAsync(
            EncryptionAlgorithm.RsaOaep,
            aes.Key
        );

        // COMBINE METADATA
        using var finalStream = new MemoryStream();
        using var writer = new BinaryWriter(finalStream);

        writer.Write(aes.IV.Length);
        writer.Write(aes.IV);

        writer.Write(encryptedKey.Ciphertext.Length);
        writer.Write(encryptedKey.Ciphertext);

        writer.Write(encryptedImage.Length);
        writer.Write(encryptedImage);

        writer.Flush();

        // UPLOAD TO BLOB
        var blobClient = new BlobContainerClient(
            new Uri("https://10aprcapgeminicu8cba.blob.core.windows.net/data"),
            credential
        );

        var blob = blobClient.GetBlobClient("secure-image.bin");

        finalStream.Position = 0;
        await blob.UploadAsync(finalStream, overwrite: true);

        Console.WriteLine("Encrypted image uploaded!");
    }
}



//using Azure.Identity;
//using Azure.Security.KeyVault.Keys;
//using Azure.Security.KeyVault.Keys.Cryptography;
//using Azure.Storage.Blobs;
//using System.Security.Cryptography;

//namespace BlobEncryptAndDecrypt
//{
//    internal class Program
//    {
//        static async System.Threading.Tasks.Task Main()
//        {
//            // AUTH
//            var credential = new ClientSecretCredential(
//                tenantId: "26982901-0512-43d5-b490-d67ca78de0da",
//                clientId: "6da7a87d-81c8-4e39-8009-3f9df0ecfcf8",
//                clientSecret: "g.V8Q~1ZjvfAxheb_OOKtKDzi5aAcjkChvlOtbDx"
//            );

//            // KEY VAULT CLIENT
//            var keyClient = new KeyClient(new Uri("https://key-vault-capgemini-cu.vault.azure.net/"), credential);
//            var key = await keyClient.GetKeyAsync("secret-key-imageBlib");

//            var cryptoClient = new CryptographyClient(key.Value.Id, credential);

//            // READ IMAGE
//            byte[] imageBytes = await File.ReadAllBytesAsync(@"D:\NET\repos\71\BlobEncryptAndDecrypt\sadie.jpeg");

//            Console.WriteLine("Encrypting image...");

//            // ENCRYPT
//            var encryptResult = await cryptoClient.EncryptAsync(
//                EncryptionAlgorithm.RsaOaep,
//                imageBytes
//            );

//            byte[] encryptedData = encryptResult.Ciphertext;

//            // SAVE LOCALLY (optional)
//            await File.WriteAllBytesAsync("encrypted.bin", encryptedData);

//            // UPLOAD TO BLOB
//            var blobClient = new BlobContainerClient(
//                new Uri("https://10aprcapgeminicu8cba.blob.core.windows.net/data"),
//                credential
//            );

//            var blob = blobClient.GetBlobClient("encrypted-image.bin");

//            using (var ms = new MemoryStream(encryptedData))
//            {
//                await blob.UploadAsync(ms, overwrite: true);
//            }

//            Console.WriteLine("Uploaded encrypted image to blob.");

//            // DOWNLOAD FROM BLOB
//            var download = await blob.DownloadAsync();
//            using var downloadStream = new MemoryStream();
//            await download.Value.Content.CopyToAsync(downloadStream);

//            byte[] downloadedEncrypted = downloadStream.ToArray();

//            Console.WriteLine("Decrypting image...");

//            // DECRYPT
//            var decryptResult = await cryptoClient.DecryptAsync(
//                EncryptionAlgorithm.RsaOaep,
//                downloadedEncrypted
//            );

//            byte[] decryptedImage = decryptResult.Plaintext;

//            // SAVE IMAGE
//            await File.WriteAllBytesAsync("output.jpg", decryptedImage);

//            Console.WriteLine("Decrypted image saved as output.jpg");
//        }
//    }
//}