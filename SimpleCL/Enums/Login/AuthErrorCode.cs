namespace SimpleCL.Enums.Login
{
    public enum AuthErrorCode: byte
    {
        InvalidCredentials = 1,
        ServerFull = 4,
        IpLimit = 5,
    }
}