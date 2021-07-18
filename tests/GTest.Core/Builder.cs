namespace GTest.Core
{
    public class Builder<T>
    {
        protected T Object;
        
        public T Build()
        {
            return Object;
        }
    }
}