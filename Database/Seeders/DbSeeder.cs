using meal_menu_api.Database.Context;
using meal_menu_api.Entities;
using System.Threading.Tasks;

namespace meal_menu_api.Database.Seeders
{
    public class DbSeeder
    {
        public static void SeedUnits(DataContext context)
        {
            try
            {
                // 1. Units
                var units = new List<string> { "ml", "cl", "dl", "l", "mg", "g", "hg", "kg", "st", "msk", " ", "tsk" };
                foreach (var unitName in units)
                {
                    if (!context.Units.Any(u => u.Name == unitName))
                    {
                        context.Units.Add(new UnitEntity
                        {
                            Name = unitName,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }

                // 2. User
                var email = "kungen1990@hotmail.com";
                var user = context.Users.FirstOrDefault(x => x.Email == email);
   
                // 3. Recipes
                var recipes = new List<RecipeEntity>
                {
                    new() { Name = "Köttbullar", Description = "Bullar gjorda av lök och kött.", Ppl = 4 },
                    new() { Name = "Kycklinggryta", Description = "En gryta gjord av kyckling och curry.", Ppl = 4 },
                    new() { Name = "Räksallad med sparris och sojamajonnäs", Description = "Fräsch och god sallad med grön sparris, mango och rödlök.", Ppl = 3 },
                    new() { Name = "Potatissoppa med fänkål och purjolök", Description = "Slät soppa med potatis, fänkål och purjolök.", Ppl = 2 },
                    new() { Name = "Lammgryta med kikärtor", Description = "Smakrik gryta med lamm och kikärtor. Serveras med couscous och yoghurt-sås.", Ppl = 2 }
                };

                foreach (var recipe in recipes)
                {
                    if (!context.Recipes.Any(r => r.Name == recipe.Name))
                    {
                        recipe.UserId = user!.Id;
                        recipe.CreatedAt = DateTime.Now;
                        recipe.UpdatedAt = DateTime.Now;
                        context.Recipes.Add(recipe);
                    }
                }

                context.SaveChanges();

                var units_1 = context.Units.ToDictionary(u => u.Name, u => u.Id);
                var recipes_2 = context.Recipes.ToDictionary(r => r.Name, r => r.Id);


                // 4. Ingredients
                var ingredients = new List<IngredientEntity>
                {
                    new() { UnitId = 6,  RecipeId = recipes_2["Köttbullar"], Name = "lammgrytbitar", Amount = 300 },
                    new() { UnitId = 9,  RecipeId = recipes_2["Köttbullar"], Name = "salt och peppar", Amount = 0 },
                    new() { UnitId = 10, RecipeId = recipes_2["Köttbullar"], Name = "vitlöksklyfta", Amount = 1 },
                    new() { UnitId = 3, RecipeId = recipes_2["Köttbullar"], Name = "tsk malen spiskummin", Amount = 1 },
                    new() { UnitId = 9, RecipeId = recipes_2["Köttbullar"], Name = "vatten", Amount = 2 },

                    new() { UnitId = 8, RecipeId = 23, Name = "skalad potatis, mjölig sort", Amount = 200 },
                    new() { UnitId = 9, RecipeId = 23, Name = "fänkål", Amount = 1 },
                    new() { UnitId = 10, RecipeId = 23, Name = "smör", Amount = 1 },
                    new() { UnitId = 3, RecipeId = 23, Name = "vatten", Amount = 4 },
                    new() { UnitId = 9, RecipeId = 23, Name = "tärning grönsaksbuljong", Amount = 1 },

                    new() { UnitId = 6, RecipeId = 22, Name = "(hand)skalade räkor", Amount = 150 },
                    new() { UnitId = 6, RecipeId = 22, Name = "grön sparris", Amount = 250 },
                    new() { UnitId = 9, RecipeId = 22, Name = "hjärtsallad", Amount = 2 },
                    new() { UnitId = 9, RecipeId = 22, Name = "mogen mango", Amount = 1 },
                    new() { UnitId = 10, RecipeId = 22, Name = "citronjuice", Amount = 1 },

                    new() { UnitId = 8, RecipeId = 20, Name = "nötfärs", Amount = 1 },
                    new() { UnitId = 6, RecipeId = 20, Name = "ströbröd", Amount = 50 },
                    new() { UnitId = 9, RecipeId = 20, Name = "ägg", Amount = 1 },
                    new() { UnitId = 9, RecipeId = 20, Name = "mjölk", Amount = 0 },
                    new() { UnitId = 9, RecipeId = 20, Name = "mjölk", Amount = 5 },
                };

                foreach (var ingredient in ingredients)
                {
                    if (!context.Ingredients.Any(i => i.RecipeId == ingredient.RecipeId && i.Name == ingredient.Name && i.Amount == ingredient.Amount))
                    {
                        ingredient.CreatedAt = DateTime.Now;
                        ingredient.UpdatedAt = DateTime.Now;
                        context.Ingredients.Add(ingredient);
                    }
                }

                context.SaveChanges();

                // 5. Steps
                var steps = new List<StepEntity>
        {
            new() { RecipeId = 21, Description = "Hacka löken." },
            new() { RecipeId = 21, Description = "Blanda färs, lök, ägg och kryddor." },
            new() { RecipeId = 21, Description = "Rulla till små bollar." },

            new() { RecipeId = 22, Description = "Blanda ihop sojamajonnäsen. Skala räkorna..." },
            new() { RecipeId = 22, Description = "Ansa sparrisen och koka ca 3 min i saltat vatten..." },
            new() { RecipeId = 22, Description = "Strimla salladen. Dela mango... och smaka av." },

            new() { RecipeId = 23, Description = "Skär potatis och fänkål i bitar... fräs i smör." },
            new() { RecipeId = 23, Description = "Häll grädden i kastrullen och mixa till en slät soppa." },
            new() { RecipeId = 23, Description = "Toppa med fänkålsfrön och purjolök." },

            new() { RecipeId = 24, Description = "Bryn grytbitarna i olja, salta, peppra och fräs lök." },
            new() { RecipeId = 24, Description = "Tillaga couscous enligt förpackningen." },
            new() { RecipeId = 24, Description = "Blanda yoghurt, paprika och mynta." },

            new() { RecipeId = 20, Description = "Blanda färsen med kryddor." },
            new() { RecipeId = 20, Description = "Forma till bollar." },
            new() { RecipeId = 20, Description = "Stek tills de är gyllenbruna." }
        };

                foreach (var step in steps)
                {
                    if (!context.Steps.Any(s => s.RecipeId == step.RecipeId && s.Description == step.Description))
                    {
                        step.CreatedAt = DateTime.Now;
                        step.UpdatedAt = DateTime.Now;
                        context.Steps.Add(step);
                    }
                }

                context.SaveChanges();

                // 6. Images
                var images = new List<ImageEntity>
        {
            new() { RecipeId = 20, ImageUrl = "Images\\83923389_2656711937758318_2068353204898234368_o.jpg" },
            new() { RecipeId = 21, ImageUrl = "Images\\kycklinggryta-med-curry-foto-nurlan-mathem.jpg" },
            new() { RecipeId = 22, ImageUrl = "Images\\raksallad-med-sparris-och-sojamajonnas_17560.avif" },
            new() { RecipeId = 23, ImageUrl = "Images\\potatis-och-fankalssoppa-med-purjolok_17562.avif" },
            new() { RecipeId = 24, ImageUrl = "Images\\lammgryta-med-kikartor-och-yoghurtsas_17561.avif" }
        };

                foreach (var image in images)
                {
                    if (!context.Images.Any(i => i.ImageUrl == image.ImageUrl))
                    {
                        image.CreatedAt = DateTime.Now;
                        image.UpdatedAt = DateTime.Now;
                        context.Images.Add(image);
                    }
                }

                context.SaveChanges();

                Console.WriteLine("Databasen seedad!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Fel under seeding: " + ex.Message);
            }
        }
    }
}


