using Backend.Dtos;

namespace Backend.Validators {
    public static class RegisterValidator {
        public static List<string> Validate(RegisterRequest request) {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(request.FirstName))
                errors.Add("Nama depan harus diisi.");
            if (string.IsNullOrWhiteSpace(request.Email))
                errors.Add("Email harus diisi.");
            if (string.IsNullOrWhiteSpace(request.Password))
                errors.Add("Password harus diisi.");
            else
            {
                if (request.Password.Length < 8)
                    errors.Add("Password harus terdiri dari minimal 8 karakter.");
                if (!request.Password.Any(char.IsDigit))
                    errors.Add("Password harus memiliki angka.");
                if (!request.Password.Any(ch => !char.IsLetterOrDigit(ch)))
                    errors.Add("Password harus memiliki symbol.");
            }
            if (string.IsNullOrWhiteSpace(request.ConfirmPassword))
                errors.Add("Konfirmasi password harus diisi.");
            if (!string.IsNullOrWhiteSpace(request.Password) &&
                !string.IsNullOrWhiteSpace(request.ConfirmPassword) &&
                request.Password != request.ConfirmPassword)
                errors.Add("Konfirmasi password tidak sama.");
            if (request.IsVolunteer)
            {
                var today = DateOnly.FromDateTime(DateTime.UtcNow);
                var minDob = today.AddYears(-18);
                if (request.Dob > minDob)
                {
                    errors.Add("Volunteer berusia minimal 18 tahun.");
                }
            }
            return errors;
        }
    }
}