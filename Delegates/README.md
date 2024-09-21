# Delegates, Actions, Funcs, Closures

<br/>

Once and for all:
Action, Func, closure - all theese are nothing but a syntactic sugar for delegates.

<br/>

Delegate is a pointer to a specific method with certain signature.

<br />

The main difference is how compiler handles variours declarations.


## Delegates

<br/>

Lets say we defined two delegates using the dedicated ```delegate``` keyword. One is for multiplying input number, one is for dividing.


```
delegate int MulDelegate(int i);
delegate int DivDelegate(int i);
```

Now we define the actual functions theese delegates will be pointing at:

```
int MultiplyByTwo(int i)
{
    return i * 2;
}

int DivideByTwo(int i)
{
    return i / 2;
}
```

Assign functions to delegate variables:

```
MulDelegate mulByTwo = MultiplyByTwo;
DivDelegate divByTwo = DivideByTwo;
```

And call them:

```
mulByTwo(4);
divByTwo(4);
```

So far so good, compiler gives us pretty straightforward code, almost identical: two internal static methods, two delegate assignments and two calls:

```
[CompilerGenerated]
internal class Program
{
  private static void \u003CMain\u003E\u0024(string[] args)
  {
    MulDelegate mulByTwo = new MulDelegate((object) null, __methodptr(\u003C\u003CMain\u003E\u0024\u003Eg__MultiplyByTwo\u007C0_0));
    DivDelegate divByTwo = new DivDelegate((object) null, __methodptr(\u003C\u003CMain\u003E\u0024\u003Eg__DivideByTwo\u007C0_1));
    int num1 = mulByTwo(4);
    int num2 = divByTwo(4);
  }

  public Program()
  {
    base.\u002Ector();
  }

  [CompilerGenerated]
  internal static int \u003C\u003CMain\u003E\u0024\u003Eg__MultiplyByTwo\u007C0_0(int i)
  {
    return i * 2;
  }

  [CompilerGenerated]
  internal static int \u003C\u003CMain\u003E\u0024\u003Eg__DivideByTwo\u007C0_1(int i)
  {
    return i / 2;
  }
}
```

However we explicitly defined both functions in advance (```MultiplyByTwo``` and ```DivideByTwo```). What if we do it with lambda expressions? Lets start all over again this time with only one delegate:

```
delegate int MulDelegate(int i);
```

Assign function to it using lambda expression and call it:

```
MulDelegate mulByTwo = (a) => a * 2;

mulByTwo(4);
```

What the compiler gives us this time:

```diff
[CompilerGenerated]
internal class Program
{
  private static void \u003CMain\u003E\u0024(string[] args)
  {
    int num = (Program.\u003C\u003Ec.\u003C\u003E9__0_0 ?? (Program.\u003C\u003Ec.\u003C\u003E9__0_0 = new MulDelegate((object) Program.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003C\u003CMain\u003E\u0024\u003Eb__0_0))))(4);
  }

  public Program()
  {
    base.\u002Ector();
  }

  [CompilerGenerated]
  [Serializable]
  private sealed class \u003C\u003Ec
  {
    public static readonly Program.\u003C\u003Ec \u003C\u003E9;
+   public static MulDelegate \u003C\u003E9__0_0;

    static \u003C\u003Ec()
    {
      Program.\u003C\u003Ec.\u003C\u003E9 = new Program.\u003C\u003Ec();
    }

    public \u003C\u003Ec()
    {
      base.\u002Ector();
    }

+   internal int \u003C\u003CMain\u003E\u0024\u003Eb__0_0(int a)
+   {
+     return a * 2;
+   }
  }
}
```

So compiler created a sealed class containing both our delegate definition as a public field and an internal multiplication function which delegate will point to. Lets bring back our second delegate for number division:

```
delegate int MulDelegate(int i);
delegate int DivDelegate(int i);
```

Assign functions and call them:

```
MulDelegate mulByTwo = (a) => a * 2;
DivDelegate divByTwo = (a) => a / 2;

mulByTwo(4);
divByTwo(4);
```

Now the compiler should create two classes? No, it won't. New delegate will be added into the same class:

