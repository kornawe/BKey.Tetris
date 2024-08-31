using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace BKey.Tetris.Logic.Settings;

public class SettingProvider<TSettings, TValue> : ISettingProvider<TValue> where TSettings : notnull
{
    private readonly TSettings _settings;
    private readonly Func<TSettings, TValue> _getter;
    private readonly Action<TSettings, TValue> _setter;

    public SettingProvider(TSettings settings, Expression<Func<TSettings, TValue>> propertyExpression)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));

        // Compile the expression to create getter and setter delegates
        _getter = propertyExpression.Compile();
        _setter = CreateSetter(propertyExpression);

        // Store the property name for validation
        MemberExpression memberExpression = (MemberExpression)propertyExpression.Body;
        PropertyName = memberExpression.Member.Name;
    }

    public string PropertyName { get; }

    public TValue Get()
    {
        return _getter(_settings);
    }

    public TValue Set(TValue value)
    {
        // Validate the value using DataAnnotations
        var context = new ValidationContext(_settings) { MemberName = PropertyName };
        var validationResults = new List<ValidationResult>();
        if (Validator.TryValidateProperty(value, context, validationResults))
        {
            // If validation passes, set the value
            _setter(_settings, value);
        }

        return Get();
    }

    private static Action<TSettings, TValue> CreateSetter(Expression<Func<TSettings, TValue>> propertyExpression)
    {
        var memberExpression = (MemberExpression)propertyExpression.Body;
        var property = (PropertyInfo)memberExpression.Member;

        var parameterExpression = Expression.Parameter(typeof(int), "value");
        var targetExpression = Expression.Parameter(typeof(TSettings), "target");

        var setterExpression = Expression.Lambda<Action<TSettings, TValue>>(
            Expression.Assign(Expression.Property(targetExpression, property), parameterExpression),
            targetExpression, parameterExpression);

        return setterExpression.Compile();
    }
}