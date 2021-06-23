namespace SimpleCL.Enums.Login.Error
{
    public enum AuthErrorCode: byte
    {
        InvalidCredentials = 1,
        ServerFull = 4,
        IpLimit = 5,
    }
}