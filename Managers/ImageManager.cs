using meal_menu_api.Database.Context;
using meal_menu_api.Entities;
using Microsoft.EntityFrameworkCore;

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
                string filePath = Path.Combine("Images/groups", $"{id.ToString()}_" + Image.FileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await Image.CopyToAsync(stream);
                return filePath;
            }
            return "";
        }
    }
}
