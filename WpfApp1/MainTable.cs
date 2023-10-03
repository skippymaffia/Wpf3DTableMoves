using System.Windows.Media.Media3D;

namespace WpfApp1;

public class MainTable : MainTableBase
{
    public MainTable() { InitPoints(); }

    public Point3DCollection Point3Ds { get { return point3Ds; } }

    public void Transform(TranslateTransform3D t)
    {
        for (int i = 0; i < point3Ds.Count; i++)
        {
            point3Ds[i] = t.Transform(point3Ds[i]);
        }
    }
}
