namespace WiGi
{
	using System.Collections.Generic;

	public interface IValidationResult
	{
		object Entity { get; set; }
		ICollection<IValidationError> ValidationErrors { get; }
		bool IsValid { get; }
	}
}