using OpenQA.Selenium;

namespace Selenium.Tests;

[TestFixture]
public class LoginTests : TestBase
{
    [Test]
    public void Login_CaminoFeliz_CredencialesValidas_RedirigeAGames()
    {
        Driver.Navigate().GoToUrl($"{BaseUrl}/login");

        SafeSendKeys(By.Id("username"), "admin");
        SafeSendKeys(By.Id("password"), "Admin123");
        SafeClick(By.Id("login-submit"));

        Wait.Until(driver => driver.Url.Contains("/games"));

        Assert.That(Driver.Url, Does.Contain("/games"));
    }

    [Test]
    public void Login_PruebaNegativa_CredencialesInvalidas_MuestraError()
    {
        Driver.Navigate().GoToUrl($"{BaseUrl}/login");

        SafeSendKeys(By.Id("username"), "admin");
        SafeSendKeys(By.Id("password"), "ClaveIncorrecta");
        SafeClick(By.Id("login-submit"));

        Wait.Until(driver => driver.Url.Contains("error=1"));

        var errorMessage = WaitForElement(By.Id("login-error"));
        Assert.That(errorMessage.Displayed, Is.True);
        Assert.That(errorMessage.Text, Does.Contain("incorrectos"));
    }
    [Test]
    public void Login_PruebaLimites_CamposVacios_MuestraError()
    {
        Driver.Navigate().GoToUrl($"{BaseUrl}/login");

        SafeClick(By.Id("login-submit"));

        Wait.Until(driver => driver.Url.Contains("error=1"));
        var errorMessage = WaitForElement(By.Id("login-error"));
        Assert.That(errorMessage.Text, Does.Contain("incorrectos"));
    }
}