using FluentAssertions;
using System;
using System.Linq.Expressions;

namespace SmtpSender.WebApi.Tests.PseudoIntegration
{
    public static class Equivalent
    {
        public static Expression<Func<TValue, bool>> To<TValue>(TValue expected)
        {
            Func<TValue, bool> f = actual =>
            {
                actual.Should().BeEquivalentTo(expected);
                return true;
            };
            Expression<Func<TValue, bool>> match = actual => f(actual);
            return match;
        }
    }
}
