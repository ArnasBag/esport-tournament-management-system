using ESTMS.API.DataAccess.Data;
using ESTMS.API.Host.Models.Auth;
using ESTMS.API.Host.Models.Users;
using ESTMS.API.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Text.Json;
using Xunit;

namespace ESTMS.API.IntegrationTests;

public class UserControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public UserControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        factory.DefaultUserId = "5";

        var scope = factory.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        // database is now shared across tests
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        foreach (var user in context.Users)
        {
            context.Remove(user);
        }
    
        if (!context.Roles.Any())
        {
            context.Roles.Add(new()
            {
                Name = "player",
                NormalizedName = "PLAYER",
                Id = Guid.NewGuid().ToString()
            });

            context.Roles.Add(new()
            {
                Name = "teammanager",
                NormalizedName = "TEAMMANAGER",
                Id = Guid.NewGuid().ToString()
            });

            context.SaveChanges();
        }

        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Test");
    }

    private async Task CreateUser(RegistrationRequest registerRequest)
    {
        var requestContent = new StringContent(JsonSerializer.Serialize(registerRequest),
            Encoding.UTF8, "application/json");

        var postResult = await _client.PostAsync("/auth/register", requestContent);
        postResult.EnsureSuccessStatusCode();
    }

    private async Task<UserResponse> GetCreatedUser() 
    {
        var getResponse = await _client.GetAsync("/users");

        var usersJson = await getResponse.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        var users = JsonSerializer.Deserialize<List<UserResponse>>(usersJson, options);
        var user = users.Single();

        return user;
    }

    [Fact]
    public async Task RegisterUser_ValidData_UserRegistered()
    {
        var registerRequest = new RegistrationRequest
        {
            Email = "test1@test.com",
            Username = "test1",
            Password = "test123"
        };

        await CreateUser(registerRequest);
        var user = await GetCreatedUser();

        Assert.Equal(registerRequest.Email, user.Email);
        Assert.Equal(registerRequest.Username, user.Username);
    }

    [Fact]
    public async Task ChangeUserRole()
    {
        var registerRequest = new RegistrationRequest
        {
            Email = "test2@test.com",
            Username = "test2",
            Password = "test123"
        };

        await CreateUser(registerRequest);
        var user = await GetCreatedUser();

        var changeUserRoleRequest = new ChangeUserRoleRequest
        {
            Role = "teammanager"
        };

        var putContent = new StringContent(JsonSerializer.Serialize(changeUserRoleRequest),
            Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Put, $"/users/{user.Id}/role");
        request.Content = putContent;

        var putResponse = await _client.SendAsync(request);
        putResponse.EnsureSuccessStatusCode();

        var getMeRequest = new HttpRequestMessage(HttpMethod.Get, "/auth/me");
        getMeRequest.Headers.Add("UserId", user.Id);

        var userWithRoleResponse = await _client.SendAsync(getMeRequest);
        userWithRoleResponse.EnsureSuccessStatusCode();

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        var userWithRole = JsonSerializer.Deserialize<UserWithRole>(await userWithRoleResponse.Content.ReadAsStringAsync(), options);

        Assert.Equal(changeUserRoleRequest.Role, userWithRole.Role);
    }

    [Fact]
    public async Task Login_ValidData_TokenReturned()
    {
        var registerRequest = new RegistrationRequest
        {
            Email = "test3@test.com",
            Username = "test3",
            Password = "test123"
        };

        await CreateUser(registerRequest);
        var user = await GetCreatedUser();

        var loginRequest = new LoginRequest
        {
            Email = registerRequest.Email,
            Password = registerRequest.Password,
        };

        var loginContent = new StringContent(JsonSerializer.Serialize(loginRequest),
            Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/auth/login", loginContent);
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task ChangeUserActivationStatus_ValidData_StatusChanged()
    {
        var registerRequest = new RegistrationRequest
        {
            Email = "test4@test.com",
            Username = "test4",
            Password = "test123"
        };

        await CreateUser(registerRequest);
        var user = await GetCreatedUser();

        var changeActivationStatusRequest = new ChangeUserActivationStatusRequest
        {
            ActivationStatus = false
        };

        var changeActivationStatusRequestJson = new StringContent(JsonSerializer.Serialize(changeActivationStatusRequest),
            Encoding.UTF8, "application/json");

        var response = await _client.PutAsync($"/users/{user.Id}/activation-status", changeActivationStatusRequestJson);
        response.EnsureSuccessStatusCode();
    }
}
