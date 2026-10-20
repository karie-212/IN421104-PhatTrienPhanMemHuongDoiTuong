using EduHub.Ch02;
using EduHub.Ch02.Domain;

namespace Ch02.Refactor.Tests;

// Mẫu AAA: Arrange (chuẩn bị) - Act (thực hiện) - Assert (kiểm tra).
// Mỗi test chỉ kiểm tra MỘT hành vi. Đặt tên: TênMethod_TìnhHuống_KếtQuảMongĐợi.
public class DomainTests
{
    // Test mẫu đã viết sẵn: chạy được ngay trên ClubManager hiện tại.
    [Fact]
    public void CreateClub_WithEmptyName_ThrowsArgumentException()
    {
        // Arrange
        var manager = new ClubManager();

        // Act
        var act = () => manager.CreateClub("", "mô tả", "p@gmail.com");

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    // Test cho lớp Domain (Club, Event), gồm cả trường hợp hợp lệ và trường hợp lỗi.

    [Fact]
    public void Club_Create_WithValidName_ReturnsClub()
    {
        // Arrange
        var name = "CLB Lập trình";
        var description = "Sinh hoạt lập trình mỗi thứ Bảy";

        // Act
        var club = Club.Create(name, description);

        // Assert
        Assert.NotEqual(Guid.Empty, club.Id);
        Assert.Equal(name, club.Name);
        Assert.Equal(description, club.Description);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Club_Create_WithInvalidName_ThrowsArgumentException(string name)
    {
        // Arrange
        var description = "CLB chưa đặt tên";

        // Act
        var act = () => Club.Create(name, description);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Club_Create_WithNameTooLong_ThrowsArgumentException()
    {
        // Arrange
        var name = new string('A', Club.MaxNameLength + 1);

        // Act
        var act = () => Club.Create(name, "CLB có tên quá dài");

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(49, true)]
    [InlineData(50, false)]
    public void Club_HasRoom_ByMemberCount_ReturnsTrueOnlyBelowMaxMembers(int currentMemberCount, bool expected)
    {
        // Arrange
        var club = Club.Create("CLB Lập trình", "Sinh hoạt lập trình mỗi thứ Bảy");

        // Act
        var hasRoom = club.HasRoom(currentMemberCount);

        // Assert
        Assert.Equal(expected, hasRoom);
    }

    [Fact]
    public void Event_Register_NewMember_ReturnsTrue()
    {
        // Arrange
        var workshop = CreateSampleEvent();
        var memberId = Guid.NewGuid();

        // Act
        var registered = workshop.Register(memberId);

        // Assert
        Assert.True(registered);
        Assert.Contains(memberId, workshop.RegisteredMemberIds);
    }

    [Fact]
    public void Event_Register_SameMemberTwice_ReturnsFalse()
    {
        // Arrange
        var workshop = CreateSampleEvent();
        var memberId = Guid.NewGuid();
        workshop.Register(memberId);

        // Act
        var registeredAgain = workshop.Register(memberId);

        // Assert
        Assert.False(registeredAgain);
        Assert.Single(workshop.RegisteredMemberIds);
    }

    [Fact]
    public void Event_CheckIn_UnregisteredMember_ThrowsInvalidOperationException()
    {
        // Arrange
        var workshop = CreateSampleEvent();
        var memberId = Guid.NewGuid();

        // Act
        var act = () => workshop.CheckIn(memberId);

        // Assert
        Assert.Throws<InvalidOperationException>(act);
    }

    [Fact]
    public void Event_CheckIn_RegisteredMember_AddsToCheckedIn()
    {
        // Arrange
        var workshop = CreateSampleEvent();
        var memberId = Guid.NewGuid();
        workshop.Register(memberId);

        // Act
        workshop.CheckIn(memberId);

        // Assert
        Assert.Contains(memberId, workshop.CheckedInMemberIds);
    }

    [Fact]
    public void Event_AttendanceRate_NoRegistration_ReturnsZero()
    {
        // Arrange
        var workshop = CreateSampleEvent();

        // Act
        var rate = workshop.AttendanceRate;

        // Assert
        Assert.Equal(0, rate);
    }

    [Fact]
    public void Event_AttendanceRate_OneOfTwoCheckedIn_ReturnsHalf()
    {
        // Arrange
        var workshop = CreateSampleEvent();
        var attendee = Guid.NewGuid();
        var absentee = Guid.NewGuid();
        workshop.Register(attendee);
        workshop.Register(absentee);
        workshop.CheckIn(attendee);

        // Act
        var rate = workshop.AttendanceRate;

        // Assert
        Assert.Equal(0.5, rate);
    }

    private static Event CreateSampleEvent()
    {
        return new Event
        {
            Id = Guid.NewGuid(),
            ClubId = Guid.NewGuid(),
            Title = "Workshop Git",
            Date = new DateTime(2026, 11, 15)
        };
    }
}
