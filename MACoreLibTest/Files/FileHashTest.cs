using System.IO;
using Xunit;
using MACoreLib.Files;

namespace MACoreLibTest.Files
{
	public class FileHashTest
	{
		[Fact]
		public void FileNotFoundTest()
		{
			var filepath = "_______not______exists________";
			Assert.Throws<FileNotFoundException>(() => new FileHash(filepath));
		}

		[Fact]
		public void EmptyFileHashTest()
		{

			var created = false;
			string tempfilename = "";

			try
			{

				tempfilename = Path.GetTempFileName();
				created = true;

				var filehash = new FileHash(tempfilename);

				// see: Additional Information of https://virustotal.com/en/file/e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855/analysis/
				Assert.Equal(filehash.GetSha1Sum(), "DA39A3EE5E6B4B0D3255BFEF95601890AFD80709");
				Assert.Equal(filehash.GetSha256Sum(), "E3B0C44298FC1C149AFBF4C8996FB92427AE41E4649B934CA495991B7852B855");
				Assert.Equal(filehash.GetMd5Sum(), "D41D8CD98F00B204E9800998ECF8427E");

			}
			finally
			{
				if (created)
				{
					File.Delete(tempfilename);
				}
			}
		}

		[Fact]
		public void HashCalcFaildTest()
		{
			string tempfilename = "";


			tempfilename = Path.GetTempFileName();

			var filehash = new FileHash(tempfilename);

			File.Delete(tempfilename);

			Assert.Throws<HashCalculationFailedException>(() => filehash.GetSha1Sum());
		}

		[Fact]
		public void CacheWorksTest()
		{
			string tempfilename = "";

			tempfilename = Path.GetTempFileName();

			var filehash = new FileHash(tempfilename);

			Assert.Equal(filehash.GetSha256Sum(), "E3B0C44298FC1C149AFBF4C8996FB92427AE41E4649B934CA495991B7852B855");

			File.Delete(tempfilename);

			Assert.Equal(filehash.GetSha256Sum(), "E3B0C44298FC1C149AFBF4C8996FB92427AE41E4649B934CA495991B7852B855");
		}

		[Fact]
		public void CacheClearWorksTest()
		{
			string tempfilename = "";

			tempfilename = Path.GetTempFileName();

			var filehash = new FileHash(tempfilename);

			Assert.Equal(filehash.GetSha256Sum(), "E3B0C44298FC1C149AFBF4C8996FB92427AE41E4649B934CA495991B7852B855");

			filehash.ClearCache();
			File.Delete(tempfilename);

			Assert.Throws<HashCalculationFailedException>(() => filehash.GetSha256Sum());
		}
	}
}
