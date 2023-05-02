using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Constants;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;
using ESTMS.API.Services;
using ESTMS.API.Services.Users;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;

namespace ESTMS.API.UnitTests;

public class UserServiceTests
{
    private static Mock<IUserRepository> _userRepositoryMock;
    private static Mock<IPlayerRepository> _playerRepositoryMock;
    private static Mock<ITeamManagerRepository> _teamManagerRepositoryMock;
    private static Mock<ITournamentManagerRepository> _tournamentManagerRepositoryMock;

    private Mock<UserManager<ApplicationUser>> _userManagerMock;
    private Mock<IUserStore<ApplicationUser>> _userStoreMock;

    private IUserService _userService;

    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _playerRepositoryMock = new Mock<IPlayerRepository>();
        _teamManagerRepositoryMock = new Mock<ITeamManagerRepository>();
        _tournamentManagerRepositoryMock = new Mock<ITournamentManagerRepository>();

        _userStoreMock = new Mock<IUserStore<ApplicationUser>>();
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            _userStoreMock.Object, null, null, null, null, null, null, null, null);
        _userManagerMock.Object.UserValidators.Add(new UserValidator<ApplicationUser>());
        _userManagerMock.Object.PasswordValidators.Add(new PasswordValidator<ApplicationUser>());

        _userService = new UserService(_userRepositoryMock.Object, _userManagerMock.Object, 
            _playerRepositoryMock.Object, _teamManagerRepositoryMock.Object, 
            _tournamentManagerRepositoryMock.Object);
    }

    [Test]
    public void ChangeUserActivityAsync_UserNotFound_ThrowsException()
    {
        _userRepositoryMock.Setup(x => x.GetUserByIdAsync(It.IsAny<string>())).ReturnsAsync(default(ApplicationUser));

        Assert.ThrowsAsync<BadRequestException>(
            () => _userService.ChangeUserActivityAsync(It.IsAny<string>(), It.IsAny<bool>()));
    }

    [Test]
    public async Task ChangeUserActivityAsync_ValidData_Ok()
    {
        _userRepositoryMock.Setup(x => x.GetUserByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());

        await _userService.ChangeUserActivityAsync(It.IsAny<string>(), It.IsAny<bool>());

        _userRepositoryMock.Verify(x => x.GetUserByIdAsync(It.IsAny<string>()), Times.Once);

        _userRepositoryMock.Verify(x => x.UpdateUserAsync(It.IsAny<ApplicationUser>()), Times.Once);
    }

    [Test]
    public void ChangeUserRoleAsync_UserNotFound_ThrowsException()
    {
        _userRepositoryMock.Setup(x => x.GetUserByIdAsync(It.IsAny<string>())).ReturnsAsync(default(ApplicationUser));

        Assert.ThrowsAsync<BadRequestException>(
            () => _userService.ChangeUserRoleAsync(It.IsAny<string>(), It.IsAny<string>()));
    }

    [Test]
    public void ChangeUserRoleAsync_FailedToRemoveRoleFromUser_ThrowsException()
    {
        _userRepositoryMock.Setup(x => x.GetUserByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
        _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(new List<string>
        {
            "role"
        });
        _userManagerMock.Setup(x => x.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(new IdentityResult());

        Assert.ThrowsAsync<BadRequestException>(
            () => _userService.ChangeUserRoleAsync(It.IsAny<string>(), It.IsAny<string>()));
    }

    public static IEnumerable<TestCaseData> ChangeUserRoleTestCases() 
    {
        yield return new TestCaseData(
            Roles.Player,
            () => _playerRepositoryMock.Verify(x => x.GetPlayerByIdAsync(It.IsAny<string>()), Times.Once),
            () => _playerRepositoryMock.Verify(x => x.RemovePlayerAsync(It.IsAny<Player>()), Times.Once),
            () => _userRepositoryMock.Verify(x => x.CreatePlayerAsync(It.IsAny<Player>()), Times.Once));
        yield return new TestCaseData(
            Roles.TeamManager,
            () => _userRepositoryMock.Verify(x => x.GetTeamManagerByUserIdAsync(It.IsAny<string>()), Times.Once),
            () => _teamManagerRepositoryMock.Verify(x => x.RemoveAsync(It.IsAny<TeamManager>()), Times.Once),
            () => _teamManagerRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<TeamManager>()), Times.Once));
        yield return new TestCaseData(
            Roles.TournamentManager,
            () => _userRepositoryMock.Verify(x => x.GetTournamentManagerByUserIdAsync(It.IsAny<string>()), Times.Once),
            () => _tournamentManagerRepositoryMock.Verify(x => x.RemoveAsync(It.IsAny<TournamentManager>()), Times.Once),
            () => _tournamentManagerRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<TournamentManager>()), Times.Once));
    }

    [Test]
    [TestCaseSource(nameof(ChangeUserRoleTestCases))]
    public async Task ChangeUserRoleAsync_ValidData_RoleIsChanged(string userRole, 
        Action verifyUserGet, Action verifyOldRoleRemoval, Action verifyNewRoleCreation)
    {
        _userRepositoryMock.Setup(x => x.GetUserByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
        _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(new List<string>
        {
            userRole
        });
        _userManagerMock.Setup(x => x.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        await _userService.ChangeUserRoleAsync(It.IsAny<string>(), userRole);

        _userRepositoryMock.Verify(x => x.GetUserByIdAsync(It.IsAny<string>()), Times.Once);
        _userManagerMock.Verify(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()), Times.Once);
        _userManagerMock.Verify(x => x.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), 
            It.IsAny<string>()), Times.Once);

        verifyUserGet.Invoke();
        verifyOldRoleRemoval.Invoke();
        verifyNewRoleCreation.Invoke();
    }

    [Test]
    public void GetUserByIdAsync_UserNotFound_ThrowsException()
    {
        _userRepositoryMock.Setup(x => x.GetUserByIdAsync(It.IsAny<string>())).ReturnsAsync(default(ApplicationUser));

        Assert.ThrowsAsync<BadRequestException>(() => _userService.GetUserByIdAsync(It.IsAny<string>()));
    }

    [Test]
    public async Task GetUserByIdAsync_ValidData_Ok()
    {
        _userRepositoryMock.Setup(x => x.GetUserByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());

        await _userService.GetUserByIdAsync(It.IsAny<string>());

        _userRepositoryMock.Verify(x => x.GetUserByIdAsync(It.IsAny<string>()), Times.Once);
    }
}
