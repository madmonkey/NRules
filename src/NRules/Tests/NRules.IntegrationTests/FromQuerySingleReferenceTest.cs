﻿using System.Collections.Generic;
using System.Linq;
using NRules.Fluent.Dsl;
using NRules.IntegrationTests.TestAssets;
using Xunit;

namespace NRules.IntegrationTests;

public class FromQuerySingleReferenceTest : BaseRulesTestFixture
{
    [Fact]
    public void From_MultipleKeys_ShouldFireOnceForEachGroup()
    {
        // Arrange
        var keys = new[] { "K1", "K2", "K2", "K1" };
        var facts = keys.Select(k => new Fact { Value = k })
            .ToArray();

        Session.InsertAll(facts);

        // Act
        Session.Fire();

        // Assert
        Verify.Rule().FiredTimes(2);
    }

    [Fact]
    public void From_SingleKey_FiresOnce()
    {
        // Arrange
        var keys = new[] { "K1", "K1", "K1", "K1" };
        var facts = keys.Select(k => new Fact { Value = k })
            .ToArray();

        Session.InsertAll(facts);

        // Act
        Session.Fire();

        // Assert
        Verify.Rule().FiredTimes(1);
    }

    [Fact]
    public void From_HandlesRetracts_FiresThenDoesNotFire()
    {
        // Arrange
        var keys = new[] { "K1", "K2", "K2", "K1" };
        var facts = keys.Select(k => new Fact { Value = k })
            .ToArray();

        Session.InsertAll(facts);

        // Act
        Session.Fire();

        // Assert
        Verify.Rule().FiredTimes(2);

        // Arrange
        Session.RetractAll(facts);

        // Act
        Session.Fire();

        // Assert
        Verify.Rule().FiredTimes(2);
    }

    protected override void SetUpRules(Testing.IRepositorySetup setup)
    {
        setup.Rule<FromQuerySingleReferenceRule>();
    }

    public class Fact
    {
        public string Value { get; set; }
    }

    public class FromQuerySingleReferenceRule : Rule
    {
        public override void Define()
        {
            IEnumerable<Fact> factsAll = null;
            IGrouping<string, Fact> factsGrouped = null;

            When()
                .Query(() => factsAll, q => q
                    .Match<Fact>()
                    .Collect()
                    .Where(c => c.Any()))
                .Query(() => factsGrouped, q => q
                    .From(() => factsAll)
                    .SelectMany(c => c)
                    .GroupBy(f => f.Value));

            Then()
                .Do(ctx => ctx.NoOp());
        }
    }
}