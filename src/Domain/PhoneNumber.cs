namespace JustinWritesCode.Domain;
using global::PhoneNumbers;

public class PhoneNumber
{
    public PhoneNumbers.PhoneNumber? ParsedPhoneNumber { get; protected set; }
    public string? Number { get; protected set; }
    public int? CountryCode => ParsedPhoneNumber?.CountryCode;
    public ulong? NationalNumber => ParsedPhoneNumber?.NationalNumber;
    public string? Extension => ParsedPhoneNumber?.Extension;

    public static implicit operator PhoneNumber?(string? s)
    {
        try { return new PhoneNumber { ParsedPhoneNumber = PhoneNumbers.PhoneNumberUtil.GetInstance().Parse(s, "US") }; }
        catch { return default; }
    }

    public static implicit operator string?(PhoneNumber? n)
        => n != null ? PhoneNumberUtil.GetInstance().Format(n?.ParsedPhoneNumber, PhoneNumberFormat.INTERNATIONAL) : default;
}
