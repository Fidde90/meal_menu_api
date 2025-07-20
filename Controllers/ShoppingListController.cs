using meal_menu_api.Dtos;
using meal_menu_api.Filters;
using meal_menu_api.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace meal_menu_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [UseApiKey]
    [Authorize]
    public class ShoppingListController : ControllerBase
    {
        private readonly ShoppingListManager _shoppingListManager;

        public ShoppingListController(ShoppingListManager shoppingListManager)
        {
            _shoppingListManager = shoppingListManager;
        }

        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> GetShoppingList()
        {

            var shoppingList = await _shoppingListManager.CreateShoppingList(User.Identity!.Name!);

            if(shoppingList != null)
            {
                var shoppingListDto = new ShoppingListDto
                {
                    Id = shoppingList.Id,
                    Name = shoppingList.Name,
                    Notes = shoppingList.Notes,
                    Status = shoppingList.Status,
                    CreatedAt = shoppingList.CreatedAt,
                    UpdatedAt = shoppingList.UpdatedAt,
                };


                foreach (var ingredient in shoppingList.Ingredients)
                {
                    var newIngredientDto = new ShoppingListIngredientDto
                    {
                        Id = ingredient.Id,
                        ShoppingListId = ingredient.Id,
                        Description = ingredient.Description,
                        Name = ingredient.Name,
                        Amount = ingredient.Amount,
                        Unit = ingredient.Unit,
                        IsChecked = ingredient.IsChecked,
                        CreatedAt = ingredient.CreatedAt,
                        UpdatedAt = ingredient.UpdatedAt,
                    };

                    shoppingListDto.Ingredients.Add(newIngredientDto);
                }

                return Ok(shoppingListDto);
            }

            return StatusCode(500);

        }
    }
}

