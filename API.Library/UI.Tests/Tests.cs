using API.Library;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using Todo.API.Client.Models;
using Todo.UI.POM.PageObjects;

namespace Todo.UI.Tests
{
    public class Tests
    {
        IWebDriver driver;
        IApi api;
        private User user;
        private LoginPage login;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("http://localhost:3000/");
            login = new LoginPage(driver);
            api = new Api();
            user = new User
            {
                Email = $"{DateTime.Now.Ticks}@test.com",
                Password = "Password01"
            };
        }

        [Test]
        public void GIVEN_RegisteredUser_WHEN_UserLoginWithValidData_THEN_ShouldLoginUserSuccessfully()
        {
            api.RegisterUser(user);
            login.LoginUser(user.Email, user.Password);
            var home = new HomePage(driver);
            home.IsMyProfileLinkVisible().Should().BeTrue();
        }

        [Test]
        public void GIVEN_LoggedInUserWithActiveTodo_WHEN_UserClicksOnItems_THEN_ShouldSeeTodoCreatedWithAPI()
        {
            api.RegisterUser(user);
            var authUserResponse = api.AuthenticateUser(user);
            var userToken = JsonConvert.DeserializeObject<AuthenticateUserResponse>(authUserResponse.Content).Token;
            var todo = NewToDo();
            api.CreateToDo(todo, userToken);
            login.LoginUser(user.Email, user.Password);
            var home = new HomePage(driver);
            home.IsTodoDisplayed(todo.Title).Should().BeTrue();
        }

        [Test]
        public void GIVEN_LoggedInUser_WHEN_UserAddsNewTodo_THEN_ShouldCreateTodoSuccessfully()
        {
            api.RegisterUser(user);
            login.LoginUser(user.Email, user.Password);
            var home = new HomePage(driver);
            var todo = NewToDo();
            home.CreateTodo(todo);
            home.IsTodoDisplayed(todo.Title).Should().BeTrue();
        }

        private static ToDo NewToDo()
        {
            return new ToDo
            {
                Title = $"Create a PR tomorrow at {DateTime.Now.AddDays(1)}.",
                Description = $"I must create a hotfix PR tomorrow.",
                IsComplete = false
            };
        }

        [TearDown]
        public void CleanUp()
        {
            driver.Quit();
        }
    }
}
