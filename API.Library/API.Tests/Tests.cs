using API.Library;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Todo.API.Client.Models;

namespace Todo.API.Tests
{
    public class Tests
    {
        IApi api;
        private User admin;
        private User user;

        [SetUp]
        public void Setup()
        {
            admin = new User
            {
                Email = "admin@test.com",
                Password = "test123"
            };
            user = new User
            {
                Email = $"{DateTime.Now.Ticks}@test.com",
                Password = "Password01"
            };
            api = new Api();
        }

        [Test]
        public void GIVEN_RegisterUserEndpointAndValidUser_WHEN_UserSendsRegisterUserRequest_THEN_ShouldGetSuccessfulResponse()
        {
            var response = api.RegisterUser(user);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Test]
        public void GIVEN_RegisteredUserAndAuthenticateUserEndpoint_WHEN_UserSendsAuthenticateUserRequest_THEN_ShouldGetSuccessfulResponse()
        {
            api.RegisterUser(user);
            var actualResponse = api.AuthenticateUser(user);
            var userToken = JsonConvert.DeserializeObject<AuthenticateUserResponse>(actualResponse.Content).Token;
            actualResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            userToken.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void GIVEN_AuthenticatedUserAndCreateTodoEndpoint_WHEN_UserSendsCreateTodoRequest_THEN_ShouldCreateTodoSuccessfully()
        {
            api.RegisterUser(user);
            var authUserResponse = api.AuthenticateUser(user);
            var userToken = JsonConvert.DeserializeObject<AuthenticateUserResponse>(authUserResponse.Content).Token;
            var todo = new ToDo
            {
                Title = $"{user.Email}_todo"
            };
            api.CreateToDo(todo, userToken);

            var actualTodo = GetCreatedToDo(userToken).FirstOrDefault();
            actualTodo.Title.Should().Be(todo.Title);
            actualTodo.IsComplete.Should().BeFalse();
        }

        [Test]
        public void GIVEN_AuthenticatedUserAndActiveTodo_WHEN_UserCompletesTodoAndGetsTodoList_THEN_IsCompleteFieldFromResponseShouldBeTrue()
        {
            api.RegisterUser(user);
            var authUserResponse = api.AuthenticateUser(user);
            var userToken = JsonConvert.DeserializeObject<AuthenticateUserResponse>(authUserResponse.Content).Token;
            api.CreateToDo(CreateToDo(), userToken);

            var actualTodo = GetCreatedToDo(userToken).FirstOrDefault();
            api.CompleteToDo(actualTodo.Id, userToken, true);

            actualTodo = GetCreatedToDo(userToken).FirstOrDefault();
            actualTodo.IsComplete.Should().BeTrue();
        }

        [Test]
        public void GIVEN_AuthenticatedAdminAndGetUsersEndpoint_WHEN_AdminSendsGetUsersRequest_THEN_ShouldGetAllRegisteredUsersSuccessfully()
        {
            api.RegisterUser(user);
            var secondUser = new User()
            {
                Email = $"{DateTime.Now.Ticks}@test.com",
                Password = "Password01"
            };
            api.RegisterUser(secondUser);

            var authAdminResponse = api.AuthenticateUser(admin);
            var adminToken = JsonConvert.DeserializeObject<AuthenticateUserResponse>(authAdminResponse.Content).Token;
            var response = api.GetUsers(adminToken);

            var actualRegisteredUsers = JsonConvert.DeserializeObject<List<User>>(response.Content);
            actualRegisteredUsers.FirstOrDefault(u => u.Email.Equals(user.Email)).Should().NotBeNull();
            actualRegisteredUsers.FirstOrDefault(u => u.Email.Equals(secondUser.Email)).Should().NotBeNull();
            actualRegisteredUsers.FirstOrDefault(u => u.Email.Equals(admin.Email)).Should().NotBeNull();
        }

        [Test]
        public void GIVEN_AuthenticatedUserAndActiveTodo_WHEN_UserSendsRequestToUpdateTodo_THEN_TodoShouldBeUpdatedSuccessfully()
        {
            api.RegisterUser(user);
            var authUserResponse = api.AuthenticateUser(user);
            var userToken = JsonConvert.DeserializeObject<AuthenticateUserResponse>(authUserResponse.Content).Token;
            var todo = CreateToDo();
            api.CreateToDo(todo, userToken);

            var actual = GetCreatedToDo(userToken).FirstOrDefault();
            todo.Description = "Join friends for a hike tomorrow morning.";
            api.UpdateToDo(todo, actual.Id, userToken);
            actual = GetCreatedToDo(userToken).FirstOrDefault();
            actual.Description.Should().Be(todo.Description);
        }

        private static ToDo CreateToDo()
        {
            return new ToDo
            {
                Title = $"Create a PR tomorrow at {DateTime.Now.AddDays(1)}.",
                Description = $"I must create a hotfix PR tomorrow.",
                IsComplete = false
            };
        }

        private List<ToDo> GetCreatedToDo(string userToken)
        {
            var getTodosResponse = api.GetToDos(userToken);
            return JsonConvert.DeserializeObject<List<ToDo>>(getTodosResponse.Content);
        }

        [TearDown]
        public void CleanUp()
        {

        }
    }
}
