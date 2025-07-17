namespace BE.Domain.Models
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new();
        public Dictionary<string, List<string>> FieldErrors { get; set; } = new();

        public static ValidationResult Success() => new() { IsValid = true };

        public static ValidationResult Failure(string error) => new()
        {
            IsValid = false,
            Errors = [error]
        };

        public static ValidationResult Failure(List<string> errors) => new()
        {
            IsValid = false,
            Errors = errors
        };

        public void AddError(string error)
        {
            IsValid = false;
            Errors.Add(error);
        }

        public void AddFieldError(string field, string error)
        {
            IsValid = false;
            if (!FieldErrors.ContainsKey(field))
                FieldErrors[field] = new List<string>();
            FieldErrors[field].Add(error);
        }

        public void AddFieldError(string field, List<string> errors)
        {
            IsValid = false;
            if (!FieldErrors.ContainsKey(field))
                FieldErrors[field] = new List<string>();
            FieldErrors[field].AddRange(errors);
        }

        public void Combine(ValidationResult other)
        {
            if (!other.IsValid)
            {
                IsValid = false;
                Errors.AddRange(other.Errors);
                foreach (var fieldError in other.FieldErrors)
                {
                    AddFieldError(fieldError.Key, fieldError.Value);
                }
            }
        }
    }
}
