using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace IFramesTesting
{
    public class IFfameTests
    {
        private IWebDriver driver;
        private string url = "https://codepen.io/pervillalva/full/abPoNLd";

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl(url);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        [Test]
        public void HandleIframes_WithIndex()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.TagName("iframe")));

            var dropButton = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@class='dropbtn']")));
            dropButton.Click();

            var dropdownLinks = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.CssSelector(".dropdown-content a")));

            foreach(var link in dropdownLinks)
            {
                Assert.IsTrue(link.Displayed);
                Console.WriteLine(link.Text);
            }
        }
    }
}