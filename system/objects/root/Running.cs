namespace Gudron
{
    public class Gudron
    {
        public static void run<ObjectType>() 
            where ObjectType : system.objects.main.Object, new()
                => ((system.objects.root.description.ILife)
                    new system.objects.root.Object<ObjectType>()).Run();
    }
}