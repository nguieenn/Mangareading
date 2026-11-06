using Mangareading.Models;
using System.Collections.Generic;

namespace Mangareading.Models.ViewModelsViewModels
{
    public class MangaEditViewModel
    {
        public Manga Manga { get; set; }
        public List<Genre> AllGenres { get; set; }
        public List<Chapter> Chapters { get; set; }

        // Optional: You might bind selected genres directly using the name "GenreIds" in the form,
        // or use a property like this if you prefer explicit model binding.
        // public List<int> SelectedGenreIds { get; set; } = new List<int>();
    }
}
