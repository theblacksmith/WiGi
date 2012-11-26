namespace WiGi.Domain
{
	using System;
	using System.Linq.Expressions;

	public class PagingOptions
	{
		public int PageSize;
		public int Page;
		public int Total;

		public int PageCount
		{
			get
			{
				return decimal.ToInt32(Math.Ceiling((decimal)Total / PageSize));
			}
		}

		/// <summary>
		/// Creates a new PagingOption object
		/// </summary>
		/// <param name="pageSize">How many items per page</param>
		/// <param name="page">The page to show. Starts at 1.</param>
		public PagingOptions(int pageSize, int page)
		{
			PageSize = pageSize;
			Page = page;
		}

		/// <summary>
		/// Returns the start page number to show on pagination components
		/// </summary>
		/// <remarks>
		/// Based on the amount of numbers displayed on the pagination component, this method will return the 
		/// starting number so the selected page is in middle, if possible.
		/// </remarks>
		/// <param name="amountOfNumbers">How many numbers will be displayed on the component</param>
		/// <returns></returns>
		public int GetStartPageNumber(int amountOfNumbers)
		{
			if (PageCount < amountOfNumbers)
				return 1;

			if (Page - (PageCount / 2) > 0)
			{
				// we are over the middle of the list
				return (Page + amountOfNumbers / 2 > PageCount ? PageCount : Page + amountOfNumbers / 2) - amountOfNumbers;
			}
			
			// before the middle
			return (Page - amountOfNumbers / 2 < 1 ? 1 : Page - amountOfNumbers / 2);
		}
	}

	public class OrderedPagingOptions<TEntity, TOrderByType> : PagingOptions
	{
		public Expression<Func<TEntity, TOrderByType>> OrderBy;

		public OrderedPagingOptions(int pageSize, int page, Expression<Func<TEntity, TOrderByType>> orderBy)
			: base(pageSize, page)
		{
			OrderBy = orderBy;
		}
	}
}