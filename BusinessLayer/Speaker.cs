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

        private int requiredCertifications = 3;
        private int requiredYearsOfExperience = 10;
        private int minRequiredBrowserVersion = 9;

        List<string> oldTechnologies = new List<string>() { "Cobol", "Punch Cards", "Commodore", "VBScript" };
        List<string> domains = new List<string>() { "aol.com", "hotmail.com", "prodigy.com", "CompuServe.com" };
        List<string> employers = new List<string>() { "Microsoft", "Google", "Fog Creek Software", "37Signals" };

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

            calculateRegistrationFee();

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
            String[] splitted = Email.Split('@');
            String emailDomain = splitted[splitted.Length - 1];
            result = ((Experience > requiredYearsOfExperience || HasBlog
                    || Certifications.Count() > requiredCertifications || employers.Contains(Employer)
                    || (!domains.Contains(emailDomain) && Browser.Name != WebBrowser.BrowserName.InternetExplorer
                            && Browser.MajorVersion >= minRequiredBrowserVersion)));
            return result;
        }

        private void calculateRegistrationFee()
        {
            if (Experience <= 1)
            {
                RegistrationFee = 500;
            }
            else if (Experience >= 2 && Experience <= 3)
            {
                RegistrationFee = 250;
            }
            else if (Experience >= 4 && Experience <= 5)
            {
                RegistrationFee = 100;
            }
            else if (Experience >= 6 && Experience <= 9)
            {
                RegistrationFee = 50;
            }
            else
            {
                RegistrationFee = 0;
            }
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