```diff
[CompilerGenerated]
internal class Program
{
  private static void \u003CMain\u003E\u0024(string[] args)
  {
    MulDelegate mulByTwo = Program.\u003C\u003Ec.\u003C\u003E9__0_0 ?? (Program.\u003C\u003Ec.\u003C\u003E9__0_0 = new MulDelegate((object) Program.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003C\u003CMain\u003E\u0024\u003Eb__0_0)));
    DivDelegate divByTwo = Program.\u003C\u003Ec.\u003C\u003E9__0_1 ?? (Program.\u003C\u003Ec.\u003C\u003E9__0_1 = new DivDelegate((object) Program.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003C\u003CMain\u003E\u0024\u003Eb__0_1)));
    int num1 = mulByTwo(4);
    int num2 = divByTwo(4);
  }

  public Program()
  {
    base.\u002Ector();
  }

  [CompilerGenerated]
  [Serializable]
  private sealed class \u003C\u003Ec
  {
    public static readonly Program.\u003C\u003Ec \u003C\u003E9;
+   public static MulDelegate \u003C\u003E9__0_0;
+   public static DivDelegate \u003C\u003E9__0_1;

    static \u003C\u003Ec()
    {
      Program.\u003C\u003Ec.\u003C\u003E9 = new Program.\u003C\u003Ec();
    }

    public \u003C\u003Ec()
    {
      base.\u002Ector();
    }

+   internal int \u003C\u003CMain\u003E\u0024\u003Eb__0_0(int a)
+   {
+     return a * 2;
+   }

+   internal int \u003C\u003CMain\u003E\u0024\u003Eb__0_1(int a)
+   {
+     return a / 2;
+   }
  }
}
```

So it's the same private sealed class but now it has two internal methods which correspond to each delegate definition.

<br/>

Now it gets better. Remember the common (and wrong) definition of value types in .NET? "Value types live on stack". 

More correct way to define it would be "Value types may live on stack if they are non static, local, not captured by anonymous function and not a field of a class.". Lets see what delegates have to say about that. 

In particular lets see how closures will behave. Closures are well known for capturing variables out of scope even when scope has finished its execution. 

Lets define a couple of value type variables and modify our delegate for number multiplication so it points to slightly different function:


```
int a = 2;
int b = 4;


MulDelegate mul = (i) => i * b;

mul(2);

Console.WriteLine(a);
```

Here both ```a``` and ```b``` are integers, value types. However ```b``` is now captured by a closure delegate is pointing at. And Console.WriteLine there just to prevent compiler of throwing away ```a```. Lets see what compiler gives us this time:

```diff
[CompilerGenerated]
internal class Program
{
  private static void \u003CMain\u003E\u0024(string[] args)
  {
    Program.\u003C\u003Ec__DisplayClass0_0 cDisplayClass00 = new Program.\u003C\u003Ec__DisplayClass0_0();
    int a = 2;
+   cDisplayClass00.b = 4;
    int num = new MulDelegate((object) cDisplayClass00, __methodptr(\u003C\u003CMain\u003E\u0024\u003Eb__0))(2);
    Console.WriteLine(a);
  }

  public Program()
  {
    base.\u002Ector();
  }

  [CompilerGenerated]
  private sealed class \u003C\u003Ec__DisplayClass0_0
  {
+   public int b;

    public \u003C\u003Ec__DisplayClass0_0()
    {
      base.\u002Ector();
    }

+   internal int \u003C\u003CMain\u003E\u0024\u003Eb__0(int i)
+   {
+     return i * this.b;
+   }
  }
}

```

So ```a``` still a local variable (obviously, since we did nothing to it), however ```b``` is a public field now of a private sealed class. However, it's not a delegate wrapper. It's a closure wrapper this time, notice how it has no field for ```MulDelegate```, however it has a field for ```b``` value type and a multiplication method delegate supposed to be pointing at. The delegate itself is created in the main method using ```new``` operator. So lambdas give us a delegate wrapper class with public field for delegate while closures give us a closure wrapper class with public fields for captured variables. 
The difference between lambda and closure is that lambda is a function without a name while closure is a function which has an access to variables not necessarily listed in its parameters.

<br/>






