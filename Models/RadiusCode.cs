namespace Models
{
    public enum RadiusCode : byte
    {
        ACCESS_REQUEST = 1,
        ACCESS_ACCEPT = 2,
        ACCESS_REJECT = 3,
        ACCOUNTING_REQUEST = 4,
        ACCOUNTING_RESPONSE = 5,
        ACCOUNTING_STATUS = 6,
        PASSWORD_REQUEST = 7,
        PASSWORD_ACCEPT = 8,
        PASSWORD_REJECT = 9,
        ACCOUNTING_MESSAGE = 10,
        ACCESS_CHALLENGE = 11,
        DISCONNECT_REQUEST = 40,
        DISCONNECT_ACK = 41,
        DISCONNECT_NACK = 42,
        COA_REQUEST = 43,
        COA_ACK = 44,
        COA_NACK = 45
    }

}
