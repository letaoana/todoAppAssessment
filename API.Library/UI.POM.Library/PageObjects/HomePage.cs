using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using Todo.API.Client.Models;

namespace Todo.UI.POM.PageObjects
{
    public class HomePage
    {
        readonly IWebDriver driver;
        readonly WebDriverWait wait;
        private readonly By MyProfile = By.LinkText("My Profile");
        private IWebElement AddTodo => wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("[class='svg-inline--fa fa-plus fa-w-14 fa-2x ']")));
        private IWebElement TodoTitle => driver.FindElement(By.Name("title"));
        private IWebElement TodoDescription => driver.FindElement(By.Name("description"));
        private IWebElement SaveTodo => driver.FindElement(By.CssSelector("[class='btn btn-primary w-25 mr-2']"));

        public HomePage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(5));
        }

        public void CreateTodo(ToDo todo)
        {
            AddTodo.Click();
            TodoTitle.SendKeys(todo.Title); ;
            TodoDescription.SendKeys(todo.Description);
            SaveTodo.Click();
        }

        public bool IsMyProfileLinkVisible()
        {
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(MyProfile)).Displayed;
        }

        public bool IsTodoDisplayed(string todoTitle)
        {
            var todo = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath($"/html//div[@id='root']/div/div[@class='container-fluid']/div[@class='row']//h5[.='{todoTitle}']")));
            return todo.Displayed;
        }
    }
}
