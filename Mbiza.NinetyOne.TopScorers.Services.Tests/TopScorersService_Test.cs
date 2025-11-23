using FluentAssertions;
using Mbiza.NinetyOne.TopScorers.Application.Interfaces;
using Mbiza.NinetyOne.TopScorers.Domain.Entities;
using Moq;
using System.Text;
using Xunit;

namespace Mbiza.NinetyOne.TopScorers.Services.Tests
{
    [TestClass]
    public sealed class TopScorersService_Test
    {
        private readonly Mock<ITopScorerRepository> _repoMock;
        private readonly TopScorersService _service;

        public TopScorersService_Test()
        {
            _repoMock = new Mock<ITopScorerRepository>();
            _service = new TopScorersService(_repoMock.Object);
        }

        [Fact]
        public async Task GetTopScorersAsync_ShouldReturnTopScorersOnly()
        {
            // Arrange
            var data = new List<TopScorer>
            {
                new() { FirstName = "A", SecondName = "One", Score = 10 },
                new() { FirstName = "B", SecondName = "Two", Score = 15 },
                new() { FirstName = "C", SecondName = "Three", Score = 15 }
            };

            _repoMock.Setup(x => x.GetTopScorersAsync(It.IsAny<CancellationToken>())).ReturnsAsync(data);

            // Act
            var result = await _service.GetTopScorersAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Should().OnlyContain(x => x.Score == 15);
        }

        [Fact]
        public async Task GetTopScorerByNameAsync_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            _repoMock.Setup(r => r.GetTopScorerByNameAsync("Liso", It.IsAny<CancellationToken>()))
                     .ReturnsAsync((TopScorer?)null);

            // Act
            var result = await _service.GetTopScorerByNameAsync("Liso");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetTopScorerByNameAsync_ShouldReturnDto_WhenFound()
        {
            // Arrange
            var topSscorer = new TopScorer
            {
                FirstName = "Liso",
                SecondName = "Mbiza",
                Score = 25
            };

            _repoMock.Setup(r => r.GetTopScorerByNameAsync("Liso", It.IsAny<CancellationToken>()))
                     .ReturnsAsync(topSscorer);

            // Act
            var result = await _service.GetTopScorerByNameAsync("Liso");

            // Assert
            result.Should().NotBeNull();
            result!.FirstName.Should().Be("Liso");
            result.Score.Should().Be(25);
        }

        [Fact]
        public async Task CreateTopScorersAsync_String_ShouldParseAndSave()
        {
            // Arrange
            string data = @"FirstName,SecondName,Score\r\nBrown,Njemza,10\r\nLiso,Mbiza,20";

            List<TopScorer> savedTopScorers =
            [
                new() { FirstName = "Brown", SecondName = "Njemza", Score = 10 },
                new() { FirstName = "Liso", SecondName = "Mbiza", Score = 20 },
            ];

            _repoMock.Setup(r => r.AddTopScorersAsync(It.IsAny<List<TopScorer>>()))
                     .ReturnsAsync(savedTopScorers);

            // Act
            var result = await _service.CreateTopScorersAsync(data);

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(x => x.FirstName == "Liso" && x.Score == 20);
        }

        [Fact]
        public async Task CreateTopScorersAsync_Stream_ShouldParseAndSave()
        {
            // Arrange
            string data = @"FirstName,SecondName,Score\r\nBrown,Njemza,10\r\nLiso,Mbiza,20";

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(data));

            List<TopScorer> savedTopScorers =
            [
                new() { FirstName = "Brown", SecondName = "Njemza", Score = 10 },
                new() { FirstName = "Liso", SecondName = "Mbiza", Score = 20 },
            ];

            _repoMock.Setup(r => r.AddTopScorersAsync(It.IsAny<List<TopScorer>>()))
                     .ReturnsAsync(savedTopScorers);

            // Act
            var result = await _service.CreateTopScorersAsync(stream);

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(x => x.FirstName == "Liso");
        }
    }
}
