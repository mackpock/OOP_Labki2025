namespace WebApplication1.Models
{
    public class Meme
    {
        public string Category { get; set; } // Категория: "Рабочие мемы", "Научные мемы"
        public string ImagePath { get; set; } // Путь к изображению, например: "/images/work_meme1.jpg"
        public string Description { get; set; } // Опционально
    }
}