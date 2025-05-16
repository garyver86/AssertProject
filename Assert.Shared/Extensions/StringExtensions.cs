using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Assert.Shared.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Separa el código de país y el número de teléfono de un string con formato internacional
    /// </summary>
    /// <param name="phone">Teléfono en formato internacional, por ejemplo +59172724711</param>
    /// <returns>Tupla con código de país y número, o vacio y la cadena si no es válido</returns>
    public static (string CountryCode, string PhoneNumber) SplitCountryCode(this string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return ("", phone);

        var match = Regex.Match(phone, @"^\+(?<CountryCode>\d{1,4})(?<PhoneNumber>\d{7,15})$");

        if (!match.Success)
            return ("", phone);

        var countryCode = match.Groups["CountryCode"].Value;
        var phoneNumber = match.Groups["PhoneNumber"].Value;

        return (countryCode, phoneNumber);
    }
}
