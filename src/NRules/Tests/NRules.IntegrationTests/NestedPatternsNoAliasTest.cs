﻿using NRules.Fluent.Dsl;
using NRules.IntegrationTests.TestAssets;
using Xunit;

namespace NRules.IntegrationTests;

public class NestedPatternsNoAliasTest : BaseRulesTestFixture
{
    [Fact]
    public void Fire_EmptySession_DoesNotFire()
    {
        // Arrange - Act
        Session.Fire();

        // Assert
        Verify.Rule().FiredTimes(0);
    }

    [Fact]
    public void Fire_FactTypeOneTwoThreeInserted_FiresOnce()
    {
        // Arrange
        Session.Insert(new FactType1());
        Session.Insert(new FactType2());
        Session.Insert(new FactType3());

        // Act
        Session.Fire();

        // Assert
        Verify.Rule().FiredTimes(1);
    }

    [Fact]
    public void Fire_FactTypeOneTwoFourInserted_FiresOnce()
    {
        // Arrange
        Session.Insert(new FactType1());
        Session.Insert(new FactType2());
        Session.Insert(new FactType4());

        // Act
        Session.Fire();

        // Assert
        Verify.Rule().FiredTimes(1);
    }

    protected override void SetUpRules(Testing.IRepositorySetup setup)
    {
        setup.Rule<TestRule>();
    }

    public class FactType1
    {
    }

    public class FactType2
    {
    }

    public class FactType3
    {
    }

    public class FactType4
    {
    }

    public class TestRule : Rule
    {
        public override void Define()
        {
            When()
                .Match<FactType1>()
                .Match<FactType2>()
                .Or(x => x
                    .Match<FactType3>()
                    .Match<FactType4>());

            Then()
                .Do(ctx => ctx.NoOp());
        }
    }
}
