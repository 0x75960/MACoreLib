using MACoreLib.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;


namespace MACoreLib.Files
{

    /// <summary>
    /// Exception thrown when failed to calculate hash sum .
    /// </summary>
    class HashCalculationFailedException : MACoreException {};

    /// <summary>
    /// hash sums of a file.
    /// </summary>
    /// <remarks>
    /// This class calculate hash when call get methods at the first time.
    /// Methods returns cached results after the second time.
    /// Because It's very heavy process.
    /// </remarks>
    class FileHash
	{
		private string filepath;
		private Dictionary<string, string> cache;

		/// <summary></summary>
		/// <param name="filepath">path to get hash values</param>
		/// <exception cref="System.IO.FileNotFoundException">
		///   <paramref name="filepath">throwed when file not found.</paramref>
		/// </exception>
		public FileHash(string filepath)
		{

			if (File.Exists(filepath) == false)
			{
				throw new FileNotFoundException(filepath + " not found.");
			}

			this.filepath = filepath;
			this.cache = new Dictionary<string, string>();
		}

		/// <summary>clear cached hash values</summary>
		public void ClearCache()
		{
			this.cache.Clear();
		}

		/// <summary>get SHA512 of file</summary>
		/// <returns>hex-string of SHA512 sum</returns>
        /// <exception cref="MACoreLib.Files.HashCalculationFailedException"></exception>
		public string GetSha512Sum()
		{
			return DealGetRequest("sha512", SHA512.Create());
		}

		/// <summary>get SHA256 of file</summary>
		/// <returns>hex-string of SHA256 sum</returns>
        /// <exception cref="MACoreLib.Files.HashCalculationFailedException"></exception>
		public string GetSha256Sum()
		{
			return DealGetRequest("sha256", SHA256.Create());
		}

		/// <summary>get SHA1 of file</summary>
		/// <returns>hex-string of SHA1 sum</returns>
        /// <exception cref="MACoreLib.Files.HashCalculationFailedException"></exception>
		public string GetSha1Sum()
		{
			return DealGetRequest("sha1", SHA1.Create());
		}

		/// <summary>get Md5 of file</summary>
		/// <returns>hex-string of Md5 sum</returns>
        /// <exception cref="MACoreLib.Files.HashCalculationFailedException"></exception>
		public string GetMd5Sum()
		{
			return DealGetRequest("md5", MD5.Create());
		}

		/// <summary>deal get method</summary>
		/// <param name="key">key to cache. key must be uniqued for algorithm</param>
		/// <param name="hasher">HashAlgorithm instance to calc hash</param>
		/// <returns>hex-string of hash sum</returns>
        /// <exception cref="MACoreLib.Files.HashCalculationFailedException"></exception>
		private string DealGetRequest(string key, HashAlgorithm hasher)
		{

			if (this.cache.ContainsKey(key))
			{
				// return cached value if already calculated.
				return this.cache[key];
			}

			// at the first time, calc and cache hash value.
			var value = GetHashSumWith(hasher);
			this.cache[key] = value;

			return value;
		}

		/// <summary>calc hash sum of file with speciefied Hash Algorithm</summary>
		/// <param name="hasher">HashAlgorithm instance to calc hash</param>
		/// <returns>hex-string of hash sum</returns>
        /// <exception cref="MACoreLib.Files.HashCalculationFailedException"></exception>
		private string GetHashSumWith(HashAlgorithm hasher)
		{
			string hexstring;

            try
            {
                using (var fs = File.OpenRead(this.filepath))
                {
                    var checksum = hasher.ComputeHash(fs);
                    hexstring = BitConverter.ToString(checksum).Replace("-", String.Empty);
                }
            }
            catch
            {
                throw new HashCalculationFailedException();
            }

			return hexstring;
		}

	}

}
