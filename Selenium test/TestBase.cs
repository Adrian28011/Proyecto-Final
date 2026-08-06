using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Selenium.Tests;


[SetUpFixture]
public class ReportSetup
{
    public static ExtentReports Extent = null!;
    private static ExtentSparkReporter _reporter = null!;

    [OneTimeSetUp]
    public void RunBeforeAllTests()
    {
        var reportPath = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Reporte.html");
        _reporter = new ExtentSparkReporter(reportPath);
        Extent = new ExtentReports();
        Extent.AttachReporter(_reporter);
    }

    [OneTimeTearDown]
    public void RunAfterAllTests()
    {
        Extent.Flush();
        Console.WriteLine("Reporte generado en: " + Path.Combine(TestContext.CurrentContext.WorkDirectory, "Reporte.html"));
    }
}
public class TestBase
{
    protected IWebDriver Driver = null!;
    protected WebDriverWait Wait = null!;
    protected const string BaseUrl = "https://localhost:7298";
    protected static string Unique(string baseName) => $"{baseName}_{DateTime.Now:HHmmssfff}";

    private ExtentTest _test = null!;
    private string _screenshotDir = null!;

    [SetUp]
    public void Setup()
    {
        var options = new ChromeOptions();
        options.AddArgument("--ignore-certificate-errors");
        Driver = new ChromeDriver(options);
        Driver.Manage().Window.Maximize();
        Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));

        _test = ReportSetup.Extent.CreateTest($"{TestContext.CurrentContext.Test.ClassName}.{TestContext.CurrentContext.Test.Name}");

        _screenshotDir = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Screenshots");
        Directory.CreateDirectory(_screenshotDir);
    }

    [TearDown]
    public void TearDown()
    {
        var status = TestContext.CurrentContext.Result.Outcome.Status;
        var screenshotPath = TakeScreenshot(TestContext.CurrentContext.Test.Name);

        if (status == NUnit.Framework.Interfaces.TestStatus.Passed)
        {
            _test.Pass("Prueba exitosa").AddScreenCaptureFromPath(screenshotPath);
        }
        else
        {
            var message = TestContext.CurrentContext.Result.Message ?? "Sin detalle";
            _test.Fail(message).AddScreenCaptureFromPath(screenshotPath);
        }

        Driver.Quit();
        Driver.Dispose();
    }
    private string TakeScreenshot(string testName)
    {
        var fileName = $"{testName}_{DateTime.Now:HHmmssfff}.png";
        var fullPath = Path.Combine(_screenshotDir, fileName);
        var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
        screenshot.SaveAsFile(fullPath);
        return fullPath;
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