using OpenQA.Selenium;

namespace Selenium.Tests;

[TestFixture]
public class GamesTests : TestBase
{
    [SetUp]
    public void LoginBeforeEachTest()
    {
        Driver.Navigate().GoToUrl($"{BaseUrl}/login");
        SafeSendKeys(By.Id("username"), "admin");
        SafeSendKeys(By.Id("password"), "Admin123");
        SafeClick(By.Id("login-submit"));
        Wait.Until(driver => driver.Url.Contains("/games"));
    }

    [Test]
    public void CrearJuego_CaminoFeliz_DatosValidos_AparecEnTabla()
    {
        SafeSendKeys(By.Id("game-title"), "The Legend of Zelda");
        SafeSendKeys(By.Id("game-genre"), "Aventura");
        SafeSendKeys(By.Id("game-platform"), "Switch");
        SafeSendKeys(By.Id("game-price"), "59.99");
        SafeSendKeys(By.Id("game-stock"), "10");
        SafeClick(By.Id("game-save-btn"));

        var table = WaitForElement(By.Id("games-table"));
        Assert.That(table.Text, Does.Contain("The Legend of Zelda"));
    }

    [Test]
    public void CrearJuego_PruebaNegativa_TituloVacio_MuestraError()
    {
        SafeSendKeys(By.Id("game-genre"), "Accion");
        SafeSendKeys(By.Id("game-platform"), "PC");
        SafeSendKeys(By.Id("game-price"), "29.99");
        SafeSendKeys(By.Id("game-stock"), "5");
        SafeClick(By.Id("game-save-btn"));

        var error = WaitForElement(By.CssSelector(".validation-message"));
        Assert.That(error.Text, Does.Contain("título"));
    }

    [Test]
    public void CrearJuego_PruebaLimites_PrecioMaximoPermitido_SeGuarda()
    {
        SafeSendKeys(By.Id("game-title"), "Juego Precio Limite");
        SafeSendKeys(By.Id("game-genre"), "RPG");
        SafeSendKeys(By.Id("game-platform"), "PS5");
        SafeSendKeys(By.Id("game-price"), "9999");
        SafeSendKeys(By.Id("game-stock"), "1");
        SafeClick(By.Id("game-save-btn"));

        var table = WaitForElement(By.Id("games-table"));
        Assert.That(table.Text, Does.Contain("Juego Precio Limite"));
    }
    [Test]
    public void EditarJuego_CaminoFeliz_CambiaTitulo_SeActualizaEnTabla()
    {
        var tituloOriginal = Unique("Juego Original");
        var tituloEditado = Unique("Juego Editado");

        SafeSendKeys(By.Id("game-title"), tituloOriginal);
        SafeSendKeys(By.Id("game-genre"), "Accion");
        SafeSendKeys(By.Id("game-platform"), "Xbox");
        SafeSendKeys(By.Id("game-price"), "39.99");
        SafeSendKeys(By.Id("game-stock"), "3");
        SafeClick(By.Id("game-save-btn"));

        WaitForElement(By.Id("games-table"));

        var row = Driver.FindElements(By.CssSelector("tr[data-testid='game-row']"))
            .First(r => r.Text.Contains(tituloOriginal));
        var rowId = row.GetAttribute("id")!.Replace("game-row-", "");

        SafeClick(By.Id($"game-edit-{rowId}"));

        Wait.Until(driver => driver.FindElement(By.Id("game-title")).GetAttribute("value") == tituloOriginal);

        var titleInput = Driver.FindElement(By.Id("game-title"));
        titleInput.Clear();
        titleInput.SendKeys(tituloEditado);
        SafeClick(By.Id("game-save-btn"));

        var table = WaitForElement(By.Id("games-table"));
        Assert.That(table.Text, Does.Contain(tituloEditado));
        Assert.That(table.Text, Does.Not.Contain(tituloOriginal));
    }

