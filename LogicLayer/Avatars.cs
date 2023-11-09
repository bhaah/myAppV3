namespace myFirstAppSol.LogicLayer
{
    public class Avatars
    {
        public static Dictionary<string, int> AvatarSales = new Dictionary<string, int>
        {
            {"Bird",89 },
            {"Cat",69 },
            {"BabyCat",129 },
            {"Cheetah",139 },
            {"Elephant",89 },
            {"BabyLeon",149 },
            {"Leon",149 },
            {"Fish",99 },
            {"Baby",69 }
            
        };

        public static Dictionary<string,int> getAvatars(List<string> purchased)
        {
            Dictionary<string, int> toRet = AvatarSales;
            foreach(string avatar in purchased)
            {
                toRet.Remove(avatar);
            }
            return toRet;
        } 

    }
}
