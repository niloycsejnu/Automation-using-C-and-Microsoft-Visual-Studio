using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

namespace SeleniumLoginTest.Tests
{
    public class OntrakLoginTest
    {
        private IWebDriver driver;
        private ExtentReports extent;
        private ExtentTest test;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            string reportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ExtentReports", "OntrakLoginTestReport.html");
            var htmlReporter = new ExtentHtmlReporter(reportPath);
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
        }

        [Test]
        public void TestPositiveLogin()
        {
            test = extent.CreateTest("TestPositiveLogin", "Valid Login Credentials Test");

            driver.Navigate().GoToUrl("https://dev.ontrak.onsite.com.au/dashboard2");
            test.Log(Status.Info, "Opened Ontrak Login Page");

            Thread.Sleep(5000);

            driver.FindElement(By.Id("Username")).SendKeys("ju");
            test.Log(Status.Info, "Entered Username: ju");

            driver.FindElement(By.Id("btn-login")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("Password")).SendKeys("ju");
            test.Log(Status.Info, "Entered Password: ju");

            driver.FindElement(By.Id("btn-login")).Click();
            test.Log(Status.Info, "Clicked Login Button");

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement dashboard = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#connect-dashboard-v2")));

            Assert.IsTrue(dashboard.Displayed, "Dashboard is displayed.");
            test.Log(Status.Pass, "Login Successful");
        }

        [Test]
        public void TestNegativeUsername()
        {
            test = extent.CreateTest("TestNegativeUsername", "Invalid Username Test");

            driver.Navigate().GoToUrl("https://dev.ontrak.onsite.com.au/dashboard2");
            test.Log(Status.Info, "Opened Ontrak Login Page");

            Thread.Sleep(5000);

            driver.FindElement(By.Id("Username")).SendKeys("invalidUser");
            test.Log(Status.Info, "Entered Invalid Username: invalidUser");

            driver.FindElement(By.Id("btn-login")).Click();
            Thread.Sleep(2000);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement errorMessage = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("ul > li")));

            Assert.IsTrue(errorMessage.Displayed, "Invalid username");
            Assert.That(errorMessage.Text, Is.EqualTo("Invalid username"));
            test.Log(Status.Pass, "Invalid username test passed.");
        }

        [Test]
        public void TestNegativePassword()
        {
            test = extent.CreateTest("TestNegativePassword", "Invalid Password Test");

            driver.Navigate().GoToUrl("https://dev.ontrak.onsite.com.au/dashboard2");
            test.Log(Status.Info, "Opened Ontrak Login Page");

            Thread.Sleep(5000);

            driver.FindElement(By.Id("Username")).SendKeys("ju");
            test.Log(Status.Info, "Entered Username: ju");

            driver.FindElement(By.Id("btn-login")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("Password")).SendKeys("invalidPass");
            test.Log(Status.Info, "Entered Incorrect Password: invalidPass");

            driver.FindElement(By.Id("btn-login")).Click();
            test.Log(Status.Info, "Clicked Login Button");

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement errorMessage = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("ul > li")));

            Assert.IsTrue(errorMessage.Displayed, "Invalid password");
            test.Log(Status.Pass, "Invalid password test passed.");
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
