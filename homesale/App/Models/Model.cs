
namespace homesale.App.Models
{
    abstract class Model<T> : Libs.ORM.Model<T> where T : Libs.ORM.Model<T>, new()
    {
    }

    class Personal : Libs.ORM.ModelAttributes.Personal { }
    class Inside : Libs.ORM.ModelAttributes.Inside { }
}
