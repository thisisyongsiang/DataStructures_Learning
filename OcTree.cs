using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures_Learning
{
    public class D_Point3d
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public object customUserObject { get; set; }
        public D_Point3d(double x, double y, double z, object userObject = null)
        {
            X = x;
            Y = y;
            Z = z;
            customUserObject = userObject;
        }
    }
    public class D_Box
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
       // public D_Point3d Origin { get; set; }
        public D_Point3d Center { get; set; }
        public D_Box(D_Point3d Center, double x, double y, double z)
        {
            this.Center = Center;
            X = x;
            Y = y;
            Z = z;
        }
        public bool Contains(D_Point3d point)
        {

            if(Math.Abs(point.X-Center.X)<=X/2&& 
                Math.Abs(point.Y - Center.Y) <= Y / 2 &&
                Math.Abs(point.Z - Center.Z) <= Y / 2)
            {
                return true;
            }
            return false;
        }

    }
    public class OcTree
    {
        private int _capacity;
        private List<OcTree> _childNodes = new List<OcTree>();
        private double _minSize=1;
        public double MinSize
        {
            get { return _minSize; }
            set { _minSize = value; }
        }

        /// <summary>
        /// Boundary box of current node
        /// </summary>
        public D_Box BoundaryBox { get; set; }

        /// <summary>
        /// Points Contained in Leaf Node. If null, current node is an Internal node
        /// </summary>
        public List<D_Point3d> PointList { get; set; }

        /// <summary>
        /// Parent Node Of Current Quadrant
        /// </summary>
        public OcTree ParentNode { get; set; }
        public OcTree(D_Box boundaryBox, int Capacity,OcTree parent=null)
        {
            BoundaryBox = boundaryBox;
            _capacity = Capacity;
            ParentNode = parent;
            PointList = new List<D_Point3d>();
        }
        public void Insert(D_Point3d point)
        {
            if (!BoundaryBox.Contains(point)) return;
            if (PointList != null)
            {
                {
                    if (PointList.Count < _capacity)
                    {
                        PointList.Add(point);
                    }
                    else
                    {
                        this.Subdivide();
                    }
                }
            }

            if (PointList == null)
            {
                if (_childNodes.Count != 8) { throw new Exception(String.Format("There are {0} child nodes. Expected 8 child nodes", _childNodes.Count)); }
                foreach(OcTree ocTree in _childNodes)
                {
                    if (ocTree.BoundaryBox.Contains(point)) ocTree.Insert(point);
                    break;
                }
            }
        }
        private void Subdivide()
        {
            D_Point3d centPt = BoundaryBox.Center;
            D_Point3d Oct1 = new D_Point3d(centPt.X + BoundaryBox.X / 4, centPt.Y + BoundaryBox.Y / 4, centPt.Z + BoundaryBox.Z / 4);
            D_Point3d Oct2 = new D_Point3d(centPt.X + BoundaryBox.X / 4, centPt.Y + BoundaryBox.Y / 4, centPt.Z - BoundaryBox.Z / 4);
            D_Point3d Oct3 = new D_Point3d(centPt.X + BoundaryBox.X / 4, centPt.Y - BoundaryBox.Y / 4, centPt.Z + BoundaryBox.Z / 4);
            D_Point3d Oct4 = new D_Point3d(centPt.X + BoundaryBox.X / 4, centPt.Y - BoundaryBox.Y / 4, centPt.Z - BoundaryBox.Z / 4);
            D_Point3d Oct5 = new D_Point3d(centPt.X - BoundaryBox.X / 4, centPt.Y + BoundaryBox.Y / 4, centPt.Z + BoundaryBox.Z / 4);
            D_Point3d Oct6 = new D_Point3d(centPt.X - BoundaryBox.X / 4, centPt.Y + BoundaryBox.Y / 4, centPt.Z - BoundaryBox.Z / 4);
            D_Point3d Oct7 = new D_Point3d(centPt.X - BoundaryBox.X / 4, centPt.Y - BoundaryBox.Y / 4, centPt.Z + BoundaryBox.Z / 4);
            D_Point3d Oct8 = new D_Point3d(centPt.X - BoundaryBox.X / 4, centPt.Y - BoundaryBox.Y / 4, centPt.Z - BoundaryBox.Z / 4);

            D_Box Octant1 = new D_Box(Oct1, BoundaryBox.X / 2, BoundaryBox.Y / 2, BoundaryBox.Z / 2);
            D_Box Octant2 = new D_Box(Oct2, BoundaryBox.X / 2, BoundaryBox.Y / 2, BoundaryBox.Z / 2);
            D_Box Octant3 = new D_Box(Oct3, BoundaryBox.X / 2, BoundaryBox.Y / 2, BoundaryBox.Z / 2);
            D_Box Octant4 = new D_Box(Oct4, BoundaryBox.X / 2, BoundaryBox.Y / 2, BoundaryBox.Z / 2);
            D_Box Octant5 = new D_Box(Oct5, BoundaryBox.X / 2, BoundaryBox.Y / 2, BoundaryBox.Z / 2);
            D_Box Octant6 = new D_Box(Oct6, BoundaryBox.X / 2, BoundaryBox.Y / 2, BoundaryBox.Z / 2);
            D_Box Octant7 = new D_Box(Oct7, BoundaryBox.X / 2, BoundaryBox.Y / 2, BoundaryBox.Z / 2);
            D_Box Octant8 = new D_Box(Oct8, BoundaryBox.X / 2, BoundaryBox.Y / 2, BoundaryBox.Z / 2);

            _childNodes.Add(new OcTree(Octant1, _capacity, ParentNode));
            _childNodes.Add(new OcTree(Octant2, _capacity, ParentNode));
            _childNodes.Add(new OcTree(Octant3, _capacity, ParentNode));
            _childNodes.Add(new OcTree(Octant4, _capacity, ParentNode));
            _childNodes.Add(new OcTree(Octant5, _capacity, ParentNode));
            _childNodes.Add(new OcTree(Octant6, _capacity, ParentNode));
            _childNodes.Add(new OcTree(Octant7, _capacity, ParentNode));
            _childNodes.Add(new OcTree(Octant8, _capacity, ParentNode));

            foreach (D_Point3d pt in PointList)
            {
                if (Octant1.Contains(pt)) { _childNodes[0].Insert(pt); continue; }
                if (Octant2.Contains(pt)) { _childNodes[1].Insert(pt); continue; }
                if (Octant3.Contains(pt)) { _childNodes[2].Insert(pt); continue; }
                if (Octant4.Contains(pt)) { _childNodes[3].Insert(pt); continue; }
                if (Octant5.Contains(pt)) { _childNodes[4].Insert(pt); continue; }
                if (Octant6.Contains(pt)) { _childNodes[5].Insert(pt); continue; }
                if (Octant7.Contains(pt)) { _childNodes[6].Insert(pt); continue; }
                if (Octant8.Contains(pt)) { _childNodes[7].Insert(pt); continue; }
            }
            PointList = null;

        }
        public void Remove()
        {

        }
        public void Query()
        {

        }
    }
}
