using meal_menu_api.Database.Context;
using meal_menu_api.Entities;
using meal_menu_api.Entities.Recipes;
using meal_menu_api.Mappers;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace meal_menu_api.Managers
{
    public class ImageManager
    {
        private readonly DataContext _dataContext;

        public ImageManager(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<string> DeleteGroupImage(int Id)
        {
            GroupEntity? existingGroup = await _dataContext.Groups.FirstOrDefaultAsync(i => i.Id == Id);
            if (existingGroup != null && !string.IsNullOrEmpty(existingGroup.IconUrl))
            {
                var filePath = Path.Combine(existingGroup.IconUrl!);
                File.Delete(filePath);
                return filePath;
            }
            return "";
        }

        public async Task<string> SaveImage(IFormFile Image, int id)
        {
            if(Image != null)
            {
                string filePath = Path.Combine("Images\\groups", $"{id.ToString()}_" + Image.FileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await Image.CopyToAsync(stream);
                return filePath;
            }
            return "";
        }

        public async Task<List<ImageEntity>> CopyImages(List<ImageEntity> images, RecipeEntity recipe)
        {
            List<ImageEntity> copiedImages = [];

            foreach (var img in images.ToList())
            {
                try
                {
                    var newUrl = await CopyImageFile(img.ImageUrl, recipe);
                    var imageEntity = new ImageEntity
                    {
                        RecipeId = recipe.Id,
                        Recipe = recipe,
                        ImageUrl = newUrl,
                    };
                    copiedImages.Add(imageEntity);       
                }
                catch (Exception ex) { Debug.WriteLine($"Error Copying Image '{img.ImageUrl}' In CopyImages or CopyImageFile methods inside the imageManager: {ex.Message}"); }
               
            }
            _dataContext.Images.AddRange(copiedImages);
            await _dataContext.SaveChangesAsync();
            return copiedImages;
        }

        public Task<string> CopyImageFile(string imageUrl, RecipeEntity recipe)
        {
            string fileName = Path.GetFileName(imageUrl);
            if (fileName == null || !fileName.Contains("_"))
                throw new Exception("Unvalid filename");

            string originalFileName = fileName[(fileName.IndexOf('_') + 1)..];
            string sourcePath = Path.Combine("Images", fileName);
            if (!File.Exists(sourcePath))
                throw new FileNotFoundException("Original picture dont exist on the server.", sourcePath);

            string newFileName = $"{recipe.Id}_{originalFileName}";
            string newFilePath = Path.Combine("Images", newFileName);

            File.Copy(sourcePath, newFilePath, overwrite: false);

            string relativePath = Path.Combine("Images", newFileName);
            return Task.FromResult(relativePath);
        }
    }
}
