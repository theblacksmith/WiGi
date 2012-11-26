namespace WiGi.Data
{
	using System.Collections.Generic;
	using System.Data.Entity.Validation;
	using System.Linq;

	public static class DbEntityValidationResultExt
	{
		public static EFValidationResult ToEFValidationResult(this DbEntityValidationResult dbResult)
		{
			var errors = (from e in dbResult.ValidationErrors
			              select new EFValidationError(e.PropertyName, e.ErrorMessage))
				             .ToList<IValidationError>() as ICollection<IValidationError>;

			return new EFValidationResult(dbResult.Entry.Entity, errors);
		}

		public static IEnumerable<EFValidationResult> ToEFValidationResults(this IEnumerable<DbEntityValidationResult> dbResults)
		{
			return (from r in dbResults select r.ToEFValidationResult()).ToList<EFValidationResult>();
		}
	}
}