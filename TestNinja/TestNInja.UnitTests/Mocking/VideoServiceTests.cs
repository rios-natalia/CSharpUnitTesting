﻿using Moq;
using NUnit.Framework;
using TestNinja.Mocking;
using Assert = NUnit.Framework.Assert;

namespace TestNInja.UnitTests.Mocking;

[TestFixture]
public class VideoServiceTests
{
    [SetUp]
    public void SetUp()
    {
        _fileReader = new Mock<IFileReader>();
        _repository = new Mock<IVideoRepository>();
        _videoService = new VideoService(_fileReader.Object, _repository.Object);
    }

    private VideoService _videoService;
    private Mock<IFileReader> _fileReader;
    private Mock<IVideoRepository> _repository;

    [Test]
    public void ReadVideoTitle_EmptyFile_ReturnError()
    {
        _fileReader.Setup(fr => fr.Read("video.txt")).Returns("");

        _videoService = new VideoService(_fileReader.Object);
        var result = _videoService.ReadVideoTitle();

        Assert.That(result, Does.Contain("error").IgnoreCase);
    }

    [Test]
    public void GetUnprocessedVideosAsCsv_AllVideosAreProcessed_ReturnString()
    {
        _repository.Setup(r => r.GetUnprocessedVideos()).Returns(new List<Video>());

        var result = _videoService.GetUnprocessedVideosAsCsv();

        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetUnprocessedVideosAsCsv_AFewUnprocessedVideos_ReturnVideoIds()
    {
        _repository.Setup(r => r.GetUnprocessedVideos()).Returns(new List<Video>
        {
            new() { Id = 1 },
            new() { Id = 2 }
        });

        var result = _videoService.GetUnprocessedVideosAsCsv();

        Assert.That(result, Is.EqualTo("1,2"));
    }
}