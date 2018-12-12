namespace DICTest
{
    public interface IWrong { }
    public abstract class WrongClass1 : IWrong { }
    public class NotWrong : WrongClass1 { }

    public interface IFoo { }
    public class Foo : IFoo { }
    public class Foo2 : IFoo { }

    public class NotWrongInFoo : IFoo
    {
        public NotWrongInFoo(WrongClass1 notWrong)
        {
            this.notWrong = (NotWrong)notWrong;
        }

        public NotWrong notWrong { get; set; }
    }

    public class GenClass<T>
    {
        T value;
    }

    public interface IService<T> { }
    public class ServiceImpl<T> : IService<T> { }
}
