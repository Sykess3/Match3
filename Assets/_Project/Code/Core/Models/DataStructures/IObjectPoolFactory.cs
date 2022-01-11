namespace _Project.Code.Core.Models.DataStructures
{
    public interface IObjectPoolFactory<in TArg, out TReturn> 
    {
        TReturn Create(TArg type);
    }
}