namespace Ninject.Tests.Unit.BindingActionStrategyTests
{
    using System;
    using FluentAssertions;
    using Moq;
    using Ninject.Activation;
    using Ninject.Activation.Strategies;
    using Ninject.Planning.Bindings;
    using Xunit;

    public class BindingActionStrategyContext
    {
        protected BindingActionStrategy strategy;
        protected Mock<IContext> contextMock;
        protected Mock<IBinding> bindingMock;

        public BindingActionStrategyContext()
        {
            this.contextMock = new Mock<IContext>();
            this.bindingMock = new Mock<IBinding>();
            this.strategy = new BindingActionStrategy();
        }
    }

    public class WhenActivateIsCalled : BindingActionStrategyContext
    {
        [Fact]
        public void StrategyInvokesActivationActionsDefinedInBinding()
        {
            bool action1WasCalled = false;
            bool action2WasCalled = false;

            Action<IContext, object> action1 = (ctx, c) => action1WasCalled = ctx == this.contextMock.Object;
            Action<IContext, object> action2 = (ctx, c) => action2WasCalled = ctx == this.contextMock.Object;
            var actions = new[] { action1, action2 };

            this.contextMock.SetupGet(x => x.Binding).Returns(this.bindingMock.Object);
            this.bindingMock.SetupGet(x => x.ActivationActions).Returns(actions);
            this.strategy.Activate(this.contextMock.Object, new InstanceReference());

            action1WasCalled.Should().BeTrue();
            action2WasCalled.Should().BeTrue();
        }
    }

    public class WhenDeactivateIsCalled : BindingActionStrategyContext
    {
        [Fact]
        public void StrategyInvokesDeactivationActionsDefinedInBinding()
        {
            bool action1WasCalled = false;
            bool action2WasCalled = false;

            Action<IContext, object> action1 = (ctx, c) => action1WasCalled = ctx == this.contextMock.Object;
            Action<IContext, object> action2 = (ctx, c) => action2WasCalled = ctx == this.contextMock.Object;
            var actions = new[] { action1, action2 };

            this.contextMock.SetupGet(x => x.Binding).Returns(this.bindingMock.Object);
            this.bindingMock.SetupGet(x => x.DeactivationActions).Returns(actions);
            this.strategy.Deactivate(this.contextMock.Object, new InstanceReference());

            action1WasCalled.Should().BeTrue();
            action2WasCalled.Should().BeTrue();
        }
    }
}