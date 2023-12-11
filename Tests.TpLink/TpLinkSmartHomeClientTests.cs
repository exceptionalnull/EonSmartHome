using EonData.SmartHome.TpLink;

namespace Tests.TpLink
{
    public class TpLinkSmartHomeClientTests
    {
        readonly byte[] expectedBytes = new byte[] {
                0,
                0,
                0,
                29,
                208,
                242,
                129,
                248,
                139,
                255,
                154,
                247,
                213,
                239,
                148,
                182,
                209,
                180,
                192,
                159,
                236,
                149,
                230,
                143,
                225,
                135,
                232,
                202,
                240,
                139,
                246,
                139,
                246
            };

        readonly string expectedString = "{\"system\":{\"get_sysinfo\":{}}}";

        [Fact]
        public void CanEncrypt()
        {
            var p = new EonData.SmartHome.TpLink.TpLinkSmartHomeClient("127.0.0.1");
            var returnValue = p.Encrypt(expectedString);
            Assert.Equivalent(expectedBytes, returnValue);
        }

        [Fact]
        public void CanDecrypt()
        {
            var p = new EonData.SmartHome.TpLink.TpLinkSmartHomeClient("127.0.0.1");
            var returnValue = p.Decrypt(expectedBytes, expectedBytes.Length);
            Assert.Equal(expectedString, returnValue);
        }

        [Fact]
        public void CanEncryptDecrypt()
        {
            string value = "this is a test. this is only a test. everything should work. {}!@#$%^&*()_+=-/.,;''][=-0123456789*-/+";
            var p = new EonData.SmartHome.TpLink.TpLinkSmartHomeClient("127.0.0.1");
            var encValue = p.Encrypt(value);
            var decValue = p.Decrypt(encValue, encValue.Length);
            Assert.Equal(value, decValue);
        }
    }
}