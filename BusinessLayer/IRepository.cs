namespace BusinessLayer
{
    public interface IRepository
	{
		int SaveSpeaker(Speaker speaker);
		int calculateRegistrationFee(int? yearsOfExperience);
	}
}
