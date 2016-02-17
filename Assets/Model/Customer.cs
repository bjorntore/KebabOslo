using System.Collections;

public class Customer
{

    public int x;
    public int z;

    public Customer(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public override string ToString()
    {
        return "Customer_" + x + "_" + z;
    }

}
