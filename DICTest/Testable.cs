namespace DICTest
{
    public interface IWrong { }
    public abstract class WrongClass1 : IWrong { }
    public class NotWrong : WrongClass1 { }

    public interface IFoo { }
    public class Foo : IFoo { }
    public class Foo2 : IFoo { }

    internal class NotWrongInFoo : IFoo
    {
        public NotWrongInFoo(NotWrong notWrong)
        {
            this.notWrong = notWrong;
        }

        NotWrong notWrong;
    }

    public class Someclass { }

    internal class GenClass<T>
    {
        T value;
    }
}
