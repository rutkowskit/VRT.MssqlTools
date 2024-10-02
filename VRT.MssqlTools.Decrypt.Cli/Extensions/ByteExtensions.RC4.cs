namespace VRT.MssqlTools.Decrypt.Cli.Extensions;

public static class ByteExtensions
{
    private const int KeySize = 256;

    public static byte[] EncryptOrDecryptRC4(this byte[] data, byte[] key)
    {
        var s = Initialize(key);
        byte[] result = new byte[data.Length];
        int x = 0;
        int y = 0;

        for (int i = 0; i < data.Length; i++)
        {
            x = (x + 1) % KeySize;
            y = (y + s[x]) % KeySize;
            Swap(s, x, y);

            byte xorIndex = (byte)((s[x] + s[y]) % KeySize);
            result[i] = (byte)(data[i] ^ s[xorIndex]);
        }
        return result;
    }

    private static byte[] Initialize(byte[] key)
    {
        var s = Enumerable.Range(0, KeySize).Select(i => (byte)i).ToArray();
        var keyLength = key.Length;
        int j = 0;
        for (int i = 0; i < KeySize; i++)
        {
            j = (j + s[i] + key[i % keyLength]) % KeySize;
            Swap(s, i, j);
        }
        return s;
    }

    private static void Swap(byte[] arr, int i, int j) => (arr[i], arr[j]) = (arr[j], arr[i]);
}
