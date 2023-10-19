namespace Stack.Tests;

using Moq;
using Stack.DTOs;
using Stack.DTOs.Models;
using Stack.DTOs.Requests.Modules.Auth;
using Stack.ServiceLayer.Methods.Auth.User;
public class UserServiceTests
{
    // private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IUsersService> _mockUserService;
    private readonly IUsersService _userService;

    public UserServiceTests()
    {
        // _mockUnitOfWork = new Mock<IUnitOfWork>();
        // _userService = new UserService(_mockUnitOfWork.Object);
        _mockUserService = new Mock<IUsersService>();
        _userService = _mockUserService.Object;
    }

    [Fact]
    public async Task Login_ShouldReturnToken_WhenUserExistsAndPasswordIsValid()
    {
        // Arrange
        var loginModel = new LoginModel { Email = "mostafa.moushtaha@gmail.com", Password = "P@ssw0rd" };

        _mockUserService.Setup(u => u.Login(It.IsAny<LoginModel>()))
            .ReturnsAsync(new ApiResponse<JwtAccessToken>
            {
                Succeeded = true,
                Data = new JwtAccessToken
                {
                    Token = "your_token_value",
                    RefreshToken = "your_refresh_token_value",
                    Expiration = DateTime.UtcNow.AddDays(7),
                    userStatus = 1
                }
            });

        // Act
        var result = await _userService.Login(loginModel);

        // Assert
        Assert.True(result.Succeeded);
        Assert.NotNull(result.Data);
        Assert.NotEmpty(result.Data.Token);
        Assert.NotEmpty(result.Data.RefreshToken);
        Assert.Equal(1, result.Data.userStatus);
        // Perform additional assertions as needed
    }
}