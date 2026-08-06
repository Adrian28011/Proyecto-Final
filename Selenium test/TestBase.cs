using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Selenium.Tests;

public class TestBase
{
    protected IWebDriver Driver = null!;
    protected WebDriverWait Wait = null!;
    protected const string BaseUrl = "https://localhost:7298";
    protected static string Unique(string baseName) => $"{baseName}_{DateTime.Now:HHmmssfff}";

    [SetUp]
    public void Setup()
    {
        var options = new ChromeOptions();
        options.AddArgument("--ignore-certificate-errors");
        Driver = new ChromeDriver(options);
        Driver.Manage().Window.Maximize();
        Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
    }

    [TearDown]
    public void TearDown()
    {
        Driver.Quit();
        Driver.Dispose();
    }

    protected IWebElement WaitForElement(By by)
    {
        return Wait.Until(driver =>
        {
            try
            {
                var element = driver.FindElement(by);
                return element.Displayed ? element : null;
            }
            catch (StaleElementReferenceException)
            {
                return null;
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        });
    }

    protected void SafeSendKeys(By by, string text)
    {
        Wait.Until(driver =>
        {
            try
            {
                driver.FindElement(by).SendKeys(text);
                return true;
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
        });
    }

    protected void SafeClick(By by)
    {
        Wait.Until(driver =>
        {
            try
            {
                driver.FindElement(by).Click();
                return true;
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
        });
    }
}