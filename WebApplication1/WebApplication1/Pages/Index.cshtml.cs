using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        private const string SessionKeyPrefix = "UsedMemes_";

        public string DisplayedImage { get; set; } = "";

        // Словарь: категория ? имя папки
        private static readonly Dictionary<string, string> _categories = new()
        {
            { "Рабочие мемы", "Рабочие мемы" },
            { "Научные мемы", "Научные мемы" },
            { "Мемы про жизнь", "Мемы про жизнь" },
            { "Рандом мем", "Рандом мем" }
        };

        public void OnGet()
        {
            // Ничего не делаем при загрузке
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult OnPostSearch([FromBody] SearchRequest request)
        {
            var category = request.Category?.Trim();
            if (string.IsNullOrEmpty(category) || !_categories.TryGetValue(category, out var folderName))
            {
                return new JsonResult(new { imagePath = "", error = "Неверная категория" });
            }

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", folderName);
            if (!Directory.Exists(folderPath))
            {
                return new JsonResult(new { imagePath = "", error = "Папка не найдена" });
            }

            var allFiles = Directory.GetFiles(folderPath, "*.*")
                .Where(f => IsImageFile(f))
                .Select(f => Path.GetFileName(f))
                .ToArray();

            if (allFiles.Length == 0)
            {
                return new JsonResult(new { imagePath = "", error = "Нет изображений" });
            }

            var sessionKey = SessionKeyPrefix + folderName;
            var usedListJson = HttpContext.Session.GetString(sessionKey);
            var usedList = string.IsNullOrEmpty(usedListJson)
                ? new List<string>()
                : System.Text.Json.JsonSerializer.Deserialize<List<string>>(usedListJson) ?? new List<string>();

            var remaining = allFiles.Except(usedList).ToList();

            // Если все мемы показаны — сбросить
            if (!remaining.Any())
            {
                usedList.Clear();
                remaining = allFiles.ToList();
            }

            var random = new Random();
            var selectedFile = remaining[random.Next(remaining.Count)];
            usedList.Add(selectedFile);

            // Сохранить обновлённый список в сессию
            var updatedJson = System.Text.Json.JsonSerializer.Serialize(usedList);
            HttpContext.Session.SetString(sessionKey, updatedJson);

            var imagePath = $"/images/{folderName}/{selectedFile}";
            return new JsonResult(new { imagePath });
        }

        private bool IsImageFile(string path)
        {
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return ext is ".jpg" or ".jpeg" or ".png" or ".gif" or ".bmp" or ".webp";
        }
    }




    public class SearchRequest
    {
        public string Category { get; set; }
    }
}