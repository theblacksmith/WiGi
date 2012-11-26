namespace WiGi
{
	public interface IValidationError
	{
		string PropertyName { get; }
		string ErrorMessage { get; }
	}
}