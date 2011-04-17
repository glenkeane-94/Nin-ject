﻿namespace Ninject.Tests.Integration.ManualReleaseTests
{
    using Ninject.Tests.Fakes;
    using Xunit;
    using Xunit.Should;
    
    public class ManualReleaseContext
    {
        protected StandardKernel kernel;

        public ManualReleaseContext()
        {
            this.kernel = new StandardKernel();            
        }
    }

    public class WhenReleaseIsCalled : ManualReleaseContext
    {
        [Fact]
        public void InstanceIsDeactivated()
        {
            kernel.Bind<NotifiesWhenDisposed>().ToSelf().InSingletonScope();

            var instance = kernel.Get<NotifiesWhenDisposed>();
            kernel.Release(instance);

            instance.IsDisposed.ShouldBeTrue();
        }

        [Fact]
        public void InstanceIsRemovedFromCache()
        {
            kernel.Bind<NotifiesWhenDisposed>().ToSelf().InSingletonScope();

            var instance1 = kernel.Get<NotifiesWhenDisposed>();
            var instance2 = kernel.Get<NotifiesWhenDisposed>();
            instance1.ShouldBeSameAs(instance2);

            kernel.Release(instance1);

            var instance3 = kernel.Get<NotifiesWhenDisposed>();
            instance3.ShouldNotBeSameAs(instance1);
            instance3.ShouldNotBeSameAs(instance2);
        }
    }
}