using EduHub.Ch02.Patterns.Strategies;

namespace EduHub.Ch02.Services;

/// <summary>Tạo báo cáo CLB theo định dạng do strategy quyết định.</summary>
public interface IReportService
{
    string Generate(IReportStrategy strategy);
}
