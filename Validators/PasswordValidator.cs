using Backend.Dtos;

namespace Backend.Validators
{
    public static class PasswordValidator
    {
        public static List<string> Validate(string password, string confirmPassword)
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(password))
                errors.Add("Password harus diisi.");
            else
            {
                if (password.Length < 8)
                    errors.Add("Password harus terdiri dari minimal 8 karakter.");
                if (!password.Any(char.IsDigit))
                    errors.Add("Password harus memiliki angka.");
                if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
                    errors.Add("Password harus memiliki symbol.");
            }

            if (string.IsNullOrWhiteSpace(confirmPassword))
                errors.Add("Konfirmasi password harus diisi.");

            if (!string.IsNullOrWhiteSpace(password) &&
                !string.IsNullOrWhiteSpace(confirmPassword) &&
                password != confirmPassword)
                errors.Add("Konfirmasi password tidak sama.");

            return errors;
        }
    }
}