using MACoreLib.Exceptions;
using System.Collections.Generic;
using System.IO;

namespace MACoreLib.Files
{

	/// <summary>Exception thrown when enumeration failed.</summary>
	public class FileEnumerationFailedException : MACoreException { };

	/// <summary>Uniqify files in directory</summary>
	public class Uniqifier
	{
		private string root;

		/// <summary></summary>
		/// <exception cref="System.IO.DirectoryNotFoundException">
		///   <paramref name="dirroot">throwed when dirroot not found.</paramref>
		/// </exception>
		public Uniqifier(string dirroot)
		{
			if (Directory.Exists(dirroot) == false)
			{
				throw new DirectoryNotFoundException(dirroot + " not founds.");
			}
			this.root = dirroot;
		}

		/// <summary>get unique files. files uniqued by sha256</summary>
		/// <returns>iterator of string</returns>
		/// <exception cref="MACoreLib.Files.FileEnumerationFailedException"></exception>
		public IEnumerable<string> GetUniquedFiles()
		{

			string[] files;

			var alreadySeen = new Dictionary<string, bool>();

			try
			{
				// listup all files in directory
				files = Directory.GetFiles(this.root, "*", SearchOption.AllDirectories);
			}
			catch
			{
				throw new FileEnumerationFailedException();
			}

			foreach (var f in files)
			{
				string sha256 = "";

				try
				{
					// calc sha256
					var filehash = new FileHash(f);
					sha256 = filehash.GetSha256Sum();
				}
				catch
				{
					// skip when calcuration failed.
					continue;
				}


				if (alreadySeen.ContainsKey(sha256))
				{
					// skip already picked.
					continue;
				}

				yield return f;
				alreadySeen[sha256] = true;
			}
		}

	}

}
