namespace WiGi.Wiki
{
	public interface IContentProvider
	{
		bool Exists(string docId);

		/// <summary>
		/// Returns the content for the specified document
		/// </summary>
		/// <param name="docId">The document id</param>
		/// <returns></returns>
		string GetContent(string docId);

		/// <summary>
		/// Returns the file extension used to save a files of the content type stored in this document.
		/// </summary>
		/// <param name="docId">The document id</param>
		/// <returns></returns>
		string GetFileExtension(string docId);
	}
}