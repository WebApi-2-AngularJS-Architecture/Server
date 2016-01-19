namespace CompanySystem.Data.Common.Constants
{
    public class ValidationConstants
    {
        // User
        public const int FullNameMaxLength = 150;
        public const int FullNameMinLength = 2;
        public const string FullNameMaxLengthErrorMessage = "Full name max length cannot exceed 50 characters.";
        public const string FullNameMinLengthErrorMessage = "Full name min length cannot be less than 2 characters.";
       
        public const int UsernameMaxLength = 50;
        public const int UsernameMinLength = 3;
        public const string UsernameMaxLengthErrorMessage = "Username max length cannot exceed 50 characters.";
        public const string UsernameMinLengthErrorMessage = "Username min length cannot be less than 3 characters.";

        public const int EmailMinLength = 3;
        public const string EmailMinLengthErrorMessage = "Email min length cannot be less than 3 characters.";
    }
}