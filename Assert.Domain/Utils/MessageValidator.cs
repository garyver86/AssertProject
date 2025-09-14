using System.Text.RegularExpressions;

namespace Assert.Domain.Utils
{
    public class MessageValidator
    {
        private readonly Regex _phoneRegex;
        private readonly Regex _emailRegex;

        public MessageValidator()
        {
            // Patrón para detectar números de teléfono (internacionales y locales)
            _phoneRegex = new Regex(@"""
            (?:\+?(\d{1,3})[-.\s]?)?   # Código de país opcional
            (\(?\d{2,4}\)?[-.\s]?)?     # Código de área opcional
            (\d{2,4}[-.\s]?){2,4}       # Número principal
            \d{2}                       # Últimos dígitos
            """, RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

            // Patrón para detectar emails
            _emailRegex = new Regex(@"""
            [a-zA-Z0-9._%+-]+           # Usuario
            @                           # Separador
            [a-zA-Z0-9.-]+              # Dominio
            \.[a-zA-Z]{2,}              # TLD
            """, RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
        }

        public ValidationResult ValidateMessage(string messageBody)
        {
            var result = new ValidationResult { IsValid = true };

            // Verificar números de teléfono
            var phoneMatches = _phoneRegex.Matches(messageBody);
            if (phoneMatches.Count > 0)
            {
                result.IsValid = false;
                result.BlockedPatterns.AddRange(phoneMatches.Select(m => m.Value));
                result.Message = "El mensaje contiene números de teléfono no permitidos";
            }

            // Verificar emails
            var emailMatches = _emailRegex.Matches(messageBody);
            if (emailMatches.Count > 0)
            {
                result.IsValid = false;
                result.BlockedPatterns.AddRange(emailMatches.Select(m => m.Value));
                result.Message = "El mensaje contiene direcciones de email no permitidas";
            }

            return result;
        }
    }

    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public List<string> BlockedPatterns { get; set; } = new List<string>();
    }
}
