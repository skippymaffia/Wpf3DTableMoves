using System.Windows.Media.Media3D;

namespace WpfApp1;

public abstract class MainTableBase
{
    protected Point3DCollection point3Ds = new();

    public MainTableBase()
    {
    }

    public Point3DCollection Vector3D
    {
        get { return point3Ds; }
        set { point3Ds = value; }
    }

    public void InitPoints()
    {
        point3Ds.Clear();
        point3Ds.Add(new Point3D(0, 0, 0));
        point3Ds.Add(new Point3D(1, 0, 0));
        point3Ds.Add(new Point3D(0, 0.1, 0));
        point3Ds.Add(new Point3D(1, 0.1, 0));
        point3Ds.Add(new Point3D(0, 0, 1));
        point3Ds.Add(new Point3D(1, 0, 1));
        point3Ds.Add(new Point3D(0, 0.1, 1));
        point3Ds.Add(new Point3D(1, 0.1, 1));
    }
}
