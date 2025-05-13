namespace meal_menu_api.Helpers
{
    public class ToolBox
    {
        public List<T> ShuffleArray<T>(List<T> array)
        {
            Random rng = new Random();
            
            for (int i = array.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                T temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }

            return array;
        }
    }
}
