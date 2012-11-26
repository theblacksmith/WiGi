namespace WiGi
{
	public interface IValidationResult<TEntity> : IValidationResult where TEntity : class
    {
        new TEntity Entity { get; set; }
    }
}
