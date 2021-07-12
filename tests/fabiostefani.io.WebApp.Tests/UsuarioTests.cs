using fabiostefani.io.WebApp.MVC;
using fabiostefani.io.WebApp.Tests.Config;

namespace fabiostefani.io.WebApp.Tests
{
    public class UsuarioTests
    {
        private readonly IntegrationTestsFixture<StartupWebTests> _testsFixture;

        public UsuarioTests(IntegrationTestsFixture<StartupWebTests> testsFixture)
        {
            _testsFixture = testsFixture;
        }
    }
}