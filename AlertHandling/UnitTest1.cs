using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace AlertHandling
{
    public class Tests
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/javascript_alerts");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [TearDown] 
        public void TearDown()
        {
            driver.Quit();
        }

        [Test, Order(1)]
        public void HandleOnclickAlert()
        {
            driver.FindElement(By.XPath("//button[@onclick]")).Click();

            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                Assert.That(alert.Text, Is.EqualTo("I am a JS Alert"));

                alert.Accept();

                string alertMessage = driver.FindElement(By.XPath("//p[@id='result']")).Text;

                Assert.That(alertMessage, Is.EqualTo("You successfully clicked an alert"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        [Test, Order(2)]
        public void HandleConfirmAlert()
        {
            driver.FindElement(By.XPath("//button[@onclick='jsConfirm()']")).Click();
            IAlert alert = driver.SwitchTo().Alert();
            alert.Accept();

            var acceptMessage = driver.FindElement(By.XPath("//p[@id='result']")).Text;

            Assert.That(acceptMessage, Is.EqualTo("You clicked: Ok"));
        }

        [Test, Order(3)]
        public void HandleConfirmCancelAlert()
        {
            driver.FindElement(By.XPath("//button[@onclick='jsConfirm()']")).Click();
            IAlert alert = driver.SwitchTo().Alert();
            alert.Dismiss();

            var acceptMessage = driver.FindElement(By.XPath("//p[@id='result']")).Text;

            Assert.That(acceptMessage, Is.EqualTo("You clicked: Cancel"));
        }

        [Test, Order(4)]
        public void HandlePromptAlert()
        {
            driver.FindElement(By.XPath("//button[@onclick='jsPrompt()']")).Click();
            IAlert alert = driver.SwitchTo().Alert();

            
            string inputAlert = "test";
            alert.SendKeys(inputAlert);
            alert.Accept();

            var result = driver.FindElement(By.Id("result"));

            Assert.That(result.Text, Does.Contain(inputAlert));
        }
    }
}