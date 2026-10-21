namespace EduHub.Ch02.Patterns.Strategies;

/// <summary>Số liệu thống kê của một CLB, dữ liệu đầu vào cho các strategy báo cáo.</summary>
public record ClubStat(string ClubName, int Members, int Events, int Registered, int CheckedIn);
