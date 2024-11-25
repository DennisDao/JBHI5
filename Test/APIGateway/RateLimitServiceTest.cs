using ApiGateway;
using NSubstitute;

namespace Test.APIGateway
{
    [TestFixture]
    public class RateLimitServiceTest
    {
        private RateLimitService _rateLimitService;
        private IRateLimitRuleRepository _rateLimitRuleRepository;
        private IDateTimeProvider _mockDate;

        [SetUp]
        public void SetUp()
        {
            _rateLimitRuleRepository = Substitute.For<IRateLimitRuleRepository>();
            _mockDate = Substitute.For<IDateTimeProvider>();
            _rateLimitService = new RateLimitService(_rateLimitRuleRepository, _mockDate);
        }

        [Test]
        public void Given_First_Request_Should_ReturnsFalse()
        {
            // Arrange
            string apiKey = "XYZ-789";
            _mockDate.Now.Returns(DateTime.Now);
            var mockRule = new RateLimitRule()
            {
                ApiKey = apiKey,
                EndPoint = "/Custom",
                Limit = 3,
                Window = TimeSpan.FromMinutes(10),
                RequestHistory = new LinkedList()
            };

            _rateLimitRuleRepository.Get(apiKey).Returns(mockRule);

            // Act
            bool result = _rateLimitService.IsLimitReached(apiKey);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Given_RequestLimitOf3Within10MinuteWindowIsExceeded_Should_ReturnTrue()
        {
            // Arrange
            string apiKey = "ABC-123";
            DateTime first_request   =    DateTime.Now;
            DateTime second_reequest =    DateTime.Now.AddSeconds(20);
            DateTime third_request   =    DateTime.Now.AddSeconds(40);
            DateTime fourth_request  =    DateTime.Now.AddSeconds(60);

            _mockDate.Now.Returns(first_request, second_reequest, third_request, fourth_request);
            var mockRule = new RateLimitRule()
            {
                ApiKey = apiKey,
                EndPoint = "/Custom",
                Limit = 3,
                Window = TimeSpan.FromMinutes(10),
                RequestHistory = new LinkedList()
            };

            _rateLimitRuleRepository.Get(apiKey).Returns(mockRule);

            // Act
            _rateLimitService.IsLimitReached(apiKey); 
            _rateLimitService.IsLimitReached(apiKey); 
            _rateLimitService.IsLimitReached(apiKey);
            var result = _rateLimitService.IsLimitReached(apiKey);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Given_After_Limit_Exceeded_Waiting_For_Next_Window_Should_Return_False()
        {
            // Arrange
            string apiKey = "ABC-123";
            DateTime first_request  =   DateTime.Now;
            DateTime second_request =   DateTime.Now.AddMinutes(1);
            DateTime third_request  =   DateTime.Now.AddMinutes(2);
            DateTime fourth_request =   DateTime.Now.AddMinutes(3);
            DateTime fifth_request  =   DateTime.Now.AddMinutes(20);

           
            var mockRule = new RateLimitRule()
            {
                ApiKey = apiKey,
                EndPoint = "/Custom",
                Limit = 3,
                Window = TimeSpan.FromMinutes(10),
                RequestHistory = new LinkedList()
            };

            _rateLimitRuleRepository.Get(apiKey).Returns(mockRule);

            // Act
            _mockDate.Now.Returns(first_request);
            _rateLimitService.IsLimitReached(apiKey);

            _mockDate.Now.Returns(second_request);
            _rateLimitService.IsLimitReached(apiKey);

            _mockDate.Now.Returns(third_request);
            _rateLimitService.IsLimitReached(apiKey);

            _mockDate.Now.Returns(fourth_request);
            var result = _rateLimitService.IsLimitReached(apiKey);

            // 20 minutes has elasped the limit is lifted
            _mockDate.Now.Returns(fifth_request);
            var result2 = _rateLimitService.IsLimitReached(apiKey);

            // Assert
            Assert.IsTrue(result);
            Assert.IsFalse(result2);
        }

        [Test]
        public void Given_RequestLimitOf5Within60MinuteWindowIsExceeded_Should_ReturnTrue()
        {
            // Arrange
            string apiKey = "ABC-123";
            DateTime first_request  =  DateTime.Now;
            DateTime second_request =  first_request.AddMinutes(10);
            DateTime third_request  =  second_request.AddMinutes(10);
            DateTime fourth_request =  third_request.AddMinutes(10);
            DateTime fifth_request  =  fourth_request.AddMinutes(10);
            DateTime six_request    =  fifth_request.AddMinutes(10);


            var mockRule = new RateLimitRule()
            {
                ApiKey = apiKey,
                EndPoint = "/Custom",
                Limit = 5,
                Window = TimeSpan.FromMinutes(60),
                RequestHistory = new LinkedList()
            };

            _rateLimitRuleRepository.Get(apiKey).Returns(mockRule);

            // Act
            _mockDate.Now.Returns(first_request);
            _rateLimitService.IsLimitReached(apiKey);

            _mockDate.Now.Returns(second_request);
            _rateLimitService.IsLimitReached(apiKey);

            _mockDate.Now.Returns(third_request);
            _rateLimitService.IsLimitReached(apiKey);

            _mockDate.Now.Returns(fourth_request);
            _rateLimitService.IsLimitReached(apiKey);

            // 20 minutes has elasped the limit is lifted
            _mockDate.Now.Returns(fifth_request);
            _rateLimitService.IsLimitReached(apiKey);

            _mockDate.Now.Returns(six_request);
            var result = _rateLimitService.IsLimitReached(apiKey);

            // Assert
            Assert.IsTrue(result);
        }

    }
}
