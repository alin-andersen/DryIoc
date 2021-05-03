using NUnit.Framework;
using System.Threading.Tasks;
using DryIoc.Testing;

namespace DryIoc.IssuesTests
{
    [TestFixture]
    public class GHIssue391_Deadlock_during_Resolve : ITest
    {
        public int Run()
        {
            Test1();
            return 1;
        }

        [Test]
        public void Test1()
        {
            var container = new Container(rules => rules
                .With(FactoryMethod.ConstructorWithResolvableArguments)
                .WithoutEagerCachingSingletonForFasterAccess()
                .WithoutThrowOnRegisteringDisposableTransient()
                .WithDefaultIfAlreadyRegistered(IfAlreadyRegistered.Replace));

            container.Register(typeof(A), Reuse.Singleton, setup: Setup.With(asResolutionCall: true));
            container.RegisterMany(new [] { typeof(A), typeof(IA) }, typeof(A), Reuse.Singleton, setup: Setup.With(asResolutionCall: true));

            container.Register(typeof(B), Reuse.Singleton, setup: Setup.With(asResolutionCall: true));
            container.RegisterMany(new [] { typeof(B), typeof(IB) }, typeof(B), Reuse.Singleton, setup: Setup.With(asResolutionCall: true));

            container.Register(typeof(C), Reuse.Singleton, setup: Setup.With(asResolutionCall: true));
            container.RegisterMany(new [] { typeof(C), typeof(IC) }, typeof(C), Reuse.Singleton, setup: Setup.With(asResolutionCall: true));

            // the missing dependency
            // container.Register<ID, D>(Reuse.Singleton);

            // A -> B -> C -> D(missing)
            //   \----->

            Assert.Throws<ContainerException>(() => container.Resolve<IA>());

            var ex = Assert.Throws<ContainerException>(() => container.Resolve<IA>());
            Assert.AreSame(Error.NameOf(Error.WaitForScopedServiceIsCreatedTimeoutExpired), ex.ErrorName);
        }

        public interface IA {}
        public interface IB {}
        public interface IC {}
        public interface ID {}

        public class A : IA
        {
            private IB B;
            private IC C;
            public A(IC c,IB b)
            {
                B = b;
                C = c;
            }
        }

        public class B : IB
        {
            private IC C;
            public B(IC c) => C = c;
        }

        public class C : IC
        {
            private ID D;
            public C(ID d) => D = d;
        }

        public class D : ID {}
    }
}