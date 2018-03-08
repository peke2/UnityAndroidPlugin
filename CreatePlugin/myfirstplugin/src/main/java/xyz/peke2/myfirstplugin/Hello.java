package xyz.peke2.myfirstplugin;

/**
 * Created by peke2 on 2018/03/04.
 */

public class Hello {
    public static String say()
    {
        return "Hello!";
    }

    public static int add(int a, int b)
    {
        return a + b;
    }

    public static int sum(HelloParam param)
    {
        int total = 0;
        /*
        int len = param.values.length;
        for(int i=0; i<len; i++)
        {
            total += param.values[i];
        }*/
        total = 12345;
        return total;
    }

    public static String getName(HelloParam param)
    {
        return param.name;
    }
}
