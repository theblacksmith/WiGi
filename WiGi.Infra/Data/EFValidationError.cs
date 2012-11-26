namespace WiGi.Data
{
	using System.Data.Entity.Validation;

	public class EFValidationError : DbValidationError, IValidationError
	{
		public EFValidationError(string propertyName, string errorMessage) : base(propertyName,errorMessage)
		{
		}
	}
}