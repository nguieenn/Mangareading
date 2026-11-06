using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mangareading.Models;
using Microsoft.Data.SqlClient;

namespace Mangareading.Repositories
{
    public interface IViewCountRepository
    {
        Task AddViewAsync(int mangaId, int chapterId, int? userId, string ipAddress);
        Task<int> GetMangaViewCountAsync(int mangaId);
        Task<int> GetChapterViewCountAsync(int chapterId);
    }

    public class ViewCountRepository : IViewCountRepository
    {
        private readonly YourDbContext _context;

        public ViewCountRepository(YourDbContext context)
        {
            _context = context;
        }

        public async Task AddViewAsync(int mangaId, int chapterId, int? userId, string ipAddress)
        {
            try
            {
                // Nếu không có IP, vẫn tính lượt xem nhưng không lưu IP
                if (string.IsNullOrEmpty(ipAddress))
                {
                    ipAddress = "unknown";
                }

                // Thay đổi query để phù hợp với cấu trúc mới
                int time=1;
                var recentView = await _context.ViewCounts
                    .Where(v => v.ChapterId == chapterId && v.IpAddress == ipAddress && v.ViewedAt > DateTime.UtcNow.AddMinutes(-time))
                    .FirstOrDefaultAsync();

                if (recentView == null)
                {
                    // Add new view record
                    var viewCount = new ViewCount
                    {
                        MangaId = mangaId,
                        ChapterId = chapterId,
                        UserId = userId,
                        IpAddress = ipAddress,
                        ViewedAt = DateTime.UtcNow
                    };

                    try 
                    {
                        _context.ViewCounts.Add(viewCount);
                        
                        // Cập nhật lượt xem trong bảng Manga
                        var manga = await _context.Mangas.FindAsync(mangaId);
                        if (manga != null)
                        {
                            manga.ViewCount = (manga.ViewCount ?? 0) + 1;
                            _context.Mangas.Update(manga);
                        }
                        
                        await _context.SaveChangesAsync();
                        Console.WriteLine($"Added new view for manga {mangaId}, chapter {chapterId} from IP {ipAddress}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Database error while adding view: {ex.Message}");
                        
                        // Trường hợp không tồn tại bảng, tạo bảng
                        if (ex.Message.Contains("Invalid column name") || ex.Message.Contains("doesn't exist"))
                        {
                            try
                            {
                                // Cách giải quyết tạm thời: sử dụng raw SQL
                                string sql = @"
                                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ViewCounts')
                                BEGIN
                                    CREATE TABLE ViewCounts (
                                        MangaId int NOT NULL,
                                        ChapterId int NOT NULL,
                                        UserId int NULL,
                                        IpAddress nvarchar(100) NOT NULL,
                                        ViewedAt datetime2 NOT NULL,
                                        CONSTRAINT PK_ViewCounts PRIMARY KEY (ChapterId, IpAddress, ViewedAt)
                                    );
                                END";
                                await _context.Database.ExecuteSqlRawAsync(sql);
                                
                                // Thử lại thêm lượt xem
                                _context.ViewCounts.Add(viewCount);
                                
                                // Cập nhật lượt xem trong bảng Manga
                                var manga = await _context.Mangas.FindAsync(mangaId);
                                if (manga != null)
                                {
                                    manga.ViewCount = (manga.ViewCount ?? 0) + 1;
                                    _context.Mangas.Update(manga);
                                }
                                
                                await _context.SaveChangesAsync();
                                Console.WriteLine("Created ViewCounts table and added view record");
                            }
                            catch (Exception innerEx)
                            {
                                Console.WriteLine($"Failed to create ViewCounts table: {innerEx.Message}");
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"View already recorded for manga {mangaId}, chapter {chapterId} from IP {ipAddress} within last {time}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding view: {ex.Message}");
                // Vẫn cho phép ứng dụng tiếp tục mà không báo lỗi
            }
        }

        public async Task<int> GetMangaViewCountAsync(int mangaId)
        {
            return await _context.ViewCounts
                .Where(v => v.MangaId == mangaId)
                .CountAsync();
        }

        public async Task<int> GetChapterViewCountAsync(int chapterId)
        {
            return await _context.ViewCounts
                .Where(v => v.ChapterId == chapterId)
                .CountAsync();
        }
    }
}
