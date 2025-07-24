using Backend.Dtos;

namespace Backend.Validators {
    public static class UpdateUserValidator {
        public static List<string> Validate(UpdateUserRequest request) {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(request.FirstName))
                errors.Add("Nama depan harus diisi.");
            if (string.IsNullOrWhiteSpace(request.Email))
                errors.Add("Email harus diisi.");
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