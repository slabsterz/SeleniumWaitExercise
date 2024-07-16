using NuGet.Frameworks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace ExplicitWaits
{

    public class ExplicitWaitsTest
    {
        private IWebDriver driver;
        private string url = "https://practice.bpbonline.com/";
        private WebDriverWait? wait;

        [SetUp]
        public void Setup()
        {
            this.driver = new ChromeDriver();
            this.driver.Navigate().GoToUrl(url);
            this.driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

        }

        [TearDown]
        public void Teardown()
        {
            this.driver.Quit();
            this.driver.Dispose();
        }

        [Test, Order(1)]
        public void Search_Keyboard_ExplicitWait()
        {
            driver.FindElement(By.XPath("//form[@name='quick_find']//input")).SendKeys("keyboard");
            driver.FindElement(By.XPath("//form[@name='quick_find']//input[@type='image']")).Click();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);

            try
            {
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                //var buyButton = wait.Until(ExpectedConditions.ElementExists(By.LinkText("Buy Now")));
                var buyButton = wait.Until(x => x.FindElement(By.LinkText("Buy Now")));
                buyButton.Click();

                var cartMessage = wait.Until(ExpectedConditions.ElementExists(By.XPath("//div[@id='bodyContent']//h1")));
                var totalValue = wait.Until(ExpectedConditions.ElementExists(By.XPath("//div[@id='bodyContent']//strong[text()='Sub-Total: $69.99']")));
                var checkoutButton = wait.Until(ExpectedConditions.ElementExists(By.XPath("//div[@class='buttonSet']//a")));

                Assert.IsTrue(cartMessage.Displayed);
                Assert.IsTrue(totalValue.Displayed);
                Assert.IsTrue(checkoutButton.Displayed);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        [Test, Order(2)]
        public void Search_Junk_ShouldNotFindElements()
        {
            driver.FindElement(By.XPath("//form[@name='quick_find']//input")).SendKeys("junk");
            driver.FindElement(By.XPath("//form[@name='quick_find']//input[@type='image']")).Click();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);

            try
            {
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                var searchResultMessage = wait.Until(x => x.FindElement(By.XPath("//div[@id='bodyContent']//p"))).Text;
                var shoppingCartValue = wait.Until(x => x.FindElement(By.XPath("//div[@class='ui-widget-content infoBoxContents' and text()='0 items']"))).Text;
                var button = wait.Until(x => x.FindElement(By.LinkText("Buy Now")));
                button.Click();

                Assert.That(searchResultMessage, Is.EqualTo("There is no product that matches the search criteria."));
                Assert.That(shoppingCartValue, Is.EqualTo("0 items"));
                Assert.Fail("Button not present");
            }
            catch (WebDriverTimeoutException)
            {
                Assert.Pass("Webdriver exception is thrown");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}