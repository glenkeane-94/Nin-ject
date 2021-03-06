namespace Ninject.Tests.Integration.ExternalInjectionTests
{
    using System;

    using FluentAssertions;
    using Ninject.Tests.Fakes;
    using Xunit;

    public class ExternalInjectionContext : IDisposable
    {
        protected StandardKernel kernel;

        public ExternalInjectionContext()
        {
            this.kernel = new StandardKernel();
        }

        public void Dispose()
        {
            this.kernel.Dispose();
        }
    }

    public class WhenInjectIsCalled : ExternalInjectionContext
    {
        [Fact]
        public void InstanceOfKernelIsInjected()
        {
            this.kernel.Bind<IWeapon>().To<Sword>();

            var warrior = new ExternalWarrior();
            this.kernel.Inject(warrior);

            warrior.Weapon.Should().NotBeNull();
            warrior.Weapon.Should().BeOfType<Sword>();
        }

        [Fact]
        public void InstanceIsNotTrackedForDeactivation()
        {
            var instance = new NotifiesWhenDisposed();

            this.kernel.Inject(instance);
            this.kernel.Dispose();

            instance.IsDisposed.Should().BeFalse();
        }
    }

    public class ExternalWarrior
    {
        [Inject] public IWeapon Weapon { get; set; }
    }
}