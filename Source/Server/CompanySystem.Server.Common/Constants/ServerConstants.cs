namespace CompanySystem.Server.Common.Constants
{
    public class ServerConstants
    {
        // Common
        public const string RequestCannotBeEmpty = "Request cannot be empty or have NULL properties.";

        // Votes Controller
        public const string VoteSuccessfulMessage = "Vote successful";
        public const string VoteFailedMessage = "Vote failed";
        public const string NoVotesMessage = "There are no votes currently for this event.";

        // Events Controller
        public const string CancelEventErrorMessage = "Event cannnot be cancelled.";
        public const string CancelEventSuccessMessage = "Event successfully cancelled.";
        public const string EventInsertionErrorMessage = "Event insertion failed.";
        public const string EventCreationErrorMessage = "Event creation failed. An active event from the same type already exists or you must wait for a year to pass untill next creation for the selected birthday guy.";
    }
}