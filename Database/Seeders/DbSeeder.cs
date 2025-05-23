using meal_menu_api.Database.Context;
using meal_menu_api.Entities;

namespace meal_menu_api.Database.Seeders
{
    public class DbSeeder
    {
        public static void SeedUnits(DataContext context)
        {
            try
            {
                if (!context.Units.Any())
                {
                    // 1. Units
                    var newUnits = new List<string> { "krm", "tsk", "msk", "cl", "ml", "dl", "l", "g", "hg", "kg", "nypa", "förp", "st", " " };
                    foreach (var unitName in newUnits)
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

                    context.SaveChanges();
                }

                // 2. User
                var email = "kungen1990@hotmail.com";
                var user = context.Users.FirstOrDefault(x => x.Email == email);

                if (!context.Recipes.Any() && user != null)
                {
                    // 3. Recipes
                    var newNecipes = new List<RecipeEntity>
                    {
                        new() { Name = "Köttbullar", Description = "Bullar gjorda av lök och kött.", Ppl = 4 },
                        new() { Name = "Kycklinggryta", Description = "En gryta gjord av kyckling och curry.", Ppl = 4 },
                        new() { Name = "Räksallad med sparris och sojamajonnäs", Description = "Fräsch och god sallad med grön sparris, mango och rödlök.", Ppl = 3 },
                        new() { Name = "Potatissoppa med fänkål och purjolök", Description = "Slät soppa med potatis, fänkål och purjolök.", Ppl = 2 },
                        new() { Name = "Lammgryta med kikärtor", Description = "Smakrik gryta med lamm och kikärtor. Serveras med couscous och yoghurt-sås.", Ppl = 2 },
                        new() { Name = "Spaghetti Bolognese", Description = "Klassisk italiensk pastarätt med köttfärssås och parmesan.", Ppl = 4 },
                        new() { Name = "Vegetarisk lasagne", Description = "Lasagne med spenat, ricotta och tomatsås, perfekt för vegetarianer.", Ppl = 4 },
                        new() { Name = "Tacos med guacamole", Description = "Färska tacos med köttfärs, grönsaker och en krämig guacamole.", Ppl = 3 },
                        new() { Name = "Fiskgryta med saffran", Description = "Lyxig fiskgryta med saffran, fänkål och vitvinssås.", Ppl = 4 },
                        new() { Name = "Falafel med hummus", Description = "Krispiga falafelbollar serveras med hummus och sallad.", Ppl = 3 }

                    };

                    foreach (var recipe in newNecipes)
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

                    var units = context.Units.ToDictionary(u => u.Name, u => u.Id);
                    var recipes = context.Recipes.ToDictionary(r => r.Name, r => r.Id);

                    // 4. Ingredients
                    var ingredients = new List<IngredientEntity>
                    {
                        new() { UnitId = units["st"], RecipeId = recipes["Spaghetti Bolognese"], Name = "Gul lök", Amount = 1, Description = "hackad" },
                        new() { UnitId = units["st"], RecipeId = recipes["Spaghetti Bolognese"], Name = "Vitlöksklyfta", Amount = 2, Description = "pressade" },
                        new() { UnitId = units["g"], RecipeId = recipes["Spaghetti Bolognese"], Name = "Köttfärs", Amount = 400, Description = "nötfärs" },
                        new() { UnitId = units["g"], RecipeId = recipes["Spaghetti Bolognese"], Name = "Krossade tomater", Amount = 400, Description = "" },
                        new() { UnitId = units["msk"], RecipeId = recipes["Spaghetti Bolognese"], Name = "Tomatpuré", Amount = 1, Description = "" },
                        new() { UnitId = units["msk"], RecipeId = recipes["Spaghetti Bolognese"], Name = "Olivolja", Amount = 1, Description = "" },

                        new() { UnitId = units["st"], RecipeId = recipes["Köttbullar"], Name = "Gul lök", Amount = 1, Description = "finhackad" },
                        new() { UnitId = units["dl"], RecipeId = recipes["Köttbullar"], Name = "Mjölk", Amount = 1, Description = "" },
                        new() { UnitId = units["dl"], RecipeId = recipes["Köttbullar"], Name = "Ströbröd", Amount = 1, Description = "" },
                        new() { UnitId = units["g"], RecipeId = recipes["Köttbullar"], Name = "Köttfärs", Amount = 500, Description = "blandfärs" },
                        new() { UnitId = units["st"], RecipeId = recipes["Köttbullar"], Name = "Ägg", Amount = 1, Description = "" },

                        new() { UnitId = units["st"], RecipeId = recipes["Vegetarisk lasagne"], Name = "Zucchini", Amount = 1, Description = "skivad" },
                        new() { UnitId = units["g"], RecipeId = recipes["Vegetarisk lasagne"], Name = "Spenat", Amount = 200, Description = "fryst, tinad" },
                        new() { UnitId = units["g"], RecipeId = recipes["Vegetarisk lasagne"], Name = "Krossade tomater", Amount = 400, Description = "" },
                        new() { UnitId = units["dl"], RecipeId = recipes["Vegetarisk lasagne"], Name = "Riven ost", Amount = 2, Description = "" },
                        new() { UnitId = units["st"], RecipeId = recipes["Vegetarisk lasagne"], Name = "Lasagneplattor", Amount = 9, Description = "" },

                        new() { UnitId = units["st"], RecipeId = recipes["Falafel med hummus"], Name = "Vitlöksklyfta", Amount = 1, Description = "" },
                        new() { UnitId = units["g"], RecipeId = recipes["Falafel med hummus"], Name = "Kikärtor", Amount = 400, Description = "kokta" },
                        new() { UnitId = units["msk"], RecipeId = recipes["Falafel med hummus"], Name = "Persilja", Amount = 2, Description = "färsk, hackad" },
                        new() { UnitId = units["tsk"], RecipeId = recipes["Falafel med hummus"], Name = "Spiskummin", Amount = 1, Description = "" },
                        new() { UnitId = units["msk"], RecipeId = recipes["Falafel med hummus"], Name = "Vetemjöl", Amount = 2, Description = "" },

                        new() { UnitId = units["g"], RecipeId = recipes["Tacos med guacamole"], Name = "Köttfärs", Amount = 400, Description = "nötfärs" },
                        new() { UnitId = units["st"], RecipeId = recipes["Tacos med guacamole"], Name = "Tacokrydda", Amount = 1, Description = "" },
                        new() { UnitId = units["st"], RecipeId = recipes["Tacos med guacamole"], Name = "Tomat", Amount = 2, Description = "tärnad" },
                        new() { UnitId = units["st"], RecipeId = recipes["Tacos med guacamole"], Name = "Avokado", Amount = 2, Description = "mosad" },
                        new() { UnitId = units["st"], RecipeId = recipes["Tacos med guacamole"], Name = "Tortillabröd", Amount = 8, Description = "små" },

                        new() { UnitId = units["st"], RecipeId = recipes["Kycklinggryta"], Name = "Gul lök", Amount = 1, Description = "hackad" },
                        new() { UnitId = units["g"], RecipeId = recipes["Kycklinggryta"], Name = "Kycklingfilé", Amount = 500, Description = "strimlad" },
                        new() { UnitId = units["dl"], RecipeId = recipes["Kycklinggryta"], Name = "Grädde", Amount = 2, Description = "matlagningsgrädde" },
                        new() { UnitId = units["msk"], RecipeId = recipes["Kycklinggryta"], Name = "Curry", Amount = 1, Description = "" },
                        new() { UnitId = units["msk"], RecipeId = recipes["Kycklinggryta"], Name = "Olivolja", Amount = 1, Description = "" },

                        new() { UnitId = units["g"], RecipeId = recipes["Lammgryta med kikärtor"], Name = "Lammkött", Amount = 500, Description = "tärnat" },
                        new() { UnitId = units["g"], RecipeId = recipes["Lammgryta med kikärtor"], Name = "Kikärtor", Amount = 400, Description = "kokta" },
                        new() { UnitId = units["st"], RecipeId = recipes["Lammgryta med kikärtor"], Name = "Gul lök", Amount = 1, Description = "hackad" },
                        new() { UnitId = units["msk"], RecipeId = recipes["Lammgryta med kikärtor"], Name = "Tomatpuré", Amount = 1, Description = "" },
                        new() { UnitId = units["dl"], RecipeId = recipes["Lammgryta med kikärtor"], Name = "Vatten", Amount = 2, Description = "" },

                        new() { UnitId = units["st"], RecipeId = recipes["Fiskgryta med saffran"], Name = "Vit fisk", Amount = 400, Description = "torsk, tärnad" },
                        new() { UnitId = units["dl"], RecipeId = recipes["Fiskgryta med saffran"], Name = "Grädde", Amount = 2, Description = "matlagningsgrädde" },
                        new() { UnitId = units["msk"], RecipeId = recipes["Fiskgryta med saffran"], Name = "Fiskfond", Amount = 2, Description = "" },
                        new() { UnitId = units["g"], RecipeId = recipes["Fiskgryta med saffran"], Name = "Tomat", Amount = 200, Description = "tärnad" },
                        new() { UnitId = units["st"], RecipeId = recipes["Fiskgryta med saffran"], Name = "Saffran", Amount = 1, Description = "påse" },

                        new() { UnitId = units["st"], RecipeId = recipes["Potatissoppa med fänkål och purjolök"], Name = "Potatis", Amount = 4, Description = "mjölig" },
                        new() { UnitId = units["st"], RecipeId = recipes["Potatissoppa med fänkål och purjolök"], Name = "Fänkål", Amount = 1, Description = "strimlad" },
                        new() { UnitId = units["st"], RecipeId = recipes["Potatissoppa med fänkål och purjolök"], Name = "Purjolök", Amount = 1, Description = "skivad" },
                        new() { UnitId = units["dl"], RecipeId = recipes["Potatissoppa med fänkål och purjolök"], Name = "Grönsaksbuljong", Amount = 5, Description = "" },
                        new() { UnitId = units["dl"], RecipeId = recipes["Potatissoppa med fänkål och purjolök"], Name = "Grädde", Amount = 1, Description = "" },

                        new() { UnitId = units["g"], RecipeId = recipes["Räksallad med sparris och sojamajonnäs"], Name = "Räkor", Amount = 200, Description = "skalade" },
                        new() { UnitId = units["st"], RecipeId = recipes["Räksallad med sparris och sojamajonnäs"], Name = "Sparris", Amount = 8, Description = "grön, kokt" },
                        new() { UnitId = units["dl"], RecipeId = recipes["Räksallad med sparris och sojamajonnäs"], Name = "Majonnäs", Amount = 1, Description = "blandad med soja" },
                        new() { UnitId = units["st"], RecipeId = recipes["Räksallad med sparris och sojamajonnäs"], Name = "Sallad", Amount = 1, Description = "valfri, sköljd" },
                        new() { UnitId = units["st"], RecipeId = recipes["Räksallad med sparris och sojamajonnäs"], Name = "Ägg", Amount = 2, Description = "kokta" }
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
                        new() { RecipeId = recipes["Köttbullar"], Description = "Hacka löken." },
                        new() { RecipeId = recipes["Köttbullar"], Description = "Blanda färs, lök, ägg och kryddor." },
                        new() { RecipeId = recipes["Köttbullar"], Description = "Rulla till små bollar." },

                        new() { RecipeId = recipes["Räksallad med sparris och sojamajonnäs"], Description = "Blanda ihop sojamajonnäsen. Skala räkorna..." },
                        new() { RecipeId = recipes["Räksallad med sparris och sojamajonnäs"], Description = "Ansa sparrisen och koka ca 3 min i saltat vatten..." },
                        new() { RecipeId = recipes["Räksallad med sparris och sojamajonnäs"], Description = "Strimla salladen. Dela mango... och smaka av." },

                        new() { RecipeId = recipes["Potatissoppa med fänkål och purjolök"], Description = "Skär potatis och fänkål i bitar... fräs i smör." },
                        new() { RecipeId = recipes["Potatissoppa med fänkål och purjolök"], Description = "Häll grädden i kastrullen och mixa till en slät soppa." },
                        new() { RecipeId = recipes["Potatissoppa med fänkål och purjolök"], Description = "Toppa med fänkålsfrön och purjolök." },

                        new() { RecipeId = recipes["Lammgryta med kikärtor"], Description = "Bryn grytbitarna i olja, salta, peppra och fräs lök." },
                        new() { RecipeId = recipes["Lammgryta med kikärtor"], Description = "Tillaga couscous enligt förpackningen." },
                        new() { RecipeId = recipes["Lammgryta med kikärtor"], Description = "Blanda yoghurt, paprika och mynta." },

                        new() { RecipeId = recipes["Kycklinggryta"], Description = "Strimla kycklingfilén" },
                        new() { RecipeId = recipes["Kycklinggryta"], Description = "Koka ris." },
                        new() { RecipeId = recipes["Kycklinggryta"], Description = "Stek tills de är gyllenbruna." },

                        new() { RecipeId = recipes["Spaghetti Bolognese"], Description = "Hacka löken och fräs den i en panna." },
                        new() { RecipeId = recipes["Spaghetti Bolognese"], Description = "Tillsätt köttfärs och bryn den." },
                        new() { RecipeId = recipes["Spaghetti Bolognese"], Description = "Häll i tomatsåsen och låt sjuda i 15-20 minuter." },
                        new() { RecipeId = recipes["Spaghetti Bolognese"], Description = "Koka pasta enligt anvisningarna på förpackningen." },
                        new() { RecipeId = recipes["Spaghetti Bolognese"], Description = "Servera köttfärssåsen över pastan och strö över parmesan." },

                        new() { RecipeId = recipes["Vegetarisk lasagne"], Description = "Fräs spenaten i en panna tills den mjuknar." },
                        new() { RecipeId = recipes["Vegetarisk lasagne"], Description = "Blanda spenaten med ricotta och kryddor." },
                        new() { RecipeId = recipes["Vegetarisk lasagne"], Description = "Varva lasagneplattor med spenat- och ricottablandningen och tomatsås." },
                        new() { RecipeId = recipes["Vegetarisk lasagne"], Description = "Gratinera lasagnen i ugnen i 30-40 minuter." },
                        new() { RecipeId = recipes["Vegetarisk lasagne"], Description = "Låt lasagnen vila innan den skärs upp och serveras." },

                        new() { RecipeId = recipes["Tacos med guacamole"], Description = "Koka köttfärsen med tacokryddor och häll i salsa." },
                        new() { RecipeId = recipes["Tacos med guacamole"], Description = "Fyll taco-skal med köttfärsblandningen." },
                        new() { RecipeId = recipes["Tacos med guacamole"], Description = "Mosa avokadon och blanda med limejuice och vitlök för att göra guacamole." },
                        new() { RecipeId = recipes["Tacos med guacamole"], Description = "Toppa tacos med guacamole, riven ost och grönsaker." },
                        new() { RecipeId = recipes["Tacos med guacamole"], Description = "Servera direkt med en frisk sallad vid sidan av." },

                        new() { RecipeId = recipes["Fiskgryta med saffran"], Description = "Fräs fänkålen och tillsätt fiskbuljong och saffran." },
                        new() { RecipeId = recipes["Fiskgryta med saffran"], Description = "Lägg i fisken och låt sjuda i några minuter." },
                        new() { RecipeId = recipes["Fiskgryta med saffran"], Description = "Häll i grädden och låt allt koka samman i 10 minuter." },
                        new() { RecipeId = recipes["Fiskgryta med saffran"], Description = "Smaka av med salt och peppar." },
                        new() { RecipeId = recipes["Fiskgryta med saffran"], Description = "Servera med ett gott bröd och en sallad." },

                        new() { RecipeId = recipes["Falafel med hummus"], Description = "Blötlägg kikärtorna och mixa dem med lök och kryddor." },
                        new() { RecipeId = recipes["Falafel med hummus"], Description = "Forma smeten till bollar och fritera dem tills de är gyllenbruna." },
                        new() { RecipeId = recipes["Falafel med hummus"], Description = "Servera falafelbollarna med hummus och pitabröd." },
                        new() { RecipeId = recipes["Falafel med hummus"], Description = "Tillsätt grönsaker som tomater, gurka och sallad." },
                        new() { RecipeId = recipes["Falafel med hummus"], Description = "Njut av en god och näringsrik måltid!" }
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
                        new() { RecipeId = recipes["Köttbullar"], ImageUrl = "Images\\köttbullar.jpg" },
                        new() { RecipeId = recipes["Kycklinggryta"], ImageUrl = "Images\\kyckliggryta.jpg" },
                        new() { RecipeId = recipes["Räksallad med sparris och sojamajonnäs"], ImageUrl = "Images\\räksallad.jpg" },
                        new() { RecipeId = recipes["Potatissoppa med fänkål och purjolök"], ImageUrl = "Images\\potatissoppa.jpg" },
                        new() { RecipeId = recipes["Lammgryta med kikärtor"], ImageUrl = "Images\\lammgryta.jpg" },
                        new() { RecipeId = recipes["Spaghetti Bolognese"], ImageUrl = "Images\\Spaghetti Bolognese.jpg" },
                        new() { RecipeId = recipes["Vegetarisk lasagne"], ImageUrl = "Images\\Vegetarisk lasagne.jpg" },
                        new() { RecipeId = recipes["Tacos med guacamole"], ImageUrl = "Images\\Tacos med guacamole.jpg" },
                        new() { RecipeId = recipes["Fiskgryta med saffran"], ImageUrl = "Images\\Fiskgryta med saffran.jpg" },
                        new() { RecipeId = recipes["Falafel med hummus"], ImageUrl = "Images\\falaffel.jpg" },
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

                    Console.WriteLine("\n\n\n\nDatabasen seedad!\n\n\n\n");
                }
                else
                {
                    Console.WriteLine("\n\n\n\nInget att Seeda! \n\n\n\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Fel under seeding: " + ex.Message);
            }
        }
    }
}


