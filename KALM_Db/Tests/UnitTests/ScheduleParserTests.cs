using Xunit;
using System.Text;
using Infrastructure.Parsers;
// using CsvHelper;

namespace Tests.UnitTests;
public class ScheduleParserTests
{
    [Fact]
    public async Task ParseScheduleAsync_ValidCsv_ReturnsParsedModels()
    {
        // Arrange
        var csvContent = "GroupName,Teacher,Subject\nGroup1,Иванов И.И.,Математика\nGroup2,Петров П.П.,Физика";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
        
        var parser = new ScheduleParser(); // Замените на ваш класс с методом

        // Act
        var result = await parser.ParseScheduleAsync(stream);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        
        Assert.Equal("Group1", result[0].Group.Group_Name);
        Assert.Equal("Иванов И.И.", result[0].Teacher.FullName);
        Assert.Equal("Математика", result[0].Subject.SubjectName);
    }
}