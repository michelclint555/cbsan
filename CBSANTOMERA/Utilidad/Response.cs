namespace CBSANTOMERA.Utilidad
{
    public class Response<T> //la hacemos generica
    {
        public bool status { get; set; }
        public T value { get; set; }
        public string msg { get; set; }
    }

    public class ResponseModel<TModel> //la hacemos generica
    {
        public bool status { get; set; }
        public TModel value { get; set; }
        public string msg { get; set; }
    }
}
