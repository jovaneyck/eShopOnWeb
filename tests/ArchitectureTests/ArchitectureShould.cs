﻿using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.Fluent;
using ArchUnitNET.xUnitV3;
using Xunit;

using static ArchUnitNET.Fluent.ArchRuleDefinition;
using Assembly = System.Reflection.Assembly;

namespace ArchitectureTests;

public class ArchitectureShould
{
    private static Assembly _publicApiAssembly = Assembly.Load("PublicApi");
    private static Assembly _applicationCoreAssembly = Assembly.Load("ApplicationCore");
    private static Assembly _infrastructureAssembly = Assembly.Load("Infrastructure");
    private static Assembly _efCoreAssembly = Assembly.Load("Microsoft.EntityFrameworkCore");

    private static readonly Architecture Architecture = new ArchLoader() 
        .LoadAssemblies(
            _publicApiAssembly,
            _applicationCoreAssembly,
            _infrastructureAssembly,
            _efCoreAssembly)
        .Build();
    
    private readonly IObjectProvider<IType> PublicApi =
        Types().That().ResideInAssembly(_publicApiAssembly).As("PublicApi");
    
    private readonly IObjectProvider<IType> ApplicationCore =
        Types().That().ResideInAssembly(_applicationCoreAssembly).As("ApplicationCore");
    
    private readonly IObjectProvider<IType> Infrastructure =
        Types().That().ResideInAssembly(_infrastructureAssembly).As("Infrastructure");

    private readonly IObjectProvider<IType> EfCore =
        Types().That().ResideInAssembly(_infrastructureAssembly).As("EfCore");

    [Fact]
    public void SanityCheck()
    {
        Assert.NotEmpty(ApplicationCore.GetObjects(Architecture));
        Assert.NotEmpty(Infrastructure.GetObjects(Architecture));
        Assert.NotEmpty(PublicApi.GetObjects(Architecture));
        Assert.NotEmpty(EfCore.GetObjects(Architecture));
    }
    
    [Fact]
    public void CoreShouldNotDependOnInfrastructure()
    {
        Types().That()
            .Are(ApplicationCore)
            .Should()
            .NotDependOnAny(Infrastructure)
            .Because("DIP")
            .Check(Architecture);
    }
    
    [Fact]
    public void CoreShouldNotDependOnEntityFramework()
    {
        Types().That()
            .Are(ApplicationCore)
            .Should()
            .NotDependOnAny(EfCore)
            .Because("DIP")
            .Check(Architecture);
    }
}
