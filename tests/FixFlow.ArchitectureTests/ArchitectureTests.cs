using FixFlow.Api.Common.Domain;
using FixFlow.Contracts.Common;
using FluentAssertions;
using NetArchTest.Rules;
using Xunit;

namespace FixFlow.ArchitectureTests;

public class ArchitectureBoundaryTests
{
    private static readonly System.Reflection.Assembly ApiAssembly = typeof(Entity<>).Assembly;
    private static readonly System.Reflection.Assembly ContractsAssembly = typeof(IIntegrationEvent).Assembly;

    [Fact]
    public void Domain_ShouldNotHaveDependencyOn_Infrastructure()
    {
        var result = Types.InAssembly(ApiAssembly)
            .That()
            .ResideInNamespaceEndingWith("Domain")
            .ShouldNot()
            .HaveDependencyOn("FixFlow.Api.Infrastructure")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Domain_ShouldNotHaveDependencyOn_Endpoints()
    {
        var result = Types.InAssembly(ApiAssembly)
            .That()
            .ResideInNamespaceEndingWith("Domain")
            .ShouldNot()
            .HaveDependencyOn("FixFlow.Api.Modules.*.Endpoints")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void IntegrationEvents_ShouldImplement_IIntegrationEvent()
    {
        var result = Types.InAssembly(ContractsAssembly)
            .That()
            .ResideInNamespaceStartingWith("FixFlow.Contracts.Events")
            .And()
            .AreClasses()
            .Should()
            .ImplementInterface(typeof(IIntegrationEvent))
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Contracts_ShouldNotDependOn_Api()
    {
        var result = Types.InAssembly(ContractsAssembly)
            .ShouldNot()
            .HaveDependencyOn("FixFlow.Api")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
