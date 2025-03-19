using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using System;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumLoginTest.Tests
{
    public class LoginTest
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [Test]
        public void TestPositiveLogin()
        {
            driver.Navigate().GoToUrl("https://practicetestautomation.com/practice-test-login/");

            IWebElement usernameField = driver.FindElement(By.Id("username"));
            IWebElement passwordField = driver.FindElement(By.Id("password"));           
            IWebElement loginButton = driver.FindElement(By.Id("submit"));
           
            usernameField.SendKeys("student");
            Thread.Sleep(1000);
            passwordField.SendKeys("Password123");
           
            loginButton.Click();
         

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement logoutButton = wait.Until(ExpectedConditions.ElementIsVisible(By.LinkText("Log out")));
            Assert.That(driver.Url.Contains("practicetestautomation.com/logged-in-successfully/"));
            Assert.IsTrue(driver.PageSource.Contains("Congratulations student. You successfully logged in!"));
            Assert.IsTrue(logoutButton.Displayed, "Logout button is displayed.");
            Thread.Sleep(2000);
        }

        [Test]
        public void TestNegativeUsername()
        {
            driver.Navigate().GoToUrl("https://practicetestautomation.com/practice-test-login/");
            

            IWebElement usernameField = driver.FindElement(By.Id("username"));
            IWebElement passwordField = driver.FindElement(By.Id("password"));            
            IWebElement loginButton = driver.FindElement(By.Id("submit"));

            usernameField.SendKeys("incorrectUser");
            Thread.Sleep(1000);
            passwordField.SendKeys("Password123");
            loginButton.Click();
  
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement errorMessage = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("error")));

            Assert.IsTrue(errorMessage.Displayed, "Error message is displayed.");
            Assert.That(errorMessage.Text, Is.EqualTo("Your username is invalid!"));
            Thread.Sleep(2000);
        }

        [Test]
        public void TestNegativePassword()
        {
            driver.Navigate().GoToUrl("https://practicetestautomation.com/practice-test-login/");

            IWebElement usernameField = driver.FindElement(By.Id("username"));
            IWebElement passwordField = driver.FindElement(By.Id("password"));
            IWebElement loginButton = driver.FindElement(By.Id("submit"));

            usernameField.SendKeys("student");
            Thread.Sleep(1000);
            passwordField.SendKeys("incorrectPassword");
            loginButton.Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement errorMessage = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("error")));
            

            Assert.IsTrue(errorMessage.Displayed, "Error message is displayed.");
            Assert.That(errorMessage.Text, Is.EqualTo("Your password is invalid!"));
            Thread.Sleep(2000);
        }

        [TearDown]
        public void Teardown()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
            }
        }
    }
}
