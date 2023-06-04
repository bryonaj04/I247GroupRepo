using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using I247GroupTest.Interfaces;
using I247GroupTest.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using NUnit.Framework;


namespace RandomUserApiTests.Tests
{
    [TestFixture]
    public class RandomUserApiServiceTests
    {
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private HttpClient _httpClient;
        private IRandomUserApiService _randomUserApiService;

        [SetUp]
        public void SetUp()
        {
            var configWrapperMock = new Mock<IRandomUserConfigWrapper>();
            configWrapperMock.Setup(c => c.GetValue()).Returns(5);
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _randomUserApiService = new RandomUserApiService(_httpClient, configWrapperMock.Object);
        }

        [Test]
        public async Task GetDataFromApi_Success()
        {
            // Arrange
            var json = @"{""results"":[{""gender"":""male"",""name"":{""title"":""Mr"",""first"":""Wilfrido"",""last"":""Pizarro""},""location"":{""street"":{""number"":4631,""name"":""Viaducto Ucrania""},""city"":""Santo Tomás Hueyotlipan"",""state"":""Chiapas"",""country"":""Mexico"",""postcode"":38209,""coordinates"":{""latitude"":""28.8723"",""longitude"":""-60.7791""},""timezone"":{""offset"":""0:00"",""description"":""Western Europe Time, London, Lisbon, Casablanca""}},""email"":""wilfrido.pizarro@example.com"",""login"":{""uuid"":""fb77fc25-c6d1-47b0-95ea-6e96255cadfc"",""username"":""tinyostrich484"",""password"":""face"",""salt"":""VyZfBSb2"",""md5"":""16d6ea6a3871de7e621d4035c3b9b49a"",""sha1"":""552c83be83023c2dd86427de54b58f262ae9b395"",""sha256"":""a549ce77901ca80564b917cfdef9f639b192ce3e2da065efe84549e2702ebf4f""},""dob"":{""date"":""1995-07-09T22:22:43.889Z"",""age"":27},""registered"":{""date"":""2013-09-20T03:15:11.161Z"",""age"":9},""phone"":""(613) 386 8988"",""cell"":""(682) 970 9686"",""id"":{""name"":""NSS"",""value"":""30 35 69 2036 8""},""picture"":{""large"":""https://randomuser.me/api/portraits/men/37.jpg"",""medium"":""https://randomuser.me/api/portraits/med/men/37.jpg"",""thumbnail"":""https://randomuser.me/api/portraits/thumb/men/37.jpg""},""nat"":""MX""}],""info"":{""seed"":""3bbd7885cf8c5441"",""results"":1,""page"":1,""version"":""1.4""}}";
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json)
            };
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _randomUserApiService.GetRandomUserDataFromApi();

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetDataFromApi_Failure()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act and Assert
            Assert.ThrowsAsync<Exception>(async () => await _randomUserApiService.GetRandomUserDataFromApi());
        }
    }
}
