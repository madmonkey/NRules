﻿using System;

namespace NRules.RuleModel.Builders;

/// <summary>
/// Builder to compose an existential element.
/// </summary>
public class ExistsBuilder : RuleElementBuilder, IBuilder<ExistsElement>
{
    private IBuilder<RuleElement> _sourceBuilder;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExistsBuilder"/>.
    /// </summary>
    public ExistsBuilder()
    {
    }

    /// <summary>
    /// Sets a pattern as the source of the existential element.
    /// </summary>
    /// <param name="element">Element to set as the source.</param>
    public void Pattern(PatternElement element)
    {
        AssertSingleSource();
        _sourceBuilder = BuilderAdapter.Create(element);
    }

    /// <summary>
    /// Sets a pattern builder as the source of the existential element.
    /// </summary>
    /// <param name="builder">Element builder to set as the source.</param>
    public void Pattern(PatternBuilder builder)
    {
        AssertSingleSource();
        _sourceBuilder = builder;
    }

    /// <summary>
    /// Creates a pattern builder that builds the source of the existential element.
    /// </summary>
    /// <param name="type">Type of the element the pattern matches.</param>
    /// <param name="name">Pattern name (optional).</param>
    /// <returns>Pattern builder.</returns>
    public PatternBuilder Pattern(Type type, string name = null)
    {
        var declaration = Element.Declaration(type, DeclarationName(name));
        return Pattern(declaration);
    }

    /// <summary>
    /// Creates a pattern builder that builds the source of the existential element.
    /// </summary>
    /// <param name="declaration">Pattern declaration.</param>
    /// <returns>Pattern builder.</returns>
    public PatternBuilder Pattern(Declaration declaration)
    {
        AssertSingleSource();
        var sourceBuilder = new PatternBuilder(declaration);
        _sourceBuilder = sourceBuilder;
        return sourceBuilder;
    }

    /// <summary>
    /// Sets a group as the source of the existential element.
    /// </summary>
    /// <param name="element">Element to set as the source.</param>
    public void Group(GroupElement element)
    {
        AssertSingleSource();
        var builder = BuilderAdapter.Create(element);
        _sourceBuilder = builder;
    }

    /// <summary>
    /// Sets a group builder as the source of the existential element.
    /// </summary>
    /// <param name="builder">Element builder to set as the source.</param>
    public void Group(GroupBuilder builder)
    {
        AssertSingleSource();
        _sourceBuilder = builder;
    }

    /// <summary>
    /// Creates a group builder that builds a group as the source of the existential element.
    /// </summary>
    /// <param name="groupType">Group type.</param>
    /// <returns>Group builder.</returns>
    public GroupBuilder Group(GroupType groupType)
    {
        AssertSingleSource();
        var sourceBuilder = new GroupBuilder();
        sourceBuilder.GroupType(groupType);
        _sourceBuilder = sourceBuilder;
        return sourceBuilder;
    }

    ExistsElement IBuilder<ExistsElement>.Build()
    {
        var sourceElement = _sourceBuilder?.Build();
        var existsElement = Element.Exists(sourceElement);
        return existsElement;
    }

    private void AssertSingleSource()
    {
        if (_sourceBuilder != null)
        {
            throw new InvalidOperationException("EXISTS element can only have a single source");
        }
    }
}
