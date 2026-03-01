namespace NDB.Kit.Base64;
public static class Base64Helper
{
    public static byte[] ToBytes(string base64)
    {
        if (string.IsNullOrWhiteSpace(base64))
            throw new ArgumentException("Base64 is empty");

        var commaIndex = base64.IndexOf(',');
        if (commaIndex >= 0)
            base64 = base64[(commaIndex + 1)..];

        return Convert.FromBase64String(base64);
    }

    public static string FromBytes(byte[] bytes, string? contentType = null)
    {
        if (bytes == null || bytes.Length == 0)
            throw new ArgumentException("Byte array is empty");

        var base64 = Convert.ToBase64String(bytes);

        return string.IsNullOrWhiteSpace(contentType)
            ? base64
            : $"data:{contentType};base64,{base64}";
    }
}