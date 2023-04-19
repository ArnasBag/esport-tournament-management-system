using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;
using ESTMS.API.Services;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;

namespace ESTMS.API.UnitTests;

public class AuthServiceTests
{
    private Mock<IUserStore<ApplicationUser>> _userStoreMock;
    private Mock<UserManager<ApplicationUser>> _userManagerMock;
    private Mock<ITokenProvider> _tokenProviderMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private IAuthService _authService;

    [SetUp]
    public void Setup()
    {
        _userStoreMock = new Mock<IUserStore<ApplicationUser>>();
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            _userStoreMock.Object, null, null, null, null, null, null, null, null);
        _userManagerMock.Object.UserValidators.Add(new UserValidator<ApplicationUser>());
        _userManagerMock.Object.PasswordValidators.Add(new PasswordValidator<ApplicationUser>());

        _tokenProviderMock = new Mock<ITokenProvider>();
        _userRepositoryMock = new Mock<IUserRepository>();

        _authService = new AuthService(_userManagerMock.Object, _tokenProviderMock.Object, _userRepositoryMock.Object);
    }

    [Test]
    public async Task LoginUserAsync_CorrectInput_AllDependenciesCalled()
    {
        _userManagerMock.Setup(x => x.FindByEmailAsync(
            It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
        _userManagerMock.Setup(x => x.CheckPasswordAsync(
            It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(true);
        _userManagerMock.Setup(x => x.GetRolesAsync(
            It.IsAny<ApplicationUser>())).ReturnsAsync(new List<string>());

        await _authService.LoginUserAsync(It.IsAny<string>(), It.IsAny<string>());

        _userManagerMock.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Once);
        _userManagerMock.Verify(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
        _tokenProviderMock.Verify(x => x.GetToken(It.IsAny<ApplicationUser>(), It.IsAny<List<string>>()), Times.Once);
    }

    [Test]
    public void LoginUserAsync_UserDoesNotExist_ThrowsException()
    {
        Assert.ThrowsAsync<BadRequestException>(() => _authService.LoginUserAsync(It.IsAny<string>(), It.IsAny<string>()));
    }

    [Test]
    public void LoginUserAsync_PasswordDoesNotMatchEmail_ThrowsException()
    {
        _userManagerMock.Setup(x => x.FindByEmailAsync(
            It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
        _userManagerMock.Setup(x => x.CheckPasswordAsync(
            It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(false);

        Assert.ThrowsAsync<BadRequestException>(() => _authService.LoginUserAsync(It.IsAny<string>(), It.IsAny<string>()));
    }

    [Test]
    public async Task RegisterUserAsync_CorrectInput_AllDependenciesCalled()
    {
        _userManagerMock.Setup(x => x.CreateAsync(
            It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        await _authService.RegisterUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

        _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
        _userRepositoryMock.Verify(x => x.CreatePlayerAsync(It.IsAny<Player>()), Times.Once);
    }

    [Test]
    public void RegisterUserAsync_UserAlreadyExists_ThrowsException()
    {
        _userManagerMock.Setup(x => x.CreateAsync(
            It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed());

        Assert.ThrowsAsync<BadRequestException>(() => _authService.RegisterUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
    }
}