    [Test]
    public void EditarJuego_PruebaLimites_PrecioFueraDeRango_MuestraError()
    {
        SafeSendKeys(By.Id("game-title"), "Juego Para Editar Limite");
        SafeSendKeys(By.Id("game-genre"), "Deportes");
        SafeSendKeys(By.Id("game-platform"), "PS5");
        SafeSendKeys(By.Id("game-price"), "19.99");
        SafeSendKeys(By.Id("game-stock"), "2");
        SafeClick(By.Id("game-save-btn"));

        WaitForElement(By.Id("games-table"));

        var row = Driver.FindElements(By.CssSelector("tr[data-testid='game-row']"))
            .First(r => r.Text.Contains("Juego Para Editar Limite"));
        var rowId = row.GetAttribute("id")!.Replace("game-row-", "");

        SafeClick(By.Id($"game-edit-{rowId}"));

        var priceInput = WaitForElement(By.Id("game-price"));
        priceInput.Clear();
        priceInput.SendKeys("10000"); 
        SafeClick(By.Id("game-save-btn"));

        var error = WaitForElement(By.CssSelector(".validation-message"));
        Assert.That(error.Text, Does.Contain("precio"));
    }
    [Test]
    public void EditarJuego_PruebaNegativa_TituloVacio_MuestraError()
    {
        SafeSendKeys(By.Id("game-title"), "Juego Para Editar Negativo");
        SafeSendKeys(By.Id("game-genre"), "Accion");
        SafeSendKeys(By.Id("game-platform"), "PC");
        SafeSendKeys(By.Id("game-price"), "25.00");
        SafeSendKeys(By.Id("game-stock"), "4");
        SafeClick(By.Id("game-save-btn"));

        WaitForElement(By.Id("games-table"));

        var row = Driver.FindElements(By.CssSelector("tr[data-testid='game-row']"))
            .First(r => r.Text.Contains("Juego Para Editar Negativo"));
        var rowId = row.GetAttribute("id")!.Replace("game-row-", "");

        SafeClick(By.Id($"game-edit-{rowId}"));

        Wait.Until(driver => driver.FindElement(By.Id("game-title")).GetAttribute("value") == "Juego Para Editar Negativo");

        var titleInput = Driver.FindElement(By.Id("game-title"));
        titleInput.Clear();
        SafeClick(By.Id("game-save-btn"));

        var error = WaitForElement(By.CssSelector(".validation-message"));
        Assert.That(error.Text, Does.Contain("título"));
    }

    [Test]
    public void EliminarJuego_CaminoFeliz_DesaparecedeLaTabla()
    {
        SafeSendKeys(By.Id("game-title"), "Juego A Eliminar");
        SafeSendKeys(By.Id("game-genre"), "Terror");
        SafeSendKeys(By.Id("game-platform"), "PC");
        SafeSendKeys(By.Id("game-price"), "14.99");
        SafeSendKeys(By.Id("game-stock"), "1");
        SafeClick(By.Id("game-save-btn"));

        WaitForElement(By.Id("games-table"));

        var row = Driver.FindElements(By.CssSelector("tr[data-testid='game-row']"))
            .First(r => r.Text.Contains("Juego A Eliminar"));
        var rowId = row.GetAttribute("id")!.Replace("game-row-", "");

        SafeClick(By.Id($"game-delete-{rowId}"));

        Wait.Until(driver => !driver.PageSource.Contains("Juego A Eliminar"));
        Assert.That(Driver.PageSource, Does.Not.Contain("Juego A Eliminar"));
    }

    [Test]
    public void EliminarJuego_PruebaNegativa_ElementoYaEliminado_NoRompeLaApp()
    {
        SafeSendKeys(By.Id("game-title"), "Juego Doble Eliminacion");
        SafeSendKeys(By.Id("game-genre"), "Puzzle");
        SafeSendKeys(By.Id("game-platform"), "PC");
        SafeSendKeys(By.Id("game-price"), "9.99");
        SafeSendKeys(By.Id("game-stock"), "1");
        SafeClick(By.Id("game-save-btn"));

        WaitForElement(By.Id("games-table"));

        var row = Driver.FindElements(By.CssSelector("tr[data-testid='game-row']"))
            .First(r => r.Text.Contains("Juego Doble Eliminacion"));
        var rowId = row.GetAttribute("id")!.Replace("game-row-", "");

        SafeClick(By.Id($"game-delete-{rowId}"));
        Wait.Until(driver => !driver.PageSource.Contains("Juego Doble Eliminacion"));

        var buttons = Driver.FindElements(By.Id($"game-delete-{rowId}"));
        Assert.That(buttons.Count, Is.EqualTo(0));
    }

    [Test]
    public void EliminarJuego_PruebaLimites_UltimoJuegoDeLaTabla_MuestraEstadoVacio()
    {
        SafeSendKeys(By.Id("game-title"), "Ultimo Juego Prueba");
        SafeSendKeys(By.Id("game-genre"), "Estrategia");
        SafeSendKeys(By.Id("game-platform"), "PC");
        SafeSendKeys(By.Id("game-price"), "1.00");
        SafeSendKeys(By.Id("game-stock"), "0"); 
        SafeClick(By.Id("game-save-btn"));

        WaitForElement(By.Id("games-table"));

        var row = Driver.FindElements(By.CssSelector("tr[data-testid='game-row']"))
            .First(r => r.Text.Contains("Ultimo Juego Prueba"));
        var rowId = row.GetAttribute("id")!.Replace("game-row-", "");

        SafeClick(By.Id($"game-delete-{rowId}"));
        Wait.Until(driver => !driver.PageSource.Contains("Ultimo Juego Prueba"));

        Assert.That(Driver.PageSource, Does.Not.Contain("Ultimo Juego Prueba"));
    }
}