using System;

using Xunit;
using BulkPaymentServer.Domain.Entities;


namespace BulkPaymentServer.Tests.Domain;

public class UploadTests
{
    [Fact]
    public void Constructor_Should_SetProperties_When_ValidInput()
    {
        //Arrange
        string userId = "userTest";
        string fileName = "paymentsTest.csv";
        string blobUrl = "http://example.com/blob/paymentsTest.csv";

        //Act
        var upload = new Upload(userId, fileName, blobUrl);

        //Assert
        Assert.Equal(userId, upload.UserId);
        Assert.Equal(fileName, upload.FileName);
        Assert.Equal(blobUrl, upload.BlobUrl);
        Assert.NotEqual(Guid.Empty, upload.Id);
        Assert.Empty(upload.Payments);
    }
}