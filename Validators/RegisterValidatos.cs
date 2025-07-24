using Backend.Dtos;

namespace Backend.Validators {
    public static class RegisterValidator {
        public static List<string> Validate(RegisterRequest request) {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(request.FirstName))
                errors.Add("First name is required.");
            if (string.IsNullOrWhiteSpace(request.Email))
                errors.Add("Email is required.");
            if (string.IsNullOrWhiteSpace(request.Password))
                errors.Add("Password is required.");
            else
            {
                if (request.Password.Length < 8)
                    errors.Add("Password must be at least 8 characters long.");
                if (!request.Password.Any(char.IsDigit))
                    errors.Add("Password must contain at least one number.");
                if (!request.Password.Any(ch => !char.IsLetterOrDigit(ch)))
                    errors.Add("Password must contain at least one symbol.");
            }
            if (string.IsNullOrWhiteSpace(request.ConfirmPassword))
                errors.Add("Confirm password is required.");
            if (!string.IsNullOrWhiteSpace(request.Password) &&
                !string.IsNullOrWhiteSpace(request.ConfirmPassword) &&
                request.Password != request.ConfirmPassword)
                errors.Add("Password and Confirm Password must match.");
            if (request.IsVolunteer)
            {
                var today = DateOnly.FromDateTime(DateTime.UtcNow);
                var minDob = today.AddYears(-18);
                if (request.Dob > minDob)
                {
                    errors.Add("Volunteer must be at least 18 years old.");
                }
            }
            return errors;
        }
    }
}