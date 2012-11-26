namespace WiGi.Data
{
	using System.Collections.Generic;

	public class EFValidationResult : IValidationResult
	{
		public object Entity { get; set; }
		public ICollection<IValidationError> ValidationErrors { get; set; }

		public bool IsValid
		{
			get { return ValidationErrors.Count == 0; }
		}

		public EFValidationResult(object entity, ICollection<IValidationError> validationErrors)
		{
			Entity = entity;
			ValidationErrors = validationErrors;
		}
	}
}