using BusinessLayer;

namespace DataAccessLayer
{
    public class SqlServerCompactRepository : IRepository
	{
		public int SaveSpeaker(Speaker speaker)
		{
			//TODO: Save speaker to DB for now. For demo, just assume success and return 1.
			return 1;
		}

		public int calculateRegistrationFee(int? yearsOfExperience) {
		int registrationFee;
		if (yearsOfExperience <= 1) {
			registrationFee = 500;
		} else if (yearsOfExperience >= 2 && yearsOfExperience <= 3) {
			registrationFee = 250;
		} else if (yearsOfExperience >= 4 && yearsOfExperience <= 5) {
			registrationFee = 100;
		} else if (yearsOfExperience >= 6 && yearsOfExperience <= 9) {
			registrationFee = 50;
		} else {
			registrationFee = 0;
		}
		return registrationFee;
	}
	}
}
