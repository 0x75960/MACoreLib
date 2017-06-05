using Xunit;
using MACoreLib.Net;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace MACoreLibTest.Net
{

    [DataContract]
    internal class VirusTotalResponse
    {
        [DataMember]
        internal int response_code;
    }

    public class RestClientTest
    {
        [Fact]
        public async void TestConnectToVirusTotalAsync()
        {
            var r = new RestClient("https://www.virustotal.com");

            Assert.True(false); // Please input your VirusTotal key in apikey.

            var param = new Dictionary<string, string>() {
                { "apikey", "-- PLEASE YOUR KEY --" },  // FIXME
                { "resource", "7657fcb7d772448a6d8504e4b20168b8" },
            };

            VirusTotalResponse vtr = await r.Post<VirusTotalResponse>("/vtapi/v2/file/report", param);

            Assert.Equal(vtr.response_code, 1);
        }
    }
}
