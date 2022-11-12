namespace Fraxiinus.LeagueBackupManager.LeagueVersionControl.Helpers;

using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

public static class FileStreamExtensions
{
    public static async Task<byte[]> ReadBytesAsync(this FileStream fileStream, int count, CancellationToken cancellationToken = default)
    {
        byte[] buffer = new byte[count];

        await fileStream.ReadAsync(buffer.AsMemory(0, count), cancellationToken);

        if (buffer.Length != count)
        {
            throw new Exception("did not read correct amount of bytes");
        }
        return buffer;
    }

    public static async Task<byte[]> ReadBytesAsync(this FileStream fileStream, int count, int offset, SeekOrigin origin, CancellationToken cancellationToken = default)
    {
        byte[] buffer = new byte[count];

        fileStream.Seek(offset, origin);
        await fileStream.ReadAsync(buffer.AsMemory(0, count), cancellationToken);

        if (buffer.Length != count)
        {
            throw new Exception("did not read correct amount of bytes");
        }
        return buffer;
    }

    /// <summary>
    /// Computes MD5 hash from input filestream, returns hexademical string
    /// </summary>
    /// <param name="fileStream"></param>
    /// <returns></returns>
    public static async Task<string> ComputeMD5Hash(this FileStream fileStream)
    {
        using var md5 = MD5.Create();
        var hashArray = await md5.ComputeHashAsync(fileStream);
        return Convert.ToHexString(hashArray);
    }
}

