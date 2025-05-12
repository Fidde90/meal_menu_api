using meal_menu_api.Database.Context;
using meal_menu_api.Entities;
using static System.Net.Mime.MediaTypeNames;

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
                    var newUnits = new List<string> { "krm", "tsk", "msk", "cl", "ml", "dl", "l", "g", "hg", "kg", "nypa", "paket", "st"," " };
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
                        new() { UnitId = units["g"], RecipeId = recipes["Lammgryta med kikärtor"], Name = "lammgrytbitar", Amount = 300 },
                        new() { UnitId = units[" "], RecipeId = recipes["Lammgryta med kikärtor"], Name = "salt och peppar", Amount = 0 },
                        new() { UnitId = units["st"], RecipeId = recipes["Lammgryta med kikärtor"], Name = "vitlöksklyfta", Amount = 1 },
                        new() { UnitId = units["tsk"], RecipeId = recipes["Lammgryta med kikärtor"], Name = "tsk malen spiskummin", Amount = 1 },
                        new() { UnitId = units["dl"], RecipeId = recipes["Lammgryta med kikärtor"], Name = "vatten", Amount = 2 },

                        new() { UnitId = units["g"], RecipeId = recipes["Potatissoppa med fänkål och purjolök"], Name = "skalad potatis, mjölig sort", Amount = 200 },
                        new() { UnitId = units["st"], RecipeId = recipes["Potatissoppa med fänkål och purjolök"], Name = "fänkål", Amount = 1 },
                        new() { UnitId = units["g"],RecipeId = recipes["Potatissoppa med fänkål och purjolök"], Name = "smör", Amount = 100 },
                        new() { UnitId = units["dl"], RecipeId = recipes["Potatissoppa med fänkål och purjolök"], Name = "vatten", Amount = 4 },
                        new() { UnitId = units["st"], RecipeId = recipes["Potatissoppa med fänkål och purjolök"], Name = "tärning grönsaksbuljong", Amount = 1 },

                        new() { UnitId = units["g"], RecipeId = recipes["Räksallad med sparris och sojamajonnäs"], Name = "(hand)skalade räkor", Amount = 150 },
                        new() { UnitId = units["g"], RecipeId = recipes["Räksallad med sparris och sojamajonnäs"], Name = "grön sparris", Amount = 250 },
                        new() { UnitId = units["st"], RecipeId = recipes["Räksallad med sparris och sojamajonnäs"], Name = "hjärtsallad", Amount = 2 },
                        new() { UnitId = units["st"], RecipeId = recipes["Räksallad med sparris och sojamajonnäs"], Name = "mogen mango", Amount = 1 },
                        new() { UnitId = units["cl"], RecipeId = recipes["Räksallad med sparris och sojamajonnäs"], Name = "citronjuice", Amount = 1 },

                        new() { UnitId = units["g"], RecipeId = recipes["Köttbullar"], Name = "nötfärs", Amount = 1 },
                        new() { UnitId = units["g"], RecipeId = recipes["Köttbullar"], Name = "ströbröd", Amount = 50 },
                        new() { UnitId = units["st"], RecipeId = recipes["Köttbullar"], Name = "ägg", Amount = 1 },
                        new() { UnitId = units["dl"], RecipeId = recipes["Köttbullar"], Name = "mjölk", Amount = 3 },
                        new() { UnitId = units["msk"], RecipeId = recipes["Köttbullar"], Name = "fond", Amount = 2 },

                        new() { UnitId = units["g"], RecipeId = recipes["Kycklinggryta"], Name = "Kycklingfilé", Amount = 500 },
                        new() { UnitId = units["tsk"], RecipeId = recipes["Kycklinggryta"], Name = "Curry", Amount = 2 },
                        new() { UnitId = units["st"], RecipeId = recipes["Kycklinggryta"], Name = "ägg", Amount = 1 },
                        new() { UnitId = units["dl"], RecipeId = recipes["Kycklinggryta"], Name = "mjölk", Amount = 2 },
                        new() { UnitId = units["msk"], RecipeId = recipes["Kycklinggryta"], Name = "fond", Amount = 2 },

                        new() { UnitId = units["g"], RecipeId = recipes["Spaghetti Bolognese"], Name = "Köttfärs", Amount = 500 },
                        new() { UnitId = units["dl"], RecipeId = recipes["Spaghetti Bolognese"], Name = "Tomatsås", Amount = 2 },
                        new() { UnitId = units["st"], RecipeId = recipes["Spaghetti Bolognese"], Name = "Lök", Amount = 1 },
                        new() { UnitId = units["tsk"], RecipeId = recipes["Spaghetti Bolognese"], Name = "Kryddor (oregano, basilika)", Amount = 2 },
                        new() { UnitId = units["g"], RecipeId = recipes["Spaghetti Bolognese"], Name = "Pasta", Amount = 400 },

                        new() { UnitId = units["g"], RecipeId = recipes["Vegetarisk lasagne"], Name = "Spenat", Amount = 300 },
                        new() { UnitId = units["g"], RecipeId = recipes["Vegetarisk lasagne"], Name = "Ricotta", Amount = 200 },
                        new() { UnitId = units["dl"], RecipeId = recipes["Vegetarisk lasagne"], Name = "Tomatsås", Amount = 2 },
                        new() { UnitId = units["st"], RecipeId = recipes["Vegetarisk lasagne"], Name = "Lasagneplattor", Amount = 12 },
                        new() { UnitId = units["tsk"], RecipeId = recipes["Vegetarisk lasagne"], Name = "Kryddor (oregano, basilika)", Amount = 2 },

                        new() { UnitId = units["g"], RecipeId = recipes["Tacos med guacamole"], Name = "Köttfärs", Amount = 400 },
                        new() { UnitId = units["st"], RecipeId = recipes["Tacos med guacamole"], Name = "Taco-skal", Amount = 8 },
                        new() { UnitId = units["dl"], RecipeId = recipes["Tacos med guacamole"], Name = "Salsa", Amount = 1 },
                        new() { UnitId = units["st"], RecipeId = recipes["Tacos med guacamole"], Name = "Avokado", Amount = 2 },
                        new() { UnitId = units["g"], RecipeId = recipes["Tacos med guacamole"], Name = "Riven ost", Amount = 100 },

                        new() { UnitId = units["g"], RecipeId = recipes["Fiskgryta med saffran"], Name = "Fisk (t.ex. torsk)", Amount = 400 },
                        new() { UnitId = units["dl"], RecipeId = recipes["Fiskgryta med saffran"], Name = "Fiskbuljong", Amount = 1 },
                        new() { UnitId = units["dl"], RecipeId = recipes["Fiskgryta med saffran"], Name = "Grädde", Amount = 2 },
                        new() { UnitId = units["g"], RecipeId = recipes["Fiskgryta med saffran"], Name = "Fänkål", Amount = 150 },
                        new() { UnitId = units["msk"], RecipeId = recipes["Fiskgryta med saffran"], Name = "Saffran", Amount = 1 },

                        new() { UnitId = units["g"], RecipeId = recipes["Falafel med hummus"], Name = "Kikärtor", Amount = 400 },
                        new() { UnitId = units["g"], RecipeId = recipes["Falafel med hummus"], Name = "Lök", Amount = 1 },
                        new() { UnitId = units["tsk"], RecipeId = recipes["Falafel med hummus"], Name = "Spiskummin", Amount = 1 },
                        new() { UnitId = units["msk"], RecipeId = recipes["Falafel med hummus"], Name = "Olivolja", Amount = 2 },
                        new() { UnitId = units["dl"], RecipeId = recipes["Falafel med hummus"], Name = "Hummus", Amount = 1 }

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


