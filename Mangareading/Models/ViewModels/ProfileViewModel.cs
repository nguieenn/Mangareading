using System;
using System.ComponentModel.DataAnnotations;

namespace Mangareading.Models
{
    public class ProfileViewModel
    {
        public int UserId { get; set; }

        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "URL Ảnh đại diện")]
        [Url]
        public string AvatarUrl { get; set; }

        [Display(Name = "Ngày tham gia")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Quyền quản trị")]
        public bool IsAdmin { get; set; }

        [Display(Name = "Trạng thái hoạt động")]
        public bool IsActive { get; set; }

        [Display(Name = "Giao diện ưu thích")]
        public string ThemePreference { get; set; }
    }
} 