using NuGet.Frameworks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumWaitExercise
{
    public class ImplicitWaits
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            this.driver = new ChromeDriver();
            this.driver.Navigate().GoToUrl("https://practice.bpbonline.com/");
            this.driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [TearDown]
        public void TearDown()
        {
            this.driver.Quit();
            this.driver.Dispose();
        }

        [Test, Order(1)]
        public void Search_Keyboard_ShouldAddToCart()
        {
            driver.FindElement(By.XPath("//form[@name='quick_find']//input")).SendKeys("keyboard");
            driver.FindElement(By.XPath("//form[@name='quick_find']//input[@type='image']")).Click();

            var tableRowElements = driver.FindElements(By.XPath("//table[@class='productListingData']//tbody/tr"));

            Assert.That(tableRowElements.Count(), Is.AtLeast(1));

            foreach (var item in tableRowElements)
            {
                Assert.That(item.Text, Does.Contain("Keyboard"));
            }

            try
            {
                driver.FindElement(By.LinkText("Buy Now")).Click();

                Assert.IsTrue(driver.PageSource.Contains("Keyboard"));
            }
            catch (Exception exception)
            {

                Assert.Fail("Exception caught:" + exception);
            }

        }

        [Test, Order(2)]
        public void Search_Junk_ShouldNotFindItems()
        {
            driver.FindElement(By.XPath("//form[@name='quick_find']//input")).SendKeys("junk");
            driver.FindElement(By.XPath("//form[@name='quick_find']//input[@type='image']")).Click();

            try
            {
                driver.FindElements(By.XPath("//table[@class='productListingData']//tbody/tr"));
            }
            catch (NoSuchElementException exception)
            {
                Assert.Pass("NoSuchElementException is thrown");
            }
            catch (Exception exception)
            {
                Assert.Fail(exception + "Is thrown");
            }
        }
    }
}