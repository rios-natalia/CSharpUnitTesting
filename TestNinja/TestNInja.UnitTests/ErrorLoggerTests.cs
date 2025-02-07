﻿using NUnit.Framework;
using TestNinja.Fundamentals;
using Assert = NUnit.Framework.Assert;

namespace TestNInja.UnitTests;

[TestFixture]
public class ErrorLoggerTests
{
    [SetUp]
    public void SetUp()
    {
        _logger = new ErrorLogger();
    }

    private ErrorLogger _logger;

    [Test]
    public void Log_WhenCalled_SetTheLastErrorProperty()
    {
        _logger.Log("a");
        Assert.That(_logger.LastError, Is.EqualTo("a"));
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void Log_InvalidError_ThrowArgumentNullException(string error)
    {
        Assert.That(() => _logger.Log(error), Throws.ArgumentNullException);
    }

    [Test]
    public void Log_ValidError_RaiseErrorLoggedEvent()
    {
        var id = Guid.Empty;
        _logger.ErrorLogged += (sender, args) => { id = args; };

        _logger.Log("a");

        Assert.That(id, Is.Not.EqualTo(Guid.Empty));
    }
}