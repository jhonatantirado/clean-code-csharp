using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer
{
    public class Speaker
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? Experience { get; set; }
        public bool HasBlog { get; set; }
        public string BlogURL { get; set; }
        public WebBrowser Browser { get; set; }
        public List<string> Certifications { get; set; }
        public string Employer { get; set; }
        public int RegistrationFee { get; set; }
        public List<BusinessLayer.Session> Sessions { get; set; }

        public int? Register(IRepository repository)
        {

            if (string.IsNullOrWhiteSpace(FirstName)) throw new ArgumentNullException("First Name is required");
            if (string.IsNullOrWhiteSpace(LastName)) throw new ArgumentNullException("Last name is required.");
            if (string.IsNullOrWhiteSpace(Email)) throw new ArgumentNullException("Email is required.");
            if (Sessions.Count() == 0) throw new ArgumentException("Can't register speaker with no sessions to present.");

            int? speakerId = null;
            bool isGood = false;
            bool isApproved = false;

            isGood = meetsMinimunRequirements();

            if (!isGood)
                throw new SpeakerDoesntMeetRequirementsException(
                        "Speaker doesn't meet our abitrary and capricious standards.");

            isApproved = hasSessionApproved();

            if (!isApproved)
                throw new NoSessionsApprovedException("No sessions approved.");

            RegistrationFee = repository.calculateRegistrationFee(Experience);

            try
            {
                speakerId = repository.SaveSpeaker(this);
            }
            catch (Exception e)
            {
                throw e;
            }

            return speakerId;
        }

        private bool hasSessionApproved()
        {
            bool result = true;
            List<string> oldTechnologies = new List<string>() { "Cobol", "Punch Cards", "Commodore", "VBScript" };
            foreach (Session session in Sessions)
            {
                foreach (String technology in oldTechnologies)
                {
                    if (session.Title.Contains(technology) || session.Description.Contains(technology))
                    {
                        session.Approved = false;
                        result = false;
                        break;
                    }
                    else
                    {
                        session.Approved = true;
                    }
                }
            }
            return result;
        }
        private bool meetsMinimunRequirements()
        {
            bool result;
            int requiredCertifications = 3;
            int requiredYearsOfExperience = 10;
            int minRequiredBrowserVersion = 9;
            List<string> domains = new List<string>() { "aol.com", "hotmail.com", "prodigy.com", "CompuServe.com" };
            List<string> employers = new List<string>() { "Microsoft", "Google", "Fog Creek Software", "37Signals" };
            String[] splitted = Email.Split('@');

            String emailDomain = splitted[splitted.Length - 1];
            result = ((Experience > requiredYearsOfExperience || HasBlog
                    || Certifications.Count() > requiredCertifications || employers.Contains(Employer)
                    || (!domains.Contains(emailDomain) && Browser.Name != WebBrowser.BrowserName.InternetExplorer
                            && Browser.MajorVersion >= minRequiredBrowserVersion)));
            return result;
        }

        #region Custom Exceptions
        public class SpeakerDoesntMeetRequirementsException : Exception
        {
            public SpeakerDoesntMeetRequirementsException(string message)
                : base(message)
            {
            }

            public SpeakerDoesntMeetRequirementsException(string format, params object[] args)
                : base(string.Format(format, args)) { }
        }

        public class NoSessionsApprovedException : Exception
        {
            public NoSessionsApprovedException(string message)
                : base(message)
            {
            }
        }
        #endregion
    }
}