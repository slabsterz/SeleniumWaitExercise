using NuGet.Frameworks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;

namespace HandlingTabs
{
    public class TabHandlingTests
    {
        private IWebDriver driver;
        private string url = "https://the-internet.herokuapp.com/windows";

        [SetUp]
        public void Setup()
        {
            this.driver = new ChromeDriver();
            this.driver.Navigate().GoToUrl(url);
            this.driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [TearDown]
        public void TearDown()
        {
            this.driver.Quit();
            this.driver.Dispose();             
        }

        [Test]
        public void HandlingMultipleWindows()
        {
            driver.FindElement(By.XPath("//div[@id='content']//a")).Click();

            ReadOnlyCollection<string> handles = driver.WindowHandles;

            Assert.That(handles.Count, Is.EqualTo(2));

            driver.SwitchTo().Window(handles[1]);

            string newWindowContent = driver.PageSource;

            Assert.That(newWindowContent, Does.Contain("New Window"));

            string path = Path.Combine(Directory.GetCurrentDirectory(), "windows.txt");

            if(File.Exists(path))
            {
                File.Delete(path);
            }

            File.AppendAllText(path, "Window handle for new window: " + driver.CurrentWindowHandle + "\n\n");
            File.AppendAllText(path, "Current tab contains: " + newWindowContent + "\n\n");

            driver.Close();

            driver.SwitchTo().Window(handles[0]);
            var initialPageContent = driver.PageSource;

            Assert.IsTrue(initialPageContent.Contains("Opening a new window"));

            File.AppendAllText(path, $"Window handle for initial window: {driver.CurrentWindowHandle} \n\n");
            File.AppendAllText(path, $"Initial tab contains: {initialPageContent} \n\n");
        }

        [Test, Order(2)]
        public void Handling_NoSuchWindowException()
        {
            driver.FindElement(By.XPath("//div[@id='content']//a")).Click();

            var windowHandles = driver.WindowHandles;

            driver.SwitchTo().Window(windowHandles[1]);
            driver.Close();

            try
            {                
                driver.SwitchTo().Window(windowHandles[1]);
            }
            catch (NoSuchWindowException ex)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "windowExceptions.txt");

                File.AppendAllText(path, ex.Message);
                Assert.Pass("Exception handled");
            }
            catch (Exception ex)
            {
                Assert.Fail("Caught exception: " + ex.Message);
            }
            finally
            {
                driver.SwitchTo().Window(windowHandles[0]);
            }
        }
    }
}