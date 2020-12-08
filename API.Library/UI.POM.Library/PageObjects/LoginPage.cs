using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace Todo.UI.POM.PageObjects
{
    public class LoginPage
    {
        readonly IWebDriver driver;
        readonly WebDriverWait wait;
        private IWebElement Username => wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Name("username")));
        private IWebElement Password => driver.FindElement(By.Name("password"));
        private IWebElement Login => driver.FindElement(By.XPath("//div[@id='root']//form[@class='form-todo']/button[.='Login']"));

        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(5));
        }
        public void LoginUser(string username, string password)
        {
            Username.SendKeys(username);
            Password.SendKeys(password);
            Login.Click();
        }
    }
}
